using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingLift : MonoBehaviour
{
    public Transform forcePosition;
    public Rigidbody2D planeRB;

    [Header("Wing Stats")]
    public float wingArea;
    private float pressure;
    private float angleOfAttack;
    public float wingSpan;
    public bool controlableWing = false;
    public float maxRotation;
    private float drag;
    private float inducedLift;
    private float inducedDrag;
    private Vector2 dragDirection;
    public float angle;
    public float dragCoefficient = 1f;
    public float liftCoefficient = 1;

    [Header("UI")]
    public Slider pitchSlider;
    public Text pitch;

    // Start is called before the first frame update
    void Start()
    {
        if (pitchSlider == null)
        {
            pitchSlider = GameObject.Find("PitchSlider").GetComponent<Slider>();
        }

        if (pitch == null)
        {
            pitch = GameObject.Find("PitchSliderText").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        Vector2 localVelocity = transform.InverseTransformDirection(planeRB.velocity);
        angleOfAttack = Mathf.Atan2(-localVelocity.y, localVelocity.x);

        if (controlableWing && pitchSlider)
        {
            angle = Mathf.Clamp(Mathf.Atan2((transform.right * Mathf.Lerp(transform.rotation.z, maxRotation * pitchSlider.value, .05f)).x, localVelocity.x), -10, 10);

            transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Lerp(transform.rotation.z, maxRotation * pitchSlider.value, .02f));

            pitch.text = (Mathf.Floor(pitchSlider.value * 100f) / 100f).ToString();
        }

        float aspectRatio = (wingSpan * wingSpan) / wingArea;
        inducedLift = (angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI) / 2;
        inducedDrag = (inducedLift * inducedLift) / (aspectRatio);
        pressure = (planeRB.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea) / 2;

        dragDirection = -planeRB.velocity.normalized;
        Vector2 liftDirection = transform.up;

        planeRB.drag = (((0.021f + inducedDrag) * pressure) * dragCoefficient) / 500000f;

        planeRB.AddForceAtPosition(liftDirection * (inducedLift * pressure * liftCoefficient), forcePosition.position);
    }
}
