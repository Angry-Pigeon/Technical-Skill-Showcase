using UnityEngine;
namespace Game.GameLogic.Managers.UISystems
{
    public static class UIUtility
    {
        public static CanvasGroup SetActive(this CanvasGroup canvasGroup, bool active)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.interactable = active;
            return canvasGroup;
        }
        
        public static CanvasGroup SetActive(this CanvasGroup canvasGroup, bool active, float alpha)
        {
            canvasGroup.alpha = active ? alpha : 0;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.interactable = active;
            return canvasGroup;
        }
        
        public static CanvasGroup SetActive(this CanvasGroup canvasGroup, bool active, float alpha, bool blocksRaycasts, bool interactable)
        {
            canvasGroup.alpha = active ? alpha : 0;
            canvasGroup.blocksRaycasts = blocksRaycasts;
            canvasGroup.interactable = interactable;
            return canvasGroup;
        }
        
        public static CanvasGroup SetAlpha(this CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
            return canvasGroup;
        }
        
        public static CanvasGroup SetBlocksRaycasts(this CanvasGroup canvasGroup, bool blocksRaycasts)
        {
            canvasGroup.blocksRaycasts = blocksRaycasts;
            return canvasGroup;
        }
        
        public static CanvasGroup SetInteractable(this CanvasGroup canvasGroup, bool interactable)
        {
            canvasGroup.interactable = interactable;
            return canvasGroup;
        }
        
    }
}