using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : StateMachine {
    public BaseEnemy enemy;
    private bool startedPerforming = false;
    public enum EnemyActionState
    {
        WAITING,PERFORMING,DEAD
    }
    public EnemyActionState currState;

	void Start ()
    {
        battleManager = BattleManager.instance;
        currState = EnemyActionState.WAITING;
	}
	
	void Update () {
        switch (currState)
        {
            case (EnemyActionState.WAITING):

                break;
            case (EnemyActionState.PERFORMING):
                if (!startedPerforming)
                {
                    //Perform action animation
                }
                break;
            case (EnemyActionState.DEAD):
                break;
        }
	}
    public void ChooseAction()
    {
        ActionHandler myAction = new ActionHandler()
        {
            actionID = enemy.actionOptions[Random.Range(0, enemy.actionOptions.Count)],//Decides random action
            actorName = enemy.name,
            actorGameObject = this.gameObject,
            stateMachine = this,
            unitType = ActionHandler.UnitType.ENEMY,
            actionTarget = battleManager.heroList[Random.Range(0, battleManager.heroList.Count)]
        };//initializes random targets and attack
        battleManager.actionQueue.Add(myAction);
        //Debug.Log("Enemy action added to actionqueue");
        currState = EnemyActionState.WAITING;

    }
    //Updates if the hero is dead and returns if it is
    public override bool IsDead()
    {
        if (enemy.currHP <= 0f)
        {
            currState = EnemyActionState.DEAD;
            return true;
        }
        return currState == EnemyActionState.DEAD;
    }
    //Returns Enemies Attack
    public override float GetAttack()
    {
        return enemy.currAttack;
    }

    //Applies Damage and returns the damage-def
    public float ApplyDamage(float damage)
    {
        float modifiedDamage = (damage - enemy.currDef);
        if (modifiedDamage < 0)
            modifiedDamage = 0;
        enemy.currHP -= modifiedDamage;
        Debug.Log(enemy.name + " took " + modifiedDamage + " damage / Current Hp:" + enemy.currHP);
        IsDead();
        return modifiedDamage;
    }
}
