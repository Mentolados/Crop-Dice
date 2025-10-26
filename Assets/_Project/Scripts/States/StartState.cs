using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : BaseState
{
    public override GameState state { get => GameState.Start; }

    public bool isPayRent;

    public override void InitializeState(GameManager gm)
    {
        base.InitializeState(gm);

        gm.buttonDone.interactable = false;
        gm.buttonRoll.interactable = false;
        gm.buttonEnd.gameObject.SetActive(false);

        gm.gameStats.day++;
        gm.gameStats.week--;

        isPayRent = false;

        if(gm.gameStats.week == 0)
        {
            isPayRent = true;

            gm.gameStats.week = 7;

            gm.OnChangeState(gm.stateRent);
        }

        gm.UpdateTextDays();
        gm.UpdateTextGold();
        gm.UpdateTextDaysRemains();
        gm.ChangeTextTable("DRAWING DICES!");

        gm.StartCoroutine(gm.PlayDrawDelay());

        foreach(var dice in gm.listDices)
        {
            if(dice.dice.diceState != DiceState.None)
            {
                dice.dice.diceState = DiceState.Unselected;
            }

            dice.TakeDiceFromBag();
        }

        if (isPayRent)
        {
            gm.ShowEventRent();

            return;
        }

        gm.StartCoroutine(DoTransition());
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

    public IEnumerator DoTransition()
    {
        yield return new WaitForSeconds(4);

        GameManager.instance.OnChangeState(GameManager.instance.stateRoll);
    }
}
