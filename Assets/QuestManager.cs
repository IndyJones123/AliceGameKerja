using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;


public class QuestManager : MonoBehaviour
{
    public UnityEvent Quest1;
    private FirebaseFirestore db;
    // Start is called before the first frame update

    public void Start()
    {

            Debug.Log("Test");
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
                    }
                });
            }
            else
            {
                InitializeFirestore();
            }
        
         Quest1.Invoke();
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
}
