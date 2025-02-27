using System;
using DG.Tweening;
using Game.EatableObjects;
using Game.GameLogic;
using Testing.HoleSystem.Scripts.HoleCreation;
using UnityEngine;

namespace Testing.HoleSystem.Scripts.HoleLogic
{
    public class HoleEatLogicController : MonoBehaviour
    {
        [field: SerializeField]
        public bool Player { get; private set; }
        
        [field: SerializeField]
        public Renderer StencilRenderer { get; private set; }

        [field: SerializeField]
        public SpriteRenderer Circle { get; private set; }
        
        private int originalRenderQueue;
        private int originalCircleRenderQueue;
        
        private int maximumQueueDifference = 100;
        
        [field: SerializeField]
        public HoleFallAreaEatableCounter HoleEatableCounter { get; private set; }

        
        [field: SerializeField]
        public int CurrentLevel { get; private set; } = 1;

        [field: SerializeField]
        public int CurrentExperience { get; private set; } = 0;



        private HoleDataPerLevel holeData;
        private Vector3 startSize;

        private Tween sizeIncreaseTween;

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
            CurrentExperience += eatableObject.Experience;
            CheckForLevelUp();
        }

        private void CheckForLevelUp()
        {
            if (CurrentLevel >= LevelSettingsDatabase.Instance().GetLevelSettings(-1).HoleData.GetMaxLevel())
                return;

            if (CurrentExperience >= holeData.ExperienceRequirementForNextLevel)
            {
                OnLevelUp();
            }

            if (CurrentExperience < 0)
            {
                CurrentExperience = 0;
            }

            if (CurrentExperience >= holeData.ExperienceRequirementForNextLevel)
            {
                CheckForLevelUp();
            }
        }

        private void OnLevelUp()
        {
            float previousFactor = holeData.TotalSizeIncreaseFactor;

            CurrentExperience -= holeData.ExperienceRequirementForNextLevel;
            CurrentLevel++;

            GetCurrentHoleData();
            float newFactor = holeData.TotalSizeIncreaseFactor;
            ChangeOrderSizeDependingOnLevel();
            PlaySizeIncreaseTween(previousFactor, newFactor);
        }

        private void PlaySizeIncreaseTween(float previousFactor, float newFactor)
        {
            Vector3 previousSize = startSize * previousFactor;
            Vector3 targetSize = startSize * newFactor;

            sizeIncreaseTween?.Kill();

            transform.localScale = previousSize;

            sizeIncreaseTween = transform.DOScale(targetSize, 1f).SetEase(Ease.OutBack);
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
                Circle.material.renderQueue = originalCircleRenderQueue - offset;
                Circle.sortingOrder = offset;
            }
        }

    }
}