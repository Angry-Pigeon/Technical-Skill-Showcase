using Base;
using UnityEngine;

namespace Game.EatableObjects
{
    public class HideableObject : MonoBehaviour
    {
        private float[] initialAlphaValues;
        private const float minimumAlphaValue = 0.1f;
        private Renderer[] renderers;
        
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            initialAlphaValues = new float[renderers.Length];
            
            for (int i = 0; i < renderers.Length; i++)
            {
                // Create an instance of the material so changes here won't affect other objects
                Material instancedMat = new Material(renderers[i].material);
                renderers[i].material = instancedMat;
                
                // Store the initial alpha value from the instanced material
                initialAlphaValues[i] = instancedMat.color.a;
            }
        }
        
        public void Hide()
        {
            foreach (var renderer in renderers)
            {
                // Use the instanced material here
                renderer.material.ChangeRenderMode(BlendMode.Transparent);
                Color color = renderer.material.color;
                color.a = minimumAlphaValue;
                renderer.material.color = color;
            }
        }
        
        public void Show()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                // Use the instanced material here as well
                renderers[i].material.ChangeRenderMode(BlendMode.Opaque);
                Color color = renderers[i].material.color;
                color.a = initialAlphaValues[i];
                renderers[i].material.color = color;
            }
        }
    }
}