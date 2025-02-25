using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleSizeController : MonoBehaviour
    {
        [field: SerializeField]
        public HoleEnterPointCollisionDisabler HoleEnterPoint { get; private set; }
        
        [Tooltip("How much the hole grows per consumed object.")]
        [SerializeField] private float scaleFactor = 0.25f;
        
        private Vector3 startScale;
        
        private void Start()
        {
            startScale = transform.localScale;
        }
        
        private void Update()
        {
            // Calculate the scale multiplier:
            // When ConsumedObjectCount is 0, multiplier is 1 (original size)
            // When > 0, multiplier increases gradually.
            float multiplier = 1f + HoleEnterPoint.ConsumedObjectCount * scaleFactor;
            
            Vector3 targetScale = startScale * multiplier;
            Vector3 currentScale = transform.localScale;
            
            // Gradually scale the hole to the target scale.
            transform.localScale = Vector3.Lerp(currentScale, targetScale, Time.deltaTime * 5f);
            
        }
    }
}