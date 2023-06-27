using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Tower : MonoBehaviour
    {

        [SerializeField] protected int cost;
        [SerializeField] protected int power;
        [SerializeField] protected float range;
        [SerializeField] protected float attackSpeed;


        public int Cost { get { return cost; } }
        public int Power { get { return power; } }
        public float Range { get { return range; } }
        public float AttackSpeed { get { return attackSpeed; } }


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
