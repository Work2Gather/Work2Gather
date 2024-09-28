using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MongoDB.Bson;
using MongoDB.Driver;
using Unity.VisualScripting;

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

    // 서버가 시작되었을 때 호출될 함수
    private void HandleServerStarted()
    {
        Debug.Log("Server started");    //CLI에서 한글이 깨져서 영어로 작성
        databaseManager.mongoDBContext = new MongoDBContext();
        databaseManager.mongoDBContext.ConnectToMongoDB(); // DB 연결 하고싶음 > 연결 완!
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        if (networkManager.IsServer)
        {
            Debug.Log("Client" + clientId + " connected");
            InstantiatePlayer(clientId);
        }
    }

    //플레이어를 스폰하는 함수
    private void InstantiatePlayer(ulong clientId)
    {
        GameObject playerObject = null;

        playerObject = NetworkManager.Singleton.SpawnManager.PlayerObjects[1].gameObject;
        //playerObject = PlayerObjectArray[1];
        //플레이어 타입 DB를 통해 가져오기
        //플레이어 현재 맵 DB를 통해 가져오기
        GameObject playerInstance = Instantiate(playerObject);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        // 플레이어를 네트워크에 등록하고 클라이언트에 동기화
        networkObject.SpawnAsPlayerObject(clientId);
    }

    // 서버를 시작하는 함수
    public void StartServer()
    {
        networkManager.StartServer();
    }

}