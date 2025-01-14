using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlameThrowerAttackRadius : MonoBehaviour
{
    public delegate void PlayerEnteredEvent(Vehicle_Status Player);
    public delegate void PlayerExitedEvent(Vehicle_Status Player);

    public PlayerEnteredEvent OnPlayerEnter;
    public PlayerExitedEvent OnPlayerExit;

    private List<Vehicle_Status> VehiclesInRadius = new List<Vehicle_Status>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Vehicle_Status>(out Vehicle_Status player))
        {
            VehiclesInRadius.Add(player);
            OnPlayerEnter?.Invoke(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Vehicle_Status>(out Vehicle_Status player))
        {
            VehiclesInRadius.Remove(player);
            OnPlayerExit?.Invoke(player);
        }
    }

    private void OnDisable()
    {
        foreach (Vehicle_Status player in VehiclesInRadius)
        {
            OnPlayerExit?.Invoke(player);
        }
        VehiclesInRadius.Clear();
    }
}
