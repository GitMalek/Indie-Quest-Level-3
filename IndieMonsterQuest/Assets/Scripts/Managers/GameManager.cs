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
        [SerializeField] private Sprite[] monsterSprite = new Sprite[3];

        void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            combatManager = combatTransform.GetComponent<CombatManager>();
            combatPresenter = combatTransform.GetComponent<CombatPresenter>();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            NewGame();
            yield return Simulate();
        }

        void NewGame()
        {
            Party party = new Party(new List<Character>
            {
                new Character("Jazlyn", characterSprite[0], 10, SizeCategory.Medium), 
                new Character("Theron", characterSprite[1], 10, SizeCategory.Medium), 
                new Character("Dayana", characterSprite[2], 10, SizeCategory.Medium), 
                new Character("Rolando", characterSprite[3], 10, SizeCategory.Medium)
            });

            gameState = new GameState(party);
        }

        IEnumerator Simulate()
        {
            combatPresenter.InitializeParty(gameState);
            Monster orc = new Monster("orc", monsterSprite[0], DiceHelper.Roll("2d8+6"), SizeCategory.Medium, 10);
            Monster azer = new Monster("azer", monsterSprite[1], DiceHelper.Roll("6d8+12"), SizeCategory.Medium, 12);
            Monster troll = new Monster("troll", monsterSprite[2], DiceHelper.Roll("8d10+20"), SizeCategory.Large, 10);

            gameState.EnterCombatWithMonster(orc);
            combatPresenter.InitializeMonster(gameState);

            yield return combatManager.Simulate(gameState);

            if (gameState.party.characters.Count > 0)
            {
                gameState.EnterCombatWithMonster(azer);
                combatPresenter.InitializeMonster(gameState);
                yield return combatManager.Simulate(gameState);
            }



            if (gameState.party.characters.Count > 0)
            {
                gameState.EnterCombatWithMonster(troll);
                combatPresenter.InitializeMonster(gameState);
                yield return combatManager.Simulate(gameState);
            }

            if (gameState.party.characters.Count == 0)
            {
                Console.WriteLine($"The party was defeated.");
            }
            else
            {
                Console.WriteLine($"After 3 battles, the party emerges victorious.");
            }
        }
    }
}
