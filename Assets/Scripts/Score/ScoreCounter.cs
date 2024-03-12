using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Object = System.Object;

public static class ScoreCounter
{
    private static Dictionary<ulong, int> scoreboard;
    public static Dictionary<ulong, int> Scoreboard => scoreboard;
    public static event EventHandler<ScoreData> OnValueChanged;
    public static event EventHandler<ulong> ClientDisconnected;
    public static event EventHandler<ulong> ClientConnected;


    public static void AddClient(ulong networkID)
    {
        if (scoreboard == null) scoreboard = new Dictionary<ulong, int>();
        scoreboard.Add(networkID, 0);
        ClientConnected?.Invoke(typeof(ScoreCounter), networkID);
    }
    
    public static void RemoveClient(ulong networkID)
    {
        try
        {
            scoreboard.Remove(networkID);
            ClientDisconnected?.Invoke(typeof(ScoreCounter), networkID);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static void IncreaseScore(ulong networkID)
    {
        scoreboard[networkID]++;
        OnValueChanged?.Invoke(typeof(ScoreCounter), new ScoreData()
        {
            NetworkID = networkID,
            Score = scoreboard[networkID],
        });
    }
    
    public static void DecreaseScore(ulong networkID)
    {
        scoreboard[networkID]--;
        OnValueChanged?.Invoke(typeof(ScoreCounter), new ScoreData()
        {
            NetworkID = networkID,
            Score = scoreboard[networkID],
        });
    }

    public static int GetScore(ulong networkID)
    {
        return scoreboard[networkID];
    }
}

public struct ScoreData
{
    public ulong NetworkID;
    public int Score;
}