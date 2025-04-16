using System;
using Game.GameLogic;
using Game.GameLogic.Managers;
using Game.SceneDataLogic;
using UnityEngine;
using Zenject;

namespace Game.JoystickController
{
    public class JoystickController : MonoBehaviour
    {
        [field: SerializeField]
        public float MovementSpeed { get; private set; } = 5f;
        [field: SerializeField]
        public Collider ObjectCollider { get; private set; }
        
        private Joystick joystick;

        private void OnEnable()
        {
            joystick = GameLogic.Managers.GameEvents.Game.GetBootStrapperContext().Joystick;
            if (ObjectCollider == null)
                ObjectCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (joystick == null)
                return;

            float moveX = joystick.Horizontal;
            float moveZ = joystick.Vertical;

            Vector3 movement = new Vector3(moveX, 0f, moveZ);

            if (movement.magnitude > 1f)
                movement.Normalize();

            Vector3 moveDelta = movement * MovementSpeed * Time.deltaTime;
            Bounds worldBounds = SceneDataContext.instance.WorldBounds;

            Bounds newColliderBounds = new Bounds(ObjectCollider.bounds.center + moveDelta, ObjectCollider.bounds.size);

            if (newColliderBounds.min.x < worldBounds.min.x || newColliderBounds.max.x > worldBounds.max.x)
            {
                moveDelta.x = 0;
            }
            if (newColliderBounds.min.z < worldBounds.min.z || newColliderBounds.max.z > worldBounds.max.z)
            {
                moveDelta.z = 0;
            }
            if (newColliderBounds.min.y < worldBounds.min.y || newColliderBounds.max.y > worldBounds.max.y)
            {
                moveDelta.y = 0;
            }

            // Move the GameObject in world space.
            transform.Translate(moveDelta, Space.World);
        }
    }
}
