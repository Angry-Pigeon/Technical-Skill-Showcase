using Testing.HoleSystem.Scripts.HoleLogic;
using UnityEngine;
namespace Game.EatableObjects
{
    public class EatableHoleObject : EatableObject
    {
        [field: SerializeField]
        public HoleEatLogicController Parent { get; private set; }

        public override void Eat()
        {
            base.Eat();
        }
        
    }
}