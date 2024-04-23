using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneNameToSwitchTo;

    private void Start()
    {
        // Ensure the cursor is visible when the scene starts
        Cursor.visible = true;
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneNameToSwitchTo);
    }

    // Add this method to unlock the cursor when the scene is loaded
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
