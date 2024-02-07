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

        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;
        static readonly bool[] _deathSavingThrows = new bool[0];

        public override int armorClass => _armorClass;
        public int _armorClass;

        public Monster(MonsterType type) : base(type.displayName, type.bodySprite, type.sizeCategory)
        {
            displayName = type.displayName;
            bodySprite = type.bodySprite;
            hitPointsMaximum = Mathf.Max(1, DiceHelper.Roll($"{type.hitPointsRoll}"));
            sizeCategory = type.sizeCategory;
            _armorClass = type.armorClass;

            weapons = type.weaponTypes;

            Initialize();
        }

        public override IAction TakeTurn(GameState gameState)
        {
            int targetIndex = Random.Range(0, gameState.party.aliveCharacters.Count);
            int weaponChoice = Random.Range(0, gameState.combat.monster.weapons.Count);

            return new AttackAction(this, gameState.party.aliveCharacters[targetIndex], gameState.combat.monster.weapons[weaponChoice]);
        }
    }
}
