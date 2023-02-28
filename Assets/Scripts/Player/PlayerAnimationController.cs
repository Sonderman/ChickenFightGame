using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerMovementController _pmovcon;

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
    }
}