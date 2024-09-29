using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RPCManager : NetworkBehaviour
{
    [SerializeField] DatabaseManager databaseManager;
    public bool Server;
    public string currentUserId;
    void Start()
    {
        // 클라이언트가 연결되었을 때 호출될 콜백 등록
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        string[] args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-server" || Server)
            {
                Debug.Log("======Running as a server======");

                // 서버가 시작되었을 때 호출될 콜백 등록
                NetworkManager.Singleton.OnServerStarted += HandleServerStarted;

                StartServer();
            }
        }
    }

    #region Server

    // 서버가 시작되었을 때 호출될 함수
    private async void HandleServerStarted()
    {
        Debug.Log("Server started");

        // databaseManager.mongoDBContext = new MongoDBContext();
        // await databaseManager.mongoDBContext.ConnectToMongoDB();

    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Client" + clientId + " connected");
            InstantiatePlayer(clientId);
        }
        else
        {
            Debug.Log($"서버에 접속했습니다. Client ID: {clientId}");
        }
    }

    //플레이어를 스폰하는 함수
    private async void InstantiatePlayer(ulong clientId)
    {
        GameObject playerObject = null;
        string player_id = currentUserId.ToString();
        UserClass temp = await databaseManager.collectionManager.userCollectionManager.GetUserById(player_id);
        Vector3 position = new Vector3(60, 1, 40);

        if (temp != null)
        {
            playerObject = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[temp.user_character_id - 1].Prefab;
            GameObject playerInstance = Instantiate(playerObject, position, Quaternion.identity);
            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

            // 플레이어를 네트워크에 등록하고 클라이언트에 동기화
            networkObject.SpawnAsPlayerObject(clientId);
        }
        else
        {
            Debug.Log("Can't find user ID");
        }
    }

    // 서버를 시작하는 함수
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    #endregion

    #region Client

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    #endregion
}