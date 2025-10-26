using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimParticles : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AnimationShine());
    }

    public IEnumerator AnimationShine()
    {
        yield return new WaitForSeconds(0.5f);

        transform.eulerAngles = new Vector3 (0f, -179f, 0f);

        yield return new WaitForSeconds(0.5f);

        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        StartCoroutine(AnimationShine());
    }
}
