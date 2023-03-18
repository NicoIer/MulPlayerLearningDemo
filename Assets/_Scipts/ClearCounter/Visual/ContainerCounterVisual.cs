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
        }

        private void OnDisable()
        {
            _containerCounter.OnInteractEvent -= OnInteractEvent;
        }

        private void OnInteractEvent(object sender, EventArgs e)
        {
            _animator.SetTrigger(_openParam);
        }
    }
}