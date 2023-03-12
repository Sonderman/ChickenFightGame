using System.Collections;
using Enemy;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class AttackTrigger : MonoBehaviour
    {
        private bool _attackPerforming;
        public PlayerScriptableObj playerSo;
        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if (context.started && Locator.Instance.playerSo.isAnimationsAllowed)
            {
                StartCoroutine(AttackPerformer());
            }
        }

        private IEnumerator AttackPerformer()
        {
            _attackPerforming = true;
            yield return new WaitForSeconds(0.5f);
            _attackPerforming = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Enemy"))
            {
                if (_attackPerforming)
                {
                    other.GetComponent<EnemyAIController>().OnDamageTaken(playerSo.damageGiven);
                    _attackPerforming = false;
                }
            }
        }
    }
}
