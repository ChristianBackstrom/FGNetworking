using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();


    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        currentHealth.Value = 100;
    }


    public void TakeDamage(int damage){
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;
    }

    public void Heal(int health)
    {
        health = health < 0 ? -health : health;
        
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + health, 0, 100);
    }

}
