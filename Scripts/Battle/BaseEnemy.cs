using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy {
    public string name;

    public float baseHP;
    public float currHP;

    public float speed;

    public enum Type
    {
        Slime,Human
    }
    public enum Size
    {
        Tiny,Small,Medium,Large
    }

    public Type enemyType;
    public Size enemySize;

    public float baseAttack;
    public float currAttack;

    public float baseDef;
    public float currDef;

    public int xp;

    public List<ActionList> actionOptions = new List<ActionList>();
}
