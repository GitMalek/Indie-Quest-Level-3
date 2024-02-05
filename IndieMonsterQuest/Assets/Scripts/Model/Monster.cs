using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public List<WeaponType> weapons;

        public Monster(MonsterType type) : base(type.displayName, type.bodySprite, type.sizeCategory)
        {
            displayName = type.displayName;
            bodySprite = type.bodySprite;
            hitPointsMaximum = DiceHelper.Roll($"{type.hitPointsRoll}");
            sizeCategory = type.sizeCategory;

            weapons = type.weaponTypes;

            Initialize();
        }
    }
}
