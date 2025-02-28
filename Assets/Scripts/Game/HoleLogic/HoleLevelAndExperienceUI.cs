using System;
using DG.Tweening;
using Testing.HoleSystem.Scripts.HoleLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Game.HoleLogic
{
    [DefaultExecutionOrder(100)]
    public class HoleLevelAndExperienceUI : MonoBehaviour
    {
        [field: SerializeField]
        public HoleEatLogicController Parent { get; private set; }

        
        [field: SerializeField]
        public Image Fill { get; private set; }

        [field: SerializeField]
        public TMP_Text LevelText { get; private set; }

        [field: SerializeField]
        public TMP_Text UsernameText { get; private set; }
        
        [field: SerializeField]
        public TMP_Text ExperienceText { get; private set; }
        
        private Tween experienceChangeTween;

        private void Start()
        {
            Initialize();
        }

        private void OnDisable()
        {
            DeInitialize();
        }

        public void Initialize()
        {
            Parent.OnExperienceChanged += OnExperienceChanged;
            Parent.OnLevelChanged += OnLevelChanged;
            Fill.fillAmount = 0;
            ExperienceText.text = GenerateExperienceText();
        }
        
        public void DeInitialize()
        {
            Parent.OnExperienceChanged -= OnExperienceChanged;
            Parent.OnLevelChanged -= OnLevelChanged;
        }
        
        public void OnExperienceChanged(int previousExperience)
        {
            ExperienceText.text = GenerateExperienceText();
            float fillPercentage = GetCurrentFillPercentage();
            experienceChangeTween?.Kill();
            experienceChangeTween = Fill.DOFillAmount(fillPercentage, 0.5f);
        }
        
        public void OnLevelChanged(int previousLevel)
        {
            LevelText.text = $"Level {Parent.CurrentLevel}";
        }

        private float GetCurrentFillPercentage()
        {
            float currentExperience = Parent.CurrentExperience;
            float experienceNeeded = Parent.GetExperienceForNextLevel();
            return currentExperience / experienceNeeded;
        }
        
        private string GenerateExperienceText()
        {
            return $"{Parent.CurrentExperience}/{Parent.GetExperienceForNextLevel()}";
        }
        
    }
}