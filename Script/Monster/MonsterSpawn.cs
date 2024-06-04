using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterInfo
{
    public GameObject monsterObject; // 몬스터 객체
    public Transform monsterTransform; // 몬스터의 Transform 정보

    public MonsterInfo(GameObject monster, Transform transform)
    {
        monsterObject = monster;
        monsterTransform = transform;
    }
}



public class MonsterSpawn : MonoBehaviour
{
    public GameObject normalMonsterPrefab;    // 일반 몬스터 프리팹
    public GameObject eliteMonsterPrefab;     // 엘리트 몬스터 프리팹

    [SerializeField] public int maxNormalMonsters = 5;         // 생성할 일반 몬스터 수
    [SerializeField] public int maxEliteMonsters = 1;          // 생성할 엘리트 몬스터 수

    private int currentNormalMonsters = 0;    // 생성된 일반 몬스터 수
    private int currentEliteMonsters = 0;     // 생성된 엘리트 몬스터 수

    private int dieMonsterCount;

    [SerializeField] private float minSpawnDistance = 10f;      // 최소 스폰 거리
    [SerializeField] private float maxSpawnDistance = 15f;      // 최대 스폰 거리


    private GameObject monsterFolder;
    private Transform folderTransform;

    public static event Action<MonsterInfo> OnMonsterSpawn; // 몬스터 생성 이벤트

    private void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 몬스터 생성 속도

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 일반 몬스터 스폰
                if (currentNormalMonsters < maxNormalMonsters)
                {
                    SpawnMonster(normalMonsterPrefab , false);
                    normalMonsterPrefab.name = "일반몬스터";
                    currentNormalMonsters++;
                }

                // 엘리트 몬스터 스폰
                if (currentEliteMonsters < maxEliteMonsters)
                {
                    if (dieMonsterCount > 5) 
                    { 
                        SpawnMonster(eliteMonsterPrefab , true);
                        eliteMonsterPrefab.name = "정예몬스터";
                        currentEliteMonsters++;
                        dieMonsterCount = 0;
                    }
                }
            }
        }
    }

    private void SpawnMonster(GameObject monsterPrefab, bool isElite)
    {
        // Monster 폴더를 생성
        if (monsterFolder == null)
        {
            monsterFolder = new GameObject("Monster");
            folderTransform = monsterFolder.transform;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform playerTransform = player.transform;
        float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);

        Vector3 playerPosition = player.transform.position;

        // 플레이어 주변의 일정 범위 정의
        float playerSpawnRadius = 5f;

        // 맵의 경계 정의
        Vector2 mapBoundaryMin = new Vector2(-21f, -18f);
        Vector2 mapBoundaryMax = new Vector2(24f, -2f);

        // 맵 내에서 몬스터의 스폰 위치를 결정
        float spawnX = UnityEngine.Random.Range(mapBoundaryMin.x, mapBoundaryMax.x);
        float spawnZ = UnityEngine.Random.Range(mapBoundaryMin.y, mapBoundaryMax.y);

        // 플레이어와의 거리를 확인하여 일정 범위 내에 있으면 다시 결정
        while (Vector3.Distance(playerPosition, new Vector3(spawnX, 0f, spawnZ)) <= playerSpawnRadius)
        {
            spawnX = UnityEngine.Random.Range(mapBoundaryMin.x, mapBoundaryMax.x);
            spawnZ = UnityEngine.Random.Range(mapBoundaryMin.y, mapBoundaryMax.y);
        }

        Vector3 spawnPosition = new Vector3(spawnX, playerTransform.position.y, spawnZ);

        // 몬스터 스폰
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
        currentNormalMonsters--; // 몬스터 사망 시 카운트 감소
        dieMonsterCount++;
        if (dieMonsterCount < 0)
        {
            dieMonsterCount = 0;
        }
    }
    public void EliteMonsterDied()
    {
        currentEliteMonsters--; // 엘리트 몬스터 사망 시 카운트 감소
        if (currentEliteMonsters < 0)
        {
            currentEliteMonsters = 0;
        }
    }
}
