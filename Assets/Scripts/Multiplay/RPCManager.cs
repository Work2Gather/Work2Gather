using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using KinematicCharacterController.Examples;

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
        DontDestroyOnLoad(gameObject);
    }

    #region Server

    // 서버가 시작되었을 때 호출될 함수
    private void HandleServerStarted()
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
            RequestPlayerModelIdServerRpc(clientId);
            InstantiatePlayer(clientId, currentUserId.ToString());
        }
        else
        {
            Debug.Log($"서버에 접속했습니다. Client ID: {clientId}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestPlayerModelIdServerRpc(ulong clientId)
    {
        Debug.Log("S");
        // 클라이언트에게 player_model_id를 요청
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        RequestPlayerModelIdClientRpc(clientRpcParams);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendPlayerModelIdServerRpc(string playerModelId, ServerRpcParams serverRpcParams = default)
    {
        // 서버에서 받은 player_model_id를 사용하여 플레이어 인스턴스화
        InstantiatePlayer(serverRpcParams.Receive.SenderClientId, playerModelId);
    }

    //플레이어를 스폰하는 함수
    private async void InstantiatePlayer(ulong clientId, string playerModelId)
    {
        GameObject playerObject = null;
        //string player_id = currentUserId.ToString();
        string player_id = playerModelId;
        UserClass temp = await databaseManager.collectionManager.userCollectionManager.GetUserById(player_id);
        Vector3 position = new Vector3(60, 1, 40);

        if (temp != null)
        {
            playerObject = NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[temp.user_character_id - 1].Prefab;
            GameObject playerInstance = Instantiate(playerObject, position, Quaternion.identity);
            playerInstance.GetComponent<ExamplePlayer>().Character.Motor.SetPosition(position);
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

    [ClientRpc]
    private void RequestPlayerModelIdClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Request Received");
        if (IsClient && IsOwner)
        {
            // 클라이언트에서 player_model_id를 가져와 서버로 전송
            string playerModelId = GetPlayerModelIdFromClientData();
            SendPlayerModelIdServerRpc(playerModelId);
        }
    }

    private string GetPlayerModelIdFromClientData()
    {
        // 클라이언트에서 player_model_id를 가져오는 로직
        // 예: PlayerPrefs, 로컬 DB 등에서 데이터 로드
        Debug.Log(GameManager.Instance.ClientManager.currentUserId);
        return GameManager.Instance.ClientManager.currentUserId;
    }

    #endregion
}