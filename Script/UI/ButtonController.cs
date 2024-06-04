using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameManager;

public class ButtonController : MonoBehaviour
{
    public GameObject canvas; // Canvas 게임 오브젝트를 참조

    public Animator playerAnimator;

    public PlayerSkill playerSkill;
    public PlayerController playerController;

    public GameObject upgradeUiPanel; // 활성화할 UI 패널
    public GameObject characterUiPanel; // 활성화할 UI 패널
    public GameObject settingUiPanel;

    public Text uiText;

    private string upgradeType; // 강화 타입을 나타내는 변수

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
    private int currentStageIndex = -1; // 현재 선택된 스테이지 인덱스

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

        // 스테이지 버튼에 클릭 이벤트 연결
        stage1Btn.onClick.AddListener(() => OnStageButtonClick(1));
        stage2Btn.onClick.AddListener(() => OnStageButtonClick(2));
        stage3Btn.onClick.AddListener(() => OnStageButtonClick(3));

        // 이동하기 버튼에 클릭 이벤트 연결
        stageMoveBtn.onClick.AddListener(OnStageMoveButtonClick);

        // 초기화 시 모든 스테이지 이미지를 비활성화
        foreach (var image in stageImages)
        {
            image.SetActive(false);
        }


        // Fire 버튼 클릭 시
        fireButton.onClick.AddListener(OnFireButton);

        // Ice 버튼 클릭 시
        iceButton.onClick.AddListener(OnIceButton);

        startPos = charaImage.rectTransform.position; // 이미지의 시작 위치 저장

        // 슬라이더 초기 값 설정
        bgmSlider.value = SoundManager.instance.bgmVolume;
        sfxSlider.value = SoundManager.instance.sfxVolume;

        // 슬라이더 값 변경 이벤트 핸들러 설정
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

