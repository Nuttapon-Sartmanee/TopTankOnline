using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float bulletspeed = 15;
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * bulletspeed;
    }

}
