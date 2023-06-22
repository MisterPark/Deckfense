using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GoblinGames
{
    public class Skill : MonoBehaviour
    {

        protected GameObject towerSummon = null;
        protected GameObject dummyTower = null;
        protected Hand ownerHand;
        public Hand OwnerHand { get { return ownerHand; } set { ownerHand = value; } }
        protected Card ownerCard;
        public Card OwnerCard { get { return ownerCard; } set { ownerCard = value; } }

        public virtual void Use_Start()
        {
            
        }

        public virtual void Use_Update()
        {
        
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
                        newTower.transform.SetParent(ownerHand.TowerField.transform);
                        towerTile.tileState = TowerTile.TileState.Used;

                        //isPlaceTower = false;
                        Destroy(dummyTower);
                        dummyTower = null;
                        ownerHand.CardUseSuccess(ownerCard);
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Destroy(dummyTower);
                dummyTower = null;
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
