using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    public BattleManager battleManager;

    void Start () {
        battleManager = BattleManager.instance;
	}
	public virtual bool IsDead()
    {
        return false;
    }

    public virtual float GetAttack()
    {
        return 0;
    }
}
