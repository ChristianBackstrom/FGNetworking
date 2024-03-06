using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private PlayerName nameScript;

    private void Awake()
    {
        nameScript.playerName.OnValueChanged += ChangeText;
    }

    private void Start()
    {
        // this is to sync the UI over all clients since name doesnt change once players have joined
        ChangeText("", nameScript.playerName.Value);
    }

    private void ChangeText(FixedString64Bytes oldString, FixedString64Bytes newString)
    {
        textUI.text = newString.ToString();
    }
}
