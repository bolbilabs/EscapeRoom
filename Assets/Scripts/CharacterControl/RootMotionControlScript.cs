using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class RootMotionControlScript : MonoBehaviour
{
    private Animator anim;	
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public float animationSpeed = 1.0f;
    public float rootMovementSpeed = 1.0f;
    public float rootTurnSpeed = 1.0f;
    public float jumpHeight = 100.0f;

    private int groundContactCount = 0;

    public GameObject buttonObject;

    private bool once = false;
    private float lastInputForward;

    public Vector3 lastGroundPosition;

    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }

    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");
    }


    // Use this for initialization
    void Start()
    {
		//example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");

        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");
            
    }
        





    void Update()
    {

        float inputForward=0f;
        float inputTurn=0f;
        bool inputAction = false;
        bool jump = false;
        // input is polled in the Update() step, not FixedUpdate()
        // Therefore, you should ONLY use input state that is NOT event-based in FixedUpdate()
        // Input events should be handled in Update(), and possibly passed on to FixedUpdate() through 
        // the state of the MonoBehavior
        if (cinput.enabled)
        {
            inputForward = cinput.Forward;
            inputTurn = cinput.Turn;
            inputAction = cinput.Action;
            jump = cinput.Jump;
        }

        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground
        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        //anim.SetBool("jump", true);

        if (!isGrounded)
        {

            anim.SetFloat("vely", lastInputForward);
        }


        if (isGrounded)
        {
            anim.SetFloat("velx", inputTurn);
            anim.SetFloat("vely", inputForward);
            lastInputForward = inputForward;
            anim.SetBool("doButtonPress", inputAction);
            anim.speed = animationSpeed;
        }

        if (jump && isGrounded)
        {
            rbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }

        if (inputAction)
            Debug.Log("Action pressed");

    }


    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {

            ++groundContactCount;

            // Generate an event that might play a sound, generate a particle effect, etc.
            EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
						
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            --groundContactCount;
        }

    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        bool isGrounded = IsGrounded || CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.1f, 1f, out closeToJumpableGround);

        //if (isGrounded)
        //{
         	//use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        //}
        //else
        //{
        //    //Simple trick to keep model from climbing other rigidbodies that aren't the ground
        //    newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        //}

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        newRootPosition = Vector3.LerpUnclamped(this.transform.position, newRootPosition, rootMovementSpeed);
        newRootRotation = Quaternion.LerpUnclamped(this.transform.localRotation, newRootRotation, rootTurnSpeed);


        this.transform.position = newRootPosition;
        this.transform.rotation = newRootRotation;


        //float totalRayLen = 1f + 0.1f;

        //Ray ray = new Ray(this.transform.position + Vector3.up * 1f, Vector3.down);

        //int layerMask = 1 << LayerMask.NameToLayer("Default");

        //RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask);

        //RaycastHit boundHit = new RaycastHit();
        //bool isHit = false;

        //foreach (RaycastHit hit in hits)
        //{

        //    if (hit.collider.gameObject.CompareTag("Rebound"))
        //    {
           
        //        boundHit = hit;
        //        isHit = true;
        //        break; //only need to find the ground once
        //    }

        //}

        //if (isHit)
        //{
        //    lastGroundPosition = boundHit.point;
        //}
    }

    private void OnAnimatorIK()
    {
        if (anim)
        {
            AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo(0);
            if (astate.IsName("ButtonPress"))
            {
                float buttonWeight = anim.GetFloat("buttonClose");
                // Set the look target position, if one has been assigned
                if (buttonObject != null)
                {
                    anim.SetLookAtWeight(buttonWeight);
                    anim.SetLookAtPosition(buttonObject.transform.position);
                    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, buttonWeight);
                    anim.SetIKPosition(AvatarIKGoal.RightHand, buttonObject.transform.position);
                }
            }
            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetLookAtWeight(0);
            }
        }
    }

    public void SnapToGround()
    {
        transform.position = lastGroundPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rebound")
        {
            lastGroundPosition = other.ClosestPoint(transform.position);
        }

        if (other.tag == "Reset")
        {
            GameControl.GetInstance().ReloadLevel();
        }

    }



}
