using UnityEngine;
namespace Game.EatableObjects
{
    public class HideableObject : MonoBehaviour
    {
        private float[] initialAlphaValues;
        private const float minimumAlphaValue = 0.3f;
        
        private Renderer[] renderers;
        
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            initialAlphaValues = new float[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                initialAlphaValues[i] = renderers[i].material.color.a;
            }
        }
        
        public void Hide()
        {
            foreach (var renderer in renderers)
            {
                var color = renderer.material.color;
                color.a = minimumAlphaValue;
                renderer.material.color = color;
            }
        }
        
        public void Show()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                var color = renderers[i].material.color;
                color.a = initialAlphaValues[i];
                renderers[i].material.color = color;
            }
        }
        
    }
}