using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_camera_follow : MonoBehaviour
{
    public Intro controller;
    public Transform target;
    private int moving = -1;
    private float x_move = 0f;
    private float y_move = 0f;
    public bool black;
    public int shaking = -1;
    public float shakiness = 0;
    void Start()
    {
        controller = FindObjectOfType<Intro>();
        if (black)
        {
            target = controller.bking.transform;
        }
        else
        {
            target = controller.wking.transform;
        }
    }

    void FixedUpdate()
    {
        float targetx;
        float targety;
        x_move = 0f;
        y_move = 0f;
        targetx = target.transform.position.x;
        targety = target.transform.position.y;
        if (controller.piece_count == -1 && black != controller.white_turn)
        {
            targety = controller.true_y;
        }
        if (transform.position.x < targetx - 0.025f)
        {
            x_move = 0.05f;
        }
        else if (transform.position.x > targetx + 0.025f)
        {
            x_move = -0.05f;
        }
        if (controller.y_dir != 0)
        {
            if (transform.position.y < targety - 0.025f)
            {
                y_move = 0.05f;
            }
            else if (transform.position.y > targety + 0.025f)
            {
                y_move = -0.05f;
            }
        }
        if (Mathf.Abs(transform.position.y - targety) > 0.2f)
        {
            y_move *= 4;
        }
        else if (Mathf.Abs(transform.position.x - targetx) > 0.2f)
        {
            x_move *= 4;
        }

        if(shaking >= 10)
        {
            shakiness = 1f/21f;
            shaking = 10;
        }
        if(shaking > 0)
        {
            shaking--;
            shakiness = -shakiness;
        }
        if(shaking == 0)
        {
            shakiness = 0;
        }
        transform.position += new Vector3(x_move + shakiness, y_move, 0);
    }
}
