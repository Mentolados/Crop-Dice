using DG.Tweening;
using EasyTextEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public List<int> pricesNextRent;
    public List<int> pricesNextDice;
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

    public List<DiceController> listDices = new List<DiceController>();
    public List<CropController> listCrops = new List<CropController>();

    public List<ItemBase> listDicesSO = new List<ItemBase>();
    public List<SeedBase> listSeedSO = new List<SeedBase>();

    public Transform pivotEventRent, pivotEventShop, pivotEventTutorial, pivotShowEvent, pivotHideEvent, pivotDiscardBag;
    public TextMeshProUGUI textPayRent, textCoins, textRent, textDays, textTable;

    public List<GameObject> listEvents = new List<GameObject>();
    public List<GameObject> particlesSystems = new List<GameObject>();
    public List<AudioSource> sfxs = new List<AudioSource>();

    public Button buttonRoll, buttonDone, buttonEnd;

    public AudioMixer master;
    private float volume;
    private int volumeCount = 5;

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

    public void ShowEventRent()
    {
        listEvents[0].SetActive(true);

        pivotEventRent.DOMove(pivotShowEvent.position, 1f).SetEase(Ease.OutElastic);
    }

    public IEnumerator HideEventRent()
    {
        yield return pivotEventRent.DOMove(pivotHideEvent.position, 1f).SetEase(Ease.InElastic).WaitForCompletion();

        yield return new WaitForSeconds(0.5f);

        OnChangeState(stateRoll);
    }

    public void DOPayRent()
    {
        if(gameStats.nextPayRent > gameStats.gold)
        {
            for(int i = 0; i < listEvents[0].transform.childCount; i++)
            {
                listEvents[0].transform.GetChild(i).gameObject.SetActive(false);
            }

            listEvents[0].transform.GetChild(3).gameObject.SetActive(true);
            listEvents[0].transform.GetChild(4).gameObject.SetActive(true);

            PlaySFX(9);

            return;
        }

        gameStats.gold -= gameStats.nextPayRent;
        UpdateTextGold();

        PlaySFX(2);
        PlaySFX(0);

        for(int i = 0;i < listCrops.Count; i++)
        {
            if (!listCrops[i].isCropActive)
            {
                listCrops[i].isCropActive = true;
                listCrops[i].GetComponent<CanvasGroup>().alpha = 1f;

                if(i == listCrops.Count - 1)
                {
                    for (int e = 0; e < listEvents[0].transform.childCount; e++)
                    {
                        listEvents[0].transform.GetChild(e).gameObject.SetActive(false);
                    }

                    listEvents[0].transform.GetChild(5).gameObject.SetActive(true);

                    foreach(var particle in particlesSystems)
                    {
                        particle.SetActive(true);
                    }

                    PlaySFX(10);

                    return;
                }

                break;
            }
        }

        StartCoroutine(HideEventRent());
    }

    public void DORestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowEventDiceBag()
    {
        if (DOTween.IsTweening(pivotEventShop)) { return; }

        listEvents[1].SetActive(true);

        pivotEventShop.DOMove(pivotShowEvent.position, 1f).SetEase(Ease.OutElastic, 0.5f, 0.5f);
    }

    public void HideEventDiceBag()
    {
        if (DOTween.IsTweening(pivotEventShop)) { return; }

        pivotEventShop.DOMove(pivotHideEvent.position, 1f).SetEase(Ease.InElastic, 0.5f, 0.5f).onComplete += delegate
        {
            listEvents[1].SetActive(false);
        };
    }

    public void DOShowHideDiceBag()
    {
        if (listEvents[1].activeInHierarchy)
        {
            HideEventDiceBag();

            PlaySFX(17);
        }
        else
        {
            ShowEventDiceBag();

            PlaySFX(16);
        }
    }

    public void DOBuyNextDice(Button button)
    {
        int index = button.transform.GetSiblingIndex();

        if(gameStats.gold < gameStats.pricesNextDice[index - 6])
        {
            return;
        }

        gameStats.gold -= gameStats.pricesNextDice[index - 6];

        UpdateTextGold();

        button.transform.GetChild(0).gameObject.SetActive(false);
        button.transform.GetChild(1).gameObject.SetActive(false);

        if (index + 1 < button.transform.parent.childCount)
        {
            button.transform.parent.GetChild(index + 1).GetComponent<Button>().interactable = true;
        }

        button.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
        listDices[index - 1].dice.diceState = DiceState.Unselected;
        listDices[index - 1].dice.diceTopface = listDices[index - 1].dice.listFaces[0];

        PlaySFX(14);
    }

    // --------------------- START --------------------------

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        volume = 0.5f;
        volumeCount = 5;

        master.SetFloat("Master", Mathf.Log10(volume) * 20);

        OnChangeState(stateTutorial);
    }

    public void RollDices()
    {
        if(currentState != stateRoll) { return; }

        foreach (var dice in listDices)
        {
            if (DOTween.IsTweening(dice.icon.transform)) { return; }
        }

        float delay = 0f;

        foreach(var dice in listDices)
        {
            StartCoroutine(dice.RollDice(delay));

            delay += 0.15f;
        }
    }

    public void DoFinishDay()
    {
        StartCoroutine(FinishDay());
    }

    public IEnumerator FinishDay()
    {
        foreach (var dice in listDices)
        {
            dice.Discard();
        }

        buttonEnd.interactable = false;

        yield return new WaitForSeconds(3f);

        OnChangeState(stateStart);
    }

    public void DoFinishTutorial()
    {
        StartCoroutine(FinishTutorial());
    }

    public IEnumerator FinishTutorial()
    {
        yield return pivotEventTutorial.DOMove(pivotHideEvent.position, 1f).SetEase(Ease.InElastic, 0.5f, 0.5f).WaitForCompletion();

        yield return new WaitForSeconds(2);

        OnChangeState(stateStart);
    }

    public void ChangeStateToPlay()
    {
        OnChangeState(statePlaying);
    }

    public void ChangeTextTable(string text)
    {
        textTable.text = text;
        //textTable.GetComponent<TextEffect>().Refresh();
    }

    public void UpdateTextGold()
    {
        textCoins.text = gameStats.gold.ToString();
    }

    public void UpdateTextDays()
    {
        textDays.text = gameStats.day.ToString();
    }

    public void UpdateTextDaysRemains()
    {
        textRent.text = gameStats.nextPayRent.ToString() + "$ due in " + gameStats.week.ToString() + " days";
        //textRent.GetComponent<TextEffect>().Refresh();
    }

    public void PlaySFX(int index = 0)
    {
        sfxs[index].Play();
    }

    public void FadeIn(AudioSource audioSource)
    {
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0.2f, 1f).onComplete += delegate
        {
            audioSource.Play();
        };
    }

    public void FadeOut(AudioSource audioSource)
    {
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0f, 1f).onComplete += delegate
        {
            audioSource.Stop();
        };
    }

    public void ChangeVolume(GameObject go)
    {
        volume += 0.1f;
        volumeCount++;

        if(volume > 1)
        {
            volume = 0.001f;
            volumeCount = 0;
        }

        go.GetComponentInChildren<TextMeshProUGUI>().text = volumeCount.ToString();

        PlaySFX(0);

        if (!DOTween.IsTweening(go.transform))
        {
            go.transform.DOPunchScale(new Vector3(0.5f, 1.05f, 1), 0.35f, 5);
        }

        master.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public IEnumerator PlayDrawDelay()
    {
        yield return new WaitForSeconds(1f);

        PlaySFX(12);
    }
}
