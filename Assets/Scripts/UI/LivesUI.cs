using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    [SerializeField] private Text livesText;
    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager)
        {
            gameObject.SetActive(!gameManager.gameOptions.Value.UnlimitedLives);
        }
        
        print("Searching for Health");
        foreach (var health in FindObjectsOfType<Health>())
        {
            if (!health.IsOwner) continue;
            print("Health Found");
            health.lives.OnValueChanged += OnLivesChanged;
        }
        
        livesText.text = "3/3";
    }

    public void OnLivesChanged(int previousValue, int newValue)
    {
        livesText.text = $"{newValue}/3";
    }

    public void ChangeText(string text)
    {
        livesText.text = text;
    }
}
