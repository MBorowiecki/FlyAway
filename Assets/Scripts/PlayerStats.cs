using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerVars{
    [Header("Stats")]
    public static int currentPlane = 0;
}

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    static List<int> expToLvl = new List<int>();
    public PlanesManager planesManager;
    public GameManager manager;

    [Header("VarsCopied")]
    public int level = 0;
    public int playerExp = 0;
    public float money = 0;
    public float fuel = 0;
    public float fuelCapacity = 50;
    public List<int> ownedPlanes = new List<int>();
    //public List<int> publicOwnedPlanes;
    public string playerName = "";
    public int passengersWaiting = 0;
    public int maxPassengers = 30;
    public string lastDateTime;
    public string dateTime = System.DateTime.Now.ToString();

    void Awake(){
        //PlayerVars.publicOwnedPlanes = ownedPlanes;
        LoadPlayer();
    }

    void OnApplicationQuit() {
        lastDateTime = System.DateTime.Now.ToString();
        SaveSystem.SaveProgress(this);
    }

    void Start(){
        expToLvl.Clear();
        expToLvl.Add(0);
        expToLvl.Add(50);
        expToLvl.Add(100);
        expToLvl.Add(150);
        expToLvl.Add(200);
        expToLvl.Add(300);
        expToLvl.Add(450);

        GetLevel();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        System.DateTime _date = System.DateTime.Parse(dateTime);
    }

    void Update(){
        dateTime = System.DateTime.Now.ToString();
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
        //SaveSystem.SaveProgress(this);
        SaveSystem.SaveProgress(this);
    }

    public void LoadPlayer(){
        SaveData data = SaveSystem.LoadData();

        if(data != null){
            playerExp = data.exp;
            money = data.money;
            ownedPlanes.Clear();
            for(int i = 0; i <= data.planesArray.Length - 1; i++){
                ownedPlanes.Add(data.planesArray[i]);
            }
            fuel = data.fuel;
            playerName = data.playerName;
            fuelCapacity = data.fuelCapacity;
            passengersWaiting = data.passengersWaiting;
            lastDateTime = data.lastDateTime;
        }else{
            Debug.Log("Initializing new save.");
            money = 1000;
            playerExp = 0;
            fuelCapacity = 30;
            fuel = fuelCapacity;
            passengersWaiting = maxPassengers;
            ownedPlanes.Clear();
            ownedPlanes.Add(0);
            
            SavePlayer();
        }
    }

    public void ResetProgress(){
        SaveSystem.ResetProgress();
        LoadPlayer();
    }

    public void GetLevel(){
        for(int i = 0; i < expToLvl.Count - 1; i++){
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

        for(int i = 0; i < expToLvl.Count - 1; i++){
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

    public void BuyPlane(int planeNumber){
        RemoveMoney(planesManager.planes[planeNumber].price);
        ownedPlanes.Add(planeNumber);
        SavePlayer();
    }

    public void NextPlane(){
        if(PlayerVars.currentPlane + 1 <= ownedPlanes.Count - 1){
            PlayerVars.currentPlane += 1;
        }else{
            PlayerVars.currentPlane = 0;
        }

        manager.SetPlane();
    }
}
