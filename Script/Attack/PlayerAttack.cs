using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackPrefab;    // 공격 프리팹
    public GameObject hitEffectPrefab;      // 이펙트 프리팹

    [SerializeField] public float attackSpeed = 10f;    // 공격 속도

    private GameObject player;
    private GameObject targetMonster;  // 현재 공격할 몬스터
    
    private GameManager gameManager; // GameManager 객체

    public PlayerHealth playerHealth;

    private Coroutine attackCoroutine; // 현재 실행 중인 공격 코루틴


    void Start()
    {
        gameManager = GameManager.Instance;

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        StartAttackRoutine();
    }

    void StartAttackRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine); // 이전에 실행 중이던 공격 코루틴이 있다면 중지
        }
        attackCoroutine = StartCoroutine(AttackRoutine()); // 공격 루틴 시작 및 코루틴 참조 저장
    }


    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (playerHealth.playerAlive) {
                FindClosestMonster();
                StartCoroutine(SpawnAttack());

                yield return new WaitForSeconds(GameManager.Instance.playerAttackTime);
            }
            else
            {
                yield return null;
            }
        }
    }


    IEnumerator SpawnAttack()
    {
        GameObject currentTarget = targetMonster;  // 현재 타겟 몬스터를 기억

        if (currentTarget != null)
        {
            float offsetX = 1f;
            SoundManager.instance.PlaySfx(SoundManager.Sfx.Attack);
            Vector3 attackDirection = (currentTarget.transform.position - player.transform.position).normalized;
            Quaternion initialRotation = Quaternion.LookRotation(Camera.main.transform.forward); // 카메라의 방향을 기억


            Vector3 attackStartPosition = player.transform.position;

            if (attackDirection.x < 0)
                attackStartPosition = player.transform.position + attackDirection * -offsetX;
            else
            attackStartPosition = player.transform.position + attackDirection * offsetX;

            GameObject attack = Instantiate(attackPrefab, attackStartPosition, initialRotation); // 회전 방향 설정
            SpriteRenderer atSprite = attack.GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 가져오기

            
            // 몬스터를 추적하면서 공격 이동
            while (currentTarget != null && Vector3.Distance(attack.transform.position, currentTarget.transform.position) > 0.1f)
            {
                // 몬스터가 파괴되었는지 검사
                if (currentTarget == null)
                {
                    Destroy(attack); // 몬스터가 파괴되었으면 공격 제거
                    yield break; // 코루틴 종료
                }

                attackDirection = (currentTarget.transform.position - attack.transform.position).normalized;
                attack.transform.position += attackDirection * attackSpeed * Time.deltaTime;

                if (attackDirection.x > 0) // 오른쪽으로 날아갈 때
                {
                    atSprite.flipX = true;
                }
                else // 왼쪽으로 날아갈 때
                {
                    atSprite.flipX = false;
                }

                attack.transform.rotation = initialRotation; // 회전값을 카메라의 방향으로 고정


                yield return null;
            }

            if (currentTarget != null)
            {
                MonsterHealth monsterHealth = currentTarget.GetComponent<MonsterHealth>();  // 몬스터 체력 컴포넌트 가져오기
                int playerDamage = gameManager.playerDamage;

                GameObject hitEffect = Instantiate(hitEffectPrefab, currentTarget.transform.position, currentTarget.transform.rotation);
                Destroy(hitEffect, 0.5f);

                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(playerDamage);
                }
            }

            Destroy(attack);  // 몬스터에 도달한 후 공격 제거
        }
    }




    void FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(player.transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetMonster = monster;
            }
        }
    }
}