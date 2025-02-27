using System;
using System.Collections.Generic;
using Game.EatableObjects;
using Testing.HoleSystem.Scripts.HoleLogic;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleFallAreaCollisionDetection : MonoBehaviour
    {
        [field: SerializeField]
        public HoleEatLogicController Parent { get; private set; }

        private HashSet<EatableHoleObject> holeObjectsInArea;


        private void OnEnable()
        {
            holeObjectsInArea = new HashSet<EatableHoleObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EatableHoleObject holeObject))
            {
                if (holeObject.CanBeEaten)
                {
                    Debug.Log("Hole object is eatable");
                    if(holeObject.Parent.CurrentLevel < Parent.CurrentLevel)
                    {
                        holeObjectsInArea.Add(holeObject);
                        return;
                    }
                }
            }
            
            if (other.TryGetComponent(out EatableObject eatable))
            {
                if (eatable.CanBeEaten)
                {
                    eatable.SetIgnoreCollisionWithGround(true);
                    return;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EatableHoleObject holeObject))
            {
                if (holeObjectsInArea.Contains(holeObject))
                {
                    holeObjectsInArea.Remove(holeObject);
                }
            }
            
            if (other.TryGetComponent(out EatableObject eatable))
            {
                eatable.SetIgnoreCollisionWithGround(false);
            }
        }


        private void Update()
        {
            holeObjectsInArea.RemoveWhere(holeObject => holeObject == null);
            foreach (var holeObject in holeObjectsInArea)
            {
                CheckIfObjectsNearHoleCenter(holeObject);
            }
            
        }

        private void CheckIfObjectsNearHoleCenter(EatableHoleObject holeObject)
        {
            Vector3 center = transform.position;
            float acceptableDistance = 0.5f;
            if (Vector3.Distance(holeObject.transform.position, center) < acceptableDistance)
            {
                Parent.HoleEatableCounter.EatObject(holeObject);
                Destroy(holeObject.Parent.gameObject);
            }
        }
    }
}