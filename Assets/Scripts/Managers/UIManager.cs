using ScriptableObjects;
using ScriptableObjects.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private GameManagerSo _gameManagerSo;
        private PlayerScriptableObj _playerSo;
        [Header("InGameUI")] public GameObject inGamePanel;
        public TextMeshProUGUI inGameScoreText;
        public Slider healthBarSlider;
        public Gradient healthBarGradient;
        public Image healthBarImage;
        [Header("GamePauseUI")] public GameObject pausePanel;
        [Header("GameOverUI")] public GameObject gameOverPanel;
        public TextMeshProUGUI gameOverScoreText;
        [Header("LevelEndUI")] public GameObject levelEndPanel;
        public TextMeshProUGUI levelEndScoreText;

        private void Awake()
        {
            _gameManagerSo = Locator.Instance.gameManagerSo;
            _playerSo = Locator.Instance.playerSo;
        }

        private void Start()
        {
            healthBarSlider.maxValue = _playerSo.maxHealth;
            healthBarSlider.value = _playerSo.maxHealth;
            healthBarImage.color = healthBarGradient.Evaluate(1f);
            _playerSo.OnUIUpdateNeeded += UpdateInGameUI;
            _gameManagerSo.OnStateChanged += OnStateChange;
            inGamePanel.SetActive(true);
            UpdateInGameUI();
        }

        private void UpdateInGameUI()
        {
            inGameScoreText.text = _playerSo.killScore.ToString();
            healthBarSlider.value = _playerSo.health;
            healthBarImage.color = healthBarGradient.Evaluate(healthBarSlider.normalizedValue);
        }

        private void OnStateChange(GameManagerSo.GameStates state)
        {
            switch (state)
            {
                case GameManagerSo.GameStates.InGame:
                    OnInGameUI();
                    break;
                case GameManagerSo.GameStates.Pause:
                    OnPauseUI();
                    break;
                case GameManagerSo.GameStates.GameOver:
                    OnGameOverUI();
                    break;
                case GameManagerSo.GameStates.LevelEnd:
                    OnLevelEndUI();
                    break;
            }
        }

        private void OnInGameUI()
        {
            pausePanel.SetActive(false);
            inGamePanel.SetActive(true);
        }

        private void OnPauseUI()
        {
            gameOverPanel.SetActive(false);
            pausePanel.SetActive(true);
        }

        private void OnGameOverUI()
        {
            inGamePanel.SetActive(false);
            gameOverPanel.SetActive(true);
            gameOverScoreText.text = _playerSo.killScore.ToString();
        }

        private void OnLevelEndUI()
        {
            inGamePanel.SetActive(false);
            levelEndPanel.SetActive(true);
            levelEndScoreText.text = _playerSo.killScore.ToString();
        }
    }
}