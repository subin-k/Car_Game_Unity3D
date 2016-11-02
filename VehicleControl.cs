using UnityEngine;
using System.Collections;


public class VehicleControl : MonoBehaviour
{

    public int bhp;
    public float torque;
    public int brakeTorque;

    public float[] gearRatio;
    public int currentGear;

    public bool ignP1 = false;
    public bool IgnP2 = false;

    public GameObject IgnBeep;
    public GameObject IgnMain;
    public GameObject idleSound;
    public GameObject turboSound;

    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider RL;
    public WheelCollider RR;

    public float currentSpeed;
    public int maxSpeed;
    public int maxRevSpeed;

    public float SteerAngle;

    public GameObject BrakeLgts;
    public GameObject RevLgts;

    public bool engineIsOn = false;
    public bool isManual = false;

    public float engineRPM;
    public float gearUpRPM;
    public float gearDownRPM;
    private GameObject COM;

    public bool handBraked;

	private Vector3 Exh1_Pos;
	private Vector2 Exh2_Pos;
	public AudioSource BacFire_Sound;
	public AudioSource skidAudio;

    void Start()
    {

        FL = GameObject.Find("WlFL.Col").GetComponent<WheelCollider>();
        FR = GameObject.Find("WlFR.Col").GetComponent<WheelCollider>();
        RL = GameObject.Find("WlRL.Col").GetComponent<WheelCollider>();
        RR = GameObject.Find("WlRR.Col").GetComponent<WheelCollider>();
		
		Exh1_Pos = GameObject.Find("Exh1_BF").transform.position;
		Exh2_Pos = GameObject.Find("Exh2_BF").transform.position;
		BacFire_Sound = GameObject.Find ("BackFire").GetComponent<AudioSource>();
		skidAudio = GameObject.Find("SkidAudio").GetComponent<AudioSource>();

        COM = GameObject.Find("COM");
        handBraked = false;

        BrakeLgts = GameObject.Find("BrakeLights");
        RevLgts = GameObject.Find("RevLights");
        BrakeLgts.SetActive(false);
        GetComponent<Rigidbody>().centerOfMass = new Vector3(COM.transform.localPosition.x * transform.localScale.x, COM.transform.localPosition.y * transform.localScale.y, COM.transform.localPosition.z * transform.localScale.z);
    }

    void Update()
    {

        Steer();

        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        //GetComponent<Rigidbody>().drag = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude / 250);
        engineRPM = (((RL.rpm + RR.rpm) / 2) * gearRatio[currentGear]);

        if (Input.GetKeyDown(KeyCode.I))
        {

            if (IgnP2 == false && ignP1 == false)
            {

                ignP1 = true;
                IgnBeep.GetComponent<AudioSource>().Play();
                torque = 0;
                BrakeLgts.SetActive(true);
            }

            else if (ignP1 == true && IgnP2 == false)
            {

                IgnP2 = true;
                IgnBeep.GetComponent<AudioSource>().Stop();
                IgnMain.GetComponent<AudioSource>().PlayOneShot(IgnMain.GetComponent<AudioSource>().clip);
                engineIsOn = true;
                BrakeLgts.SetActive(false);

            }

            else if (ignP1 == true && IgnP2 == true)
            {

                engineIsOn = false;
                ignP1 = false;
                IgnP2 = false;
            }

            else { engineIsOn = false; }
        }

        if (engineIsOn == true)
        {

            torque = bhp * gearRatio[currentGear];
            idleSound.GetComponent<AudioSource>().Play();
            Accelerate();
            BrakeLights();

        }
        else
        {
            torque = 0;
            idleSound.GetComponent<AudioSource>().Stop();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            isManual = true;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            isManual = false;
        }

        if (isManual == true)
        {
            ManualGears();
        }
        else
        {
            AutoGears();
        }

        if (Input.GetButton("Jump"))
        {
            HandBrakes();
        }

        if (engineRPM < 0 && currentSpeed > 5 && Input.GetAxis("Vertical") < 0)
        {
            RevLgts.SetActive(true);
        }
        else
        {
            RevLgts.SetActive(false);
        }

		WheelHit CorrespondingGroundHit;;
		RR.GetGroundHit(out CorrespondingGroundHit );
		if(Mathf.Abs(CorrespondingGroundHit.sidewaysSlip) > 10) {
			skidAudio.enabled = true;
		}else{
			skidAudio.enabled = false;
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

        turboSound.GetComponent<AudioSource>().pitch = Mathf.Abs(engineRPM / gearUpRPM) + .5f;
        if (turboSound.GetComponent<AudioSource>().pitch > 2.0f)
        {
            turboSound.GetComponent<AudioSource>().pitch = 2.0f;
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

    void BrakeLights()
    {

        if (currentSpeed == 0 && engineIsOn == true || engineRPM > 0 && Input.GetAxis("Vertical") < 0)
        {

            BrakeLgts.SetActive(true);
        }
        else
        {
            BrakeLgts.SetActive(false);
        }
    }

    void ManualGears()
    {

        if (currentGear >= 0 && currentGear < gearRatio.Length)
        {
            if (Input.GetKeyDown(KeyCode.A)) { 

				currentGear = currentGear + 1; 
			}
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                currentGear = currentGear - 1;
            }
        }

        if (currentGear == -1)
        {
            currentGear = 0;
        }

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
			BacFire_Sound.PlayOneShot(BacFire_Sound.clip);
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
        
