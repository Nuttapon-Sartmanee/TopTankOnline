using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    void Update()
    {
        if (!IsOwner) return;
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                SpawnBulletServerRpc();
            }
        }
    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        Quaternion spawnRot = transform.rotation;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position , spawnRot);
        bullet.GetComponent<NetworkObject>().Spawn();
    }
}
