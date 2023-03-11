using ScriptableObjects;
using ScriptableObjects.Player;
using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerScriptableObj _playerSo;
        private GameManagerSo _gameManagerSo;

        private void Awake()
        {
            _playerSo = Locator.Instance.playerSo;
            _gameManagerSo = Locator.Instance.gameManagerSo;
            _playerSo.Initialize();
        }

        private void Start()
        {
            _gameManagerSo.OnEnemyKilled += TakeScore;
        }

        public void OnDamageTaken(float damage)
        {
            _playerSo.health -= damage;
            if (_playerSo.health <= 0f)
            {
                _playerSo.OnDie?.Invoke();
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            _playerSo.OnUIUpdateNeeded?.Invoke();
        }

        private void TakeScore(float score)
        {
            _playerSo.killScore += score;
            _playerSo.OnUIUpdateNeeded?.Invoke();
        }

        public void Die()
        {
            Destroy(gameObject);
            _gameManagerSo.ChangeState(GameManagerSo.GameStates.GameOver);
        }
    }
}