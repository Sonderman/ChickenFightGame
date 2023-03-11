using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerMovementController _pmovcon;
        private bool _attackLeft;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _pmovcon = GetComponent<PlayerMovementController>();
            Locator.Instance.playerSo.OnDie += OnDie;
        }

        private void Update()
        {
            if (_pmovcon.IsMoving)
            {
                _animator.SetBool("isRunning", true);
            }
            else
                _animator.SetBool("isRunning", false);
        }

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            
            if (context.performed)
            {
                //Disables player movement.
                _pmovcon.IsMoveAllowed = false;
                TriggerAttack();
                _attackLeft = !_attackLeft;
            }
        }
        //Enables movement when attack animation finishes
        public void OnAttackFinished()
        {
            _pmovcon.IsMoveAllowed = true;
        }

        private void TriggerAttack()
        {
            _animator.SetTrigger(_attackLeft ? "PunchL" : "PunchR");
        }

        private void OnDie()
        {
            _animator.SetTrigger("Die");
            //_pmovcon.IsMoveAllowed = false;
            //_pmovcon.NotInGame = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}