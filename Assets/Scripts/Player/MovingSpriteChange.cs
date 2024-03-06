using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpriteChange : MonoBehaviour
{
    [SerializeField] private Moving moving;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        moving.IsMoving.OnValueChanged += OnMovingChanged;
    }

    private void OnMovingChanged(bool previousValue, bool newValue)
    {
        spriteRenderer.enabled = newValue;
    }

    private void Update()
    {
        if (!moving.IsOwner) return;
        
        moving.IsMoving.Value = rigidbody2D.velocity.magnitude > 1f;
    }
}
