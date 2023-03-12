using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameManagerSO", menuName = "ScriptableObjects/GameManager")]
    public class GameManagerSo : ScriptableObject
    {
        public enum GameStates
        {
            InGame,
            Pause,
            GameOver,
            LevelEnd
        }
        public GameStates currentGameState;

        [Header("Events")] 
        public UnityAction<GameStates> OnStateChanged;
        public UnityAction<float> OnEnemyKilled;
        
        public void ChangeState(GameStates state)
        {
            currentGameState = state;
            OnStateChanged?.Invoke(state);
        }

        public void Reset()
        {
            OnStateChanged = null;
            OnEnemyKilled = null;
        }
    }
}