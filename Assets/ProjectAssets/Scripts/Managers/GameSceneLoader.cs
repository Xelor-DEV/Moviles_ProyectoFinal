using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameSceneLoader : SingletonPersistent<GameSceneLoader>
{
    [SerializeField] private string mainScene = "ExitGate";
    [SerializeField] private string[] additiveScenes;

    private IEnumerator Start()
    {
        Scene loadingScene = SceneManager.GetActiveScene();

        AsyncOperation mainLoadOp = GlobalSceneManager.Instance.LoadSceneAsyncWithoutActivation(mainScene, true);

        List<AsyncOperation> additiveOps = new List<AsyncOperation>();
        for (int i = 0; i < additiveScenes.Length; ++i)
        {
            AsyncOperation op = GlobalSceneManager.Instance.LoadSceneAsyncWithoutActivation(additiveScenes[i], true);
            additiveOps.Add(op);
        }

        yield return GlobalSceneManager.Instance.WaitUntilAllOperationsReady(mainLoadOp, additiveOps);

        mainLoadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => mainLoadOp.isDone);

        for (int i = 0; i < additiveOps.Count; ++i)
        {
            additiveOps[i].allowSceneActivation = true;
        }

        yield return GlobalSceneManager.Instance.WaitForAllOperations(additiveOps);
 
        GlobalSceneManager.Instance.SetActiveScene(mainScene);

        yield return SceneManager.UnloadSceneAsync(loadingScene);

        Destroy(gameObject);
    }
}