using System.Collections;
using UnityEngine;

public static class APRBody
{
    public static int Root = 0;
    public static int Body = 1;
    public static int Head = 2;
    public static int UpperRightArm = 3;
    public static int LowerRightArm = 4;
    public static int UpperLeftArm = 5;
    public static int LowerLeftArm = 6;

    public static int UpperRightLeg = 7;
    public static int LowerRightLeg = 8;
    public static int UpperLeftLeg = 9;
    public static int LowerLeftLeg = 10;
    public static int RightFoot = 11;
    public static int LeftFoot = 12;
}

public class APRController : MonoBehaviour
{
	public GameObject
        Root, Body, Head,
	    UpperRightArm, LowerRightArm,
	    UpperLeftArm, LowerLeftArm, 
	    UpperRightLeg, LowerRightLeg,
	    UpperLeftLeg, LowerLeftLeg,
	    RightFoot, LeftFoot;

    private ConfigurableJoint
        cfj_Root, cfj_Body, cfj_Head,
        cfj_UpperRightArm, cfj_LowerRightArm,
        cfj_UpperLeftArm, cfj_LowerLeftArm,
        cfj_UpperRightLeg, cfj_LowerRightLeg,
        cfj_UpperLeftLeg, cfj_LowerLeftLeg,
        cfj_RightFoot, cfj_LeftFoot;

    private Rigidbody
        rgbd_Root, rgbd_Body, rgbd_Head,
        rgbd_UpperRightArm, rgbd_LowerRightArm,
        rgbd_UpperLeftArm, rgbd_LowerLeftArm,
        rgbd_UpperRightLeg, rgbd_LowerRightLeg,
        rgbd_UpperLeftLeg, rgbd_LowerLeftLeg,
        rgbd_RightFoot, rgbd_LeftFoot;

    //Rigidbody Hands
    public Rigidbody RightHand, LeftHand;
    
    //Center of mass point
	public Transform COMP;
    
    [Header("Hand Dependancies")]
    //Hand Controller Scripts & dependancies
    public HandContact GrabRight;
    public HandContact GrabLeft;
    
    [Header("Input on this player")]
    //Enable controls
    public bool useControls = true;
    
    [Header("Player Input Axis")]
    //Player Axis controls
    public string forwardBackward = "Vertical";
    public string leftRight = "Horizontal";
    public string jump = "Jump";
    public string reachLeft = "Fire1";
    public string reachRight = "Fire2";
    
    [Header("Player Input KeyCodes")]
    //Player KeyCode controls
    public string punchLeft = "q";
    public string punchRight = "e";

    public bool CtrlHead = false;
    
    [Header("The Layer Only This Player Is On")]
    //Player layer name
    public string thisPlayerLayer = "Player_1";
    
    [Header("Movement Properties")]
    //Player properties
    public bool forwardIsCameraDirection = true;
    //Movement
    public float moveSpeed = 10f;
    public float turnSpeed = 6f;
    public float jumpForce = 18f;
    
    [Header("Balance Properties")]
    //Balance
    public bool autoGetUpWhenPossible = true;
    public bool useStepPrediction = true;
	public float balanceHeight = 2.5f;
    public float balanceStrength = 5000f;
    public float coreStrength = 1500f;
    public float limbStrength = 500f;
    //Walking
	public float StepDuration = 0.2f;
	public float StepHeight = 1.7f;
    public float FeetMountForce = 25f;
    
    [Header("Reach Properties")]
    //Reach
    public float reachSensitivity = 25f;
    public float armReachStiffness = 2000f;
    
    [Header("Actions")]
    //Punch
    public bool canBeKnockoutByImpact = true;
    public float requiredForceToBeKO = 20f;
    public bool canPunch = true;
    public float punchForce = 15f;
    
    [Header("Audio")]
    //Impact sounds
    public float ImpactForce = 10f;
    public AudioClip[] Impacts;
    public AudioClip[] Hits;
    public AudioSource SoundSource;
    
    
    //Hidden variables
    private float 
    timer, Step_R_timer, Step_L_timer,
    MouseYAxisArms, MouseXAxisArms, MouseYAxisBody;
    
	private bool 
    WalkForward, WalkBackward,
    StepRight, StepLeft, Alert_Leg_Right,
    Alert_Leg_Left, balanced = true, GettingUp,
    ResetPose, isRagdoll, isKeyDown, moveAxisUsed,
    jumpAxisUsed, reachLeftAxisUsed, reachRightAxisUsed;
    
    [HideInInspector]
    public bool 
    jumping, isJumping, inAir,
    punchingRight, punchingLeft;
    
    private Camera cam;
    private Vector3 Direction;
	private Vector3 CenterOfMassPoint;
    
    //Active Ragdoll Player Parts Array
	private GameObject[] APR_Parts;
    
    //Joint Drives on & off
	JointDrive
	//
	BalanceOn, PoseOn, CoreStiffness, ReachStiffness, DriveOff;
	
	//Original pose target rotation
	Quaternion
	//
	HeadTarget, BodyTarget,
	UpperRightArmTarget, LowerRightArmTarget,
	UpperLeftArmTarget, LowerLeftArmTarget,
	UpperRightLegTarget, LowerRightLegTarget,
	UpperLeftLegTarget, LowerLeftLegTarget;
    
	[Header("Player Editor Debug Mode")]
	//Debug
	public bool editorDebugMode;
    
    
    
