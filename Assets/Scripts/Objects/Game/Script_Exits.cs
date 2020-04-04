﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Exits : MonoBehaviour
{
    public AudioClip exitSFX;
    public CanvasGroup canvas;

    private AudioSource audioSource;
    private Script_Game game;
    private IEnumerator coroutine;


    public float InitiateLevelWaitTime;
    public float fadeSpeed;

    private bool exitsDisabled;
    private bool isFadeOut;
    private bool isFadeIn;
    private bool isHandlingExit;
    private int levelToGo;
    
    void Update()
    {
        if (isFadeOut)  FadeOut();
        if (isFadeIn)   FadeIn();    
    }

    public void Exit(
        int level,
        Vector3 playerNextSpawnPosition,
        string playerFacingDirection,
        bool isExit
    )
    {
        print("Exit() called with: " + level + ", " + isExit);
        if (isHandlingExit)             return;
        // still allow player to go back where they came from
        if (isExit && exitsDisabled)    return;
        
        if (!isHandlingExit)    isHandlingExit = true;

        int x = (int)playerNextSpawnPosition.x;
        int z = (int)playerNextSpawnPosition.z;

        game.ChangeStateToInitiateLevel();
        game.SetPlayerState(
            new Model_PlayerState(null, x, z, playerFacingDirection)
        );
        
        isFadeOut = true;
        levelToGo = level;
        audioSource.PlayOneShot(exitSFX, 0.15f);
    }

    public void DisableExits()
    {
        exitsDisabled = true;
    }

    public void EnableExits()
    {
        exitsDisabled = false;
    }

    public void StartFadeIn()
    {
        isFadeIn = true;
    }

    public void StartFadeOut()
    {
        isFadeOut = true;
    }

    void FadeOut()
    {
        canvas.alpha += fadeSpeed * Time.deltaTime;

        if (canvas.alpha >= 1f)
        {
            canvas.alpha = 1f;
            isFadeOut = false;
            
            game.DestroyLevel();
            
            isHandlingExit = false;
            game.level = levelToGo;
            
            game.InitiateLevel();
            
            isFadeIn = true;
        }
    }

    void FadeIn()
    {
        canvas.alpha -= fadeSpeed * Time.deltaTime;

        if (canvas.alpha <= 0f)
        {
            canvas.alpha = 0f;
            isFadeIn = false;

            // after faded in, player can then move
            // change from initiate-level state
            game.SetInitialGameState();
        }
    }

    public void Setup(Script_Game _game)
    {
        game = _game;
        audioSource = GetComponent<AudioSource>();
    }
}