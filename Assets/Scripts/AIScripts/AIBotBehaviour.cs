using System.Collections.Generic;
using GameEnums;
using RelationMatrix;
using Random = UnityEngine.Random;

namespace AIScripts
{
    public class AIBotBehaviour
    {
        //Tune factors for checking behavior
        private const float ForgetFactor = 0.8f;
        private const float RandomnessFactor = 0.2f;
        private const float Smoothing = 1f; // Laplace smoothing for non-zero probabilities
        
        private SymmetricRelationMatrix _relationMatrix;
        private readonly Dictionary<RelationElement, float> _possibilities = new();

        public void Initialize(SymmetricRelationMatrix matrix)
        {
            _relationMatrix = matrix;
            var usedElements = _relationMatrix.Elements;
            foreach (var element in usedElements)
            {
                _possibilities[element] = 1; //Every element is possible
            }
        }

        public void ProcessPlayerMove(RelationElement playerMove)
        {
            var keys = new List<RelationElement>(_possibilities.Keys);
            
            // Slowly forget old beliefs
            foreach (var key in keys)
            {
                _possibilities[key] *= ForgetFactor;
            }
            
            // Bayesian update (increment observed hand)
            _possibilities[playerMove] += 1f;
        }
        
        public RelationElement ChooseMove()
        {
            var predictedPlayerMove = GetMostLikely(GetNormalizedProbabilities()); // Predict player's next move (arg max)
            var bestCounter = GetCounterPicksAgainst(predictedPlayerMove); // Try to counter it or get a draw
            return Random.value < RandomnessFactor ? PickRandomHand() : bestCounter; //Randomly pick between a counter or a random pick
        }

        private Dictionary<RelationElement, float> GetNormalizedProbabilities()
        {
            var total = 0f; 
            
            //Get total of all the possibilities
            foreach (var pair in _possibilities)
                total += pair.Value + Smoothing;
            
            var result = new Dictionary<RelationElement, float>();

            //Normalize possibilities
            foreach (var pair in _possibilities)
            {
                result[pair.Key] = (pair.Value + Smoothing) / total;
            }

            return result;
        }

        private RelationElement GetMostLikely(Dictionary<RelationElement, float> normalizedPossibilities)
        {
            var best = RelationElement.Rock; //todo: change this to be the last move player did
            var max = float.MinValue;

            foreach (var pair in normalizedPossibilities)
            {
                if (!(pair.Value > max)) continue;
                
                max = pair.Value;
                best = pair.Key;
            }

            return best;
        }

        private RelationElement GetCounterPicksAgainst(RelationElement pick)
        {
            var elements = _relationMatrix.Elements;
            
            foreach (var element in elements)
            {
                var result = _relationMatrix.GetTrueRelation(element, pick);
                if (result != 1) continue;
                return element; //pick one that results in win!
            }

            return pick; //try to get a draw instead
        }
        
        private RelationElement PickRandomHand()
        {
            var elements = _relationMatrix.Elements;
            var randomIndex = Random.Range(0, elements.Count);
            return elements[randomIndex];
        }
    }
}