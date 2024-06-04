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
        // IsAttack Ʈ���Ÿ� �����ϰ� �ִϸ��̼� ���¸� Ȯ��
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        // ���� ���� �ƴ϶�� �ø� ����
        if (!isAttacking)
        {
            if (!playerController.isMovingLeft)
            {
                spriteRenderer.flipX = false; // �������� �̵��� ���� ��������Ʈ�� �״�� ǥ��
            }

            else
            {
                spriteRenderer.flipX = true; // ���������� �̵��� ���� ��������Ʈ�� X ������ ����
            }
        }
    }

}
