using EventManagerScripts;
using GameEnums;
using TMPro;
using UnityEngine;

namespace UIScripts.UIViewHandlers
{
    public class ScoreViewHandler
    {
        private TMP_Text _scoreViewText;
        private GameObject _scoreViewParent;

        private UIEffects.PulseInOutInfo _pulseInfo = new(0.2f, 0.1f, 1.2f, 1);
        
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject scoreViewParent, TMP_Text scoreViewText)
        {
            _scoreViewText = scoreViewText;
            _scoreViewParent = scoreViewParent;
            
            _eventManager = eventManager;
            Subscribe();
        }

        private void SetScoreText(int score)
        {
            _scoreViewText.text = "Score: "+score;
            
            if (score <= 0) return;
            UIEffects.PulseInOutEffect.StartPulsingInOut(_scoreViewParent, _pulseInfo);
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.GameStarted, GameStarted);
            _eventManager?.Subscribe<int>(GameEvent.ScoreUpdated, SetScoreText);
        }

        private void GameStarted()
        {
            SetScoreText(0);
        }
    }
}