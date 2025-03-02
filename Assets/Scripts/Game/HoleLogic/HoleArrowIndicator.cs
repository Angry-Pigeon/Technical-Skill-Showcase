using System;
using Game.SceneDataLogic;
using UnityEngine;
using Zenject;
namespace Game.HoleLogic
{
    public class HoleArrowIndicator : MonoBehaviour
    {
        [Inject]
        private SceneData SceneData;

        private Joystick joystick;
        
        public float RotationEasing = 12f;
        
        private void OnEnable()
        {
            
        }

        private void Update()
        {
            if (SceneData == null)
                return;
            
            Vector3 direction = SceneData.Joystick.Direction;

            direction.z = direction.y;
            direction.y = 0;
            
            if (direction.magnitude > 1f)
                direction.Normalize();
            
            
            // Rotate the GameObject in world space.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationEasing * Time.deltaTime);

        }
    }
}