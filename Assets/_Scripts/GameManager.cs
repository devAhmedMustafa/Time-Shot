using System;
using UnityEngine;

namespace _Scripts
{
    enum GameState
    {
        Idle, Running, Played, Overflow
    }

    class GameManager : MonoBehaviour
    {
        public GameState state;

        public event Action<GameState> StateChanged; 

        private void Start() {
            state = GameState.Idle;
            StateChanged?.Invoke(state);
            Debug.Log("Game state changed to: " + state.ToString()); 
        }

        public void ProceedGame()
        {
            if (state == GameState.Idle) state = GameState.Running;
            else if (state == GameState.Running) state = GameState.Played;
            else if (state == GameState.Played) state = GameState.Idle;
            else if (state == GameState.Overflow) state = GameState.Idle;

            StateChanged?.Invoke(state);
            Debug.Log("Game state changed to: " + state.ToString()); 
        }

        public void TriggerOverflow()
        {
            state = GameState.Overflow;
            StateChanged?.Invoke(state);
            Debug.Log("Game state changed to: " + state.ToString()); 
        }
    }
}