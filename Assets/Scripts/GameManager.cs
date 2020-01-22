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

        SpawnPlane();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlane(){
        if(startingPosition == null){
            startingPosition.position = new Vector2(0f, 0f);
        }

        currPlaneGO = Instantiate(plane, startingPosition.position, startingPosition.rotation);
        cinemachine.Follow = currPlaneGO.transform;
        cinemachine.LookAt = currPlaneGO.transform;
    }

    public void SetScene(string scene){
        SceneManager.LoadScene(scene);
    }

    public void SetPlane(){
        Destroy(currPlaneGO);
        plane = planesManager.planes[playerStats.ownedPlanes[playerStats.currentPlane]].plane;
        SpawnPlane();
    }

    public string GetCurrentSceneName(){
        return SceneManager.GetActiveScene().name;
    }
}
