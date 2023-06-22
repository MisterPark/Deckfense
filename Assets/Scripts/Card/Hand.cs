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
        private const int maxCardCount = 10;
        private const float maxCardIntervalPos = 360f;
        private Vector3 firstCardPos = new Vector3(GameManager.screenWidth, 0f, 0f);


        private List<Card> hands = new List<Card>();
        public List<Card> Hands { get { return Hands; } }
        private Card usedCard = null;
        private bool isCardAvailable = true;
        [HideInInspector] public bool IsCardAvailable { get{ return isCardAvailable; } set { isCardAvailable = value; } }

        [SerializeField] GameObject towerField;
        public GameObject TowerField { get { return towerField; } set { towerField = value; } }

        private Card cardBeingDragging = null;
        public Card CardBeingDragging { get { return cardBeingDragging; } set { cardBeingDragging = value; } }
        //public Card cardBeingHovering = null;
        [SerializeField] Canvas canvas;
        public Canvas Canvas { get { return canvas; } }


        // public 변수들 private 으로, get set 함수 다 만들것 // 프로퍼티로
        // card 스크립에 skill 클래스들어갈 변수 만들어주고, skill_fireball 처럼 하위클래스 만들고, card의 skill 클래스 변수에 넣기
        // 해상도에 따라 카드 위치, 크기 조정하기
        // 마우스댔을때 카드 맨앞으로 // 해결


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(usedCard != null)
            {
                usedCard.CardSkill.Use_Update();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Draw();
                sort();
                Debug.Log(canvas.pixelRect.width);
            }
        }

        private void sort()
        {
            int handsCount = hands.Count;
            if (handsCount == 0)
            {
                return;
            }
            else if (handsCount == 1)
            {
                //hands[0].transform.position = new Vector3(screenWidth * 0.5f, screenHeight * 0.05f, 0f);
                hands[0].SetActionPos(new Vector3(GameManager.screenWidth * 0.5f, GameManager.screenHeight * 0.05f, 0f));
                hands[0].transform.rotation = Quaternion.identity;
            }
            else {
                float intervalAngle = 60f / (handsCount - 1);
                float intervalPos = (maxCardIntervalPos * 0.2f + (maxCardIntervalPos * handsCount * 0.14f)) / (handsCount - 1); // 180
                Vector3 cardPos = new Vector3((GameManager.screenWidth * 0.5f) - (intervalPos * (handsCount - 0.5f - (handsCount / 2f))), GameManager.screenHeight * 0.05f, 0f);
                for (int i = 0; i < handsCount; i++)
                {
                    //hands[i].transform.position = new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * screenHeight * 0.01f), cardPos.z);
                    hands[i].SetActionPos(new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * GameManager.screenHeight * 0.012f), cardPos.z));
                    
                    cardPos.x += intervalPos;
                    hands[i].transform.rotation = Quaternion.Euler(0f, 0f, 30f - (intervalAngle * i));
                    hands[i].SiblingIndex = i;
                }
            }
        }

        public Vector3 GetCardPosInHand(Card cardToFind)
        {
            int handsCount = hands.Count;
            if (handsCount == 0)
            {
                return Vector3.zero;
            }
            else if (handsCount == 1)
            {
                return new Vector3(GameManager.screenWidth * 0.5f, GameManager.screenHeight * 0.05f, 0f);
            }
            else
            {
                float intervalPos = (maxCardIntervalPos * 0.2f + (maxCardIntervalPos * handsCount * 0.14f)) / (handsCount - 1);
                Vector3 cardPos = new Vector3((GameManager.screenWidth * 0.5f) - (intervalPos * (handsCount - 0.5f - (handsCount / 2f))), GameManager.screenHeight * 0.05f, 0f);
                for (int i = 0; i < handsCount; i++)
                {
                    if(hands[i] != cardToFind)
                    {
                        cardPos.x += intervalPos;
                        continue;
                    }
                    return new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * GameManager.screenHeight * 0.012f), cardPos.z);
                }
            }
            return Vector3.zero;
        }

        public Quaternion GetCardRotateInHand(Card cardToFind)
        {
            int handsCount = hands.Count;
            if (handsCount <= 1)
            {
                return Quaternion.identity;
            }
            else
            {
                float intervalAngle = 60f / (handsCount - 1);
                for (int i = 0; i < handsCount; i++)
                {
                    if (hands[i] != cardToFind)
                    {
                        continue;
                    }
                    return Quaternion.Euler(0f, 0f, 30f - (intervalAngle * i));
                }
            }
            return Quaternion.identity;
        }

        public void CardBackToOriginPos(Card card)
        {
            card.SetActionPos(GetCardPosInHand(card));

            card.transform.position = new Vector3(GameManager.screenWidth * 0.5f, GameManager.screenHeight * -0.1f, 0f);
            card.transform.rotation = GetCardRotateInHand(card);
            card.transform.localScale = card.OriginScale;
            card.transform.SetSiblingIndex(card.SiblingIndex);
        }

        public void CardUse(Card _usedCard)
        {
            usedCard = _usedCard;
            usedCard.GetComponent<Image>().enabled = false;
            usedCard.CardSkill.Use_Start();
            isCardAvailable = false;
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

        public void CardUseSuccess(Card _usedCard)
        {
            RemoveCard(usedCard);
            usedCard = null;
            isCardAvailable = true;
        }

        public void CardUseCancel(Card _usedCard)
        {
            usedCard = null;
            isCardAvailable = true;
            CardBackToOriginPos(_usedCard);
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
            GameObject prefab = Resources.Load<GameObject>("Card/Card");
            GameObject newCard = Instantiate(prefab);
            newCard.transform.SetParent(transform);
            newCard.transform.position = firstCardPos;
            Card newCardComp = newCard.GetComponent<Card>();
            newCardComp.OwnerHand = this;
            newCardComp.CardNumber = 1;
            newCardComp.CardSkill = newCard.AddComponent<Skill_Test>();
            newCardComp.CardSkill.OwnerHand = this;
            newCardComp.CardSkill.OwnerCard = newCardComp;
            //newCard.GetComponent<Image>().so
            hands.Add(newCardComp);
        }

    }
}
