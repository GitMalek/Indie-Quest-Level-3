using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(menuName = "Create New Monster Type")]
    public class MonsterType : ScriptableObject
    {
        [SerializeField] public string displayName;
        [SerializeField] public string alignment;
        [SerializeField] public string hitPointsRoll;
        [SerializeField] public int armorClass;
        [SerializeField] public Sprite bodySprite;
        [SerializeField] public SizeCategory sizeCategory;
        [SerializeField] public ArmorType armorType;
        [SerializeField] public List<WeaponType> weaponTypes;
        [SerializeField] public AbilityScores abilityScores;
    }
}
