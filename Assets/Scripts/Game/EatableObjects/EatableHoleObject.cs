using Testing.HoleSystem.Scripts.HoleLogic;
using UnityEngine;
namespace Game.EatableObjects
{
    public class EatableHoleObject : EatableObject
    {
        [field: SerializeField]
        public HoleEatLogicController Parent { get; private set; }

        protected override void OnEnable()
        {
            
        }

        public override void Eat()
        {
            base.Eat();
        }
        
    }
}