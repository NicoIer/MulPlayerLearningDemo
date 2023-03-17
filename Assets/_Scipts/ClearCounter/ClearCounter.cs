using Kitchen.Interface;
using Nico;
using UnityEngine;

namespace Kitchen
{
    public class ClearCounter : MonoBehaviour, IInteract
    {
        [SerializeField] private Transform tomatoSpawnPoint;

        public void Interact()
        {
            Debug.Log($"{name}->Interact");

            var tomato = ObjectPoolManager.Singleton.GetObject("Tomato");
            tomato.transform.SetParent(tomatoSpawnPoint);
            tomato.transform.localPosition = Vector3.zero;
        }
    }
}