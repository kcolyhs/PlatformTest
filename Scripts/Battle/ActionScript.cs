using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionHandler{
    public GameObject actionTarget;
    public GameObject actorGameObject;
    public StateMachine stateMachine;
    public string actorName;
    public ActionList actionID;
    public UnitType unitType;

    

    public enum UnitType
    {
        HERO,ENEMY
    }
        
    public ActionHandler()
    {
        actionTarget = null;
        actorGameObject = null;
        actorName = "N/A";
        actionID = ActionList.Pass;
    }
}

public enum ActionList
{
    Pass,
    BasicEnemyAttack,
    BasicHeroAttack,
    SlimeSlap
}
//When actions are handled one method is chosen depending 