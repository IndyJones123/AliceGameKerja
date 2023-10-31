using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using System.Collections.Generic;

public class Register : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseFirestore db;

    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private void Start()
    {
        // Inisialisasi FirebaseApp jika belum dilakukan
        if (FirebaseApp.DefaultInstance == null)
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError($"Failed to initialize Firebase: {task.Exception}");
                    return;
                }

                InitializeFirestore();
            });
        else
            InitializeFirestore();
    }

    private void InitializeFirestore()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public void SaveUser()
    {
    // if (auth == null || auth.CurrentUser == null)
    // {
    //     Debug.LogError("User not logged in or FirebaseAuth not initialized!");
    //     return;
    // }

    string username = usernameInput.text;
    string email = emailInput.text;
    string password = passwordInput.text;

    // Mengambil referensi koleksi "users"
    CollectionReference usersRef = db.Collection("users");

    // Membuat data yang akan disimpan
    var user = new Dictionary<string, object>
    {
        { "Username", username },
        { "Email", email },
        { "Password", password }
    };

    // Menyimpan data ke Firestore dengan menggunakan metode SetAsync
    usersRef.Document(auth.CurrentUser.UserId).SetAsync(user)
        .ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to save user data: {task.Exception}");
                return;
            }

            Debug.Log("User data saved successfully!");
        });
    }

}
