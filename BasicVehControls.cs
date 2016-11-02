using UnityEngine;
using System.Collections;


public class BasicVehControls : MonoBehaviour
{

    public int bhp;
    public float torque;
    public int brakeTorque;

    public float[] gearRatio;
    public int currentGear;

    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider RL;
    public WheelCollider RR;

    public float currentSpeed;
    public int maxSpeed;
    public int maxRevSpeed;

    public float SteerAngle;

    public float engineRPM;
    public float gearUpRPM;
    public float gearDownRPM;
    private GameObject COM;

    public bool handBraked;

    void Start()
    {

        FL = GameObject.Find("FL.Col").GetComponent<WheelCollider>();
        FR = GameObject.Find("FR.Col").GetComponent<WheelCollider>();
        RL = GameObject.Find("RL.Col").GetComponent<WheelCollider>();
        RR = GameObject.Find("RR.Col").GetComponent<WheelCollider>();
		
        COM = GameObject.Find("Col");
        GetComponent<Rigidbody>().centerOfMass = new Vector3(COM.transform.localPosition.x * transform.localScale.x, COM.transform.localPosition.y * transform.localScale.y, COM.transform.localPosition.z * transform.localScale.z);
    }

    void Update()
    {

        Steer();
		AutoGears();

        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        //GetComponent<Rigidbody>().drag = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude / 250);
        engineRPM = (((RL.rpm + RR.rpm) / 2) * gearRatio[currentGear]);


        torque = bhp * gearRatio[currentGear];
        Accelerate();


        if (Input.GetButton("Jump"))
        {
            HandBrakes();
        }
		
		if (Input.GetKey(KeyCode.R)) {

			transform.position.Set(transform.position.x, transform.position.y + 5f, transform.position.z);
			transform.rotation.Set(0,0,0,0);
		}
    }


    void Accelerate()
    {

        if (currentSpeed < maxSpeed && currentSpeed > maxRevSpeed && engineRPM <= gearUpRPM)
        {

            RL.motorTorque = torque * Input.GetAxis("Vertical");
            RR.motorTorque = torque * Input.GetAxis("Vertical");
            RL.brakeTorque = 0;
            RR.brakeTorque = 0;
        }
        else
        {

            RL.motorTorque = 0;
            RR.motorTorque = 0;
            RL.brakeTorque = brakeTorque;
            RR.brakeTorque = brakeTorque;
        }

        if (engineRPM > 0 && Input.GetAxis("Vertical") < 0)
        {

            FL.brakeTorque = brakeTorque;
            FR.brakeTorque = brakeTorque;
        }
        else
        {
            FL.brakeTorque = 0;
            FR.brakeTorque = 0;
        }
    }

    void Steer()
    {

        if (currentSpeed < 100)
        {
            SteerAngle = 13 - (currentSpeed / 10);
        }
        else
        {
            SteerAngle = 2;
        }

        FL.steerAngle = SteerAngle * Input.GetAxis("Horizontal");
        FR.steerAngle = SteerAngle * Input.GetAxis("Horizontal");
    }


    void AutoGears()
    {

        int AppropriateGear = currentGear;

        if (engineRPM >= gearUpRPM)
        {

            for (var i = 0; i < gearRatio.Length; i++)
            {
                if (RL.rpm * gearRatio[i] < gearUpRPM)
                {
                    AppropriateGear = i;
                    break;
                }
            }
            currentGear = AppropriateGear;
        }

        if (engineRPM <= gearDownRPM)
        {
            AppropriateGear = currentGear;
            for (var j = gearRatio.Length - 1; j >= 0; j--)
            {
                if (RL.rpm * gearRatio[j] > gearDownRPM)
                {
                    AppropriateGear = j;
                    break;
                }
            }
            currentGear = AppropriateGear;
        }
    }

    void HandBrakes()
    {

        RL.brakeTorque = brakeTorque;
        RR.brakeTorque = brakeTorque;
        FL.brakeTorque = brakeTorque;
        FR.brakeTorque = brakeTorque;
    }
}
        
