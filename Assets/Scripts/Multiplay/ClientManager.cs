using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using MongoDB.Bson;
using System;

public class ClientManager : MonoBehaviour
{
    public string currentUserId;
    public int currentMap;
    // Start is called before the first frame update
    void Start()
    {
        //currentUserId = "66f7f90c1c582de6b073faca";
        currentMap = 0;

        DontDestroyOnLoad(this.gameObject);
    }
}