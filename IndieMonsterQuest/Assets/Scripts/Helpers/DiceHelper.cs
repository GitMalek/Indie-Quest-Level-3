using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MonsterQuest
{
    public static class DiceHelper
    {
        public static int Roll(string diceNotation)
        {
            Regex notation = new Regex(@"(\d+)?d(\d+)([\+\-]\d+)?");
            Match match = notation.Match(diceNotation);

            int numberOfRolls = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 1;
            int diceSides = int.Parse(match.Groups[2].Value);
            int fixedBonus = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;
            
            return Roll(numberOfRolls, diceSides, fixedBonus);
        }

        private static int Roll(int numberOfRolls, int diceSides, int fixedBonus)
        {
            int result = 0;

            for (int i = 0; i < numberOfRolls; i++)
            {
                result += Random.Range(1, diceSides + 1);
            }

            result += fixedBonus;

            return result;
        }
    }
}
