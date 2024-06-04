using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    private static PlayerHealth instance;

    [SerializeField] public int maxHealth = 500;
    [SerializeField] public int currentHealth;
    [SerializeField] private int damagePerMonster = 10; // ���ʹ� �Դ� ������
    [SerializeField] public int recoveryHealth = 10;
    [SerializeField] int loopCounter = 0; // ü�� ȸ�� ���� ī����

    public bool playerAlive = true;

    public List<Collider> monstersInContact = new List<Collider>(); // ������ ���� ����Ʈ

    [SerializeField] private UnityEngine.UI.Slider hpBar;

    public EffectSprite effectSprite;

    public ButtonController buttonController;

    public bool isCoroutineOn = false;


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

    void Start()
    {
        AssignReferences();

        effectSprite = GetComponent<EffectSprite>();
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);

        if (!isCoroutineOn)
        {
            StartCoroutine(ApplyDamageOverTime());
            StartCoroutine(EventCoroutine());
            isCoroutineOn = true;
        }
        
        playerAlive = true;
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

        effectSprite = GetComponent<EffectSprite>();
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);

        if (!isCoroutineOn)
        {
            StartCoroutine(ApplyDamageOverTime());
            StartCoroutine(EventCoroutine());
            isCoroutineOn = true;

            currentHealth = maxHealth;
            SetMaxHealth(maxHealth);
        }

        playerAlive = true;
    }

    private void AssignReferences()
    {
        GameObject hpBarObject = GameObject.Find("HpUI");
        if (hpBarObject != null)
        {
            hpBar = hpBarObject.GetComponent<UnityEngine.UI.Slider>();
        }
        else
        {
            Debug.LogError("Hp Bar Slider�� ã�� �� �����ϴ�.");
        }

        buttonController = FindObjectOfType<ButtonController>();
        effectSprite = FindObjectOfType<EffectSprite>();

        if (hpBar == null)
        {
            Debug.LogError("Hp Bar Slider�� ã�� �� �����ϴ�.");
        }
        if (buttonController == null)
        {
            Debug.LogError("UIButtonController ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
        if (effectSprite == null)
        {
            Debug.LogError("effectSprite ��ũ��Ʈ ����");
        }
    }

    private void SetMaxHealth(int health)
    {
        hpBar.maxValue = health;
        hpBar.value = health;
    }
    

    // ������
    public IEnumerator ApplyDamageOverTime()
    {
        while (playerAlive)
        {
            loopCounter++;
            int totalDamage = monstersInContact.Count * damagePerMonster;
            TakeDamage(totalDamage);

            if (totalDamage > 0)
            {
                if (effectSprite == null)
                {
                    effectSprite = FindObjectOfType<EffectSprite>();
                }
                else
                {
                    effectSprite.ApplyEffect();
                    loopCounter = 0;
                }
            }

            if (loopCounter == 4)
            {
                if (maxHealth > currentHealth)
                {
                    currentHealth += recoveryHealth;
                    hpBar.value = currentHealth;
                    loopCounter = 0;
                }
            }

            CleanUpMonstersInContact(); // ����Ʈ ����
            yield return new WaitForSeconds(1f);
        }
    }


    void TakeDamage(int damage)
    {
        if (playerAlive) {
            currentHealth -= damage;
            hpBar.value = currentHealth;

            if (currentHealth <= 0)
            {
                playerAlive = false;
                hpBar.value = 0;
                Die();
            }
        }
    }

    void Die()  // �÷��̾� ���
    {
        StartCoroutine(buttonController.DeadEventCor());
        StopCoroutine(ApplyDamageOverTime());
        StopCoroutine(EventCoroutine());
    }

    public void PlayerRestart()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monstersInContact.Add(other); // ���͸� ���� ����Ʈ�� �߰�
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monstersInContact.Clear(); ; // ���͸� ���� ����Ʈ���� ����
            StopCoroutine(ApplyDamageOverTime());
        }
    }

    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // ���͸� ���� ����Ʈ���� ����
    }

    private void CleanUpMonstersInContact()
    {
        monstersInContact.RemoveAll(monster => monster == null || monster.gameObject == null);
    }

    public IEnumerator EventCoroutine()
    {
        while (true)
        {
            foreach (MonsterHealth monsterHealth in FindObjectsOfType<MonsterHealth>())
            {
                monsterHealth.OnMonsterDeath += RemoveMonsterFromContactList;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
