using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GlobalLists
{
[System.Serializable]
public class Mob
{
    public string idName;
    public string Name;
    [TextArea(10,7)] public string Description;
    public GameObject gameObject;
}

public class MobList : MonoBehaviour
{
    public static MobList instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance == this)
            Destroy(gameObject);
    }

    [SerializeField] private List<Mob> mobs;

    public Mob FindMob(string idName)
    {
        foreach (var mob in mobs)
        {
            if (mob.idName == idName)
            {
                return mob;
            }
        }
        return null;
    }
}
}