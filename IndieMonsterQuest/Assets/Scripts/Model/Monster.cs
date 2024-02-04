using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        public int savingThrowDC { get; private set; }
        public Monster(string displayName, Sprite bodySprite, int hitPointsMaximum, SizeCategory sizeCategory, int savingThrowDC) : base(displayName, bodySprite, hitPointsMaximum, sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.hitPointsMaximum = hitPointsMaximum;
            this.sizeCategory = sizeCategory;
            this.savingThrowDC = savingThrowDC;
        }
    }
}
