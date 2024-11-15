using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GlobalLists
{
[System.Serializable]
public class Place
{
    public string idName;
    public string Name;
    [TextArea(10,7)] public string Description;
    public Transform Coordinates;
}

public class PlaceList : MonoBehaviour
{
    public static PlaceList instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance == this)
            Destroy(gameObject);
    }

    [SerializeField] private List<Place> places;

    public Place FindPlace(string idName)
    {
        foreach (var place in places)
        {
            if (place.idName == idName)
            {
                return place;
            }
        }
        return null;
    }
}
}