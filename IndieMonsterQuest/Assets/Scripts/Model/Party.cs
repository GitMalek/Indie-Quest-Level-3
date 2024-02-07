using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Party
    {
        public List<Character> characters {  get; private set; }
        public List<Character> aliveCharacters {  get; private set; }
        public Party(IEnumerable<Character> initialCharacters)
        {
            characters = initialCharacters.ToList();
            aliveCharacters = initialCharacters.ToList();
        }
    }
}
