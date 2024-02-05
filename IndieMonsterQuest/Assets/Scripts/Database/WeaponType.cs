using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Create New Weapon Type")]
    public class WeaponType : ItemType
    {
        public string damageRoll;
    }
}
