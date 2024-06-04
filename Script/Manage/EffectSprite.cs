using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSprite : MonoBehaviour
{
    [SerializeField] private float duration = 0.1f;

    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyEffect()
    {
        if (this != null && gameObject != null) // 객체가 유효한지 확인
        {
            StartCoroutine(PlayDamageEffect());
        }
    }

    IEnumerator PlayDamageEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.2f); // 이펙트 처리
            yield return new WaitForSeconds(duration); // 일정 시간 대기
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // 원래 색상으로 복구
            yield return new WaitForSeconds(duration); // 일정 시간 대기
        }
    }
}
