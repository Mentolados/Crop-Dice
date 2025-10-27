using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimWatering : MonoBehaviour
{
    public Transform endPoint, hidePosition;

    public List <Sprite> sprites = new List<Sprite>();

    void Start()
    {
        //StartCoroutine(AnimationWatering());
    }

    public IEnumerator AnimationWatering()
    {
        yield return transform.DOMove(endPoint.position, 0.5f).WaitForCompletion();

        GameManager.instance.PlaySFX(6);

        var image = GetComponent<Image>();

        image.sprite = sprites[0];
        transform.eulerAngles = new Vector3(0, 0, 15);

        yield return new WaitForSeconds(0.25f);

        image.sprite = sprites[1];
        transform.eulerAngles = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.25f);

        image.sprite = sprites[0];
        transform.eulerAngles = new Vector3(0, 0, 15);

        yield return new WaitForSeconds(0.25f);

        image.sprite = sprites[1];
        transform.eulerAngles = new Vector3(0, 0, 0);

        var go = this.gameObject;

        yield return transform.DOMove(hidePosition.position, 0.5f).onComplete += delegate
        {
            go.SetActive(false);
        };
    }
}
