using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GoblinGames
{
    public class Skill : MonoBehaviour
    {

        [SerializeField] protected List<GameObject> towerSummonPrebs = null;
        protected GameObject towerSummon = null;

        protected Hand ownerHand;
        protected Card ownerCard;
        protected int cardNumber;

        protected int skillProgress;
        

        public Hand OwnerHand { get { return ownerHand; } set { ownerHand = value; } }
        public Card OwnerCard { get { return ownerCard; } set { ownerCard = value; } }
        public int CardNumber { get { return cardNumber; } set { cardNumber = value; } }

        // ��Ÿ��, ����Ÿ�� 1���� �����, �Ұ���
        // Ÿ���� ��Ÿ�� ��ũ��Ʈ, ����Ÿ�� ��ũ��Ʈ 1���� �ڰ�, ó���� ����Ÿ����ũ��Ʈ�� Ȱ��ȭ, ��ȯ�� ��Ÿ�� ��ũ��Ʈ�� Ȱ��ȭ �صѰ���.
        public virtual void Use_Start()
        {
            
        }

        public virtual void Use_Update()
        {
        
        }

        public virtual void Use_End()
        {
            ownerHand.CardUseEnd(ownerCard);
        }

        public virtual void CardBeginDrag(PointerEventData eventData)
        {

        }

        public virtual void CardDrag(PointerEventData eventData)
        {

        }

        public virtual void CardEndDrag(PointerEventData eventData)
        {

        }

        protected bool PlaceTowersOnScreenPoint() // Ÿ���� ���콺Ŀ�� �ִ� �ʿ� ��ġ
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
                        return false;
                    }

                    ConvertDummyTower(towerSummon);
                    towerSummon.transform.SetParent(ownerHand.TowerField.transform);
                    towerTile.tileState = TowerTile.TileState.Used;
                    //newTower.transform.position = hit.transform.parent.position;
                    //newTower.transform.Translate(new Vector3(0f, 1f, 0f));

                    return true;
                }
                else
                {
                    UseCancel();
                    return false;
                }
            }
            return false;
        }

        protected void ConvertDummyTower(GameObject tower)
        {
            tower.GetComponent<DummyTower>().enabled = false;
            tower.GetComponent<Tower>().enabled = true;
            tower.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        protected void UseCancel()
        {
            Destroy(towerSummon);
            towerSummon = null;
            ownerHand.CardUseCancel(ownerCard);
        }

        protected void MethodPlaceTower_2() // Test
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    PlaceTowersOnScreenPoint();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ConvertDummyTower(towerSummon);
                ownerCard.GetComponent<Image>().enabled = true;

                ownerHand.CardUseCancel(ownerCard);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("TowerTile"))
                    {
                        towerSummon.transform.position = hit.transform.parent.position;
                        towerSummon.transform.Translate(new Vector3(0f, 1f, 0f));
                    }
                    else
                    {
                        towerSummon.transform.position = new Vector3(0f, -100f, 0f);
                    }
                }
            }
        }

        protected bool DragCardOntoTheField() // ī�带 �ʵ����� �巡���ߴ���
        {
            if (transform.position.y > Screen.height * Card.heightRequiredUseCard)
            {
                return true;
            }
            return false;
        }

        protected void MoveDummyTowerByDragging(GameObject tower) // �巡�� ���߿� ����Ÿ�� �̵�
        {
            if (DragCardOntoTheField())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("TowerTile"))
                    {
                        tower.transform.position = hit.transform.parent.position;
                        tower.transform.Translate(new Vector3(0f, 1f, 0f));
                    }
                    else
                    {
                        tower.transform.position = new Vector3(0f, -100f, 0f);
                    }
                }
            }
            else
            {
                tower.transform.position = new Vector3(0f, -100f, 0f);
            }
        }

        protected void UseAfterDraggingTheCard() // ī��巡����, �ʵ忡 �ִٸ� ���, �׿ܴ� ���
        {
            if (DragCardOntoTheField())
            {
                if (PlaceTowersOnScreenPoint())
                {
                    ownerHand.CardUse(OwnerCard);
                }
                else
                {
                    UseCancel();
                }
            }
            else
            {
                UseCancel();
            }
        }
    }
}
