using UnityEngine;

namespace Kitchen
{
    public static class TransformSetter
    {
        public static void Move(Transform transform,Vector3 moveDir, float speed)
        {
            transform.position += speed * moveDir * Time.deltaTime;
        }
        public static void SetForward(Transform transform, Vector3 moveDir)
        {
            transform.forward = moveDir;
        }
        public static void SetForward(Transform transform, Vector3 moveDir, float setSpeed)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * setSpeed);
        }
    }
}