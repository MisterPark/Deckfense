using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class TowerCard : Card
    {
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.Tower;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public override void Skill_Init()
        {
            
        }

        public override void Skill_Update()
        {

        }
    }
}
