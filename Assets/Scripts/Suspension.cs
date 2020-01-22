using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Suspension : MonoBehaviour
{
    private Transform wheelTransform;
    public PlaneController planeController;
    public Transform wheelPoint;
    public Rigidbody2D planeRB;
    public Transform planeTransform;
    public float rideHeight;
    public float tireRadius;
    public float brakeForce;
    public bool brakingWheel = false;
    private float actualBrakeForce;
    public bool isBraking = true;
    public bool isGrounded;
    private float lastFramePlaneYVel;

    [Header("Spring")]
    public float minLength;
    public float springForce;
    public float springCompression;

    [Header("Damper")]
    public float lastLength;
    public float springLength;
    public float springVelocity;
    public float damperStiffness;
    public float damperForce;

    [Header("Graphics")]
    public Sprite wheelGraphics;
    public SpriteRenderer spriteRenderer;
    public Transform wheelGraphicsTransform;

    private bool changingBrakes = false;

    // Start is called before the first frame update
    void Start()
    {
        wheelTransform = GetComponent<Transform>();

        spriteRenderer.sprite = wheelGraphics;
        lastFramePlaneYVel = planeRB.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {
        Button brakesButton = GameObject.Find("BrakesButton").GetComponent<Button>();

        if(!changingBrakes){
            brakesButton.onClick.AddListener(delegate {StartCoroutine(ChangeBrakesStatus());});
        }
    }

    void FixedUpdate()
    {
        lastLength = springLength;

        RaycastHit2D hit = Physics2D.Raycast(wheelPoint.position, -wheelPoint.up, rideHeight);

        if (hit.collider != null)
        {
            springCompression = Vector2.Distance(wheelPoint.position + (-wheelPoint.up * minLength), hit.point) / rideHeight;
            springCompression = springCompression - 1;
            springCompression = Mathf.Abs(springCompression);
            springLength = Vector2.Distance(wheelPoint.position + (-wheelPoint.up * minLength), hit.point);
            wheelTransform.position = hit.point + (new Vector2(wheelPoint.up.x, wheelPoint.up.y) * tireRadius);
            isGrounded = true;
        }
        else
        {
            springCompression = 0f;
            springLength = rideHeight;
            wheelTransform.position = wheelPoint.position + (-wheelPoint.up * (rideHeight - tireRadius));
            isGrounded = false;
        }

        springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
        damperForce = damperStiffness * springVelocity;

        Vector3 localVel = transform.InverseTransformDirection(planeRB.velocity);

        planeRB.AddForceAtPosition((wheelPoint.up * springCompression * (springForce + damperForce)), wheelPoint.position);

        if (brakingWheel && isGrounded)
        {
            if (isBraking)
            {
                planeRB.AddForceAtPosition(planeRB.transform.right * Mathf.Lerp(actualBrakeForce, -planeRB.velocity.normalized.x * brakeForce, .1f), wheelGraphicsTransform.position);
            }
            else
            {
                actualBrakeForce = Mathf.Lerp(actualBrakeForce, 0f, .1f);
            }
        }

        float tireCircumference = 2 * Mathf.PI * tireRadius;
        float speedInMetersPerMinute = planeRB.velocity.magnitude * 6;
        float rotations = speedInMetersPerMinute / tireCircumference;

        if (isGrounded)
        {
            wheelTransform.Rotate(new Vector3(0f, 0f, rotations * -planeRB.velocity.normalized.x));
        }

        if(Mathf.Abs(lastFramePlaneYVel) > planeController.destroyVel && isGrounded)
        {
            planeController.isDestroyed = true;
        }

        lastFramePlaneYVel = planeRB.velocity.y;
    }

    IEnumerator ChangeBrakesStatus()
    {
        changingBrakes = true;
        isBraking = !isBraking;
        yield return new WaitForSeconds(.5f);
        changingBrakes = false;
    }
}
