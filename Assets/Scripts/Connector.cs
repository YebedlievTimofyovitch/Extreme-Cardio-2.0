using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    private GameObject[] children = new GameObject[3] { null, null, null };
    [SerializeField] public bool is_Triple = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void ProvideDirections()
    {
        int i = 0;

        foreach(GameObject child in transform)
        {
            if(child.tag == "connector")
            {
                children[i] = child;
                i += 1;
            }
        }
    }

    public Transform PickDirection(bool is_Triple)
    {
        GameObject go = null;
        if (is_Triple)
        { go = children[Random.Range(0, 2)]; }
        else
        { go = children[Random.Range(0, 1)]; }
        
        return go.transform;
    }
}
