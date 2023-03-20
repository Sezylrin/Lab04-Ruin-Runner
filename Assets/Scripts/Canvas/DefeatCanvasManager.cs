using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefeatCanvasManager : MonoBehaviour
{
    [SerializeField] private RectTransform playerRectTransform;
    [SerializeField] private TMP_Text mainMenuTxt;
    [SerializeField] private TMP_Text retryLvlTxt;

    private int selectedOptionIndex = 0;
    private Vector2[] optionPositions = new Vector2[2];
    private bool animating = false;
    private AudioSource audioSource;

    private void Awake()
    {
        optionPositions[0] = new Vector2(playerRectTransform.anchoredPosition.x, playerRectTransform.anchoredPosition.y);
        optionPositions[1] = new Vector2(playerRectTransform.anchoredPosition.x, -62f);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !animating)
        {
            animating = true;
            selectedOptionIndex = selectedOptionIndex == 0 ? 1 : 0;
            StartCoroutine(MovePlayerSprite(playerRectTransform.anchoredPosition, optionPositions[selectedOptionIndex]));
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !animating)
        {
            animating = true;
            selectedOptionIndex = selectedOptionIndex == 1 ? 0 : 1;
            StartCoroutine(MovePlayerSprite(playerRectTransform.anchoredPosition, optionPositions[selectedOptionIndex]));
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedOptionIndex == 0)
            {
                audioSource.Play();
                StartCoroutine(ScaleText(retryLvlTxt, 0.2f, 1.11f));
                Scene sceneToLoad = HelperFunctions.GetNextScene(GameManager.Instance.level);
                StartCoroutine(LoadLevel(audioSource.clip.length - 1.0f, sceneToLoad));
            }
            else
            {
                audioSource.Play();
                StartCoroutine(ScaleText(mainMenuTxt, 0.2f, 1.11f));
                StartCoroutine(LoadLevel(audioSource.clip.length - 1.0f, Scene.MainMenu));
            }
        }
    }

    private IEnumerator MovePlayerSprite(Vector2 from, Vector2 to)
    {
        const float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            playerRectTransform.anchoredPosition = Vector2.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerRectTransform.anchoredPosition = to;
        animating = false;
    }

    private IEnumerator LoadLevel(float seconds, Scene scene)
    {
        yield return new WaitForSeconds(seconds);
        Loader.Load(scene);
    }

    private IEnumerator ScaleText(TMP_Text text, float duration, float targetScale)
    {
        Vector3 originalScale = text.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            text.transform.localScale = Vector3.Lerp(originalScale, Vector3.one * targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = Vector3.one * targetScale;

        // Scale down the text
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            text.transform.localScale = Vector3.Lerp(Vector3.one * targetScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = originalScale;
    }
}
