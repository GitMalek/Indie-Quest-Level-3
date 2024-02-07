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
            List<string> characterNames = party.characters.Select(x => x.displayName).ToList();

            string monsterName = gameState.combat.monster.displayName;

            Console.WriteLine($"{StringHelper.JoinWithAnd(characterNames)} proceed deeper into the dungeon.");
            Console.WriteLine($"Watch out, {monsterName} with {gameState.combat.monster.hitPoints} HP appears!");

            List<Creature> turnOrder = new List<Creature>();

            for (int i = 0; i < gameState.party.aliveCharacters.Count; i++)
            {
                turnOrder.Add(gameState.party.aliveCharacters[i]);
            }

            turnOrder.Add(gameState.combat.monster);

            ListHelper.Shuffle(turnOrder);

            int turnIndexer = 0;
            while (gameState.combat.monster.hitPoints > 0 && gameState.party.aliveCharacters.Count > 0)
            {
                gameState.party.aliveCharacters.RemoveAll(character => character.lifeStatus == LifeStatus.Dead);
                turnOrder.RemoveAll(character => character.lifeStatus == LifeStatus.Dead);
                if (gameState.party.aliveCharacters.Count == 0 || gameState.combat.monster.hitPoints == 0)
                {
                    break;
                }
                if (turnIndexer >= turnOrder.Count)
                {
                    turnIndexer = 0;
                }
                yield return turnOrder[turnIndexer].TakeTurn(gameState).Execute();
                turnIndexer++;
            }

            if (gameState.party.aliveCharacters.Count == 0)
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
