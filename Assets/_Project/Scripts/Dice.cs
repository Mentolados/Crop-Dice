using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceState { None, Selected, Unselected, Set, Discard };

[System.Serializable]
public class Dice
{
    public DiceState diceState = DiceState.None;

    public List<ItemBase> listFaces = new List<ItemBase>();
    public ItemBase diceTopface;
}
