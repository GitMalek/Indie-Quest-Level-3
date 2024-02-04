using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class GameState
    {
        public Party party { get; private set; }
        public Combat combat { get; private set; }

        public GameState(Party party)
        {
            this.party = party;
        }

        public void EnterCombatWithMonster(Monster monster)
        {
            combat = new Combat(monster);
        }
    }
}
