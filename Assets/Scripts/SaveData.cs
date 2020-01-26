using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int exp;
    public float money;
    public int[] planesArray;
    public float fuel;
    public float fuelCapacity;
    public string playerName;
    public int passengersWaiting;
    public string lastDateTime;

    public SaveData(PlayerStats playerStats){
        exp = playerStats.playerExp;
        money = playerStats.money;
        fuel = playerStats.fuel;
        playerName = playerStats.playerName;
        fuelCapacity = playerStats.fuelCapacity;
        passengersWaiting = playerStats.passengersWaiting;
        lastDateTime = playerStats.dateTime;

        planesArray = new int[playerStats.ownedPlanes.Count];
        for(int i = 0; i <= planesArray.Length - 1; i++){
            planesArray[i] = playerStats.ownedPlanes[i];
        }
    }
}
