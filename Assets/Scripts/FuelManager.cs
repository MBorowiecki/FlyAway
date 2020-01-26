using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlaneController planeController;
    public PlayerStats playerStats;
    public float fuelPrice = 5;

    void Start(){
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
    }

    void Update(){
        planeController = GameObject.FindObjectOfType<PlaneController>();
    }

    // Update is called once per frame
    public void TransferFuelFromHangarToPlane(int count){
        if(planeController.publicLiters + count <= planeController.maxLiters && playerStats.fuel >= count){
            float addedFuel = planeController.AddFuel(count);
            playerStats.RemoveFuel(addedFuel);
            playerStats.SavePlayer();
        }else{
            Debug.Log("Cannot add passengers to plane.");
        }
    }

    public void TransferFuelFromPlaneToHangar(int count){
        if(planeController.publicLiters >= count && playerStats.fuelCapacity + count <= playerStats.fuelCapacity){
            float removedFuel = planeController.RemoveFuel(count);
            playerStats.AddFuel(removedFuel);
            playerStats.SavePlayer();
        }else{
            if(planeController.publicLiters >= count)
                Debug.Log("Cannot remove passengers to hangar. Not enough passengers.");
            if(playerStats.fuel + count <= playerStats.fuelCapacity)
                Debug.Log("Cannot remove passengers to hangar. Not enough room." + (playerStats.fuel + count) + playerStats.fuelCapacity);
        }
    }

    public void BuyFuel(int count){
        if(playerStats.fuel + count <= playerStats.fuelCapacity){
            float price = fuelPrice * count;
            playerStats.RemoveMoney(price);
            playerStats.AddFuel(count);
            playerStats.SavePlayer();
        }
    }
}
