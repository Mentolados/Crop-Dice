using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RentState : BaseState
{
    public override GameState state { get => GameState.Rent; }

    public override void InitializeState(GameManager gm)
    {
        base.InitializeState(gm);

        gm.textPayRent.text = gm.gameStats.pricesNextRent[gm.gameStats.nextPayRent].ToString() + "$";
        gm.ShowEventRent();
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
