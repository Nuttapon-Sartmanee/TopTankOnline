using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSuperPrefab;

    public Transform bulletSpawnPoint;
    public float cooldownTime = 0.5f;

    bool bulletFireAble = true;


    void Update()
    {
        if (!IsOwner) return;
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) && bulletFireAble)
            {
                StartCoroutine(FireBullet());
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) && bulletFireAble)
            {
                StartCoroutine(FireBulletSuper());
            }
        }
    }

    IEnumerator FireBullet()
    {
        if (bulletFireAble)
        {
            SpawnBulletServerRpc();
        }
        bulletFireAble = false;
        yield return new WaitForSeconds(cooldownTime);
        bulletFireAble = true;
    }

    IEnumerator FireBulletSuper()
    {
        if (bulletFireAble)
        {
            SpawnBulletSuperServerRpc();
        }
        bulletFireAble = false;
        yield return new WaitForSeconds(cooldownTime);
        bulletFireAble = true;
    }

    [ServerRpc]
    private void SpawnBulletServerRpc()
    {
        Quaternion spawnRot = transform.rotation;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position , spawnRot);
        bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void SpawnBulletSuperServerRpc()
    {
        Quaternion spawnRot = transform.rotation;

        GameObject bulletSuper = Instantiate(bulletSuperPrefab, bulletSpawnPoint.position, spawnRot);
        bulletSuper.GetComponent<NetworkObject>().Spawn();
    }
}
