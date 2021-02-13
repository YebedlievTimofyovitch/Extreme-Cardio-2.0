using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    private Transform LR_Lane_Transform = null;

    #region "Left Right Platform Movement Variables"
    [SerializeField] private float LR_movementSpeed = 10f;
    [SerializeField] private float[] LR_Positions = new float[2] { 0.0f , 0.0f };
    private Vector3 LR_P_position = new Vector3();
    private bool is_GoingLeft = false;
    private bool has_SwitchedLanes = false;
    static public bool is_In_LRlane = false;
    #endregion

    #region Inside the Connector Variables
    private bool is_In_Connector = false;
    #endregion

    //zain

    #region
    Rigidbody player_RB = null;
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

        if(!is_In_LRlane & is_In_Connector)
        {
            ReturnToCenter(LR_Lane_Transform);
        }
    }

    void FixedUpdate()
    {
        MoveForward();
    }

    #region Left & Right Movement Methods
    private void SidewaysMovement(bool isGL)
        {
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

    private void ReturnToCenter(Transform FPp)
    {
        transform.position = Vector3.Lerp( transform.position , new Vector3(FPp.position.x , transform.position.y , transform.position.z) , LR_movementSpeed * Time.deltaTime );
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PForward")
        {
            is_In_LRlane = true;
            LR_P_position = other.transform.position;
        }

        if(other.tag == "Connector")
        {
            is_In_Connector = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PForward")
        {
            is_In_LRlane = false;
            LR_Lane_Transform = other.transform;
        }

        if (other.tag == "Connector")
        {
            is_In_Connector = false;
        }
    }


}
