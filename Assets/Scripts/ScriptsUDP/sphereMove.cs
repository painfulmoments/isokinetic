using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereMove : MonoBehaviour
{
    public static sphereMove instance;

    public float x_position = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.localPosition = new Vector3 (x_position, 2, 0);


    }
}
