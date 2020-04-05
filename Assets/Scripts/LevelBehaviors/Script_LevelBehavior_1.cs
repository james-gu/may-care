﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_1 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone = false;
    public bool isExitsDisabled = true;
    /* ======================================================================= */
    
    
    public Vector3[] triggerLocations;
    public Model_Dialogue dialogue;
    

    protected override void HandleTriggerLocations() {
        if (isDone)   return;

        for (int i = 0; i < triggerLocations.Length; i++)
        {
            if (
                game.GetPlayerLocation() == triggerLocations[i]
                && game.state == "interact"
            )
            {
                game.PauseBgMusic();
                game.PlayEroTheme();
                game.ChangeStateCutScene();
                
                game.PlayerFaceDirection("down");
                game.ChangeCameraTargetToNPC(0);
                game.StartDialogue(dialogue);
            }
        }
    }

    protected override void HandleAction()
    {
        if (game.state == "cut-scene" && !game.GetPlayerIsTalking())
        {
            isDone = true;

            isExitsDisabled = false;
            game.EnableExits();
            game.ChangeStateCutSceneNPCMoving();
            game.TriggerMovingNPCMove(0);

            // ero then leaves through door
        }

        if (Input.GetButtonDown("Action1") && game.state == "cut-scene" && !isDone)
        {
            game.HandleContinuingDialogueActions("Action1");
        }

        if (Input.GetButtonDown("Submit") && game.state == "cut-scene" && !isDone)
            game.HandleContinuingDialogueActions("Submit");
    }

    public override void Setup()
    {
        if (isExitsDisabled)    game.DisableExits();
        else game.EnableExits();

        // base.Setup();
        if (!isDone)
        {
            game.CreateNPCs();
        }
    }
}
