using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class KickPlayer : NetworkBehaviour
{
    [SerializeField] private Text UIText;
    
    [ClientRpc]
    public void PlayerKickedClientRpc(string name)
    {
        UIText.enabled = true;
        UIText.text = $"{name} has been kicked for cheating";
        Invoke(nameof(HideUI), 2f);
    }

    private void HideUI()
    {
        UIText.enabled = false;
    }
}
