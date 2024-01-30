using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        private CombatManager combatManager;
        void Awake()
        {
            Transform combatTransform = transform.Find("Combat");
            combatManager = combatTransform.GetComponent<CombatManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            List<string> characterNames = new List<string>();
            characterNames.Add("Jazlyn");
            characterNames.Add("Theron");
            characterNames.Add("Dayana");
            characterNames.Add("Rolando");

            int orcHP = DiceHelper.Roll("2d8+6");
            int orcDC = 10;

            int azerHP = DiceHelper.Roll("6d8+12");
            int azerDC = 12;

            int trollHP = DiceHelper.Roll("8d10+20");
            int trollDC = 14;

            combatManager.Simulate(characterNames, "orc", orcHP, orcDC);
            if (characterNames.Count > 0)
            {
                combatManager.Simulate(characterNames, "azer", azerHP, azerDC);
            }
            if (characterNames.Count > 0)
            {
                combatManager.Simulate(characterNames, "troll", trollHP, trollDC);
            }

            if (characterNames.Count == 1)
            {
                Console.WriteLine($"After three grueling battles, the hero {StringHelper.JoinWithAnd(characterNames)} returns from the dungeons to live another day.");
            }
            else if (characterNames.Count != 0)
            {
                Console.WriteLine($"After three grueling battles, the heroes {StringHelper.JoinWithAnd(characterNames)} return from the dungeons to live another day.");    
            }
        }
    }
}
