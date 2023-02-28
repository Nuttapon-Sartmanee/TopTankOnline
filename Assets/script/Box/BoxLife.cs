using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BoxLife : NetworkBehaviour
{
    public int hp = 3;
    public GameObject BoxParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hp--;
            collision.gameObject.GetComponent<NetworkObject>().Despawn();
            SpawnBombEffect();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BulletSuper")
        {
            hp -= 3;
            SpawnBombEffect();
        }
        if (hp <= 0)
        {
            gameObject.GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
        }
    }
    private void SpawnBombEffect()
    {
        GameObject BoxParticleEffect = Instantiate(BoxParticle, transform.position, Quaternion.identity);
        BoxParticleEffect.GetComponent<NetworkObject>().Spawn();
    }
}
