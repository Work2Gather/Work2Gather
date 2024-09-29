using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MongoDB.Bson;
using System;

public class ClientManager : MonoBehaviour
{
    // NetworkManager를 참조하기 위한 변수
    public NetworkManager networkManager;
    public string currentUserId;

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 클라이언트가 연결될 때 호출될 콜백 등록
        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnTransportFailure += OnTransportFailure;
        currentUserId = "66f7f90c1c582de6b073faca";
    }

    // void Update()
    // {
    //     if(networkManager.IsListening)
    //     {
    //         Debug.Log("S");
    //     }
    // }

    private void OnTransportFailure()
    {
        Debug.Log("Client connect fail");
    }

    public void StartClient()
    {
        Debug.Log(networkManager.StartClient());
    }

    // 클라이언트가 연결되었을 때 호출될 함수
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"클라이언트가 연결되었습니다. Client ID: {clientId}");
    }
}