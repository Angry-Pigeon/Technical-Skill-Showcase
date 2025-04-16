using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.GameLogic.Managers
{
    public enum ManagerState
    {
        None,
        InitializationStarted,
        Initialized,
        PostInitializationStarted,
        PostInitialized,
        PauseStarted,
        Paused,
        ResumeStarted,
        Resumed,
        Disposed
    }
    public abstract class Manager_Base : SerializedMonoBehaviour
    {
        [field: SerializeField]
        public ManagerState State { get; protected set; }
        [field: SerializeField]
        public int Priority { get; protected set; }
        
        public abstract IEnumerator Initialize();
        public abstract IEnumerator PostInitialize();
        public abstract IEnumerator Pause();
        public abstract IEnumerator Resume();
        public abstract IEnumerator Tick();
        public abstract IEnumerator FixedTick();
        public abstract IEnumerator LateTick();
        public abstract void Dispose();

    }
}