using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    public Transform playerTransform;
    public Camera mainCamera;
    public FloatingJoystick floatingJoystick;

    private Rigidbody rb;
    private Transform mainCameraTransform;

    public PlayerSkill playerSkill;
    public bool isMovingLeft = false; // �������� �̵� ������ ����

    public bool MoveAuto = false; // �ڵ� �̵� ���θ� ��Ÿ���� ����

    //------------------------------------------------------//

    private GameObject player;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    void Start()
    {
        AssignReferences();

        mainCameraTransform = Camera.main.transform;
        GetComponent<Rigidbody>().freezeRotation = true;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(RotationFix());
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AssignReferences();
    }

    private void AssignReferences()
    {
        mainCamera = Camera.main;
        floatingJoystick = FindObjectOfType<FloatingJoystick>();
        playerSkill = FindObjectOfType<PlayerSkill>();

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� ã�� �� �����ϴ�.");
        }
        if (floatingJoystick == null)
        {
            Debug.LogError("Floating Joystick�� ã�� �� �����ϴ�.");
        }
        if (playerSkill == null)
        {
            Debug.LogError("PlayerSkill ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    void FixedUpdate()
    {
        // ���̽�ƽ �Է� ó��
        float x = floatingJoystick.Horizontal;
        float z = floatingJoystick.Vertical;

        // �÷��̾��� ���̽�ƽ �Է¿� ���� ������
        if (x != 0 || z != 0)
        {
            MoveAuto = false;

            if (x > 0)      // ���������� �̵�
            {
                isMovingLeft = false; // �������� �̵� ������ ���θ� ��Ÿ���� bool ����
            }
            else if (x < 0) // �������� �̵�
            {
                isMovingLeft = true; // �������� �̵� ������ ���θ� ��Ÿ���� bool ����
            }

            Vector3 movement = new Vector3(x, 0, z) * GameManager.Instance.playerMoveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
        else
        {
            MoveAuto = true;
            animator.SetBool("isMoving", false);
        }

        if (MoveAuto && playerSkill.autoMode)
        {
            GameObject[] allMonsters = GameObject.FindGameObjectsWithTag("Monster");
            GameObject targetMonster = FindClosestMonsterInAllRange(allMonsters);

            if (targetMonster != null)
            {
                animator.SetBool("isMoving", true);

                Vector3 relativePosition = targetMonster.transform.position - transform.position;   //���Ͱ� ����ʿ� �ִ��� ���
                if (relativePosition.x < 0.1)
                {
                    isMovingLeft = true; // �������� �̵� ������ ���θ� ��Ÿ���� bool ����
                }
                else
                {
                    isMovingLeft = false; // �������� �̵� ������ ���θ� ��Ÿ���� bool ����
                }

                float distanceToMonster = Vector3.Distance(transform.position, targetMonster.transform.position);
                // ���Ϳ��� �ڵ� �̵� ����
                if (distanceToMonster < 10f)
                {
                    MoveAuto = false;
                    animator.SetBool("isMoving", false);
                }
                else
                {
                    Vector3 direction = (targetMonster.transform.position - transform.position).normalized;
                    transform.position += direction * GameManager.Instance.playerMoveSpeed * Time.deltaTime;
                }
            }
        }
    }

    IEnumerator RotationFix()
    {
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(mainCameraTransform);
    }


    private void OnCollisionStay(Collision other)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    // --------------- ������ ------------------

    private GameObject FindClosestMonsterInAllRange(GameObject[] allMonsters)
    {
        GameObject autoMonster = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = player.transform.position;

        foreach (GameObject monster in allMonsters)
        {
            Vector3 monsterPosition = monster.transform.position;
            float distance = Vector3.Distance(playerPosition, monsterPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                autoMonster = monster;
            }
        }

        return autoMonster;
    }
}