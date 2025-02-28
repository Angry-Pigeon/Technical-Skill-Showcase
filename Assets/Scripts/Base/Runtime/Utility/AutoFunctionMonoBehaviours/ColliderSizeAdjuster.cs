using Sirenix.OdinInspector;
using UnityEngine;

namespace Base.Runtime.Utility
{
    public class ColliderSizeAdjuster : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Multiplier used to adjust the collider size. Set values >1 to enlarge and <1 to shrink.")]
        public float AdjustAmount = 1f;

        [Button("Adjust Collider Size")]
        public void AdjustCollidersSize()
        {
            Collider collider = GetComponent<Collider>();

            if (collider == null)
            {
                Debug.LogWarning("No Collider component found on " + gameObject.name);
                return;
            }

            if (collider is BoxCollider box)
            {
                Vector3 newSize = box.size;
                newSize.x *= AdjustAmount;
                newSize.z *= AdjustAmount;
                box.size = newSize;
                Debug.Log("BoxCollider size adjusted to: " + box.size);
            }
            else if (collider is SphereCollider sphere)
            {
                sphere.radius *= AdjustAmount;
                Debug.Log("SphereCollider radius adjusted to: " + sphere.radius);
            }
            else if (collider is CapsuleCollider capsule)
            {
                capsule.radius *= AdjustAmount;
                capsule.height *= AdjustAmount;
                Debug.Log("CapsuleCollider adjusted to: radius = " + capsule.radius + ", height = " + capsule.height);
            }
            else
            {
                Debug.LogWarning("Collider type " + collider.GetType().Name + " is not supported for size adjustment.");
            }
        }
    }
}