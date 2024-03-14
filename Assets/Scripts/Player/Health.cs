using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new();

    public NetworkVariable<int> lives = new();

    public GameManager gameManager;

    public NetworkVariable<int> shield = new(2); 


    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        currentHealth.Value = 100;
        lives.Value = 3;
        
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        if (shield.Value > 0)
        {
            shield.Value--;
            return;
        }
        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;
        
        if (currentHealth.Value <= 0) PlayerDied();
    }

    public void Heal(int health)
    {
        health = health < 0 ? -health : health;
        
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + health, 0, 100);
    }

    private void PlayerDied()
    {
        lives.Value--;

        if (!gameManager)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        print(gameManager.gameOptions.Value.UnlimitedLives);

        if (lives.Value > 0 || gameManager.gameOptions.Value.UnlimitedLives)
        {
            Respawn();
            return;
        }
        
        NetworkManager.Singleton.DisconnectClient(NetworkObject.OwnerClientId);
    }

    public void ReplenishShield()
    {
        if (!IsServer) return;
        
        shield.Value = 2;
    }
    
    private void Respawn()
    {
        Vector3 randomPosition = new (Random.Range(-4, 4),Random.Range(-2, 2));
        GetComponent<FiringAction>().ammo.Value = 30;
        currentHealth.Value = 100;
        RespawnClientRpc(randomPosition);
    }
    
    [ClientRpc]
    private void RespawnClientRpc(Vector3 position)
    {
        if (!IsOwner) return;
        transform.position = position;
        transform.rotation = quaternion.identity;
    }

}
