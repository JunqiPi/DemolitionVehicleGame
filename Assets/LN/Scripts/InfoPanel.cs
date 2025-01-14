using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public float nitroSliderSpeed = 0.2f;
    public Launcher launcher;
    public CarController carController;

    public Slider nitroSlider;
    public Image BulletIconFill;
    private float sliderValue;


    void Start()
    {
        sliderValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        NitroSliderUpdate();
        BulletIconUpdate();


    }
    private void NitroSliderUpdate()
    {
        if (carController != null)
        {
            if (carController.IsBoosting)
            {
                sliderValue += Time.deltaTime * nitroSliderSpeed;

            }
            else
            {
                sliderValue -= Time.deltaTime * nitroSliderSpeed;
            }
            sliderValue = Mathf.Clamp(sliderValue, 0, 1);
            nitroSlider.value = Mathf.Clamp(sliderValue, 0, 1);
        }

    }
    private void BulletIconUpdate()
    {
        if (launcher != null)
        {
            BulletIconFill.fillAmount = launcher.timer / launcher.cooldown;

        }
    }
}
