using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    [Header("Thrust")]
    public float throttle;
    public float thrust;
    public float maxThrust;
    public Transform thrustPosition;
    public float thrustIncreaseCoefficient = 1f;

    [Header("Fuel")]
    public float literPerThrust = 0.00008f;
    static float liters = 23;
    public float maxLiters = 23;

    [Header("Others")]
    public Transform COM;
    public float distance;
    public float altitude = 0f;
    public float destroyVel = 20f;
    public bool isDestroyed = false;
    public GameObject explosionEffect;
    private bool startedDestroing = false;
    private SpriteRenderer planeSprite;
    private Vector2 destroyPosition;
    public SpriteRenderer frontWheelSprite;
    public SpriteRenderer rearWheelSprite;
    public PlayerStats playerStats;
    public float pricePerMeter = .2f;

    public Rigidbody2D planeRB;
    private bool finishing = false;

    [Header("UI")]
    public Slider throttleSlider;
    public Slider fuelSlider;
    public Text planeSpeedText;
    public Text planeAltitude;
    public Text planeThrottle;

    [Header("Sound")]
    public float minPitch = .3f;
    public float maxPitch = 1f;
    public bool engineOff = false;

    private AudioSource audioSource;

    [Header("Passengers")]
    public int maxPassengers = 2;
    static int currentPassengers = 0;
    public int publicPassengers;

    // Start is called before the first frame update
    void Start()
    {
        planeRB = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        planeSprite = GetComponent<SpriteRenderer>();
        playerStats = GameObject.Find("GameManager").GetComponent<PlayerStats>();
        StartCoroutine(CountDistance());

        if(throttleSlider == null){
            throttleSlider = GameObject.Find("ThrottleSlider").GetComponent<Slider>();
        }

        if(fuelSlider == null){
            fuelSlider = GameObject.Find("FuelSlider").GetComponent<Slider>();
        }

        if(planeSpeedText == null){
            planeSpeedText = GameObject.Find("PlaneSpeedText").GetComponent<Text>();
        }

        if(planeAltitude == null){
            planeAltitude = GameObject.Find("AltitudeText").GetComponent<Text>();
        }

        if(planeThrottle == null){
            planeThrottle = GameObject.Find("ThrottleSliderText").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        publicPassengers = currentPassengers;
        planeRB.centerOfMass = COM.localPosition;

        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(manager.GetCurrentSceneName() != "Garage"){
            engineOff = false;
        }else{
            engineOff = true;
        }

        if(engineOff){
            audioSource.volume = 0f;
        }else{
            audioSource.volume = 1f;
        }

        /*if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle += .3f * Time.deltaTime;
            throttle = Mathf.Clamp(throttle, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= .3f * Time.deltaTime;
            throttle = Mathf.Clamp(throttle, 0f, 1f);
        }*/

        throttle = throttleSlider.value;
        throttle = Mathf.Clamp(throttle, 0f, 1f);

        altitude = 0f;
        int layerMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, Mathf.Infinity, layerMask);

        if(hit.point != null){
            altitude = transform.position.y - hit.point.y;
        }

        planeSpeedText.text = Mathf.Floor(Mathf.Abs(planeRB.velocity.magnitude * 3.6f)).ToString() + " km/h";
        planeAltitude.text = Mathf.Floor(altitude).ToString() + " m";
        planeThrottle.text = Mathf.Floor(throttle * 100f).ToString() + " %";
        if(liters > 0f){
            thrust = Mathf.Lerp(thrust, throttle * maxThrust, (thrust * Time.deltaTime * .000001f * thrustIncreaseCoefficient) + 0.0001f);
        }else{
            thrust = Mathf.Lerp(thrust, 0f, (thrust * Time.deltaTime * .000001f * thrustIncreaseCoefficient) + 0.0001f);
        }
        
        liters -= literPerThrust * thrust * Time.deltaTime;

        audioSource.pitch = Mathf.Clamp(Mathf.Lerp(audioSource.pitch, ((thrust / maxThrust) * (maxPitch - minPitch)) + minPitch, .1f), minPitch, maxPitch);

        if(liters >= 0f){
            fuelSlider.value = liters / maxLiters;
        }else{
            liters = 0f;
            fuelSlider.value = 0f;
        }

        if(isDestroyed){
            transform.position = destroyPosition;

            if(!startedDestroing)
                StartCoroutine(Explosion());
        }
    }

    public int AddPassengers(int count){
        if(currentPassengers + count <= maxPassengers){
            currentPassengers += count;
            return count;
        }else{
            int addedPeds = maxPassengers - currentPassengers;
            currentPassengers = maxPassengers;
            return addedPeds;
        }
    }

    public int RemovePassengers(int count){
        if(currentPassengers - count <= 0){
            currentPassengers -= count;
            return count;
        }else{
            Debug.Log("Cannot remove " + count + " passengers. There's not enough passengers.");
            int removedPeds = currentPassengers;
            currentPassengers = 0;
            return removedPeds;
        }
    }

    public void RemoveAllPassengers(){
        currentPassengers = 0;
    }

    void FixedUpdate()
    {
        planeRB.AddForceAtPosition(thrustPosition.transform.right * thrust * 10f, thrustPosition.position);
    }

    IEnumerator CountDistance()
    {
        while (true)
        {
            Vector3 planeLastPos = transform.position;
            yield return new WaitForSeconds(.001f);
            if(altitude > 2f || altitude < -2f){
                distance += (transform.position - planeLastPos).magnitude;
            }
        }
    }

    IEnumerator Explosion(){
        startedDestroing = true;
        destroyPosition = transform.position;
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        planeSprite.sprite = null;
        frontWheelSprite.sprite = null;
        rearWheelSprite.sprite = null;
        audioSource.volume = 0f;
        yield return new WaitForSeconds(4);
        Destroy(effect);
        Destroy(gameObject);
    }
}
