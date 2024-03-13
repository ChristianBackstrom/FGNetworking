using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUI : NetworkBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement rootElement;

    private ListView usernameListView;
    private ListView scoreListView;

    [SerializeField] private List<int> scores = new();
    [SerializeField] private List<string> usernames = new();

    private NetworkList<ulong> connectedUsers = new(); 
    private NetworkList<int> userScores = new();



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        rootElement = uiDocument.rootVisualElement;
        scoreListView = rootElement.Q<ListView>("ScoreListView");
        usernameListView = rootElement.Q<ListView>("UserListView");
        scoreListView.itemsSource = scores;
        usernameListView.itemsSource = usernames;
        
        connectedUsers.OnListChanged += ConnectedUsersOnListChanged;
        userScores.OnListChanged += UserScoresOnListChanged;
        
        UpdateNetworkLists();
        
        if (!IsServer) return;

        foreach (var data in ScoreCounter.GetAllData())
        {
            connectedUsers.Add(data.NetworkID);
            userScores.Add(data.Score);
            Debug.Log(data.Score);
        }
    }

    private void Update()
    {
        // if (scoreListView.itemsSource.Count != userScores.Count || usernameListView.itemsSource.Count != connectedUsers.Count) 
        //     UpdateNetworkLists();
        
        if (!IsServer) return;

        Dictionary<ulong, int> scoreboard = ScoreCounter.Scoreboard;
        
        if (scoreboard.Count != connectedUsers.Count)
        {
            UpdateNetworkLists();
            return;
        }

        for (int i = 0; i < connectedUsers.Count; i++)
        {
            if (userScores[i] == scoreboard[connectedUsers[i]]) continue;
            
            UpdateNetworkLists();
            return;
        }
    }

    private void UpdateNetworkLists()
    {
        if (!IsServer) return;
        List<int> scoreboardScores = new();
        List<ulong> names = new();
        
        foreach (var scoreData in ScoreCounter.Scoreboard)
        {
            names.Add(scoreData.Key);
            scoreboardScores.Add(scoreData.Value);
        }

        connectedUsers = new NetworkList<ulong>(names);
        userScores = new NetworkList<int>(scoreboardScores);
    }


    private void ConnectedUsersOnListChanged(NetworkListEvent<ulong> changeevent)
    {
        usernames.Clear();
        print("usernames updated");

        foreach (var name in connectedUsers)
        {
            usernames.Add(SavedClientInformationManager.GetUserData(name).userName);
        }
        usernameListView.Rebuild();
    }
    
    private void UserScoresOnListChanged(NetworkListEvent<int> changeevent)
    {
        scores.Clear();
        
        foreach (var score in userScores)
        {
            scores.Add(score);
        }
        scoreListView.Rebuild();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ScoreUI))]
public class ScoreUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var data in ScoreCounter.Scoreboard)
        {
            EditorGUILayout.TextField($"{SavedClientInformationManager.GetUserData(data.Key).userName} || {data.Value} ");
        }
    }
}

#endif
