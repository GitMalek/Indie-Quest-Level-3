using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScore
    {
        public int score
        {
            get
            {
                return score = _score;
            }
            set
            {
                if (value < 1)
                {
                    _score = 1;
                }
                if (value > 30)
                {
                    _score = 30;
                }
                _score = value;
            }
        }
        [field: SerializeField] public int _score;
        public int modifier { get => (int)Mathf.Floor((score - 10f) / 2f); }

        public static implicit operator int(AbilityScore abilityScore)
        {
            return abilityScore.score;
        }
    }
}
