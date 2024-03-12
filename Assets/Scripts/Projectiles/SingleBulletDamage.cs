using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBulletDamage : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    public ulong OwnerID;

    void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.transform.GetComponent<Health>();
        if(health == null) return;
        health.TakeDamage(damage, OwnerID);
    }
}
