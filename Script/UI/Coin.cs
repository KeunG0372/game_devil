using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // ��尡 ���ư��� �ӵ�

    private Transform player; // �÷��̾��� ��ġ
    private Vector3 direction; // �÷��̾ ���ϴ� ����

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        StartCoroutine(MoveTowardsPlayerDelayed(1f));
    }

    private IEnumerator MoveTowardsPlayerDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            direction = (player.position - transform.position).normalized;  // �÷��̾ ���� �̵�
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

