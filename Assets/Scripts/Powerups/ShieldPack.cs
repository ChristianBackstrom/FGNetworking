using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShieldPack : NetworkBehaviour
{
    [SerializeField] GameObject shieldPackPrefab;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        Health health = other.GetComponent<Health>();
        if(!health) return;
        health.ReplenishShield();
       
        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);
        GameObject newMine = Instantiate(shieldPackPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        NetworkObject no = newMine.GetComponent<NetworkObject>();
        no.Spawn();
       
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn(); 
    }
}
