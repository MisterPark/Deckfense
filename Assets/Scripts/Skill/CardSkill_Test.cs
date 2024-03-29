using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoblinGames
{
    public class CardSkill_Test : CardSkill
    {
        // Start is called before the first frame update
        void Start()
        {
            ownerCard.Type = Card.CardType.Tower;
            towerSummonPrefabs.Add(Resources.Load<GameObject>("Tower/0001_BasicTower"));
        }

        public override void Use_Start()
        {
            base.Use_Start();
        }

        public override void Use_Update()
        {
            base.Use_Update();
            Use_End();
        }

        public override void Use_End()
        {
            base.Use_End();
        }

        public override void CardBeginDrag(PointerEventData eventData)
        {
            towerSummon = Instantiate(towerSummonPrefabs[0]);
        }

        public override void CardDrag(PointerEventData eventData)
        {
            MoveDummyTowerByDragging(towerSummon);
        }

        public override void CardEndDrag(PointerEventData eventData)
        {
            UseAfterDraggingTheCard();
        }

    }
}
