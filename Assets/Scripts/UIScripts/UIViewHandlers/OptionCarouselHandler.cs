using System;
using System.Collections.Generic;
using EventManagerScripts;
using GameEnums;
using ScriptableObjectScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts.UIViewHandlers
{
    public class OptionCarouselHandler
    {
        private GameObject _carouselParent;
        private Button _buttonPrefab;
        
        private List<Button> _buttons;
        private OptionButtonSpawner _optionButtonSpawner;
        
        //UI Effects
        private readonly UIEffects.RotateShakeInfo _shakeInfo = new(5f,0.25f,0.6f);
        private readonly UIEffects.HoverFloatInfo _hoverInfo = new(0.5f, 0.3f, 5f, 20f);
        
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager eventManager, GameObject carouselParent, Button buttonPrefab, IReadOnlyList<RelationElement> elements, SOButtonImageData buttonImageData)
        {
            _eventManager = eventManager;
            _buttonPrefab = buttonPrefab;
            _carouselParent = carouselParent;
            _optionButtonSpawner = new OptionButtonSpawner();
            
            _buttons = _optionButtonSpawner.SpawnButtons(_carouselParent,_buttonPrefab, elements, buttonImageData, _eventManager);
            Subscribe();
        }

        private void GameStarted()
        {
            ActivateButtons();
        }
        
        private void OnOptionSelected(RelationElement element)
        {
            foreach (var button in _buttons)
            {
                button.interactable = false;
                LeanTween.cancel(button.gameObject);
                ResetButtonTransform(button);
                
                if (Enum.TryParse(button.name, out RelationElement outEnum) &&  outEnum == element)
                {
                    UIEffects.HoverFloatEffect.StartHoverFloat(button.gameObject,_hoverInfo);
                }
            }   
        }

        private void ResetButtonTransform(Button button)
        {
            var angle = button.transform.localEulerAngles;
            var position = button.transform.localPosition;

            angle.z = 0f;
            position.y = 0f;
            
            button.transform.localEulerAngles = angle;
            button.transform.localPosition = position;
        }

        private void ActivateButtons()
        {
            foreach (var button in _buttons)
            {
                button.interactable = true;
                UIEffects.RotateShakeEffect.StartRotateShake(button.gameObject,_shakeInfo);
            }
        }
        
        private void DeactivateButtons()
        {
            foreach (var button in _buttons)
            {
                button.interactable = false;
                LeanTween.cancel(button.gameObject);
                ResetButtonTransform(button);
            }
        }
        
        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.GameStarted, GameStarted);
            _eventManager?.Subscribe<string>(GameEvent.PlayerWon, _ => DeactivateButtons());
            _eventManager?.Subscribe<string>(GameEvent.PlayerLost, _ => DeactivateButtons());
            _eventManager?.Subscribe<string>(GameEvent.MatchTied, _ => DeactivateButtons());
            _eventManager?.Subscribe(GameEvent.NewRoundStarted, ActivateButtons);
            _eventManager?.Subscribe<RelationElement>(GameEvent.OptionSelected, OnOptionSelected);
        }
    }
}