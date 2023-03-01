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
                _pmovcon.EnableMovement = false;
                TriggerAttack();
                _attackLeft = !_attackLeft;
            }
        }
        //Enables movement when attack animation finishes
        public void OnAttackFinished()
        {
            _pmovcon.EnableMovement = true;
        }

        private void TriggerAttack()
        {
            _animator.SetTrigger(_attackLeft ? "PunchL" : "PunchR");
        }
    }
}