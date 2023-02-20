using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLife : MonoBehaviour
{
    public int hp = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp--;
            Destroy(collision.gameObject);
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
