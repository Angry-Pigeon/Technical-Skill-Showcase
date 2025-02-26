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
        public HoleEnterPointEatableCounter HoleEatableCounter { get; private set; }

        private int CurrentLevel = 1;
        private int CurrentExperience = 0;
        
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
        }
        
        public void DeInitialize()
        {
            HoleEatableCounter.OnObjectEaten -= OnObjectEaten;
        }
        
        private void GetCurrentHoleData()
        {
            // Retrieve the hole data corresponding to the current level.
            holeData = LevelSettingsDatabase.Instance().GetLevelSettings(-1).HoleData.GetHoleDataForLevel(CurrentLevel);
        }

        private void OnObjectEaten(EatableObject eatableObject)
        {
            CurrentExperience += eatableObject.Experience;
            CheckForLevelUp();
        }

        private void CheckForLevelUp()
        {
            // Do not level up if we've reached the maximum level.
            if (CurrentLevel >= LevelSettingsDatabase.Instance().GetLevelSettings(-1).HoleData.GetMaxLevel())
                return;
            
            // Check if we've accumulated enough experience to level up.
            if (CurrentExperience >= holeData.ExperienceRequirementForNextLevel)
            {
                OnLevelUp();
            }
            
            // Ensure experience doesn't go negative.
            if (CurrentExperience < 0)
            {
                CurrentExperience = 0;
            }
            
            // In case the player has enough experience to level up more than once.
            if (CurrentExperience >= holeData.ExperienceRequirementForNextLevel)
            {
                CheckForLevelUp();
            }
        }

        private void OnLevelUp()
        {
            // Store the previous level's total size factor.
            float previousFactor = holeData.TotalSizeIncreaseFactor;

            // Deduct the experience and increment the level.
            CurrentExperience -= holeData.ExperienceRequirementForNextLevel;
            CurrentLevel++;
            
            // Update to the new level's data.
            GetCurrentHoleData();
            float newFactor = holeData.TotalSizeIncreaseFactor;
            
            // Animate from the previous scale to the new scale.
            PlaySizeIncreaseTween(previousFactor, newFactor);
        }

        private void PlaySizeIncreaseTween(float previousFactor, float newFactor)
        {
            // Calculate the sizes based on the startSize and the factors.
            Vector3 previousSize = startSize * previousFactor;
            Vector3 targetSize = startSize * newFactor;
            
            // Kill any existing tween.
            sizeIncreaseTween?.Kill();

            // Set the current scale to the previous size (in case it was modified).
            transform.localScale = previousSize;
            
            // Animate to the target size.
            sizeIncreaseTween = transform.DOScale(targetSize, 1f).SetEase(Ease.OutBack);
        }
    }
}
