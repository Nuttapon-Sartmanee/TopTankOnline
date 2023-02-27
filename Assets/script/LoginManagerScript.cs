using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using QFSW.QC;

public class LoginManagerScript : MonoBehaviour
{
    public TMP_InputField userNameInput;
    public GameObject loginPanel;
    public GameObject leaveButton;
    public GameObject scorePanel;

    public GameObject hostSpawnpoint;
    public GameObject clientSpawnpoint;


    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleServerDisconnected;
        SetUIVisible(false);
    }

    private void SetUIVisible(bool isUserLogin)
    {
        if (isUserLogin)
        {
            loginPanel.SetActive(false);
            leaveButton.SetActive(true);
            scorePanel.SetActive(true);
        }
        else 
        {
            loginPanel.SetActive(true);
            leaveButton.SetActive(false);
            scorePanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) { return;}
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleServerDisconnected;
    }

    private void HandleServerDisconnected(ulong clientID)
    {
        throw new System.NotImplementedException();
    }

    public void Leave() 
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if(NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SetUIVisible(false);
    }

    private void HandleClientConnected(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            SetUIVisible(true);
        }
    }

    private void HandleServerStarted()
    {
    }

    public void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }
    
    public void Client()
    {
        string userName = userNameInput.GetComponent<TMP_InputField>().text;

        NetworkManager.Singleton.NetworkConfig.ConnectionData = 
            System.Text.Encoding.ASCII.GetBytes(userName); //String > Byte

        NetworkManager.Singleton.StartClient();
    }

    private bool approveConnection(string clientData , string serverData)
    {
        bool isApprove = System.String.Equals(clientData.Trim(), serverData.Trim()) ? false : true;
        return isApprove;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;
        int byteLength = connectionData.Length;
        bool isApprove = false;
        if (byteLength > 0)
        {
            string clientData = System.Text.Encoding.ASCII.GetString(connectionData, 0, byteLength);
            string hostData = userNameInput.GetComponent<TMP_InputField>().text;
            isApprove = approveConnection(clientData,hostData);
        }

        // Your approval logic determines the following values
        response.Approved = isApprove;
        response.CreatePlayerObject = true;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        response.Position = Vector3.zero;
        response.Rotation = Quaternion.identity;
        setSpawnLocation(clientId,response);

        response.Reason = "Some reason for not approving the client";
        response.Pending = false;
    }

    private void setSpawnLocation(ulong clientID, NetworkManager.ConnectionApprovalResponse response)
    {
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;
        int randNum = Random.Range(-20, 20);

        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            spawnPos = new Vector3(randNum, 0, hostSpawnpoint.transform.position.z); spawnRot = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            switch (NetworkManager.Singleton.ConnectedClients.Count)
            {
                case 1:
                    spawnPos = new Vector3(randNum, 0, clientSpawnpoint.transform.position.z); spawnRot = Quaternion.Euler(0, 0, 0);
                    break;
                case 2:
                    spawnPos = new Vector3(randNum, 0, clientSpawnpoint.transform.position.z); spawnRot = Quaternion.Euler(0, 0, 0);
                    break;
            }
        }
        response.Position = spawnPos; 
        response.Rotation = spawnRot;
    }
}
