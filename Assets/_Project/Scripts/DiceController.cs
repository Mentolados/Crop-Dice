using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DiceController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image image, icon, message;
    private Vector3 backPosition;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // ------------------------ SELECTABLES UI --------------------------

    public void OnDrag(PointerEventData eventData)
    {
        icon.transform.position = CalculateMousePosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        message.transform.localPosition = Vector3.zero;

        var alpha = image.color;
        alpha.a = 0.25f;
        image.color = alpha;

        icon.gameObject.SetActive(true);
        icon.GetComponent<Image>().sprite = image.sprite;
        icon.transform.position = CalculateMousePosition();

        backPosition = this.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (icon != null)
        {
            StartCoroutine(BackToPostion());
        }
        else
        {
            var alpha = image.color;
            alpha.a = 1f;

            image.color = alpha;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    private Vector3 CalculateMousePosition()
    {
        var screenPoint = (Vector3)Input.mousePosition;

        return screenPoint;
    }

    private IEnumerator BackToPostion()
    {
        yield return icon.transform.DOMove(backPosition, 0.35f).SetUpdate(true).SetEase(Ease.OutElastic, 2, 0.6f).WaitForCompletion();

        var alpha = image.color;
        alpha.a = 1f;

        image.color = alpha;

        icon.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        message.transform.position = CalculateMousePosition();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        message.transform.localPosition = Vector3.zero;
    }
}
