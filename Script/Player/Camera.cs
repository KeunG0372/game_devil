using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform

    [SerializeField] private float distance = 0.1f; // ī�޶�� �÷��̾� ������ �Ÿ�
    [SerializeField] private float height = 16f; // ī�޶��� ����

    [SerializeField] private float maxX = 15f;
    [SerializeField] private float minX = -12.6f;
    [SerializeField] private float minZ = -12.5f;

    private Vector3 offset; // �ʱ� �Ÿ� ����

    void Start()
    {
        // �ʱ� �Ÿ� ����
        offset = new Vector3(0f, height, -distance);

        StartCoroutine(CameraRotation());

        AssignPlayer();
    }

    void OnEnable()
    {
        // ���� �ε�� ������ �÷��̾� ��ü�� �ٽ� �����մϴ�.
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
            Debug.LogError("Player ��ü�� ã�� �� �����ϴ�.");
        }
    }

    void LateUpdate()
    {
        // �÷��̾��� ���� ��ġ
        Vector3 playerPosition = player.position;

        // �÷��̾��� ��ġ���� x�� y�� ����
        float camerax = playerPosition.x;
        float cameraz = playerPosition.z;

        // x��ǥ�� -16 ������ ���
        if (camerax <= minX)
        {
            camerax = minX;
        }
        // x��ǥ�� 18 �̻��� ���
        else if (camerax >= maxX)
        {
            camerax = maxX;
        }

        // z��ǥ�� -15 ������ ���
        if (cameraz <= minZ)
        {
            cameraz = minZ;
        }

        // ī�޶��� ��ġ ������Ʈ
        transform.position = new Vector3(camerax, playerPosition.y + height, cameraz - distance);
    }

    IEnumerator CameraRotation()
    {
        yield return new WaitForSeconds(0.3f);
        transform.LookAt(player);
    }
}
