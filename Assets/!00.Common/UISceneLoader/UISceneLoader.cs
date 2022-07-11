using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneLoader : MonoSingleton<UISceneLoader>
{
    private static readonly string[] IGNORESCENES = {"UIScene", "StartScene"};
    private const string UISCENE = "UIScene";
    private const string CAMNAME = "UICam";
    private const string NEWGAMESCENE = "NewGameScene";
    static AsyncOperation _async = new AsyncOperation();
    private static Camera _cam;


    [RuntimeInitializeOnLoadMethod]
    private static void LoadingUIScene()
    {
        _cam = GameObject.Find("UICam").GetComponent<Camera>();
        UICam = _cam;
        
        StartAfterCor(Instance);
    }

    private static bool CheckIgnoreScene(){
        for (int i = 0; i < IGNORESCENES.Length; ++i){
            if(SceneManager.GetActiveScene().name.Equals(IGNORESCENES[i])){
                return true;
            }
        }
        return false;
    }

    public static void StartAfterCor(MonoBehaviour script)
    {
        script.StartCoroutine(AfterLoadUIScene());
    }

    public static IEnumerator AfterLoadUIScene()
    {
        yield return WaitUntil(() => SceneManager.GetActiveScene().name.Equals(NEWGAMESCENE));
        _async = SceneManager.LoadSceneAsync(UISCENE, LoadSceneMode.Additive);
        yield return WaitUntil(() => _async.isDone);
        GameObject[] objs = SceneManager.GetSceneByName(UISCENE).GetRootGameObjects();
        for (int i = 0; i < objs.Length; i++)
        {
            SceneManager.MoveGameObjectToScene(objs[i], SceneManager.GetActiveScene());
        }

        SceneManager.UnloadSceneAsync(UISCENE);
    }




}
