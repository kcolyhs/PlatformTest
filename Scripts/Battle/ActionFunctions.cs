using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionFunctions{

    static BattleManager battleManager;

    static SceneController sceneController;

    static InformationPanel infoPanel;

    public static void SetupAactionFunctions()
    {
        battleManager = BattleManager.instance;
        sceneController = SceneController.GetInstance();
        infoPanel = BattleManager.instance.informationPanel;
    }

    private static float ModifyDamage(float attackValue, float factor)
    {
        float randomValue = attackValue * Random.Range((1 - factor), (1 + factor));
        if (randomValue < 0)
        {
            return 0;
        }
        return Mathf.Round(randomValue) ;
    }


    public static IEnumerator Pass(ActionHandler action)
    {
        string str = (action.actorName + " has passed");
        DisplayText(str);
        
        yield break;
    }

    public static IEnumerator BasicEnemyAttack(ActionHandler action)
    {
        HeroState target = action.actionTarget.GetComponent <HeroState> ();
        //If target is dead reacquire new target


        float attackValue = action.stateMachine.GetAttack();
        string str = (action.actorName + " attacked: " + target.hero.name+" for: "+attackValue+ " damage/ Remaining HP:" + target.hero.currHP);
        DisplayText(str);
        target.ApplyDamage(ModifyDamage(attackValue, .25f));
        yield break;
    }

    //Aoe hit that ignores armor
    public static IEnumerator SlimeSlap(ActionHandler action)
    {
        foreach (HeroState target in battleManager.heroStates)
        {
            if (!target.IsDead())
            {
                float attackValue = action.stateMachine.GetAttack() / 2;
                attackValue = ModifyDamage(attackValue, .5f);
                string str = ("Slime Slap hit " + target.hero.name + " for " + attackValue + " damage / Remaining HP:" + target.hero.currHP);
                DisplayText(str);
                target.ApplyCorrosiveDamage(attackValue);
            }
        }
        
        yield break;
    }

    public static IEnumerator BasicHeroAttack(ActionHandler action)
    {
        EnemyState target = action.actionTarget.GetComponent<EnemyState>();

        if (target.IsDead())
        {
            Debug.Log(action.actorName + " passed - its target is dead");

        }
        else
        {
            float attackValue = action.stateMachine.GetAttack();
            string str = (action.actorName + " attacked:" + target.enemy.name + " for:" + attackValue + " damage/ Remaining HP:" + target.enemy.currHP);
            DisplayText(str);
            target.ApplyDamage(ModifyDamage(attackValue, .25f));
        }
        yield break;
    }

    public static void DisplayText(string str)
    {
        Debug.Log(str);
        infoPanel.EditText(str);
    }
}
