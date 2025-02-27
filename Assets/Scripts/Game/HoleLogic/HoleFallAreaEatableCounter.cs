using System;
using Game.EatableObjects;
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
            OnObjectEaten?.Invoke(eatableObject);
        }
    }
}