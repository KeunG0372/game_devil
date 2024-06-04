using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterInfo
{
    public GameObject monsterObject; // ���� ��ü
    public Transform monsterTransform; // ������ Transform ����

    public MonsterInfo(GameObject monster, Transform transform)
    {
        monsterObject = monster;
        monsterTransform = transform;
    }
}



public class MonsterSpawn : MonoBehaviour
{
    public GameObject normalMonsterPrefab;    // �Ϲ� ���� ������
    public GameObject eliteMonsterPrefab;     // ����Ʈ ���� ������

    [SerializeField] public int maxNormalMonsters = 5;         // ������ �Ϲ� ���� ��
    [SerializeField] public int maxEliteMonsters = 1;          // ������ ����Ʈ ���� ��

    private int currentNormalMonsters = 0;    // ������ �Ϲ� ���� ��
    private int currentEliteMonsters = 0;     // ������ ����Ʈ ���� ��

    private int dieMonsterCount;

    [SerializeField] private float minSpawnDistance = 10f;      // �ּ� ���� �Ÿ�
    [SerializeField] private float maxSpawnDistance = 15f;      // �ִ� ���� �Ÿ�


    private GameObject monsterFolder;
    private Transform folderTransform;

    public static event Action<MonsterInfo> OnMonsterSpawn; // ���� ���� �̺�Ʈ

    private void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // ���� ���� �ӵ�

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // �Ϲ� ���� ����
                if (currentNormalMonsters < maxNormalMonsters)
                {
                    SpawnMonster(normalMonsterPrefab , false);
                    normalMonsterPrefab.name = "�Ϲݸ���";
                    currentNormalMonsters++;
                }

                // ����Ʈ ���� ����
                if (currentEliteMonsters < maxEliteMonsters)
                {
                    if (dieMonsterCount > 5) 
                    { 
                        SpawnMonster(eliteMonsterPrefab , true);
                        eliteMonsterPrefab.name = "��������";
                        currentEliteMonsters++;
                        dieMonsterCount = 0;
                    }
                }
            }
        }
    }

    private void SpawnMonster(GameObject monsterPrefab, bool isElite)
    {
        // Monster ������ ����
        if (monsterFolder == null)
        {
            monsterFolder = new GameObject("Monster");
            folderTransform = monsterFolder.transform;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform playerTransform = player.transform;
        float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 playerPosition = player.transform.position;

        // �÷��̾� �ֺ��� ���� ���� ����
        float playerSpawnRadius = 5f;

        // ���� ��� ����
        Vector2 mapBoundaryMin = new Vector2(-21f, -18f);
        Vector2 mapBoundaryMax = new Vector2(24f, -2f);

        // �� ������ ������ ���� ��ġ�� ����
        float spawnX = UnityEngine.Random.Range(mapBoundaryMin.x, mapBoundaryMax.x);
        float spawnZ = UnityEngine.Random.Range(mapBoundaryMin.y, mapBoundaryMax.y);

        // �÷��̾���� �Ÿ��� Ȯ���Ͽ� ���� ���� ���� ������ �ٽ� ����
        while (Vector3.Distance(playerPosition, new Vector3(spawnX, 0f, spawnZ)) <= playerSpawnRadius)
        {
            spawnX = UnityEngine.Random.Range(mapBoundaryMin.x, mapBoundaryMax.x);
            spawnZ = UnityEngine.Random.Range(mapBoundaryMin.y, mapBoundaryMax.y);
        }

        Vector3 spawnPosition = new Vector3(spawnX, playerTransform.position.y, spawnZ);

        // ���� ����
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        monster.transform.SetParent(folderTransform);
        monster.transform.rotation = player.transform.rotation;

        MonsterInfo monsterInfo = new MonsterInfo(monster, monster.transform);
        CallMonsterSpawnEvent(monsterInfo);

        if (isElite)
        {
            MonsterHealth monsterHealth = monster.GetComponent<MonsterHealth>();
            if (monsterHealth != null)
            {
                monsterHealth.isEliteMonster = true;
            }
        }
    }

    private void CallMonsterSpawnEvent(MonsterInfo monsterInfo)
    {
        OnMonsterSpawn?.Invoke(monsterInfo);
    }

    public void MonsterDied()
    {
        currentNormalMonsters--; // ���� ��� �� ī��Ʈ ����
        dieMonsterCount++;
        if (dieMonsterCount < 0)
        {
            dieMonsterCount = 0;
        }
    }
    public void EliteMonsterDied()
    {
        currentEliteMonsters--; // ����Ʈ ���� ��� �� ī��Ʈ ����
        if (currentEliteMonsters < 0)
        {
            currentEliteMonsters = 0;
        }
    }
}
