using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class ScoreCounter
{
    private static Dictionary<ulong, int> scoreboard;

    public static Dictionary<ulong, int> Scoreboard => scoreboard;

    public static void AddClient(ulong networkID)
    {
        if (scoreboard == null) scoreboard = new Dictionary<ulong, int>();
        scoreboard.Add(networkID, 0);
    }
    
    public static void RemoveClient(ulong networkID)
    {
        try
        {
            scoreboard.Remove(networkID);
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
    }

    public static int GetScore(ulong networkID)
    {
        return scoreboard[networkID];
    }
}
