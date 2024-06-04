using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{

    /*
    IEnumerator SpawnSkill()
    {
        // �÷��̾� �ֺ��� ��ų ������ �����ϴ� �Լ�
        Vector3 skillIndicatorPosition = player.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hit))
        {
            Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // ���� ���� �ø���
            skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
        }

        yield return new WaitForSeconds(skillIndicatorDuration);

        Destroy(skillIndicator); // ��ų ���� ����

        // ������ ����� �� �ֺ��� �ִ� ���͸� Ÿ�����Ͽ� ��ų �ߵ�
        FindMonstersInArea();

        if (targetMonster != null)
        {
            // ���Ϳ��� ��ų�� �ߵ��ϴ� �Լ�
            Vector3 skillPosition = player.transform.position;

            RaycastHit hitSkill;
            if (Physics.Raycast(skillIndicatorPosition, Vector3.down, out hitSkill))
            {
                Vector3 spawnSkillArea = hitSkill.point;
                GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);

                // ���Ϳ��� ���ظ� ������ ��ų ����
                MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
                playerHealth = player.GetComponent<PlayerHealth>();    //������ ������Ʈ ��������
                playerSkillDamage = playerHealth.playerDamage;

                Collider targetCollider = targetMonster.GetComponent<Collider>();
                monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

                yield return new WaitForSeconds(skillDuration);
                Destroy(skill);// ���� �ð� �Ŀ� ��ų ����
            }
        }
        else
        {
            Debug.Log("���͸� ã�� ����");
            // ���͸� ã�� �������Ƿ� �÷��̾�� ��ų ������ ����
            Vector3 spawnSkillArea = player.transform.position;
            GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);

            // �÷��̾�� ��ų ȿ���� ����

            yield return new WaitForSeconds(skillDuration);
            Destroy(skill); // ���� �ð� �Ŀ� ��ų ����
        }
    }



    void AttackMonster()
    {
        // ���Ϳ��� ��ų�� �ߵ��ϴ� �Լ�
        Vector3 attackDirection = (targetMonster.transform.position - player.transform.position).normalized;
        Quaternion initialRotation = Quaternion.LookRotation(attackDirection);
        GameObject skill = Instantiate(skillPrefab, targetMonster.transform.position, initialRotation);

        // ���Ϳ��� ���ظ� ������ ��ų ����
        MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();    //������ ������Ʈ ��������
        playerSkillDamage = playerHealth.playerDamage;

        Collider targetCollider = targetMonster.GetComponent<Collider>();
        monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

        Destroy(skill);
    }



    void FindMonstersInArea()
    {

        
        // �ֺ��� �ִ� ���͸� ã�� �Լ�
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, attackDistance);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                targetMonster = collider.gameObject;
                return; // ���� ����� ���͸� ã���� �ٷ� ����
            }
        }
        targetMonster = null; // �ֺ��� ���Ͱ� ������ null�� ����





    void FindMonstersInArea(GameObject player, GameObject[] monsters)
    {

        float closestDistance = Mathf.Infinity;
        targetMonster = null; // �ʱ�ȭ

        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(player.transform.position, monster.transform.position);
            if (distance <= attackDistance) // ���� �Ÿ� ���� ���͸� ���
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetMonster = monster;
                }
            }
        }
    }














     // ���Ϳ��� ���ظ� ������ ��ų ����
                MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();
                playerHealth = player.GetComponent<PlayerHealth>();    //������ ������Ʈ ��������
                playerSkillDamage = playerHealth.playerDamage;

                Collider targetCollider = targetMonster.GetComponent<Collider>();
                monsterHealth.TakeDamage(playerSkillDamage, targetCollider);

                







    public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 100; // ������ �⺻ ü��
    public int currentHealth; // ���� ü��

    public int monsterDamage = 10;

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;

    public event Action<Collider> OnMonsterDeath; // ���Ͱ� �׾��� �� �߻��ϴ� �̺�Ʈ




    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �⺻ ü������ �ʱ�ȭ
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


    // �������� �޴� �Լ�
    public void TakeDamage(int damage, Collider targetMonster)
    {
        // ���� ü�¿��� ��������ŭ ����
        currentHealth -= damage;

        // ü���� 0 ���Ϸ� ��������
        if (currentHealth <= 0)
        {
            Die(targetMonster); // ���� ��� ó�� �Լ� ȣ��
        }
    }

    // ���Ͱ� �׾��� �� ������ �Լ�
    void Die(Collider monsterCollider)
    {

        OnMonsterDeath?.Invoke(monsterCollider); // ���Ͱ� Destroy�� �� �̺�Ʈ ȣ���Ͽ� �÷��̾�� �ش� ���� ����

        //Debug.Log("ondestroy ȣ��");

        monsterSpawn.MonsterDied();
        Destroy(gameObject);
    }

    public void SetTargetMonster(GameObject target)
    {
        // ���� ü�� Ŭ������ Ÿ�� ���� ����
        targetMonster = target;
    }
}





    public class PlayerSkill : MonoBehaviour
{
    public GameObject skillPrefab;    // ��ų ������
    public GameObject skillIndicatorPrefab; // ��ų ���� ������
    public float attackDistance = 20f; // ��ų ��Ÿ�
    public float skillDuration = 1f;   // ���� ���� �ð�
    public float skillIndicatorDuration = 1f;   // ���� ���� �ð�


    PlayerHealth playerHealth;
    int plSkillDamage; // ��ų ������

    private GameObject player;
    private GameObject skill;   // ��ų
    private GameObject skillIndicator; // ��ų ����
    private GameObject targetMonster;  // ���� ������ ����


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
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // ���� ���� �ø���
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(skillIndicatorDuration); // ���� �ð� �Ŀ� ���� ����

            Destroy(skillIndicator); // ��ų ���� ����


            // ��ų�� �ߵ��ϴ� �Լ�
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

                            MonsterHealth monsterHealth = targetMonster.GetComponent<MonsterHealth>();  // ���� ü�� ������Ʈ ��������
                            playerHealth = player.GetComponent<PlayerHealth>();    //������ ������Ʈ ��������
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
                Destroy(skill);     // ���� �ð� �Ŀ� ��ų ����


            }
            else
            {
                Debug.Log("���͸� ã�� ����");

                // ���͸� ã�� �������Ƿ� �÷��̾�� ��ų ������ ����
                Vector3 spawnSkillArea = player.transform.position;
                GameObject skill = Instantiate(skillPrefab, spawnSkillArea, Quaternion.identity);   // �÷��̾�� ��ų ȿ���� ����


                yield return new WaitForSeconds(skillDuration);
                Destroy(skill); // ���� �ð� �Ŀ� ��ų ����

                //GameObject[] skills = GameObject.FindGameObjectsWithTag("Skill");   //���� ���� �ʱ�ȭ �۾�
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

            // x�� z ��ǥ�� 20 �̳��� �ִ� ���͸� ���
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





















    public GameObject monsterPrefab;    // ���� ������
    public GameObject eliteMonsterPrefab;   //������ ������

    public int maxMonster = 5;        // ������ ���� ��
    private int currentMonster = 0;     //������ ���� ��
    public float minSpawnDistance = 10f; // �ּ� ���� �Ÿ�
    public float maxSpawnDistance = 15f; // �ִ� ���� �Ÿ�

    public static event Action<MonsterInfo> OnMonsterSpawn;     //���� ���� �̺�Ʈ
    //public event Action<Monster> OnMonsterDied; 

    private int dieMonsterCount;   //����Ʈ ���͸� ���� ���� ī��Ʈ

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

    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        GameObject monsterFolder = new GameObject("Monster");   // ���� ���� ����
        Transform folderTransform = monsterFolder.transform;    // ���� ���� ��ġ����

        Transform playerTransform; // �÷��̾��� Transform

        while (true) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");  //�÷��̾� ã��
            if (player != null ) {
                if (currentMonster < maxMonster)
                {
                    playerTransform = player.transform;

                    float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);


                    Vector3 spawnDirection = UnityEngine.Random.insideUnitSphere.normalized; // ������ ���� ����
                    spawnDirection.y = 0;
                    Vector3 spawnPosition = player.transform.position + spawnDirection * spawnDistance;     // ���� �Ÿ�����


                    GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);    //���� ���� ������, ��ġ ����

                    monster.transform.SetParent(folderTransform);   //���� �θ����� ����

                    currentMonster++;

                    monster.transform.rotation = playerTransform.rotation;  //������ rotation�� �÷��̾��� rotation���� ����


                    MonsterInfo monsterInfo = new MonsterInfo(monster, monster.transform);  //���� �������
                    CallMonsterSpawnEvent(monsterInfo);                                     //�̺�Ʈ ���


                    //����Ʈ ���� �߰�
                    if (dieMonsterCount > 5 )                
                    {
                        SpawnEliteMonster(folderTransform, player, spawnPosition);
                        dieMonsterCount = 0;
                    }
                }
            }
            yield return new WaitForSeconds(1f);    //���� ���� �ӵ�
        } // while ����
    }

    public void MonsterDied()
    {
        //OnMonsterDied?.Invoke(monster);
        currentMonster--; // ���� ��� �� ī��Ʈ ����

        dieMonsterCount++;
    }

    private void CallMonsterSpawnEvent(MonsterInfo monsterInfo)
    {
        OnMonsterSpawn?.Invoke(monsterInfo); // ���� ���� ����
    }

    void SpawnEliteMonster(Transform folderTransform, GameObject player ,Vector3 spawnPosition)
    {
        Transform playerTransform = player.transform;

        GameObject eliteMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);    //����Ʈ ���� ���� ������, ��ġ ����
        eliteMonster.name = "����Ʈ����";
        eliteMonster.transform.SetParent(folderTransform);   //���� �θ����� ����
        eliteMonster.transform.rotation = playerTransform.rotation;  //������ rotation�� �÷��̾��� rotation���� ����

        MonsterHealth monsterHealth = eliteMonster.GetComponent<MonsterHealth>();
        monsterHealth.maxHealth = Mathf.RoundToInt(monsterHealth.maxHealth * 1.5f); // �Ϲ� ������ ü���� 1.5��� ����
        monsterHealth.currentHealth = monsterHealth.maxHealth; // ���� ü���� �ִ� ü������ ����

        MonsterInfo eliteMonsterInfo = new MonsterInfo(eliteMonster, eliteMonster.transform);   //���� �������
        CallMonsterSpawnEvent(eliteMonsterInfo);                                                //�̺�Ʈ ���
    }



    ���ӸŴ����κ�

    private void Awake()
    {
        Instance = this; // �̱��� �ν��Ͻ� ����

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
    public GameObject skillPrefab;    // ��ų ������
    public GameObject skillIndicatorPrefab; // ��ų ���� ������
    public float attackDistance = 10f; // ��ų ��Ÿ�
    public float skillDuration = 1f;   // ���� ���� �ð�
    public float skillIndicatorDuration = 1f;   // ���� ���� �ð�

    private GameObject player;
    private GameObject skillIndicator; // ��ų ����
    private GameObject[] monsters;  // ���� �����ϴ� ��� ���� �迭

    private Transform playerTransform;

    private GameManager gameManager; // GameManager ��ü
    private PlayerController playerController; // PlayerController ��ũ��Ʈ ����

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
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 1f, 0); // ���� ���� �ø���
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
                skillIndicator.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);

            }

            yield return new WaitForSeconds(skillIndicatorDuration); // ���� �ð� �Ŀ� ���� ����

            Destroy(skillIndicator); // ��ų ���� ����

            // ��ų�� �ߵ��ϴ� �Լ�
            GameObject skillInstance = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);

            skillInstance.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);



            // ��ų �ݶ��̴� ���� �ִ� ���͵鿡�� �������� �ִ� �ڵ�

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
            Destroy(skillInstance); // ���� �ð� �Ŀ� ��ų ����
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
                Vector3 spawnSkillIndicatorPosition = hit.point + new Vector3(0, 0.1f, 0); // ���� ���� �ø���
                skillIndicator = Instantiate(skillIndicatorPrefab, spawnSkillIndicatorPosition, Quaternion.identity);
                skillIndicator.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);
            }

            yield return new WaitForSeconds(skillIndicatorDuration); // ���� �ð� �Ŀ� ���� ����

            Destroy(skillIndicator); // ��ų ���� ����

            // ��ų�� �ߵ��ϴ� �Լ�
            GameObject skillInstance = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);

            skillInstance.transform.rotation = Quaternion.Euler(playerRotationEuler.x, playerRotationEuler.y, playerRotationEuler.z);
            


            // ��ų �ݶ��̴� ���� �ִ� ���͵鿡�� �������� �ִ� �ڵ�

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
            Destroy(skillInstance); // ���� �ð� �Ŀ� ��ų ����

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
    



    public Transform player; // �÷��̾��� Transform
    public float distance = 0f; // ī�޶�� �÷��̾� ������ �Ÿ�
    public float height = 10f; // ī�޶��� ����

    private Vector3 offset; // �ʱ� �Ÿ� ����

    void Start()
    {
        // �ʱ� �Ÿ� ����
        offset = new Vector3(0f, height, -distance); 
    }

    void LateUpdate()
    {
        // �÷��̾��� ��ġ�� ���� ī�޶� ��ġ ������Ʈ
        transform.position = player.position + offset;

        // �÷��̾ �ٶ󺸵��� ȸ��
        transform.LookAt(player);
    }


    GameObject player = GameObject.FindGameObjectWithTag("Player");

        Transform playerTransform = player.transform;

        float spawnDistance = UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);


        Vector3 spawnDirection = UnityEngine.Random.insideUnitSphere.normalized; // ������ ���� ����
        spawnDirection.y = 0;
        Vector3 spawnPosition = player.transform.position + spawnDirection * spawnDistance;     // ���� �Ÿ�����

        //Vector3 spawnPosition = player.transform.position + UnityEngine.Random.insideUnitSphere.normalized * UnityEngine.Random.Range(minSpawnDistance, maxSpawnDistance);
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);




    if (!playerSkill.isUsingSkill && !fireIsCooldown && !isSkillUse)


    if (!playerSkill.isUsingSkill && !iceIsCooldown && !isSkillUse)




            /*
            if (!isAttacking)  // ���� ���� ���� �ƴϸ�
            {
                FindClosestMonster();
                StartCoroutine(SpawnAttack());  // ������ �����ϰ� �Ϸ�� ������ ��ٸ�
            }
            yield return new WaitForSeconds(attackTime);  // 1�� �Ŀ� �ٽ� �õ�


    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // ���͸� ���� ����Ʈ���� ����
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
    [SerializeField] private int damagePerMonster = 10; // ���ʹ� �Դ� ������


    public List<Collider> monstersInContact = new List<Collider>(); // ������ ���� ����Ʈ

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


    // ������
    private IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            Debug.Log(monstersInContact.Count);
            totalDamage = monstersInContact.Count * damagePerMonster; // �� ������ ���
            TakeDamage(totalDamage); // ������ ����
            if (totalDamage > 0) { effectSprite.ApplyEffect(); }    // �������� ������ ����Ʈ ����
            monstersInContact.Clear(); // ���� ����Ʈ ����
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
        Debug.Log("�÷��̾� ���");
        // �÷��̾� ��� ó��
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
            monstersInContact.Remove(other); // �ش� ���͸� ����Ʈ���� ����
            if (monstersInContact.Count == 0)
            {
                StopCoroutine(ApplyDamageOverTime()); // ��� ���Ϳ��� ������ ������ ���� �ڷ�ƾ�� ����
            }
        }
    }

    private void RemoveMonsterFromContactList(Collider monsterCollider)
    {
        monstersInContact.Remove(monsterCollider); // ���͸� ���� ����Ʈ���� ����
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
    [SerializeField] public int maxHealth = 100; // ������ �⺻ ü��
    [SerializeField] public int currentHealth; // ���� ü��
    [SerializeField] public int monsterDamage = 10;
    [SerializeField] public int giveExp = 50;  // �÷��̾� ����ġ ������
    [SerializeField] public bool isEliteMonster;
    [SerializeField] public bool isDamageCooldown = false;
    [SerializeField] private float damageCooldownDuration = 0.5f; // 0.5�� ���� �������� ���ߵ��� ����

    private MonsterSpawn monsterSpawn;
    private GameObject targetMonster;
    private GameObject player;
    private EffectSprite effectSprite;


    public event Action<Collider> OnMonsterDeath; // ���Ͱ� �׾��� �� �߻��ϴ� �̺�Ʈ
    public event Action<MonsterHealth> OnMonsterDrop;
    public static event Action<int> OnMonsterExp;

    

    public GameObject goldPrefab;

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �⺻ ü������ �ʱ�ȭ
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


    // �������� �޴� �Լ�
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
        GameObject goldInstance = Instantiate(goldPrefab, transform.position, Quaternion.identity); // ��� ����
        goldInstance.transform.rotation = player.transform.rotation;

        OnMonsterDeath?.Invoke(GetComponent<Collider>()); // ���� ����
        OnMonsterDrop?.Invoke(this);
        OnMonsterExp?.Invoke(giveExp);


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

        /*
    public void UpgradeAttackDamage()
    {
        // GameManager�� ��ȭ �Լ� ȣ��
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






    // ���׷��̵� ---------------------------------------------------------------------------------------------

    public bool UpgradeAddGold()
    {
        int upgradeCost = CalculateGoldUpgradeCost(baseGoldUpgradeCost);      // ���׷��̵� ��� ���

        if (gold >= upgradeCost)        // �÷��̾��� ��尡 ������� Ȯ��
        {
            baseGoldUpgradeCost = upgradeCost;
            msGold += goldIncrease;    
            gold -= upgradeCost;    // ��� �Һ�
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("ȹ�� ��差 ���׷��̵� �Ϸ�. ���� ȹ�淮: " + msGold);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeAddJewel()
    {
        int upgradeCost = CalculateJewelUpgradeCost(baseJewelUpgradeCost);      // ���׷��̵� ��� ���

        if (gold >= upgradeCost)        // �÷��̾��� ��尡 ������� Ȯ��
        {
            baseJewelUpgradeCost = upgradeCost;
            msJewel += goldIncrease;     
            gold -= upgradeCost;    // ��� �Һ�
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("ȹ�� ��差 ���׷��̵� �Ϸ�. ���� ȹ�淮: " + msGold);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeAttackDamage()
    {
        int upgradeCost = CalculateAttackUpgradeCost(baseAttackUpgradeCost);      // ���׷��̵� ��� ���

        if (gold >= upgradeCost)        // �÷��̾��� ��尡 ������� Ȯ��
        {
            baseAttackUpgradeCost = upgradeCost;
            playerDamage += damageIncrease;     // ��ȭ�� ���ݷ� ����
            gold -= upgradeCost;    // ��� �Һ�
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + playerDamage);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeFireDamage()
    {
        int upgradeCost = CalculateFireUpgradeCost(baseFireUpgradeCost);      // ���׷��̵� ��� ���

        if (gold >= upgradeCost)        // �÷��̾��� ��尡 ������� Ȯ��
        {
            baseFireUpgradeCost = upgradeCost;
            fireDamage += damageIncrease;     // ��ȭ�� ���ݷ� ����
            gold -= upgradeCost;    // ��� �Һ�
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + fireDamage);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return false;
        }
    }

    public bool UpgradeIceDamage()
    {
        int upgradeCost = CalculateIceUpgradeCost(baseIceUpgradeCost);      // ���׷��̵� ��� ���

        if (gold >= upgradeCost)        // �÷��̾��� ��尡 ������� Ȯ��
        {
            baseIceUpgradeCost = upgradeCost;
            iceDamage += damageIncrease;     // ��ȭ�� ���ݷ� ����
            gold -= upgradeCost;    // ��� �Һ�
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("���ݷ� ���׷��̵� �Ϸ�. ���� ��ȭ�� ���ݷ�: " + iceDamage);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
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
            Debug.Log("���׷��̵忡 ����� ���: " + upgradeCost);
            Debug.Log("�̵��ӵ� ���׷��̵� �Ϸ�. ���� �̵��ӵ�: " + playerController.moveSpeed);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
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
            jewel -= upgradeCost;    // ���� �Һ�
            Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
            Debug.Log("����ġ ȹ�淮 ���׷��̵� �Ϸ�. ���� ����ġ ȹ�淮 : " + msExp);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
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
            Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
            Debug.Log("���� �ӵ� ���׷��̵� �Ϸ�. ���� ���� �ӵ� : " + playerAttack.attackTime);
            return true;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�. ���׷��̵忡 �ʿ��� ����: " + upgradeCost);
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
            Debug.Log("���׷��̵忡 ����� ����: " + upgradeCost);
            Debug.Log("ü�� ȸ�� ���׷��̵� �Ϸ�. ���� ü�� ȸ���� : " + playerHealth.recoveryHealth);
            return true;
        }
        else
        {
            Debug.Log("������ �����մϴ�. ���׷��̵忡 �ʿ��� ���: " + upgradeCost);
            return false;
        }
    }

    // ���׷��̵� ��� ��� ----------------------------------------------------

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
        // ���������� ���� ��ư�� Ÿ�Կ� ���� ��ȭ ����
        switch (upgradeType)
        {
            case "attack":
                upgradeSuccess = gameManager.UpgradeAttackDamage();
                uiText.text = "�⺻���� ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.playerDamage + "\n��ȭ �� ���ݷ� : " + (gameManager.playerDamage + gameManager.damageIncrease);
                uiText.text += "\n �ʿ� ��� : " + 
                break;
            case "fire":
                upgradeSuccess = gameManager.UpgradeFireDamage();
                uiText.text = "�ҵ� ��ų ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.fireDamage + "\n��ȭ �� ���ݷ� : " + (gameManager.fireDamage + gameManager.damageIncrease);
                break;
            case "gold":
                upgradeSuccess = gameManager.UpgradeAddGold();
                uiText.text = "��� ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msGold + "\n��ȭ �� ȹ�淮 : " + (gameManager.msGold + gameManager.goldIncrease);
                break;
            case "jewel":
                upgradeSuccess = gameManager.UpgradeAddJewel();
                uiText.text = "���� ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msJewel + "\n��ȭ �� ȹ�淮 : " + (gameManager.msJewel + gameManager.goldIncrease);
                break;
            case "ice":
                upgradeSuccess = gameManager.UpgradeIceDamage();
                uiText.text = "���̽� ��ų ��ȭ";
                uiText.text += "\n\n���� ���ݷ� : " + gameManager.iceDamage + "\n��ȭ �� ���ݷ� : " + (gameManager.iceDamage + gameManager.damageIncrease);
                break;
            case "moveSpeed":
                upgradeSuccess = gameManager.UpgradePlMoveSpeed();
                uiText.text = "�̵��ӵ� ��ȭ";
                uiText.text += "\n\n���� �̵��ӵ� : " + playerController.moveSpeed + "\n��ȭ �� �̵��ӵ� : " + (playerController.moveSpeed + 0.3f).ToString("F1");
                break;
            case "exp":
                upgradeSuccess = gameManager.UpgradeExpIncrease();
                uiText.text = "����ġ ȹ�淮 ��ȭ";
                uiText.text += "\n\n���� ȹ�淮 : " + gameManager.msExp + "\n��ȭ �� ȹ�淮 : " + (gameManager.msExp + gameManager.expIncrease);
                break;
            case "attackSpeed":
                upgradeSuccess = gameManager.UpgradeAtkSpeed();
                uiText.text = "���ݼӵ� ��ȭ";
                uiText.text += "\n\n���� ���ݼӵ� : " + playerAttack.attackTime + "\n��ȭ �� ���ݼӵ� : " + (playerAttack.attackTime - 0.05f).ToString("F2");
                break;
            case "health":
                upgradeSuccess = gameManager.UpgradePlHpIncrease();
                uiText.text = "ü�� ȸ���� ��ȭ";
                uiText.text += "\n\n���� ȸ���� : " + playerHealth.recoveryHealth + "\n��ȭ �� ȹ�淮 : " + (playerHealth.recoveryHealth + gameManager.recoveryHpIncrease);
                break;
        }
        SetCharaText();
    }
    */




}
