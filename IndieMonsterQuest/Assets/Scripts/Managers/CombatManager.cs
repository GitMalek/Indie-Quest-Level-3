using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class CombatManager : MonoBehaviour
    {
        public IEnumerator Simulate(GameState gameState)
        {
            Party party = gameState.party;
            List<string> characterNames = new List<string>();

            foreach (Character character in party.characters)
            {
                characterNames.Add(character.displayName);
            }

            string monsterName = gameState.combat.monster.displayName;
            int savingThrowDC = gameState.combat.monster.savingThrowDC;

            Console.WriteLine($"{StringHelper.JoinWithAnd(characterNames)} enter the dungeon.");
            Console.WriteLine($"Watch out, {monsterName} with {gameState.combat.monster.hitPoints} HP appears!");

            while (gameState.combat.monster.hitPoints > 0)
            {
                for (int i = 0; i < gameState.party.characters.Count; i++)
                {
                    int damage = DiceHelper.Roll("2d6");

                    yield return gameState.party.characters[i].presenter.Attack();

                    yield return gameState.combat.monster.ReactToDamage(damage);


                    if (gameState.combat.monster.hitPoints == 0)
                    {
                        Console.WriteLine($"{gameState.party.characters[i].displayName} hits the {monsterName} for {damage} damage. The {monsterName} has {gameState.combat.monster.hitPoints} HP left.");
                        break;
                    }
                    Console.WriteLine($"{gameState.party.characters[i].displayName} hits the {monsterName} for {damage} damage. {monsterName} has {gameState.combat.monster.hitPoints} HP left.");
                }

                if (gameState.combat.monster.hitPoints == 0)
                {
                    break;
                }

                int target = Random.Range(0, gameState.party.characters.Count);

                Console.WriteLine($"The {monsterName} attacks {gameState.party.characters[target].displayName}!");

                int constitutionSave = DiceHelper.Roll("1d20+3");

                if (constitutionSave < savingThrowDC)
                {
                    yield return gameState.combat.monster.presenter.Attack();
                    Console.WriteLine($"{gameState.party.characters[target].displayName} rolls a {constitutionSave} and fails to be saved. {gameState.party.characters[target].displayName} is knocked out.");
                    yield return gameState.party.characters[target].ReactToDamage(10);
                    party.characters.Remove(party.characters[target]);
                }
                else
                {
                    Console.WriteLine($"{gameState.party.characters[target].displayName} rolls a {constitutionSave} and is saved from the attack.");
                }

                if (gameState.party.characters.Count == 0)
                {
                    break;
                }

            }

            if (characterNames.Count == 0)
            {
                Console.WriteLine($"The party has failed and the {monsterName} roams free.");
            }
            else if (gameState.combat.monster.hitPoints == 0)
            {
                Console.WriteLine($"The {monsterName} collapses and the heroes celebrate their victory!");
            }
        }
    }
}
