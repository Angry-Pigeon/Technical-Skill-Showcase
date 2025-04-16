using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> objects;
        public List<GameObject> Objects => objects;
        
        private RectTransform rectTransform;

        [SerializeField]
        private float animationDuration = 0.2f;
        public float AnimationDuration => animationDuration;

        [SerializeField]
        private float activeYOffset = -50f;

        private bool isActive = false;
        private bool isAnimating = false;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(ToggleState);
            }
            
            foreach (var obj in Objects)
            {
                obj.SetActive(isActive);
            }
        }

        private void OnDisable()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveListener(ToggleState);
            }
        }


        public void ToggleState()
        {
            if (isAnimating)
                return;
            
            isActive = !isActive;
            isAnimating = true;

            if (isActive)
            {
                foreach (var obj in Objects)
                {
                    obj.SetActive(true);
                }
            }
            
            Sequence sequence = DOTween.Sequence();
            Vector2 startPos = Vector2.zero;
            Vector2 targetPos = startPos;
            for (int ındex = 0; ındex < Objects.Count; ındex++)
            {
                GameObject obj = Objects[ındex];
                RectTransform rt = obj.GetComponent<RectTransform>();
                if (rt == null)
                    continue;

                if (isActive)
                {
                    targetPos.y = activeYOffset * (ındex + 1);
                }
                else
                {
                    targetPos.y = 0;
                }

                sequence.Join(rt.DOAnchorPos(targetPos, AnimationDuration));
            }

            sequence.OnComplete(() =>
            {
                if (!isActive)
                {
                    foreach (var obj in Objects)
                    {
                        obj.SetActive(false);
                    }
                }
                isAnimating = false;
            });
        }
        
        [Button]
        private void GetObjects()
        {
            objects = new List<GameObject>();
            foreach (Transform child in transform)
            {
                objects.Add(child.gameObject);
            }
        }
    }
}
