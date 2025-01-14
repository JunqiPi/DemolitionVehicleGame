using Ln;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



public class Launcher : MonoBehaviour
{

    //IVehicle owner;
    public Transform launchPoint;
    public float speed;
    public Bullet bulletPrefab;
    private Transform bulletsTransform;
    private AudioSource audioSource;
    public AudioClip launchSE;
    public ParticleSystem launchParticle;
    public ParticleSystem muzzleFlash;


    public float cooldown;
    public float timer;
    void Start()
    {
        bulletsTransform = transform.Find("Bullets");
        bulletsTransform.SetParent(null);
        audioSource = GetComponent<AudioSource>();
        ObjectPoolManager.Instance.RegisterCreateFunc(RecyclableType.Bullet, () =>
        {
            Bullet bullet = RecyclableFactory.Create(bulletPrefab.gameObject) as Bullet;
            return bullet;
        });
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {

        //Test
        if (Input.GetMouseButtonDown(0))
        {
            Launch();
        }
        timer += Time.deltaTime;
    }
    public void Launch()
    {
        if (timer >= cooldown)
        {
            timer = 0;
            Bullet bullet = BulletGenerate();
            audioSource.clip = launchSE;
            audioSource.Play();
            launchParticle.Play();
            muzzleFlash?.Play();
            bullet.GetComponent<Rigidbody>().velocity = launchPoint.forward * speed;
            bullet.OnInitialize();
        }


    }
    private Bullet BulletGenerate()
    {
        Bullet bullet = ObjectPoolManager.Instance.PopAndInitializeObject(bulletPrefab.RecyclableType) as Bullet;

        bullet.transform.SetParent(bulletsTransform);
        bullet.transform.position = launchPoint.position;
        bullet.transform.LookAt(launchPoint.forward);
        return bullet;
    }
}
