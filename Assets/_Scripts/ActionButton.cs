using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    
    class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameManager _gameManager;

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
                _button.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
            }
            else if (state == GameState.Running) {
                _button.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
            }
            else if (state == GameState.Played || state == GameState.Overflow)
            {
                _button.GetComponentInChildren<TextMeshProUGUI>().text = "Play Again";
            }
        }
    }

}