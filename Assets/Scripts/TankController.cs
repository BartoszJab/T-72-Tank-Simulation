using UnityEngine;

public class TankController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isHandBrake;

    public Transform tankCenter;
    public float caterpillarOffset;
    public float wheelRadius = 1f; // radius of wheels

    public AudioSource tankMoveAudio;
    public ParticleSystem drivingSmoke;

    // left side track and wheels
    [Header("LEFT TANK SIDE")]
    public GameObject leftTrack;
    private Renderer leftTrackRenderer;
    public float leftTrackTextureSpeed;
    public WheelCollider[] leftWheelColliders;
    public Transform[] leftTrackUpperWheels;
    public Transform[] leftTrackWheels;
    public Transform[] leftTrackBones;
    public Vector3 leftWheelPosOffset;

    // right side track and wheels
    [Header("RIGHT TANK SIDE")]
    public GameObject rightTrack;
    private Renderer rightTrackRenderer;
    public float rightTrackTextureSpeed;
    public WheelCollider[] rightWheelColliders;
    public Transform[] rightTrackUpperWheels;
    public Transform[] rightTrackWheels;
    public Transform[] rightTrackBones;
    public Vector3 rightWheelPosOffset;

    protected WheelInfo[] leftTrackWheelData;
    protected WheelInfo[] rightTrackWheelData;

    [Header("MOVEMENT")]
    float verticalMovement = 0f;
    float horizontalMovement = 0f;

    public float maxSpeed = 85.0f;
    public float maxStandRotationSpeed = 25f;
    public float standRotateTorque = 600.0f;
    public float maxBrake = 2000.0f;

    public float forwardTorque = 500.0f;
    public float rotateInMotionBrake = 50.0f;
    public float turnSpeed = 5.0f;
    public float standRotationSpeed = 15.0f;


    private float topSpeed = 33;
    private float currentSpeed = 0;
    private float pitch = 1;

    private void Awake() {
        leftTrackWheelData = new WheelInfo[leftTrackWheels.Length];
        rightTrackWheelData = new WheelInfo[rightTrackWheels.Length];

        for (int i = 0; i < leftTrackWheelData.Length; i++) {
            leftTrackWheelData[i] = SetWheelsInfo(leftTrackWheels[i], leftTrackBones[i], leftWheelColliders[i]);
        }

        for (int i = 0; i < rightTrackWheelData.Length; i++) {
            rightTrackWheelData[i] = SetWheelsInfo(rightTrackWheels[i], rightTrackBones[i], rightWheelColliders[i]);
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = tankCenter.localPosition;

        leftTrackRenderer = leftTrack.GetComponent<Renderer>();
        rightTrackRenderer = rightTrack.GetComponent<Renderer>();

        SetTracksTextureSpeed(0, 0);
    }

    // get user input
    private void Update() {
        verticalMovement = Input.GetAxis("Vertical");
        horizontalMovement = Input.GetAxis("Horizontal");

        isHandBrake = false;
        if (Input.GetKey(KeyCode.Space)) {
            isHandBrake = true;
        }
        
        if (rb.velocity.magnitude > 1) {
            drivingSmoke.Play();
        } else {
            drivingSmoke.Stop();
        }

        SetDrivingAudio();
    }

    private void FixedUpdate() {
        UpdateWheels(verticalMovement, horizontalMovement);
    }

    public void UpdateWheels(float vertical, float horizontal) {

        // left wheels
        foreach (WheelInfo lwd in leftTrackWheelData) {  
            SetWheelsAndBones(lwd.wheelTransform, lwd.boneTransform, lwd.wheelCollider, leftWheelPosOffset);

            // move left wheel colliders
            TankDrive(lwd.wheelCollider, vertical, horizontal);
        }

        // set rotation of upper left side wheels
        for (int i = 0; i < leftTrackUpperWheels.Length; i++) {
            leftTrackUpperWheels[i].rotation = GetColliderPositionAndRotation(leftTrackWheelData[0].wheelCollider).rotation;
        }

        // move left track texture
        leftTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time * leftTrackTextureSpeed));

        // right wheels
        float rwdRpm = 0f;
        foreach (WheelInfo rwd in rightTrackWheelData) {
            SetWheelsAndBones(rwd.wheelTransform, rwd.boneTransform, rwd.wheelCollider, rightWheelPosOffset);

            // move right wheel colliders
            TankDrive(rwd.wheelCollider, vertical, -horizontal);
        }
        Debug.Log("Right side rpm: " + rwdRpm);
        // set rotation of upper right side wheels
        for (int i = 0; i < rightTrackUpperWheels.Length; i++) {
            rightTrackUpperWheels[i].rotation = GetColliderPositionAndRotation(rightTrackWheelData[0].wheelCollider).rotation;
        }

        // move right track texture
        rightTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, Time.time * rightTrackTextureSpeed));

        //if (Mathf.Round(horizontal) == 0 && Mathf.Round(vertical) == 0)
            
    }

    // set necessary data describing wheels
    WheelInfo SetWheelsInfo(Transform wheel, Transform bone, WheelCollider collider) {
        WheelInfo wheelDataResult = new WheelInfo();

        wheelDataResult.wheelTransform = wheel;
        wheelDataResult.boneTransform = bone;
        wheelDataResult.wheelCollider = collider;

        return wheelDataResult;
    }

    // set position and rotation for wheels as well as position for caterpillar bones respectively to the terrain the tank stays on
    private void SetWheelsAndBones(Transform wheel, Transform bone, WheelCollider collider, Vector3 wheelPosOffset) {
        PositionRotation posRot = GetColliderPositionAndRotation(collider);

        wheel.position = posRot.position;
        wheel.localPosition += wheelPosOffset;
        wheel.rotation = posRot.rotation;

        Vector3 bonePosition = new Vector3(posRot.position.x, posRot.position.y, bone.position.z);
        bone.position = posRot.position + transform.up * caterpillarOffset; 
    }

    private PositionRotation GetColliderPositionAndRotation(WheelCollider collider) {
        collider.GetWorldPose(out var pos, out var rot);

        return new PositionRotation(pos, rot);
    }

    private void TankDrive(WheelCollider wheelCollider, float vertical, float horizontal) {
        var localVelocity = transform.InverseTransformDirection(rb.velocity);

        WheelFrictionCurve sidewaysFricitionCurve = wheelCollider.sidewaysFriction;
        // none movement keys are pressed
        if (vertical == 0 && horizontal == 0) {
            wheelCollider.brakeTorque = maxBrake;
            SetTracksTextureSpeed(0f, 0f);
        // forward/backward movement key is not pressed and the tank is not moving forward/backward significantly
        } else if (vertical == 0f) {
            wheelCollider.brakeTorque = 0f;
            wheelCollider.motorTorque = horizontal * standRotateTorque * standRotationSpeed;
            if (Mathf.Abs(wheelCollider.rpm) > maxStandRotationSpeed) {
                wheelCollider.motorTorque = 0f;
            }

            if (horizontal < 0) {
                SetTracksTextureSpeed(0.3f, -0.3f);
            } else if (horizontal > 0) {
                SetTracksTextureSpeed(-0.3f, 0.3f);
            }

            sidewaysFricitionCurve.extremumSlip = 1.0f;

        // either forward/backward movement key is pressed or forward/backward and left/right keys are pressed simultaneously
        } else {
            sidewaysFricitionCurve.extremumSlip = 0.75f; // reduces 'drift' during the tank turn
            wheelCollider.brakeTorque = 0f; 
            wheelCollider.motorTorque = vertical * forwardTorque;
            

            if (vertical < 0)
                SetTracksTextureSpeed(0.5f, -0.5f);
            else
                SetTracksTextureSpeed(-0.5f, 0.5f);


            if (horizontal > 0) {
                wheelCollider.motorTorque = horizontal * forwardTorque * turnSpeed;
            }

            if (horizontal < 0) {
                //wheelCollider.brakeTorque = rotateInMotionBrake;
                wheelCollider.motorTorque = horizontal * forwardTorque * turnSpeed;
            }

            if (Mathf.Abs(wheelCollider.rpm) > maxSpeed) { wheelCollider.brakeTorque = maxBrake;}

            if (isHandBrake) wheelCollider.brakeTorque = maxBrake;


        }

        wheelCollider.sidewaysFriction = sidewaysFricitionCurve;
    }

    private void SetTracksTextureSpeed(float leftTrackTextureSpeed, float rightTrackTextureSpeed) {
        this.leftTrackTextureSpeed = leftTrackTextureSpeed;
        this.rightTrackTextureSpeed = rightTrackTextureSpeed;
    }

    private void SetDrivingAudio() {
        // change tank-idle audio pitch to simulate engine sound when driving
        currentSpeed = rb.velocity.magnitude * 3.6f;
        if (currentSpeed < topSpeed) {
            pitch = currentSpeed / topSpeed;
            tankMoveAudio.pitch = pitch + 1;

            if (tankMoveAudio.pitch > 1.75f) {
                tankMoveAudio.pitch = 1.75f;
            }
        }
    }

}
