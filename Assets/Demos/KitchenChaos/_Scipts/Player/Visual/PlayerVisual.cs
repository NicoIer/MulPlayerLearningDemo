using UnityEngine;

namespace Kitchen.Visual
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer headRenderer;
        [SerializeField] private MeshRenderer bodyRenderer;
        [SerializeField] private Material material;

        private void Awake()
        {
            material = new Material(headRenderer.material);
            headRenderer.material = material;
            bodyRenderer.material = material;
        }

        public void SetColor(Color color)
        {
            material.color = color;
        }
    }
}