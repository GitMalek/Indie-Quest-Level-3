using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        private Creature attacker;
        private Creature target;

        private WeaponType weaponType;
        private int attackRoll;

        private Ability? ability;


        public AttackAction(Creature attacker, Creature target, WeaponType weaponType, Ability? ability = null)
        {
            this.attacker = attacker;
            this.target = target;
            this.weaponType = weaponType;
            this.ability = ability;
        }

        public IEnumerator Execute()
        {
            yield return attacker.presenter.FaceCreature(target);
            yield return attacker.presenter.Attack();

            attackRoll = DiceHelper.Roll("1d20");

            Ability weaponAbility = (Ability)ability;
            int attackModifier = attacker.abilityScores[weaponAbility].modifier;
            int toHit = attackRoll + attackModifier;

            Console.WriteLine($"{attacker.displayName} rolled a {attackRoll}! with their {ability} bonus of {attackModifier}, it's a {toHit} to hit!");

            if (target.lifeStatus != LifeStatus.UnconsciousUnstable && target.lifeStatus != LifeStatus.UnconsciousStable)
            {
                if (attackRoll == 20)
                {
                    Console.WriteLine("Critical hit!");
                    int damage = DiceHelper.Roll(weaponType.damageRoll) + DiceHelper.Roll(weaponType.damageRoll) + attackModifier;
                    Console.WriteLine($"{attacker.displayName} hit with a {weaponType.displayName} for {damage} damage! {target.displayName} has {Mathf.Max(0, target.hitPoints - damage)} HP left.");
                    yield return target.ReactToDamage(damage, true);
                }
                else if (toHit >= target.armorClass)
                {
                    Console.WriteLine($"It beats the {target.displayName}'s {target.armorClass} AC!");
                    int damage = DiceHelper.Roll(weaponType.damageRoll) + attackModifier;
                    Console.WriteLine($"{attacker.displayName} hit with a {weaponType.displayName} for {damage} damage! {target.displayName} has {Mathf.Max(0, target.hitPoints - damage)} HP left.");
                    yield return target.ReactToDamage(damage);

                }
                else if (attackRoll == 1)
                {
                    Console.WriteLine($"Critical miss!");
                }
                else
                {
                    Console.WriteLine($"It fails to beat the {target.displayName}'s {target.armorClass} AC, it misses!");
                }
            }
            else
            {
                int damage = DiceHelper.Roll(weaponType.damageRoll) + DiceHelper.Roll(weaponType.damageRoll) + attackModifier;
                Console.WriteLine($"{attacker.displayName} hit with a {weaponType.displayName} for {damage} damage! {target.displayName} has {Mathf.Max(0, target.hitPoints - damage)} HP left.");
                yield return target.ReactToDamage(damage, true);
            }
        }
    }
}
