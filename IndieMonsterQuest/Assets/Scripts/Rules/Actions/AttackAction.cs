using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class AttackAction : IAction
    {
        private Creature attacker;
        private Creature target;

        private WeaponType weaponType;
        int attackRoll;

        public AttackAction(Creature attacker, Creature target, WeaponType weaponType)
        {
            this.attacker = attacker;
            this.target = target;
            this.weaponType = weaponType;
        }

        public IEnumerator Execute()
        {
            yield return attacker.presenter.FaceCreature(target);
            yield return attacker.presenter.Attack();

            if (target.lifeStatus != LifeStatus.UnconsciousUnstable && target.lifeStatus != LifeStatus.UnconsciousStable)
            {
                attackRoll = DiceHelper.Roll("1d20");
                Console.WriteLine($"{attacker.displayName} rolled a {attackRoll} to hit!");

                if (attackRoll == 20)
                {
                    Console.WriteLine("Critical hit!");
                    int damage = DiceHelper.Roll(weaponType.damageRoll) + DiceHelper.Roll(weaponType.damageRoll);
                    Console.WriteLine($"{attacker.displayName} hit with a {weaponType.displayName} for {damage} damage! {target.displayName} has {Mathf.Max(0, target.hitPoints - damage)} HP left.");
                    yield return target.ReactToDamage(damage, true);
                }
                else if (attackRoll >= target.armorClass)
                {
                    Console.WriteLine($"It beats the {target.displayName}'s {target.armorClass} AC!");
                    int damage = DiceHelper.Roll(weaponType.damageRoll);
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
                int damage = DiceHelper.Roll(weaponType.damageRoll) + DiceHelper.Roll(weaponType.damageRoll);
                Console.WriteLine($"{attacker.displayName} hit with a {weaponType.displayName} for {damage} damage! {target.displayName} has {Mathf.Max(0, target.hitPoints - damage)} HP left.");
                yield return target.ReactToDamage(damage, true);
            }
        }
    }
}
