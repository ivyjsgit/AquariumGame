using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        _instance = this as T;
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}