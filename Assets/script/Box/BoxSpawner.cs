using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BoxSpawner : NetworkBehaviour
{
    public GameObject boxPrefab;
    public int numBoxs = 100;

    public override void OnNetworkSpawn()
    {
        SpawnAllBox();
    }

    private void SpawnAllBox()
    {
        for (int i = 0; i < numBoxs; i++)
        {
            int randomX = Random.Range(-30, 30);
            int randomZ = Random.Range(-30, 30);
            SpawnBoxServerRpc(randomX , randomZ);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBoxServerRpc(int xPos ,int zPos)
    {
        Quaternion spawnRot = transform.rotation;
        Vector3 spawnPos = new Vector3(xPos, 3, zPos);

        GameObject box = Instantiate(boxPrefab, spawnPos, spawnRot);
        box.GetComponent<NetworkObject>().Spawn();
    }
}
