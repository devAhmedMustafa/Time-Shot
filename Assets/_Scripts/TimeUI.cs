using System;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    class TimeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private Color normalColor, errorColor;

        private void OnEnable() {
            if (_gameManager != null)
            {
                _gameManager.StateChanged += HandleGameStateChanged;
            }
        }

        private void OnDisable() {
            if (_gameManager != null)
            {
                _gameManager.StateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.Idle)
            {
                counterText.enabled = true;
                counterText.text = "00:00";
                counterText.color = normalColor;
            }
            else if (state == GameState.Running)
            {
                counterText.enabled = false;
            }
            else if (state == GameState.Played)
            {
                float currentTimeInSec = timeManager.timer / 1000.0f;
                TimeSpan span = TimeSpan.FromSeconds(currentTimeInSec);
                counterText.text = span.ToString(@"ss\:ff");
                counterText.enabled = true;
            }
            else if (state == GameState.Overflow)
            {
                counterText.enabled = true;
                counterText.text = "EXCEED 60s";
                counterText.color = errorColor;
            }
        }

    }
}