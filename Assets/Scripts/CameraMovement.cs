using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 position_FromPlayer = new Vector3();
    [SerializeField] private Transform player_Transform = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FollowPlayer()
    {
        if(PlayerMovement.is_In_LRlane)
        transform.position = player_Transform.position + position_FromPlayer;
    }
}
