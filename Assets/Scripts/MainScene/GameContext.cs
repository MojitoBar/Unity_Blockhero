using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameContext : Singleton<GameContext>
{
    public List<SHeroView> HeroprefabList = new List<SHeroView>();
    public Dictionary<string,SHeroView> heroDictionary = 
        new Dictionary<string,SHeroView>();

    public List<SHeroView> AllHeroList = new List<SHeroView>();
    public SHeroView[] SelectedHero = new SHeroView[3];
    public int Gold;
    public int Diamond;
    public int Heart;
    public int Level;

}
