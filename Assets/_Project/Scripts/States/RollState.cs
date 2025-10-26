using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : BaseState
{
    public override GameState state { get => GameState.Roll; }

    public override void InitializeState(GameManager gm)
    {
        base.InitializeState(gm);

        gm.buttonRoll.interactable = true;
        gm.buttonDone.interactable = true;

        gm.ChangeTextTable("ROLL PHASE: Roll until you're happy then click done!\r\nClick dices that you want to keep for next rerolls!");

        gm.RollDices();
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
