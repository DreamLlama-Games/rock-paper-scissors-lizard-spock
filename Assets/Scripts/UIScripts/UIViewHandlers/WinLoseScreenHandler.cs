using EventManagerScripts;
using TMPro;
using UnityEngine;

namespace UIScripts.UIViewHandlers
{
    public class WinLoseScreenHandler
    {
        private GameObject _winLoseScreen;
        private TMP_Text _winLoseScreenText;
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject winLoseScreen, TMP_Text winLoseScreenText)
        {
            _eventManager = eventManager;
            _winLoseScreen = winLoseScreen;
            _winLoseScreenText = winLoseScreenText;
            Subscribe();
        }

        private void Subscribe()
        {
            
        }
    }
}