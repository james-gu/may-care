﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerGhost : MonoBehaviour
{
    public AnimationCurve progressCurve;
    public SpriteRenderer spriteRenderer;
    
    
    private Animator animator;
    
    
    public float speed;
    public Vector3 startLocation;
    public Vector3 location;
    public float progress;
    public string facingDirection;

    [SerializeField] private Light l;


    private bool isMoving;

    void Update()
    {
        if (isMoving)   ActuallyMove();   
    }

    public void Move(string dir)
    {
        SetIsMoving();
        progress = 0f;
    }

    public void SetIsNotMoving()
    {
        isMoving = false;
        spriteRenderer.enabled = false;
    }

    void SetIsMoving()
    {
        isMoving = true;
        spriteRenderer.enabled = true;
    }

    void ActuallyMove()
    {
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(
            startLocation,
            location,
            progressCurve.Evaluate(progress)
        );

        if (progress >= 1f)
        {
            progress = 1f;
        }
    }

    public void AnimatorSetDirection(string dir)
    {
        facingDirection = dir;
        
        if (dir == "up")
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", 1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", 1f);
        }
        else if (dir == "down")
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", -1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", -1f);
        }
        else if (dir == "left")
        {
            animator.SetFloat("LastMoveX", -1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", -1f);
            animator.SetFloat("MoveZ", 0f);
        }
        else if (dir == "right")
        {
            animator.SetFloat("LastMoveX", 1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", 1f);
            animator.SetFloat("MoveZ", 0f);
        }
    }

    public void SetMoveAnimation()
    {
        animator.SetBool(
            "PlayerMoving",
            Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f
        );
    }
    
    public void SwitchLight(bool isOn)
    {
        l.enabled = isOn;
    }

    public void AdjustRotation()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void Setup(Vector3 loc)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        progress = 1f;
        location = loc;
        spriteRenderer.enabled = false;
        
        AdjustRotation();
    }
}
