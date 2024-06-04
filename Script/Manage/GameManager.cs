using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // �̱��� �ν��Ͻ�

    private SoundManager soundManager; // ���� �Ŵ����� ���� ����

    public static event Action<int> OnMonsterExp;

    public event Action OnGoldChanged;
    public event Action OnJewelChanged;

    [SerializeField] public int gold = 100000; // �÷��̾��� ���� ���
    [SerializeField] public int jewel = 10000; // �÷��̾��� ���� ����


    [SerializeField] public int msGold = 100;
    [SerializeField] public int msJewel = 10;
    [SerializeField] public int msExp = 50;
    [SerializeField] public int jewelDropChance = 50;    //���� ȹ�� Ȯ��
    public bool jewelDrop;


    [SerializeField] public int playerDamage = 80; // �÷��̾��� �⺻����
    [SerializeField] public int fireDamage = 100; // �÷��̾��� ��ų����
    [SerializeField] public int iceDamage = 100; // �÷��̾��� ��ų����

    public float playerMoveSpeed = 5.0f;
    public float playerAttackTime = 1.0f;
    public int playerRecoveryHealth = 10;

    public enum UpgradeType
    {
        Gold,
        Jewel,
        Attack,
        Fire,
        Ice,
        PlMove,
        Exp,
        AtkSpeed,
        PlHp
    }

    [SerializeField] private int baseGoldUpgradeCost = 100; // ��� ���׷��̵� ���
    [SerializeField] private int baseJewelUpgradeCost = 100; // ���� ���׷��̵� ���
    [SerializeField] private int baseAttackUpgradeCost = 100; // ���� ���׷��̵� ���
    [SerializeField] private int baseFireUpgradeCost = 100; // ���̾� ���׷��̵� ���
    [SerializeField] private int baseIceUpgradeCost = 100; // ���̽� ���׷��̵� ���
    [SerializeField] private int basePlMoveUpgradeCost = 100;
    [SerializeField] private int baseExpUpgradeCost = 100;
    [SerializeField] private int baseAtkSpeedUpgradeCost = 100;
    [SerializeField] private int basePlHpUpgradeCost = 100;

    private int currentGoldUpgradeCost;
    private int currentJewelUpgradeCost;
    private int currentAttackUpgradeCost;
    private int currentFireUpgradeCost;
    private int currentIceUpgradeCost;
    private int currentPlMoveUpgradeCost;
    private int currentExpUpgradeCost;
    private int currentAtkSpeedUpgradeCost;
    private int currentPlHpUpgradeCost;

    public int damageIncrease = 10; // ���ݷ� ������
    public int goldIncrease = 50;
    public int expIncrease = 25;
    public int recoveryHpIncrease = 10;
    private float multipIncrease = 1.2f;

    [SerializeField] private int currentStage;
    [SerializeField] private int monsterCount;
    [SerializeField] private int monsterIncreaseHealth;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
        MonsterSpawn.OnMonsterSpawn += HandleMonsterSpawn;
        SceneManager.sceneLoaded += OnSceneLoaded; // ���� �ε�� ������ ȣ��� �̺�Ʈ ����

        currentGoldUpgradeCost = baseGoldUpgradeCost;
        currentJewelUpgradeCost = baseJewelUpgradeCost;
        currentAttackUpgradeCost = baseAttackUpgradeCost;
        currentFireUpgradeCost = baseFireUpgradeCost;
        currentIceUpgradeCost = baseIceUpgradeCost;
        currentPlMoveUpgradeCost = basePlMoveUpgradeCost;
        currentExpUpgradeCost = baseExpUpgradeCost;
        currentAtkSpeedUpgradeCost = baseAtkSpeedUpgradeCost;
        currentPlHpUpgradeCost = basePlHpUpgradeCost;
    }

    void HandleMonsterSpawn(MonsterInfo monsterInfo)
    {
        MonsterHealth monsterHealth = monsterInfo.monsterObject.GetComponent<MonsterHealth>();
        if (monsterHealth != null)
        {
            monsterHealth.OnMonsterDrop += MonsterDrop;
        }
    }



    void MonsterDrop(MonsterHealth monsterHealth)
    {
        AddGold(msGold);
        AddJewel(msJewel);

        OnMonsterExp?.Invoke(msExp);
    }

    // �÷��̾�� ��带 �߰��ϴ� �Լ�
    public void AddGold(int amount)
    {
        int stage = currentStage;
        if (stage == 0 || stage == 1)
            gold += amount;
        else
            gold += amount * (int)Math.Pow(10, stage - 1);
        OnGoldChanged?.Invoke();
    }

    // �÷��̾�� Ȯ���� ������ �߰��ϴ� �Լ�
    public void AddJewel(int amount)
    {
        if (UnityEngine.Random.Range(0, 100) < jewelDropChance)
        {
            int stage = currentStage;
            if (stage == 0 || stage == 1)
            {
                jewel += amount;
                jewelDrop = true;
            }
            else
            {
                jewel += amount * (int)Math.Pow(10, stage - 1);
                jewelDrop = true;
            }
            OnJewelChanged?.Invoke();
        }
        else
        {
            jewelDrop = false;
        }
    }


    // ���׷��̵� ---------------------------------------------------------------------------------------------

    public string Upgrade(UpgradeType upgradeType)
    {
        int upgradeCost = CalculateUpgradeCost(upgradeType);

        // ���� ���׷��̵��ϴ� ���
        if ((upgradeType == UpgradeType.Gold || upgradeType == UpgradeType.Jewel || upgradeType == UpgradeType.Attack || upgradeType == UpgradeType.Fire || upgradeType == UpgradeType.Ice) && gold >= upgradeCost)
        {
            gold -= upgradeCost;
            OnGoldChanged?.Invoke();

            switch (upgradeType)
            {
                case UpgradeType.Gold:
                    currentGoldUpgradeCost = upgradeCost;
                    msGold += goldIncrease;
                    //Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
                    //Debug.Log("ȹ�� ��差 ���׷��̵� �Ϸ�. ���� ȹ�淮: " + msGold);
                    return "���׷��̵� ����";
                case UpgradeType.Jewel:
                    currentJewelUpgradeCost = upgradeCost;
                    msJewel += goldIncrease;
                    //Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
                    //Debug.Log("ȹ�� ������ ���׷��̵� �Ϸ�. ���� ȹ�淮: " + msJewel);
                    return "���׷��̵� ����";
                case UpgradeType.Attack:
                    currentAttackUpgradeCost = upgradeCost;
                    playerDamage += damageIncrease;
                    //Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
                    //Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + playerDamage);
                    return "���׷��̵� ����";
                case UpgradeType.Fire:
                    currentFireUpgradeCost = upgradeCost;
                    fireDamage += damageIncrease;
                    //Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
                    //Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + fireDamage);
                    return "���׷��̵� ����";
                case UpgradeType.Ice:
                    currentIceUpgradeCost = upgradeCost;
                    iceDamage += damageIncrease;
                    //Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
                    //Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + iceDamage);
                    return "���׷��̵� ����";
             }
        }
        // �������� ���׷��̵��ϴ� ���
        else if (( upgradeType == UpgradeType.PlMove || upgradeType == UpgradeType.Exp || upgradeType == UpgradeType.AtkSpeed || upgradeType == UpgradeType.PlHp) && jewel >= upgradeCost)
        {
            if (upgradeType == UpgradeType.AtkSpeed && playerAttackTime <= 0.4f)
            {
                //Debug.Log("���� �ӵ� ���׷��̵尡 �ִ�ġ�� �����߽��ϴ�.");
                return "���� �ӵ� ���׷��̵尡 �ִ�ġ�� �����߽��ϴ�.";
            }

            jewel -= upgradeCost;
            OnJewelChanged?.Invoke();

            switch (upgradeType)
            {
                case UpgradeType.PlMove:
                    currentPlMoveUpgradeCost = upgradeCost;
                    playerMoveSpeed += 0.3f;
                    //Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
                    //Debug.Log("�̵��ӵ� ���׷��̵� �Ϸ�. ���� �̵��ӵ�: " + playerMoveSpeed);
                    return "���׷��̵� ����";
                case UpgradeType.Exp:
                    currentExpUpgradeCost = upgradeCost;
                    msExp += expIncrease;
                    //Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
                    //Debug.Log("����ġ ȹ�淮 ���׷��̵� �Ϸ�. ���� ����ġ ȹ�淮: " + msExp);
                    return "���׷��̵� ����";
                case UpgradeType.AtkSpeed:
                    currentAtkSpeedUpgradeCost = upgradeCost;
                    playerAttackTime -= 0.05f;
                    //Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
                    //Debug.Log("���� �ӵ� ���׷��̵� �Ϸ�. ���� ���� �ӵ�: " + playerAttackTime);
                    return "���׷��̵� ����";
                case UpgradeType.PlHp:
                    currentPlHpUpgradeCost = upgradeCost;
                    playerRecoveryHealth += 10;
                    //Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
                    //Debug.Log("ü�� ȸ�� ���׷��̵� �Ϸ�. ���� ü�� ȸ����: " + playerRecoveryHealth);
                    return "���׷��̵� ����";
            }
        }
        else
        {
            //Debug.Log("��� �Ǵ� ������ �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return "�ڿ��� �����մϴ�.";
        }
        return "�� �� ���� ������ �߻��߽��ϴ�.";
    }


    // ���׷��̵� ��� ��� ----------------------------------------------------

    public int CalculateUpgradeCost(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Gold:
                return (int)(currentGoldUpgradeCost * multipIncrease);
            case UpgradeType.Jewel:
                return (int)(currentJewelUpgradeCost * multipIncrease);
            case UpgradeType.Attack:
                return (int)(currentAttackUpgradeCost * multipIncrease);
            case UpgradeType.Fire:
                return (int)(currentFireUpgradeCost * multipIncrease);
            case UpgradeType.Ice:
                return (int)(currentIceUpgradeCost * multipIncrease);
            case UpgradeType.PlMove:
                return (int)(currentPlMoveUpgradeCost * multipIncrease);
            case UpgradeType.Exp:
                return (int)(currentExpUpgradeCost * multipIncrease);
            case UpgradeType.AtkSpeed:
                return (int)(currentAtkSpeedUpgradeCost * multipIncrease);
            case UpgradeType.PlHp:
                return (int)(currentPlHpUpgradeCost * multipIncrease);
            default:
                return 0;
        }
    }

    public int GetCurrentUpgradeCost(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Gold:
                return currentGoldUpgradeCost;
            case UpgradeType.Jewel:
                return currentJewelUpgradeCost;
            case UpgradeType.Attack:
                return currentAttackUpgradeCost;
            case UpgradeType.Fire:
                return currentFireUpgradeCost;
            case UpgradeType.Ice:
                return currentIceUpgradeCost;
            case UpgradeType.PlMove:
                return currentPlMoveUpgradeCost;
            case UpgradeType.Exp:
                return currentExpUpgradeCost;
            case UpgradeType.AtkSpeed:
                return currentAtkSpeedUpgradeCost;
            case UpgradeType.PlHp:
                return currentPlHpUpgradeCost;
            default:
                return 0;
        }
    }

    // �������� ����, ��Ʈ�� --------------------------------------------------------

    public void MonsterKilled()
    {
        CountIncrease(); // ���͸� ���� Ƚ�� ����

        if (monsterCount % 10 == 0) // 20������ ���� ������
        {
            monsterIncreaseHealth += 5; // ���� ü�� 5 ����
        }
    }

    public int GetIncreaseMonsterHealth()
    {
        return monsterIncreaseHealth;
    }

    private void CountIncrease()
    {
        monsterCount++;
    }

    public void MonsterCountReset()
    {
        monsterCount = 0;
        monsterIncreaseHealth = 0;
    }

    

    

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public int SetStage(int value)
    {
        currentStage = value;       // ���� 0 �������� 1���� ����
        return currentStage;
    }

    void PlayBackgroundMusic()
    {

        // ���� �������� ��ȣ�� ������� BGM ����
        switch (currentStage)
        {
            case 1:
                SoundManager.instance.PlayBgm(true, SoundManager.Bgm.Stage1);
                break;
            case 2:
                SoundManager.instance.PlayBgm(true, SoundManager.Bgm.Stage2);
                break;
            case 3:
                SoundManager.instance.PlayBgm(true, SoundManager.Bgm.Stage3);
                break;
            default:
                SoundManager.instance.PlayBgm(true, SoundManager.Bgm.Main);
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� �ε�� ������ ���� ����� �����ϰ� ���ο� ����� ���
        SoundManager.instance.StopAllAudio();
        PlayBackgroundMusic();
    }

    private void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� ����� �̺�Ʈ�� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}