using GoblinGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameEvent<string> sceneChangedEvnet;


    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        sceneChangedEvnet.Invoke(arg1.name);
        Debug.Log($"Active Scene Changed: {arg0.name} to {arg1.name}");
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