    //-------------------------------------------------------------
    //--Calling Functions
    //-------------------------------------------------------------
    
    
    
    //---Setup---//
    //////////////
    void Awake()
	{
        PlayerSetup();
	}



    //---Updates---//
    ////////////////
    void Update()
    {
        if(useControls && !inAir)
        {
            //PlayerMovement();
            
            if(canPunch)
            {
                PlayerPunch();
            }
        }
        
        if(useControls)
        {
            PlayerReach();
        }
        
        if(balanced && useStepPrediction)
        {
            StepPrediction();
            CenterOfMass();
        }
        
        if(!useStepPrediction)
        {
            ResetWalkCycle();
        }
        
        GroundCheck();
        CenterOfMass();
    }
    
    
    
    //---Fixed Updates---//
    //////////////////////
    void FixedUpdate()
    {
        if (useControls && !inAir)
        {
            PlayerMovement();
        }

        Walking();
        
        if(useControls)
        {
            PlayerRotation();
            ResetPlayerPose();
            
            PlayerGetUpJumping();
        }
    }



    //-------------------------------------------------------------
    //--Functions
    //-------------------------------------------------------------


    
    //---Player Setup--//
    ////////////////////
    void PlayerSetup()
    {
        cam = Camera.main;
        
		//Setup joint drives
		BalanceOn = new JointDrive();
        BalanceOn.positionSpring = balanceStrength;
        BalanceOn.positionDamper = 0;
        BalanceOn.maximumForce = Mathf.Infinity;
        
        PoseOn = new JointDrive();
        PoseOn.positionSpring = limbStrength;
        PoseOn.positionDamper = 0;
        PoseOn.maximumForce = Mathf.Infinity;
        
        CoreStiffness = new JointDrive();
        CoreStiffness.positionSpring = coreStrength;
        CoreStiffness.positionDamper = 0;
        CoreStiffness.maximumForce = Mathf.Infinity;
        
        ReachStiffness = new JointDrive();
        ReachStiffness.positionSpring = armReachStiffness;
        ReachStiffness.positionDamper = 0;
        ReachStiffness.maximumForce = Mathf.Infinity;
		
		DriveOff = new JointDrive();
        DriveOff.positionSpring = 25;
        DriveOff.positionDamper = 0;
        DriveOff.maximumForce = Mathf.Infinity;
		
		//Setup/reroute active ragdoll parts to array
		APR_Parts = new GameObject[]
		{
			//array index numbers
			
			//0
			Root,
			//1
			Body,
			//2
			Head,
			//3
			UpperRightArm,
			//4
			LowerRightArm,
			//5
			UpperLeftArm,
			//6
			LowerLeftArm,
			//7
			UpperRightLeg,
			//8
			LowerRightLeg,
			//9
			UpperLeftLeg,
			//10
			LowerLeftLeg,
			//11
			RightFoot,
			//12
			LeftFoot
		};
        InitCache();
        //Setup original pose for joint drives
        BodyTarget = cfj_Body.targetRotation;
		HeadTarget = cfj_Head.targetRotation;
		UpperRightArmTarget = cfj_UpperRightArm.targetRotation;
        LowerRightArmTarget = cfj_LowerRightArm.targetRotation;
        UpperLeftArmTarget = cfj_UpperLeftArm.targetRotation;
        LowerLeftArmTarget = cfj_LowerLeftArm.targetRotation;
		UpperRightLegTarget = cfj_UpperRightLeg.targetRotation;
        LowerRightLegTarget = cfj_LowerRightLeg.targetRotation;
        UpperLeftLegTarget = cfj_UpperLeftLeg.targetRotation;
        LowerLeftLegTarget = cfj_LowerLeftLeg.targetRotation;
    }

    void InitCache()
    {
        cfj_Root = APR_Parts[0].GetComponent<ConfigurableJoint>();
        cfj_Body = APR_Parts[1].GetComponent<ConfigurableJoint>();
        cfj_Head = APR_Parts[2].GetComponent<ConfigurableJoint>();
        cfj_UpperRightArm = APR_Parts[3].GetComponent<ConfigurableJoint>();
        cfj_LowerRightArm = APR_Parts[4].GetComponent<ConfigurableJoint>();
        cfj_UpperLeftArm = APR_Parts[5].GetComponent<ConfigurableJoint>();
        cfj_LowerLeftArm = APR_Parts[6].GetComponent<ConfigurableJoint>();
        cfj_UpperRightLeg = APR_Parts[7].GetComponent<ConfigurableJoint>();
        cfj_LowerRightLeg = APR_Parts[8].GetComponent<ConfigurableJoint>();
        cfj_UpperLeftLeg = APR_Parts[9].GetComponent<ConfigurableJoint>();
        cfj_LowerLeftLeg = APR_Parts[10].GetComponent<ConfigurableJoint>();
        cfj_RightFoot = APR_Parts[11].GetComponent<ConfigurableJoint>();
        cfj_LeftFoot = APR_Parts[12].GetComponent<ConfigurableJoint>();

        rgbd_Root = APR_Parts[0].GetComponent<Rigidbody>();
        rgbd_Body = APR_Parts[1].GetComponent<Rigidbody>();
        rgbd_Head = APR_Parts[2].GetComponent<Rigidbody>();
        rgbd_UpperRightArm = APR_Parts[3].GetComponent<Rigidbody>();
        rgbd_LowerRightArm = APR_Parts[4].GetComponent<Rigidbody>();
        rgbd_UpperLeftArm = APR_Parts[5].GetComponent<Rigidbody>();
        rgbd_LowerLeftArm = APR_Parts[6].GetComponent<Rigidbody>();
        rgbd_UpperRightLeg = APR_Parts[7].GetComponent<Rigidbody>();
        rgbd_LowerRightLeg = APR_Parts[8].GetComponent<Rigidbody>();
        rgbd_UpperLeftLeg = APR_Parts[9].GetComponent<Rigidbody>();
        rgbd_LowerLeftLeg = APR_Parts[10].GetComponent<Rigidbody>();
        rgbd_RightFoot = APR_Parts[11].GetComponent<Rigidbody>();
        rgbd_LeftFoot = APR_Parts[12].GetComponent<Rigidbody>();
    }



