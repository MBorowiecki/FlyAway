using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightFinisher : MonoBehaviour
{
    public PlaneController planeController;

    [Header("UI")]
    public GameObject finishFlightButton;
    public GameObject finishLevelUI;
    public Text distanceTravelledText;
    public Text passengersDeliveredText;
    public Text moneyText;
    public Text expText;
    public Button leaveFlightButton;

    void Start()
    {
        planeController = GameObject.FindObjectOfType<PlaneController>();

        if(leaveFlightButton){
            leaveFlightButton.onClick.AddListener(() => {
                GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
                manager.SetScene("Garage");
            });
        }
    }

    void Update() {
        if(finishFlightButton){
            if (IsPlaneAbleToFinish()){
                finishFlightButton.SetActive(true);
            }else{
                finishFlightButton.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    public void FinishFlight()
    {
        if (IsPlaneAbleToFinish())
        {
            float moneyEarned = planeController.distance / 100f * planeController.pricePerMeter * planeController.publicPassengers * planeController.pricePerPassenger;
            int expEarned = (int)Mathf.Floor(planeController.distance / 1000 * planeController.publicPassengers);
            if(finishLevelUI){
                finishLevelUI.SetActive(true);
            }

            if(distanceTravelledText){
                distanceTravelledText.text = "Distance travelled: " + Mathf.Round(planeController.distance).ToString() + " m";
            }

            if(passengersDeliveredText){
                passengersDeliveredText.text = "Passengers delivered: " + planeController.publicPassengers.ToString();
            }

            if(moneyText){
                moneyText.text = (Mathf.Round(moneyEarned * 100f) / 100f).ToString() + " $";
            }

            if(expText){
                expText.text = expEarned.ToString() + " EXP";
            }

            planeController.RemoveAllPassengers();
            planeController.playerStats.AddMoney(moneyEarned);
            planeController.playerStats.AddExp(expEarned);
        }
    }

    bool IsPlaneAbleToFinish() {
        return planeController.planeRB.velocity.magnitude < 1f && planeController.planeRB.velocity.magnitude > -1f && planeController.altitude < 5f && planeController.altitude > -2f;
    }
}
