using System;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class HoleGroundCollisionDisabler : MonoBehaviour
    {
        public Collider GroundCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collider col))
            {
                Physics.IgnoreCollision(col, GroundCollider, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Collider col))
            {
                Physics.IgnoreCollision(col, GroundCollider, false);
            }
        }
    }
}