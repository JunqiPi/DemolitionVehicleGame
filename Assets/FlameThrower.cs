using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FlameThrower : MonoBehaviour
{
    public ParticleSystem shootingSystem;
    public ParticleSystem onFireSystemPrefab;
    public FlameThrowerAttackRadius attackRadius;

    public int burningDPS = 5;
    public float burnDuration = 3f;

    private ObjectPool<ParticleSystem> onFirePool;
    private Dictionary<Vehicle_Status, ParticleSystem> vehicleParticleSystems = new();

    private void Awake()
    {
        onFirePool = new ObjectPool<ParticleSystem>(CreateOnFireSystem);
        attackRadius.OnPlayerEnter += StartDamagingPlayer;
        attackRadius.OnPlayerExit += StopDamagingPlayer;
    }

    private ParticleSystem CreateOnFireSystem()
    {
        return Instantiate(onFireSystemPrefab);
    }

    private void StartDamagingPlayer(Vehicle_Status player)
    {
        player.StartBurning(burningDPS);
        player.OnDeath += HandlePlayerDeath;
        ParticleSystem onFireSystem = onFirePool.Get();
        onFireSystem.transform.SetParent(player.transform, false);
        onFireSystem.transform.localPosition = Vector3.zero;
        ParticleSystem.MainModule main = onFireSystem.main;
        main.loop = true;
        vehicleParticleSystems.Add(player, onFireSystem);
    }

    private void HandlePlayerDeath(Vehicle_Status player)
    {
        player.OnDeath -= HandlePlayerDeath;
        if (vehicleParticleSystems.ContainsKey(player))
        {
            StartCoroutine(DelayedDisableBurn(player, vehicleParticleSystems[player], burnDuration));
            vehicleParticleSystems.Remove(player);
        }
    }

    private IEnumerator DelayedDisableBurn(Vehicle_Status player, ParticleSystem instance, float duration)
    {
        ParticleSystem.MainModule main = instance.main;
        main.loop = false;
        yield return new WaitForSeconds(duration);
        instance.gameObject.SetActive(false);
        player.StopBurning();
    }

    private void StopDamagingPlayer(Vehicle_Status player)
    {
        player.OnDeath -= HandlePlayerDeath;
        if (vehicleParticleSystems.ContainsKey(player))
        {
            StartCoroutine(DelayedDisableBurn(player, vehicleParticleSystems[player], burnDuration));
            vehicleParticleSystems.Remove(player);
        }
    }

    public void Shoot()
    {
        shootingSystem.gameObject.SetActive(true);
        attackRadius.gameObject.SetActive(true);
    }

    public void StopShooting()
    {
        attackRadius.gameObject.SetActive(false);
        shootingSystem.gameObject.SetActive(false);
    }
}
