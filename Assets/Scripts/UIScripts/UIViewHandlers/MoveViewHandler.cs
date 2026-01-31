using EventManagerScripts;
using GameEnums;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.UIViewHandlers
{
    public class MoveViewHandler
    {
        private GameObject _playerMove;
        private GameObject _opponentMove;
        
        private Image _playerMoveImage;
        private Image _opponentMoveImage;

        private int _currentBobCount;
        private const int BobCount = 3;
        private readonly UIEffects.PulseInOutInfo _pulseInfo = new(0.12f, 0.4f, 1.5f, BobCount);
        
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject playerMove, GameObject opponentMove)
        {
            _eventManager = eventManager;
            _playerMove = playerMove;
            _opponentMove = opponentMove;
            
            _playerMoveImage = _playerMove.GetComponent<Image>();
            _opponentMoveImage = _opponentMove.GetComponent<Image>();
            
            Subscribe();
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.BobMotion, OnBobMotion);
            _eventManager?.Subscribe<RelationElement>(GameEvent.OptionSelected, PlayerOptionSelected);
            _eventManager?.Subscribe<RelationElement>(GameEvent.BotOptionSelected, BotOptionSelected);
        }

        private void OnBobMotion()
        {
            _currentBobCount = 1;
            _eventManager?.Raise(GameEvent.BobCount, this, _currentBobCount);
            UIEffects.PulseInOutEffect.StartPulsingInOut(_opponentMove, _pulseInfo);
            UIEffects.PulseInOutEffect.StartPulsingInOut(_playerMove, _pulseInfo).setOnComplete(IncrementBobCount);
        }

        private void IncrementBobCount()
        {
            if (_currentBobCount < BobCount)
            {
                _currentBobCount += 1;
                _eventManager?.Raise(GameEvent.BobCount, this, _currentBobCount);
                return;
            }
            _currentBobCount = 0;
            _eventManager?.Raise(GameEvent.FetchResults, this);
        }

        private void BotOptionSelected(RelationElement obj)
        {
            
        }

        private void PlayerOptionSelected(RelationElement obj)
        {
            
        }
    }
}