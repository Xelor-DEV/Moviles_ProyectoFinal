using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GlobalSceneManager : NonPersistentSingleton<GlobalSceneManager>
{
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.isDone == false)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}