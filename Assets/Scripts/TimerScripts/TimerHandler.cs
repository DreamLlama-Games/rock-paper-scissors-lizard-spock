using System;

namespace TimerScripts
{
    public class TimerHandler
    {
        private float _duration;
        private float _timeRemaining;
        private bool _isRunning;
        private bool IsFinished => _timeRemaining <= 0f;
        public Action OnTimerComplete { get; set; }

        public TimerHandler(float duration, Action onTimerComplete = null)
        {
            OnTimerComplete = onTimerComplete;
            ResetTimer(duration);
        }
        
        public void Tick(float deltaTime)
        {
            if (!_isRunning) 
                return;
            
            _timeRemaining -= deltaTime;

            if (_timeRemaining > 0f) return; 
            
            StopTimer();
            OnTimerComplete?.Invoke();
        }

        public void StartTimer()
        {
            _isRunning = true;
        }

        public void PauseTimer()
        {
            _isRunning = false;
        }

        public void ResumeTimer()
        {
            if (!IsFinished)
                _isRunning = true;
        }

        public void StopTimer()
        {
            _isRunning = false;
            _timeRemaining = 0f;
        }

        public void ResetTimer()
        {
            _timeRemaining = _duration;
            _isRunning = false;
        }

        public void ResetTimer(float newDuration)
        {
            _duration = newDuration;
            ResetTimer();
        }
    }
}