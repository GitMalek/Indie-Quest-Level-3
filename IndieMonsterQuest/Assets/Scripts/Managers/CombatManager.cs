using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        public void Simulate(GameState gameState)
        {
            Party party = gameState.party;
            List<string> characterNames = new List<string>();

            foreach (Character character in party.characters)
            {
                characterNames.Add(character.displayName);
            }

            string monsterName = gameState.combat.monster.displayName;
            int monsterHP = gameState.combat.monster.hitPoints;
            int savingThrowDC = gameState.combat.monster.savingThrowDC;

            Console.WriteLine($"{StringHelper.JoinWithAnd(characterNames)} enter the dungeon.");
            Console.WriteLine($"Watch out, {monsterName} with {monsterHP} HP appears!");

            while (monsterHP > 0)
            {
                for (int i = 0; i < characterNames.Count; i++)
                {
                    int damage = DiceHelper.Roll("2d6");

                    monsterHP -= damage;

                    if (monsterHP < 0)
                    {
                        monsterHP = 0;
                        Console.WriteLine($"{characterNames[i]} hits the {monsterName} for {damage} damage. The {monsterHP} has {monsterHP} HP left.");
                        break;
                    }
                    Console.WriteLine($"{characterNames[i]} hits the {monsterName} for {damage} damage. {monsterName} has {monsterHP} HP left.");
                }

                if (monsterHP == 0)
                {
                    break;
                }

                int target = Random.Range(0, characterNames.Count);

                Console.WriteLine($"The {monsterName} attacks {characterNames[target]}!");

                int constitutionSave = DiceHelper.Roll("1d20+3");

                if (constitutionSave < savingThrowDC)
                {
                    Console.WriteLine($"{characterNames[target]} rolls a {constitutionSave} and fails to be saved. {characterNames[target]} is knocked out.");
                    characterNames.RemoveAt(target);
                }
                else
                {
                    Console.WriteLine($"{characterNames[target]} rolls a {constitutionSave} and is saved from the attack.");
                }

                if (characterNames.Count == 0)
                {
                    break;
                }

            }

            if (characterNames.Count == 0)
            {
                Console.WriteLine($"The party has failed and the {monsterName} roams free.");
            }
            else if (monsterHP == 0)
            {
                Console.WriteLine($"The {monsterName} collapses and the heroes celebrate their victory!");
            }
        }
    }
}
