using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Basic_Status_UI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public TMP_Text healthText; //Health text I created in main scene
    [SerializeField] public TMP_Text mphText;
    [SerializeField] public Slider healthBar; //Health Bar slider I created in main scene
    [SerializeField] public TMP_Text ShieldText;
    [SerializeField] public Slider shieldBar;

    public static Basic_Status_UI basic_Status_UI;
    void Start()
    {
        
    }
    private void Awake(){
        healthText=GetComponentInChildren<TMP_Text>();
        healthBar=GetComponentInChildren<Slider>();
        if(basic_Status_UI!=null){
            Destroy(gameObject);
        }else{
            basic_Status_UI=this;
        }
        DontDestroyOnLoad(basic_Status_UI);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateUI(float currentHealth, float maxHealth, int shieldPoint){
        healthText.text=Mathf.Ceil(currentHealth)+"/"+maxHealth;
        healthBar.value=currentHealth/maxHealth;
        ShieldText.text=Mathf.Ceil(shieldPoint)+"/"+50;
        shieldBar.value = shieldPoint/50;
    }

    public void mphUpdate(String mphText){
        this.mphText.text=mphText;
    }
}
