using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class uiManagerScript : NetworkBehaviour
{
    public GameObject waitPanel;
    public override void OnNetworkSpawn()
    {
        OnGameStartedServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void OnGameStartedServerRpc()
    {
        if (IsHost)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                waitPanel.SetActive(false);
            }
            else
            {
                waitPanel.SetActive(true);
            }
        }
    }
    public void whenPlayerLeave()
    {
        waitPanel.SetActive(false);
    }
}
