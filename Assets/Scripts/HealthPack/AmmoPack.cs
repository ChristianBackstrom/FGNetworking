using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AmmoPack : NetworkBehaviour
{
    [SerializeField] GameObject ammoPrefab;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!IsServer) return;

        FiringAction firingAction = other.GetComponent<FiringAction>();
        if(!firingAction) return;
        firingAction.ReplenishAmmo();
       
        int xPosition = Random.Range(-4, 4);
        int yPosition = Random.Range(-2, 2);
        GameObject newMine = Instantiate(ammoPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        NetworkObject no = newMine.GetComponent<NetworkObject>();
        no.Spawn();
       
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn(); 
    }
}