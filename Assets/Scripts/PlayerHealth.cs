using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 100; // Initial health value

    // Method to reduce player health
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Max(playerHealth, 0); // Ensure health doesn’t go below 0
    }

    void FixedUpdate()
    {
        // Check if health is zero in FixedUpdate
        if (playerHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        // Ensure the game is not paused when loading GameOverScene
        Time.timeScale = 1;
        SceneManager.LoadScene("GameOverScene"); // Replace with the exact name of your Game Over scene
    }
}
