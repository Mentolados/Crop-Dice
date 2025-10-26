using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimWorm : MonoBehaviour
{
    private void Start()
    {
        transform.DOPunchScale(new Vector3(-0.25f, 0.25f, 1), 0.75f, 2).SetLoops(-1);
    }
}
