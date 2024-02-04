using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        public Character(string displayName, Sprite bodySprite, int hitPointsMaximum, SizeCategory sizeCategory) : base(displayName, bodySprite, hitPointsMaximum, sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.hitPointsMaximum = hitPointsMaximum;
            this.sizeCategory = sizeCategory;
        }
    }
}
