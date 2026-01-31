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
    public class OptionButtonSpawner : MonoBehaviour
    {
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private SymmetricRelationMatrix matrix;

        private void Start()
        {
            SpawnButtons();
        }

        private void SpawnButtons()
        {
            ClearExisting();

            foreach (var element in matrix.Elements)
            {
                var btn = Instantiate(buttonPrefab, transform);
                btn.name = element + "Button";

                // Set label
                var label = btn.GetComponentInChildren<TMP_Text>();
                label.text = element.ToString();

                // Capture local copy (IMPORTANT)
                var capturedElement = element;
                btn.onClick.AddListener(() => OnButtonClicked(capturedElement));
            }
        }

        private void ClearExisting()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void OnButtonClicked(RelationElement option)
        {
            Debug.Log($"Button clicked: {option}");
        } 
    }
}
