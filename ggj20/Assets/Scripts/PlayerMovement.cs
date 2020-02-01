using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] private int speed = 4;
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;

        private PlayerInput _playerInput;
        private Rigidbody2D _rigidBody2D;
        private Vector2 _movementInput;
        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _playerInput = new PlayerInput();
            _playerInput.PlayerControls.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            var moveX = _movementInput.x * speed;
            var moveY = _movementInput.y * speed;

            Vector3 targetVelocity = new Vector2(moveX, moveY);
            _rigidBody2D.velocity =
                Vector3.SmoothDamp(_rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        private void OnEnable()
        {
            _playerInput?.Enable();
        }

        private void OnDisable()
        {
            _playerInput?.Disable();
        }
    }
}