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
        
        private void OnEnable()
        {
            joystick = GameLogic.Managers.GameEvents.Game.GetBootStrapperContext().Joystick;
        }

        private void Update()
        {
            
            Vector3 direction = joystick.Direction;

            direction.z = direction.y;
            direction.y = 0;
            
            if (direction.magnitude > 1f)
                direction.Normalize();
            
            
            // Rotate the GameObject in world space.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationEasing * Time.deltaTime);

        }
    }
}