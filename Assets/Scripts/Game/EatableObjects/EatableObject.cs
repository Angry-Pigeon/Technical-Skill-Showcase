using UnityEngine;
namespace Game.EatableObjects
{
    public class EatableObject : MonoBehaviour
    {
        public int Experience = 1; 
        public bool CanBeEaten = true;
        
        public void Eat()
        {
            if (!CanBeEaten) return;
            CanBeEaten = false;
            // Destroy(gameObject);
        }
    }
}