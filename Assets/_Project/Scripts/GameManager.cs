using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStats
{
    public int day;
    public int week;
    public int gold;
    public int nextPayRent;
    public int plotCount;
    public int reRollMax;
    public int reRollsCount;

    public int plantUsedCount;
    public int waterUsedCount;
    public int rollUsedCount;

    public List<Dice> listDices = new List<Dice>();
}

public class GameManager : MonoBehaviour
{
    // --------------------- STATES DATA AND CONTROLLERS --------------------------

    public readonly StartState stateStart = new StartState();
    public readonly TutorialState stateTutorial = new TutorialState();
    public readonly RollState stateRoll = new RollState();
    public readonly PlayingState statePlaying = new PlayingState();
    public readonly EventState stateEvent = new EventState();
    public readonly RentState stateRent = new RentState();
    public readonly ResumeState stateResume = new ResumeState();

    public BaseState currentState;
    public GameStats gameStats;

    public List<ItemBase> listDicesSO = new List<ItemBase>();

    public static GameManager instance;

    public void OnChangeState(BaseState newState)
    {
        EndState();

        currentState = newState;

        InitializeState();
    }

    private void InitializeState()
    {
        if (currentState != null)
        {
            currentState.InitializeState(instance);
        }
    }

    private void EndState()
    {
        if (currentState != null)
        {
            currentState.EndState(instance);
        }
    }

    // --------------------- START --------------------------

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnChangeState(stateStart);
    }
}
