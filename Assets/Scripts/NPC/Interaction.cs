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

public class Interaction : MonoBehaviour
{
    public GameObject dialogUI;

    private FirebaseFirestore db;

    public UnityEvent Quest;
    public UnityEvent Leave;

    private void Start()
    {
        dialogUI.SetActive(false);

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
            Quest.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check the tag of the other GameObject
        {
            Leave.Invoke();
        }
    }

}
