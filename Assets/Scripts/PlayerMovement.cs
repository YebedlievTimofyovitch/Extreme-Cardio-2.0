using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    private Transform LR_Lane_Transform = null;
    private Transform direction = null;


    #region "Left Right Platform Movement Variables"
    [SerializeField] private float LR_movementSpeed = 10f;
    [SerializeField] private float[] LR_Positions = new float[2] { 0.0f , 0.0f };
    private Vector3 LR_P_position = new Vector3();
    private bool is_GoingLeft = false;
    private bool has_SwitchedLanes = false;
    static public bool is_In_LRlane = false;
    private bool is_Turning = false;
    #endregion

    #region Inside the Connector Variables
    [SerializeField] private float rotation_Multiplyer = 10.0f;
    [SerializeField] private float Amount_to_Rotate = 90.0f;
    private bool is_In_Connector = false;
    public static bool was_In_LR = false;
    #endregion


    #region
    private Rigidbody player_RB = null;
    [SerializeField] float forward_Speed = 10f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player_RB = GetComponent<Rigidbody>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

        if (is_In_LRlane && !is_In_Connector)
        {
            LeftRightMovementCheck();
            if(has_SwitchedLanes)
            SidewaysMovement(is_GoingLeft);
        }

        if(!is_In_LRlane && was_In_LR)
        {
            ReturnToCenter(LR_Lane_Transform);
        }

        if(is_Turning)
        {
            RotateTowardsWest_East(direction);
        }

        if(!is_Turning)
            MoveForward();
    }

    void FixedUpdate()
    {
        
        
    }

    #region Left & Right Movement Methods
    private void SidewaysMovement(bool isGL)
        {
            // in order to make the movement continues without stopping when leaving the button
            // a boolean was added. named "is going left" that changes value when a direction is selected
            if(isGL)
            transform.position = Vector3.Lerp(transform.position, new Vector3(LR_P_position.x + LR_Positions[1], transform.position.y, transform.position.z), LR_movementSpeed * Time.deltaTime);
            else if(!isGL)
            transform.position = Vector3.Lerp(transform.position, new Vector3(LR_P_position.x + LR_Positions[0], transform.position.y, transform.position.z), LR_movementSpeed * Time.deltaTime);
        }

    private void LeftRightMovementCheck()
    {
        if(Input.GetKey(KeyCode.D))
        {
            has_SwitchedLanes = true;
            is_GoingLeft = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            has_SwitchedLanes = true;
            is_GoingLeft = true;
        }
    }
    #endregion

    private void MoveForward()
    {
        player_RB.velocity = transform.forward * forward_Speed * Time.deltaTime;
    }

    private void RotateTowardsWest_East(Transform dir)
    {
        if (dir != null)
        {
            if (dir.tag == "east")
            {

                TurnSetAmountOfDegrees(rotation_Multiplyer , "east");
            }
            else if (dir.tag == "west")
            {

                TurnSetAmountOfDegrees(rotation_Multiplyer , "west");
            }

        }
    }

    private void ReturnToCenter(Transform FPp)
    {
        //let's the player move back to the center whenever he has left the forward platform (AKA left right lane)
        transform.position = Vector3.Lerp( transform.position , new Vector3(FPp.position.x , transform.position.y , transform.position.z) , LR_movementSpeed * Time.deltaTime );
    }

    private void TurnSetAmountOfDegrees(float multiplyer , string tag)
    {
        float atr;

        if (tag == "east")
            atr = Amount_to_Rotate;
        else
            atr = -Amount_to_Rotate;

        player_RB.velocity = Vector3.zero;
        Quaternion tr = transform.rotation;
        print("is turning");
        tr.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, atr , 0.0f) , multiplyer * Time.deltaTime);
        transform.rotation = tr;

        if(transform.eulerAngles.y >= 89.5f && tag == "east")
        {
            is_Turning = false;
        }
        else if((transform.eulerAngles.y >= -89.5f ) && tag == "west")
        {
            is_Turning = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PForward")
        {
            LR_Lane_Transform = other.transform;
            is_In_LRlane = true;
            was_In_LR = true;
            LR_P_position = other.transform.position;
        }

        if (other.tag == "Turn Point")
        {
            is_Turning = true;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Connector")
        {
            is_In_Connector = true;
            direction = other.GetComponent<Connector>().PickDirection(other.GetComponent<Connector>().is_Triple);
            print(direction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PForward")
        {
            is_In_LRlane = false;
            
        }

        

        if (other.tag == "Connector")
        {
            was_In_LR = false;
            is_In_Connector = false;
        }
    }


}
