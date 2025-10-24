using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public enum GameState { None, Tutorial, Start, Roll, Rent, Playing, Resume, Event };

public abstract class BaseState
{
    public virtual GameState state { get; set; }

    public virtual void InitializeState(GameManager gm)
    {
    }

    public virtual void Update(GameManager gm)
    {
    }

    public virtual void EndState(GameManager gm)
    {

    }
}

