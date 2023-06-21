using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Card_0001 : TowerCard
    {
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
        }
        
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public override void Skill_Init()
        {
            dummyTower = Instantiate(towerSummon);
        }

        public override void Skill_Update()
        {
            PlaceTower();
        }

    }
}
