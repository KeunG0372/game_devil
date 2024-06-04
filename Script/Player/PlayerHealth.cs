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
    [SerializeField] private int damagePerMonster = 10; // 몬스터당 입는 데미지
    [SerializeField] public int recoveryHealth = 10;
    [SerializeField] int loopCounter = 0; // 체력 회복 루프 카운터

    public bool playerAlive = true;

    public List<Collider> monstersInContact = new List<Collider>(); // 접촉한 몬스터 리스트

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
            Debug.LogError("Hp Bar Slider를 찾을 수 없습니다.");
        }

        buttonController = FindObjectOfType<ButtonController>();
        effectSprite = FindObjectOfType<EffectSprite>();

        if (hpBar == null)
        {
            Debug.LogError("Hp Bar Slider를 찾을 수 없습니다.");
        }
        if (buttonController == null)
        {
            Debug.LogError("UIButtonController 스크립트를 찾을 수 없습니다.");
        }
        if (effectSprite == null)
        {
            Debug.LogError("effectSprite 스크립트 없음");
        }
    }

    private void SetMaxHealth(int health)
    {
        hpBar.maxValue = health;
        hpBar.value = health;
    }
    

    // 데미지
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

            CleanUpMonstersInContact(); // 리스트 정리
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

    void Die()  // 플레이어 사망
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
            monstersInContact.Add(other); // 몬스터를 접촉 리스트에 추가
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            monstersInContact.Clear(); ; // 몬스터를 접촉 리스트에서 제거
            StopCoroutine(ApplyDamageOverTime());
        }
    }

    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // 몬스터를 접촉 리스트에서 제거
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
