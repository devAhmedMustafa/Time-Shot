using System;
using UnityEngine;

namespace _Scripts
{
    class PrizeCatcher : MonoBehaviour
    {
        [SerializeField] private PrizePool pool;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TimeManager timeManager;

        private Prize? _prizeWon;
        
        [SerializeField] private bool hasWon;

        public event Action<Prize> OnWin;

        void OnEnable()
        {
            gameManager.StateChanged += HandleGameStateChanged;
        }

        void OnDisable()
        {
            gameManager.StateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.Played)
            {
                _prizeWon = pool.GetPrize(timeManager.timer);
                hasWon = _prizeWon != null;

                if (_prizeWon != null) OnWin?.Invoke(_prizeWon); 
            }
            else
            {
                _prizeWon = null;
            }
        }
    }
}