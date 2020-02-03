using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] private int speed = 4;
        [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
        [SerializeField] private Transform aiPathTarget;

        private AIPath aiPath;
        private Rigidbody2D _rigidBody2D;
        private PlayerInput _playerInput;
        private Vector2 _movementInput;
        private Vector3 velocity = Vector3.zero;
        private float moveX;
        private float moveY;
        

        private void Awake()
        {
            aiPath = GetComponent<AIPath>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _playerInput = new PlayerInput();
            _playerInput.PlayerControls.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerInput.PlayerControls.Exit.performed += ctx => ExitGame();
            _playerInput.Enable();
            aiPath.enabled = false;
        }

        private void ExitGame()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }

        private void Update()
        {
            UpdatePathfinding();
            UpdateMouseMovement();
            UpdateTouchMovement();
            Debug.Log("Movement" + _movementInput.x);
            moveX = _movementInput.x * speed;
            moveY = _movementInput.y * speed;

            Vector3 targetVelocity = new Vector2(moveX, moveY);
            _rigidBody2D.velocity =
                Vector3.SmoothDamp(_rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            /*if (moveX > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (moveX < 0)
            {
                _spriteRenderer.flipX = false;
            }*/

        }

        private void UpdatePathfinding()
        {
            if (aiPath.enabled && aiPath.reachedEndOfPath)
            {
                aiPath.enabled = false;
            }
        }

        private void UpdateMouseMovement()
        {
            if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame || Camera.main == null)
            {
                return;
            }

            var mousePosition2d = Mouse.current.position.ReadValue();
            SetMovementTarget(mousePosition2d);
        }

        private void SetMovementTarget(Vector2 mousePosition2d)
        {
            if (Camera.main == null)
            {
                return;
            }

            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition2d);
            var raycastHit2d = Physics2D.Raycast(
                new Vector2(screenToWorldPoint.x, screenToWorldPoint.y),
                Vector2.zero,
                0f);
            if (raycastHit2d)
            {
                Debug.Log(raycastHit2d.point);
                aiPathTarget.position = raycastHit2d.point;
                aiPath.enabled = true;
            }
        }

        private void UpdateTouchMovement()
        {
            if (Touchscreen.current == null || !Touchscreen.current.wasUpdatedThisFrame || Camera.main == null)
            {
                return;
            }

            var mousePosition2d = Touchscreen.current.primaryTouch.position.ReadValue();
            SetMovementTarget(mousePosition2d);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Contains("AStarTarget"))
            {
                aiPath.enabled = false;
            }
        }

        private void OnEnable()
        {
            // _playerInput?.Enable();
        }

        private void OnDisable()
        {
            // _playerInput?.Disable();
        }

        public bool IsIdle()
        {
            return _movementInput == Vector2.zero && !aiPath.enabled;
        }

        
    }
}