using System.Collections.Generic;
using EventManagerScripts;
using GameEnums;
using RelationMatrix;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    // ===============================
    // SymmetricRelationMatrix.cs
    // For the ui element which will spawn buttons
    // ===============================
    public class OptionButtonSpawner
    {
        private SymmetricRelationMatrix _matrix;

        public List<Button> SpawnButtons(GameObject buttonsParent, Button buttonPrefab, IReadOnlyList<RelationElement> elements, GameEventManager gameEventManager)
        {
            ClearExisting(buttonsParent);
            
            var buttons = new List<Button>();
            foreach (var element in elements)
            {
                var btn = Object.Instantiate(buttonPrefab, buttonsParent.transform);
                btn.name = element + "Button";

                // Set label
                var label = btn.GetComponentInChildren<TMP_Text>();
                label.text = element.ToString();

                // Capture local copy
                var capturedElement = element;
                btn.onClick.AddListener(() =>
                {
                    gameEventManager?.Raise(GameEvent.OptionSelected, btn, capturedElement);
                    OnButtonClicked(capturedElement);
                });
                btn.interactable = false;
            }

            return buttons;
        }

        private void ClearExisting(GameObject buttonsParent)
        {
            for (var i = buttonsParent.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(buttonsParent.transform.GetChild(i).gameObject);
            }
        }

        private void OnButtonClicked(RelationElement option)
        {
            Debug.Log($"Button clicked: {option}");
        } 
    }
}
