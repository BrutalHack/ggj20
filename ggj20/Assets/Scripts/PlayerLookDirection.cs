using Pathfinding;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class PlayerLookDirection : MonoBehaviour
    {
        private Vector2 _lastFramePosition = Vector2.zero;
        private Animator _animator;
        private bool _idle = true;
        private WatchDirection _watchDirection = WatchDirection.SouthWest;
        private PlayerMovement _playerMovement;
        private AIPath aiPath;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            aiPath = GetComponent<AIPath>();
        }

        private void Update()
        {
            UpdateLookDirection();
        }

        /// <summary>
        ///      +Y
        ///       |
        /// -X ---+---- +X
        ///       |
        ///      -Y
        /// </summary>
        private void UpdateLookDirection()
        {
            var currentPosition = new Vector2(transform.position.x, transform.position.y);
            var direction = currentPosition - _lastFramePosition;
            var newWatchDirection = CalculateWatchDirection(direction);
            var newIdle = _playerMovement.enabled ? _playerMovement.IsIdle() : !aiPath.enabled;


            _lastFramePosition = currentPosition;

            if (_watchDirection != newWatchDirection ||
                _idle != newIdle)
            {
                _watchDirection = newWatchDirection;
                _idle = newIdle;

                _animator.SetBool("Idle", _idle);
                _animator.SetTrigger(_watchDirection.ToString());

                Debug.Log($"{_watchDirection} {_idle}");
            }
        }

        private WatchDirection CalculateWatchDirection(Vector2 direction)
        {
            var normalizedDirection = direction.normalized;

            if (normalizedDirection.x > 0)
            {
                if (normalizedDirection.y > 0)
                {
                    //TODO decision North || East
                    return WatchDirection.NorthEast;
                }
                else if (normalizedDirection.y < 0)
                {
                    //TODO decision South || East
                    return WatchDirection.SouthEast;
                }
                else // y == 0
                {
                    return WatchDirection.East;
                }
            }
            else if (normalizedDirection.x < 0)
            {
                if (normalizedDirection.y > 0)
                {
                    //TODO decision North || West
                    return WatchDirection.NorthWest;
                }
                else if (normalizedDirection.y < 0)
                {
                    //TODO decision South || West
                    return WatchDirection.SouthWest;
                }
                else // y == 0
                {
                    return WatchDirection.West;
                }
            }
            else // x == 0
            {
                if (normalizedDirection.y > 0)
                {
                    return WatchDirection.North;
                }
                else if (normalizedDirection.y < 0)
                {
                    return WatchDirection.South;
                }
                else // y == 0
                {
                    return _watchDirection;
                }
            }
        }


        public void ForceIdle()
        {
            _idle = true;

            _animator.SetBool("Idle", _idle);
            _animator.SetTrigger(_watchDirection.ToString());
        }
    }
}