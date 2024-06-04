using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 골드가 날아가는 속도

    private Transform player; // 플레이어의 위치
    private Vector3 direction; // 플레이어를 향하는 방향

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
            direction = (player.position - transform.position).normalized;  // 플레이어를 향해 이동
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

