using EventManagerScripts;
using GameEnums;
using TimerScripts;
using UnityEngine.UI;

namespace UIScripts.UIViewHandlers
{
    public class TimerViewHandler
    {
        private Slider _timerSlider;
        private TimerHandler _timerHandler;
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager eventManager, Slider slider, float maxDuration)
        {
            _timerHandler = new TimerHandler(maxDuration,TimerEnd);
            
            _timerSlider = slider;
            _timerSlider.maxValue = maxDuration;
            _timerSlider.value = maxDuration;
            
            _eventManager = eventManager;
            Subscribe();
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.GameStarted, GameStarted);
            _eventManager?.Subscribe(GameEvent.NewRoundStarted, () =>
            {
                ResetTimer();
                StartTimer();
            });
            _eventManager?.Subscribe<RelationElement>(GameEvent.OptionSelected, _ => PauseTimer());
        }

        private void GameStarted()
        {
            StartTimer();
        }

        private void TimerEnd()
        {
            _eventManager?.Raise(GameEvent.TimerEnded,this);
        }

        private void StartTimer() => _timerHandler.StartTimer();
        private void StopTimer() => _timerHandler.StopTimer();
        
        private void PauseTimer() => _timerHandler.PauseTimer();
        private void ResumeTimer() => _timerHandler.ResumeTimer();
        
        private void ResetTimer() => _timerHandler.ResetTimer();
        private void ResetTimer(float duration) => _timerHandler.ResetTimer(duration);
        

        public void Tick(float deltaTime)
        {
            _timerHandler.Tick(deltaTime);
            _timerSlider.value = _timerHandler.TimeRemaining;
        }
    }
}