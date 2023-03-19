using System;
using UnityEngine;

namespace Kitchen.Player
{
    public class PlayerMoveController : PlayerController
    {
        private Transform transform => player.transform;
        private readonly Animator _animator;
        private readonly PlayerInput _playerInput;
        private readonly PlayerData _data;
        private readonly int _walking;

        public event Action OnStartMove;
        public event Action OnStopMove;
        public Action OnMoving;

        public PlayerMoveController(Player player) : base(player)
        {
            _animator = player.animator;
            _data = player.data;
            _playerInput = player.input;
            _walking = Animator.StringToHash(_data.animWalking);
        }

        private bool _isMoving;

        public override void Update()
        {
            if (_playerInput.move != Vector2.zero)
            {
                var inputDir = new Vector3(_playerInput.move.x, 0, _playerInput.move.y);
                TransformSetter.Move(transform, GetMoveDirection(), _data.speed);
                TransformSetter.SetForward(transform, inputDir, _data.rotateSpeed);
                //如果没在移动状态，就触发开始移动事件
                OnMoving?.Invoke();
                if (!_isMoving)
                {
                    _animator.SetBool(_walking, true);
                    OnStartMove?.Invoke();
                    _isMoving = true;
                }
            }
            else
            {
                _isMoving = false;
                OnStopMove?.Invoke();
                _animator.SetBool(_walking, false);
            }
        }

        private Vector3 GetMoveDirection()
        {
            //ToDo 太丑了，需要优化
            //尝试按照输入进行移动
            var moveDir = new Vector3(_playerInput.move.x, 0, _playerInput.move.y);
            var position = transform.position;
            var canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDir, _data.playerRadius);
            if (canMove) return moveDir;

            //尝试在X方向移动
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDirX, _data.playerRadius);
            if (canMove)
            {
                return moveDirX.normalized;
            }

            //尝试在Z方向移动
            var moveDirZ = new Vector3(0, 0, moveDir.z);
            canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDirZ, _data.playerRadius);
            moveDir = canMove ? moveDirZ : Vector3.zero;

            return moveDir.normalized;
        }
    }
}