using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameManagerSO", menuName = "ScriptableObjects/GameManager")]
    public class GameManagerSo : ScriptableObject
    {
        public enum GameStates
        {
            Menu,
            InGame,
            Pause,
            GameOver,
            LevelEnd
        }
        public GameStates currentGameState;

        public void ChangeState(GameStates state)
        {
            currentGameState = state;
            OnStateChanged?.Invoke(state);
        }

        [Header("Events")] 
        public UnityAction<GameStates> OnStateChanged;
        public UnityAction<float> OnEnemyKilled;
    }
}