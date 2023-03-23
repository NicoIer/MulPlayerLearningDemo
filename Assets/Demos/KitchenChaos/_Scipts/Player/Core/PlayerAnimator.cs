using System;
using Unity.Netcode;
using UnityEngine;

namespace Kitchen.Player
{
    public class PlayerAnimator : NetworkBehaviour
    {
        private Player _player;
        private Animator _animator;

        private int _walking;
        

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _player = GetComponentInParent<Player>();
            _animator = GetComponent<Animator>();
            _walking = Animator.StringToHash(_player.data.animWalking);
            
            _player.MoveController.OnStartMove += _OnStartMove;
            _player.MoveController.OnStopMove += _OnStopMove;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            _player.MoveController.OnStartMove -= _OnStartMove;
            _player.MoveController.OnStopMove -= _OnStopMove;
        }
        private void _OnStopMove()
        {
            _animator.SetBool(_walking, false);
        }

        private void _OnStartMove()
        {
            _animator.SetBool(_walking, true);
        }
    }
}