using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 패 각도 최대 30 ~ -30
// 카드 크기 400, 560 패는 크기 0.5f

namespace GoblinGames
{
    public class Hand : MonoBehaviour
    {
        public static float screenWidth = 1920f;
        public static float screenHeight = 960f;

        private const int maxCardCount = 10;
        private const float maxCardIntervalPos = 360f;
        private Vector3 initCardPos = new Vector3(screenWidth, 0f, 0f);


        private List<Card> hands = new List<Card>();
        public Card usedCard = null; // 나중에 private
        public bool cardAvailable = true;
        
        // Start is called before the first frame update
        void Start()
        {
            sort();
        }

        // Update is called once per frame
        void Update()
        {
            if(usedCard != null)
            {
                usedCard.Skill_Update();
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                Draw();
                sort();
            }
        }

        private void sort()
        {
            int handsCount = hands.Count;
            if (handsCount == 1)
            {
                //hands[0].transform.position = new Vector3(screenWidth * 0.5f, screenHeight * 0.05f, 0f);
                hands[0].SetActionPos(new Vector3(screenWidth * 0.5f, screenHeight * 0.05f, 0f));
                hands[0].transform.rotation = Quaternion.identity;
            }
            else {
                float intervalAngle = 60f / (handsCount - 1);
                float intervalPos = (maxCardIntervalPos * 0.2f + (maxCardIntervalPos * handsCount * 0.14f)) / (handsCount - 1); // 180
                Vector3 cardPos = new Vector3((screenWidth * 0.5f) - (intervalPos * (handsCount - 0.5f - (handsCount / 2f))), screenHeight * 0.05f, 0f);
                for (int i = 0; i < handsCount; i++)
                {
                    //hands[i].transform.position = new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * screenHeight * 0.01f), cardPos.z);
                    hands[i].SetActionPos(new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * screenHeight * 0.01f), cardPos.z));
                    
                    
                    cardPos.x += intervalPos;
                    hands[i].transform.rotation = Quaternion.Euler(0f, 0f, 30f - (intervalAngle * i));
                }
            }
        }

        public void UseCard(Card card)
        {
            usedCard = card;
            usedCard.GetComponent<Image>().enabled = false;
            usedCard.Skill_Init();
            cardAvailable = false;
            //switch(card.cardType)
            //{
            //    case Card.CardType.Tower:
            //        {
                        
            //            break;
            //        }
            //    case Card.CardType.Spell:
            //        {

            //            break;
            //        }
            //}
        }

        public void RemoveCard(Card card)
        {
            Card foundCard = hands.Find(x=>x==card);
            hands.Remove(card);
            Destroy(foundCard.gameObject);
            sort();
        }

        public void Draw()
        {
            GameObject prefab = Resources.Load<GameObject>("Card/Card_0001");
            GameObject newCard = Instantiate(prefab);
            newCard.transform.SetParent(transform);
            newCard.transform.position = initCardPos;
            Card newCardComp = newCard.GetComponent<Card>();
            newCardComp.ownerHand = this;
            newCardComp.cardNumber = 1;
            newCardComp.cardType = Card.CardType.Tower;
            hands.Add(newCardComp);
        }
    }
}
