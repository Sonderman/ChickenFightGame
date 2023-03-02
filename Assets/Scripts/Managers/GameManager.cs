using ScriptableObjects;
using ScriptableObjects.Enemy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
         public GameManagerSo gameManagerSo;
         public GlobalSettings globalSettings;
         public GameObject enemyPrefab;
         public EnemyScriptableObj enemySo;
         private int _activeEnemyCount;

        private void Start()
        {
            gameManagerSo.currentGameState = GameManagerSo.GameStates.InGame;
            SpawnEnemies(globalSettings.enemyAmount);
            gameManagerSo.OnEnemyKilled += WinCheck;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasReleasedThisFrame)
            {
                if (gameManagerSo.currentGameState == GameManagerSo.GameStates.Pause)
                {
                    ResumeGame();
                }
                else
                {
                    gameManagerSo.ChangeState(GameManagerSo.GameStates.Pause);
                    Time.timeScale = 0f;
                }
            }
        }

        public void ResumeGame()
        {
            gameManagerSo.ChangeState(GameManagerSo.GameStates.InGame);
            Time.timeScale = 1f;
        }

        private void SpawnEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(enemyPrefab, Utilities.GetRandomTargetPosition(enemySo.aiNavigationRange.x, transform.position.y,
                    enemySo.aiNavigationRange.z), Quaternion.identity);
            }

            _activeEnemyCount = amount;
        }

        private void WinCheck(float _)
        {
            if (--_activeEnemyCount <= 0)
            {
                gameManagerSo.ChangeState(GameManagerSo.GameStates.LevelEnd);
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