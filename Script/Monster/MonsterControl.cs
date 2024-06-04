using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private Transform player; // 플레이어의 위치를 저장할 변수

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
            Vector3 direction = (player.position - transform.position).normalized;  // 플레이어를 향해 이동
            transform.position += direction * msSpeed * Time.deltaTime;

            if (player.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;  // 플레이어가 왼쪽에 있을 때
            }
            else
            {
                spriteRenderer.flipX = false;  // 플레이어가 오른쪽에 있을 때
            }
        }
    }
}
