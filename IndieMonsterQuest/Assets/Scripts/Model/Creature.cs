using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

namespace MonsterQuest
{
    public abstract class Creature
    {
        public string displayName { get; protected set; }
        public Sprite bodySprite { get; protected set; }
        public int hitPointsMaximum { get; protected set; }


        public int hitPoints { get; protected set; }

        public SizeCategory sizeCategory { get; protected set; }

        public float spaceInFeet => SizeHelper.spaceInFeetPerSizeCategory[sizeCategory];

        public CreaturePresenter presenter { get; private set; }

        public abstract IEnumerable<bool> deathSavingThrows { get; }

        public int deathSavingThrowSuccesses => _deathSavingThrowSuccesses;
        public int _deathSavingThrowSuccesses = 0;

        public int deathSavingThrowFailures => _deathSavingThrowFailures;
        public int _deathSavingThrowFailures = 0;

        public LifeStatus lifeStatus { get; protected set; }

        public abstract int armorClass { get; }

        public abstract AbilityScores abilityScores { get; }

        public Creature(string displayName, Sprite bodySprite, SizeCategory sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.sizeCategory = sizeCategory;

            lifeStatus = LifeStatus.Conscious;
        }

        public void InitializePresenter(CreaturePresenter presenter)
        {
            this.presenter = presenter;
        }

        public virtual IEnumerator ReactToDamage(int damageAmount, bool wasCriticalHit = false)
        {
            hitPoints = Mathf.Max(0, hitPoints - damageAmount);
            yield return presenter.TakeDamage();


            if (hitPoints == 0)
            {
                Console.WriteLine("They die!");
                lifeStatus = LifeStatus.Dead;
                yield return presenter.Die();
            }
        }

        public virtual IEnumerator Heal(int amount)
        {
            hitPoints = Mathf.Min(hitPointsMaximum, hitPoints + amount);
            yield return presenter.Heal();
        }

        public abstract IAction TakeTurn(GameState gameState);

        protected void Initialize()
        {
            hitPoints = hitPointsMaximum;
        }

        public Ability getAttackModifier(WeaponType weaponType)
        {
            if (weaponType.isRanged)
            {
                return Ability.Dexterity;
            }
            else if (weaponType.isFinesse)
            {
                if (abilityScores.dexterity > abilityScores.strength)
                {
                    return Ability.Dexterity;
                }
                else
                {
                    return Ability.Strength;
                }
            }
            else
            {
                return Ability.Strength;
            }
        }
    }
}
