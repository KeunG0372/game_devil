using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100; // ������ �⺻ ü��
    [SerializeField] public int currentHealth; // ���� ü��
    [SerializeField] public int monsterDamage = 10;
    [SerializeField] public bool isEliteMonster;
    [SerializeField] public bool isDamageCooldown = false;
    [SerializeField] private float damageCooldownDuration = 0.5f; // 0.5�� ���� �������� ���ߵ��� ����

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;
    private GameObject player;
    private EffectSprite effectSprite;

    private GameManager gameManager;

    public event Action<Collider> OnMonsterDeath; // ���Ͱ� �׾��� �� �߻��ϴ� �̺�Ʈ
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
        if (stage == 1) stage = 0;  // 2�������� ���� ����
        maxHealth = maxHealth + (stage * 100); // 2 �������� ���� 200, 300 , 400 ������ ����

        int countHealth = GameManager.Instance.GetIncreaseMonsterHealth();
        maxHealth += countHealth;
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
    }


    public int GetMonsterDamage()
    {
        return monsterDamage;
    }

    public GameObject GetTargetMonster()
    {
        return targetMonster;
    }


    // �������� �޴� �Լ�
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
        GameObject goldInstance = Instantiate(goldPrefab, transform.position, Quaternion.identity); // ��� ����
        goldInstance.transform.rotation = player.transform.rotation;

        if (gameManager.jewelDrop)
        {
            GameObject jewelInstance = Instantiate(jewelPrefab, transform.position, Quaternion.identity); // ���� ����
            jewelInstance.transform.rotation = player.transform.rotation;
        }

        OnMonsterDeath?.Invoke(GetComponent<Collider>()); // ���� ����
        OnMonsterDrop?.Invoke(this);


        if (isEliteMonster) // ����Ʈ ������ ��쿡�� �ش� �̺�Ʈ ȣ��
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