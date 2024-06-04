using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform

    [SerializeField] private float distance = 0.1f; // 카메라와 플레이어 사이의 거리
    [SerializeField] private float height = 16f; // 카메라의 높이

    [SerializeField] private float maxX = 15f;
    [SerializeField] private float minX = -12.6f;
    [SerializeField] private float minZ = -12.5f;

    private Vector3 offset; // 초기 거리 설정

    void Start()
    {
        // 초기 거리 설정
        offset = new Vector3(0f, height, -distance);

        StartCoroutine(CameraRotation());

        AssignPlayer();
    }

    void OnEnable()
    {
        // 씬이 로드될 때마다 플레이어 객체를 다시 참조합니다.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        AssignPlayer();
    }

    private void AssignPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player 객체를 찾을 수 없습니다.");
        }
    }

    void LateUpdate()
    {
        // 플레이어의 현재 위치
        Vector3 playerPosition = player.position;

        // 플레이어의 위치에서 x와 y값 저장
        float camerax = playerPosition.x;
        float cameraz = playerPosition.z;

        // x좌표가 -16 이하인 경우
        if (camerax <= minX)
        {
            camerax = minX;
        }
        // x좌표가 18 이상인 경우
        else if (camerax >= maxX)
        {
            camerax = maxX;
        }

        // z좌표가 -15 이하인 경우
        if (cameraz <= minZ)
        {
            cameraz = minZ;
        }

        // 카메라의 위치 업데이트
        transform.position = new Vector3(camerax, playerPosition.y + height, cameraz - distance);
    }

    IEnumerator CameraRotation()
    {
        yield return new WaitForSeconds(0.3f);
        transform.LookAt(player);
    }
}
