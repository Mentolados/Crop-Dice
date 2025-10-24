using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceFaceType { None, Blank, Water, Grow, Harvest, Defend, Fertilize, Wild }

[Serializable, CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item", order = 1)]
public class ItemBase : ScriptableObject
{
    public int id, idName, idDescription;
    public Sprite sprite;
    public float value;

    public DiceFaceType type;

    public void OnItemEffect()
    {
        switch(type)
        {
            case DiceFaceType.None:



                break;

                default: type = DiceFaceType.None; break;
        }
    }
}
