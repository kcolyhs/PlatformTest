using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour {

    public List<GameObject> heroList;
    public GameObject heroTemplate;
    private static HeroManager instance;
        
	void Start () {
        if (instance != null)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject hero = Instantiate(heroTemplate);
            heroList.Add(hero);
            hero.SetActive(false);

        }


	}
    
	
    public void ImportHeroList(List<BaseHero> updatedHeroes)
    {
        if (updatedHeroes == null)
            return;
        for (int i = 0; i < updatedHeroes.Count; i++)
        {
            BaseHero updatedHero = updatedHeroes[i];
            updatedHero.currHP = updatedHero.currHP > 1 ? updatedHero.currHP : 1;
            UpdateLevel(updatedHero);
            heroList[i].GetComponent < HeroState >().hero = updatedHero;
        }
    }
    BaseHero UpdateLevel(BaseHero hero)
    {
        int neededXP = 250 + 250 * hero.level;
        if (hero.xp > neededXP)
        {
            hero.xp -= neededXP;
            hero.level += 1;
        }
        return hero;
    }
    public static HeroManager GetInstance()
    {
        if (instance != null)
            return instance;
        else
        {
            Debug.Log("No HeroManager Instanced");
            return null;
        }
    }
    
}
