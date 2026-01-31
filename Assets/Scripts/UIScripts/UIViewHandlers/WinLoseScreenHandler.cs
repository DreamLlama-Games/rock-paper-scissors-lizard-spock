using EventManagerScripts;
using GameEnums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.UIViewHandlers
{
    public class WinLoseScreenHandler
    {
        private GameObject _winLoseScreen;
        private TMP_Text _winLoseScreenText;
        private Button _homeScreenButton;
        
        private CanvasGroup _screenCanvasGroup;
        
        private const string WinMessage = "You won!";
        private const string LoseMessage = "You lost..try again?";
        
        private const float StayDuration = 0.6f;
        private const float AppearDuration = 0.3f;
        private const float WinDelayDuration = 0.3f;
        private const float LostDelayDuration = 0.5f;
        
        private UIEffects.MoveByInfo _moveUpByInfo = new(0, 250, AppearDuration);
        private UIEffects.MoveByInfo _moveDownByInfo = new(0, -250, AppearDuration);
        
        private GameEventManager _eventManager;
        
        public void Initialize(GameEventManager eventManager, GameObject winLoseScreen, TMP_Text winLoseScreenText, Button homeScreenButton)
        {
            _eventManager = eventManager;
            _winLoseScreen = winLoseScreen;
            _homeScreenButton = homeScreenButton;
            _winLoseScreenText = winLoseScreenText;
            _screenCanvasGroup = _winLoseScreen.GetComponent<CanvasGroup>();
            _homeScreenButton.gameObject.SetActive(false);
            Subscribe();
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.GameStarted, OnGameStarted);
            _eventManager?.Subscribe<string>(GameEvent.PlayerWon,_ => OnPlayerWin());
            _eventManager?.Subscribe<string>(GameEvent.PlayerLost,_ => OnPlayerLost());
        }

        private void OnPlayerWin()
        {
            _winLoseScreenText.text = WinMessage;
            LeanTween.delayedCall(_winLoseScreen, WinDelayDuration, AppearAndDisappear);
        }
        
        private void OnPlayerLost()
        {
            _winLoseScreenText.text = LoseMessage;
            _homeScreenButton.gameObject.SetActive(true);
            LeanTween.delayedCall(_winLoseScreen, LostDelayDuration, Appear);
        }

        private void Appear()
        {
            UIEffects.MoveByEffect.StartMoveBy(_winLoseScreen, _moveUpByInfo);
            UIEffects.AppearDisappearEffect.StartAppearing(_screenCanvasGroup, AppearDuration);
        }

        private void AppearAndDisappear()
        {
            UIEffects.MoveByEffect.StartMoveBy(_winLoseScreen, _moveUpByInfo);
            UIEffects.AppearDisappearEffect.StartAppearing(_screenCanvasGroup, AppearDuration).setOnComplete(() =>
            {
                LeanTween.delayedCall(_winLoseScreen,StayDuration,Disappear);
            });
        }

        private void Disappear()
        {
            UIEffects.MoveByEffect.StartMoveBy(_winLoseScreen, _moveDownByInfo);
            UIEffects.AppearDisappearEffect.StartDisappearing(_screenCanvasGroup, AppearDuration);
        }

        private void OnGameStarted()
        {
            
        }
    }
}