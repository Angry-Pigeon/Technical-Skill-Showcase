using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.EatableObjects
{
    public class EatableObjectsWeightDistributer : MonoBehaviour
    {
        [Button]
        private void GenerateWeights()
        {
            // Get the main Rigidbody.
            Rigidbody mainRb = GetComponent<Rigidbody>();
            if (mainRb == null)
            {
                Debug.LogError("Main object must have a Rigidbody.");
                return;
            }

            // Get the BoxCollider to determine the corners.
            BoxCollider box = GetComponent<BoxCollider>();
            if (box == null)
            {
                Debug.LogError("Main object must have a BoxCollider to determine corner positions.");
                return;
            }

            // Calculate the corners of the BoxCollider in local space.
            // Here we choose the bottom four corners (using the minimum y value).
            Vector3 center = box.center;
            Vector3 extents = box.size * 0.5f;
            float y = center.y - extents.y; // bottom face y value

            Vector3[] cornerPositions = new Vector3[4];
            cornerPositions[0] = new Vector3(center.x - extents.x, y, center.z - extents.z);
            cornerPositions[1] = new Vector3(center.x - extents.x, y, center.z + extents.z);
            cornerPositions[2] = new Vector3(center.x + extents.x, y, center.z - extents.z);
            cornerPositions[3] = new Vector3(center.x + extents.x, y, center.z + extents.z);

            // Create each heavy corner object.
            for (int i = 0; i < cornerPositions.Length; i++)
            {
                // Create a new GameObject.
                GameObject heavyCorner = new GameObject("HeavyCorner_" + (i + 1));

                // Set it as a child of the main object.
                heavyCorner.transform.parent = transform;
                heavyCorner.transform.localPosition = cornerPositions[i];
                heavyCorner.transform.localRotation = Quaternion.identity;

                // Add a Rigidbody and set its mass to 10x the main body's mass.
                Rigidbody cornerRb = heavyCorner.AddComponent<Rigidbody>();
                cornerRb.mass = mainRb.mass * 3;

                // Add a FixedJoint and connect it to the main object's Rigidbody.
                FixedJoint joint = heavyCorner.AddComponent<FixedJoint>();
                joint.connectedBody = mainRb;
            }
        }
    }
}