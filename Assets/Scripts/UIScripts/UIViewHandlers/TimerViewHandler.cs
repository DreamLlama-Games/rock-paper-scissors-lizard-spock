using EventManagerScripts;
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
            _eventManager = eventManager;
            _timerSlider.maxValue = maxDuration;
            _timerSlider.value = maxDuration;
        }

        private void Subscribe()
        {
            
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