using EventManagerScripts;
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
        }
    }
}