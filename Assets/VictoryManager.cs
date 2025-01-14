using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;
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

    // Update is called once per frame
    public void NextLevel()
    {
        script.LoadIndexNumberPlus(-2);
    }
}
