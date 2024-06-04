using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100; // 몬스터의 기본 체력
    [SerializeField] public int currentHealth; // 현재 체력
    [SerializeField] public int monsterDamage = 10;
    [SerializeField] public bool isEliteMonster;
    [SerializeField] public bool isDamageCooldown = false;
    [SerializeField] private float damageCooldownDuration = 0.5f; // 0.5초 동안 움직임을 멈추도록 설정

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;
    private GameObject player;
    private EffectSprite effectSprite;

    private GameManager gameManager;

    public event Action<Collider> OnMonsterDeath; // 몬스터가 죽었을 때 발생하는 이벤트
    public event Action<MonsterHealth> OnMonsterDrop;



    public GameObject goldPrefab;
    public GameObject jewelPrefab;

    void Start()
    {
        gameManager = GameManager.Instance;
        monsterSpawn = FindObjectOfType<MonsterSpawn>();
        effectSprite = GetComponent<EffectSprite>();
        player = GameObject.FindGameObjectWithTag("Player");

        SetMonsterHealth();
    }

    private void SetMonsterHealth()
    {
        int stage = GameManager.Instance.GetCurrentStage();
        if (stage == 1) stage = 0;  // 2스테이지 부터 적용
        maxHealth = maxHealth + (stage * 100); // 2 스테이지 부터 200, 300 , 400 순으로 증가

        int countHealth = GameManager.Instance.GetIncreaseMonsterHealth();
        maxHealth += countHealth;
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
    }


    public int GetMonsterDamage()
    {
        return monsterDamage;
    }

    public GameObject GetTargetMonster()
    {
        return targetMonster;
    }


    // 데미지를 받는 함수
    public void TakeDamage(int damage)
    {
        isDamageCooldown = true;
        StartCoroutine(DamageCooldown());

        currentHealth -= damage;
        if (effectSprite != null)
        {
            effectSprite.ApplyEffect();
        }

        if (currentHealth <= 0)
        {
            GameManager.Instance.MonsterKilled();
            Die();
        }
    }
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldownDuration);
        isDamageCooldown = false;
    }

    public void SetTargetMonster(GameObject target)
    {
        targetMonster = target;
    }

    void Die()
    {
        GameObject goldInstance = Instantiate(goldPrefab, transform.position, Quaternion.identity); // 골드 생성
        goldInstance.transform.rotation = player.transform.rotation;

        if (gameManager.jewelDrop)
        {
            GameObject jewelInstance = Instantiate(jewelPrefab, transform.position, Quaternion.identity); // 보석 생성
            jewelInstance.transform.rotation = player.transform.rotation;
        }

        OnMonsterDeath?.Invoke(GetComponent<Collider>()); // 몬스터 제거
        OnMonsterDrop?.Invoke(this);


        if (isEliteMonster) // 엘리트 몬스터일 경우에만 해당 이벤트 호출
        {
            monsterSpawn.EliteMonsterDied();
        }
        else
        {
            monsterSpawn.MonsterDied();
        }

        Destroy(gameObject);
    }
}