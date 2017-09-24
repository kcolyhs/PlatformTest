using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        OUTOFBATTLE,WAIT,STARTOFTURN,ENEMYDECIDE,PLAYERDECIDE,PROCESSING,ENDOFTURN
    }
    public BattleState currState;

    public static BattleManager instance;
    private int returnSceneID;
    private int turnNumber;

    public InformationPanel informationPanel;
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> heroList = new List<GameObject>();
    public List<EnemyState> enemyStates = new List<EnemyState>();
    public List<HeroState> heroStates = new List<HeroState>();
    public List<ActionHandler> actionQueue = new List<ActionHandler>();

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
            return;
        }
        instance.currState = BattleState.OUTOFBATTLE;
        DontDestroyOnLoad(this.gameObject);
        ActionFunctions.SetupAactionFunctions();
    }

    void Update()
    {
        
        switch (currState)
        {
            case (BattleState.WAIT):
                break;
            case (BattleState.STARTOFTURN)://Apply Dots and tick down all buffs
                StartOfTurn();
                break;
            case (BattleState.ENEMYDECIDE)://Enemyy adds their actions to queue
                EnemyDecide();
                break;
            case (BattleState.PLAYERDECIDE)://Player adds their actions to queue
                PlayerDecide();

                break;
            case (BattleState.PROCESSING)://Turn plays out
                StartCoroutine(ProcessActionList());
                currState = BattleState.WAIT;
                break;
            case (BattleState.ENDOFTURN):
                EndOfTurn();
                break;
            case (BattleState.OUTOFBATTLE):
                return;

        }
    }

    public void NewBattle(int returnID,GameObject[] enemiesToAdd)
    {
        //Finds Information Panel
        informationPanel = GameObject.Find("InformationPanelText").GetComponent<InformationPanel>();
        ActionFunctions.SetupAactionFunctions();
        //Sets sceneId to return to
        returnSceneID = returnID;
        //Repopulate the enemyList
        enemyList.Clear();
        GameObject[] enemyLocations = GameObject.FindGameObjectsWithTag("Enemy Loc");
        for (int i = 0; i < enemiesToAdd.Length; i++)
        {
            GameObject newEnemy = Instantiate(enemiesToAdd[i]);
            newEnemy.transform.position = enemyLocations[i].transform.position;
            enemyList.Add(newEnemy);
        }
        //Adds enemyStates to List
        enemyStates.Clear();
        for (int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i]!=null)
                enemyStates.Add(enemyList[i].GetComponent<EnemyState>());
        }
        //Add heroes to scene
        heroList.Clear();
        heroStates.Clear();
        List<GameObject> heroesToAdd = HeroManager.GetInstance().heroList;
        GameObject[] heroLocations = GameObject.FindGameObjectsWithTag("Hero Loc");
        for(int i = 0;i < heroesToAdd.Count; i++)
        {

            GameObject newHero = Instantiate(heroesToAdd[i]);
            newHero.transform.position = heroLocations[i].transform.position;
            newHero.SetActive(true);
            heroList.Add(newHero);
            heroStates.Add(newHero.GetComponent<HeroState>());
        }
        turnNumber = 1;
        currState = BattleState.ENEMYDECIDE;

    }

    public void EndBattle(bool didHeroesWin)
    {
        ActionFunctions.DisplayText("Victory: " + didHeroesWin);
        //Sets current state of battle
        currState = BattleState.OUTOFBATTLE;
        //Reset states of BattleManager

        //Reward xp and update the heroes
        if (didHeroesWin)
        {
            int totalXP = 0;
            foreach (EnemyState enemyState in enemyStates)
            {
                totalXP += enemyState.enemy.xp;
            }
            foreach (HeroState heroState in heroStates)
            {
                heroState.hero.xp += totalXP;
            }
            List<BaseHero> updatedHeroes = new List<BaseHero>();
            foreach (HeroState state in heroStates)
            {
                updatedHeroes.Add(state.hero);
            }
            HeroManager.GetInstance().ImportHeroList(updatedHeroes);
        }

        //Empty the enemylist
        enemyList.Clear();

        //Return to platform level
        //Remove Enemies from level
        Debug.Log("Returning to: " + returnSceneID);
        SceneController.GetInstance().StartCoroutine("SwitchScene",returnSceneID);
        return;
    }

    private void SortActionList()
    {
        
    }

    private IEnumerator ProcessActionList()
    {
        //Sorts the Action List for speed and priority
        SortActionList();

        Debug.Log("TURN: " + turnNumber);

        foreach (ActionHandler actionHandler in actionQueue)
        {
            if (CheckEnemyDeaths() || CheckHeroDeaths()) {
                Debug.Log("ProcessActionList has exited due to one side being dead");
                    break;
            }
           yield return StartCoroutine(ProcessAction(actionHandler));
        }

        Debug.Log("END TURN: " + turnNumber);
        yield return null;

        actionQueue.Clear();
        currState = BattleState.ENDOFTURN;
    }

    private IEnumerator ProcessAction(ActionHandler action)
    {
        if (action.stateMachine.IsDead())
            yield break;//Skips the turn if actor is dead

        //Calls ActionFunction method depending on which action type is being executed
        switch (action.actionID)
        {
            case (ActionList.Pass):
                yield return ActionFunctions.Pass(action);
                break;
            case (ActionList.BasicEnemyAttack):
                yield return ActionFunctions.BasicEnemyAttack(action);
                break;
            case (ActionList.BasicHeroAttack):
                yield return ActionFunctions.BasicHeroAttack(action);
                break;
            case (ActionList.SlimeSlap):
                yield return ActionFunctions.SlimeSlap(action);
                break;
        }
        yield return WaitForKeyPress(KeyCode.Space);
        yield return null;
    }

    private void EnemyDecide()
    {
        currState = BattleState.WAIT;
        foreach (EnemyState enemy in enemyStates)
        {
            if(!enemy.IsDead())
            enemy.ChooseAction();
        }
        currState = BattleState.PLAYERDECIDE;
    }

    private void PlayerDecide()
    {
        currState = BattleState.WAIT;
       //Debug.Log("Deciding Player Turns");
        foreach(HeroState hero in heroStates)
        {
            hero.ChooseAction();
        }

       // Debug.Log("Finished Deciding Player Turns");
        currState = BattleState.PROCESSING;
    }

    //returns true if all heroes are dead
    private bool CheckHeroDeaths()
    {
        foreach (HeroState hero in heroStates)
        {
            if (!hero.IsDead())
            {
                return false;
            }
        }
        return true;
    }
    //returns true if all enemies are dead
    private bool CheckEnemyDeaths()
    {
        foreach (EnemyState enemy in enemyStates)
        {
            if (!enemy.IsDead())
            {
                return false;
            }
        }
        return true;
    }

    private void StartOfTurn()
    {
        //Call all start of turn functions
        currState = BattleState.ENEMYDECIDE;
        return;
    }

    private void EndOfTurn()
    {
        //Check if either side is dead
        
        if (CheckHeroDeaths())
        {
            ActionFunctions.DisplayText("All heroes are dead");
            EndBattle(false);
            return;
        }
        if (CheckEnemyDeaths())
        {
            ActionFunctions.DisplayText("All enemies are dead");
            EndBattle(true);
            return;
        }
        turnNumber++;
        //If so start new turn
        currState = BattleState.STARTOFTURN;
        return;
    }

    IEnumerator WaitForKeyPress(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }
}
