using GoblinGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEvent<string> sceneChangedEvent;

    private bool isGameScene = false;
    private bool isPlaceTower = false;
    private GameObject selectedTower = null;

    private void OnEnable()
    {
        sceneChangedEvent.AddListener(OnSceneChanged);
    }

    private void OnDisable()
    {
        sceneChangedEvent.RemoveListener(OnSceneChanged);
    }

    private void Update()
    {
        //if (isGameScene == false) return;

        //Debug.Log("∞‘¿” ¡ﬂ");
        PlaceTower();
    }

    private void OnSceneChanged(string sceneName)
    {
        if(sceneName == "InGame")
        {
            isGameScene = true;
        }
        else
        {
            isGameScene = false;
        }
    }

    private void PlaceTower()
    {
        if(!isPlaceTower)
        {
            if(Input.GetKeyDown(KeyCode.T))
            {
                isPlaceTower = true;
                GameObject prefab = Resources.Load<GameObject>("TestDummy_01");
                selectedTower = Instantiate(prefab);
            }
        }
        else
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

                        isPlaceTower = false;
                        Destroy(selectedTower);
                        selectedTower = null;
                    }
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("TowerTile"))
                    {
                        selectedTower.transform.position = hit.transform.parent.position;
                        selectedTower.transform.Translate(new Vector3(0f, 1f, 0f));
                    }
                    else
                    {
                        selectedTower.transform.position = new Vector3(0f, -100f, 0f);
                    }
                }
            }
        }
    }
}
