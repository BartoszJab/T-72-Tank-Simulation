using UnityEngine;

public class TankTrackController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isHandBrake;

    public Transform tankCenter;
    public float caterpillarOffset;
    public float wheelRadius = 1f; // radius of wheels
    public float suspensionOffset = 0.05f; // offset of the wheel from its starting position when not touching surface

    // public float trackTextureSpeed = 2.5f;
    // protected float leftTrackTextureOffset = 0.0f;
    // protected float rightTrackTextureOffset = 0.0f;

    // left side track and wheels
    [Header("LEFT TANK SIDE")]
    public GameObject leftTrack;
    public WheelCollider[] leftWheelColliders;
    public Transform[] leftTrackUpperWheels;
    public Transform[] leftTrackWheels;
    public Transform[] leftTrackBones;

    // right side track and wheels
    [Header("RIGHT TANK SIDE")]
    public GameObject rightTrack;
    public WheelCollider[] rightWheelColliders;
    public Transform[] rightTrackUpperWheels;
    public Transform[] rightTrackWheels;
    public Transform[] rightTrackBones;

    protected WheelInfo[] leftTrackWheelData;
    protected WheelInfo[] rightTrackWheelData;

    [Header("MOVEMENT")]
    float verticalMovement = 0f;
    float horizontalMovement = 0f;

    public float maxSpeed;
    public float standRotateTorque = 600.0f;
    public float maxBrake = 2000.0f;

    public float forwardTorque = 500.0f;
    public float rotateInMotionBrake = 50.0f;
    public float turnSpeed = 5.0f;
    public float standRotationSpeed = 15.0f;


    
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
    }

    // get user input
    private void Update() {
        verticalMovement = Input.GetAxis("Vertical");
        horizontalMovement = Input.GetAxis("Horizontal");

        isHandBrake = false;
        if (Input.GetKey(KeyCode.Space)) {
            isHandBrake = true;
        }
    }

    private void FixedUpdate() {
        UpdateWheels(verticalMovement, horizontalMovement);
    }

    public void UpdateWheels(float vertical, float horizontal) {

        // left wheels
        foreach (WheelInfo lwd in leftTrackWheelData) {  
            SetWheelsAndBones(lwd.wheelTransform, lwd.boneTransform, lwd.wheelCollider);

            // move left wheel colliders
            TankDrive(lwd.wheelCollider, vertical, horizontal);
        }

        // set rotation of upper left side wheels
        for (int i = 0; i < leftTrackUpperWheels.Length; i++) {
            leftTrackUpperWheels[i].rotation = GetColliderPositionAndRotation(leftTrackWheelData[0].wheelCollider).rotation;
        }

        // right wheels
        foreach (WheelInfo rwd in rightTrackWheelData) {
            SetWheelsAndBones(rwd.wheelTransform, rwd.boneTransform, rwd.wheelCollider);

            // move right wheel colliders
            TankDrive(rwd.wheelCollider, vertical, -horizontal);
        }

        // set rotation of upper right side wheels
        for (int i = 0; i < rightTrackUpperWheels.Length; i++) {
            rightTrackUpperWheels[i].rotation = GetColliderPositionAndRotation(rightTrackWheelData[0].wheelCollider).rotation;
        }

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
    private void SetWheelsAndBones(Transform wheel, Transform bone, WheelCollider collider) {
        PositionRotation posRot = GetColliderPositionAndRotation(collider);

        wheel.position = posRot.position;
        wheel.rotation = posRot.rotation;

        bone.position = posRot.position + transform.up * caterpillarOffset; 
    }

    private PositionRotation GetColliderPositionAndRotation(WheelCollider collider) {
        collider.GetWorldPose(out var pos, out var rot);

        return new PositionRotation(pos, rot);
    }

    private void TankDrive(WheelCollider wheelCollider, float vertical, float horizontal) {
        var localVelocity = transform.InverseTransformDirection(rb.velocity);

        WheelFrictionCurve fricitionCurve = wheelCollider.sidewaysFriction;
        // none movement keys are pressed
        if (vertical == 0 && horizontal == 0) {
            wheelCollider.brakeTorque = maxBrake;

        // forward/backward movement key is not pressed and the tank is not moving forward/backward significantly
        } else if (vertical == 0f && Mathf.Abs(localVelocity.z) < 0.5f) {
            wheelCollider.brakeTorque = 0f;
            wheelCollider.motorTorque = horizontal * standRotateTorque * standRotationSpeed;

            fricitionCurve.extremumSlip = 1.0f;

        // either forward/backward movement key is pressed or forward/backward and left/right keys are pressed simultaneously
        } else {
            fricitionCurve.extremumSlip = 0.85f; // reduces 'drift' during the tank turn
            wheelCollider.brakeTorque = 0f; 
            wheelCollider.motorTorque = vertical * forwardTorque;

            if (horizontal > 0) {
                wheelCollider.motorTorque = horizontal * forwardTorque * turnSpeed;
            }

            if (horizontal < 0) {
                wheelCollider.brakeTorque = rotateInMotionBrake;
                wheelCollider.motorTorque = horizontal * forwardTorque * turnSpeed;
            }

            if (wheelCollider.rpm > maxSpeed) wheelCollider.motorTorque = 0;

            if (isHandBrake) wheelCollider.brakeTorque = maxBrake;
        }

        wheelCollider.sidewaysFriction = fricitionCurve;
    }

}
