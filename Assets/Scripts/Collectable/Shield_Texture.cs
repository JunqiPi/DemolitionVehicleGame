using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Texture : MonoBehaviour
{
    // Start is called before the first frame update
    public bool big=true;
    public float changeSpeed=0.01f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(big){
            
            float temp=transform.localScale.z+changeSpeed;
            transform.localScale = new Vector3(temp,temp,temp);
            if(transform.localScale.x>=5) big=false;
        }else
        if(!big){
            float temp=transform.localScale.z-changeSpeed;
            transform.localScale = new Vector3(temp,temp,temp);
            if(transform.localScale.x<=3) big=true;
        }
    }
}
