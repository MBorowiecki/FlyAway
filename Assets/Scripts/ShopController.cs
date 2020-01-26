using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject openShopButton;
    public GameObject closeShopButton;
    public GameManager gameManager;
    public PlayerStats playerStats;
    public PlanesManager planesManager;
    public int currentPlane = 0;
    public List<GameObject> UItoDisable = new List<GameObject>();
    public List<GameObject> UItoEnable = new List<GameObject>();

    private bool shopOpen = false;

    [Header("UI")]
    public Text planeName;
    public Text planePrice;

    public void OpenShop(){
        for(int i = 0; i <= UItoDisable.Count - 1; i++){
            UItoDisable[i].SetActive(false);
        }

        for(int i = 0; i <= UItoEnable.Count - 1; i++){
            UItoEnable[i].SetActive(true);
        }
        shopOpen = true;
        openShopButton.SetActive(false);
        closeShopButton.SetActive(true);
        gameManager.DespawnCurrPlane();
        gameManager.SpawnPlane(planesManager.planes[currentPlane].plane);
        planeName.text = planesManager.planes[currentPlane].name;
        planePrice.text = planesManager.planes[currentPlane].price.ToString() + " $";
        planeName.gameObject.SetActive(true);
        planePrice.gameObject.SetActive(true);
    }

    public void CloseShop(){
        for(int i = 0; i <= UItoDisable.Count - 1; i++){
            UItoDisable[i].SetActive(true);
        }

        for(int i = 0; i <= UItoEnable.Count - 1; i++){
            UItoEnable[i].SetActive(false);
        }
        shopOpen = false;
        openShopButton.SetActive(true);
        closeShopButton.SetActive(false);
        gameManager.DespawnCurrPlane();
        gameManager.SetPlane();
        currentPlane = 0;
        planeName.gameObject.SetActive(false);
        planePrice.gameObject.SetActive(false);
    }

    public void NextPlane(){
        if(currentPlane + 1 <= planesManager.planes.Count - 1 && shopOpen){
            currentPlane += 1;
            gameManager.DespawnCurrPlane();
            gameManager.SpawnPlane(planesManager.planes[currentPlane].plane);
            planeName.text = planesManager.planes[currentPlane].name;
            planePrice.text = planesManager.planes[currentPlane].price.ToString() + " $";
        }else{
            currentPlane = 0;
            gameManager.DespawnCurrPlane();
            gameManager.SpawnPlane(planesManager.planes[currentPlane].plane);
            planeName.text = planesManager.planes[currentPlane].name;
            planePrice.text = planesManager.planes[currentPlane].price.ToString() + " $";
        }
    }

    public void BuyPlane(){
        playerStats.BuyPlane(currentPlane);
    }
}
