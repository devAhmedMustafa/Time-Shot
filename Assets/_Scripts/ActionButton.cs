using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameManager _gameManager;

        private readonly Color goldenAmber = new Color(0.929f, 0.686f, 0.298f, 1f);  // #EDAF4C
        private readonly Color darkNavyText = new Color(0.043f, 0.035f, 0.122f, 1f); // #0B091F

        private void Start()
        {
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (_button != null)
            {
                Image bgImage = _button.GetComponent<Image>();
                if (bgImage != null)
                {
                    bgImage.color = goldenAmber;
                }

                ColorBlock cb = _button.colors;
                cb.normalColor = goldenAmber;
                cb.highlightedColor = new Color(0.95f, 0.73f, 0.35f, 1f);
                cb.pressedColor = new Color(0.83f, 0.60f, 0.23f, 1f);
                _button.colors = cb;

                TextMeshProUGUI label = _button.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null)
                {
                    label.color = darkNavyText;
                    label.fontStyle = FontStyles.Bold;
                }
            }
        }

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
            if (_button == null) return;
            TextMeshProUGUI label = _button.GetComponentInChildren<TextMeshProUGUI>();
            if (label == null) return;

            if (state == GameState.Idle)
            {
                label.text = "START";
            }
            else if (state == GameState.Running) {
                label.text = "STOP";
            }
            else if (state == GameState.Played || state == GameState.Overflow)
            {
                label.text = "PLAY AGAIN";
            }
        }
    }
}
