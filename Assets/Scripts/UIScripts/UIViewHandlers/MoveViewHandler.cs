using EventManagerScripts;
using GameEnums;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.UIViewHandlers
{
    public class MoveViewHandler
    {
        private GameObject _playerMove;
        private GameObject _opponentMove;
        
        private Sprite _defaultSpritePlayer;
        private Sprite _defaultSpriteOpponent;
        
        private Sprite _playerMoveSprite;
        private Sprite _opponentMoveSprite;
        
        private Image _playerMoveImage;
        private Image _opponentMoveImage;

        private SOMoveImageData _moveImageData;

        private int _currentBobCount;
        private const int BobCount = 3;
        private readonly UIEffects.PulseInOutInfo _pulseInfo = new(0.12f, 0.4f, 1.5f, BobCount);
        
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject playerMove, GameObject opponentMove, SOMoveImageData moveImageData)
        {
            _eventManager = eventManager;
            _playerMove = playerMove;
            _opponentMove = opponentMove;
            _moveImageData = moveImageData;
            
            _playerMoveImage = _playerMove.GetComponent<Image>();
            _opponentMoveImage = _opponentMove.GetComponent<Image>();
            
            _defaultSpritePlayer = _playerMoveImage.sprite;
            _defaultSpriteOpponent = _opponentMoveImage.sprite;
            
            Subscribe();
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.BobMotion, OnBobMotion);
            _eventManager?.Subscribe<RelationElement>(GameEvent.OptionSelected, PlayerOptionSelected);
            _eventManager?.Subscribe<RelationElement>(GameEvent.BotOptionSelected, BotOptionSelected);
            _eventManager?.Subscribe(GameEvent.FetchResults, OnFetchResults);
            _eventManager?.Subscribe(GameEvent.NewRoundStarted, OnNewRoundStarted);
        }

        private void OnFetchResults()
        {
            _playerMoveImage.sprite = _playerMoveSprite;
            _opponentMoveImage.sprite = _opponentMoveSprite;
        }

        private void OnNewRoundStarted()
        {
            _playerMoveImage.sprite = _defaultSpritePlayer;
            _opponentMoveImage.sprite = _defaultSpriteOpponent;
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

        private void BotOptionSelected(RelationElement opponentMove)
        {
            _opponentMoveSprite = _moveImageData.GetSprite(opponentMove);
        }

        private void PlayerOptionSelected(RelationElement playerMove)
        {
            _playerMoveSprite = _moveImageData.GetSprite(playerMove);
        }
    }
}