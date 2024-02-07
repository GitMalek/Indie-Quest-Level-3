using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private GameState gameState;

        private CombatManager combatManager;
        private CombatPresenter combatPresenter;

        [SerializeField] private Sprite[] characterSprite = new Sprite[4];
        [SerializeField] private MonsterType[] monsterTypes = new MonsterType[3];

        void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            combatManager = combatTransform.GetComponent<CombatManager>();
            combatPresenter = combatTransform.GetComponent<CombatPresenter>();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return Database.Initialize();
            NewGame();
            yield return Simulate();
        }

        void NewGame()
        {
            List<WeaponType> validWeapons = Database.itemTypes.Where(itemType => itemType is WeaponType { weight: > 0 }).Cast<WeaponType>().ToList();

            Party party = new Party(new List<Character>
            {
                new Character("Jazlyn", characterSprite[0], 10, SizeCategory.Medium, validWeapons[Random.Range(0, validWeapons.Count)], Database.GetItemType<ArmorType>("Studded Leather")), 
                new Character("Theron", characterSprite[1], 10, SizeCategory.Medium, validWeapons[Random.Range(0, validWeapons.Count)], Database.GetItemType<ArmorType>("Studded Leather")), 
                new Character("Dayana", characterSprite[2], 10, SizeCategory.Medium, validWeapons[Random.Range(0, validWeapons.Count)], Database.GetItemType < ArmorType >("Studded Leather")), 
                new Character("Rolando", characterSprite[3], 10, SizeCategory.Medium, validWeapons[Random.Range(0, validWeapons.Count)], Database.GetItemType < ArmorType >("Studded Leather"))
            });

            gameState = new GameState(party);
        }

        IEnumerator Simulate()
        {
            yield return combatPresenter.InitializeParty(gameState);
            Monster Kobold = new Monster(monsterTypes[0]);
            Monster orc = new Monster(monsterTypes[1]);
            Monster azer = new Monster(monsterTypes[2]);
            Monster troll = new Monster(monsterTypes[3]);

            gameState.EnterCombatWithMonster(Kobold);
            yield return combatPresenter.InitializeMonster(gameState);
            yield return combatManager.Simulate(gameState);

            if (gameState.party.aliveCharacters.Count > 0)
            {
                gameState.EnterCombatWithMonster(orc);
                yield return combatPresenter.InitializeMonster(gameState);
                yield return combatManager.Simulate(gameState);
            }

            if (gameState.party.aliveCharacters.Count > 0)
            {
                gameState.EnterCombatWithMonster(azer);
                yield return combatPresenter.InitializeMonster(gameState);
                yield return combatManager.Simulate(gameState);
            }



            if (gameState.party.aliveCharacters.Count > 0)
            {
                gameState.EnterCombatWithMonster(troll);
                yield return combatPresenter.InitializeMonster(gameState);
                yield return combatManager.Simulate(gameState);
            }

            if (gameState.party.aliveCharacters.Count == 0)
            {
                Console.WriteLine($"The party was defeated.");
            }
            else if (gameState.party.aliveCharacters.Count != 1)
            {
                Console.WriteLine($"After 3 battles, {StringHelper.JoinWithAnd(gameState.party.aliveCharacters.Select(x => x.displayName).ToList())} emerge victorious.");
            }
            else
            {
                Console.WriteLine($"After 3 battles, {gameState.party.aliveCharacters[0].displayName} emerges victorious.");
            }
        }
    }
}
