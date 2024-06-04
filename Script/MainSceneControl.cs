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

    // 텍스트 Fade In Fade Out
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
        // 다음 씬으로의 로딩을 시작합니다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Stage1");
        asyncLoad.allowSceneActivation = false; // 씬 활성화를 일시 중단합니다.

        // 씬 로딩이 완료될 때까지 기다립니다.
        while (!asyncLoad.isDone)
        {
            // 씬 로딩 진행도를 표시합니다.
            startText.text = "Loading... " + (asyncLoad.progress * 100) + "%";

            // 씬 로딩이 90%까지 진행되면 버튼을 활성화하고 "Press To Start"로 텍스트를 변경합니다.
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
        Vector3 startPos = charaImage.rectTransform.position; // 이미지의 시작 위치 저장
        Vector3 targetPos = startPos + Vector3.up * 30f; // 이미지가 올라갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            charaImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 시작 위치로 되돌아오게 합니다.
        StartCoroutine(CharaFloatBack());
    }

    IEnumerator CharaFloatBack()
    {
        Vector3 targetPos = charaImage.rectTransform.position; // 이미지의 현재 위치를 목표 위치로 설정
        Vector3 startPos = targetPos - Vector3.up * 30f; // 이미지가 시작 위치로 되돌아갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            charaImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 올라가는 부분을 반복합니다.
        StartCoroutine(CharaFloatUp());
    }

    IEnumerator BatFloatUp()
    {
        Vector3 startPos = batImage.rectTransform.position; // 이미지의 시작 위치 저장
        Vector3 targetPos = startPos + Vector3.up * 25f; // 이미지가 올라갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            batImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 시작 위치로 되돌아오게 합니다.
        StartCoroutine(BatFloatBack());
    }

    IEnumerator BatFloatBack()
    {
        Vector3 targetPos = batImage.rectTransform.position; // 이미지의 현재 위치를 목표 위치로 설정
        Vector3 startPos = targetPos - Vector3.up * 25f; // 이미지가 시작 위치로 되돌아갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            batImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 올라가는 부분을 반복합니다.
        StartCoroutine(BatFloatUp());
    }

    IEnumerator CoinFloatUp()
    {
        Vector3 startPos = coinImage.rectTransform.position; // 이미지의 시작 위치 저장
        Vector3 targetPos = startPos + Vector3.up * 20f; // 이미지가 올라갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 1.8f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            coinImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 시작 위치로 되돌아오게 합니다.
        StartCoroutine(CoinFloatBack());
    }

    IEnumerator CoinFloatBack()
    {
        Vector3 targetPos = coinImage.rectTransform.position; // 이미지의 현재 위치를 목표 위치로 설정
        Vector3 startPos = targetPos - Vector3.up * 20f; // 이미지가 시작 위치로 되돌아갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 1.8f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            coinImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 올라가는 부분을 반복합니다.
        StartCoroutine(CoinFloatUp());
    }

    IEnumerator JewelFloatUp()
    {
        Vector3 startPos = jewelImage.rectTransform.position; // 이미지의 시작 위치 저장
        Vector3 targetPos = startPos + Vector3.up * 15f; // 이미지가 올라갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2.1f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            jewelImage.rectTransform.position = Vector3.Lerp(startPos, targetPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 시작 위치로 되돌아오게 합니다.
        StartCoroutine(JewelFloatBack());
    }

    IEnumerator JewelFloatBack()
    {
        Vector3 targetPos = jewelImage.rectTransform.position; // 이미지의 현재 위치를 목표 위치로 설정
        Vector3 startPos = targetPos - Vector3.up * 15f; // 이미지가 시작 위치로 되돌아갈 목표 위치
        float elapsedTime = 0f;
        while (elapsedTime < 2.1f)
        {
            float t = Mathf.Sin(elapsedTime / 10f * Mathf.PI); // 사인 함수를 이용하여 부드러운 움직임 구현
            jewelImage.rectTransform.position = Vector3.Lerp(targetPos, startPos, t); // 이미지 위치 부드럽게 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 다시 코루틴을 시작하여 이미지가 올라가는 부분을 반복합니다.
        StartCoroutine(JewelFloatUp());
    }
}
