using UnityEngine;

[ExecuteAlways] // Runs in edit mode as well as play mode.
public class MaintainWorldScale : MonoBehaviour
{
    // The desired world scale that you want to maintain.
    // (You can expose this in the Inspector if you wish to adjust it manually.)
    [SerializeField]
    private Vector3 targetWorldScale = Vector3.one;
    
    private void Awake()
    {
        // Optionally, record the initial world scale as the target.
    }

    private void LateUpdate()
    {
        // If the text has a parent, compute the required local scale.
        if (transform.parent != null)
        {
            Vector3 parentLossyScale = transform.parent.lossyScale;
            
            // Prevent division by zero (unlikely, but good practice).
            if (parentLossyScale.x == 0 || parentLossyScale.y == 0 || parentLossyScale.z == 0)
                return;
            
            // To maintain the target world scale, set localScale to targetWorldScale divided by parent's lossy scale.
            transform.localScale = new Vector3(
                targetWorldScale.x / parentLossyScale.x,
                targetWorldScale.y / parentLossyScale.y,
                targetWorldScale.z / parentLossyScale.z
            );
        }
        else
        {
            // If there's no parent, just ensure the local scale matches the target.
            transform.localScale = targetWorldScale;
        }
    }
}