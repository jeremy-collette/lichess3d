﻿using UnityEngine;

public class FlyCamera : MonoBehaviour {
    private readonly float mainSpeed = 1.0f;

    private readonly float shiftAdd = 2.0f;

    private readonly float maxShift = 20.0f;

    private readonly float camSens = 0.20f;

    private Vector3 lastMouse = new Vector3(255, 255, 255);

    private float totalRun= 1.0f;

    void Update ()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        // Mouse camera angle
        if (Input.GetMouseButton(0))
        {
            lastMouse = Input.mousePosition - lastMouse ;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse =  Input.mousePosition;
        }

        // Keyboard commands
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p  = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        var newPosition = transform.position;

        // If player wants to move on X and Z axis only
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}