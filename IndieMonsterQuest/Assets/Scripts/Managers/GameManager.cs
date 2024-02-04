using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private CombatManager combatManager;
        private GameState gameState;

        void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            combatManager = combatTransform.GetComponent<CombatManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            NewGame();
            Simulate();
        }

        void NewGame()
        {
            Party party = new Party(new List<Character>
            {
                new Character("Jazlyn"), new Character("Theron"), new Character("Dayana"), new Character("Rolando")
            });

            gameState = new GameState(party);
        }

        void Simulate()
        {
            Monster orc = new Monster("orc", DiceHelper.Roll("2d8+6"), 10);
            Monster azer = new Monster("azer", DiceHelper.Roll("6d8+12"), 12);
            Monster troll = new Monster("troll", DiceHelper.Roll("8d10+20"), 10);

            gameState.EnterCombatWithMonster(orc);
            gameState.EnterCombatWithMonster(azer);
            gameState.EnterCombatWithMonster(troll);

            combatManager.Simulate(gameState);
        }
    }
}
