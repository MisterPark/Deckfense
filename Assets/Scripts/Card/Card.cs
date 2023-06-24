using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoblinGames
{
    public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum CardType {Tower, Spell, none};

        //private Vector3 originPosition;
        //[HideInInspector] public Quaternion originRotation;
        //public Vector3 originScale;
        //[HideInInspector] public Vector3 OriginScale { get { return originScale; } }
        protected bool isDragging = false;
        protected Vector3 destination;
        protected bool isActionMove = false;
        protected Vector3 vel;
        private bool isMouseHover;
        private float hoverTime;

        private int cardNumber;
        private Hand ownerHand;
        private int siblingIndex;
        private CardType type;
        private Skill cardSkill;
        private RectTransform rectTransform;


        public int CardNumber { get { return cardNumber; } set { cardNumber = value; } }
        public Hand OwnerHand { get { return ownerHand; } set { ownerHand = value; } }
        public int SiblingIndex { get { return siblingIndex; } set { siblingIndex = value; } }
        public CardType Type { get { return type; } set { type = value; } }
        public Skill CardSkill { get { return cardSkill; } set { cardSkill = value; } }
        public RectTransform RectTransform { get { return rectTransform; } }


        protected virtual void Awake()
        {
            hoverTime = 0f;
            //originScale = new Vector3(0.6f, 0.6f, 0.6f);
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            CardMove();
            MouseHover();
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log($"{transform.position.x}x{transform.position.y}");
                Debug.Log($"{transform.localPosition.x}x{transform.localPosition.y}");
                Debug.Log($"{transform.localPosition.x}x{transform.localPosition.y}");

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.position = new Vector3(0f, 0f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            if(Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log($"{rectTransform.anchoredPosition.x}x{rectTransform.anchoredPosition.y}");
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ownerHand.IsCardAvailable)
            {
                return;
            }

            isDragging = true;
            CardZoomIn();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                transform.position = eventData.position;
            }
            hoverTime = 0f;
            ownerHand.CardBeingDragging = this;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            if (transform.position.y > Screen.height * 0.25f)
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
            if (isDragging)
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
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref vel, 0.5f);
                if (Vector3.Distance(transform.position, destination) < 1f)
                {
                    isActionMove = false;
                }
            }
        }

        private void MouseHover()
        {
            if (isMouseHover && !isActionMove && !isDragging && ownerHand.CardBeingDragging == null)
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
                        transform.position = new Vector3(transform.position.x, Screen.height * 0.15f, transform.position.z);
                        hoverTime = -1;
                        transform.SetAsLastSibling();
                    }
                }
            }
        }

        public void SetDestination(Vector3 vec3)
        {
            destination = vec3;
            isActionMove = true;
        }

        public void CardZoomIn()
        {
            //originPosition = transform.localPosition;
            //originRotation = transform.rotation;
            //originScale = transform.localScale;

            transform.rotation = Quaternion.identity;
            rectTransform.localScale = new Vector3(1.4f, 1.4f, 1f);
        }

    }
}
