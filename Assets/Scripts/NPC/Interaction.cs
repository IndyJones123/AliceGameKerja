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
    public MonoBehaviour scriptToToggle;
    public GameObject dialogUI;
    public TMP_Text NamaNPC;
    public TMP_Text dialogText;
    public string npcName;
    private FirebaseFirestore db;
    private bool isScriptEnabled = false;
    private int playerCollisions;
    [SerializeField] public string quest;
    private int index = 0;
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
            // CollectionReference npcDocRef = db.Collection("UsersLog");
            // Query query = npcDocRef.WhereEqualTo("Username", "Alfian");

            // query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            // {
            //     if (task.Exception != null)
            //     {
            //         Debug.Log(playerCollisions);
            //         Debug.LogError($"Failed to fetch user data: {task.Exception}");
            //     }
            //     else
            //     {
            //         QuerySnapshot querySnapshot = task.Result;

            //         if (querySnapshot != null && querySnapshot.Any())
            //         {
            //             Debug.Log(quest);
            //             DocumentSnapshot userData = querySnapshot.Documents.FirstOrDefault();
            //             IDictionary<string, object> userLogMap = userData.ToDictionary();
            //             Debug.Log(userLogMap.ToString());
            //             if (userLogMap != null)
            //             {
            //                 // Now you can access specific values from the map using keys.
            //                 // Assuming the key you want to read is "collisionIndex" as an integer.
            //                 if (userLogMap.TryGetValue("Condition", out object ConditionObj) && ConditionObj is long ConditionLong)
            //                 {
            //                     if (playerCollisions != 0)
            //                     {
            //                         // Convert the long value to an integer.
            //                         int playerCollisions = (int)ConditionLong;
            //                         Debug.Log($"Collision Index: {playerCollisions}");
            //                         // Now you can use playerCollisions as needed.
            //                         // ToggleScript();
            //                         FetchNPCDataAndShowDialog(playerCollisions, index);
            //                     }
            //                 }
            //                 else
            //                 {
            //                     Debug.LogError("Failed to retrieve collisionIndex from the map or it's not of type long.");
            //                 }
            //             }
            //         }
            //     }
            // });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check the tag of the other GameObject
        {
            Leave.Invoke();
        }
    }



    private void FetchNPCDataAndShowDialog(int condition, int index)
    {
        // Mendapatkan referensi collection NPC dari Firestore
        CollectionReference npcDocRef = db.Collection("Dialog");

        // Membuat query untuk mencari dokumen dengan field dan nilai tertentu
        Query query = npcDocRef.WhereEqualTo("NPC", "Dokter").WhereEqualTo("Condition", condition);

        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to fetch NPC data: {task.Exception}");
            }
            else
            {
                QuerySnapshot querySnapshot = task.Result;

                if (querySnapshot != null && querySnapshot.Any())
                {
                    // Ambil dokumen pertama dari hasil query
                    DocumentSnapshot npcData = querySnapshot.Documents.FirstOrDefault();

                    string npcName = npcData.GetValue<string>("npc");
                    List<string> dialogList = npcData.GetValue<List<string>>("massage");

                    if (dialogList != null && dialogList.Count > 0 && index < dialogList.Count)
                    {
                        ShowDialog(npcName, dialogList[index]);
                    }
                    else
                    {
                        CloseDialog();
                    }
                }
                else
                {
                    CloseDialog();
                }
            }
        });
    }

    private void ShowDialog(string npcName, string dialog)
    {
        dialogUI.SetActive(false);
        dialogUI.SetActive(true);
        NamaNPC.text = npcName;
        dialogText.text = dialog;
    }

    public void NextDialog()
    {
        index++;
        FetchNPCDataAndShowDialog(playerCollisions, index);
    }

    public void CloseDialog()
    {
        // ToggleScript();
        dialogUI.SetActive(false);
    }

    private void ToggleScript()
    {
        if (scriptToToggle != null)
        {
            isScriptEnabled = !isScriptEnabled;
            scriptToToggle.enabled = isScriptEnabled;
        }
    }


}
