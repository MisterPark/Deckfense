using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 패 각도 최대 30 ~ -30
// 카드 크기 400, 560 패는 크기 0.5f

namespace GoblinGames
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private GameEvent<Vector2> resolutionChangedEvent;
        [SerializeField] private GameObject towerField;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject testPrefab;

        private const int maxCardCount = 10;
        private float maxCardIntervalPos = 360f;

        private List<Card> hands = new List<Card>();
        private Card usedCard = null;
        private bool isCardAvailable = true;
        private Card cardBeingDragging = null;

        #region Resolution
        private RectTransform rectTransform;
        #endregion
        public GameObject TowerField { get { return towerField; } set { towerField = value; } }
        public Card CardBeingDragging { get { return cardBeingDragging; } set { cardBeingDragging = value; } }
        public Canvas Canvas { get { return canvas; } }
        public List<Card> Hands { get { return Hands; } }
        public bool IsCardAvailable { get { return isCardAvailable; } set { isCardAvailable = value; } }

        #region Test
        private List<TestCard> testCards = new List<TestCard>();
        #endregion


        private void Awake()
        {
            // 해상도 
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            resolutionChangedEvent.AddListener(OnResolutionChanged);
        }

        private void OnDisable()
        {
            resolutionChangedEvent.RemoveListener(OnResolutionChanged);
        }

        private void Update()
        {
            if (usedCard != null)
            {
                usedCard.CardSkill.Use_Update();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Draw();
                Sort();

            }
        }

        private void Sort()
        {
            int handsCount = hands.Count;
            if (handsCount == 0)
            {
                return;
            }
            else if (handsCount == 1)
            {
                hands[0].SetDestination(new Vector3(rectTransform.rect.width * 0.5f, rectTransform.rect.height * 0.05f, 0f));
                hands[0].transform.rotation = Quaternion.identity;
            }
            else
            {
                float intervalAngle = 60f / (handsCount - 1);
                maxCardIntervalPos = Screen.width / 5.3f;
                float intervalPos = (maxCardIntervalPos * 0.2f + (maxCardIntervalPos * handsCount * 0.14f)) / (handsCount - 1); // 180
                Vector3 cardPos = new Vector3((rectTransform.rect.width * 0.5f) - (intervalPos * (handsCount - 0.5f - (handsCount / 2f))), rectTransform.rect.height * 0.05f, 0f);
                for (int i = 0; i < handsCount; i++)
                {
                    //hands[i].transform.position = new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * screenHeight * 0.01f), cardPos.z);
                    hands[i].SetDestination(new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * rectTransform.rect.height * 0.012f), cardPos.z));

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
                return new Vector3(rectTransform.rect.width * 0.5f, rectTransform.rect.height * 0.05f, 0f);
            }
            else
            {
                maxCardIntervalPos = Screen.width / 5.4f;
                float intervalPos = (maxCardIntervalPos * 0.2f + (maxCardIntervalPos * handsCount * 0.14f)) / (handsCount - 1);
                Vector3 cardPos = new Vector3((rectTransform.rect.width * 0.5f) - (intervalPos * (handsCount - 0.5f - (handsCount / 2f))), rectTransform.rect.height * 0.05f, 0f);
                for (int i = 0; i < handsCount; i++)
                {
                    if (hands[i] != cardToFind)
                    {
                        cardPos.x += intervalPos;
                        continue;
                    }
                    return new Vector3(cardPos.x, cardPos.y - Mathf.Abs(((handsCount - 1) / 2f - i) * rectTransform.rect.height * 0.012f), cardPos.z);
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
            card.SetDestination(GetCardPosInHand(card));

            card.transform.position = new Vector3(rectTransform.rect.width * 0.5f, rectTransform.rect.height * -0.1f, 0f);
            card.transform.rotation = GetCardRotateInHand(card);
            //card.transform.localScale = card.OriginScale;
            card.RectTransform.localScale = new Vector3(1f, 1f, 1f);
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
            Card foundCard = hands.Find(x => x == card);
            hands.Remove(card);
            Destroy(foundCard.gameObject);
            Sort();
        }

        public void Draw()
        {
            GameObject newCard = Instantiate(testPrefab, transform);
            newCard.transform.SetParent(transform);
            newCard.transform.position = new Vector3(Screen.width, 0f);
            Card newCardComp = newCard.GetComponent<Card>();
            newCardComp.OwnerHand = this;
            newCardComp.CardNumber = 1;
            newCardComp.CardSkill = newCard.AddComponent<Skill_Test>();
            newCardComp.CardSkill.OwnerHand = this;
            newCardComp.CardSkill.OwnerCard = newCardComp;
            //newCard.GetComponent<Image>().so
            hands.Add(newCardComp);
        }


        private void OnResolutionChanged(Vector2 resolution)
        {
            // TODO: 해상도 변경시 작성해야 할 코드
            Sort();
        }

    }
}
