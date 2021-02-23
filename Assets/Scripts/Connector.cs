using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    private Transform[] children = new Transform[3] { null, null, null };
    [SerializeField] public bool is_Triple = false;
    [SerializeField] private GameObject turn_Point = null;

    private void OnEnable()
    {
        ProvideDirections();
    }

    private void OnDisable()
    {
        
    }

    private void ProvideDirections()
    {
        int i = 0;

        foreach(Transform child in transform)
        {
            if(child.tag == "Connector")
            {
                foreach(Transform gChild in child)
                {
                    if(gChild != null)
                    children[i] = gChild;

                    print(children[i].tag);
                    i += 1;
                }
                break;
            }
        }
    }

    public Transform PickDirection(bool is_Triple)
    {
        Transform go = null;
        if (is_Triple)
        { go = children[Random.Range(0, 3)]; print(go.tag); }
        else if (!is_Triple)
        { go = children[Random.Range(0, 2)]; print(go.tag); }

        if (go != null)
        {
            if((go.tag == "west" || go.tag == "east") && PlayerMovement.was_In_LR)
            {
                turn_Point.SetActive(true);
            }
            return go;
        }
        else
        {
            print("did not find any connector");
            return null;
        }
    }
}
