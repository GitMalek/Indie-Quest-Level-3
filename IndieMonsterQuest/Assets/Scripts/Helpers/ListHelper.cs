using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class ListHelper
    {
        public static void Shuffle(List<Creature> creatures)
        {
            for (int i = creatures.Count - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i);
                Creature temp = creatures[i];
                creatures[i] = creatures[j];
                creatures[j] = temp;
            }
        }
    }
}
