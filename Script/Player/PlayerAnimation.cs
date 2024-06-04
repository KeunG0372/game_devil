using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private static PlayerAnimation instance;
    private Animator animator;
    private bool isAttacking = false;
    

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // IsAttack 트리거를 감지하고 애니메이션 상태를 확인
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        // 공격 중이 아니라면 플립 변경
        if (!isAttacking)
        {
            if (!playerController.isMovingLeft)
            {
                spriteRenderer.flipX = false; // 왼쪽으로 이동할 때는 스프라이트를 그대로 표시
            }

            else
            {
                spriteRenderer.flipX = true; // 오른쪽으로 이동할 때는 스프라이트를 X 축으로 반전
            }
        }
    }

}
