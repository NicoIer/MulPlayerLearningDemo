using UnityEngine;

namespace Nico
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData: ScriptableObject
    {
        public float speed=7f;
        public float rotateSpeed = 7f;
        public float playerRadius = 1f;
        public float playerHeight = 2f;
        public string animWalking = "walking";
        public float interactDistance = 1f;
        public LayerMask interactLayer;
    }
}