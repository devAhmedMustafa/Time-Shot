using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    class WinUI : MonoBehaviour
    {
        [SerializeField] private PrizeCatcher catcher;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject uiBox;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private void OnEnable()
        {
            catcher.OnWin += HandleWinEvent;
        }

        private void OnDisable() {
            catcher.OnWin -= HandleWinEvent;
        }

        private void HandleWinEvent(Prize prize)
        {
            text.text = $"You've won a {prize.name}";
            uiBox.SetActive(true);
        }
        
        private void HandleGameStateChange(GameState state)
        {
            if (state == GameState.Idle)
            {
                uiBox.SetActive(false);
            }
        }
    }
}