using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplicationSingletonLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadSingleton()
    {
        SceneManager.LoadScene("ApplicationSingletonScene", LoadSceneMode.Additive);
    }
}