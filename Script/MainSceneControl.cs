using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneControl : MonoBehaviour
{
    public UnityEngine.UI.Text startText;
    public float fadeDuration = 1f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;

    public Button startButton;

    public GameObject fadeObj;
    public FadeInOut fade;
    public Image charaImage;
    public Image batImage;
    public Image coinImage;
    public Image jewelImage;

    private GameManager gameManager;

    // �ؽ�Ʈ Fade In Fade Out
    private void Start()
    {
        StartCoroutine(CharaFloatUp());
        StartCoroutine(BatFloatUp());
        StartCoroutine(CoinFloatUp());
        StartCoroutine(JewelFloatUp());
        StartCoroutine(LoadNextScene());
        startButton.interactable = false;

    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            // Fade out
            float elapsedTime = 0f;
            Color startColor = startText.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, minAlpha);

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                startText.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            // Fade in
            elapsedTime = 0f;
            startColor = startText.color;
            endColor = new Color(startColor.r, startColor.g, startColor.b, maxAlpha);

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                startText.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator LoadNextScene()
    {
        // ���� �������� �ε��� �����մϴ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage1");
        asyncLoad.allowSceneActivation = false; // �� Ȱ��ȭ�� �Ͻ� �ߴ��մϴ�.

        // �� �ε��� �Ϸ�� ������ ��ٸ��ϴ�.
        while (!asyncLoad.isDone)
        {
            // �� �ε� ���൵�� ǥ���մϴ�.
            startText.text = "Loading... " + (asyncLoad.progress * 100) + "%";

            // �� �ε��� 90%���� ����Ǹ� ��ư�� Ȱ��ȭ�ϰ� "Press To Start"�� �ؽ�Ʈ�� �����մϴ�.
            if (asyncLoad.progress >= 0.9f)
            {
                startText.text = "- Press To Start -";
                StartCoroutine(FadeText());

                startButton.interactable = true;
            }

            yield return null;
        }
    }

    public void LoadNextSceneOnClick()
    {
        GameManager.Instance.SetStage(1);
        StartCoroutine(WaitForLoadScene());
    }

    private IEnumerator WaitForLoadScene()
    {
        UnityEngine.UI.Image fadeImage = fadeObj.GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(fade.FadeOut(fadeImage));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage1");
    }

    IEnumerator CharaFloatUp()
    {
        Vector3 startPos = charaImage.rectTransform.position; // �̹����� ���� ��ġ ����
        Vector3 targetPos = startPos + Vector3.up * 30f; // �̹����� �ö� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            charaImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� ���� ��ġ�� �ǵ��ƿ��� �մϴ�.
        StartCoroutine(CharaFloatBack());
    }

    IEnumerator CharaFloatBack()
    {
        Vector3 targetPos = charaImage.rectTransform.position; // �̹����� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 startPos = targetPos - Vector3.up * 30f; // �̹����� ���� ��ġ�� �ǵ��ư� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            charaImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� �ö󰡴� �κ��� �ݺ��մϴ�.
        StartCoroutine(CharaFloatUp());
    }

    IEnumerator BatFloatUp()
    {
        Vector3 startPos = batImage.rectTransform.position; // �̹����� ���� ��ġ ����
        Vector3 targetPos = startPos + Vector3.up * 25f; // �̹����� �ö� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            batImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� ���� ��ġ�� �ǵ��ƿ��� �մϴ�.
        StartCoroutine(BatFloatBack());
    }

    IEnumerator BatFloatBack()
    {
        Vector3 targetPos = batImage.rectTransform.position; // �̹����� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 startPos = targetPos - Vector3.up * 25f; // �̹����� ���� ��ġ�� �ǵ��ư� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            batImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� �ö󰡴� �κ��� �ݺ��մϴ�.
        StartCoroutine(BatFloatUp());
    }

    IEnumerator CoinFloatUp()
    {
        Vector3 startPos = coinImage.rectTransform.position; // �̹����� ���� ��ġ ����
        Vector3 targetPos = startPos + Vector3.up * 20f; // �̹����� �ö� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 1.8f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            coinImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� ���� ��ġ�� �ǵ��ƿ��� �մϴ�.
        StartCoroutine(CoinFloatBack());
    }

    IEnumerator CoinFloatBack()
    {
        Vector3 targetPos = coinImage.rectTransform.position; // �̹����� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 startPos = targetPos - Vector3.up * 20f; // �̹����� ���� ��ġ�� �ǵ��ư� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 1.8f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            coinImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� �ö󰡴� �κ��� �ݺ��մϴ�.
        StartCoroutine(CoinFloatUp());
    }

    IEnumerator JewelFloatUp()
    {
        Vector3 startPos = jewelImage.rectTransform.position; // �̹����� ���� ��ġ ����
        Vector3 targetPos = startPos + Vector3.up * 15f; // �̹����� �ö� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2.1f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            jewelImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� ���� ��ġ�� �ǵ��ƿ��� �մϴ�.
        StartCoroutine(JewelFloatBack());
    }

    IEnumerator JewelFloatBack()
    {
        Vector3 targetPos = jewelImage.rectTransform.position; // �̹����� ���� ��ġ�� ��ǥ ��ġ�� ����
        Vector3 startPos = targetPos - Vector3.up * 15f; // �̹����� ���� ��ġ�� �ǵ��ư� ��ǥ ��ġ
        float elapsedTime = 0f;
        while (elapsedTime < 2.1f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // ���� �Լ��� �̿��Ͽ� �ε巯�� ������ ����
            jewelImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // �̹��� ��ġ �ε巴�� ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // �ٽ� �ڷ�ƾ�� �����Ͽ� �̹����� �ö󰡴� �κ��� �ݺ��մϴ�.
        StartCoroutine(JewelFloatUp());
    }
}
