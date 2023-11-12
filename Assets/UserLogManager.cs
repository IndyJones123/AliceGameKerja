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


public class UserLogManager : MonoBehaviour
{
    public SaveUsersLogData data = new SaveUsersLogData();

    public TMP_InputField EnteredUsername;
    public TMP_InputField EnteredPassword;
    public TMP_Text StatusText;
    public string namagame;
    private FirebaseFirestore db;
    public string sceneName;
    public bool Logedin = false;
    public UnityEvent test;
    
    public int value;

    //mustdelete
    public string temp;

    private static UserLogManager instance;
    public void Start()
    {
        // PlayerPrefs.DeleteAll();
        // name = "Quest1";
        // value = 8080;
        // testing();
        // testing("Quest2",1010);
        if (Logedin == true)
        {
            Debug.Log(name);
            Debug.Log(value);

        }
        else
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
        }
    }

    private void InitializeFirestore()
    {
        // Mendapatkan instance FirebaseFirestore
        db = FirebaseFirestore.DefaultInstance;

    }

    // Update is called once per frame
    public void Login()
    {

        string Username = EnteredUsername.text;
        string Password = EnteredPassword.text;

        CollectionReference npcDocRef = db.Collection("Users");
        Query query = npcDocRef.WhereEqualTo("username", Username).WhereEqualTo("password", Password);

        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
           {
               if (task.Exception != null)
               {
                   StatusText.text = "Login Gagal";
               }
               else
               {
                   bool loginSuccessful = false;

                   foreach (DocumentSnapshot document in task.Result.Documents)
                   {
                       if (document.Exists)
                       {
                           loginSuccessful = true;
                           break; // Exit the loop since a match was found
                       }
                   }

                   if (loginSuccessful)
                   {
                       Logedin = true;
                       LoadScene();
                   }
                   else
                   {
                       StatusText.text = "Username or password is incorrect";
                   }
               }
           });
    }

    public void LoadScene()
{
    string username = EnteredUsername.text;
    Debug.Log(username);
    
    CollectionReference collectionRef = db.Collection("UsersLog");
    Query query = collectionRef.WhereEqualTo("Username", username).WhereEqualTo("game", namagame);

    query.GetSnapshotAsync().ContinueWith(task =>
    {
        if (task.Result.Documents.Count() > 0)
        {
            Debug.Log("Document already exists for username: " + username);
            SaveToJson();
        }
        else
        {
            // Create a new document with the specified data
            DocumentReference newDocRef = collectionRef.Document();
            
            Dictionary<string, object> userData = new Dictionary<string, object>
            {
                {"quest", new int[] {0, -1, -1, -1, -1}},
                {"Username", username},
                {"game", namagame}
            };

            // Set the data for the new document
            newDocRef.SetAsync(userData).ContinueWithOnMainThread(addTask =>
            {
                if (addTask.IsCompleted)
                {
                    Debug.Log("Document created successfully for username: " + username);
                    SaveToJson();
                }
                else
                {
                    Debug.LogError("Error creating document: " + addTask.Exception);
                }
            });
        }
    });

    test.Invoke();
}

        // Dictionary<string, object> quest1Data = new Dictionary<string, object>
        // {
        //     {"Condition", 0}
        // };

        // Dictionary<string, object> quest2Data = new Dictionary<string, object>
        // {
        //     {"Condition", 0}
        // };

        // Dictionary<string, object> userData = new Dictionary<string, object>
        // {
        //     {"Quest1", quest1Data},
        //     {"Quest2", quest2Data},
        //     {"Username", username}
        // };
        // docRef.SetAsync(userData, SetOptions.MergeAll);
        // SceneManager.LoadScene(sceneName);

//     public void Loading()
// {
//     string username = EnteredUsername.text;
//     Debug.Log(username);
//     PlayerPrefs.SetString("nama", username);
//         DocumentReference docRef = db.Collection("UsersLog").Document(username);
        
//         docRef.GetSnapshotAsync().ContinueWithOnMainThread(snapshotTask =>
//         {
//             if (snapshotTask.IsFaulted)
//             {
//                 Debug.LogError("Error fetching document: " + snapshotTask.Exception);
//                 return;
//             }

//             DocumentSnapshot snapshot = snapshotTask.Result;

//             if (snapshot.Exists)
//             {
//                 IDictionary<string, object> userData = snapshot.ToDictionary();

//                 foreach (KeyValuePair<string, object> field in userData)
//                 {
//                     if (field.Key.StartsWith("Quest") && field.Value is long questValue)
//                     {
//                         string nameValue = field.Key;
//                         testing(nameValue, (int)questValue);
//                     }
//                 }
//             }
//             else
//             {
//                 Debug.Log("Document does not exist for username: " + username);
//             }
//         });
// }
        // docRef.GetSnapshotAsync().ContinueWith(task =>
        // {
        //     if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
        //     {
        //         DocumentSnapshot snapshot = task.Result;

        //         if (snapshot.Exists)
        //         {
        //             IDictionary<string, object> userData = snapshot.ToDictionary();

        //             for (int questNumber = 0; questNumber < 3; questNumber++) // Adjust the range as needed
        //             {

        //                 string questName = "Quest" + questNumber;
        //                 // Debug.Log(questNumber);
        //                 // Debug.Log(questName);
        //                 if (userData.ContainsKey(questName) && userData[questName] is IDictionary<string, object> questData)
        //                 {
        //                     if (questData.TryGetValue("Condition", out var questConditionObj) && questConditionObj is long questConditionLong)
        //                     {
        //                         int questCondition = (int)questConditionLong;
        //                         Debug.Log(questName + "   " + questCondition);
        //                         name = questName;
        //                         value = questNumber;
        //                         questNumber++;
        //                     }
        //                     else
        //                     {
        //                         break;
        //                     }

        //                 }
        //             }
        //         }
        //         else
        //         {
        //             Debug.Log("Document does not exist for username: " + username);
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogError("Error retrieving document: " + task.Exception);
        //     }
        // });

    // public void testing(string name, int value)
    // {
    //     Debug.Log(name);
    //     Debug.Log(value);
    //     PlayerPrefs.SetInt(name, value);
    // }
    public void SaveToJson()
    {
       
        data.username = EnteredUsername.text;

        string test12 = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath+"/LogsPlayerDatas.json";
        Debug.Log(test12);
        Debug.Log(filePath);
         Debug.Log("IwasHere");
        System.IO.File.WriteAllText(filePath,test12);
    }



}

[System.Serializable]
public class SaveUsersLogData
{
    public string username;

}