    //---Ground Check---//
    /////////////////////
    void GroundCheck()
	{
		Ray ray = new Ray (APR_Parts[0].transform.position, -APR_Parts[0].transform.up);
		RaycastHit hit;
		
		//Balance when ground is detected
        if (Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")) && !inAir && !isJumping && !reachRightAxisUsed && !reachLeftAxisUsed)
        {
            if(!balanced && rgbd_Root.velocity.magnitude < 1f)
            {
                if(autoGetUpWhenPossible)
                {
                    balanced = true;
                }
            }
		}
		
		//Fall over when ground is not detected
		else if(!Physics.Raycast(ray, out hit, balanceHeight, 1 << LayerMask.NameToLayer("Ground")))
		{
            if(balanced)
            {
                balanced = false;
            }
		}

		
		//Balance on/off
		if(balanced && isRagdoll)
		{
            DeactivateRagdoll();
		}
		else if(!balanced && !isRagdoll)
		{
            ActivateRagdoll();
		}
	}
    
    
    
	//---Step Prediction---//
	////////////////////////
	void StepPrediction()
	{
		//Reset variables when balanced
		if(!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
		
		//Check direction to walk when off balance
        //Backwards
		if (COMP.position.z < APR_Parts[11].transform.position.z && COMP.position.z < APR_Parts[12].transform.position.z)
        {
            WalkBackward = true;
        }
        else
        {
			if(!isKeyDown)
			{
				WalkBackward = false;
			}
        }
        
        //Forward
        if (COMP.position.z > APR_Parts[11].transform.position.z && COMP.position.z > APR_Parts[12].transform.position.z)
        {
            WalkForward = true;
        }
        else
        {
            if(!isKeyDown)
			{
				WalkForward = false;
			}
        }
	}
    
    
    
    //---Reset Walk Cycle---//
    /////////////////////////
    void ResetWalkCycle()
    {
        //Reset variables when not moving
        if(!WalkForward && !WalkBackward)
        {
            StepRight = false;
            StepLeft = false;
            Step_R_timer = 0;
            Step_L_timer = 0;
            Alert_Leg_Right = false;
            Alert_Leg_Left = false;
        }
    }
    
    
    
    //---Player Movement---//
    ////////////////////////
    void PlayerMovement()
    {
        //Move in camera direction
        if(forwardIsCameraDirection)
        {
            Direction = APR_Parts[0].transform.rotation * new Vector3(Input.GetAxisRaw(leftRight), 0.0f, Input.GetAxisRaw(forwardBackward));
            Direction.y = 0f;
            rgbd_Root.velocity = Vector3.Lerp(rgbd_Root.velocity, (Direction * Time.fixedDeltaTime * 70 * moveSpeed) + new Vector3(0, rgbd_Root.velocity.y, 0), 0.8f);

            if(Input.GetAxisRaw(leftRight) != 0 || Input.GetAxisRaw(forwardBackward) != 0 && balanced)
            {
                if(!WalkForward && !moveAxisUsed)
                {
                    WalkForward = true;
                    moveAxisUsed = true;
                    isKeyDown = true;
                }
            }

            else if(Input.GetAxisRaw(leftRight) == 0 && Input.GetAxisRaw(forwardBackward) == 0)
            {
                if(WalkForward && moveAxisUsed)
                {
                    WalkForward = false;
                    moveAxisUsed = false;
                    isKeyDown = false;
                }
            }
        }
        
        //Move in own direction
        else
        {
            if (Input.GetAxisRaw(forwardBackward) != 0)
            {
                var v3 = rgbd_Root.transform.forward * (Input.GetAxisRaw(forwardBackward) * moveSpeed);
                v3.y = rgbd_Root.velocity.y;
                rgbd_Root.velocity = v3;
            }

            
            if(Input.GetAxisRaw(forwardBackward) > 0)
            {
                if(!WalkForward && !moveAxisUsed)
                {
                    WalkBackward = false;
                    WalkForward = true;
                    moveAxisUsed = true;
                    isKeyDown = true;
                    
                    if(isRagdoll)
                    {
                        cfj_UpperRightLeg.angularXDrive = PoseOn;
                        cfj_UpperRightLeg.angularYZDrive = PoseOn;
                        cfj_LowerRightLeg.angularXDrive = PoseOn;
                        cfj_LowerRightLeg.angularYZDrive = PoseOn;
                        cfj_UpperLeftLeg.angularXDrive = PoseOn;
                        cfj_UpperLeftLeg.angularYZDrive = PoseOn;
                        cfj_LowerLeftLeg.angularXDrive = PoseOn;
                        cfj_LowerLeftLeg.angularYZDrive = PoseOn;
                        cfj_RightFoot.angularXDrive = PoseOn;
                        cfj_RightFoot.angularYZDrive = PoseOn;
                        cfj_LeftFoot.angularXDrive = PoseOn;
                        cfj_LeftFoot.angularYZDrive = PoseOn;
                    }
                }
            }
            
            else if(Input.GetAxisRaw(forwardBackward) < 0)
            {
                if(!WalkBackward && !moveAxisUsed)
                {
                    WalkForward = false;
                    WalkBackward = true;
                    moveAxisUsed = true;
                    isKeyDown = true;
                    
                    if(isRagdoll)
                    {
                        cfj_UpperRightLeg.angularXDrive = PoseOn;
                        cfj_UpperRightLeg.angularYZDrive = PoseOn;
                        cfj_LowerRightLeg.angularXDrive = PoseOn;
                        cfj_LowerRightLeg.angularYZDrive = PoseOn;
                        cfj_UpperLeftLeg.angularXDrive = PoseOn;
                        cfj_UpperLeftLeg.angularYZDrive = PoseOn;
                        cfj_LowerLeftLeg.angularXDrive = PoseOn;
                        cfj_LowerLeftLeg.angularYZDrive = PoseOn;
                        cfj_RightFoot.angularXDrive = PoseOn;
                        cfj_RightFoot.angularYZDrive = PoseOn;
                        cfj_LeftFoot.angularXDrive = PoseOn;
                        cfj_LeftFoot.angularYZDrive = PoseOn;
                    }
                }
            }

            else if(Input.GetAxisRaw(forwardBackward) == 0)
            {
                if(WalkForward || WalkBackward && moveAxisUsed)
                {
                    WalkForward = false;
                    WalkBackward = false;
                    moveAxisUsed = false;
                    isKeyDown = false;
                    
                    if(isRagdoll)
                    {
                        cfj_UpperRightLeg.angularXDrive = DriveOff;
                        cfj_UpperRightLeg.angularYZDrive = DriveOff;
                        cfj_LowerRightLeg.angularXDrive = DriveOff;
                        cfj_LowerRightLeg.angularYZDrive = DriveOff;
                        cfj_UpperLeftLeg.angularXDrive = DriveOff;
                        cfj_UpperLeftLeg.angularYZDrive = DriveOff;
                        cfj_LowerLeftLeg.angularXDrive = DriveOff;
                        cfj_LowerLeftLeg.angularYZDrive = DriveOff;
                        cfj_RightFoot.angularXDrive = DriveOff;
                        cfj_RightFoot.angularYZDrive = DriveOff;
                        cfj_LeftFoot.angularXDrive = DriveOff;
                        cfj_LeftFoot.angularYZDrive = DriveOff;
                    }
                }
            }
        }
    }
    
    
    
    //---Player Rotation---//
    ////////////////////////
    void PlayerRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            forwardIsCameraDirection = !forwardIsCameraDirection;
        }

        if(forwardIsCameraDirection)
        {
            //Camera Direction
            //Turn with camera
            var lookPos = cam.transform.forward;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            cfj_Root.targetRotation = Quaternion.Slerp(cfj_Root.targetRotation, Quaternion.Inverse(rotation), Time.fixedDeltaTime * turnSpeed);
        }
        
        else
        {   
            //Self Direction
            //Turn with keys
            if (Input.GetAxisRaw(leftRight) != 0)
            {
                cfj_Root.targetRotation = Quaternion.Lerp(cfj_Root.targetRotation, new Quaternion(cfj_Root.targetRotation.x,cfj_Root.targetRotation.y - (Input.GetAxisRaw(leftRight) * turnSpeed), cfj_Root.targetRotation.z, cfj_Root.targetRotation.w), 6 * Time.fixedDeltaTime);
            }
            
            //reset turn upon target rotation limit
            if(cfj_Root.targetRotation.y < -0.98f)
            {
                cfj_Root.targetRotation = new Quaternion(cfj_Root.targetRotation.x, 0.98f, cfj_Root.targetRotation.z, cfj_Root.targetRotation.w);
            }

            else if(cfj_Root.targetRotation.y > 0.98f)
            {
                cfj_Root.targetRotation = new Quaternion(cfj_Root.targetRotation.x, -0.98f, cfj_Root.targetRotation.z, cfj_Root.targetRotation.w);
            }
        }
    }
    
    
    
