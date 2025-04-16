using System;
using System.Collections;
using Game.GameLogic.Managers;
using Testing.HoleSystem.Scripts.HoleLogic;
using TMPro;
using UnityEngine;

namespace Game.GameLogic.LevelTimer
{
    public class LevelTimer : Manager_Base
    {
        [field: SerializeField]
        public TMP_Text TimerText { get; private set; }

        [field: SerializeField]
        public TMP_Text GoalText { get; private set; }
        
        private bool timerStarted;
        private float time;
        private int levelGoal;

        public override IEnumerator Initialize()
        {
            State = ManagerState.InitializationStarted;
            yield return null;
            State = ManagerState.Initialized;
        }
        public override IEnumerator PostInitialize()
        {
            State = ManagerState.PostInitializationStarted;
            yield return null;
            State = ManagerState.PostInitialized;
        }
        public override IEnumerator Pause()
        {
            State = ManagerState.PauseStarted;
            yield return null;
            State = ManagerState.Paused;
        }
        public override IEnumerator Resume()
        {
            State = ManagerState.ResumeStarted;
            yield return null;
            State = ManagerState.Resumed;
        }
        public override IEnumerator Tick()
        {

            yield return null;
        }
        public override IEnumerator FixedTick()
        {
            yield return null;
        }
        public override IEnumerator LateTick()
        {
            yield return null;
        }
        public override void Dispose()
        {
            State = ManagerState.Disposed;
        }

        public void StartTimer(LevelSettingsData data)
        {
            timerStarted = true;
            time = data.LevelSettings.LevelTimer;
            levelGoal = data.LevelSettings.LevelGoal;
            
            UpdateTimerText();
            GoalText.text = "Goal: " + levelGoal.ToString();
        }

        private void Update()
        {
            if (!timerStarted)
                return;
    
            time -= Time.deltaTime;
            if (time < 0)
                time = 0; // prevent negative time

            UpdateTimerText();

            if (time <= 0)
            {
                timerStarted = false;
                EndLevelIfNeeded();
            }
        }
        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            TimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }


        private void EndLevelIfNeeded()
        {
            HoleEatLogicController player = Managers.GameEvents.Game.Player;
            
            if (player != null && player.CurrentLevel >= levelGoal)
            {
                Managers.GameEvents.Game.EndGame(true);
            }
            else
            {
                Managers.GameEvents.Game.EndGame(false);
            }
        }
    }
}
