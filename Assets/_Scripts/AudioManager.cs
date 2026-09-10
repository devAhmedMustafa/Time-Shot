using UnityEngine;

namespace _Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PrizeCatcher catcher;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip startClip;
        [SerializeField] private AudioClip stopClip;
        [SerializeField] private AudioClip winClip;

        private void Awake()
        {
            if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
            if (catcher == null) catcher = FindObjectOfType<PrizeCatcher>();

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            LoadResources();
        }

        private void LoadResources()
        {
            if (startClip == null) startClip = Resources.Load<AudioClip>("Sfx/beep");
            if (stopClip == null) stopClip = Resources.Load<AudioClip>("Sfx/beep");
            if (winClip == null) winClip = Resources.Load<AudioClip>("Sfx/win");
        }

        private void OnEnable()
        {
            if (gameManager != null) gameManager.StateChanged += HandleGameStateChanged;
            if (catcher != null) catcher.OnWin += HandleWin;
        }

        private void OnDisable()
        {
            if (gameManager != null) gameManager.StateChanged -= HandleGameStateChanged;
            if (catcher != null) catcher.OnWin -= HandleWin;
        }

        private void HandleGameStateChanged(GameState state)
        {
            if (state == GameState.Running)
            {
                PlayStartSfx();
            }
            else if (state == GameState.Played)
            {
                PlayStopSfx();
            }
        }

        private void HandleWin(Prize prize)
        {
            PlayWinSfx();
        }

        public void PlayStartSfx()
        {
            if (audioSource != null && startClip != null)
            {
                audioSource.pitch = 1.0f;
                audioSource.PlayOneShot(startClip);
            }
        }

        public void PlayStopSfx()
        {
            if (audioSource != null && stopClip != null)
            {
                // Slightly higher pitch for stop action
                audioSource.pitch = 1.2f;
                audioSource.PlayOneShot(stopClip);
            }
        }

        public void PlayWinSfx()
        {
            if (audioSource != null && winClip != null)
            {
                audioSource.pitch = 1.0f;
                audioSource.PlayOneShot(winClip);
            }
        }
    }
}
