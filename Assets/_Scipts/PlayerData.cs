using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData: ScriptableObject
    {
        public float speed=7f;
        public float rotateSpeed = 7f;
    }
}