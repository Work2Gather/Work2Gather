using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ClientManager : MonoBehaviour
{
    // NetworkManager를 참조하기 위한 변수
    private NetworkManager networkManager;

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public GameObject InstantiatePlayer(GameObject playerObject, Vector3 position)
    {
        GameObject playerInstance = Instantiate(playerObject, position, Quaternion.identity);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        // 플레이어를 네트워크에 등록하고 클라이언트에 동기화
        networkObject.SpawnAsPlayerObject(networkManager.LocalClientId);

        return playerInstance;
    }
}