using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance => instance;
    private static T instance;

    private void Awake()
    {
        if (instance is null)
        {
            DontDestroyOnLoad(gameObject);

            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
