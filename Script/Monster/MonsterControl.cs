using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private Transform player; // �÷��̾��� ��ġ�� ������ ����

    [SerializeField] private float msSpeed = 3.5f;

    MonsterHealth monsterHealth;
    PlayerHealth playerHealth;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        monsterHealth = GetComponent<MonsterHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null && !monsterHealth.isDamageCooldown && playerHealth.playerAlive)
        {
            Vector3 direction = (player.position - transform.position).normalized;  // �÷��̾ ���� �̵�
            transform.position += direction * msSpeed * Time.deltaTime;

            if (player.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;  // �÷��̾ ���ʿ� ���� ��
            }
            else
            {
                spriteRenderer.flipX = false;  // �÷��̾ �����ʿ� ���� ��
            }
        }
    }
}
