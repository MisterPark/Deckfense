using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoblinGames
{
    public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum CardType {Tower, Spell, none};
        protected bool isDragging = false;

        //private Vector3 originPosition;
        //[HideInInspector] public Quaternion originRotation;
        public Vector3 originScale;
        [HideInInspector] public Vector3 OriginScale { get { return originScale; } }

        protected Vector3 actionPos;
        protected bool isActionMove = false;
        protected Vector3 vel;
        private bool isMouseHover;
        private float hoverTime;

        private int cardNumber;
        public int CardNumber { get { return cardNumber; } set { cardNumber = value; } }

        private Hand ownerHand;
        public Hand OwnerHand { get { return ownerHand; } set { ownerHand = value; } }

        private int siblingIndex;
        public int SiblingIndex { get { return siblingIndex; } set { siblingIndex = value; } }

        private CardType type;
        public CardType Type { get { return type; } set { type = value; } }

        private Skill cardSkill;
        public Skill CardSkill { get { return cardSkill; } set { cardSkill = value; } }

        protected virtual void Awake()
        {
            hoverTime = 0f;
            originScale = new Vector3(0.6f, 0.6f, 0.6f);
        }

        protected virtual void Update()
        {
            CardMove();
            MouseHover();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!ownerHand.IsCardAvailable)
            {
                return;
            }

            isDragging = true;
            CardZoomIn();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(isDragging)
            {
                transform.position = eventData.position;
            }
            hoverTime = 0f;
            ownerHand.CardBeingDragging = this;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            if (transform.position.y > GameManager.screenHeight * 0.4f)
            {
                ownerHand.CardUse(this); 
            }
            else
            {
                ownerHand.CardBackToOriginPos(this);
            }
            ownerHand.CardBeingDragging = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isDragging)
            {
                return;
            }
            isMouseHover = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoverTime == -1f)  // 카드에 대고있던 1f 초가 지나 확대가 이루어 졌을경우
            {
                ownerHand.CardBackToOriginPos(this);
                transform.SetSiblingIndex(siblingIndex);
            }
            isMouseHover = false;
            hoverTime = 0f;
        }

        private void CardMove()
        {
            if (isActionMove && !isDragging)
            {
                transform.position = Vector3.SmoothDamp(transform.position, actionPos, ref vel, 0.5f);
                if (Vector3.Distance(transform.position, actionPos) < 1f)
                {
                    isActionMove = false;
                }
            }
        }

        private void MouseHover()
        {
            if(isMouseHover && !isActionMove && !isDragging && ownerHand.CardBeingDragging == null)
            {
                if (hoverTime >= 0f)
                {
                    if (hoverTime < 1f)
                    {
                        hoverTime += Time.deltaTime;
                    }
                    else
                    {
                        CardZoomIn();
                        transform.position = new Vector3(transform.position.x, GameManager.screenHeight * 0.15f, transform.position.z);
                        hoverTime = -1;
                        transform.SetAsLastSibling();
                    }
                }
            }
        }

        public void SetActionPos(Vector3 vec3)
        {
            actionPos = vec3;
            isActionMove = true;
        }

        public void CardZoomIn()
        {
            //originPosition = transform.localPosition;
            //originRotation = transform.rotation;
            //originScale = transform.localScale;

            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(1f, 1f);
        }

    }
}
