using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerActive : NetworkBehaviour
{
    void Update()
    {
        CheckGameStateServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void CheckGameStateServerRpc()
    {
        if (IsHost)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                GetComponent<PlayerSpawnScript>().SetPlayerState(true);
            }
            else
            {
                GetComponent<PlayerSpawnScript>().SetPlayerState(false);
            }
        }
    }
}
