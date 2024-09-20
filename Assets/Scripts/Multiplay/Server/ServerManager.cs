using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MongoDB.Bson;
using MongoDB.Driver;

public class ServerManager : MonoBehaviour
{
    private NetworkManager networkManager;
    //[SerializeField] GameObject PlayerPrefab;

    // MongoDB 클라이언트 및 데이터베이스 변수
    private IMongoClient mongoClient;
    private IMongoDatabase database;

    private string mongoDBUserName = "work2gatherAdmin";

    private string mongoDBPassword = "BqaqH3zsAL3xSM1o";

    private string mongoDBName = "work2gather";

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        // 서버가 시작되었을 때 호출될 콜백 등록
        networkManager.OnServerStarted += HandleServerStarted;

        // 클라이언트가 연결되었을 때 호출될 콜백 등록
        networkManager.OnClientConnectedCallback += OnClientConnected;
        
        StartServer();
    }

    // 서버가 시작되었을 때 호출될 함수
    private void HandleServerStarted()
    {
        Debug.Log("서버가 시작되었습니다.");
        ConnectToMongoDB(); // MongoDB 연결
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            //InstantiatePlayer(clientId);
        }
    }

    // 플레이어를 스폰하는 함수
    // private void InstantiatePlayer(ulong clientId)
    // {
    //     GameObject playerInstance = Instantiate(PlayerPrefab);
    //     NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

    //     // 플레이어를 네트워크에 등록하고 클라이언트에 동기화
    //     networkObject.SpawnAsPlayerObject(clientId);
    // }

    // 서버를 시작하는 함수
    public void StartServer()
    {
        networkManager.StartServer();
    }

    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    // MongoDB 연결 함수
    void ConnectToMongoDB()
    {
        // MongoDB 연결 문자열
        string connectionString = $"mongodb+srv://{mongoDBUserName}:{mongoDBPassword}@qualificationalitated.rc7ev.mongodb.net/?retryWrites=true&w=majority&appName=qualificationalitated";

        // MongoDB 클라이언트 생성
        mongoClient = new MongoClient(connectionString);

        // 데이터베이스 선택
        database = mongoClient.GetDatabase(mongoDBName);

        // 데이터베이스 컬렉션 설정
        // 컴퍼니 컬렉션 = 컴퍼니 컬렉션 스트링을 넣어서 잡은 진짜 컬렉션;
        // 유저 컬렉션 = 컴퍼니 컬렉션 스트링을 넣어서 잡은 진짜 컬렉션;
        // 게임카테고리 컬렉션 = 컴퍼니 컬렉션 스트링을 넣어서 잡은 진짜 컬렉션;
        // 잡카테고리 컬렉션 = 컴퍼니 컬렉션 스트링을 넣어서 잡은 진짜 컬렉션;


        Debug.Log("MongoDB에 연결되었습니다.");
    }
}