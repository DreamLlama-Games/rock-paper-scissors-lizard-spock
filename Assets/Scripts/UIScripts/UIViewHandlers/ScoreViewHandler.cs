using EventManagerScripts;
using TMPro;
using UnityEngine;

namespace UIScripts.UIViewHandlers
{
    public class ScoreViewHandler
    {
        private TMP_Text _scoreViewText;
        private GameObject _scoreViewParent;
        
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject scoreViewParent, TMP_Text scoreViewText)
        {
            _scoreViewText = scoreViewText;
            _scoreViewParent = scoreViewParent;
            
            _eventManager = eventManager;
            Susbcribe();
        }

        private void Susbcribe()
        {
            
        }
    }
}