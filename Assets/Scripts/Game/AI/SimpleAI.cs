using Game.EatableObjects;
using Game.SceneDataLogic;
using UnityEngine;

public class SimpleAI : MonoBehaviour {
    [Header("Movement Settings")]
    public float searchRadius = 10f;
    public float movementSpeed = 5f;
    public float targetReachThreshold = 1f;

    [Header("Ground Bounds")]
    public Collider groundCollider;

    private Vector3 randomDirection;
    private bool movingRandomly = false;
    private Transform currentTarget = null;

    void Start() {
        groundCollider = SceneDataContext.instance.GroundCollider;
        ChooseRandomDirection();
    }

    void Update() {
        // Try to find a valid target every frame (or do this less frequently for performance)
        currentTarget = FindNearestEatableTarget();

        if (currentTarget != null) {
            // Move toward the target
            movingRandomly = false;
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

            // Optionally, add behavior when the target is reached
            if (Vector3.Distance(transform.position, currentTarget.position) < targetReachThreshold) {
                // Add code for when the AI reaches the target (e.g., "eat" it)
            }
        } else {
            // No valid target found, move in a random direction
            if (!movingRandomly) {
                ChooseRandomDirection();
                movingRandomly = true;
            }
            randomDirection.y = 0; // Ensure the AI stays on the XZ plane
            transform.Translate(randomDirection * movementSpeed * Time.deltaTime, Space.World);
        }

        // Ensure the AI stays within the ground bounds.
        KeepWithinBounds();
    }

    // Search for the nearest valid eatable object.
    Transform FindNearestEatableTarget() {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius);
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hit in hits) {
            EatableObject eatable = hit.GetComponent<EatableObject>();

            
            if (eatable != null && eatable.CanBeEaten) {
                
                if (eatable.TryGetComponent(out EatableHoleObject eatableHoleObject))
                {
                    continue;
                    if (eatableHoleObject.Parent == this)
                    {
                        continue;
                    }
                }
                
                // Additional conditions (e.g., MinimumLevelRequired) can be added here.
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearest = hit.transform;
                }
            }
        }
        return nearest;
    }

    // Choose a new random direction on the XZ plane.
    void ChooseRandomDirection() {
        float angle = Random.Range(0f, 360f);
        randomDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    // Check if we're still within the ground bounds; if not, adjust direction.
    void KeepWithinBounds() {
        if (groundCollider != null && !groundCollider.bounds.Contains(transform.position)) {
            Vector3 center = groundCollider.bounds.center;
            Vector3 directionToCenter = (center - transform.position).normalized;
            // Adjust random direction to steer the AI back towards the center.
            randomDirection = directionToCenter;
            transform.Translate(directionToCenter * movementSpeed * Time.deltaTime, Space.World);
        }
    }
}
