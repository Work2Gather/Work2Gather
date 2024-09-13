using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientManager : MonoBehaviour
{
    // NetworkManager를 참조하기 위한 변수
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Singleton;
        // 클라이언트가 연결될 때 호출될 콜백 등록
        networkManager.OnClientConnectedCallback += OnClientConnected;

        // 서버에 접속
        StartClient();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"클라이언트가 연결되었습니다. Client ID: {clientId}");
    }
}