using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public UnityEvent Quest1;
    public UnityEvent Quest2;

    void OnTriggerEnter(Collider other)
    {
        Quest1.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        Quest2.Invoke();
    }

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
