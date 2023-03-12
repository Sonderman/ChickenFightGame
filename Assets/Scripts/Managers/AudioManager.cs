using ScriptableObjects.Managers;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private AudioSource _audioSource;
        private ClipsDataSo _clipsDataSo;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _clipsDataSo = Locator.Instance.clipsDataSo;
        }

        public void PlayPunchClip()
        {
            _audioSource.PlayOneShot(_clipsDataSo.puchClip, 1f);
        }

        public void PlayPlayerDamageTaken()
        {
            _audioSource.PlayOneShot(_clipsDataSo.playerDamageTakenClip, 1f);
        }

        public void PlayTakingScoreClip()
        {
            _audioSource.PlayOneShot(_clipsDataSo.takingScoreClip, 1f);
        }

        public void PlayLevelFailedClip()
        {
            _audioSource.PlayOneShot(_clipsDataSo.levelFailClip, 1f);
        }
        public void PlayLevelComplatedClip()
        {
            _audioSource.PlayOneShot(_clipsDataSo.levelCompleteClip, 1f);
        }
    }
}