using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private static Bootstrapper _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoSpawn()
    {
        if (_instance != null) return;

        var prefab = Resources.Load<GameObject>("Bootstrap");
        Instantiate(prefab);
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitServices();
    }

    private void InitServices()
    {
        // Scene-based services here
    }
}