using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    private Vector3 _touchStartPosition;
    private Vector3 _touchPosition;
    public float JoystickStickRange = 0.7f;
    public float Offset = 0.1f;
    private float JoystickRange = 0.1f;
    public double YInput { get; set; }
    public double XInput { get; set; }
    public Transform Joystick;
    private SpriteRenderer m_Renderer;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            _touchStartPosition = Vector3.zero;
            m_Renderer.enabled = false;
            Joystick.gameObject.SetActive(false);
            YInput = 0f;
            XInput = 0f;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            _touchStartPosition = Input.mousePosition;
            _touchStartPosition.z = 1f;
            var point = cam.ScreenToWorldPoint(_touchStartPosition);
            m_Renderer.transform.position = point;
            m_Renderer.enabled = true;
            Joystick.gameObject.SetActive(true);
        }
        else if (Input.GetButton("Fire1"))
        {
            _touchPosition = Input.mousePosition;
            _touchPosition.z = 1f;
            var point = cam.ScreenToWorldPoint(_touchPosition);
            var pointStart = cam.ScreenToWorldPoint(_touchStartPosition);

            var x = point.x - pointStart.x;
            var z = point.z - pointStart.z;

            var xNeg = pointStart.x > point.x;
            var zNeg = pointStart.z > point.z;

            var pool = Mathf.Abs(x) + Mathf.Abs(z);
            var pX = Mathf.Abs(x) / pool;
            var pZ = Mathf.Abs(z) / pool;
            var lastX = JoystickStickRange * pX;
            var lastZ = JoystickStickRange * pZ;

            var inputRateX = pX * (xNeg ? -1f : 1f);
            var inputRateY = pZ * (zNeg ? -1f : 1f);
            var lastOneX = lastX * (xNeg ? -1f : 1f);
            var lastOneY = lastZ * (zNeg ? -1f : 1f);

            if (Vector3.Distance(point, pointStart) < JoystickRange)
            {
                Joystick.transform.position = point;

                YInput = Math.Round(inputRateY, 2);
                XInput = Math.Round(inputRateX, 2);
                if (Vector3.Distance(point, pointStart) < Offset)
                {
                    YInput = 0;
                    XInput = 0;
                }
            }
            else
            {
                YInput = Math.Round(inputRateY, 2);
                XInput = Math.Round(inputRateX, 2);
                Joystick.transform.localPosition = new Vector3(lastOneX / 10f, lastOneY / 10f);
            }
        }
    }
}