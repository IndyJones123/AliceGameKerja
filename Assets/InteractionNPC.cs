using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;
using UnityEngine.Events;
using System.IO;

public class InteractionNPC : MonoBehaviour
{
    private FirebaseFirestore db;
    public bool interact = false;
    public UnityEvent Trigger;
    public UnityEvent Talk;
    public UnityEvent Leave;
    public string username;
    private void Start()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError($"Firebase initialization failed: {task.Exception}");
                }
                else
                {
                    InitializeFirestore();
                    LoadFromJson();
                }
            });
        }
        else
        {
            InitializeFirestore();
            LoadFromJson();
        }
    }

    private void InitializeFirestore()
    {
        // Mendapatkan instance FirebaseFirestore
        db = FirebaseFirestore.DefaultInstance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Trigger.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Talk.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check the tag of the other GameObject
        {
            Leave.Invoke();
            interact = false;
        }
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath+"/LogsPlayerDatas.json");
        SaveUsersLogDataNPC data = JsonUtility.FromJson<SaveUsersLogDataNPC>(json);

        username = data.username;
    }

    void Update()
    {
                // Check if the "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            interact = !interact;
        }
    }
}

[System.Serializable]
public class SaveUsersLogDataNPC
{
    public string username;

}