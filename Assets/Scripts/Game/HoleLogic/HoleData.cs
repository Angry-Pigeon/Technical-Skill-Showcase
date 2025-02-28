using System.Collections.Generic;
using Base;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Testing.HoleSystem.Scripts.HoleLogic
{
    [CreateAssetMenu(fileName = "HoleData", menuName = "Game/Hole System/Hole Data", order = 0)]
    public class HoleData : ScriptableObject
    {
        [field: SerializeField]
        public float StartSize { get; private set; } = 1f;

        [field: SerializeField]
        public float MaxSize { get; private set; } = 10f;
        
        [field: SerializeField]
        public float SizeIncreaseFactorPerLevel { get; private set; } = 0.25f;

        [field: SerializeField]
        public int ExperienceRequirementForFirstLevel { get; private set; } = 8;

        [field: SerializeField]
        public float ExperienceRequirementIncreaseFactor { get; private set; } = 1.5f;

        [field: SerializeField]
        public float ExperienceRoundToFactor { get; private set; }



        [field: SerializeField]
        [field: TableList]
        public List<HoleDataPerLevel> HoleDataPerLevel { get; private set; }

        
        [Button]
        public void GenerateHoleLevels()
        {
            // Create a new list to store per-level data.
            List<HoleDataPerLevel> levels = new List<HoleDataPerLevel>();

            float currentSize = StartSize;
            int currentExpRequirement = ExperienceRequirementForFirstLevel;
            int level = 1;

            // Continue creating levels until the current size reaches MaxSize.
            while (currentSize < MaxSize)
            {
                // Determine the size increase for this level.
                float sizeIncrease = SizeIncreaseFactorPerLevel;
        
                // Adjust if the size increase would exceed MaxSize.
                if (currentSize + sizeIncrease > MaxSize)
                {
                    sizeIncrease = MaxSize - currentSize;
                }
        
                // Use the constructor to create a new HoleDataPerLevel.
                levels.Add(new HoleDataPerLevel(level, currentExpRequirement, currentSize));

                // Update current size and experience for the next level.
                currentSize += sizeIncrease;
                currentExpRequirement = Mathf.RoundToInt(currentExpRequirement * ExperienceRequirementIncreaseFactor);
                
                currentExpRequirement = (int)currentExpRequirement.RoundTo(ExperienceRoundToFactor);
                level++;
            }
            levels.Add(new HoleDataPerLevel(level, currentExpRequirement, currentSize));
            

            // Assign the generated list to the serialized property.
            HoleDataPerLevel = levels;
        }
        
        public HoleDataPerLevel GetHoleDataForLevel(int level)
        {
            if (level < 1 || level > HoleDataPerLevel.Count)
            {
                Debug.LogWarning($"Level {level} is out of range for HoleDataPerLevel.");
                return HoleDataPerLevel[0];
            }
            return HoleDataPerLevel.Find(x => x.Level == level);
        }
        
        public int GetMaxLevel()
        {
            return HoleDataPerLevel.Count;
        }
        
    }

    [System.Serializable]
    public struct HoleDataPerLevel
    {
        [field: SerializeField]
        public int Level { get; private set; }
        [field: SerializeField]
        public int ExperienceRequirementForNextLevel { get; private set; }
        [field: SerializeField]
        public float TotalSizeIncreaseFactor { get; private set; }

        public HoleDataPerLevel(int level, int experienceRequirementForNextLevel, float totalSizeIncreaseFactor)
        {
            Level = level;
            ExperienceRequirementForNextLevel = experienceRequirementForNextLevel;
            TotalSizeIncreaseFactor = totalSizeIncreaseFactor;
        }
    }

}