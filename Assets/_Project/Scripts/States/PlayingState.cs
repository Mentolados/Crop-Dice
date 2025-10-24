using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : BaseState
{
    public override GameState state { get => GameState.Playing; }

    public override void InitializeState(GameManager gm)
    {
        base.InitializeState(gm);
        Debug.LogFormat("Initialize state {0}", state);
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
