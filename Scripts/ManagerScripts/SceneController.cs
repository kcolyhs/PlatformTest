using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public delegate void loadAction();
    public static loadAction onSceneLoad;
    public static loadAction onSceneUnload;
    public static GameObject mainCamera;

    static int currentSceneID;
    static SceneController instance;



	void Start()
	{
        mainCamera = GameObject.FindGameObjectWithTag("Side Camera");
        if (instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(gameObject);
            instance = this;
            currentSceneID = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneID == 0)
            {
                SceneManager.LoadScene(2, LoadSceneMode.Additive);
                currentSceneID = 2;
                
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
            }
            if(mainCamera!=null)
            mainCamera.SetActive(false);
        }
	}

    public static SceneController GetInstance()
    {
        return SceneController.instance;
    }
    public int GetCurrentSceneId()
    {
        return currentSceneID;
    }
    public  IEnumerator SwitchScene(int sceneID)
    {
        if (onSceneUnload != null)
            onSceneUnload.Invoke();
        SceneManager.UnloadSceneAsync(currentSceneID);

        currentSceneID = sceneID;

        yield return StartCoroutine("LoadScene",sceneID);

        if (onSceneLoad != null)
            onSceneLoad.Invoke();

        yield return null;
        yield break;
    }
    public IEnumerator  EnterBattle(int battleID,int returnID,GameObject[] enemies)
    {
        yield return StartCoroutine(SwitchScene(battleID));
        //Debug.Log("Entering Battle: ID=" + battleID);

        BattleManager.instance.NewBattle(returnID,enemies);
        yield return null;
    }

    public IEnumerator LoadScene(int sceneID)
    {

        SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
        yield return null;

        Scene currentScene = SceneManager.GetSceneByBuildIndex(sceneID);
        SceneManager.SetActiveScene(currentScene);
        Debug.Log("Set Active scene to " + currentScene.name);
    }
}
