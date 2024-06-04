using System.Collections;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public enum SkillType { Fire, Ice };

    public bool isUsingSkill = false;
    public SkillType currentSkill = SkillType.Fire; // ���� ��ų Ÿ��
    public GameObject firePrefab;    // ���̾� ��ų ������
    public GameObject icePrefab;     // ���̽� ��ų ������
    public GameObject fireSkillIndicatorPrefab; // ���̾� ��ų ���� ������
    public GameObject iceSkillIndicatorPrefab; // ���̽� ��ų ���� ������
    public GameObject hitEffectPrefab;      // ����Ʈ ������


    [SerializeField] public float attackDistance = 5f; // ��ų ��Ÿ�

    private float skillDuration = 1f;   // ���� ���� �ð�
    private float skillIndicatorDuration = 1f;   // ���� ���� �ð�

    private GameObject player;
    private GameObject[] monsters;  // ���� �����ϴ� ��� ���� �迭

    private Transform playerTransform;

    private GameManager gameManager; // GameManager ��ü
    private PlayerController playerController; // PlayerController ��ũ��Ʈ ����

    public bool autoMode = false;

    public PlayerHealth playerHealth;


    void Start()
    {
        gameManager = GameManager.Instance;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void UseSkill()
    {
        if (playerHealth.playerAlive)
        {
            StartCoroutine(SpawnSkill());
        }
    }

    IEnumerator SpawnSkill()
    {
        isUsingSkill = true;
        player = GameObject.FindGameObjectWithTag("Player");
        monsters = GameObject.FindGameObjectsWithTag("Monster");

        GameObject skillPrefab = currentSkill == SkillType.Fire ? firePrefab : icePrefab;
        GameObject skillIndicatorPrefab = currentSkill == SkillType.Fire ? fireSkillIndicatorPrefab : iceSkillIndicatorPrefab;
        SoundManager.Sfx skillSound = currentSkill == SkillType.Fire ? SoundManager.Sfx.Sword : SoundManager.Sfx.Ice; // ��ų�� �ش��ϴ� ����

        GameObject targetMonster = FindClosestMonster(player);
        playerTransform = player.transform;
        Vector3 playerRotationEuler = playerTransform.rotation.eulerAngles;

        Vector3 skillIndicatorPosition = Vector3.zero;

        if (targetMonster != null)
        {
            skillIndicatorPosition = targetMonster.transform.position;
        }
        else
        {
            Vector3 direction = playerController.isMovingLeft ? Vector3.left : Vector3.right;
            skillIndicatorPosition = player.transform.position + direction * 5f;
        }
        // ������ �ߵ��ϴ� �Լ�
        GameObject skillIndicator = currentSkill == SkillType.Fire ? fireSkillIndicatorPrefab : iceSkillIndicatorPrefab;
        skillIndicator = Instantiate(skillIndicator, skillIndicatorPosition, Quaternion.identity);
        skillIndicator.transform.rotation = Quaternion.Euler(playerRotationEuler);
        yield return new WaitForSeconds(skillIndicatorDuration);
        Destroy(skillIndicator);

        // ��ų�� �ߵ��ϴ� �Լ�
        GameObject skillInstance = Instantiate(skillPrefab, skillIndicatorPosition, Quaternion.identity);
        skillInstance.transform.rotation = Quaternion.Euler(playerRotationEuler);


        SoundManager.instance.PlaySfx(skillSound);

        DealDamageToMonsters(skillInstance);

        yield return new WaitForSeconds(skillDuration);
        Destroy(skillInstance);

        isUsingSkill = false;
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


    private void DealDamageToMonsters(GameObject skillInstance)
    {
        float skillColliderRadius = skillInstance.GetComponent<CapsuleCollider>().radius;
        Collider[] hitColliders = Physics.OverlapSphere(skillInstance.transform.position, skillColliderRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Monster"))
            {
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                {
                    int damage = currentSkill == SkillType.Fire ? gameManager.fireDamage : gameManager.iceDamage;
                    monsterHealth.TakeDamage(damage);

                    GameObject hitEffect = Instantiate(hitEffectPrefab, collider.transform.position, collider.transform.rotation);

                    Destroy(hitEffect, 0.5f);
                }
            }
        }
    }

    
}