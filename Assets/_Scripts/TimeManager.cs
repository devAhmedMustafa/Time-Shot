using UnityEngine;

namespace _Scripts {
    
    public class TimeManager : MonoBehaviour {
        public uint timer;
        private bool _running;

        [SerializeField] private GameManager gameManager;

        void OnEnable()
        {
            if (gameManager != null)
            {
                gameManager.StateChanged += HandleGameStateChange;
            }
        }

        void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.StateChanged -= HandleGameStateChange;
            }
        }

        private void FixedUpdate() {
            if (_running)
            {
                timer += (uint)(Time.fixedDeltaTime * 1000f);
                if (timer > 60000)
                {
                    gameManager.TriggerOverflow();
                    _running = false;
                }
            }
        }

        private void HandleGameStateChange(GameState state)
        {
            if (state == GameState.Running)
            {
                StartTimer();
            }
            else if (state == GameState.Played) {
                StopTimer();
            }
            else if (state == GameState.Idle)
            {
                Reset();
            }
        }

        public void StartTimer()
        {
            _running = true;
        }

        public void StopTimer()
        {
            _running = false;
        }

        public void Reset()
        {
            timer = 0;
        }
    }
}