using System;
using Nico.Job;
using UnityEngine;

namespace Kitchen.Player
{
    public class PlayerMoveController : PlayerController
    {
        // private readonly Animator _animator;
        private readonly PlayerInput _playerInput;

        private readonly PlayerData _data;
        // private readonly int _walking;

        public event Action OnStartMove;
        public event Action OnStopMove;
        public Action<Vector3> onMoving;

        public PlayerMoveController(Player player) : base(player)
        {
            _data = player.data;
            _playerInput = player.input;
        }

        private bool _isMoving;

        public override void Update()
        {
            if (_playerInput.move != Vector2.zero)
            {
                var inputDir = new Vector3(_playerInput.move.x, 0, _playerInput.move.y);
                MoveNormal(inputDir);
                //如果没在移动状态，就触发开始移动事件
                onMoving?.Invoke(Owner.transform.position);
                if (!_isMoving)
                {
                    OnStartMove?.Invoke();
                    _isMoving = true;
                }
            }
            else
            {
                _isMoving = false;
                OnStopMove?.Invoke();
            }
        }

        private void MoveNormal(Vector3 inputDir)
        {
            TransformWorker.Move(new[] { Owner.transform }, new[] { GetMoveDirection() }, _data.speed);
            TransformWorker.SetForward(new[] { Owner.transform }, new[] { inputDir }, _data.rotateSpeed);
        }


        private Vector3 GetMoveDirection()
        {
            //ToDo 太丑了，需要优化
            //尝试按照输入进行移动
            var moveDir = new Vector3(_playerInput.move.x, 0, _playerInput.move.y);
            var position = Owner.transform.position;
            var canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDir, _data.playerRadius, _data.collisionLayer);
            if (canMove) return moveDir;

            //尝试在X方向移动
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDirX, _data.playerRadius, _data.collisionLayer);
            if (canMove)
            {
                return moveDirX.normalized;
            }

            //尝试在Z方向移动
            var moveDirZ = new Vector3(0, 0, moveDir.z);
            canMove = !Physics.CapsuleCast(position,
                position + Vector3.up * _data.playerHeight,
                _data.playerRadius, moveDirZ, _data.playerRadius, _data.collisionLayer);
            //TODO 打磨碰撞检测
            // canMove = !Physics.BoxCast(position,
            // Vector3.one*_data.playerRadius, moveDirZ, Quaternion.identity, _data.collisionLayer);
            moveDir = canMove ? moveDirZ : Vector3.zero;

            return moveDir.normalized;
        }
    }
}