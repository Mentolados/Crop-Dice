using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;
using System.Linq;

public class DiceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon, message;
    private Vector3 backPosition;

    public Transform pivot;

    public Dice dice;

    public void TakeDiceFromBag()
    {
        if(dice.diceState == DiceState.None) { return; }

        icon.transform.GetChild(0).gameObject.SetActive(false);
        icon.transform.GetChild(3).gameObject.SetActive(false);

        List<Vector3> positions = new List<Vector3>();

        if (icon != null)
        {
            icon.GetComponent<Image>().sprite = dice.listFaces[0].sprite;

            positions.Add(pivot.position);
            positions.Add(transform.GetChild(0).position);

            icon.transform.DOPath(positions.ToArray(), 3f, PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.InOutElastic, 0.5f, 1.5f).SetDelay(Random.Range(0f, 0.35f));
            icon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            icon.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).SetDelay(1.5f);
        }
    }

    public IEnumerator RollDice(float delay)
    {
        if(dice.diceState == DiceState.None || dice.diceState == DiceState.Discard || dice.diceState == DiceState.Selected) { yield break; }

        int rngValue = Random.Range(0, 6);

        yield return icon.transform.DOPunchScale(new Vector3(0.5f, 1.05f, 1), 0.35f, 5).SetDelay(delay).WaitForCompletion();

        GameManager.instance.PlaySFX(1);

        dice.diceTopface = dice.listFaces[rngValue];

        icon.transform.GetChild(2).GetComponent<Image>().sprite = dice.diceTopface.sprite;
        icon.transform.GetChild(2).GetComponent<Image>().SetNativeSize();

        Debug.Log(dice.diceTopface.type);

        if (dice.diceTopface.type == DiceFaceType.Blank)
        {
            yield return new WaitForSeconds(0.5f);

            GameManager.instance.PlaySFX(15);

            Discard();

            yield break;
        }

        if (dice.diceTopface.type == DiceFaceType.Bug)
        {
            yield return new WaitForSeconds(0.5f);

            GameManager.instance.PlaySFX(7);

            OnItemEffect();
        }
    }

    // ------------------------ SELECTABLES UI --------------------------

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.currentState != GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        icon.transform.position = CalculateMousePosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(GameManager.instance.currentState != GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        Debug.Log("OnBeginDrag");

        GameManager.instance.PlaySFX(13);

        icon.gameObject.SetActive(true);
        icon.GetComponent<Image>().maskable = false;

        var images = icon.GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            image.maskable = false;
        }

        icon.transform.position = CalculateMousePosition();

        backPosition = this.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.instance.currentState != GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        Debug.Log("OnEndDrag");

        if (icon != null)
        {
            StartCoroutine(BackToPostion());
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.instance.currentState != GameManager.instance.statePlaying) { return; }


        Debug.Log("OnDrop");
    }

    private Vector3 CalculateMousePosition()
    {
        var screenPoint = (Vector3)Input.mousePosition;

        return screenPoint;
    }

    private IEnumerator BackToPostion()
    {
        yield return icon.transform.DOMove(backPosition, 0.35f).SetUpdate(true).SetEase(Ease.OutElastic, 2, 0.6f).WaitForCompletion();

        icon.GetComponent<Image>().maskable = true;

        var images = icon.GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            image.maskable = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //message.transform.position = CalculateMousePosition();

        CHeckSelectUnselect();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //message.transform.localPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("POINTER ENTER DICE");

        if (GameManager.instance.currentState == GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard || dice.diceState == DiceState.Selected) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        GameManager.instance.PlaySFX(11);

        icon.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.instance.currentState == GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard || dice.diceState == DiceState.Selected) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        icon.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CHeckSelectUnselect()
    {
        if (GameManager.instance.currentState == GameManager.instance.statePlaying) { return; }

        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard) { return; }

        if (dice.diceTopface.type == DiceFaceType.Blank || dice.diceTopface.type == DiceFaceType.Bug) { return; }

        if (dice.diceState == DiceState.Unselected)
        {
            dice.diceState = DiceState.Selected;

            icon.transform.GetChild(0).gameObject.SetActive(true);
            icon.transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            dice.diceState = DiceState.Unselected;

            icon.transform.GetChild(0).gameObject.SetActive(false);
            icon.transform.GetChild(3).gameObject.SetActive(false);
        }

        GameManager.instance.PlaySFX(0);
    }

    public void Discard()
    {
        if (dice.diceState == DiceState.None || dice.diceState == DiceState.Discard) { return; }

        //dice.diceTopface = null;
        dice.diceState = DiceState.Discard;

        List<Vector3> positions = new List<Vector3>();

        positions.Add(pivot.position);
        positions.Add(GameManager.instance.pivotDiscardBag.position);

        icon.transform.DOScale(Vector3.zero, 1f).SetDelay(0.5f);

        icon.transform.DOPath(positions.ToArray(), 0.5f, PathType.CatmullRom, PathMode.Sidescroller2D).SetDelay(Random.Range(0f, 0.35f)).onComplete += delegate
        {
            icon.GetComponent<Image>().maskable = true;

            var images = icon.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                image.maskable = true;
            }
        };
    }

    public void OnItemEffect(CropController crop = null)
    {
        if(dice.diceTopface == null) { return; }

        switch (dice.diceTopface.type)
        {
            case DiceFaceType.Water:

                if(crop.seedPlanted.seedState != SeedState.Germination) { GameManager.instance.PlaySFX(8); return; }

                Discard();

                crop.seedPlanted.waterActualCount ++;
                crop.animWatering.endPoint = icon.transform;
                crop.animWatering.gameObject.SetActive(true);
                StartCoroutine(crop.animWatering.AnimationWatering());

                crop.TextWater();

                if(crop.seedPlanted.waterActualCount == crop.seedPlanted.waterCountToComplete)
                {
                    StartCoroutine(crop.Grow());
                }

                break;

            case DiceFaceType.TriWater:

                if (crop.seedPlanted.seedState != SeedState.Germination) { return; }

                Discard();

                crop.seedPlanted.waterActualCount += 3;

                crop.TextWater();

                if (crop.seedPlanted.waterActualCount >= crop.seedPlanted.waterCountToComplete)
                {
                    crop.seedPlanted.waterActualCount = crop.seedPlanted.waterCountToComplete;

                    StartCoroutine(crop.Grow());
                }

                break;

            case DiceFaceType.Plant:

                if (crop.seedPlanted.seedState != SeedState.None) { GameManager.instance.PlaySFX(8); return; }

                Discard();

                crop.seedPlanted.seedBaseSO = GameManager.instance.listSeedSO[(int)crop.seedPlanted.seedType - 1];
                crop.PlantSeed();

                GameManager.instance.PlaySFX(5);

                break;

            case DiceFaceType.Harvest:

                if (crop.seedPlanted.seedState != SeedState.Flowering) { GameManager.instance.PlaySFX(3); return; }

                Discard();

                crop.Collect();

                GameManager.instance.PlaySFX(8);

                break;

            case DiceFaceType.Defend:


                break;

            case DiceFaceType.Fertilize:



                break;

            case DiceFaceType.Crown:



                break;

            case DiceFaceType.Bug:

                var activeCrops = GameManager.instance.listCrops.Where(x => x.isCropActive == true).ToList();

                int rng = Random.Range(0, activeCrops.Count);

                activeCrops[rng].AddWorm();

                Discard();

                break;

            default:
                
                break;
        }
    }


}
