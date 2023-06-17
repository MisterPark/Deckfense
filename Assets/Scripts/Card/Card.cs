using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoblinGames
{
    public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        public enum CardType {Tower, Spell, none};
        private bool isDragging = false;

        private Vector3 originPosition;
        private Quaternion originRotation;
        private Vector3 originScale;

        private Vector3 actionPos;
        private bool isActionMove = false;
        private Vector3 vel;

        public int cardNumber;
        public Hand ownerHand;
        public CardType cardType;
        private GameObject towerSummon = null;
        private GameObject dummyTower = null;

        void Start()
        {
            towerSummon = Resources.Load<GameObject>("TestDummy_01");
        }

        void Update()
        {
            if (isActionMove && !isDragging)
            {
                transform.position = Vector3.SmoothDamp(transform.position, actionPos, ref vel, 0.5f);
                if(Vector3.Distance(transform.position, actionPos) < 3f)
                {
                    isActionMove = false;
                }
            }
        }

        public void Skill_Init()
        {
            dummyTower = Instantiate(towerSummon);
        }

        public void Skill_Update()
        {
            PlaceTower();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!ownerHand.cardAvailable)
            {
                return;
            }

            isDragging = true;

            originPosition = transform.localPosition;
            originRotation = transform.localRotation;
            originScale = transform.localScale;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1.2f, 1.2f);

            //Debug.Log("Begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(isDragging)
            {
                transform.localPosition = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            if (transform.localPosition.y > Hand.screenHeight * 0.4f)
            {
                ownerHand.UseCard(this); 
            }
            else
            {
                transform.localPosition = originPosition;
                transform.localRotation = originRotation;
                transform.localScale = originScale;
            }

            //Debug.Log("End");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void SetActionPos(Vector3 vec3)
        {
            actionPos = vec3;
            isActionMove = true;
        }

        private void PlaceTower()
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
                        towerTile.tileState = TowerTile.TileState.Used;

                        //isPlaceTower = false;
                        Destroy(dummyTower);
                        dummyTower = null;
                        //// Hand 쪽에서 공통적으로 처리해도 될듯
                        ownerHand.RemoveCard(this);
                        ownerHand.cardAvailable = true;
                        ////
                    }
                }
            }
            else if(Input.GetMouseButtonDown(1))
            {
                // 카드취소부분, Hand쪽과 Card쪽 뺼수있는부분 각각 나눌것
                Destroy(dummyTower);
                dummyTower = null;
                ownerHand.cardAvailable = true;
                GetComponent<Image>().enabled = true;
                ownerHand.usedCard = null;

                transform.localPosition = originPosition;
                transform.localRotation = originRotation;
                transform.localScale = originScale;
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
