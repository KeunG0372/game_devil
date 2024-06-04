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
            Debug.LogError("Xp Bar Slider�� ã�� �� �����ϴ�.");
        }
    }

    // ���� ��� �̺�Ʈ�� ó���ϴ� �޼���
    private void HandleMonsterDeath(int experienceGained)
    {
        // ����ġ ����
        currentExperience += experienceGained;
        //Debug.Log("����ġ ȹ�� : " + currentExperience);
        //Debug.Log("���� ����ġ" + (experienceToLevelUp - currentExperience));

        UpdateXpBar();

        // ���� �� üũ
        if (currentExperience >= experienceToLevelUp)
        {
            currentExperience -= experienceToLevelUp;
            LevelUp();
        }
    }

    // ����ġ �� ������Ʈ �޼���
    private void UpdateXpBar()
    {
        xpBar.value = (float)currentExperience / experienceToLevelUp;
    }

    // ���� �� ó�� �޼���
    private void LevelUp()
    {
        GameObject player = gameObject;
        Vector3 offset = new Vector3(0f, 0f, -2f); // �÷��̾� ��ġ���� ������ ��ġ
        Vector3 lvPosition = player.transform.position + offset; // �÷��̾� ��ġ���� ������ ��ŭ �̵�
        Vector3 lvRotation = new Vector3(-27f, 0f, 0f); // ȸ����
        GameObject LvupInstance = Instantiate(lvupPrefab, lvPosition, Quaternion.Euler(lvRotation), player.transform); // �÷��̾��� �ڽ����� ����


        StartCoroutine(DestroyLvup(LvupInstance));

        playerLevel++;
        experienceToLevelUp *= 2; // ���÷� ����ġ �ʿ䷮�� �� ��� ������Ŵ

        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.maxHealth += maxHealthIncreaseAmount;
            playerHealth.currentHealth = playerHealth.maxHealth; // ���� ü���� �ִ� ü������ ����
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