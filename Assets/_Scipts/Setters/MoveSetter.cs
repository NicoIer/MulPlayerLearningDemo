using UnityEngine;

namespace Kitchen
{
    public static class MoveSetter
    {
        public static void Move(Transform transform,Vector3 moveDir, float speed)
        {
            transform.position += speed * moveDir * Time.deltaTime;
        }
    }
}