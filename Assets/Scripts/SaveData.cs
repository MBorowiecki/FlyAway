using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int exp;
    public float money;
    public List<int> ownedPlanes;
    public float fuel;
    public float fuelCapacity;
    public string playerName;
    public int passengersWaiting;

    public SaveData(PlayerStats playerStats){
        exp = playerStats.playerExp;
        money = playerStats.money;
        ownedPlanes = playerStats.ownedPlanes;
        fuel = playerStats.fuel;
        playerName = playerStats.playerName;
        fuelCapacity = playerStats.fuelCapacity;
        passengersWaiting = playerStats.passengersWaiting;
    }
}
