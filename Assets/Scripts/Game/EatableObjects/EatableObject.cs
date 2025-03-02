using System;
using Game.SceneDataLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
namespace Game.EatableObjects
{
    public class EatableObject : MonoBehaviour
    {
        public Collider[] Colliders;

        private Rigidbody rb;
        
        public int MinimumLevelRequired = 1;
        
        public int Experience = 1; 
        public bool CanBeEaten = true;

        protected virtual void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            if(rb == null) return;
            rb.isKinematic = true;
        }

        public virtual void Eat()
        {
            if (!CanBeEaten) return;
            CanBeEaten = false;
        }
        
        public void SetIgnoreCollisionWithGround(bool value)
        {
            if (value)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            foreach (var collider in Colliders)
            {
                Physics.IgnoreCollision(collider, SceneDataContext.instance.GroundCollider, value);
            }
        }
        
        [Button]
        public void GetAllColliders()
        {
            Colliders = GetComponentsInChildren<Collider>();
        }
    }
}