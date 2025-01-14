using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Upgrade : MonoBehaviour
{
    // Start is called before the first frame update

    // Upgrade vehicle's health
    // Upgrade Stats
    private const float healthUpgradeMultiplier = 1.2f;
    private const float speedUpgradeMultiplier = 1.15f;
    private const float damageResistanceUpgradeIncrement = 0.05f;
    private const int upgradeCost = 5;

    // Upgrade Levels
    public int healthUpgradeLevel { get; private set; } = 0;
    public int speedUpgradeLevel { get; private set; } = 0;
    public int resistanceUpgradeLevel { get; private set; } = 0;

    void Start(){
        
    }

    public void UpgradeHealth()
    {
        if (CanUpgrade())
        {
            healthUpgradeLevel++;
            Vehicle_Status.vehicle_Status.maxHealth *= healthUpgradeLevel;
            Vehicle_Status.vehicle_Status.upgradePoints -= upgradeCost;
            Debug.Log($"Upgraded Health to Level {healthUpgradeLevel}. New Max Health: {Vehicle_Status.vehicle_Status.maxHealth}");
        }
    }

    // Upgrade vehicle's speed
    public void UpgradeSpeed()
    {
        if (CanUpgrade())
        {
            speedUpgradeLevel++;
            Vehicle_Status.vehicle_Status.speed *= speedUpgradeMultiplier;
            Vehicle_Status.vehicle_Status.upgradePoints -= upgradeCost;
            Debug.Log($"Upgraded Speed to Level {speedUpgradeLevel}. New Speed: {Vehicle_Status.vehicle_Status.speed}");
        }
    }

    // Upgrade vehicle's resistance to damage
    public void UpgradeResistance()
    {
        if (CanUpgrade())
        {
            resistanceUpgradeLevel++;
            Vehicle_Status.vehicle_Status.damageResistance += damageResistanceUpgradeIncrement;
            Vehicle_Status.vehicle_Status.damageResistance = Mathf.Clamp(Vehicle_Status.vehicle_Status.damageResistance, 0, 1); // Cap at 100%
            Vehicle_Status.vehicle_Status.upgradePoints -= upgradeCost;
            Debug.Log($"Upgraded Damage Resistance to Level {resistanceUpgradeLevel}. New Resistance: {Vehicle_Status.vehicle_Status.damageResistance * 100}%");
        }
    }

    // Check if the player has enough upgrade points to upgrade
    private bool CanUpgrade()
    {
        if (Vehicle_Status.vehicle_Status.upgradePoints >= upgradeCost)
        {
            return true;
        }
        else
        {
            Debug.Log("Not enough upgrade points.");
            return false;
        }
    }

    // Add upgrade points
    public void AddUpgradePoints(int points)
    {
        Vehicle_Status.vehicle_Status.upgradePoints += points;
        Debug.Log($"Earned {points} upgrade points. Total points: {Vehicle_Status.vehicle_Status.upgradePoints}");
    }
}
