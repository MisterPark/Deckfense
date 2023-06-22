using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Skill_Test : Skill
    {
        // Start is called before the first frame update
        void Start()
        {
            towerSummon = Resources.Load<GameObject>("TestDummy_01");
            ownerCard.Type = Card.CardType.Tower;
        }

        public override void Use_Start()
        {
            dummyTower = Instantiate(towerSummon);
        }

        public override void Use_Update()
        {
            PlaceTower();
        }

    }
}
