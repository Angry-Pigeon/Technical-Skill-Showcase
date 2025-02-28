using System;
using DG.Tweening;
using Game.EatableObjects;
using Game.GameLogic;
using Testing.HoleSystem.Scripts.HoleCreation;
using UnityEngine;

namespace Testing.HoleSystem.Scripts.HoleLogic
{
    [DefaultExecutionOrder(10)]
    public class HoleEatLogicController : MonoBehaviour
    {
        [field: SerializeField]
        public bool Player { get; private set; }
        
        [field: SerializeField]
        public Renderer StencilRenderer { get; private set; }

        [field: SerializeField]
        public SpriteRenderer Circle { get; private set; }
        
        private int maximumQueueDifference = 100;
        
        [field: SerializeField]
        public HoleFallAreaEatableCounter HoleEatableCounter { get; private set; }

        [field: SerializeField]
        public int CurrentLevel { get; private set; } = 1;

        [field: SerializeField]
        public int CurrentExperience { get; private set; } = 0;
        
        /// <summary>
        /// Sends in the previous level as a parameter.
        /// </summary>
        public Action<int> OnExperienceChanged;
        /// <summary>
        /// Sends in the previous level as a parameter.
        /// </summary>
        public Action<int> OnLevelChanged;

        public HoleDataPerLevel holeData { get; private set; }
        private Vector3 startSize;

        private Tween sizeIncreaseTween;
        
        private int originalRenderQueue;
        private int originalCircleRenderQueue = -100;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            DeInitialize();
        }

        public void Initialize()
        {
            startSize = transform.localScale;
            GetCurrentHoleData();
            HoleEatableCounter.OnObjectEaten += OnObjectEaten;
            InstanceStencilShader();
            
        }

        public void DeInitialize()
        {
            HoleEatableCounter.OnObjectEaten -= OnObjectEaten;
        }

        private void GetCurrentHoleData()
        {
            holeData = LevelSettingsDatabase.Instance().GetLevelSettings(-1).HoleData.GetHoleDataForLevel(CurrentLevel);
        }

        private void OnObjectEaten(EatableObject eatableObject)
        {
            int previousExperience = CurrentExperience;
            CurrentExperience += eatableObject.Experience;
            OnExperienceChanged?.Invoke(previousExperience);
            CheckForLevelUp();
        }

        private void CheckForLevelUp()
        {
            if (CurrentLevel >= GetMaxLevel())
                return;

            if (CurrentExperience >= GetExperienceForNextLevel())
            {
                OnLevelUp();
            }

            if (CurrentExperience < 0)
            {
                CurrentExperience = 0;
            }

            if (CurrentExperience >= GetExperienceForNextLevel())
            {
                CheckForLevelUp();
            }
        }

        private void OnLevelUp()
        {
            int previousLevel = CurrentLevel;
            int previousExperience = CurrentExperience;
            float previousFactor = holeData.TotalSizeIncreaseFactor;

            CurrentExperience -= GetExperienceForNextLevel();
            CurrentLevel++;

            GetCurrentHoleData();
            float newFactor = holeData.TotalSizeIncreaseFactor;
            ChangeOrderSizeDependingOnLevel();
            PlaySizeIncreaseTween(previousFactor, newFactor);

            OnExperienceChanged?.Invoke(0);
            OnLevelChanged?.Invoke(previousLevel);
        }

        private void PlaySizeIncreaseTween(float previousFactor, float newFactor)
        {
            Vector3 previousSize = startSize * previousFactor;
            Vector3 targetSize = startSize * newFactor;

            sizeIncreaseTween?.Kill();

            transform.localScale = previousSize;

            sizeIncreaseTween = transform.DOScale(targetSize, 1f).SetEase(Ease.OutBack);
        }
        
        public int GetExperienceForNextLevel()
        {
            return holeData.ExperienceRequirementForNextLevel;
        }
        
        public int GetMaxLevel()
        {
            return LevelSettingsDatabase.Instance().GetLevelSettings(-1).HoleData.GetMaxLevel();
        }

        private void InstanceStencilShader()
        {
            var stencilMatInstance = Instantiate(StencilRenderer.material);
            StencilRenderer.material = stencilMatInstance;
            
            originalRenderQueue = stencilMatInstance.renderQueue;
            StencilRenderer.material.renderQueue = originalRenderQueue;

            originalCircleRenderQueue = Circle.sortingOrder;
        }
        
        private void ChangeOrderSizeDependingOnLevel()
        {
            int offset = Mathf.Min((CurrentLevel - 1) * 10, maximumQueueDifference);

            if (StencilRenderer != null && StencilRenderer.material != null)
            {
                StencilRenderer.material.renderQueue = originalRenderQueue - offset;
            }

            if (Circle != null && Circle.material != null)
            {
                Circle.sortingOrder = originalCircleRenderQueue - offset;
            }
        }

    }
}