using UnityEngine;

namespace Kitchen
{
    public partial class Player
    {
        private void Update()
        {
            if (_playerInput.move != Vector2.zero)
            {
                var moveDir = new Vector3(_playerInput.move.x, 0, _playerInput.move.y);
                transform.position += playerData.speed * moveDir * Time.deltaTime;
                
                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*playerData.rotateSpeed);
            }
        }
    }
}