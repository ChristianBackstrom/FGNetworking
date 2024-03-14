using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpriteChange : MonoBehaviour
{
    [SerializeField] private Moving moving;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        moving.IsMoving.OnValueChanged += OnMovingChanged;
    }

    private void OnMovingChanged(bool previousValue, bool newValue)
    {
        spriteRenderer.enabled = newValue;
    }
}
