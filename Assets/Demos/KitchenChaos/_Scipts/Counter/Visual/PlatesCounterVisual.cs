using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        private PlatesCounter _platesCounter;
        [SerializeField] private float plateOffset = 0.1f;
        [SerializeField] private GameObject platePrefab;
        [SerializeField] private Transform spawnPoint;
        private readonly Stack<GameObject> _plates = new Stack<GameObject>();

        private void Awake()
        {
            _platesCounter = GetComponentInParent<PlatesCounter>();
        }

        private void OnEnable()
        {
            _platesCounter.OnPlateCountChanged += OnPlateCountChanged;
        }

        private void OnDisable()
        {
            _platesCounter.OnPlateCountChanged -= OnPlateCountChanged;
        }

        private void OnPlateCountChanged(int count)
        {
            if (count > _plates.Count)
            {
                _AddPlate();
            }
            else if (count < _plates.Count)
            {
                _RemovePlate();
            }
        }

        private void _AddPlate()
        {
            var plate = Instantiate(platePrefab, spawnPoint);
            plate.transform.localPosition = new Vector3(0, plateOffset * _plates.Count, 0);
            _plates.Push(plate);
        }

        private void _RemovePlate()
        {
            var plate = _plates.Pop();
            Destroy(plate);
        }
    }
}