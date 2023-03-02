using ScriptableObjects;
using ScriptableObjects.Player;
using UnityEngine;


namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerScriptableObj playerSo;
        public GameManagerSo gameManagerSo;
        
        private void Start()
        {
            playerSo.InitializePlayer();
            gameManagerSo.OnEnemyKilled += TakeScore;
        }

        public void OnDamageTaken(float damage)
        {
            playerSo.health -= damage;
            if (playerSo.health <= 0f)
            {
                playerSo.onDie?.Invoke();
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
            playerSo.onUIUpdateNeeded?.Invoke();
        }

        private void TakeScore(float score)
        {
            playerSo.killScore += score;
            playerSo.onUIUpdateNeeded?.Invoke();
        }

        public void Die()
        {
            Destroy(gameObject);
            gameManagerSo.ChangeState(GameManagerSo.GameStates.GameOver);
        }
    }
}
