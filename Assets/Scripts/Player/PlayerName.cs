using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    // The variable that stores the playername which is synced over all clients
    public NetworkVariable<FixedString64Bytes> playerName = new();
    
    public override void OnNetworkSpawn()
    {
        // if it is not the server this should not be touched
        if (!IsServer) return;
        
        // Gets the player name from the saved information from the server
        string userName = SavedClientInformationManager.GetUserData(NetworkObject.OwnerClientId).userName;
        playerName.Value = userName;
    }
}
