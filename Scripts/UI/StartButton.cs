using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {
    


    public void ChangeScene()
    {
        StartCoroutine (SceneController.GetInstance().SwitchScene(1));
    }
}
