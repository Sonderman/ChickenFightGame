using Managers;
using ScriptableObjects;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerScriptableObj _playerSo;
        private GameManagerSo _gameManagerSo;
        public Transform mainCamTransform;
        private Vector2 _movementInput;
        internal bool IsMoving;
        internal bool IsMoveAllowed;
        internal bool NotInGame;
        private Rigidbody _rigidbody;
        private float _turnSmoothVelocity;

        private void Awake()
        {
            _playerSo = Locator.Instance.playerSo;
            _gameManagerSo = Locator.Instance.gameManagerSo;
        }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            IsMoveAllowed = true;
            _gameManagerSo.OnStateChanged += ToggleMovementAndMouseLook;
        }

        private void Update()
        {
            //Checks movement for movement animation
            IsMoving = _movementInput.magnitude >= 1f;
        }

        void FixedUpdate()
        {
            if (_movementInput.magnitude >= 1f)
            {
                float targetAngle = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg +
                                    mainCamTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    0.1f);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (IsMoveAllowed)
                {
                    _rigidbody.MovePosition(moveDir.normalized * (_playerSo.speed) / 10 + transform.position);
                }
            }
        }

        //Reads Inputs
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
            if (!NotInGame)
            {
                _movementInput = input;
            }
        }

        private void ToggleMovementAndMouseLook(GameManagerSo.GameStates state)
        {
            if (state == GameManagerSo.GameStates.InGame)
            {
                NotInGame = false;
            }
            else
            {
                NotInGame = true;
            }
            
        }
    }
}