using System;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class WASDMovement : MonoBehaviour
    {
        [Tooltip("Movement speed in units per second.")]
        public float moveSpeed = 5f;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        void Update()
        {
            // Get input from WASD or arrow keys (Horizontal = A/D or Left/Right; Vertical = W/S or Up/Down)
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Create a movement vector in the XZ plane.
            Vector3 movement = new Vector3(moveX, 0f, moveZ);

            // Normalize the movement vector to maintain consistent speed in all directions.
            if(movement.magnitude > 1f)
                movement.Normalize();

            // Move the GameObject in world space.
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}