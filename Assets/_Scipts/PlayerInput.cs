using UnityEngine;

namespace Kitchen
{
    public class PlayerInput
    {
        public Vector2 move => _standerInput.Player.move2D.ReadValue<Vector2>();
        private readonly StanderInput _standerInput;

        public PlayerInput()
        {
            _standerInput = new StanderInput();
        }
        public void Enable()
        {
            _standerInput.Enable();
        }
        public void Disable()
        {
            _standerInput.Disable();
        }
    }
}