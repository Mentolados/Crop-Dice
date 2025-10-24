using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeedType { None, Carrot }
public enum SeedState { None, Germination, Growth, Flowering }

[System.Serializable]
public class Seed
{
    public SeedBase seedBaseSO;

    public SeedType seedType;
    public SeedState seedState;

    public Sprite actualSprite;

    public int waterCountToComplete;
    public int waterActualCount;
}
