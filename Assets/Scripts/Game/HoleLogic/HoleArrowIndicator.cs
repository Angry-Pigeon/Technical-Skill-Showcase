using System;
using Game.GameLogic.Managers;
using Game.SceneDataLogic;
using UnityEngine;
using Zenject;
namespace Game.HoleLogic
{
    public class HoleArrowIndicator : MonoBehaviour
    {
        private Joystick joystick;
        
        public float RotationEasing = 12f;
        
        public bool IsPlayerControlled = false;
        
        private void OnEnable()
        {
            joystick = GameLogic.Managers.GameEvents.Game.GetBootStrapperContext().Joystick;
        }

        private void Update()
        {
            if(!IsPlayerControlled) return;
            Vector3 direction = joystick.Direction;

            direction.z = direction.y;
            direction.y = 0;
            
            if (direction.magnitude > 1f)
                direction.Normalize();
            
            
            // Rotate the GameObject in world space.
            if (direction == Vector3.zero) return;
            
            Quaternion q = Quaternion.LookRotation(direction);
            if(q == Quaternion.identity) return;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, q, RotationEasing * Time.deltaTime);

        }
    }
}