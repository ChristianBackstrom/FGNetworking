using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Moving : NetworkBehaviour
{
    public NetworkVariable<bool> IsMoving = new(false, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        
    }
}
