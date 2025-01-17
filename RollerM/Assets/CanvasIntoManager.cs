using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasIntoManager : MonoBehaviour
{
    public GameObject pullcount;
    public GameObject CoinCount;
    public GameObject Title;
    public GameObject subtitle;

    public GameObject instruction;
    public bool gameStarted = false;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        // Check if the current scene is the second or third scene in the build
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1 || sceneIndex == 2) // Assuming scene indices start from 0
        {
            // Automatically start the game
            gameStarted = true;
            // Activate pullcount and CoinCount
            pullcount.SetActive(true);
            CoinCount.SetActive(true);
            // Disable Title and subtitle
            Title.SetActive(false);
            subtitle.SetActive(false);
            instruction.SetActive(false);
        }
        else
        {
            // If not second or third scene, deactivate pullcount and CoinCount
            pullcount.SetActive(false);
            CoinCount.SetActive(false);
        }

        print("Game hasn't started");
    }

    void Update()
    {
        // Check if the player clicks the screen
        if (playerMovement.started)
        {
            gameStarted = true;
            print("Game has started");
            instruction.SetActive(false);
            pullcount.SetActive(true);
            CoinCount.SetActive(true);
        }
        // Don't proceed with player movement until the game starts
    }
}
