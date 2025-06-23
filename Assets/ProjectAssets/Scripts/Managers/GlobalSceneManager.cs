using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GlobalSceneManager : NonPersistentSingleton<GlobalSceneManager>
{
    private List<AsyncOperation> activeOperations = new List<AsyncOperation>();

    public IEnumerator LoadSceneAsync(string sceneName, bool isAdditive = false)
    {
        LoadSceneMode mode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        asyncLoad.allowSceneActivation = false;
        activeOperations.Add(asyncLoad);

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        activeOperations.Remove(asyncLoad);
    }

    public IEnumerator LoadMultipleAdditiveScenesAsync(string[] sceneNames)
    {
        List<AsyncOperation> loadOperations = new List<AsyncOperation>();

        foreach (string sceneName in sceneNames)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            loadOperations.Add(asyncLoad);
            activeOperations.Add(asyncLoad);
        }

        bool allScenesReady = false;

        while (!allScenesReady)
        {
            allScenesReady = true;
            foreach (AsyncOperation op in loadOperations)
            {
                if (op.progress < 0.9f)
                {
                    allScenesReady = false;
                    break;
                }
            }
            yield return null;
        }

        foreach (AsyncOperation op in loadOperations)
        {
            op.allowSceneActivation = true;
        }

        foreach (AsyncOperation op in loadOperations)
        {
            while (!op.isDone)
            {
                yield return null;
            }
            activeOperations.Remove(op);
        }
    }

    public void SetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
        }
    }
}