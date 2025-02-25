using System;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleEnterPointCollisionDisabler : MonoBehaviour
    {
        public int ConsumedObjectCount = 0;
        
        public GameObject[] IgnoreObjects;
        private void OnTriggerEnter(Collider other)
        {
            foreach (var obj in IgnoreObjects)
            {
                if (obj == null)
                {
                    Debug.LogWarning("Ignore object is null.");
                    continue;
                }
                if (other.gameObject == obj)
                {
                    return;
                }
            }

            Collider otherCollider = other.GetComponent<Collider>();
            if (otherCollider == null)
            {
                Debug.LogWarning("Collider not found on the object with the tag 'HoleEnterPoint'.");
                return;
            }
            otherCollider.enabled = false;
            
            ConsumedObjectCount++;
            
        }
    }
}