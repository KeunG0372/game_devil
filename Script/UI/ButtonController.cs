using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;

public class ButtonController : MonoBehaviour
{
    public GameObject canvas; // Canvas ���� ������Ʈ�� ����

    public Animator playerAnimator;

    public PlayerSkill playerSkill;
    public PlayerController playerController;

    public GameObject upgradeUiPanel; // Ȱ��ȭ�� UI �г�
    public GameObject characterUiPanel; // Ȱ��ȭ�� UI �г�
    public GameObject settingUiPanel;

    public Text uiText;

    private string upgradeType; // ��ȭ Ÿ���� ��Ÿ���� ����

    private bool upgradeActivate = false;
    private bool characterActivate = false;
    private bool stageActivate = false;
    private bool settingActive = false;


    public UnityEngine.UI.Image charaImage;
    private Vector3 startPos;


    public UnityEngine.UI.Button fireButton;
    [SerializeField] public float fireCooldown = 4f;

    public UnityEngine.UI.Button iceButton;
    [SerializeField] public float iceCooldown = 4f;

    private bool fireIsCooldown = false;
    private bool iceIsCooldown = false;

    private GameManager gameManager;
    private UpgradeType selectedUpgradeType;

    public UnityEngine.UI.Button autoBtn;
    public PlayerAttack playerAttack;
    public PlayerHealth playerHealth;

    public Text[] charaText;

    public FadeInOut fade;
    public GameObject fadeLocation;
    public GameObject fadeObj;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private FadeInOut restartFade;
    public GameObject restartfadePrefab;
    private GameObject restartFadeObj;

    public GameObject deadPanel;

    public GameObject StageUiPanel;
    public UnityEngine.UI.Button stage1Btn;
    public UnityEngine.UI.Button stage2Btn;
    public UnityEngine.UI.Button stage3Btn;
    public UnityEngine.UI.Button stageMoveBtn;
    public GameObject[] stageImages;
    private int currentStageIndex = -1; // ���� ���õ� �������� �ε���

    public UnityEngine.UI.Slider bgmSlider;
    public UnityEngine.UI.Slider sfxSlider;

    public UnityEngine.UI.Text goldText;
    public UnityEngine.UI.Text jewelText;

    void Start()
    {
        gameManager = GameManager.Instance;

        upgradeUiPanel.SetActive(false);
        characterUiPanel.SetActive(false);
        StageUiPanel.SetActive(false);
        settingUiPanel.SetActive(false);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = playerObject.GetComponent<Animator>();
        playerController = playerObject.GetComponent<PlayerController>();
        playerHealth = playerObject.GetComponent<PlayerHealth>();

        // �������� ��ư�� Ŭ�� �̺�Ʈ ����
        stage1Btn.onClick.AddListener(() => OnStageButtonClick(1));
        stage2Btn.onClick.AddListener(() => OnStageButtonClick(2));
        stage3Btn.onClick.AddListener(() => OnStageButtonClick(3));

        // �̵��ϱ� ��ư�� Ŭ�� �̺�Ʈ ����
        stageMoveBtn.onClick.AddListener(OnStageMoveButtonClick);

        // �ʱ�ȭ �� ��� �������� �̹����� ��Ȱ��ȭ
        foreach (var image in stageImages)
        {
            image.SetActive(false);
        }


        // Fire ��ư Ŭ�� ��
        fireButton.onClick.AddListener(OnFireButton);

        // Ice ��ư Ŭ�� ��
        iceButton.onClick.AddListener(OnIceButton);

        startPos = charaImage.rectTransform.position; // �̹����� ���� ��ġ ����

        // �����̴� �ʱ� �� ����
        bgmSlider.value = SoundManager.instance.bgmVolume;
        sfxSlider.value = SoundManager.instance.sfxVolume;

        // �����̴� �� ���� �̺�Ʈ �ڵ鷯 ����
        bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);

        gameManager.OnGoldChanged += UpdateGoldText;
        gameManager.OnJewelChanged += UpdateJewelText;
        UpdateGoldText();
        UpdateJewelText();

