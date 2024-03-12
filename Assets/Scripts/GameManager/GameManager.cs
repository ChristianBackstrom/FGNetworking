using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<GameOptions> gameOptions = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (!IsServer) return;

        print("manager");
        
        GameOptions options = new GameOptions
        {
            UnlimitedLives = Convert.ToBoolean(PlayerPrefs.GetInt("UnlimitedLives"))
        };

        gameOptions.Value = options;
    }
}

public struct GameOptions : INetworkSerializable, System.IEquatable<GameOptions>
{
    public bool UnlimitedLives;

    GameOptions(bool unlimitedLives)
    {
        UnlimitedLives = unlimitedLives;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out UnlimitedLives);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(UnlimitedLives);
        }
    }

    public bool Equals(GameOptions other)
    {
        return UnlimitedLives == other.UnlimitedLives;
    }
}