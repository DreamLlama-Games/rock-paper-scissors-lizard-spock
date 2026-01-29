// ===============================
// SymmetricRelationMatrix.cs
// ScriptableObject data container
// ===============================

using System;
using System.Collections.Generic;
using GameEnums;
using UnityEngine;

namespace RelationMatrix
{
    [Serializable]
    public struct RelationEntry
    {
        public RelationElement a;
        public RelationElement b;
        public int value; // 0 [Tie], 1 [Win], -1[Lose]

        public RelationEntry(RelationElement a, RelationElement b, int value)
        {
            this.a = a;
            this.b = b;
            this.value = value;
        }
    }

    [CreateAssetMenu(fileName = "RelationMatrix", menuName = "Relations/Symmetric Relation Matrix", order = 0)]
    public class SymmetricRelationMatrix : ScriptableObject
    {
        [SerializeField] private List<RelationEntry> relations = new();
        [SerializeField] private List<RelationElement> elements = new();

        public IReadOnlyList<RelationElement> Elements => elements;

        public int GetTrueRelation(RelationElement a, RelationElement b)
        {
            var hash = GetUniqueHash(a, b);
            for(var i = 0; i < relations.Count; i++)  
            {
                var existingHash = GetUniqueHash(relations[i].a, relations[i].b);
                if (existingHash != hash) continue;
                return relations[i].a == a? relations[i].value: (-1 *  relations[i].value);
            }
            return 100;
        }
        
        public int Get(RelationElement a, RelationElement b)
        {
            var hash = GetUniqueHash(a, b);
            
            for(var i = 0; i < relations.Count; i++)  
            {
                var existingHash = GetUniqueHash(relations[i].a, relations[i].b);
                if (existingHash != hash) continue;
                return relations[i].value;
            }
            return -1;
        }
        
        public void Set(RelationElement a, RelationElement b, int value) 
        {
            var hash = GetUniqueHash(a, b);

            for(var i = 0; i < relations.Count; i++)  
            {
                var existingHash = GetUniqueHash(relations[i].a, relations[i].b);
                if (existingHash != hash) continue;
                var r =  relations[i];
                r.value = value;
                relations[i] = r; //found and updated
                return;
            }
            relations.Add(new RelationEntry(a, b, value)); //not found and added
        }

        public void AddElement(RelationElement element)
        {
            if (elements.Contains(element)) return;

            foreach (var existingElement in elements)
            {
                relations.Add(new RelationEntry(existingElement, element, -1)); //pair add (lose by default: -1)
            }

            relations.Add(new RelationEntry(element, element, 0)); //self add (tie by default: 0)
            elements.Add(element);
        }

        private int GetUniqueHash(RelationElement x, RelationElement y)
        {
            var a = (int)x;
            var b = (int)y;

            // Make order irrelevant
            if (a <= b) return b * (b + 1) / 2 + a; //if in order return pairing number
            (a, b) = (b, a); //if not swap the values

            // Triangular number pairing
            return b * (b + 1) / 2 + a;
        }
    }
}