using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private PrizeCatcher catcher;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject uiBox;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private Coroutine animCoroutine;

        private void Awake()
        {
            if (catcher == null) catcher = FindObjectOfType<PrizeCatcher>();
            if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            if (catcher != null) catcher.OnWin += HandleWinEvent;
            if (gameManager != null) gameManager.StateChanged += HandleGameStateChange;
        }

        private void OnDisable()
        {
            if (catcher != null) catcher.OnWin -= HandleWinEvent;
            if (gameManager != null) gameManager.StateChanged -= HandleGameStateChange;
        }

        private void HandleWinEvent(Prize prize)
        {
            if (text != null) text.text = $"YOU WON!\n{prize.name}";
            if (image != null && prize.image != null)
            {
                image.sprite = prize.image;
                image.enabled = true;
            }

            if (uiBox != null)
            {
                uiBox.SetActive(true);
                if (animCoroutine != null) StopCoroutine(animCoroutine);
                animCoroutine = StartCoroutine(AnimateWinPopup());
            }
        }

        private void HandleGameStateChange(GameState state)
        {
            if (state == GameState.Idle || state == GameState.Running)
            {
                if (uiBox != null)
                {
                    uiBox.SetActive(false);
                }
                if (animCoroutine != null)
                {
                    StopCoroutine(animCoroutine);
                    animCoroutine = null;
                }
            }
        }

        private IEnumerator AnimateWinPopup()
        {
            Transform boxTransform = uiBox.transform;
            boxTransform.localScale = Vector3.zero;

            // Phase 1: Pop up overshoot scale (0 -> 1.15)
            float duration = 0.35f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                // Elastic / overshoot curve
                float scale = Mathf.Sin(t * Mathf.PI * 0.75f) * 1.2f;
                boxTransform.localScale = Vector3.one * scale;
                yield return null;
            }

            // Phase 2: Settle to 1.0
            elapsed = 0f;
            duration = 0.15f;
            Vector3 startScale = boxTransform.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                boxTransform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
                yield return null;
            }

            boxTransform.localScale = Vector3.one;

            // Phase 3: Continuous subtle pulse
            while (true)
            {
                float pulse = 1.0f + 0.04f * Mathf.Sin(Time.time * 4f);
                boxTransform.localScale = Vector3.one * pulse;
                yield return null;
            }
        }
    }
}