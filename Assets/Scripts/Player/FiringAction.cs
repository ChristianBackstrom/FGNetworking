using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;

    [SerializeField] private PlayerAntiCheat playerAntiCheat;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float shootCooldown = 2f;

    public NetworkVariable<int> ammo;

    private NetworkVariable<bool> canShoot = new(true);

    private float timeSinceLastShot = 0;
    
    public override void OnNetworkSpawn()
    {
        playerController.onFireEvent += Fire;
        if (!IsServer) return;
        ammo.Value = maxAmmo;
    }

    public void ReplenishAmmo()
    {
        if (!IsServer) return;

        ammo.Value = maxAmmo;
    }

    private void Fire(bool isShooting)
    {
        if (isShooting && ammo.Value > 0 && canShoot.Value)
        {
            ShootLocalBullet();
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        timeSinceLastShot += Time.deltaTime;
        
        if (timeSinceLastShot < shootCooldown || canShoot.Value) return;

        canShoot.Value = true;
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        if (playerAntiCheat.IsMovingTooFast)
        {
            string username = SavedClientInformationManager.GetUserData(NetworkObject.OwnerClientId).userName;
            NetworkManager.Singleton.DisconnectClient(NetworkObject.OwnerClientId);
            FindObjectOfType<KickPlayer>().PlayerKickedClientRpc(username);
            return;
        }
        
        if (!canShoot.Value) return;
        timeSinceLastShot = 0;
        canShoot.Value = false;

        ammo.Value--;
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

    }

    private void ShootLocalBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }
}
