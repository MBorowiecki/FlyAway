﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengersManager : MonoBehaviour
{
    public PlaneController planeController;
    public PlayerStats playerStats;

    void Start(){
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
    }

    void Update(){
        planeController = GameObject.FindObjectOfType<PlaneController>();
    }

    // Update is called once per frame
    public void TransferPedsFromHangarToPlane(int count){
        if(planeController.publicPassengers + count <= planeController.maxPassengers && playerStats.passengersWaiting >= count){
            int addedPeds = planeController.AddPassengers(count);
            playerStats.RemovePassengers(addedPeds);
        }else{
            Debug.Log("Cannot add passengers to plane.");
        }
    }

    public void TransferPedsFromPlaneToHangar(int count){
        if(planeController.publicPassengers >= count && playerStats.passengersWaiting + count <= playerStats.maxPassengers){
            int removedPeds = planeController.RemovePassengers(count);
            playerStats.AddPassengers(removedPeds);
        }else{
            if(planeController.publicPassengers >= count)
                Debug.Log("Cannot remove passengers to hangar. Not enough passengers.");
            if(playerStats.passengersWaiting + count <= playerStats.maxPassengers)
                Debug.Log("Cannot remove passengers to hangar. Not enough room." + (playerStats.passengersWaiting + count) + playerStats.maxPassengers);
        }
    }
}