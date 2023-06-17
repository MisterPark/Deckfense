using GoblinGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
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

            //Debug.Log("���� ��");
        }

        private void OnSceneChanged(string sceneName)
        {
            if (sceneName == "InGame")
            {
                isGameScene = true;
            }
            else
            {
                isGameScene = false;
            }
        }

    }
}