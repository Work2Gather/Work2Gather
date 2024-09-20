using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : MonoBehaviour
{
    private NetworkManager networkManager;
    [SerializeField] GameObject PlayerPrefab;

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        // 서버가 시작되었을 때 호출될 콜백 등록
        networkManager.OnServerStarted += HandleServerStarted;

        // 클라이언트가 연결되었을 때 호출될 콜백 등록
        networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    // 서버가 시작되었을 때 호출될 함수
    private void HandleServerStarted()
    {
        Debug.Log("서버가 시작되었습니다.");
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            InstantiatePlayer(clientId);
        }
    }

    // 플레이어를 스폰하는 함수
    private void InstantiatePlayer(ulong clientId)
    {
        GameObject playerInstance = Instantiate(PlayerPrefab);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        // 플레이어를 네트워크에 등록하고 클라이언트에 동기화
        networkObject.SpawnAsPlayerObject(clientId);
    }

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
}