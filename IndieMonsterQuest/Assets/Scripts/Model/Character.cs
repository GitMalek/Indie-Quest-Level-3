using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Character : Creature
    {
        public WeaponType weaponType { get; }
        public ArmorType armorType { get; }

        public override int armorClass => armorType.armorClass;

        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;
        public List<bool> _deathSavingThrows = new List<bool>();

        public override AbilityScores abilityScores { get; } = new AbilityScores();

        public Character(string displayName, Sprite bodySprite, int hitPointsMaximum, SizeCategory sizeCategory, WeaponType weaponType, ArmorType armorType) : base(displayName, bodySprite, sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.hitPointsMaximum = hitPointsMaximum;
            this.sizeCategory = sizeCategory;
            this.weaponType = weaponType;
            this.armorType = armorType;

            for (int i = 1; i <= 6; i++)
            {
                abilityScores[(Ability)i].score = DiceHelper.StatRoll();
            }


            Initialize();
        }

        public override IEnumerator ReactToDamage(int damageAmount, bool wasCriticalHit = false)
        {
            int overflowDamage = damageAmount - hitPoints;
            hitPoints = Mathf.Max(0, hitPoints - damageAmount);

            if (overflowDamage >= hitPointsMaximum)
            {
                Console.WriteLine($"They took {overflowDamage - hitPointsMaximum + 1} more damage than they could handle! They're instantly knocked out.");
                lifeStatus = LifeStatus.Dead;
                presenter.UpdateStableStatus();
                yield return presenter.TakeDamage(true);
                yield return presenter.Die();
            }
            else if (hitPoints == 0 && lifeStatus == LifeStatus.Conscious)
            {
                Console.WriteLine($"They're knocked out!");
                lifeStatus = LifeStatus.UnconsciousUnstable;
                presenter.UpdateStableStatus();
                yield return presenter.TakeDamage();
            }
            else if (hitPoints == 0)
            {
                if (wasCriticalHit)
                {
                    Console.WriteLine("It was a critical hit! They automatically fail two death saving throws!");
                    yield return presenter.TakeDamage();
                    yield return presenter.PerformDeathSavingThrow(false);
                    _deathSavingThrowFailures++;
                    _deathSavingThrows.Add(false);
                }
                else
                {
                    Console.WriteLine("They automatically fail a death saving throw!");
                }
                if (lifeStatus == LifeStatus.UnconsciousStable)
                {
                    Console.WriteLine("They are once again Unstable!");
                    lifeStatus = LifeStatus.UnconsciousUnstable;
                    presenter.UpdateStableStatus();
                }

                if (_deathSavingThrowFailures == 3)
                {
                    yield return Die();
                }
                else
                {
                    yield return presenter.TakeDamage();
                    yield return presenter.PerformDeathSavingThrow(false);
                    _deathSavingThrowFailures++;
                    _deathSavingThrows.Add(false);
                    if (_deathSavingThrowFailures == 3)
                    {
                        yield return Die();
                    }
                }
            }
            else
            {
                yield return presenter.TakeDamage();
            }
        }

        public override IEnumerator Heal(int amount)
        {
            hitPoints = Mathf.Min(hitPointsMaximum, hitPoints + amount);
            yield return presenter.Heal();
            if (lifeStatus != LifeStatus.Conscious)
            {
                lifeStatus = LifeStatus.Conscious;
                ResetDeathSavingThrows();
                yield return presenter.RegainConsciousness();
            }
        }

        public IEnumerator HandleUnconsciousState()
        {
            yield return HandleDeathSavingThrows();
            if (deathSavingThrowFailures == 3)
            {
                yield return Die();
            }
            if (deathSavingThrowSuccesses == 3)
            {
                lifeStatus = LifeStatus.UnconsciousStable;
                presenter.UpdateStableStatus();
                ResetDeathSavingThrows();
            }
        }

        private IEnumerator Die()
        {
            Console.WriteLine("They have failed three deathsaves! They die.");
            lifeStatus = LifeStatus.Dead;
            presenter.UpdateStableStatus();
            yield return presenter.Die();
        }

        private IEnumerator HandleDeathSavingThrows()
        {
            int roll = DiceHelper.Roll("1d20");

            if (roll == 20)
            {
                yield return presenter.PerformDeathSavingThrow(true, roll);
                yield return Heal(1);
                Console.WriteLine($"{displayName} rolled a {roll}, critical success! They're back up and conscious with {hitPoints} HP.");
            }
            else if (roll >= 10)
            {
                yield return presenter.PerformDeathSavingThrow(true, roll);
                Console.WriteLine($"{displayName} rolled a {roll}, they've made a successful death save!");
                _deathSavingThrows.Add(true);
                _deathSavingThrowSuccesses++;
            }
            else if (roll < 10 && roll != 1)
            {
                yield return presenter.PerformDeathSavingThrow(false, roll);
                Console.WriteLine($"{displayName} rolled a {roll}, they fail a death save!");
                _deathSavingThrows.Add(false);
                _deathSavingThrowFailures++;
            }
            else
            {
                yield return presenter.PerformDeathSavingThrow(false, roll);
                yield return presenter.PerformDeathSavingThrow(false, roll);
                Console.WriteLine($"{displayName} rolled a {roll}, critical failure! they fail two death saves!");
                _deathSavingThrows.Add(false);
                _deathSavingThrowFailures++;
                if (_deathSavingThrowFailures != 3)
                {
                    _deathSavingThrows.Add(false);
                    _deathSavingThrowFailures++;
                }
            }
        }

        private void ResetDeathSavingThrows()
        {
            _deathSavingThrows.Clear();
            _deathSavingThrowFailures = 0;
            _deathSavingThrowSuccesses = 0;

            presenter.ResetDeathSavingThrows();
        }

        public override IAction TakeTurn(GameState gameState)
        {
            if (lifeStatus == LifeStatus.Conscious)
            {
                return new AttackAction(this, gameState.combat.monster, weaponType, getAttackModifier(weaponType));
            }
            else
            {
                return new BeUnconsciousAction(this);
            }
        }
    }
}
