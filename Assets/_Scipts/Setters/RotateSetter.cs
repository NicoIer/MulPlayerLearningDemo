using UnityEngine;

namespace Kitchen
{
    public static class RotateSetter
    {
        public static void SetForward(Transform transform, Vector3 moveDir, float rotateSpeed)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
}