    //---Player GetUp & Jumping---//
    ///////////////////////////////
    void PlayerGetUpJumping()
	{
        if(Input.GetAxis(jump) > 0)
        {
            if(!jumpAxisUsed)
            {
                if(balanced && !inAir)
                {
                    jumping = true;
                }
                
                else if(!balanced)
                {
                    DeactivateRagdoll();
                }
            }

            jumpAxisUsed = true;
        }
        
        else
        {
            jumpAxisUsed = false;
        }
        
        
		if (jumping)
        {
            isJumping = true;
                
            var v3 = rgbd_Root.transform.up * jumpForce;
            v3.x = rgbd_Root.velocity.x;
            v3.z = rgbd_Root.velocity.z;
            rgbd_Root.velocity = v3;
        }

		if (isJumping)
		{
			timer = timer + Time.fixedDeltaTime;
				
			if (timer > 0.2f)
			{
				timer = 0.0f;
				jumping = false;
				isJumping = false;
                inAir = true;
			}
		}
	}
    
    
    
    //---Player Landed---//
    //////////////////////
    public void PlayerLanded()
    {
        if(inAir && !isJumping && !jumping)
        {
            inAir = false;
            ResetPose = true;
        }
    }
    
    
    
    //---Player Reach--//
    ////////////////////
    void PlayerReach()
    {
        //Body Bending
        //return;
        if(CtrlHead)
        {
            if(MouseYAxisBody <= 0.9f && MouseYAxisBody >= -0.9f)
            {
                MouseYAxisBody = MouseYAxisBody + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            
            else if(MouseYAxisBody > 0.9f)
            {
                MouseYAxisBody = 0.9f;
            }
            
            else if(MouseYAxisBody < -0.9f)
            {
                MouseYAxisBody = -0.9f;
            }
            
            cfj_Body.targetRotation = new Quaternion(MouseYAxisBody, 0, 0, 1);
        }  
            
            
        //Reach Left
        if(Input.GetAxisRaw(reachLeft) != 0 && !punchingLeft)
        {
            
            if(!reachLeftAxisUsed)
            {
                //Adjust Left Arm joint strength
                cfj_UpperLeftArm.angularXDrive = ReachStiffness;
                cfj_UpperLeftArm.angularYZDrive = ReachStiffness;
                cfj_LowerLeftArm.angularXDrive = ReachStiffness;
                cfj_LowerLeftArm.angularYZDrive = ReachStiffness;
                    
                //Adjust body joint strength
                cfj_Body.angularXDrive = CoreStiffness;
                cfj_Body.angularYZDrive = CoreStiffness;
                
                reachLeftAxisUsed = true;
            }
            
            if(MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            
            else if(MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            
            else if(MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }
            
            //upper  left arm pose
			 cfj_UpperLeftArm.targetRotation = new Quaternion( -0.58f - (MouseYAxisArms), -0.88f - (MouseYAxisArms), -0.8f, 1);
        }
        
        if(Input.GetAxisRaw(reachLeft) == 0 && !punchingLeft)
        {
            if(reachLeftAxisUsed)
            {
                if(balanced)
                {
                    cfj_UpperLeftArm.angularXDrive = PoseOn;
                    cfj_UpperLeftArm.angularYZDrive = PoseOn;
                    cfj_LowerLeftArm.angularXDrive = PoseOn;
                    cfj_LowerLeftArm.angularYZDrive = PoseOn;
                
                    cfj_Body.angularXDrive = PoseOn;
                    cfj_Body.angularYZDrive = PoseOn;
                }
                
                else if(!balanced)
                {
                    cfj_UpperLeftArm.angularXDrive = DriveOff;
                    cfj_UpperLeftArm.angularYZDrive = DriveOff;
                    cfj_LowerLeftArm.angularXDrive = DriveOff;
                    cfj_LowerLeftArm.angularYZDrive = DriveOff;
                }
                
                ResetPose = true;
                reachLeftAxisUsed = false;
            }
        }
        
        
            
            
        //Reach Right
        if(Input.GetAxisRaw(reachRight) != 0 && !punchingRight)
        {
            
            if(!reachRightAxisUsed)
            {
                //Adjust Right Arm joint strength
                cfj_UpperRightArm.angularXDrive = ReachStiffness;
                cfj_UpperRightArm.angularYZDrive = ReachStiffness;
                cfj_LowerRightArm.angularXDrive = ReachStiffness;
                cfj_LowerRightArm.angularYZDrive = ReachStiffness;
                    
                //Adjust body joint strength
                cfj_Body.angularXDrive = CoreStiffness;
                cfj_Body.angularYZDrive = CoreStiffness;
                
                reachRightAxisUsed = true;
            }
            
            if(MouseYAxisArms <= 1.2f && MouseYAxisArms >= -1.2f)
            {
                MouseYAxisArms = MouseYAxisArms + (Input.GetAxis("Mouse Y") / reachSensitivity);
            }
            
            else if(MouseYAxisArms > 1.2f)
            {
                MouseYAxisArms = 1.2f;
            }
            
            else if(MouseYAxisArms < -1.2f)
            {
                MouseYAxisArms = -1.2f;
            }
            
            //upper right arm pose
            cfj_UpperRightArm.targetRotation = new Quaternion( 0.58f + (MouseYAxisArms), -0.88f - (MouseYAxisArms), 0.8f, 1);
        }
        
        if(Input.GetAxisRaw(reachRight) == 0 && !punchingRight)
        {
            if(reachRightAxisUsed)
            {
                if(balanced)
                {
                    cfj_UpperRightArm.angularXDrive = PoseOn;
                    cfj_UpperRightArm.angularYZDrive = PoseOn;
                    cfj_LowerRightArm.angularXDrive = PoseOn;
                    cfj_LowerRightArm.angularYZDrive = PoseOn;
                
                    cfj_Body.angularXDrive = PoseOn;
                    cfj_Body.angularYZDrive = PoseOn;
                }
                
                else if(!balanced)
                {
                    cfj_UpperRightArm.angularXDrive = DriveOff;
                    cfj_UpperRightArm.angularYZDrive = DriveOff;
                    cfj_LowerRightArm.angularXDrive = DriveOff;
                    cfj_LowerRightArm.angularYZDrive = DriveOff;
                }
                
                ResetPose = true;
                reachRightAxisUsed = false;
            }
        }
        
    }
    
    
    
    //---Player Punch---//
    /////////////////////
    void PlayerPunch()
    {
        
        //punch right
        if(!punchingRight && Input.GetKey(punchRight))
        {
            punchingRight= true;
            
            //Right hand punch pull back pose
            cfj_Body.targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            cfj_UpperRightArm.targetRotation = new Quaternion( -0.62f, -0.51f, 0.02f, 1);
            cfj_LowerRightArm.targetRotation = new Quaternion( 1.31f, 0.5f, -0.5f, 1);
		}
        
        if(punchingRight && !Input.GetKey(punchRight))
        {
            punchingRight = false;
            
            //Right hand punch release pose
			cfj_Body.targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
			cfj_UpperRightArm.targetRotation = new Quaternion( 0.74f, 0.04f, 0f, 1);
			cfj_LowerRightArm.targetRotation = new Quaternion( 0.2f, 0, 0, 1);
            
            //Right hand punch force
			RightHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
 
			rgbd_Body.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			
            StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if(!Input.GetKey(punchRight))
                {
                    cfj_UpperRightArm.targetRotation = UpperRightArmTarget;
                    cfj_LowerRightArm.targetRotation = LowerRightArmTarget;
                }
            }
        }
        
        
        //punch left
        if(!punchingLeft && Input.GetKey(punchLeft))
        {
            punchingLeft = true;
            
            //Left hand punch pull back pose
            cfj_Body.targetRotation = new Quaternion( -0.15f, 0.15f, 0, 1);
            cfj_UpperLeftArm.targetRotation = new Quaternion( 0.62f, -0.51f, 0.02f, 1);
            cfj_LowerLeftArm.targetRotation = new Quaternion( -1.31f, 0.5f, 0.5f, 1);
        }
        
        if(punchingLeft && !Input.GetKey(punchLeft))
        {
            punchingLeft = false;
            
            //Left hand punch release pose
            cfj_Body.targetRotation = new Quaternion( -0.15f, -0.15f, 0, 1);
            cfj_UpperLeftArm.targetRotation = new Quaternion( -0.74f, 0.04f, 0f, 1);
            cfj_LowerLeftArm.targetRotation = new Quaternion( -0.2f, 0, 0, 1);
            
            //Left hand punch force
            LeftHand.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
 
            rgbd_Body.AddForce(APR_Parts[0].transform.forward * punchForce, ForceMode.Impulse);
			
            StartCoroutine(DelayCoroutine());
			IEnumerator DelayCoroutine()
            {
                yield return new WaitForSeconds(0.3f);
                if(!Input.GetKey(punchLeft))
                {
                    cfj_UpperLeftArm.targetRotation = UpperLeftArmTarget;
                    cfj_LowerLeftArm.targetRotation = LowerLeftArmTarget;
                }
            }
        }
    }
    
    
    
    //---Player Walking---//
    ///////////////////////
    void Walking()
	{
        if (!inAir)
        {
            if (WalkForward)
			{
                //right leg
				if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}
                
                //left leg
				if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
				{
					StepLeft = true;
					Alert_Leg_Left = true;
					Alert_Leg_Right = true;
				}
			}

			if (WalkBackward)
			{
                //right leg
				if (APR_Parts[11].transform.position.z > APR_Parts[12].transform.position.z && !StepLeft && !Alert_Leg_Right)
				{
					StepRight = true;
					Alert_Leg_Right = true;
					Alert_Leg_Left = true;
				}
                
                //left leg
				if (APR_Parts[11].transform.position.z < APR_Parts[12].transform.position.z && !StepRight && !Alert_Leg_Left)
				{
					StepLeft = true;
					Alert_Leg_Left = true;
					Alert_Leg_Right = true;
				}
			}
		
			//Step right
			if (StepRight)
			{
				Step_R_timer += Time.fixedDeltaTime;
			 
                //Right foot force down
				rgbd_RightFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
			
				//walk simulation
				if (WalkForward)
				{                
					cfj_UpperRightLeg.targetRotation = new Quaternion(cfj_UpperRightLeg.targetRotation.x + 0.09f * StepHeight, cfj_UpperRightLeg.targetRotation.y, cfj_UpperRightLeg.targetRotation.z, cfj_UpperRightLeg.targetRotation.w);
					cfj_LowerRightLeg.targetRotation = new Quaternion(cfj_LowerRightLeg.targetRotation.x - 0.09f * StepHeight * 2, cfj_LowerRightLeg.targetRotation.y, cfj_LowerRightLeg.targetRotation.z, cfj_LowerRightLeg.targetRotation.w);

					cfj_UpperLeftLeg.targetRotation = new Quaternion(cfj_UpperLeftLeg.targetRotation.x - 0.12f * StepHeight / 2, cfj_UpperLeftLeg.targetRotation.y, cfj_UpperLeftLeg.targetRotation.z, cfj_UpperLeftLeg.targetRotation.w);
				}
                
                if (WalkBackward)
				{
					cfj_UpperRightLeg.targetRotation = new Quaternion(cfj_UpperRightLeg.targetRotation.x - 0.00f * StepHeight, cfj_UpperRightLeg.targetRotation.y, cfj_UpperRightLeg.targetRotation.z, cfj_UpperRightLeg.targetRotation.w);
					cfj_LowerRightLeg.targetRotation = new Quaternion(cfj_LowerRightLeg.targetRotation.x - 0.07f * StepHeight * 2, cfj_LowerRightLeg.targetRotation.y, cfj_LowerRightLeg.targetRotation.z, cfj_LowerRightLeg.targetRotation.w);

					cfj_UpperLeftLeg.targetRotation = new Quaternion(cfj_UpperLeftLeg.targetRotation.x + 0.02f * StepHeight / 2, cfj_UpperLeftLeg.targetRotation.y, cfj_UpperLeftLeg.targetRotation.z, cfj_UpperLeftLeg.targetRotation.w);
				}
                
                
				//step duration
				if (Step_R_timer > StepDuration)
				{
					Step_R_timer = 0;
					StepRight = false;

					if (WalkForward || WalkBackward)
					{
						StepLeft = true;
					}
				}
			}
			else
			{
				//reset to idle
				cfj_UpperRightLeg.targetRotation = Quaternion.Lerp(cfj_UpperRightLeg.targetRotation, UpperRightLegTarget, (8f) * Time.fixedDeltaTime);
				cfj_LowerRightLeg.targetRotation = Quaternion.Lerp(cfj_LowerRightLeg.targetRotation, LowerRightLegTarget, (17f) * Time.fixedDeltaTime);
				
                //feet force down
				rgbd_RightFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
				rgbd_LeftFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
			}
            
            
            //Step left
			if (StepLeft)
			{
				Step_L_timer += Time.fixedDeltaTime;
			     
                //Left foot force down
				rgbd_LeftFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
                
                //walk simulation
				if (WalkForward)
				{
					cfj_UpperLeftLeg.targetRotation = new Quaternion(cfj_UpperLeftLeg.targetRotation.x + 0.09f * StepHeight, cfj_UpperLeftLeg.targetRotation.y, cfj_UpperLeftLeg.targetRotation.z, cfj_UpperLeftLeg.targetRotation.w);
					cfj_LowerLeftLeg.targetRotation = new Quaternion(cfj_LowerLeftLeg.targetRotation.x - 0.09f * StepHeight * 2, cfj_LowerLeftLeg.targetRotation.y, cfj_LowerLeftLeg.targetRotation.z, cfj_LowerLeftLeg.targetRotation.w);

					cfj_UpperRightLeg.targetRotation = new Quaternion(cfj_UpperRightLeg.targetRotation.x - 0.12f * StepHeight / 2, cfj_UpperRightLeg.targetRotation.y, cfj_UpperRightLeg.targetRotation.z, cfj_UpperRightLeg.targetRotation.w);
				}
                
                if (WalkBackward)
				{
					cfj_UpperLeftLeg.targetRotation = new Quaternion(cfj_UpperLeftLeg.targetRotation.x - 0.00f * StepHeight, cfj_UpperLeftLeg.targetRotation.y, cfj_UpperLeftLeg.targetRotation.z, cfj_UpperLeftLeg.targetRotation.w);
					cfj_LowerLeftLeg.targetRotation = new Quaternion(cfj_LowerLeftLeg.targetRotation.x - 0.07f * StepHeight * 2, cfj_LowerLeftLeg.targetRotation.y, cfj_LowerLeftLeg.targetRotation.z, cfj_LowerLeftLeg.targetRotation.w);

					cfj_UpperRightLeg.targetRotation = new Quaternion(cfj_UpperRightLeg.targetRotation.x + 0.02f * StepHeight / 2, cfj_UpperRightLeg.targetRotation.y, cfj_UpperRightLeg.targetRotation.z, cfj_UpperRightLeg.targetRotation.w);
				}
			
			
				//Step duration
				if (Step_L_timer > StepDuration)
				{
					Step_L_timer = 0;
					StepLeft = false;

					if (WalkForward || WalkBackward)
					{
						StepRight = true;
					}
				}
			}
			else
			{
				//reset to idle
				cfj_UpperLeftLeg.targetRotation = Quaternion.Lerp(cfj_UpperLeftLeg.targetRotation, UpperLeftLegTarget, (7f) * Time.fixedDeltaTime);
				cfj_LowerLeftLeg.targetRotation = Quaternion.Lerp(cfj_LowerLeftLeg.targetRotation, LowerLeftLegTarget, (18f) * Time.fixedDeltaTime);
				
                //feet force down
				rgbd_RightFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
				rgbd_LeftFoot.AddForce(-Vector3.up * FeetMountForce * Time.fixedDeltaTime, ForceMode.Impulse);
			}
		}
	}
    
    
    
    //---Activate Ragdoll---//
    /////////////////////////
    public void ActivateRagdoll()
	{
        isRagdoll = true;
		balanced = false;
		
		//Root
		cfj_Root.angularXDrive = DriveOff;
		cfj_Root.angularYZDrive = DriveOff;
		//head
		cfj_Head.angularXDrive = DriveOff;
		cfj_Head.angularYZDrive = DriveOff;
		//arms
        if(!reachRightAxisUsed)
        {
            cfj_UpperRightArm.angularXDrive = DriveOff;
            cfj_UpperRightArm.angularYZDrive = DriveOff;
            cfj_LowerRightArm.angularXDrive = DriveOff;
            cfj_LowerRightArm.angularYZDrive = DriveOff;
        }
        
        if(!reachLeftAxisUsed)
        {
            cfj_UpperLeftArm.angularXDrive = DriveOff;
            cfj_UpperLeftArm.angularYZDrive = DriveOff;
            cfj_LowerLeftArm.angularXDrive = DriveOff;
            cfj_LowerLeftArm.angularYZDrive = DriveOff;
        }
		//legs
		cfj_UpperRightLeg.angularXDrive = DriveOff;
		cfj_UpperRightLeg.angularYZDrive = DriveOff;
		cfj_LowerRightLeg.angularXDrive = DriveOff;
		cfj_LowerRightLeg.angularYZDrive = DriveOff;
		cfj_UpperLeftLeg.angularXDrive = DriveOff;
		cfj_UpperLeftLeg.angularYZDrive = DriveOff;
		cfj_LowerLeftLeg.angularXDrive = DriveOff;
		cfj_LowerLeftLeg.angularYZDrive = DriveOff;
		cfj_RightFoot.angularXDrive = DriveOff;
		cfj_RightFoot.angularYZDrive = DriveOff;
		cfj_LeftFoot.angularXDrive = DriveOff;
		cfj_LeftFoot.angularYZDrive = DriveOff;
	}
	
    
    

	//---Deactivate Ragdoll---//
	///////////////////////////
	void DeactivateRagdoll()
	{
        isRagdoll = false;
		balanced = true;
		
		//Root
		cfj_Root.angularXDrive = BalanceOn;
		cfj_Root.angularYZDrive = BalanceOn;
		//head
		cfj_Head.angularXDrive = PoseOn;
		cfj_Head.angularYZDrive = PoseOn;
		//arms
		if(!reachRightAxisUsed)
        {
            cfj_UpperRightArm.angularXDrive = PoseOn;
            cfj_UpperRightArm.angularYZDrive = PoseOn;
            cfj_LowerRightArm.angularXDrive = PoseOn;
            cfj_LowerRightArm.angularYZDrive = PoseOn;
        }
        
        if(!reachLeftAxisUsed)
        {
            cfj_UpperLeftArm.angularXDrive = PoseOn;
            cfj_UpperLeftArm.angularYZDrive = PoseOn;
            cfj_LowerLeftArm.angularXDrive = PoseOn;
            cfj_LowerLeftArm.angularYZDrive = PoseOn;
        }
		//legs
		cfj_UpperRightLeg.angularXDrive = PoseOn;
		cfj_UpperRightLeg.angularYZDrive = PoseOn;
		cfj_LowerRightLeg.angularXDrive = PoseOn;
		cfj_LowerRightLeg.angularYZDrive = PoseOn;
		cfj_UpperLeftLeg.angularXDrive = PoseOn;
		cfj_UpperLeftLeg.angularYZDrive = PoseOn;
		cfj_LowerLeftLeg.angularXDrive = PoseOn;
		cfj_LowerLeftLeg.angularYZDrive = PoseOn;
		cfj_RightFoot.angularXDrive = PoseOn;
		cfj_RightFoot.angularYZDrive = PoseOn;
		cfj_LeftFoot.angularXDrive = PoseOn;
		cfj_LeftFoot.angularYZDrive = PoseOn;
        
        ResetPose = true;
	}
	
    
    
    //---Reset Player Pose---//
    //////////////////////////
    void ResetPlayerPose()
    {
        if(ResetPose && !jumping)
        {
            cfj_Body.targetRotation = BodyTarget;
            cfj_UpperRightArm.targetRotation = UpperRightArmTarget;
			cfj_LowerRightArm.targetRotation = LowerRightArmTarget;
            cfj_UpperLeftArm.targetRotation = UpperLeftArmTarget;
			cfj_LowerLeftArm.targetRotation = LowerLeftArmTarget;
            
            MouseYAxisArms = 0;
            
            ResetPose = false;
        }
    }
    
    
    
	//---Calculating Center of mass point---//
	/////////////////////////////////////////
	void CenterOfMass()
	{
			CenterOfMassPoint =
			
			(rgbd_Root.mass * APR_Parts[0].transform.position + 
            rgbd_Body.mass * APR_Parts[1].transform.position +
            rgbd_Head.mass * APR_Parts[2].transform.position +
            rgbd_UpperRightArm.mass * APR_Parts[3].transform.position +
            rgbd_LowerRightArm.mass * APR_Parts[4].transform.position +
            rgbd_UpperLeftArm.mass * APR_Parts[5].transform.position +
            rgbd_LowerLeftArm.mass * APR_Parts[6].transform.position +
			rgbd_UpperRightLeg.mass * APR_Parts[7].transform.position +
			rgbd_LowerRightLeg.mass * APR_Parts[8].transform.position +
			rgbd_UpperLeftLeg.mass * APR_Parts[9].transform.position +
			rgbd_LowerLeftLeg.mass * APR_Parts[10].transform.position +
			rgbd_RightFoot.mass * APR_Parts[11].transform.position +
			rgbd_LeftFoot.mass * APR_Parts[12].transform.position) 
            
            /
			
            (rgbd_Root.mass + rgbd_Body.mass +
            rgbd_Head.mass +rgbd_UpperRightArm.mass +
            rgbd_LowerRightArm.mass + rgbd_UpperLeftArm.mass +
            rgbd_LowerLeftArm.mass + rgbd_UpperRightLeg.mass +
			rgbd_LowerRightLeg.mass + rgbd_UpperLeftLeg.mass +
			rgbd_LowerLeftLeg.mass + rgbd_RightFoot.mass +
			rgbd_LeftFoot.mass);
			
			COMP.position = CenterOfMassPoint;
	}


    /// <summary>
    /// Editor Debug Mode
    /// </summary>
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (editorDebugMode)
        {
            Debug.DrawRay(Root.transform.position, -Root.transform.up * balanceHeight, Color.green);

            if (useStepPrediction)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(COMP.position, 0.3f);
            }
        }
    }
#endif
}
