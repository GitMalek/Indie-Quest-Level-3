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

            while (gameState.combat.monster.hitPoints > 0)
            {
                for (int i = 0; i < gameState.party.characters.Count; i++)
                {
                    int damage = DiceHelper.Roll(gameState.party.characters[i].weaponType.damageRoll);

                    yield return gameState.party.characters[i].presenter.Attack();
                    Console.WriteLine($"{characterNames[i]} hits the {monsterName} with a {gameState.party.characters[i].weaponType.displayName} for {damage} damage. The {monsterName} has {Mathf.Max(0, gameState.combat.monster.hitPoints - damage)} HP left.");
                    yield return gameState.combat.monster.ReactToDamage(damage);

                    if (gameState.combat.monster.hitPoints == 0)
                    {
                        break;
                    }
                }

                if (gameState.combat.monster.hitPoints == 0)
                {
                    break;
                }

                int target = Random.Range(0, gameState.party.characters.Count);
                int weaponChoice = Random.Range(0, gameState.combat.monster.weapons.Count);
                string weaponName = gameState.combat.monster.weapons[weaponChoice].displayName;
                int weaponDamage = DiceHelper.Roll(gameState.combat.monster.weapons[weaponChoice].damageRoll);

                Console.WriteLine($"The {monsterName} attacks {gameState.party.characters[target].displayName} with a {gameState.combat.monster.weapons[weaponChoice].displayName}");

                yield return gameState.combat.monster.presenter.Attack();
                Console.WriteLine($"It does {weaponDamage} damage! {gameState.party.characters[target].displayName} has {Mathf.Max(0, gameState.party.characters[target].hitPoints - weaponDamage)} HP left!");
                yield return gameState.party.characters[target].ReactToDamage(weaponDamage);
                if (gameState.party.characters[target].hitPoints == 0)
                {
                    gameState.party.characters.Remove(gameState.party.characters[target]);
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
