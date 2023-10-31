using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;

public class QuestUIScript : MonoBehaviour
{
    public Transform contentPanel; // Parent transform for UI elements
    public GameObject questPrefab; // Prefab for UI element
    public GameObject spacerPrefab; // Prefab for Space element
    public GameObject uiObjectToToggle;
    public GameObject Quest;
    public GameObject Finished;
    public KeyCode inputKey = KeyCode.I;
    public int CurrentQuest;
    public int totalQuestValue;

    //Quest Data Condition
    public int quest1;
    public int quest2;
    public int quest3;
    public int quest4;
    public int quest5;
    public string Name;

    // public string temps;

    private bool isHoldingKey = false;

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
                }
            });
        }
        else
        {
            InitializeFirestore();
        }
    }
    private async void InitializeFirestore()
    {

        // Initialize FirebaseApp (if not already initialized)
        if (FirebaseApp.CheckAndFixDependenciesAsync().Result == DependencyStatus.Available)
        {
            // Access Firestore instance
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            // Reference to the UsersLog collection
            CollectionReference usersLogCollection = db.Collection("UsersLog");

            // Query the collection to get documents where any 'Quest.Condition' is equal to 1
            QuerySnapshot snapshot = await usersLogCollection.WhereEqualTo("Quest1.Condition", 1).WhereEqualTo("Username", "Alfian").GetSnapshotAsync();
            bool firstQuest = true; // To track if it's the first quest

            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Dictionary<string, object> documentData = documentSnapshot.ToDictionary();

                // Iterate through the keys in the document's data dictionary
                foreach (string key in documentData.Keys)
                {
                    if (key.StartsWith("Quest"))
                    {
                        totalQuestValue++;
                        Dictionary<string, object> questData = (Dictionary<string, object>)documentData[key];
                        if (questData["Condition"] is long conditionValue && conditionValue != 0)
                        {
                            int condition = (int)conditionValue;
                            CurrentQuest++;
                            string goals = questData.ContainsKey("Goals") ? (string)questData["Goals"] : "";
                            string description = questData.ContainsKey("Description") ? (string)questData["Description"] : "";
                            if (!firstQuest)
                            {
                                Instantiate(spacerPrefab, contentPanel);
                            }

                            // Instantiate the UI element prefab
                            GameObject questUI = Instantiate(questPrefab, contentPanel);
                            firstQuest = false;
                            // Get UI components from the instantiated UI element
                            TextMeshProUGUI questNameText = questUI.GetComponentInChildren<TextMeshProUGUI>();
                            TextMeshProUGUI goalsText = questUI.transform.Find("GoalsText").GetComponent<TextMeshProUGUI>();
                            TextMeshProUGUI goalsText2 = questUI.transform.Find("GoalsText2").GetComponent<TextMeshProUGUI>();
                            TextMeshProUGUI descriptionText = questUI.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
                            TextMeshProUGUI ProgressText = questUI.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
                            // Update UI elements with the quest data
                            questNameText.text = key; // Set the quest name based on the key
                            goalsText.text = "Goals: " + goals;
                            goalsText2.text = "Goals: " + goals;
                            descriptionText.text = description;
                            Debug.Log(CurrentQuest);
                            Debug.Log(totalQuestValue);



                            // ProgressText.text = CurrentQuest / totalQuestValue * 100 + " %";
                        }

                    }
                }
            }

        }
        else
        {
            Debug.LogError("Firebase could not be initialized.");
        }
    }

    private void Update()
    {

        if (Input.GetKey(inputKey))
        {
            if (!isHoldingKey)
            {
                // Key is pressed, toggle the UI object
                testing();
                uiObjectToToggle.SetActive(!uiObjectToToggle.activeSelf);
                isHoldingKey = true;
            }
        }
        else
        {
            // Key is released, reset the flag
            isHoldingKey = false;
        }
    }

    public void QuestUI()
    {
        Debug.Log("asu");
        Finished.SetActive(false);
        Quest.SetActive(true);
    }

    public void FinishedQuest()
    {
        Debug.Log("asu2");
        Quest.SetActive(false);
        Finished.SetActive(true);
    }

    public void testing()
    {
        quest1 = PlayerPrefs.GetInt("Quest1");
        quest2 = PlayerPrefs.GetInt("Quest2");
        quest3 = PlayerPrefs.GetInt("Quest3");
        quest4 = PlayerPrefs.GetInt("Quest4");
        quest5 = PlayerPrefs.GetInt("Quest5");
        Name = PlayerPrefs.GetString("nama");

        Debug.Log(quest1);
        Debug.Log(quest2);
        Debug.Log(quest3);
        Debug.Log(quest4);
        Debug.Log(quest5);
        Debug.Log(Name);
    }

}