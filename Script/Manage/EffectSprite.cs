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
        if (this != null && gameObject != null) // ��ü�� ��ȿ���� Ȯ��
        {
            StartCoroutine(PlayDamageEffect());
        }
    }

    IEnumerator PlayDamageEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.2f); // ����Ʈ ó��
            yield return new WaitForSeconds(duration); // ���� �ð� ���
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // ���� �������� ����
            yield return new WaitForSeconds(duration); // ���� �ð� ���
        }
    }
}
