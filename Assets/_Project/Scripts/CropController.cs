using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public bool isCropActive, isDefended;
    public int wormCount;
    private CanvasGroup canvasGroup;
    public Seed seedPlanted;

    public List<GameObject> seedActiveState = new List<GameObject>();
    public GameObject particleShine;
    public AnimWatering animWatering;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        for (int i = 0; i < 4; i++)
        {
            seedActiveState.Add(transform.GetChild(i).gameObject);
        }

        particleShine = transform.GetChild(5).gameObject;
    }

    private void Start()
    {
        CheckCropActive();
    }

    public void CheckCropActive()
    {
        if (!isCropActive)
        {
            canvasGroup.alpha = 0.5f;
        }
        else
        {
            canvasGroup.alpha = 1f;
        }
    }

    public void PlantSeed()
    {
        seedPlanted.seedState = SeedState.Germination;
        seedPlanted.waterActualCount = 0;
        seedPlanted.waterCountToComplete = seedPlanted.seedBaseSO.waterCountToComplete;

        seedActiveState[0].SetActive(true);

        TextWater();
    }

    public void TextWater()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = seedPlanted.waterActualCount.ToString() + "/" + seedPlanted.waterCountToComplete;
    }

    public void Collect()
    {
        GameManager.instance.gameStats.gold += seedPlanted.seedBaseSO.value;
        GameManager.instance.UpdateTextGold();

        seedPlanted.seedState = SeedState.None;
        seedPlanted.waterActualCount = 0;
        seedPlanted.waterCountToComplete = 0;
        seedPlanted.seedBaseSO = null;

        particleShine.gameObject.SetActive(false);

        foreach(var states in seedActiveState)
        {
            states.SetActive(false);
        }

        GameManager.instance.PlaySFX(4);

        GetComponentInChildren<TextMeshProUGUI>().text = "-";
    }

    public IEnumerator DestroyPlant()
    {
        yield return new WaitForSeconds(1);

        foreach (var states in seedActiveState)
        {
            states.SetActive(false);
        }

        GetComponentInChildren<TextMeshProUGUI>().text = "-";

        seedPlanted.seedState = SeedState.None;
        seedPlanted.waterActualCount = 0;
        seedPlanted.waterCountToComplete = 0;
        seedPlanted.seedBaseSO = null;

        wormCount = 0;
    }

    public void AddWorm()
    {
        if (wormCount >= 2) { return; }

            int index = (transform.childCount - 2) + wormCount;

        transform.GetChild(index).gameObject.SetActive(true);

        wormCount ++;

        if(wormCount == 2)
        {
            StartCoroutine(DestroyPlant());
        }
    }

    public IEnumerator Grow()
    {
        yield return new WaitForSeconds(1f);

        seedPlanted.seedState = SeedState.Flowering;

        foreach(var go in seedActiveState)
        {
            go.gameObject.SetActive(false);
        }

        seedActiveState[(int)seedPlanted.seedType].SetActive(true);
        particleShine.gameObject.SetActive(true);

        TextWater();
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!isCropActive) { return; }

        eventData.pointerDrag.GetComponent<DiceController>().OnItemEffect(this);
        //eventData.pointerDrag.GetComponent<DiceController>().Discard();
    }
}
