using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{

    public void RestartGame()
    {
        // Ensure time scale is normal and reload the main game scene
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); // Replace with your main game scene's name
    }

}
