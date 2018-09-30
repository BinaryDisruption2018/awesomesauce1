using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbit2 : MonoBehaviour
{
    public GameObject moon; //this actually represents the earth but everytime i try to change the variable name it breaks
    public float speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OrbitAround();
    }
    void OrbitAround()
    {
        transform.RotateAround(moon.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}
