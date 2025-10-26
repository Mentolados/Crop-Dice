using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Seed", menuName = "ScriptableObjects/Item/Seed", order = 1)]
public class SeedBase : ScriptableObject
{
    public string idName, idDescription;

    public SeedType seedType;

    public int waterCountToComplete, value;
}
