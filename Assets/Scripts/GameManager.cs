using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameObject plane;
    public Transform startingPosition;
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public PlayerStats playerStats;
    public PlanesManager planesManager;
    static GameObject currPlaneGO;
    // Start is called before the first frame update
    void Start()
    {
        if(currPlaneGO == null){
            cinemachine.Follow = startingPosition;
            cinemachine.LookAt = startingPosition;
        }

        if(planesManager == null){
            planesManager = gameObject.GetComponent<PlanesManager>();
        }

        if(playerStats == null){
            playerStats = gameObject.GetComponent<PlayerStats>();
        }

        SetPlane();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlane(GameObject planeGO){
        if(startingPosition == null){
            startingPosition.position = new Vector2(0f, 0f);
        }

        currPlaneGO = Instantiate(planeGO, startingPosition.position, startingPosition.rotation);
        cinemachine.Follow = currPlaneGO.transform;
        cinemachine.LookAt = currPlaneGO.transform;
    }

    public void SetScene(string scene){
        playerStats.SavePlayer();
        SceneManager.LoadScene(scene);
    }

    public void SetPlane(){
        Destroy(currPlaneGO);
        plane = planesManager.planes[playerStats.ownedPlanes[PlayerVars.currentPlane]].plane;
        SpawnPlane(plane);
    }

    public string GetCurrentSceneName(){
        return SceneManager.GetActiveScene().name;
    }

    public void DespawnCurrPlane(){
        if(startingPosition == null){
            startingPosition.position = new Vector2(0f, 0f);
        }

        Destroy(currPlaneGO);
        currPlaneGO = null;
        cinemachine.LookAt = startingPosition;
        cinemachine.Follow = startingPosition;
    }
}
