using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameObject plane;
    public Transform startingPosition;
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    static GameObject currPlaneGO;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlane();

        if(currPlaneGO == null){
            cinemachine.Follow = startingPosition;
            cinemachine.LookAt = startingPosition;
        }
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

    public void SetPlane(GameObject planeGO){
        Destroy(currPlaneGO);
        plane = planeGO;
        SpawnPlane();
    }

    public string GetCurrentSceneName(){
        return SceneManager.GetActiveScene().name;
    }
}
