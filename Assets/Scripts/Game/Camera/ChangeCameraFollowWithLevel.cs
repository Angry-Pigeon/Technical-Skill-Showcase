using System;
using Base;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Testing.HoleSystem.Scripts.HoleLogic;
using UnityEngine;
namespace Game.HoleLogic
{
    [DefaultExecutionOrder(100)]
    public class ChangeCameraFollowWithLevel : MonoBehaviour
    {
        [field: SerializeField]
        public HoleEatLogicController Parent { get; private set; }

        [field: SerializeField]
        public CinemachineVirtualCamera Camera { get; private set; }

        [field: SerializeField]
        public Vector3 MaximumValue { get; private set; }

        private Vector3 originalValue;
        private Vector3 targetValue;


        [field: SerializeField]
        public bool Debug { get; private set; }
        
        [field: SerializeField]
        [field: Range(0f, 1f)]
        public float DebugPercentage { get; private set; }

        private CinemachineTransposer transposer;
        
        private Tween levelChangeTween;
        
        
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
            transposer = Camera.GetCinemachineComponent<CinemachineTransposer>();
            originalValue = transposer.m_FollowOffset;
            Parent.OnLevelChanged += OnLevelChanged;
            OnLevelChanged(Parent.CurrentLevel);
        }
        
        public void DeInitialize()
        {
            Parent.OnLevelChanged -= OnLevelChanged;
        }
        
        public void OnLevelChanged(int previousLevel)
        {
            float percentage = (float)(Parent.CurrentLevel - 1) / Parent.GetMaxLevel();
            if (Debug)
            {
                percentage = DebugPercentage;
            }
            
            targetValue = percentage.RemapToFloat(0f, 1f, originalValue, MaximumValue);

            levelChangeTween?.Kill();
            levelChangeTween = DOTween.To(() => transposer.m_FollowOffset, x => transposer.m_FollowOffset = x, targetValue, 0.5f);
        }


        private void LateUpdate()
        {
            if (Debug)
            {
                OnLevelChanged(Parent.CurrentLevel);
            }   
        }

    }
}