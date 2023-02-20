using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LifeTime());
    }
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
