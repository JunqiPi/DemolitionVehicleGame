using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAnimation : MonoBehaviour
{
    public Animator CameraAnimator;
    public Coroutine Invoke()
    {
        Time.timeScale = 0f;
        CameraAnimator.GetComponentInChildren<Camera>().gameObject.SetActive(true);
        CameraAnimator.Play("Show");
        return StartCoroutine(WaitAnimation());
    }

    // Update is called once per frame
    IEnumerator WaitAnimation()
    {
        
        yield return null;
        yield return new WaitWhile(() => { 
            //Debug.Log()
            AnimatorStateInfo animatorStateInfo = CameraAnimator.GetCurrentAnimatorStateInfo(0);
            return animatorStateInfo.IsName("Show"); 
        });
        Time.timeScale = 1f;
        Destroy(gameObject);
        
    }
}
