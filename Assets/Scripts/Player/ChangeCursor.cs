using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    public MonoBehaviour scriptToToggle;
    private bool isScriptEnabled = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            ToggleScript();
        }
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
