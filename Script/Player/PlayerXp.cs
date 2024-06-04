using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    private static PlayerXp instance;

    [SerializeField] public int playerLevel = 1;
    [SerializeField] public int currentExperience = 0;
    [SerializeField] public int experienceToLevelUp = 100;
    [SerializeField] public int maxHealthIncreaseAmount = 20;
    [SerializeField] private float LvDestroyDelay = 1f;

    [SerializeField] private Slider xpBar;

    public GameObject lvupPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AssignReferences();
        GameManager.OnMonsterExp += HandleMonsterDeath;
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AssignReferences();
        UpdateXpBar();
    }

    private void AssignReferences()
    {
        GameObject xpBarObject = GameObject.Find("XpUI");
        xpBar = xpBarObject.GetComponent<Slider>();

        if (xpBar == null)
        {
            Debug.LogError("Xp Bar Slider를 찾을 수 없습니다.");
        }
    }

    // 몬스터 사망 이벤트를 처리하는 메서드
    private void HandleMonsterDeath(int experienceGained)
    {
        // 경험치 증가
        currentExperience += experienceGained;
        //Debug.Log("경험치 획득 : " + currentExperience);
        //Debug.Log("남은 경험치" + (experienceToLevelUp - currentExperience));

        UpdateXpBar();

        // 레벨 업 체크
        if (currentExperience >= experienceToLevelUp)
        {
            currentExperience -= experienceToLevelUp;
            LevelUp();
        }
    }

    // 경험치 바 업데이트 메서드
    private void UpdateXpBar()
    {
        xpBar.value = (float)currentExperience / experienceToLevelUp;
    }

    // 레벨 업 처리 메서드
    private void LevelUp()
    {
        GameObject player = gameObject;
        Vector3 offset = new Vector3(0f, 0f, -2f); // 플레이어 위치에서 내려갈 위치
        Vector3 lvPosition = player.transform.position + offset; // 플레이어 위치에서 오프셋 만큼 이동
        Vector3 lvRotation = new Vector3(-27f, 0f, 0f); // 회전값
        GameObject LvupInstance = Instantiate(lvupPrefab, lvPosition, Quaternion.Euler(lvRotation), player.transform); // 플레이어의 자식으로 생성


        StartCoroutine(DestroyLvup(LvupInstance));

        playerLevel++;
        experienceToLevelUp *= 2; // 예시로 경험치 필요량을 두 배로 증가시킴

        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.maxHealth += maxHealthIncreaseAmount;
            playerHealth.currentHealth = playerHealth.maxHealth; // 현재 체력을 최대 체력으로 설정
        }

        xpBar.value = 0;

        SoundManager.instance.PlaySfx(SoundManager.Sfx.LvUp);
    }

    private IEnumerator DestroyLvup(GameObject instance)
    {
        yield return new WaitForSeconds(LvDestroyDelay);
        Destroy(instance);
    }
}