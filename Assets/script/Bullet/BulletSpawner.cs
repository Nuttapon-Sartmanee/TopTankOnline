using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float cooldownTime = 0.5f;
    bool FireAble = true;

    void Update()
    {
        if (!IsOwner) return;
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) && FireAble)
            {
                StartCoroutine(Fire());
            }
        }
    }

    IEnumerator Fire()
    {
        if (FireAble)
        {
            SpawnBulletServerRpc();
        }
        FireAble = false;
        yield return new WaitForSeconds(cooldownTime);
        FireAble = true;
    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        Quaternion spawnRot = transform.rotation;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position , spawnRot);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
