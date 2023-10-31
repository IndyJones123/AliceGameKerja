using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using System.Reflection;
using System;

public class NextManager : MonoBehaviour
{
    private string username;
    public UnityEvent Quest1;
    public UnityEvent Quest2;
    public UnityEvent Quest3;
    public UnityEvent Quest4;
    public UnityEvent Quest5;
    private FirebaseFirestore db;
    // Start is called before the first frame update

    public void Start()
    {
        LoadFromJson();
        
            // Inisialisasi FirebaseApp jika belum dilakukan
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
                        FetchQuestData();
                    }
                });
            }
            else
            {
                InitializeFirestore();
                FetchQuestData();
            }
        
    }

    private void InitializeFirestore()
    {
        // Mendapatkan instance FirebaseFirestore
        db = FirebaseFirestore.DefaultInstance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath+"/LogsPlayerDatas.json");
        SaveUsersLogDatas2 data = JsonUtility.FromJson<SaveUsersLogDatas2>(json);

        username = data.username;
    }

    public void FetchQuestData()
    {
        InitializeFirestore();
        LoadFromJson();

        CollectionReference usersLogCollectionRef = db.Collection("UsersLog");
        Query query = usersLogCollectionRef.WhereEqualTo("Username", username);

        query.GetSnapshotAsync().ContinueWithOnMainThread(queryTask =>
            {
        if (queryTask.IsCompleted && !queryTask.IsFaulted && !queryTask.IsCanceled)
        {
            QuerySnapshot querySnapshot = queryTask.Result;
    

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                DocumentSnapshot documentData = querySnapshot.Documents.FirstOrDefault();
                int quest1Value = documentData.GetValue<int>("Quest1");
                int quest2Value = documentData.GetValue<int>("Quest2");
                int quest3Value = documentData.GetValue<int>("Quest3");
                int quest4Value = documentData.GetValue<int>("Quest4");
                int quest5Value = documentData.GetValue<int>("Quest5");

                if (quest1Value!=-1 && quest1Value!=99)
                {
                    Quest1.Invoke();
                }

                if (quest2Value!=-1 && quest2Value!=99)
                {
                    Quest2.Invoke();
                }
                if (quest3Value!=-1 && quest3Value!=99)
                {
                    Quest3.Invoke();
                }
                                if (quest4Value!=-1 && quest4Value!=99)
                {
                    Quest4.Invoke();
                }
                                if (quest5Value!=-1 && quest5Value!=99)
                {
                    Quest5.Invoke();
                }

            }
        }
        else
        {
            Debug.Log($"Error fetching document: {queryTask.Exception}");
        }
    });
    }
}

[System.Serializable]
public class SaveUsersLogDatas2
{
    public string username;

}
