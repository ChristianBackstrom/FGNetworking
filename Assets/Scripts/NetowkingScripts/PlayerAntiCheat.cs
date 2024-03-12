using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAntiCheat : NetworkBehaviour
{
    [SerializeField] private NetworkTransformClientAuth networkTransformClientAuth;
    public bool IsMovingTooFast => Vector2.Distance(lastPosition, transform.position) > cheatSpeedThreshold;

    [SerializeField] private float cheatSpeedThreshold = 1;
    
    private Vector2 lastPosition;

    private void Awake()
    {
        if (IsServer)
            lastPosition = transform.position;
    }

    private void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.C))
        {
            Cheat();
        }
        
        if (!IsServer) return;
        
        // print($"{SavedClientInformationManager.GetUserData(NetworkObject.OwnerClientId).userName}\n lastPosition: {lastPosition} currentPosition: {transform.position}");

        lastPosition = transform.position;
    }

    private void Cheat()
    {
        transform.position += (Vector3) Vector2.right * 4;
        GetComponent<PlayerController>().onFireEvent.Invoke(true);
    }
}
