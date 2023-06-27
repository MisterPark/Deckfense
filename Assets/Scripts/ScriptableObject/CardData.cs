using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Object/CardData", order = int.MaxValue)]
    public class CardData : ScriptableObject
    {
        public enum CardKind
        {
            None,
            TestCard
        }

        [SerializeField] private string cardName;       //쓸건지 고민
        [SerializeField] private int cost;
        [SerializeField] private int power;
        [SerializeField] private float range;
        [SerializeField] private float attackSpeed;
        [SerializeField] private CardKind kind;         //완

        public string CardName { get { return cardName; } }
        public int Cost { get { return cost; } }
        public int Power { get { return power; } }
        public float Range { get { return range; } }
        public float AttackSpeed { get { return attackSpeed; } }
        public CardKind Kind { get { return kind; } }
    }
}
