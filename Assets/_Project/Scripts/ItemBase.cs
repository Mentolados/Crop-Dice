using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceFaceType { None, Blank, Water, RandomWater, TriWater, Plant, Grow, Harvest, Defend, Fertilize, Wild, Crown, Bug }

[Serializable, CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item", order = 1)]
public class ItemBase : ScriptableObject
{
    public string idName, idDescription;
    public Sprite sprite;
    public float value;

    public DiceFaceType type;
}
