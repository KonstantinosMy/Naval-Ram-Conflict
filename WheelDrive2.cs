using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// THE ACTUAL BRAIN OF SHIP 2

public class WheelDrive2 : MonoBehaviour
{
    #region "Variable Declaration for the car mechanism"
    [Tooltip("Maximum steering angle of the wheels")]
    public float maxAngle = 30f;
    [Tooltip("Maximum torque applied to the driving wheels")]
    public float maxTorque = 300f;
    [Tooltip("Maximum brake torque applied to the driving wheels")]
    public float brakeTorque = 30000f;
    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    [Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
    public DriveType driveType;
    #endregion

    //  ==================== DECLARATION =========================
    public Rigidbody rigidbodyShip;

    private WheelCollider[] m_Wheels;
    public Vector3 playerPos;
    private bool RamUsed = false;

    
    public Image b1;
    public Image b2;
    public Image b3;
    public GameObject crash;
    public GameObject ram;
    public Animator textAnim;
    public int HP = 3;
    public Text blueText;
    public float thrust = 900000f;
    public float thrust_back = 50000f;

    void Start()
    {

        #region "Initialisation for the car mechanism"
        m_Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];


            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
        #endregion
        // yes for some reason i didn't have to include anything in the start function, weird huh?
    }


    void Update()
    {

        // Checks HP of Player 2 every frame to update the heart count
        if (HP == 2)
        {
            b1.enabled = false;

        }
        else if (HP == 1)
        {
            b2.enabled = false;
        }
        //if HP drops below 0 game ends
        else if (HP == 0)
        {
            b3.enabled = false;
        }

        if (HP <= 0)
        {
            gameEnd();
        }
        // When space is pressed RAM starts
        if (Input.GetKeyDown("space") && RamUsed == false)
        {

            print("RAM!");
            StartCoroutine(CountdownScreen());
            Ram();


        }

        // Pre - written code for the wheel mechanics of the "car"
        #region "Wheel Mechanics"
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        float angle = maxAngle * Input.GetAxis("Horizontal2");
        float torque = maxTorque * Input.GetAxis("Vertical2");

        float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

        foreach (WheelCollider wheel in m_Wheels)
        {
            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;

            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }

            // Update visual wheels if any.
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "a0l" || wheel.name == "a1l" || wheel.name == "a2l")
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.position = p;
                    shapeTransform.rotation = q;
                }
            }
        }
        #endregion
    }

    //The coroutine timer for the RAM cooldown
    private IEnumerator CountdownScreen()
    {
        float duration = 8f;

        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            float rnormalizedTime = UnityEngine.Mathf.Round(normalizedTime * 10f);
            blueText.text = rnormalizedTime.ToString();
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        blueText.text = "READY";

    }
    // When the other player enters the specified collider zones (the 2 sides of the ship) then players takes damage, a crash sound plays, and the hit animation starts
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1")
        {
            //Plays hit text animation
            textAnim.SetBool("isHit", true);
            StartCoroutine(textRecharge());

            //Plays sound
            crash.SetActive(true);
            StartCoroutine(WaitForSecondCrash());
            //Loses HP
            HP -= 1;
            Debug.Log("Hit by Player 1, HP: " + HP);

        }

    }
    //Waits for the animation to play then returns bool back to false to have it ready for the next hit
    IEnumerator textRecharge()
    {


        yield return new WaitForSeconds(3);

        textAnim.SetBool("isHit", false);

    }
    //The RAM function
    void Ram()
    {
        //shows player that ram is ready
        ram.SetActive(true);
        StartCoroutine(WaitForSecondRam());


        Debug.Log("RAM START");

        //This little line of code adds a force of a designated value behind the ship 
        rigidbodyShip.AddForce(transform.forward * thrust);
        StartCoroutine(WaitASecond());

        Debug.Log("RAM END");

        // after usage ram goes back to cooldown
        RamUsed = true;
        Debug.Log("RAM NOT AVAILABLE");
        StartCoroutine(RechargeCoroutine());

    }

    // in case player 1 wins it plays the approporiate victory screen
    void gameEnd()
    {
        Debug.Log("Game Finished, HP: " + HP);
        SceneManager.LoadScene("EndSceneRed");
    }
    // The RAM cooldown coroutine
    IEnumerator RechargeCoroutine()
    {

        Debug.Log("Started Coroutine at timestamp : " + Time.time);


        yield return new WaitForSeconds(8);


        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        RamUsed = false;
        Debug.Log("RAM AVAILABLE");
    }

    //So i added this as just adding a force would make the ship go really fast and keep it's momentum which was
    //something i didn't want, i wanted a feel of quick fast charge forward for a moment, so after casting RAM
    // a second force is then applied from the opposite side to slow you down
    IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(.5f);
        rigidbodyShip.AddForce(-transform.forward * thrust_back);
    }

    //Just a timer for the crash sounds effect
    IEnumerator WaitForSecondCrash()
    {
        yield return new WaitForSeconds(3f);
        crash.SetActive(false);
    }

    //Another simple timer
    IEnumerator WaitForSecondRam()
    {
        yield return new WaitForSeconds(3f);
        ram.SetActive(false);
    }

}

