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
    public bool isMovingLeft = false; // 왼쪽으로 이동 중인지 여부

    public bool MoveAuto = false; // 자동 이동 여부를 나타내는 변수

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
            Debug.LogError("Main Camera를 찾을 수 없습니다.");
        }
        if (floatingJoystick == null)
        {
            Debug.LogError("Floating Joystick을 찾을 수 없습니다.");
        }
        if (playerSkill == null)
        {
            Debug.LogError("PlayerSkill 스크립트를 찾을 수 없습니다.");
        }
    }

    void FixedUpdate()
    {
        // 조이스틱 입력 처리
        float x = floatingJoystick.Horizontal;
        float z = floatingJoystick.Vertical;

        // 플레이어의 조이스틱 입력에 따른 움직임
        if (x != 0 || z != 0)
        {
            MoveAuto = false;

            if (x > 0)      // 오른쪽으로 이동
            {
                isMovingLeft = false; // 왼쪽으로 이동 중인지 여부를 나타내는 bool 변수
            }
            else if (x < 0) // 왼쪽으로 이동
            {
                isMovingLeft = true; // 왼쪽으로 이동 중인지 여부를 나타내는 bool 변수
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

                Vector3 relativePosition = targetMonster.transform.position - transform.position;   //몬스터가 어느쪽에 있는지 계산
                if (relativePosition.x < 0.1)
                {
                    isMovingLeft = true; // 왼쪽으로 이동 중인지 여부를 나타내는 bool 변수
                }
                else
                {
                    isMovingLeft = false; // 왼쪽으로 이동 중인지 여부를 나타내는 bool 변수
                }

                float distanceToMonster = Vector3.Distance(transform.position, targetMonster.transform.position);
                // 몬스터와의 자동 이동 중지
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

    // --------------- 오토모드 ------------------

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