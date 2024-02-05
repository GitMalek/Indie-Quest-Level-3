using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Creature
    {
        public string displayName { get; protected set; }
        public Sprite bodySprite { get; protected set; }
        public int hitPointsMaximum { get; protected set; }


        public int hitPoints { get; protected set; }

        public SizeCategory sizeCategory { get; protected set; }

        public float spaceInFeet => SizeHelper.spaceInFeetPerSizeCategory[sizeCategory];

        public CreaturePresenter presenter { get; private set; }


        public Creature(string displayName, Sprite bodySprite, SizeCategory sizeCategory)
        {
            this.displayName = displayName;
            this.bodySprite = bodySprite;
            this.sizeCategory = sizeCategory;
        }

        public void InitializePresenter(CreaturePresenter presenter)
        {
            this.presenter = presenter;
        }

        public IEnumerator ReactToDamage(int damageAmount)
        {
            hitPoints = Mathf.Max(0, hitPoints - damageAmount);
            yield return presenter.TakeDamage();
            if (hitPoints == 0)
            {
                Console.WriteLine("They're knocked out!");
                yield return presenter.Die();
            }
        }

        protected void Initialize()
        {
            hitPoints = hitPointsMaximum;
        }
    }
}
