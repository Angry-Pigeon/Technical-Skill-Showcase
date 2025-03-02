using System.Collections;
using UnityEngine;

namespace Testing.HoleSystem.Scripts.HoleCreation
{
    public class WASDMovement : MonoBehaviour
    {
        [Tooltip("Movement speed in units per second.")]
        public float moveSpeed = 5f;

        public float BaseShrinkSpeed = 0.1f;
        public float ShrinkSpeedAcceleration = 0.1f;
        
        public float BounceDuration = 0.5f;  
        public AnimationCurve bounceCurve;   

        private Vector3 originalScale;
        private float holdTime = 0f;
        private bool isShrinking = false;
        private bool isBouncing = false;
        private Coroutine bounceCoroutine = null;

        private void Start()
        {
            Application.targetFrameRate = 60;
            originalScale = transform.localScale;
        }

        void Update()
        {
            WASDMovementControl();

            ShrinkControl();
        }
        private void ShrinkControl()
        {

            if (!Input.GetKey(KeyCode.Space) && !isBouncing && !isShrinking)
            {
                originalScale = transform.localScale;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                if (isBouncing)
                {
                    StopCoroutine(bounceCoroutine);
                    isBouncing = false;
                }

                isShrinking = true;
                holdTime += Time.deltaTime;
                float shrinkAmount = (BaseShrinkSpeed + holdTime * ShrinkSpeedAcceleration) * Time.deltaTime;
                transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);

                float minScale = 0.1f;
                if (transform.localScale.x < minScale)
                {
                    transform.localScale = new Vector3(minScale, minScale, minScale);
                }
            }
            else
            {
                if (isShrinking)
                {
                    isShrinking = false;
                    holdTime = 0f;
                    if (transform.localScale.x < originalScale.x)
                    {
                        bounceCoroutine = StartCoroutine(BounceBack());
                    }
                }
            }
        }
        private void WASDMovementControl()
        {

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveX, 0f, moveZ);
            if (movement.magnitude > 1f)
                movement.Normalize();
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }


        IEnumerator BounceBack()
        {
            isBouncing = true;
            Vector3 startScale = transform.localScale;
            float elapsed = 0f;
            while (elapsed < BounceDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / BounceDuration;
                // float scaleT = bounceCurve != null ? bounceCurve.Evaluate(t) : EaseOutElastic(t);
                float scaleT = EaseOutElastic(t);
                transform.localScale = Vector3.LerpUnclamped(startScale, originalScale, scaleT);
                yield return null;
            }
            transform.localScale = originalScale;
            isBouncing = false;
        }

        float EaseOutElastic(float t)
        {
            if (t == 0f) return 0f;
            if (t == 1f) return 1f;
            float p = 0.3f;
            return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4f) * (2 * Mathf.PI) / p) + 1f;
        }
    }
}
