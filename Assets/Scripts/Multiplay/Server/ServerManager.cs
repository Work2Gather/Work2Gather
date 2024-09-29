using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MongoDB.Bson;
using MongoDB.Driver;
using Unity.VisualScripting;
using System;
using Unity.Netcode.Transports.UTP;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private DatabaseManager databaseManager;
    public GameObject[] PlayerObjectArray;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        //databaseManager = GetComponent<DatabaseManager>();

        // 서버가 시작되었을 때 호출될 콜백 등록
        networkManager.OnServerStarted += HandleServerStarted;

        // 클라이언트가 연결되었을 때 호출될 콜백 등록
        networkManager.OnClientConnectedCallback += OnClientConnected;

        StartServer();
    }

    #region Server

    // 서버가 시작되었을 때 호출될 함수
    private async void HandleServerStarted()
    {
        Debug.Log("Server started");    //CLI에서 한글이 깨져서 영어로 작성
        databaseManager.mongoDBContext = new MongoDBContext();
        await databaseManager.mongoDBContext.ConnectToMongoDB(); // DB 연결 하고싶음 > 연결 완!
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log(networkManager.ConnectedClients[clientId].OwnedObjects);
        if (networkManager.IsServer)
        {
            InstantiatePlayer(clientId);
            Debug.Log("Client" + clientId + " connected");
        }
    }

    //플레이어를 스폰하는 함수
    private async void InstantiatePlayer(ulong clientId)
    {
        GameObject playerObject = null;
        string player_id = GameManager.Instance.RPCManager.currentUserId.ToString();
        UserClass temp = await databaseManager.collectionManager.userCollectionManager.GetUserById(player_id);
        Vector3 position = new Vector3(60, 1, 40);

        if (temp != null)
        {
            playerObject = networkManager.NetworkConfig.Prefabs.Prefabs[temp.user_character_id - 1].Prefab;
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
        networkManager.StartServer();

        Debug.Log(GetComponent<UnityTransport>().ConnectionData.Address);
        Debug.Log(GetComponent<UnityTransport>().ConnectionData.Port);
    }

    #endregion

    #region Client

    public void StartClient()
    {
        networkManager.StartClient();
    }

    #endregion

}