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
        [HideInInspector] public Vector3 originScale;

        protected Vector3 actionPos;
        protected bool isActionMove = false;
        protected Vector3 vel;
        private bool isMouseHover;
        private float hoverTime;

        public int cardNumber;
        [HideInInspector] public Hand ownerHand;
        public CardType cardType;
        protected GameObject towerSummon = null;
        protected GameObject dummyTower = null;

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

        public virtual void Skill_Init()
        {
            
        }

        public virtual void Skill_Update()
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!ownerHand.isCardAvailable)
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
            ownerHand.cardBeingDragging = this;
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
            ownerHand.cardBeingDragging = null;
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
            if(isMouseHover && !isActionMove && !isDragging && ownerHand.cardBeingDragging == null)
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
            transform.localScale = new Vector3(1.1f, 1.1f);
        }

        protected void PlaceTower()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("TowerTile"))
                    {
                        TowerTile towerTile = hit.transform.parent.GetComponent<TowerTile>();
                        if (towerTile.tileState != TowerTile.TileState.UnUsed)
                        {
                            return;
                        }

                        GameObject prefab = Resources.Load<GameObject>("TestTower_01");
                        GameObject newTower = Instantiate(prefab);
                        newTower.transform.position = hit.transform.parent.position;
                        newTower.transform.Translate(new Vector3(0f, 1f, 0f));
                        newTower.transform.SetParent(ownerHand.towerField.transform);
                        towerTile.tileState = TowerTile.TileState.Used;

                        //isPlaceTower = false;
                        Destroy(dummyTower);
                        dummyTower = null;
                        ownerHand.CardUseSuccess(this);
                    }
                }
            }
            else if(Input.GetMouseButtonDown(1))
            {
                Destroy(dummyTower);
                dummyTower = null;
                GetComponent<Image>().enabled = true;

                ownerHand.CardUseCancel(this);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("TowerTile"))
                    {
                        dummyTower.transform.position = hit.transform.parent.position;
                        dummyTower.transform.Translate(new Vector3(0f, 1f, 0f));
                    }
                    else
                    {
                        dummyTower.transform.position = new Vector3(0f, -100f, 0f);
                    }
                }
            }
            
        }

    }
}
