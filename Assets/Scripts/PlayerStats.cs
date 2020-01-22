using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int level = 0;
    public int playerExp = 0;
    public float money = 0;
    public float fuel = 0;
    public float fuelCapacity = 50;
    public int[] ownedPlanes;
    public string playerName = "";
    public int[] expToLvl;
    public int passengersWaiting = 0;
    public int maxPassengers = 30;

    private GameManager manager;

    void Start(){
        LoadPlayer();
        GetLevel();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(manager.GetCurrentSceneName() == "Garage"){
            Text moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
            Text expText = GameObject.Find("ExpText").GetComponent<Text>();
            Text fuelText = GameObject.Find("FuelText").GetComponent<Text>();
            Text playerNameText = GameObject.Find("PlayerNameText").GetComponent<Text>();
            Text passengersText = GameObject.Find("PassengersText").GetComponent<Text>();

            moneyText.text = "Money: " + (Mathf.Floor(money * 100f) / 100f).ToString() + " $";
            expText.text = "Exp: " + playerExp.ToString();
            fuelText.text = "Fuel: " + fuel.ToString() + " / " + fuelCapacity.ToString();
            playerNameText.text = playerName;
            passengersText.text = "Peds: " + passengersWaiting.ToString() + " / " + maxPassengers.ToString();
        }
    }

    public void SavePlayer(){
        SaveSystem.SaveProgress(this);
    }

    public void LoadPlayer(){
        SaveData data = SaveSystem.LoadData();

        playerExp = data.exp;
        money = data.money;
        ownedPlanes = data.ownedPlanes;
        fuel = data.fuel;
        playerName = data.playerName;
        fuelCapacity = data.fuelCapacity;
    }

    public void GetLevel(){
        for(int i = 0; i < expToLvl.Length - 1; i++){
            if(playerExp >= expToLvl[i] && playerExp < expToLvl[i + 1]){
                level = i;
            }
        }
    }

    public void RemoveFuel(float value){
        if(fuel - value >= 0){
            fuel -= value;
        }else{
            fuel = 0f;
        }

        SavePlayer();
    }

    public void AddFuel(float value){
        if(fuel + value <= fuelCapacity){
            fuel += value;
        }else{
            fuel = fuelCapacity;
        }
        
        SavePlayer();
    }

    public void AddMoney(float value){
        money += value;

        SavePlayer();
    }

    public void RemoveMoney(float value){
        if(money - value > 0f){
            money -= value;
        }else{
            money = 0f;
        }

        SavePlayer();
    }

    public void AddExp(int value){
        playerExp += value;

        for(int i = 0; i < expToLvl.Length - 1; i++){
            if(playerExp >= expToLvl[i] && playerExp < expToLvl[i + 1]){
                if(i > level){
                    level = i;
                }
            }
        }
        SavePlayer();
    }

    public void RemovePassengers(int value){
        if(passengersWaiting - value >= 0){
            passengersWaiting -= value;
        }else{
            passengersWaiting = 0;
        }
    }

    public void AddPassengers(int value){
        if(passengersWaiting + value <= maxPassengers){
            passengersWaiting += value;
        }else{
            passengersWaiting = maxPassengers;
        }
    }
}
