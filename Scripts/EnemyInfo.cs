using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyInfo : MonoBehaviour {

    public int sceneID;
    public GameObject[] enemyList;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player has entered enemy trigger");
            SceneController sc = SceneController.GetInstance();

            int returnSceneID = SceneController.GetInstance().GetCurrentSceneId();
            sc.StartCoroutine(sc.EnterBattle(sceneID,returnSceneID,enemyList));
            Destroy(gameObject);
        }
    }
}
