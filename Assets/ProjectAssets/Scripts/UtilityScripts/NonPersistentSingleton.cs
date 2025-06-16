using UnityEngine;

public class NonPersistentSingleton<T> : MonoBehaviour where T : NonPersistentSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (T)this;
        }
    }
}