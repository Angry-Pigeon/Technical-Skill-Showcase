using System;
using UnityEditor;
using UnityEngine;
namespace Editor.Utility
{
    public class EditorPlaystateEvent
    {
        public static Action OnPlayModeEnter;
        public static Action OnPlayModeExit;

        [InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            EditorApplication.playmodeStateChanged += ModeChanged;
            OnPlayModeEnter?.Invoke();
        }

        static void ModeChanged()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode &&
                EditorApplication.isPlaying)
            {
                OnPlayModeExit?.Invoke();
            }
        }
    }
}