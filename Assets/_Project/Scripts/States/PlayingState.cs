using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : BaseState
{
    public override GameState state { get => GameState.Playing; }

    public override void InitializeState(GameManager gm)
    {
        base.InitializeState(gm);

        gm.buttonDone.interactable = false;
        gm.buttonRoll.interactable = false;
        gm.buttonEnd.gameObject.SetActive(true);

        gm.ChangeTextTable("DRAG PHASE: Drag and drop your dices to \r\napply effects on your crops!");

        foreach (var dice in gm.listDices)
        {
            dice.icon.transform.GetChild(0).gameObject.SetActive(false);
            dice.icon.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public override void Update(GameManager gm)
    {
        base.Update(gm);
        return;
    }

    public override void EndState(GameManager gm)
    {
        base.EndState(gm);
        Debug.LogFormat("End state {0}", state);
    }
}
