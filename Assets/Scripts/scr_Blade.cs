using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Blade : MonoBehaviour
{
    public GameObject GameController;
    Rigidbody2D RB;
    CircleCollider2D CC;
    Camera Cam;

    Vector2 PreviousPosition;

    bool IsCutting = false;
    public float MinCuttingVelocity = 0.001f;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        CC = GetComponent<CircleCollider2D>();
        Cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameController.GetComponent<scr_GameController>().GameIsStarted)
                StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (IsCutting)
        {
            UpdateCut();
        }
    }
    void UpdateCut()
    {
        Vector2 newPosition = Cam.ScreenToWorldPoint(Input.mousePosition);
        RB.position = newPosition;

        float velocity = (newPosition - PreviousPosition).magnitude * Time.deltaTime;
        if (velocity > MinCuttingVelocity)
        {
            CC.enabled = true;
        }
        else
        {
            CC.enabled = false;
        }
    }
    void StartCutting()
    {
        IsCutting = true;
        CC.enabled = true;
    }
    void StopCutting()
    {
        IsCutting = false;
        CC.enabled = false;
    }
}
