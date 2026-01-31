using System.Collections.Generic;
using GameEnums;
using UnityEngine;

namespace ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "MoveImageData", menuName = "Game Data/Move Image Data", order = 0)]
    public class SOMoveImageData : ScriptableObject
    {
        [SerializeField] private List<SpriteEntry> sprites = new();
        private Dictionary<RelationElement, Sprite> _lookup;

        private void OnEnable()
        {
            BuildLookup();
        }

        private void BuildLookup()
        {
            _lookup = new Dictionary<RelationElement, Sprite>();

            foreach (var entry in sprites)
            {
                if (!_lookup.ContainsKey(entry.name))
                {
                    _lookup.Add(entry.name, entry.sprite);
                }
                else
                {
                    Debug.LogWarning($"Duplicate RelationElement found: {entry.name}", this);
                }
            }
        }

        public Sprite GetSprite(RelationElement optionName)
        {
            if (_lookup == null || _lookup.Count == 0)
            {
                BuildLookup();
            }

            if (_lookup != null && _lookup.TryGetValue(optionName, out var sprite))
            {
                return sprite;
            }

            Debug.LogWarning($"Sprite not found for option name: {optionName}", this);
            return null;
        }
    }
}