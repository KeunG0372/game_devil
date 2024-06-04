using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{

    /*
    IEnumerator SpawnSkill()
    {
        // 플레이어 주변에 스킬 장판을 생성하는 함수
        Vector3 skillIndicatorPosition = player.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hit))
        {
            Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // 위로 조금 올리기
            skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
        }

        yield return new WaitForSeconds(skillIndicatorDuration);

        Destroy(skillIndicator); // 스킬 장판 삭제

        // 장판이 사라진 후 주변에 있는 몬스터를 타겟팅하여 스킬 발동
        FindMonstersInArea();

        if (targetMonster != null)
        {
            // 몬스터에게 스킬을 발동하는 함수
            Vector3 skillPosition = player.transform.position;

            RaycastHit hitSkill;
            if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hitSkill))
            {
                Vector3 spawnSkillArea = hitSkill.point;
                GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);

                // 몬스터에게 피해를 입히고 스킬 삭제
                MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
                playerHealth = player.GetComponent<PlayerHealth>();    //데미지 컴포넌트 가져오기
                playerSkillDamage = playerHealth.playerDamage;

                Collider targetCollider = targetMonster.GetComponent<Collider>();
                monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

                yield return new WaitForSeconds(skillDuration);
                Destroy(skill);// 일정 시간 후에 스킬 삭제
            }
        }
        else
        {
            Debug.Log("몬스터를 찾지 못함");
            // 몬스터를 찾지 못했으므로 플레이어에게 스킬 장판을 생성
            Vector3 spawnSkillArea = player.transform.position;
            GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);

            // 플레이어에게 스킬 효과를 적용

            yield return new WaitForSeconds(skillDuration);
            Destroy(skill); // 일정 시간 후에 스킬 삭제
        }
    }



    void AttackMonster()
    {
        // 몬스터에게 스킬을 발동하는 함수
        Vector3 attackDirection = (targetMonster.transform.position - player.transform.position).normalized;
        Quaternion initialRotation = Quaternion.LookRotation(attackDirection);
        GameObject skill = Instantiate(skillPrefab, targetMonster.transform.position, initialRotation);

        // 몬스터에게 피해를 입히고 스킬 삭제
        MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();    //데미지 컴포넌트 가져오기
        playerSkillDamage = playerHealth.playerDamage;

        Collider targetCollider = targetMonster.GetComponent<Collider>();
        monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

        Destroy(skill);
    }



    void FindMonstersInArea()
    {

        
        // 주변에 있는 몬스터를 찾는 함수
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, attackDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                targetMonster = collider.gameObject;
                return; // 가장 가까운 몬스터를 찾으면 바로 종료
            }
        }
        targetMonster = null; // 주변에 몬스터가 없으면 null로 설정





    void FindMonstersInArea(GameObject player, GameObject[] monsters)
    {

        float closestDistance = Mathf.Infinity;
        targetMonster = null; // 초기화

        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(player.transform.position, monster.transform.position);
            if (distance <= attackDistance) // 일정 거리 내의 몬스터만 고려
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetMonster = monster;
                }
            }
        }
    }














     // 몬스터에게 피해를 입히고 스킬 삭제
                MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
                playerHealth = player.GetComponent<PlayerHealth>();    //데미지 컴포넌트 가져오기
                playerSkillDamage = playerHealth.playerDamage;

                Collider targetCollider = targetMonster.GetComponent<Collider>();
                monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

                







    public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100; // 몬스터의 기본 체력
    public int currentHealth; // 현재 체력

    public int monsterDamage = 10;

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;

    public event Action<Collider> OnMonsterDeath; // 몬스터가 죽었을 때 발생하는 이벤트




    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 기본 체력으로 초기화
        monsterSpawn = FindObjectOfType<MonsterSpawn>();
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
    public void TakeDamage(int damage, Collider targetMonster)
    {
        // 현재 체력에서 데미지만큼 감소
        currentHealth -= damage;

        // 체력이 0 이하로 떨어지면
        if (currentHealth <= 0)
        {
            Die(targetMonster); // 몬스터 사망 처리 함수 호출
        }
    }

    // 몬스터가 죽었을 때 실행할 함수
    void Die(Collider monsterCollider)
    {

        OnMonsterDeath?.Invoke(monsterCollider); // 몬스터가 Destroy될 때 이벤트 호출하여 플레이어에서 해당 몬스터 제거

        //Debug.Log("ondestroy 호출");

        monsterSpawn.MonsterDied();
        Destroy(gameObject);
    }

    public void SetTargetMonster(GameObject target)
    {
        // 몬스터 체력 클래스에 타겟 몬스터 설정
        targetMonster = target;
    }
}





    public class PlayerSkill : MonoBehaviour
{
    public GameObject skillPrefab;    // 스킬 프리팹
    public GameObject skillIndicatorPrefab; // 스킬 장판 프리팹
    public float attackDistance = 20f; // 스킬 사거리
    public float skillDuration = 1f;   // 장판 유지 시간
    public float skillIndicatorDuration = 1f;   // 장판 유지 시간


    PlayerHealth playerHealth;
    int plSkillDamage; // 스킬 데미지

    private GameObject player;
    private GameObject skill;   // 스킬
    private GameObject skillIndicator; // 스킬 장판
    private GameObject targetMonster;  // 현재 공격할 몬스터


    public void UseSkill()
    {
        StartCoroutine(SpawnSkill(skills));
    }

    IEnumerator SpawnSkill(GameObject[] skills)
    {
        player = GameObject.FindGameObjectWithTag("Player");

        targetMonster = FindMonstersInArea(player);



        if (targetMonster != null)
        {
            Vector3 skillIndicatorPosition = targetMonster.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hit))
            {
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // 위로 조금 올리기
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(skillIndicatorDuration); // 일정 시간 후에 장판 삭제

            Destroy(skillIndicator); // 스킬 장판 삭제


            // 스킬을 발동하는 함수
            skill = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);

            float skillColliderRadius = skill.GetComponent<CapsuleCollider>().radius;
            Collider[] hitColliders = Physics.OverlapSphere(skill.transform.position, skillColliderRadius);
            if (targetMonster != null)
            {
                foreach (Collider collider in hitColliders)
                {
                    try
                    {
                        if (collider.CompareTag("Monster") && targetMonster != null)
                        {

                            MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();  // 몬스터 체력 컴포넌트 가져오기
                            playerHealth = player.GetComponent<PlayerHealth>();    //데미지 컴포넌트 가져오기
                            plSkillDamage = playerHealth.playerDamage;
                            if (monsterHealth != null)
                            {

                                monsterHealth.TakeDamage(plSkillDamage);
                            }
                        }
                    }
                    catch(NullReferenceException ex) { }

                }

                yield return new WaitForSeconds(skillDuration);
                Destroy(skill);     // 일정 시간 후에 스킬 삭제


            }
            else
            {
                Debug.Log("몬스터를 찾지 못함");

                // 몬스터를 찾지 못했으므로 플레이어에게 스킬 장판을 생성
                Vector3 spawnSkillArea = player.transform.position;
                GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);   // 플레이어에게 스킬 효과를 적용


                yield return new WaitForSeconds(skillDuration);
                Destroy(skill); // 일정 시간 후에 스킬 삭제

                //GameObject[] skills = GameObject.FindGameObjectsWithTag("Skill");   //에러 방지 초기화 작업
                //foreach (GameObject skill in skills)
                //{
                //    Destroy(skill);
                //}
            }
        }
    }

    private GameObject FindMonstersInArea(GameObject player)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closestMonster = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = player.transform.position;

        foreach (GameObject monster in monsters)
        {
            Vector3 monsterPosition = monster.transform.position;
            float distance = (monsterPosition - playerPosition).sqrMagnitude;

            // x와 z 좌표가 20 이내에 있는 몬스터만 고려
            if (Mathf.Abs(monsterPosition.x - playerPosition.x) <= attackDistance &&
                Mathf.Abs(monsterPosition.z - playerPosition.z) <= attackDistance)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMonster = monster;
                }
            }
        }

        return closestMonster;
    }
}





















    public GameObject monsterPrefab;    // 몬스터 프리팹
    public GameObject eliteMonsterPrefab;   //정예몹 프리팹

    public int maxMonster = 5;        // 생성할 몬스터 수
    private int currentMonster = 0;     //생성된 몬스터 수
    public float minSpawnDistance = 10f; // 최소 스폰 거리
    public float maxSpawnDistance = 15f; // 최대 스폰 거리

    public static event Action<MonsterInfo> OnMonsterSpawn;     //몬스터 생성 이벤트
    //public event Action<Monster> OnMonsterDied; 

    private int dieMonsterCount;   //엘리트 몬스터를 위한 몬스터 카운트

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

    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        GameObject monsterFolder = new GameObject("Monster");   // 몬스터 폴더 생성
        Transform folderTransform = monsterFolder.transform;    // 몬스터 폴더 위치지정

        Transform playerTransform; // 플레이어의 Transform

        while (true) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");  //플레이어 찾기
            if (player != null ) {
                if (currentMonster < maxMonster)
                {
                    playerTransform = player.transform;

                    float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);


                    Vector3 spawnDirection = UnityEngine.Random.insideUnitSphere.normalized; // 무작위 방향 벡터
                    spawnDirection.y = 0;
                    Vector3 spawnPosition = player.transform.position + spawnDirection * spawnDistance;     // 몬스터 거리지정


                    GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);    //몬스터 생성 프리팹, 위치 설정

                    monster.transform.SetParent(folderTransform);   //몬스터 부모폴더 지정

                    currentMonster++;

                    monster.transform.rotation = playerTransform.rotation;  //몬스터의 rotation을 플레이어의 rotation으로 설정


                    MonsterInfo monsterInfo = new MonsterInfo(monster, monster.transform);  //몬스터 정보등록
                    CallMonsterSpawnEvent(monsterInfo);                                     //이벤트 등록


                    //엘리트 몬스터 추가
                    if (dieMonsterCount > 5 )                
                    {
                        SpawnEliteMonster(folderTransform, player, spawnPosition);
                        dieMonsterCount = 0;
                    }
                }
            }
            yield return new WaitForSeconds(1f);    //몬스터 생성 속도
        } // while 종료
    }

    public void MonsterDied()
    {
        //OnMonsterDied?.Invoke(monster);
        currentMonster--; // 몬스터 사망 시 카운트 감소

        dieMonsterCount++;
    }

    private void CallMonsterSpawnEvent(MonsterInfo monsterInfo)
    {
        OnMonsterSpawn?.Invoke(monsterInfo); // 몬스터 정보 전달
    }

    void SpawnEliteMonster(Transform folderTransform, GameObject player ,Vector3 spawnPosition)
    {
        Transform playerTransform = player.transform;

        GameObject eliteMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);    //엘리트 몬스터 생성 프리팹, 위치 설정
        eliteMonster.name = "엘리트몬스터";
        eliteMonster.transform.SetParent(folderTransform);   //몬스터 부모폴더 지정
        eliteMonster.transform.rotation = playerTransform.rotation;  //몬스터의 rotation을 플레이어의 rotation으로 설정

        MonsterHealth monsterHealth = eliteMonster.GetComponent<MonsterHealth>();
        monsterHealth.maxHealth = Mathf.RoundToInt(monsterHealth.maxHealth * 1.5f); // 일반 몬스터의 체력의 1.5배로 설정
        monsterHealth.currentHealth = monsterHealth.maxHealth; // 현재 체력을 최대 체력으로 설정

        MonsterInfo eliteMonsterInfo = new MonsterInfo(eliteMonster, eliteMonster.transform);   //몬스터 정보등록
        CallMonsterSpawnEvent(eliteMonsterInfo);                                                //이벤트 등록
    }



    게임매니져부분

    private void Awake()
    {
        Instance = this; // 싱글톤 인스턴스 설정

        MonsterSpawn.Instance.OnMonsterSpawn += HandleMonsterSpawn;
    }

    void HandleMonsterSpawn(MonsterSpawn.MonsterInfo monsterInfo)
    {
        MonsterHealth monsterHealth = monsterInfo.monsterObject.GetComponent<MonsterHealth>();
        if (monsterHealth != null)
        {
            monsterHealth.OnMonsterDrop += HandleMonsterDrop;
        }
    }











    public class PlayerSkill : MonoBehaviour
{
    public bool isUsingSkill = false;
    public GameObject skillPrefab;    // 스킬 프리팹
    public GameObject skillIndicatorPrefab; // 스킬 장판 프리팹
    public float attackDistance = 10f; // 스킬 사거리
    public float skillDuration = 1f;   // 장판 유지 시간
    public float skillIndicatorDuration = 1f;   // 장판 유지 시간

    private GameObject player;
    private GameObject skillIndicator; // 스킬 장판
    private GameObject[] monsters;  // 현재 존재하는 모든 몬스터 배열

    private Transform playerTransform;

    private GameManager gameManager; // GameManager 객체
    private PlayerController playerController; // PlayerController 스크립트 참조

    public Animation animationComponent;
    public string animationName = "FireSkill";

    void Start()
    {
        gameManager = GameManager.Instance;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        animationComponent = GetComponent<Animation>();
    }

    public void UseSkill()
    {
        StartCoroutine(SpawnFireSkill());
    }

    IEnumerator SpawnFireSkill()
    {
        isUsingSkill = true;
        player = GameObject.FindGameObjectWithTag("Player");
        monsters = GameObject.FindGameObjectsWithTag("Monster");

        GameObject targetMonster = FindClosestMonster(player);
        playerTransform = player.transform;
        Vector3 playerRotationEuler = playerTransform.rotation.eulerAngles;

        if (targetMonster != null)
        {
            Vector3 skillIndicatorPosition = targetMonster.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hit))
            {
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 1f, 0); // 위로 조금 올리기
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
                skillIndicator.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);

            }

            yield return new WaitForSeconds(skillIndicatorDuration); // 일정 시간 후에 장판 삭제

            Destroy(skillIndicator); // 스킬 장판 삭제

            // 스킬을 발동하는 함수
            GameObject skillInstance = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);

            skillInstance.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);



            // 스킬 콜라이더 내에 있는 몬스터들에게 데미지를 주는 코드

            float skillColliderRadius = skillInstance.GetComponent<CapsuleCollider>().radius;
            Collider[] hitColliders = Physics.OverlapSphere(skillInstance.transform.position, skillColliderRadius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Monster"))
                {
                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    if (monsterHealth != null)
                    {
                        int fireDamage = gameManager.fireDamage;
                        monsterHealth.TakeDamage(fireDamage);
                    }
                }
            }

            yield return new WaitForSeconds(skillDuration);
            Destroy(skillInstance); // 일정 시간 후에 스킬 삭제
        }
        else
        {
            Vector3 skillIndicatorPosition;

            if (playerController.isMovingLeft)
            {
                skillIndicatorPosition = player.transform.position + Vector3.left * 5f;
            }
            else
            {
                skillIndicatorPosition = player.transform.position + Vector3.right * 5f;
            }

            RaycastHit hit;
            if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hit))
            {
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // 위로 조금 올리기
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
                skillIndicator.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);
            }

            yield return new WaitForSeconds(skillIndicatorDuration); // 일정 시간 후에 장판 삭제

            Destroy(skillIndicator); // 스킬 장판 삭제

            // 스킬을 발동하는 함수
            GameObject skillInstance = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);

            skillInstance.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);
            


            // 스킬 콜라이더 내에 있는 몬스터들에게 데미지를 주는 코드

            float skillColliderRadius = skillInstance.GetComponent<CapsuleCollider>().radius;
            Collider[] hitColliders = Physics.OverlapSphere(skillInstance.transform.position, skillColliderRadius);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Monster"))
                {
                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    if (monsterHealth != null)
                    {
                        int fireDamage = gameManager.fireDamage;
                        monsterHealth.TakeDamage(fireDamage);
                    }
                }
            }

            yield return new WaitForSeconds(skillDuration);
            Destroy(skillInstance); // 일정 시간 후에 스킬 삭제

            isUsingSkill = false;
        }
    }

    private GameObject FindClosestMonster(GameObject player)
    {
        GameObject closestMonster = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = player.transform.position;

        foreach (GameObject monster in monsters)
        {
            Vector3 monsterPosition = monster.transform.position;
            float distance = Vector3.Distance(playerPosition, monsterPosition);
            if (distance <= attackDistance && distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monster;
            }
        }

        return closestMonster;
    }
}
    



    public Transform player; // 플레이어의 Transform
    public float distance = 0f; // 카메라와 플레이어 사이의 거리
    public float height = 10f; // 카메라의 높이

    private Vector3 offset; // 초기 거리 설정

    void Start()
    {
        // 초기 거리 설정
        offset = new Vector3(0f, height, -distance); 
    }

    void LateUpdate()
    {
        // 플레이어의 위치에 따라 카메라 위치 업데이트
        transform.position = player.position + offset;

        // 플레이어를 바라보도록 회전
        transform.LookAt(player);
    }


    GameObject player = GameObject.FindGameObjectWithTag("Player");

        Transform playerTransform = player.transform;

        float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);


        Vector3 spawnDirection = UnityEngine.Random.insideUnitSphere.normalized; // 무작위 방향 벡터
        spawnDirection.y = 0;
        Vector3 spawnPosition = player.transform.position + spawnDirection * spawnDistance;     // 몬스터 거리지정

        //Vector3 spawnPosition = player.transform.position + UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);




    if (!playerSkill.isUsingSkill && !fireIsCooldown && !isSkillUse)


    if (!playerSkill.isUsingSkill && !iceIsCooldown && !isSkillUse)




            /*
            if (!isAttacking)  // 현재 공격 중이 아니면
            {
                FindClosestMonster();
                StartCoroutine(SpawnAttack());  // 공격을 생성하고 완료될 때까지 기다림
            }
            yield return new WaitForSeconds(attackTime);  // 1초 후에 다시 시도


    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // 몬스터를 접촉 리스트에서 제거
    }


    IEnumerator EventCoroutine()
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

    

    public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int playerDamage = 100;
    [SerializeField] public int playerSkillDamage = 100;

    [SerializeField] public int maxHealth = 300;
    [SerializeField] public int currentHealth;
    [SerializeField] private int damagePerMonster = 10; // 몬스터당 입는 데미지


    public List<Collider> monstersInContact = new List<Collider>(); // 접촉한 몬스터 리스트

    [SerializeField] private Slider hpBar;

    private EffectSprite effectSprite;

    [SerializeField] int totalDamage;

    void Start()
    {
        effectSprite = GetComponent<EffectSprite>();
        currentHealth = maxHealth;
        SetMaxHealth(maxHealth);
        StartCoroutine(ApplyDamageOverTime());
        StartCoroutine(EventCoroutine());
    }

    private void SetMaxHealth(int health)
    {
        hpBar.maxValue = health;
        hpBar.value = health;
    }


    // 데미지
    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            Debug.Log(monstersInContact.Count);
            totalDamage = monstersInContact.Count * damagePerMonster; // 총 데미지 계산
            TakeDamage(totalDamage); // 데미지 적용
            if (totalDamage > 0) { effectSprite.ApplyEffect(); }    // 데미지가 있으면 이펙트 적용
            monstersInContact.Clear(); // 몬스터 리스트 비우기
            yield return new WaitForSeconds(1f);
        }
    }


    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hpBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            hpBar.value = 0;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
        // 플레이어 사망 처리
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
            monstersInContact.Remove(other); // 해당 몬스터만 리스트에서 제거
            if (monstersInContact.Count == 0)
            {
                StopCoroutine(ApplyDamageOverTime()); // 모든 몬스터와의 접촉이 끊겼을 때만 코루틴을 멈춤
            }
        }
    }

    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // 몬스터를 접촉 리스트에서 제거
    }

    IEnumerator EventCoroutine()
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




public class MonsterHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100; // 몬스터의 기본 체력
    [SerializeField] public int currentHealth; // 현재 체력
    [SerializeField] public int monsterDamage = 10;
    [SerializeField] public int giveExp = 50;  // 플레이어 경험치 증가량
    [SerializeField] public bool isEliteMonster;
    [SerializeField] public bool isDamageCooldown = false;
    [SerializeField] private float damageCooldownDuration = 0.5f; // 0.5초 동안 움직임을 멈추도록 설정

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;
    private GameObject player;
    private EffectSprite effectSprite;


    public event Action<Collider> OnMonsterDeath; // 몬스터가 죽었을 때 발생하는 이벤트
    public event Action<MonsterHealth> OnMonsterDrop;
    public static event Action<int> OnMonsterExp;

    

    public GameObject goldPrefab;

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 기본 체력으로 초기화
        monsterSpawn = FindObjectOfType<MonsterSpawn>();
        effectSprite = GetComponent<EffectSprite>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        effectSprite.ApplyEffect();

        if (currentHealth <= 0)
        {
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

        OnMonsterDeath?.Invoke(GetComponent<Collider>()); // 몬스터 제거
        OnMonsterDrop?.Invoke(this);
        OnMonsterExp?.Invoke(giveExp);


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

        /*
    public void UpgradeAttackDamage()
    {
        // GameManager의 강화 함수 호출
        bool upgradeSuccess = gameManager.UpgradeAttackDamage();
    }

    public void UpgradeFireDamage()
    {
        bool upgradeSuccess = gameManager.UpgradeFireDamage();
    }

    public void UpgradeGold()
    {
        bool upgradeSuccess = gameManager.UpgradeAddGold();
    }

    public void UpgradeJewel()
    {
        bool upgradeSuccess = gameManager.UpgradeAddJewel();
    }

    public void UpgradeIceDamage()
    {
        bool upgradeSuccess = gameManager.UpgradeIceDamage();
    }

    public void UpgradePlMoveSpeed()
    {
        bool upgradeSuccess = gameManager.UpgradePlMoveSpeed();
    }

    public void UpgradeExp()
    {
        bool upgradeSuccess = gameManager.UpgradeExpIncrease();
    }

    public void UpgradeAtkSoeed()
    {
        bool upgradeSuccess = gameManager.UpgradeAtkSpeed();
    }

    public void UpgradePlhe()
    {
        bool upgradeSuccess = gameManager.UpgradePlHpIncrease();
    }






    // 업그레이드 ---------------------------------------------------------------------------------------------

    public bool UpgradeAddGold()
    {
        int upgradeCost = CalculateGoldUpgradeCost(baseGoldUpgradeCost);      // 업그레이드 비용 계산

        if (gold >= upgradeCost)        // 플레이어의 골드가 충분한지 확인
        {
            baseGoldUpgradeCost = upgradeCost;
            msGold += goldIncrease;    
            gold -= upgradeCost;    // 골드 소비
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("획득 골드량 업그레이드 완료. 현재 획득량: " + msGold);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeAddJewel()
    {
        int upgradeCost = CalculateJewelUpgradeCost(baseJewelUpgradeCost);      // 업그레이드 비용 계산

        if (gold >= upgradeCost)        // 플레이어의 골드가 충분한지 확인
        {
            baseJewelUpgradeCost = upgradeCost;
            msJewel += goldIncrease;     
            gold -= upgradeCost;    // 골드 소비
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("획득 골드량 업그레이드 완료. 현재 획득량: " + msGold);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeAttackDamage()
    {
        int upgradeCost = CalculateAttackUpgradeCost(baseAttackUpgradeCost);      // 업그레이드 비용 계산

        if (gold >= upgradeCost)        // 플레이어의 골드가 충분한지 확인
        {
            baseAttackUpgradeCost = upgradeCost;
            playerDamage += damageIncrease;     // 강화된 공격력 증가
            gold -= upgradeCost;    // 골드 소비
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + playerDamage);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeFireDamage()
    {
        int upgradeCost = CalculateFireUpgradeCost(baseFireUpgradeCost);      // 업그레이드 비용 계산

        if (gold >= upgradeCost)        // 플레이어의 골드가 충분한지 확인
        {
            baseFireUpgradeCost = upgradeCost;
            fireDamage += damageIncrease;     // 강화된 공격력 증가
            gold -= upgradeCost;    // 골드 소비
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + fireDamage);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeIceDamage()
    {
        int upgradeCost = CalculateIceUpgradeCost(baseIceUpgradeCost);      // 업그레이드 비용 계산

        if (gold >= upgradeCost)        // 플레이어의 골드가 충분한지 확인
        {
            baseIceUpgradeCost = upgradeCost;
            iceDamage += damageIncrease;     // 강화된 공격력 증가
            gold -= upgradeCost;    // 골드 소비
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("공격력 업그레이드 완료. 현재 강화된 공격력: " + iceDamage);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradePlMoveSpeed()
    {
        int upgradeCost = CalculatePlMoveUpgradeCost(basePlMoveUpgradeCost); 

        if (jewel >= upgradeCost) 
        {
            basePlMoveUpgradeCost = upgradeCost;
            playerController.moveSpeed += 0.3f;
            jewel -= upgradeCost; 
            Debug.Log("업그레이드에 사용한 골드: " + upgradeCost);
            Debug.Log("이동속도 업그레이드 완료. 현재 이동속도: " + playerController.moveSpeed);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeExpIncrease()
    {
        int upgradeCost = CalculateExpUpgradeCost(baseExpUpgradeCost); 

        if (jewel >= upgradeCost) 
        {
            baseExpUpgradeCost = upgradeCost;
            msExp += expIncrease;
            jewel -= upgradeCost;    // 보석 소비
            Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
            Debug.Log("경험치 획득량 업그레이드 완료. 현재 경험치 획득량 : " + msExp);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeAtkSpeed()
    {
        int upgradeCost = CalculateAtkSpeedUpgradeCost(baseAtkSpeedUpgradeCost);

        if (jewel >= upgradeCost)
        {
            baseAtkSpeedUpgradeCost = upgradeCost;
            playerAttack.attackTime -= 0.05f;
            jewel -= upgradeCost;
            Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
            Debug.Log("공격 속도 업그레이드 완료. 현재 공격 속도 : " + playerAttack.attackTime);
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다. 업그레이드에 필요한 보석: " + upgradeCost);
            return false;
        }
    }
    public bool UpgradePlHpIncrease()
    {
        int upgradeCost = CalculatePlHpUpgradeCost(basePlHpUpgradeCost);

        if (jewel >= upgradeCost) 
        {
            basePlHpUpgradeCost = upgradeCost;
            playerHealth.recoveryHealth += 10;
            jewel -= upgradeCost;  
            Debug.Log("업그레이드에 사용한 보석: " + upgradeCost);
            Debug.Log("체력 회복 업그레이드 완료. 현재 체력 회복량 : " + playerHealth.recoveryHealth);
            return true;
        }
        else
        {
            Debug.Log("보석이 부족합니다. 업그레이드에 필요한 골드: " + upgradeCost);
            return false;
        }
    }

    // 업그레이드 비용 계산 ----------------------------------------------------

    private int CalculateGoldUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateJewelUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateAttackUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateFireUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateIceUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculatePlMoveUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateExpUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculateAtkSpeedUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }

    private int CalculatePlHpUpgradeCost(int currentUpgradeCost)
    {
        return (int)(currentUpgradeCost * multipIncrease);
    }



    public void OnUpgradeButtonClick()
    {
        bool upgradeSuccess = false;
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Setting2);
        // 마지막으로 눌린 버튼의 타입에 따라 강화 수행
        switch (upgradeType)
        {
            case "attack":
                upgradeSuccess = gameManager.UpgradeAttackDamage();
                uiText.text = "기본공격 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.playerDamage + "\n강화 후 공격력 : " + (gameManager.playerDamage + gameManager.damageIncrease);
                uiText.text += "\n 필요 골드 : " + 
                break;
            case "fire":
                upgradeSuccess = gameManager.UpgradeFireDamage();
                uiText.text = "소드 스킬 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.fireDamage + "\n강화 후 공격력 : " + (gameManager.fireDamage + gameManager.damageIncrease);
                break;
            case "gold":
                upgradeSuccess = gameManager.UpgradeAddGold();
                uiText.text = "골드 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msGold + "\n강화 후 획득량 : " + (gameManager.msGold + gameManager.goldIncrease);
                break;
            case "jewel":
                upgradeSuccess = gameManager.UpgradeAddJewel();
                uiText.text = "보석 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msJewel + "\n강화 후 획득량 : " + (gameManager.msJewel + gameManager.goldIncrease);
                break;
            case "ice":
                upgradeSuccess = gameManager.UpgradeIceDamage();
                uiText.text = "아이스 스킬 강화";
                uiText.text += "\n\n현재 공격력 : " + gameManager.iceDamage + "\n강화 후 공격력 : " + (gameManager.iceDamage + gameManager.damageIncrease);
                break;
            case "moveSpeed":
                upgradeSuccess = gameManager.UpgradePlMoveSpeed();
                uiText.text = "이동속도 강화";
                uiText.text += "\n\n현재 이동속도 : " + playerController.moveSpeed + "\n강화 후 이동속도 : " + (playerController.moveSpeed + 0.3f).ToString("F1");
                break;
            case "exp":
                upgradeSuccess = gameManager.UpgradeExpIncrease();
                uiText.text = "경험치 획득량 강화";
                uiText.text += "\n\n현재 획득량 : " + gameManager.msExp + "\n강화 후 획득량 : " + (gameManager.msExp + gameManager.expIncrease);
                break;
            case "attackSpeed":
                upgradeSuccess = gameManager.UpgradeAtkSpeed();
                uiText.text = "공격속도 강화";
                uiText.text += "\n\n현재 공격속도 : " + playerAttack.attackTime + "\n강화 후 공격속도 : " + (playerAttack.attackTime - 0.05f).ToString("F2");
                break;
            case "health":
                upgradeSuccess = gameManager.UpgradePlHpIncrease();
                uiText.text = "체력 회복량 강화";
                uiText.text += "\n\n현재 회복량 : " + playerHealth.recoveryHealth + "\n강화 후 획득량 : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                break;
        }
        SetCharaText();
    }
    */




}
