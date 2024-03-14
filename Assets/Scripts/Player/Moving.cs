using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Moving : NetworkBehaviour
{
    public NetworkVariable<bool> IsMoving = new(false, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private Rigidbody2D rigidbody2D;

    private void Update()
    {
        if (!IsOwner) return;
        
        IsMoving.Value = rigidbody2D.velocity.magnitude > 1f;
    }
}
