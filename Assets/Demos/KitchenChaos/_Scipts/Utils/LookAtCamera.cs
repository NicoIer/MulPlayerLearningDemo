using System;
using UnityEngine;

namespace Kitchen
{
    internal enum LookAtUpdateEnum
    {
        LateUpdate,
        Update,
        FixedUpdate
    }

    internal enum LookMode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] internal LookMode lookMode;

        private void LateUpdate()
        {
            switch (lookMode)
            {
                case LookMode.LookAt:
                    transform.LookAt(Camera.main.transform);
                    break;
                case LookMode.LookAtInverted:
                    transform.LookAt(transform.position - Camera.main.transform.position);
                    break;

                case LookMode.CameraForward:
                    transform.forward = Camera.main.transform.forward;
                    break;
                case LookMode.CameraForwardInverted:
                    transform.forward = -Camera.main.transform.forward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}