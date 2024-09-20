using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private float xInput;
    private float zInput;
    private float speed = 0.05f;

    // 네트워크 변수를 추가하여 클라이언트 간에 이동 정보를 동기화
    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Start()
    {
        // 서버에서 위치 변수가 변경될 때마다 클라이언트에 반영
        position.OnValueChanged += (oldValue, newValue) => {
            transform.position = newValue;
        };
    }

    void Update() 
    {
        if (IsOwner) 
        {
            // 클라이언트 입력 처리
            Move();
            RequestMoveServerRpc(new Vector2(xInput, zInput));
        }

        // 위치를 업데이트 (서버에서 동기화된 위치를 클라이언트가 반영)
        transform.position = position.Value;
    }

    void Move()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    }

    // 서버에 이동 요청
    [ServerRpc]
    void RequestMoveServerRpc(Vector2 input)
    {
        Vector3 newPosition = transform.position + new Vector3(input.x, 0, input.y) * speed;

        // 서버가 위치를 갱신
        position.Value = newPosition;
    }
}