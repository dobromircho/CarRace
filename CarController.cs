using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]

public class CarController : MonoBehaviour
{
    public float zGravity;
    public float yGravity;
   
    Image speedArrow;
    Image rpmArrow;
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
    public Transform FLWheel;
    public Transform FRWheel;
    public Transform RLWheel;
    public Transform RRWheel;
    public int stearAngle;
    public float[] GearRatio;
    public int CurrentGear;
    public float EngineTorque;
    public float MaxEngineRPM = 800f;
    public float MinEngineRPM = 200f;
    public int lowSpeedSteer=15;
    public int highSpeedSteer = 1;
    public float decSpeed = 5;
    public int minSpeed = 0;
    public int currSpeed;
    float topSpeed = 110;
    public float EngineRPM;
    private int AppropriateGear;
    float rotationValue = 0f;
    public Light[] frontLight;
    public Light[] backLight;
    public Material backStops;
    public bool stoped = false;
    public float maxBreakTorque;
    private float mySidewayFric;
    private float myForwardFric;
    private float slipSidewayFric;
    private float slipForwardFric;
    public int gear;
    public ParticleSystem carSmoke;
    float currSmokeSpeed;
    float currSmokeRate ;
    void Start()
    {
        rigidbody.centerOfMass += new Vector3(0f, -yGravity, zGravity);
        backLight[0].intensity = 0f;
        SetValues();
        currSmokeSpeed = carSmoke.startSpeed;
        currSmokeRate = carSmoke.emissionRate;
        speedArrow = GameObject.FindGameObjectWithTag("SpeedArrow").GetComponent<Image>();
        rpmArrow = GameObject.FindGameObjectWithTag("RpmArrow").GetComponent<Image>();
    }
    void SetValues()
    {
        myForwardFric = RearLeftWheel.forwardFriction.stiffness;
        mySidewayFric = RearLeftWheel.forwardFriction.stiffness;
        slipForwardFric = 0.1f;
        slipSidewayFric = 0.07f;
    }
    void  SetSlip(float currentForwardFric, float currentSidewayFric )
    {
        WheelFrictionCurve RR_W = RearRightWheel.forwardFriction;
        WheelFrictionCurve RL_W = RearLeftWheel.sidewaysFriction;

        RR_W.stiffness = currentForwardFric;
        RL_W.stiffness = currentSidewayFric;

        RearRightWheel.forwardFriction = RR_W;
        RearLeftWheel.forwardFriction = RR_W;
        RearRightWheel.sidewaysFriction = RL_W;
        RearLeftWheel.sidewaysFriction = RL_W;
        
    }
    void Update()
    {
        if (networkView.isMine)
        {
            EngineTorque = 40;
            backLight[0].intensity = 0f;
            backLight[0].color = Color.white;
            RLWheel.Rotate(RearLeftWheel.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
            RRWheel.Rotate(RearRightWheel.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
            FLWheel.transform.rotation = FrontLeftWheel.transform.rotation * Quaternion.Euler(rotationValue, FrontLeftWheel.steerAngle, 0f);
            FRWheel.transform.rotation = FrontRightWheel.transform.rotation * Quaternion.Euler(rotationValue, FrontRightWheel.steerAngle, 0f);
            RLWheel.transform.rotation = RearLeftWheel.transform.rotation * Quaternion.Euler(rotationValue, -FrontLeftWheel.steerAngle, 0f);
            RRWheel.transform.rotation = RearRightWheel.transform.rotation * Quaternion.Euler(rotationValue, -FrontRightWheel.steerAngle, 0f);
            rotationValue += FrontLeftWheel.rpm * (360 / 60) * Time.deltaTime;
            if (Input.GetAxis("Vertical") < 0)
            {
                backLight[0].intensity = 5f;

                backStops.color = Color.white;
            }
            if (stoped)
            {
                backLight[0].intensity = 8;
                backLight[0].color = Color.red;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                EngineTorque = 100f;
                 
            }
        }
       
    }  

    void FixedUpdate()
    {
        if (networkView.isMine)
        {
            currSpeed = (int)(2 * Mathf.PI * FrontLeftWheel.radius * FrontLeftWheel.rpm * 60) / 1000;
            var speedFactor = currSpeed/topSpeed; 
            stearAngle = (int)Mathf.Lerp(lowSpeedSteer, highSpeedSteer, speedFactor*2);

            if (Input.GetButton("Vertical") == false)
            {
                RearRightWheel.brakeTorque = decSpeed;
                RearLeftWheel.brakeTorque = decSpeed;
                FrontLeftWheel.brakeTorque = decSpeed;
                FrontRightWheel.brakeTorque = decSpeed;
            }
            else
            {
                RearRightWheel.brakeTorque = 0;
                RearLeftWheel.brakeTorque = 0;
            }
            EngineRPM = (RearLeftWheel.rpm + RearRightWheel.rpm) / 2 * GearRatio[CurrentGear];
            if (EngineRPM > 850f)
            {
                EngineRPM = 850f;
            }
            ShiftGears();
            audio.pitch = Mathf.Abs(currSpeed / topSpeed) + 0.5f;
            if (audio.pitch > 4.0)
            {
                audio.pitch = 4.1f;
            }
            if (currSpeed < topSpeed && currSpeed > -topSpeed + 40)
            {
                RearLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxisRaw("Vertical");
                RearRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxisRaw("Vertical");
                FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxisRaw("Vertical");
                FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxisRaw("Vertical");
            }
            else
            {
                RearLeftWheel.motorTorque = 0;
                RearRightWheel.motorTorque = 0;
                FrontLeftWheel.motorTorque = 0f;
                FrontRightWheel.motorTorque = 0;
            }

            FrontLeftWheel.steerAngle = stearAngle * Input.GetAxisRaw("Horizontal");
            FrontRightWheel.steerAngle = stearAngle * Input.GetAxisRaw("Horizontal");
            RearLeftWheel.steerAngle = -stearAngle * Input.GetAxisRaw("Horizontal");
            RearRightWheel.steerAngle = -stearAngle * Input.GetAxisRaw("Horizontal");
            HandBrake();

            float smokeFactor = Mathf.Abs(currSpeed / topSpeed);
            carSmoke.startSpeed = Mathf.Lerp(currSmokeSpeed, 4f, smokeFactor);
            carSmoke.emissionRate = Mathf.Lerp(currSmokeRate, 100f, smokeFactor);
            float rpmFactor = Mathf.Abs(currSpeed / topSpeed);
            Quaternion spArrRot = Quaternion.Euler(0f, 0f, Mathf.Lerp(90f, -90f, smokeFactor));
            Quaternion rpmArrRot = Quaternion.Euler(0f, 0f, Mathf.Lerp(-80f, 70f, rpmFactor));
            speedArrow.transform.rotation = spArrRot;
            rpmArrow.transform.rotation = rpmArrRot;
       }
        
        
    }

    void ShiftGears()
    {
        if (EngineRPM >= MaxEngineRPM)
        {
            AppropriateGear = CurrentGear;

            for (int i = 0; i < GearRatio.Length; i++)
            {
                if (FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM)
                {
                    AppropriateGear = i;
                    break;
                }
            }

            CurrentGear = AppropriateGear;
            gear = CurrentGear;
        }

        if (EngineRPM <= MinEngineRPM)
        {
            AppropriateGear = CurrentGear;

            for (int j = GearRatio.Length - 1; j >= 0; j--)
            {
                if (FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM)
                {
                    AppropriateGear = j;
                    break;
                }
            }

            CurrentGear = AppropriateGear;
            gear = CurrentGear;
        }
    }

    void HandBrake()
    {
        if (networkView.isMine)
        {


            if (Input.GetButton("Jump"))
            {
                stoped = true;
            }
            else
            {
                stoped = false;
            }
            if (stoped)
            {
                FrontLeftWheel.brakeTorque = maxBreakTorque;
                FrontRightWheel.brakeTorque = maxBreakTorque;
                SetSlip(slipForwardFric, slipSidewayFric);
            }

            else
            {
                SetSlip(myForwardFric, mySidewayFric);
                FrontLeftWheel.brakeTorque = 0f;
                FrontRightWheel.brakeTorque = 0f;
            }
        }
    }
}

 


