using UnityEngine;
using System.Collections;

public class GameSceneLoader : SingletonPersistent<GameSceneLoader>
{
    [SerializeField] private string mainScene = "ExitGate";
    [SerializeField] private string[] additiveScenes;

    IEnumerator Start()
    {
        yield return StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(mainScene, false));

        yield return StartCoroutine(GlobalSceneManager.Instance.LoadMultipleAdditiveScenesAsync(additiveScenes));
        GlobalSceneManager.Instance.SetActiveScene(mainScene);
    }
}