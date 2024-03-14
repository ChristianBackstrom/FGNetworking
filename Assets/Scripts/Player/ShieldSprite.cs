using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSprite : MonoBehaviour
{
    [SerializeField] private Health health;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        health.shield.OnValueChanged += OnShieldChanged;
    }

    private void OnShieldChanged(int previousvalue, int newvalue)
    {
        Color color = sprite.color;

        color.a = newvalue / 4f;

        sprite.color = color;
    }
}
