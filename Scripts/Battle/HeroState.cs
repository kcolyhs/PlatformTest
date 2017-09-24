using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroState : StateMachine {
    public BaseHero hero;

   
    public HeroActionState currState;
    public ActionHandler actionToBePerformed;

    public enum HeroActionState
    {
        WAITING, SELECTINGACTION, SELECTINGTARGET, PERFORMING, DEAD
    }

    public HeroState(BaseHero baseHero)
    {
        hero = baseHero;
        currState = HeroActionState.WAITING;
        actionToBePerformed = null;
    }
    
    void Start ()
    {
        battleManager = BattleManager.instance;
        currState = HeroActionState.WAITING;
	}
	
	void Update () {
        switch (currState)
        {
            case (HeroActionState.WAITING):
                break;
            case (HeroActionState.PERFORMING):
                break;
            case (HeroActionState.DEAD):
                break;

        }
	}
    //Updates if the hero is dead and returns if it is
    public override bool IsDead()
    {
        if (hero.currHP <= 0f)
            currState = HeroActionState.DEAD;
        return currState == HeroActionState.DEAD;
    }

    //Returns heroes attack
    public override float GetAttack()
    {
        return hero.currAttack;
    }

    public void ChooseAction()
    {
        actionToBePerformed = new ActionHandler()
        {
            actionID = ActionList.BasicHeroAttack,
            actorGameObject = this.gameObject,
            actorName = hero.name,
            stateMachine = this
        };
        ChooseTarget();
        currState = HeroActionState.WAITING;//Change to set to select target once input is implemented
    }
    public void ChooseTarget()
    {
        EnemyState target = battleManager.enemyStates[Random.Range(0, battleManager.enemyStates.Count)];

        while (target.IsDead()){
            target = battleManager.enemyStates[Random.Range(0, battleManager.enemyStates.Count)];
        }
        actionToBePerformed.actionTarget = target.gameObject;
        battleManager.actionQueue.Add(actionToBePerformed);
        //Debug.Log("Hero action added to actionqueue");
    }


    //Applies Damage and returns the damage-def
    public float ApplyDamage(float damage)
    {
        float modifiedDamage = (damage - hero.currDef);
        IsDead();
        if (modifiedDamage < 0)
            modifiedDamage = 0;
        hero.currHP -= modifiedDamage;
        Debug.Log(hero.name + " took " + modifiedDamage +" damage / Current Hp:" + hero.currHP);
        return modifiedDamage;
    }
    public float ApplyCorrosiveDamage(float damage)
    {
        hero.currDef -= Mathf.Round(damage / 5);
        float modifiedDamage = (damage - hero.currDef);
        Debug.Log(hero.name + " took " + modifiedDamage + " damage / Current Hp:" + hero.currHP);
        hero.currHP -= modifiedDamage;
        return modifiedDamage;
    }

}
