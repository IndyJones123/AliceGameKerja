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
using System.Threading.Tasks;

public class ShowingAvailableDialog : MonoBehaviour
{

    private string username;
    public string NPCName;
    public string namagame;
    private FirebaseFirestore db;
    public int[] questValues;
    public Transform parentTransform;
    public Transform NextparentTransform;
    public GameObject PreviousDialog;
    public GameObject dialogUI;
    public GameObject Next;
    public TMP_Text NamaNPC;
    public TMP_Text dialogText;
    public Button buttonPrefab;
    public Button NextbuttonPrefab;
    public string Quest;
    public int condition;
    public UnityEvent Coba;


    //Intantiate Button Yang Akan Didelete
    private List<Button> specificInstantiatedButtons = new List<Button>();


    // Start is called before the first frame update
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
    // public void FillText()
    // {
    //     CollectionReference usersLogCollection = db.Collection("Userslog");

    //     Query usersLogQuery = usersLogCollection
    //         .WhereEqualTo("Username", username)
    //         .WhereEqualTo("game", namagame);

    //     QuerySnapshot querySnapshot = usersLogQuery.GetSnapshotAsync().Result;

    //     foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
    //     {
    //         if (documentSnapshot.Exists)
    //         {
    //             Debug.Log($"Username: {documentSnapshot.GetValue<string>("Username")}, game: {documentSnapshot.GetValue<string>("game")}, Quest: {string.Join(", ", documentSnapshot.GetValue<List<int>>("Quest"))}");
    //         }
    //     }
    // }

