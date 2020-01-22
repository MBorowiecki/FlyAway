using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightFinisher : MonoBehaviour
{
    public PlaneController planeController;

    void Start(){
        planeController = GameObject.FindObjectOfType<PlaneController>();
    }
    // Start is called before the first frame update
    public void FinishFlight(){
        if(planeController.planeRB.velocity.magnitude < 1f && planeController.planeRB.velocity.magnitude > -1f && planeController.altitude < 2f && planeController.altitude > -2f){
            float moneyEarned = planeController.distance / 100f * planeController.pricePerMeter * planeController.publicPassengers;
            int expEarned = (int) Mathf.Floor(planeController.distance / 1000 * planeController.publicPassengers);
            Debug.Log(moneyEarned);
            planeController.RemoveAllPassengers();
            planeController.playerStats.AddMoney(moneyEarned);
            planeController.playerStats.AddExp(expEarned);
            Debug.Log("Added money and exp and saved.");

            GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            Debug.Log("Found game manager.");
            manager.SetScene("Garage");
        }
    }
}
