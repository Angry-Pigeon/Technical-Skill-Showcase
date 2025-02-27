using Game.SceneDataLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
namespace Game.EatableObjects
{
    public class EatableObject : MonoBehaviour
    {
        public Collider[] Colliders;
        
        [Inject]
        private SceneData sceneData;
        
        public int Experience = 1; 
        public bool CanBeEaten = true;
        
        public virtual void Eat()
        {
            if (!CanBeEaten) return;
            CanBeEaten = false;
        }
        
        public void SetIgnoreCollisionWithGround(bool value)
        {
            foreach (var collider in Colliders)
            {
                Physics.IgnoreCollision(collider, sceneData.GroundCollider, value);
            }
        }
        
        [Button]
        public void GetAllColliders()
        {
            Colliders = GetComponentsInChildren<Collider>();
        }
    }
}