using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> enemies;
    public GameObject transitionInitiator;
    public LevelLoader script;

    public void Start()
    {
        script = transitionInitiator.GetComponent<LevelLoader>();
    }
    void Awake()
    {
        Instance = this;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            // transition to scene
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                script.LoadSpecificIndex(5);
            }
            else
            {
                script.LoadSpecificIndex(4);
            }
            // Load victory scene
            //SceneManager.LoadScene("VictoryScene");
        }
    }

    public void LoadDefeat()
    {
        script.LoadSpecificIndex(3);
        //SceneManager.LoadScene("GameOverScene");
    }
}
