using System.Collections.Generic;
using Game.EatableObjects;
using UnityEngine;

namespace Game.HoleLogic
{
    public class CameraObscuringObjectDetector : MonoBehaviour
    {
        // Adjustable parameters for the sphere cast.
        [SerializeField] private float castRadius = 1f;      // Girth of the cast.
        [SerializeField] private float castDistance = 10f;     // How far the cast goes.

        // Keeps track of the currently hidden objects.
        private HashSet<HideableObject> hiddenObjects = new HashSet<HideableObject>();

        private void Update()
        {
            // Create a ray starting from the camera's position, pointing in its forward direction.
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;
            Ray ray = new Ray(origin, direction);

            // Perform a sphere cast along the ray.
            RaycastHit[] hits = Physics.SphereCastAll(ray, castRadius, castDistance);

            // Use a temporary set to track objects hit in this frame.
            HashSet<HideableObject> currentlyHit = new HashSet<HideableObject>();

            // Process each hit.
            foreach (RaycastHit hit in hits)
            {
                // Check if the collider belongs to a HideableObject.
                HideableObject hideable = hit.collider.GetComponent<HideableObject>();
                if (hideable != null)
                {
                    currentlyHit.Add(hideable);
                    // If the object isn’t already hidden, hide it and track it.
                    if (!hiddenObjects.Contains(hideable))
                    {
                        hideable.Hide();
                        hiddenObjects.Add(hideable);
                    }
                }
            }
            
            hiddenObjects.RemoveWhere(obj => obj == null);

            // For any object that was hidden in a previous frame but is no longer hit,
            // call Show and remove it from the hidden list.
            List<HideableObject> toShow = new List<HideableObject>();
            foreach (HideableObject obj in hiddenObjects)
            {
                if (!currentlyHit.Contains(obj))
                {
                    obj.Show();
                    toShow.Add(obj);
                }
            }
            foreach (HideableObject obj in toShow)
            {
                hiddenObjects.Remove(obj);
            }
        }

        // Visualize the sphere cast using Gizmos in the Editor.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 origin = transform.position;
            Vector3 end = origin + transform.forward * castDistance;

            // Draw the central line of the cast.
            Gizmos.DrawLine(origin, end);

            // Draw spheres at the start and end of the cast.
            Gizmos.DrawWireSphere(origin, castRadius);
            Gizmos.DrawWireSphere(end, castRadius);

            // Optionally, draw additional spheres along the path for clarity.
            int segments = 10;
            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector3 pos = Vector3.Lerp(origin, end, t);
                Gizmos.DrawWireSphere(pos, castRadius);
            }
        }
    }
}