        StartFadeIn();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnGoldChanged -= UpdateGoldText;
            gameManager.OnJewelChanged -= UpdateJewelText;
        }

        // �̺�Ʈ �ڵ鷯 ����
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderValueChanged);
    }


    // ���̵� �� �ƿ� -----------------------------------------------------------------------------

    private void StartFadeIn()
    {
        fadeObj.SetActive(true);

        if (!isFadingIn)
        {
            StartCoroutine(StartFadeInCoroutine());
        }
    }
    public void StartFadeOut()
    {
        fadeObj.SetActive(true);

        if (!isFadingOut)
        {
            StartCoroutine(StartFadeOutCoroutine());
        }
    }

    private IEnumerator StartFadeInCoroutine()
    {
        isFadingIn = true; // ���̵��� ������ ǥ��
        UnityEngine.UI.Image fadeImage = fadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeIn(fadeImage));
        yield return new WaitForSeconds(0.5f);
        fadeObj.SetActive(false);
        //Destroy(fadeObj);

        isFadingIn = false; // ���̵��� ����
    }

    private IEnumerator StartFadeOutCoroutine()
    {
        isFadingOut = true; // ���̵�ƿ� ������ ǥ��
        UnityEngine.UI.Image fadeImage = fadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeOut(fadeImage));
        yield return new WaitForSeconds(3f);
        //fadeObj.SetActive(false); // ���̵�ƿ� �� ��Ȱ��ȭ
        isFadingOut = false; // ���̵�ƿ� ����
    }

    //UI Ȱ��ȭ -----------------------------------------------------------------------------

    public void OnUpgradeUiButtonClick()
    {
        if (!upgradeActivate)
        {
            upgradeUiPanel.SetActive(true);
            upgradeActivate = true;
        }
        else
        {
            upgradeUiPanel.SetActive(false);
            uiText.text = "";
            upgradeActivate = false;
        }
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting1);
    }

    public void OnCharacterUiButtonClick()
    {

        if (!characterActivate)
        {
            StartCoroutine(FloatUp());
            SetCharaText();
            characterUiPanel.SetActive(true);
            characterActivate = true;
        }
        else
        {
            characterUiPanel.SetActive(false);
            characterActivate = false;
            StopCoroutine(FloatUp());
            StopCoroutine(FloatBack());
        }
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting1);
    }

    public void OnStageBtn()
    {
        if (!stageActivate)
        {
            StageUiPanel.SetActive(true);
            stageActivate = true;
            int stage = GameManager.Instance.GetCurrentStage();
            stageImages[stage - 1].SetActive(true);
        }
        else
        {
            StageUiPanel.SetActive(false);
            stageActivate = false;
        }
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting1);
    }

    public void OnSettingButtonClick()
    {
        if (!settingActive)
        {
            settingUiPanel.SetActive(true);
            settingActive = true;
        }
        else
        {
            settingUiPanel.SetActive(false);
            settingActive = false;
        }

        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting1);
    }

    //��ų��ư -----------------------------------------------------------------------------
    public void OnFireButton()
    {
        if (!fireIsCooldown)
        {
            // ���� ��ų Ÿ���� Fire�� ����
            playerSkill.currentSkill = PlayerSkill.SkillType.Fire;
            playerSkill.UseSkill();
            playerAnimator.SetTrigger("isAttack");

            StartCoroutine(StartFireCooldown());
        }
    }

    public void OnIceButton()
    {
        if (!iceIsCooldown)
        {
            // ���� ��ų Ÿ���� Ice�� ����
            playerSkill.currentSkill = PlayerSkill.SkillType.Ice;
            playerSkill.UseSkill();
            playerAnimator.SetTrigger("isAttack");

            StartCoroutine(StartIceCooldown());
        }
    }

    IEnumerator StartFireCooldown()
    {
        fireButton.interactable = false;
        fireIsCooldown = true;

        yield return new WaitForSeconds(fireCooldown);

        // ��Ÿ�� ����
        fireButton.interactable = true;
        fireIsCooldown = false;
    }

    IEnumerator StartIceCooldown()
    {
        iceButton.interactable = false;
        iceIsCooldown = true;

        yield return new WaitForSeconds(iceCooldown);

        // ��Ÿ�� ����
        iceButton.interactable = true;
        iceIsCooldown = false;
    }

    //��ȭ��ư�� --------------------------------------------------------------------------------
    private void UpdateGoldText()
    {
        goldText.text = "" + gameManager.gold;
    }

    private void UpdateJewelText()
    {
        jewelText.text = "" + gameManager.jewel;
    }

    public void SelectUpgrade(string upgradeTypeString)
    {
        selectedUpgradeType = (UpgradeType)System.Enum.Parse(typeof(UpgradeType), upgradeTypeString, true);
        SetUpgradeDescription(selectedUpgradeType);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting2);
    }

    private void SetUpgradeDescription(UpgradeType upgradeType)
    {
        int upgradeCost = gameManager.CalculateUpgradeCost(upgradeType);

        switch (upgradeType)
        {
            case UpgradeType.Attack:
                uiText.text = "�⺻���� ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.playerDamage;
                uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.playerDamage + gameManager.damageIncrease);
                uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                break;
            case UpgradeType.Fire:
                uiText.text = "�ҵ� ��ų ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.fireDamage;
                uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.fireDamage + gameManager.damageIncrease);
                uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                break;
            case UpgradeType.Gold:
                uiText.text = "��� ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msGold;
                uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msGold + gameManager.goldIncrease);
                uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                break;
            case UpgradeType.Jewel:
                uiText.text = "���� ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msJewel;
                uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msJewel + gameManager.goldIncrease);
                uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                break;
            case UpgradeType.Ice:
                uiText.text = "���̽� ��ų ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.iceDamage;
                uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.iceDamage + gameManager.damageIncrease);
                uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                break;
            case UpgradeType.PlMove:
                uiText.text = "�̵��ӵ� ��ȭ";
                uiText.text += "\n\n���� �̵��ӵ� : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
                uiText.text += "\n��ȭ �� �̵��ӵ� : " + (GameManager.Instance.playerMoveSpeed + 0.3f).ToString("F1");
                uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                break;
            case UpgradeType.Exp:
                uiText.text = "����ġ ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msExp;
                uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msExp + gameManager.expIncrease);
                uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                break;
            case UpgradeType.AtkSpeed:
                uiText.text = "���ݼӵ� ��ȭ";
                uiText.text += "\n\n���� ���ݼӵ� : " + GameManager.Instance.playerAttackTime.ToString("F2");
                uiText.text += "\n��ȭ �� ���ݼӵ� : " + (GameManager.Instance.playerAttackTime - 0.05f).ToString("F2");
                uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                break;
            case UpgradeType.PlHp:
                uiText.text = "ü�� ȸ���� ��ȭ";
                uiText.text += "\n\n���� ȸ���� : " + playerHealth.recoveryHealth;
                uiText.text += "\n��ȭ �� ȸ���� : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                break;
        }
    }

    // ��ȭ�ϱ� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnUpgradeButtonClick()
    {
        string upgradeResult = gameManager.Upgrade(selectedUpgradeType);
        int upgradeCost = gameManager.CalculateUpgradeCost(selectedUpgradeType);

        if (upgradeResult == "���׷��̵� ����")
        {
            switch (selectedUpgradeType)
            {
                case UpgradeType.Attack:
                    uiText.text = "�⺻���� ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ���ݷ� : " + gameManager.playerDamage;
                    uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.playerDamage + gameManager.damageIncrease);
                    uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                    break;
                case UpgradeType.Fire:
                    uiText.text = "�ҵ� ��ų ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ���ݷ� : " + gameManager.fireDamage;
                    uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.fireDamage + gameManager.damageIncrease);
                    uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                    break;
                case UpgradeType.Gold:
                    uiText.text = "��� ȹ�淮 ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msGold;
                    uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msGold + gameManager.goldIncrease);
                    uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                    break;
                case UpgradeType.Jewel:
                    uiText.text = "���� ȹ�淮 ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msJewel;
                    uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msJewel + gameManager.goldIncrease);
                    uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                    break;
                case UpgradeType.Ice:
                    uiText.text = "���̽� ��ų ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ���ݷ� : " + gameManager.iceDamage;
                    uiText.text += "\n��ȭ �� ���ݷ� : " + (gameManager.iceDamage + gameManager.damageIncrease);
                    uiText.text += "\n �ʿ� ��� : " + upgradeCost;
                    break;
                case UpgradeType.PlMove:
                    uiText.text = "�̵��ӵ� ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� �̵��ӵ� : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
                    uiText.text += "\n��ȭ �� �̵��ӵ� : " + (GameManager.Instance.playerMoveSpeed + 0.3f).ToString("F1");
                    uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                    break;
                case UpgradeType.Exp:
                    uiText.text = "����ġ ȹ�淮 ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msExp;
                    uiText.text += "\n��ȭ �� ȹ�淮 : " + (gameManager.msExp + gameManager.expIncrease);
                    uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                    break;
                case UpgradeType.AtkSpeed:
                    uiText.text = "���ݼӵ� ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ���ݼӵ� : " + GameManager.Instance.playerAttackTime.ToString("F2");
                    uiText.text += "\n��ȭ �� ���ݼӵ� : " + (GameManager.Instance.playerAttackTime - 0.05f).ToString("F2");
                    uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                    break;
                case UpgradeType.PlHp:
                    uiText.text = "ü�� ȸ���� ��ȭ �Ϸ�";
                    uiText.text += "\n\n���� ȸ���� : " + playerHealth.recoveryHealth;
                    uiText.text += "\n��ȭ �� ȸ���� : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                    uiText.text += "\n �ʿ� ���� : " + upgradeCost;
                    break;
            }
        }
        else
        {
            uiText.text = upgradeResult;
        }

        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting2);
        SetCharaText();
    }
    public void UpgradeAttackDamage() => SelectUpgrade("Attack");
    public void UpgradeFireDamage() => SelectUpgrade("Fire");
    public void UpgradeGold() => SelectUpgrade("Gold");
    public void UpgradeJewel() => SelectUpgrade("Jewel");
    public void UpgradeIceDamage() => SelectUpgrade("Ice");
    public void UpgradePlMoveSpeed() => SelectUpgrade("PlMove");
    public void UpgradeExp() => SelectUpgrade("Exp");
    public void UpgradeAtkSpeed() => SelectUpgrade("AtkSpeed");
    public void UpgradePlHp() => SelectUpgrade("PlHp");


    //�����ư
    public void AutoBtn()
    {
        if (playerSkill.autoMode == true)   //������ On�� ���� Ŭ��
        {
            playerSkill.autoMode = false;
            autoBtn.image.color = new Color(autoBtn.image.color.r, autoBtn.image.color.g, autoBtn.image.color.b, 1f);
        }
        else if (playerSkill.autoMode == false) //������ OFF�� ���� Ŭ��
        {
            playerSkill.autoMode = true;
            autoBtn.image.color = new Color(autoBtn.image.color.r, autoBtn.image.color.g, autoBtn.image.color.b, 0.4f);

            StartCoroutine(AutoSkill());
        }
    }

    IEnumerator AutoSkill()
    {
        while (playerSkill.autoMode)
        {
            OnFireButton();
            yield return new WaitForSeconds(1.5f);
            OnIceButton();
            yield return new WaitForSeconds(1f);
        }
    }


    IEnumerator FloatUp()
    {
        Vector3 startPos = charaImage.rectTransform.position; // �̹����� ���� ��ġ ����
        Vector3 targetPos = startPos + Vector3.up * 10f; // �̹����� �ö� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            charaImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� ���� ��ġ�� �ǵ��ƿ��� �մϴ�.
        StartCoroutine(FloatBack());
    }

    IEnumerator FloatBack()
    {
        Vector3 targetPos = charaImage.rectTransform.position; // �̹����� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 startPos = targetPos - Vector3.up * 10f; // �̹����� ���� ��ġ�� �ǵ��ư� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            charaImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� �ö󰡴� �κ��� �ݺ��մϴ�.
        StartCoroutine(FloatUp());
    }

    void SetCharaText()
    {
        if (charaText.Length < 8)
        {
            Debug.LogError("�ؽ�Ʈ �迭�� ���̰� ������� �ʽ��ϴ�.");
            return;
        }

        charaText[0].text = "ü�� : " + playerHealth.maxHealth;
        charaText[1].text = "�⺻ ���ݷ� : " + gameManager.playerDamage;
        charaText[2].text = "�ҵ� ���ݷ� : " + gameManager.fireDamage;
        charaText[3].text = "���̽� ���ݷ� : " + gameManager.iceDamage;
        charaText[4].text = "�̵��ӵ� : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
        charaText[5].text = "ȹ�� ����ġ : " + gameManager.msExp;
        charaText[6].text = "���ݼӵ� : " + GameManager.Instance.playerAttackTime.ToString("F2");
        charaText[7].text = "ü�� ȸ���� : " + playerHealth.recoveryHealth;
    }

    public IEnumerator DeadEventCor()
    {
        playerAnimator.SetBool("isAlive", playerHealth.playerAlive);   // �ִϸ��̼� ��ȯ
        yield return new WaitForSeconds(1.5f);
        /*
        restartFadeObj = Instantiate(restartfadePrefab);
        restartFadeObj.transform.SetParent(fadeLocation.transform, false);

        UnityEngine.UI.Image fadeImage = restartFadeObj.GetComponent<UnityEngine.UI.Image>();
        restartFade = restartFadeObj.GetComponent<FadeInOut>();
        yield return StartCoroutine(restartFade.FadeOut(fadeImage));

        yield return new WaitForSeconds(1.5f);
        */
        StartFadeOut();
        deadPanel.SetActive(true);
    }


    public void OnLastStageBtn()
    {
        int currentStage = GameManager.Instance.GetCurrentStage();

        if (currentStage > 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Animator animator = player.GetComponent<Animator>();

            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.playerAlive = true;
                    playerHealth.PlayerRestart();
                    animator.SetBool("isAlive", true);
                }
            }

            GameManager.Instance.MonsterCountReset();

            gameManager.SetStage(currentStage - 1);
            LoadStage(currentStage - 1);
        }
        else
        {
            //Debug.LogWarning("�������� ���ư� �������� ����.");
        }
    }

    public void OnRestartBtn()
    {
        int currentStage = GameManager.Instance.GetCurrentStage();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator animator = player.GetComponent<Animator>();

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.playerAlive = true;
                playerHealth.PlayerRestart();
                animator.SetBool("isAlive", true);
            }
        }

        GameManager.Instance.MonsterCountReset();

        GameManager.Instance.SetStage(currentStage);
        LoadStage(currentStage);
    }

    private IEnumerator RestartFadeInCoroutine()
    {
        isFadingIn = true; // ���̵��� ������ ǥ��
        UnityEngine.UI.Image refadeImage = restartFadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeIn(refadeImage));
        yield return new WaitForSeconds(0.5f);
        Destroy(restartFadeObj);

        isFadingIn = false; // ���̵��� ����
    }


    // �������� ���� ----------------------------------------------------------------
    void OnStageButtonClick(int stageIndex)
    {
        // ��� �������� �̹����� ��Ȱ��ȭ
        foreach (var image in stageImages)
        {
            image.SetActive(false);
        }

        // ���õ� �������� �̹����� Ȱ��ȭ
        if (stageIndex >= 0 && stageIndex <= stageImages.Length)
        {
            stageImages[stageIndex - 1].SetActive(true);
            currentStageIndex = stageIndex;
        }
    }

    void OnStageMoveButtonClick()
    {
        if (currentStageIndex >= 1)
        {
            // �������� �̵� ó�� (���⼭�� Debug.Log�� ��ü)
            //Debug.Log("�̵��ϱ� ��ư Ŭ����. ��������: " + currentStageIndex);
            // ���� �������� �̵� �ڵ带 ���⿡ �߰�
            gameManager.SetStage(currentStageIndex);
            LoadStage(currentStageIndex);
        }
        else
        {
            //Debug.LogWarning("���������� ���õ��� �ʾҽ��ϴ�.");
        }
    }

    void LoadStage(int stageIndex)
    {
        StartFadeOut();
          
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Vector3 newPosition = new Vector3(1.79f, 5.31f, -2.95f);
        playerObject.transform.position = newPosition;

        playerHealth = playerObject.GetComponent<PlayerHealth>();

        if (playerHealth.isCoroutineOn)
        {
            playerHealth.StopAllCoroutines();
            playerHealth.isCoroutineOn = false;
        }

        

        // ���� �������� �̵� ó�� (�� ��ȯ ��)
        string sceneName = "Stage" + stageIndex; // ���������� �ش��ϴ� �� �̸�
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // ------------------------------------ BGM, SFX �����̴� --------------------------------------

    public void OnBgmSliderValueChanged(float value)
    {
        SoundManager.instance.SetBgmVolume(value);
    }

    public void OnSfxSliderValueChanged(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }
}
