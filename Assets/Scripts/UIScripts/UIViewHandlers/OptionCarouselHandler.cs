using System.Collections.Generic;
using EventManagerScripts;
using GameEnums;
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
        
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager eventManager, GameObject carouselParent, Button buttonPrefab, IReadOnlyList<RelationElement> elements)
        {
            _eventManager = eventManager;
            _buttonPrefab = buttonPrefab;
            _carouselParent = carouselParent;
            _optionButtonSpawner = new OptionButtonSpawner();
            
            _buttons = _optionButtonSpawner.SpawnButtons(_carouselParent,_buttonPrefab, elements, _eventManager);
            Subscribe();
        }

        private void Subscribe()
        {
            
        }
    }
}