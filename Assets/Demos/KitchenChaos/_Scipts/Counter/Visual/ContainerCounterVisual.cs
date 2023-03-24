using System;
using UnityEngine;

namespace Kitchen
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        private ContainerCounter _containerCounter;
        private Animator _animator;
        private static readonly int _openParam = Animator.StringToHash("open");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _containerCounter = GetComponentInParent<ContainerCounter>();
            
        }

        private void OnEnable()
        {
            _containerCounter.OnInteractEvent += OnInteractEvent;
            Debug.Log("注册事件");
        }

        private void OnDisable()
        {
            _containerCounter.OnInteractEvent -= OnInteractEvent;
            Debug.Log("注销事件");
        }

        private void OnInteractEvent()
        {
            Debug.Log("交互事件触发 设置动画");
            _animator.SetTrigger(_openParam);
        }
    }
}