    // Update is called once per frame
public void FillText()
{
    Debug.Log(username);
    CollectionReference docRef = db.Collection("UsersLog");
    Query query = docRef.WhereEqualTo("Username", username).WhereEqualTo("game", namagame);

    query.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshotTask =>
    {
        if (querySnapshotTask.IsFaulted)
        {
            Debug.LogError("Error fetching documents: " + querySnapshotTask.Exception);
            return;
        }

        QuerySnapshot querySnapshot = querySnapshotTask.Result;

        if (querySnapshot.Documents.Count() > 0)
        {
            foreach (DocumentSnapshot snapshot in querySnapshot.Documents)
            {
                IDictionary<string, object> userData = snapshot.ToDictionary();

                // Check if the "Quest" field exists
                if (userData.TryGetValue("quest", out object questValue) && questValue is List<object> questList)
                {
                    // Iterate through the questList
                    for (int i = 0; i < questList.Count; i++)
                    {
                        if (questList[i] is long questValueInt)
                        {
                            Debug.Log("Quest " + i + ": " + questValueInt);

                            // Your logic for handling the quest value goes here
                            if (questValueInt != -1 && questValueInt != 99)
                            {
                                AssignIntVariable((int)questValueInt, "Quest" + (i+1) );
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("No documents found for username: " + username);
        }
    });
}


    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/LogsPlayerDatas.json");
        SaveUsersLogDatas data = JsonUtility.FromJson<SaveUsersLogDatas>(json);

        username = data.username;
    }

    public void AssignIntVariable(int Values, string NameValue)
    {
        string namaquest = NameValue;
        Debug.Log(namaquest);
        // Define the collection reference
        CollectionReference dialogCollection = db.Collection("Quest");

        // Build the query
        Query query = dialogCollection.WhereEqualTo("NamaQuest", namaquest).WhereEqualTo("game", namagame);

        query.GetSnapshotAsync().ContinueWithOnMainThread(queryTask =>
            {
                if (queryTask.IsCompleted && !queryTask.IsFaulted && !queryTask.IsCanceled)
                {
                    QuerySnapshot querySnapshot = queryTask.Result;

                    foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                    {
                        if (documentSnapshot.Exists)
                        {
                            DocumentSnapshot ListData = querySnapshot.Documents.FirstOrDefault();
                            List<string> dialogList = ListData.GetValue<List<string>>("Goals");
                            //IntantiatePrefabs Quest Yang Tersedia.
                            Button instantiatedButton = Instantiate(buttonPrefab, parentTransform);
                            TextMeshProUGUI buttonText = instantiatedButton.GetComponentInChildren<TextMeshProUGUI>();
                            buttonText.text = dialogList[Values];
                            instantiatedButton.onClick.AddListener(() => OnButtonClicked(instantiatedButton, namaquest, Values, 0));
                            // //IntantiatePrefabs Next Button Berdasarkan Quest Yang Dipilih.
                            // Button instantiatedButton2 = Instantiate(NextbuttonPrefab, NextparentTransform);
                            // TextMeshProUGUI buttonNextText = instantiatedButton2.GetComponentInChildren<TextMeshProUGUI>();
                            // buttonNextText.text = "Continue";
                            // instantiatedButton2.onClick.AddListener(() => NextButton(instantiatedButton2,namaquest,Values));

                        }
                    }
                }
                else
                {
                    Debug.LogError("Error executing query: " + queryTask.Exception);
                }
            });
    }

    public void OnButtonClicked(Button clickedButton, string QuestName, int Condition, int index)
    {

        Debug.Log("Button clicked: " + clickedButton.GetComponentInChildren<TextMeshProUGUI>().text);
        PlayerPrefs.SetInt(QuestName, 0);
        //IntantiatePrefabs


        // Call your function here based on the clicked button's data
        string buttonText = clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        CollectionReference dialogCollection = db.Collection("Dialog");

        // Build the query
        Query query = dialogCollection.WhereEqualTo("NPC", NPCName).WhereEqualTo("Quest", QuestName).WhereEqualTo("Condition", Condition).WhereEqualTo("game", namagame);
        query.GetSnapshotAsync().ContinueWithOnMainThread(queryTask =>
            {
                if (queryTask.IsCompleted && !queryTask.IsFaulted && !queryTask.IsCanceled)
                {
                    Debug.Log(NPCName);
                    Debug.Log(QuestName);
                    Debug.Log(Condition);
                    QuerySnapshot querySnapshot = queryTask.Result;

                    DocumentSnapshot documentSnapshot = querySnapshot.Documents.FirstOrDefault();

                    if (documentSnapshot.Exists)
                    {
                        Debug.Log("I WAS HERE");
                        string npcName = documentSnapshot.GetValue<string>("NPC");
                        List<string> dialogList = documentSnapshot.GetValue<List<string>>("Massage");
                        Debug.Log(dialogList[0]);

                        if (dialogList != null && dialogList.Count > 0 && index < dialogList.Count)
                        {
                            DeleteSpecificInstantiatedButtons();
                            ShowDialog(npcName, dialogList[index]);
                            Button instantiatedButton = Instantiate(buttonPrefab, NextparentTransform);
                            TextMeshProUGUI buttonText = instantiatedButton.GetComponentInChildren<TextMeshProUGUI>();
                            // Add the button to the list
                            specificInstantiatedButtons.Add(instantiatedButton);
                            buttonText.text = "Continue";
                            instantiatedButton.onClick.AddListener(() => OnButtonContinueClicked(instantiatedButton, QuestName, Condition, index));
                        }
                        else
                        {
                            CloseDialog();
                        }
                    }
                    else
                    {
                        // Handle the case when no documents are found or documentSnapshot is null
                    }
                }
            });
    }

    public void OnButtonContinueClicked(Button clickedButton, string QuestName, int Condition, int index)
    {
        CollectionReference dialogCollection = db.Collection("Dialog");
        // Build the query
        Query query = dialogCollection.WhereEqualTo("NPC", NPCName).WhereEqualTo("Quest", QuestName).WhereEqualTo("Condition", Condition).WhereEqualTo("game", namagame);
        query.GetSnapshotAsync().ContinueWithOnMainThread(queryTask =>
            {
                if (queryTask.IsCompleted && !queryTask.IsFaulted && !queryTask.IsCanceled)
                {
                    QuerySnapshot querySnapshot = queryTask.Result;

                    DocumentSnapshot documentSnapshot = querySnapshot.Documents.FirstOrDefault();

                    if (documentSnapshot.Exists)
                    {
                        Debug.Log("I WAS HERE");
                        string npcName = documentSnapshot.GetValue<string>("NPC");
                        List<string> dialogList = documentSnapshot.GetValue<List<string>>("Massage");

                        if (dialogList != null && dialogList.Count > 0 && index < dialogList.Count)
                        {
                            NamaNPC.text = npcName;
                            dialogText.text = dialogList[index];

                            // Increment the index for the next dialog
                            index++;

                            // Create a local variable to capture the current index value
                            int currentIndex = index;

                            clickedButton.onClick.RemoveAllListeners();
                            clickedButton.onClick.AddListener(() => OnButtonContinueClicked(clickedButton, QuestName, Condition, currentIndex));
                        }
                        else
                        {
                            CloseDialog();
                        }
                    }
                    else
                    {
                        // Handle the case when no documents are found or documentSnapshot is null
                    }
                }
            });
    }

    private void ShowDialog(string npcName, string dialog)
    {
        Debug.Log("ShowingDialog");
        Coba.Invoke();
        NamaNPC.text = npcName;
        dialogText.text = dialog;
    }

    public void CloseDialog()
    {
        // ToggleScript();
        Next.SetActive(true);
        PreviousDialog.SetActive(false);
        dialogUI.SetActive(false);
        dialogText.text = "Selamat Datang Di RSUD Ponorogo, Ada Yang Bisa Dibantu ?";
    }

    public void DeleteSpecificInstantiatedButtons()
    {
        foreach (Button button in specificInstantiatedButtons)
        {
            Destroy(button.gameObject);
        }

        // Clear the list to remove references to the destroyed buttons
        specificInstantiatedButtons.Clear();
    }


}






[System.Serializable]
public class SaveUsersLogDatas
{
    public string username;

}
