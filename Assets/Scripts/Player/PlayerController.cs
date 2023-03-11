using ScriptableObjects;
using ScriptableObjects.Player;
using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerScriptableObj playerSo;
        public GameManagerSo gameManagerSo;

        private void Awake()
        {
            playerSo.Initialize();
        }

        private void Start()
        {
            gameManagerSo.OnEnemyKilled += TakeScore;
        }

        public void OnDamageTaken(float damage)
        {
            playerSo.health -= damage;
            if (playerSo.health <= 0f)
            {
                playerSo.OnDie?.Invoke();
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            playerSo.OnUIUpdateNeeded?.Invoke();
        }

        private void TakeScore(float score)
        {
            playerSo.killScore += score;
            playerSo.OnUIUpdateNeeded?.Invoke();
        }

        public void Die()
        {
            Destroy(gameObject);
            gameManagerSo.ChangeState(GameManagerSo.GameStates.GameOver);
        }
    }
}