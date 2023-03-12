using ScriptableObjects;
using ScriptableObjects.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
         private  GameManagerSo _gameManagerSo;
         private  GlobalSettings _globalSettings;
         private  EnemyScriptableObj _enemySo;
         public GameObject enemyPrefab;
         private int _activeEnemyCount;

         private void Awake()
         {
             _gameManagerSo = Locator.Instance.gameManagerSo;
             _globalSettings = Locator.Instance.globalSettingsSo;
             _enemySo = Locator.Instance.enemySo;
             _gameManagerSo.Reset();
             _enemySo.Reset();
         }

         private void Start()
        {
            _gameManagerSo.currentGameState = GameManagerSo.GameStates.InGame;
            SpawnEnemies(_globalSettings.enemyAmount);
            _gameManagerSo.OnEnemyKilled += WinCheck;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasReleasedThisFrame)
            {
                if (_gameManagerSo.currentGameState == GameManagerSo.GameStates.Pause)
                {
                    ResumeGame();
                }
                else
                {
                    _gameManagerSo.ChangeState(GameManagerSo.GameStates.Pause);
                    Time.timeScale = 0f;
                }
            }
        }

        public void ResumeGame()
        {
            _gameManagerSo.ChangeState(GameManagerSo.GameStates.InGame);
            Time.timeScale = 1f;
        }

        private void SpawnEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(enemyPrefab, Utilities.GetRandomTargetPosition(Locator.Instance.enemySo.aiNavigationRange.x, transform.position.y,
                    Locator.Instance.enemySo.aiNavigationRange.z), Quaternion.identity);
            }

            _activeEnemyCount = amount;
        }

        private void WinCheck(float _)
        {
            if (--_activeEnemyCount <= 0)
            {
                AudioManager.Instance.PlayLevelComplatedClip();
                _gameManagerSo.ChangeState(GameManagerSo.GameStates.LevelEnd);
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadNextLevel()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if(SceneManager.sceneCount > currentIndex)
                SceneManager.LoadScene(currentIndex+1);
        }
        
    }
}