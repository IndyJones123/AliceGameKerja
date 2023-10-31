using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.IO;

public class BajuDokter : MonoBehaviour, ICollectible
{
    private FirebaseFirestore db;
    public string username;
    void Start()
    {
        LoadFromJson();
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
                    }
                });
            }
            else
            {
                InitializeFirestore();
            } 
    }
    private void InitializeFirestore()
    {
        // Mendapatkan instance FirebaseFirestore
        db = FirebaseFirestore.DefaultInstance;

    }

    //overiding Collect Function from ICollectible
    public void Collect()
    {
        DocumentReference docRef = db.Collection("UsersLog").Document(username);
        docRef.GetSnapshotAsync().ContinueWith(task =>
        {
    if (task.Result.Exists)
    {
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            {"Quest1", 1},
        };
        docRef.UpdateAsync(userData);
        Debug.Log("succes"+username);
    }
    else
    {
        Debug.Log("gagal");

        
            
    }});
        Destroy(gameObject);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath+"/LogsPlayerDatas.json");
        SaveUsersLogDatas23 data = JsonUtility.FromJson<SaveUsersLogDatas23>(json);

        username = data.username;
    }
}

[System.Serializable]
public class SaveUsersLogDatas23
{
    public string username;

}