        // 이벤트 핸들러 해제
        bgmSlider.onValueChanged.RemoveListener(OnBgmSliderValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderValueChanged);
    }


    // 페이드 인 아웃 -----------------------------------------------------------------------------

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
        isFadingIn = true; // 페이드인 중임을 표시
        UnityEngine.UI.Image fadeImage = fadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeIn(fadeImage));
        yield return new WaitForSeconds(0.5f);
        fadeObj.SetActive(false);
        //Destroy(fadeObj);

        isFadingIn = false; // 페이드인 종료
    }

    private IEnumerator StartFadeOutCoroutine()
    {
        isFadingOut = true; // 페이드아웃 중임을 표시
        UnityEngine.UI.Image fadeImage = fadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeOut(fadeImage));
        yield return new WaitForSeconds(3f);
        //fadeObj.SetActive(false); // 페이드아웃 후 비활성화
        isFadingOut = false; // 페이드아웃 종료
    }

    //UI 활성화 -----------------------------------------------------------------------------

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

    //스킬버튼 -----------------------------------------------------------------------------
    public void OnFireButton()
    {
        if (!fireIsCooldown)
        {
            // 현재 스킬 타입을 Fire로 변경
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
            // 현재 스킬 타입을 Ice로 변경
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

        // 쿨타임 종료
        fireButton.interactable = true;
        fireIsCooldown = false;
    }

    IEnumerator StartIceCooldown()
    {
        iceButton.interactable = false;
        iceIsCooldown = true;

        yield return new WaitForSeconds(iceCooldown);

        // 쿨타임 종료
        iceButton.interactable = true;
        iceIsCooldown = false;
    }

    //강화버튼들 --------------------------------------------------------------------------------
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
                uiText.text = "기본공격 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.playerDamage;
                uiText.text += "\n강화 후 공격력 : " + (gameManager.playerDamage + gameManager.damageIncrease);
                uiText.text += "\n 필요 골드 : " + upgradeCost;
                break;
            case UpgradeType.Fire:
                uiText.text = "소드 스킬 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.fireDamage;
                uiText.text += "\n강화 후 공격력 : " + (gameManager.fireDamage + gameManager.damageIncrease);
                uiText.text += "\n 필요 골드 : " + upgradeCost;
                break;
            case UpgradeType.Gold:
                uiText.text = "골드 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msGold;
                uiText.text += "\n강화 후 획득량 : " + (gameManager.msGold + gameManager.goldIncrease);
                uiText.text += "\n 필요 골드 : " + upgradeCost;
                break;
            case UpgradeType.Jewel:
                uiText.text = "보석 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msJewel;
                uiText.text += "\n강화 후 획득량 : " + (gameManager.msJewel + gameManager.goldIncrease);
                uiText.text += "\n 필요 골드 : " + upgradeCost;
                break;
            case UpgradeType.Ice:
                uiText.text = "아이스 스킬 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.iceDamage;
                uiText.text += "\n강화 후 공격력 : " + (gameManager.iceDamage + gameManager.damageIncrease);
                uiText.text += "\n 필요 골드 : " + upgradeCost;
                break;
            case UpgradeType.PlMove:
                uiText.text = "이동속도 강화";
                uiText.text += "\n\n현재 이동속도 : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
                uiText.text += "\n강화 후 이동속도 : " + (GameManager.Instance.playerMoveSpeed + 0.3f).ToString("F1");
                uiText.text += "\n 필요 보석 : " + upgradeCost;
                break;
            case UpgradeType.Exp:
                uiText.text = "경험치 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msExp;
                uiText.text += "\n강화 후 획득량 : " + (gameManager.msExp + gameManager.expIncrease);
                uiText.text += "\n 필요 보석 : " + upgradeCost;
                break;
            case UpgradeType.AtkSpeed:
                uiText.text = "공격속도 강화";
                uiText.text += "\n\n현재 공격속도 : " + GameManager.Instance.playerAttackTime.ToString("F2");
                uiText.text += "\n강화 후 공격속도 : " + (GameManager.Instance.playerAttackTime - 0.05f).ToString("F2");
                uiText.text += "\n 필요 보석 : " + upgradeCost;
                break;
            case UpgradeType.PlHp:
                uiText.text = "체력 회복량 강화";
                uiText.text += "\n\n현재 회복량 : " + playerHealth.recoveryHealth;
                uiText.text += "\n강화 후 회복량 : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                uiText.text += "\n 필요 보석 : " + upgradeCost;
                break;
        }
    }

    // 강화하기 버튼 클릭 시 호출되는 메서드
    public void OnUpgradeButtonClick()
    {
        string upgradeResult = gameManager.Upgrade(selectedUpgradeType);
        int upgradeCost = gameManager.CalculateUpgradeCost(selectedUpgradeType);

        if (upgradeResult == "업그레이드 성공")
        {
            switch (selectedUpgradeType)
            {
                case UpgradeType.Attack:
                    uiText.text = "기본공격 강화 완료";
                    uiText.text += "\n\n현재 공격력 : " + gameManager.playerDamage;
                    uiText.text += "\n강화 후 공격력 : " + (gameManager.playerDamage + gameManager.damageIncrease);
                    uiText.text += "\n 필요 골드 : " + upgradeCost;
                    break;
                case UpgradeType.Fire:
                    uiText.text = "소드 스킬 강화 완료";
                    uiText.text += "\n\n현재 공격력 : " + gameManager.fireDamage;
                    uiText.text += "\n강화 후 공격력 : " + (gameManager.fireDamage + gameManager.damageIncrease);
                    uiText.text += "\n 필요 골드 : " + upgradeCost;
                    break;
                case UpgradeType.Gold:
                    uiText.text = "골드 획득량 강화 완료";
                    uiText.text += "\n\n현재 획득량 : " + gameManager.msGold;
                    uiText.text += "\n강화 후 획득량 : " + (gameManager.msGold + gameManager.goldIncrease);
                    uiText.text += "\n 필요 골드 : " + upgradeCost;
                    break;
                case UpgradeType.Jewel:
                    uiText.text = "보석 획득량 강화 완료";
                    uiText.text += "\n\n현재 획득량 : " + gameManager.msJewel;
                    uiText.text += "\n강화 후 획득량 : " + (gameManager.msJewel + gameManager.goldIncrease);
                    uiText.text += "\n 필요 골드 : " + upgradeCost;
                    break;
                case UpgradeType.Ice:
                    uiText.text = "아이스 스킬 강화 완료";
                    uiText.text += "\n\n현재 공격력 : " + gameManager.iceDamage;
                    uiText.text += "\n강화 후 공격력 : " + (gameManager.iceDamage + gameManager.damageIncrease);
                    uiText.text += "\n 필요 골드 : " + upgradeCost;
                    break;
                case UpgradeType.PlMove:
                    uiText.text = "이동속도 강화 완료";
                    uiText.text += "\n\n현재 이동속도 : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
                    uiText.text += "\n강화 후 이동속도 : " + (GameManager.Instance.playerMoveSpeed + 0.3f).ToString("F1");
                    uiText.text += "\n 필요 보석 : " + upgradeCost;
                    break;
                case UpgradeType.Exp:
                    uiText.text = "경험치 획득량 강화 완료";
                    uiText.text += "\n\n현재 획득량 : " + gameManager.msExp;
                    uiText.text += "\n강화 후 획득량 : " + (gameManager.msExp + gameManager.expIncrease);
                    uiText.text += "\n 필요 보석 : " + upgradeCost;
                    break;
                case UpgradeType.AtkSpeed:
                    uiText.text = "공격속도 강화 완료";
                    uiText.text += "\n\n현재 공격속도 : " + GameManager.Instance.playerAttackTime.ToString("F2");
                    uiText.text += "\n강화 후 공격속도 : " + (GameManager.Instance.playerAttackTime - 0.05f).ToString("F2");
                    uiText.text += "\n 필요 보석 : " + upgradeCost;
                    break;
                case UpgradeType.PlHp:
                    uiText.text = "체력 회복량 강화 완료";
                    uiText.text += "\n\n현재 회복량 : " + playerHealth.recoveryHealth;
                    uiText.text += "\n강화 후 회복량 : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                    uiText.text += "\n 필요 보석 : " + upgradeCost;
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


    //오토버튼
    public void AutoBtn()
    {
        if (playerSkill.autoMode == true)   //오토모드 On인 상태 클릭
        {
            playerSkill.autoMode = false;
            autoBtn.image.color = new Color(autoBtn.image.color.r, autoBtn.image.color.g, autoBtn.image.color.b, 1f);
        }
        else if (playerSkill.autoMode == false) //오토모드 OFF인 상태 클릭
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
        Vector3 startPos = charaImage.rectTransform.position; // 이미지의 시작 위치 저장
        Vector3 targetPos = startPos + Vector3.up * 10f; // 이미지가 올라갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            charaImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 시작 위치로 되돌아오게 합니다.
        StartCoroutine(FloatBack());
    }

    IEnumerator FloatBack()
    {
        Vector3 targetPos = charaImage.rectTransform.position; // 이미지의 현재 위치를 목표 위치로 설정
        Vector3 startPos = targetPos - Vector3.up * 10f; // 이미지가 시작 위치로 되돌아갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            charaImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 올라가는 부분을 반복합니다.
        StartCoroutine(FloatUp());
    }

    void SetCharaText()
    {
        if (charaText.Length < 8)
        {
            Debug.LogError("텍스트 배열의 길이가 충분하지 않습니다.");
            return;
        }

        charaText[0].text = "체력 : " + playerHealth.maxHealth;
        charaText[1].text = "기본 공격력 : " + gameManager.playerDamage;
        charaText[2].text = "소드 공격력 : " + gameManager.fireDamage;
        charaText[3].text = "아이스 공격력 : " + gameManager.iceDamage;
        charaText[4].text = "이동속도 : " + GameManager.Instance.playerMoveSpeed.ToString("F1");
        charaText[5].text = "획득 경험치 : " + gameManager.msExp;
        charaText[6].text = "공격속도 : " + GameManager.Instance.playerAttackTime.ToString("F2");
        charaText[7].text = "체력 회복량 : " + playerHealth.recoveryHealth;
    }

    public IEnumerator DeadEventCor()
    {
        playerAnimator.SetBool("isAlive", playerHealth.playerAlive);   // 애니메이션 전환
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
            //Debug.LogWarning("이전으로 돌아갈 스테이지 없음.");
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
        isFadingIn = true; // 페이드인 중임을 표시
        UnityEngine.UI.Image refadeImage = restartFadeObj.GetComponent<UnityEngine.UI.Image>();
        yield return StartCoroutine(fade.FadeIn(refadeImage));
        yield return new WaitForSeconds(0.5f);
        Destroy(restartFadeObj);

        isFadingIn = false; // 페이드인 종료
    }


    // 스테이지 선택 ----------------------------------------------------------------
    void OnStageButtonClick(int stageIndex)
    {
        // 모든 스테이지 이미지를 비활성화
        foreach (var image in stageImages)
        {
            image.SetActive(false);
        }

        // 선택된 스테이지 이미지를 활성화
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
            // 스테이지 이동 처리 (여기서는 Debug.Log로 대체)
            //Debug.Log("이동하기 버튼 클릭됨. 스테이지: " + currentStageIndex);
            // 실제 스테이지 이동 코드를 여기에 추가
            gameManager.SetStage(currentStageIndex);
            LoadStage(currentStageIndex);
        }
        else
        {
            //Debug.LogWarning("스테이지가 선택되지 않았습니다.");
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

        

        // 실제 스테이지 이동 처리 (씬 전환 등)
        string sceneName = "Stage" + stageIndex; // 스테이지에 해당하는 씬 이름
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // ------------------------------------ BGM, SFX 슬라이더 --------------------------------------

    public void OnBgmSliderValueChanged(float value)
    {
        SoundManager.instance.SetBgmVolume(value);
    }

    public void OnSfxSliderValueChanged(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }
}
