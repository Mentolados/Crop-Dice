using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType { None, Edamame, Daikon, BokChoy }
public enum SeedState { None, Germination, Flowering }

[System.Serializable]
public class Seed
{
    public SeedBase seedBaseSO;

    public SeedType seedType;
    public SeedState seedState;

    public int waterCountToComplete;
    public int waterActualCount;
}
