using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Create New Armor Type")]
    public class ArmorType : ItemType
    {
        public int armorClass;
        public ArmorCategory category;
    }
}
