using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] public PlayerScriptableObj playerSio;
        [SerializeField] public Transform mainCamTransform;
        private Vector3 _movementInput;
        internal bool IsMoving;

        //private Keyboard _keyboard = Keyboard.current;
        private Rigidbody _rigidbody;
        private float _turnSmoothVelocity;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_movementInput.magnitude >= 1f)
            {
                IsMoving = true;
            }
            else
                IsMoving = false;
        }

        void FixedUpdate()
        {
            if (_movementInput.magnitude >= 1f)
            {
                float targetAngle = Mathf.Atan2(_movementInput.x, _movementInput.z) * Mathf.Rad2Deg +
                                    mainCamTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    0.1f);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _rigidbody.MovePosition(moveDir.normalized * (playerSio.speed) / 10 + transform.position);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector3 move = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
            _movementInput = move;
        }
    }
}