using GoblinGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEvent<string> sceneChangedEvent;

    private bool isGameScene = false;

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
        if (isGameScene == false) return;

        Debug.Log("∞‘¿” ¡ﬂ");
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
}
