using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스

    private SoundManager soundManager; // 사운드 매니저에 대한 참조

    public static event Action<int> OnMonsterExp;

    public event Action OnGoldChanged;
    public event Action OnJewelChanged;

    [SerializeField] public int gold = 100000; // 플레이어의 현재 골드
    [SerializeField] public int jewel = 10000; // 플레이어의 현재 보석


    [SerializeField] public int msGold = 100;
    [SerializeField] public int msJewel = 10;
    [SerializeField] public int msExp = 50;
    [SerializeField] public int jewelDropChance = 50;    //보석 획득 확률
    public bool jewelDrop;


    [SerializeField] public int playerDamage = 80; // 플레이어의 기본공격
    [SerializeField] public int fireDamage = 100; // 플레이어의 스킬공격
    [SerializeField] public int iceDamage = 100; // 플레이어의 스킬공격

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

    [SerializeField] private int baseGoldUpgradeCost = 100; // 골드 업그레이드 비용
    [SerializeField] private int baseJewelUpgradeCost = 100; // 보석 업그레이드 비용
    [SerializeField] private int baseAttackUpgradeCost = 100; // 공격 업그레이드 비용
    [SerializeField] private int baseFireUpgradeCost = 100; // 파이어 업그레이드 비용
    [SerializeField] private int baseIceUpgradeCost = 100; // 아이스 업그레이드 비용
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

    public int damageIncrease = 10; // 공격력 증가량
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
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때마다 호출될 이벤트 연결

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

    // 플레이어에게 골드를 추가하는 함수
    public void AddGold(int amount)
    {
        int stage = currentStage;
        if (stage == 0 || stage == 1)
            gold += amount;
        else
            gold += amount * (int)Math.Pow(10, stage - 1);
        OnGoldChanged?.Invoke();
    }

    // 플레이어에게 확률로 보석을 추가하는 함수
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


    // 업그레이드 ---------------------------------------------------------------------------------------------

    public string Upgrade(UpgradeType upgradeType)
    {
        int upgradeCost = CalculateUpgradeCost(upgradeType);

        // 골드로 업그레이드하는 경우
        if ((upgradeType == UpgradeType.Gold || upgradeType == UpgradeType.Jewel || upgradeType == UpgradeType.Attack || upgradeType == UpgradeType.Fire || upgradeType == UpgradeType.Ice) && gold >= upgradeCost)
        {
            gold -= upgradeCost;
            OnGoldChanged?.Invoke();

            switch (upgradeType)
            {
                case UpgradeType.Gold:
                    currentGoldUpgradeCost = upgradeCost;
                    msGold += goldIncrease;
                    //Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
                    //Debug.Log("획득 골드량 업그레이드 완료. 현재 획득량: " + msGold);
                    return "업그레이드 성공";
                case UpgradeType.Jewel:
                    currentJewelUpgradeCost = upgradeCost;
                    msJewel += goldIncrease;
                    //Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
                    //Debug.Log("획득 보석량 업그레이드 완료. 현재 획득량: " + msJewel);
                    return "업그레이드 성공";
                case UpgradeType.Attack:
                    currentAttackUpgradeCost = upgradeCost;
                    playerDamage += damageIncrease;
                    //Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
                    //Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + playerDamage);
                    return "업그레이드 성공";
                case UpgradeType.Fire:
                    currentFireUpgradeCost = upgradeCost;
                    fireDamage += damageIncrease;
                    //Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
                    //Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + fireDamage);
                    return "업그레이드 성공";
                case UpgradeType.Ice:
                    currentIceUpgradeCost = upgradeCost;
                    iceDamage += damageIncrease;
                    //Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
                    //Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + iceDamage);
                    return "업그레이드 성공";
             }
        }
        // 보석으로 업그레이드하는 경우
        else if (( upgradeType == UpgradeType.PlMove || upgradeType == UpgradeType.Exp || upgradeType == UpgradeType.AtkSpeed || upgradeType == UpgradeType.PlHp) && jewel >= upgradeCost)
        {
            if (upgradeType == UpgradeType.AtkSpeed && playerAttackTime <= 0.4f)
            {
                //Debug.Log("공격 속도 업그레이드가 최대치에 도달했습니다.");
                return "공격 속도 업그레이드가 최대치에 도달했습니다.";
            }

            jewel -= upgradeCost;
            OnJewelChanged?.Invoke();

            switch (upgradeType)
            {
                case UpgradeType.PlMove:
                    currentPlMoveUpgradeCost = upgradeCost;
                    playerMoveSpeed += 0.3f;
                    //Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
                    //Debug.Log("이동속도 업그레이드 완료. 현재 이동속도: " + playerMoveSpeed);
                    return "업그레이드 성공";
                case UpgradeType.Exp:
                    currentExpUpgradeCost = upgradeCost;
                    msExp += expIncrease;
                    //Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
                    //Debug.Log("경험치 획득량 업그레이드 완료. 현재 경험치 획득량: " + msExp);
                    return "업그레이드 성공";
                case UpgradeType.AtkSpeed:
                    currentAtkSpeedUpgradeCost = upgradeCost;
                    playerAttackTime -= 0.05f;
                    //Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
                    //Debug.Log("공격 속도 업그레이드 완료. 현재 공격 속도: " + playerAttackTime);
                    return "업그레이드 성공";
                case UpgradeType.PlHp:
                    currentPlHpUpgradeCost = upgradeCost;
                    playerRecoveryHealth += 10;
                    //Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
                    //Debug.Log("체력 회복 업그레이드 완료. 현재 체력 회복량: " + playerRecoveryHealth);
                    return "업그레이드 성공";
            }
        }
        else
        {
            //Debug.Log("골드 또는 보석이 부족합니다. 업그레이드에 필요한 비용: " + upgradeCost);
            return "자원이 부족합니다.";
        }
        return "알 수 없는 오류가 발생했습니다.";
    }


    // 업그레이드 비용 계산 ----------------------------------------------------

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

    // 스테이지 몬스터, 컨트롤 --------------------------------------------------------

    public void MonsterKilled()
    {
        CountIncrease(); // 몬스터를 잡은 횟수 증가

        if (monsterCount % 10 == 0) // 20마리씩 잡을 때마다
        {
            monsterIncreaseHealth += 5; // 몬스터 체력 5 증가
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
        currentStage = value;       // 메인 0 스테이지 1부터 시작
        return currentStage;
    }

    void PlayBackgroundMusic()
    {

        // 현재 스테이지 번호를 기반으로 BGM 선택
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
        // 씬이 로드될 때마다 이전 오디오 중지하고 새로운 오디오 재생
        SoundManager.instance.StopAllAudio();
        PlayBackgroundMusic();
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 연결된 이벤트를 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}