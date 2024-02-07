using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class BeUnconsciousAction : IAction
    {
        private Character character;

        public BeUnconsciousAction(Character character)
        {
            this.character = character;
        }

        public IEnumerator Execute()
        {
            if (character.lifeStatus == LifeStatus.UnconsciousUnstable)
            {
                yield return character.HandleUnconsciousState();
            }
            else
            {
                Console.WriteLine($"{character.displayName} is currently stable. They don't have to make death saves.");
            }
        }
    }
}
