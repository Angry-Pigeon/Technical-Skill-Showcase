using System;
using System.Collections;
using Game.EatableObjects;
using JetBrains.Annotations;
using Lofelt.NiceVibrations;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleFallAreaEatableCounter : MonoBehaviour
    {
        public Action<EatableObject> OnObjectEaten;
        public GameObject[] IgnoreObjects;
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out EatableObject eatableObject)) return;
            if(!eatableObject.CanBeEaten) return;
            
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
            EatObject(eatableObject);

        }
        public void EatObject(EatableObject eatableObject)
        {
            eatableObject.Eat();
            StartCoroutine(IE_EatObject(eatableObject));
        }
        
        private IEnumerator IE_EatObject(EatableObject eatableObject)
        {
            
            while (eatableObject.transform.position.y > -15)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (eatableObject.Experience < 5)
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            }
            else
            {
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
            }
            
            OnObjectEaten?.Invoke(eatableObject);
            
            Destroy(eatableObject.gameObject);
        }
    }
}