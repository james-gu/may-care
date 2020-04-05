﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelBehavior_2 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    public bool isDone = false;
    public bool isActivated = false;
    public int activeTriggerIndex = 0;
    public string EroFaceDirection;
    /* ======================================================================= */
    
    public Model_Locations[] triggerLocations;
    public Model_Dialogue[] dialogues;
    
    
    protected override void OnDisable() {
        if (!isDone)
        {    
            print("changing state to interact");
            game.ChangeStateInteract();
        }
    }

    protected override void HandleTriggerLocations()
    {
        if (isDone || game.GetPlayerIsTalking()) return;
        
        foreach (Vector3 loc in triggerLocations[activeTriggerIndex].locations)
        {
            if (
                game.GetPlayerLocation() == loc
                && game.state == "interact"
            )
            {
                game.PauseBgMusic();
                
                if (game.GetEroThemeActive())
                {
                    game.UnPauseEroTheme();
                }
                else
                {
                    game.PlayEroTheme();
                }
                game.ChangeStateCutScene();
                
                // game.PlayerFaceDirection("down");
                game.StartDialogue(dialogues[activeTriggerIndex]);
                
                /*
                    trigger locations must be 1 < dialogue
                */
                if (activeTriggerIndex == triggerLocations.Length - 1) isDone = true;
                activeTriggerIndex++;
            }
        }
    }

    protected override void HandleAction()
    {
        if (
            game.state == "cut-scene"
            && !game.GetPlayerIsTalking()
        )
        {
            game.ChangeStateCutSceneNPCMoving();
            // need this bc once leave room, no longer inProgress
            game.TriggerMovingNPCMove(0);
        }

        if (Input.GetButtonDown("Action1") && game.state == "cut-scene")
        {
            game.HandleContinuingDialogueActions("Action1");
        }

        if (Input.GetButtonDown("Submit") && game.state == "cut-scene")
        {
            game.HandleContinuingDialogueActions("Submit");
        }
    }

    // called from Script_Exits()
    public override void InitGameState() {
        print("INITTING GAME STATE FROM LB2 NOT LB");
        // to happen after fadein 
        if (isActivated)
        {
            game.ChangeStateInteract();
        }
        isActivated = true;
    }
    
    public override void Setup()
    {
        game.CreateInteractableObjects();
        game.EnableExits();

        if (!isDone)
        {
            if (isActivated)
            {
                game.CreateMovingNPC(0, "right", activeTriggerIndex, true);
            }
            else
            {
                game.CreateMovingNPC(
                    0,
                    null,
                    activeTriggerIndex
                );
                game.ChangeStateCutSceneNPCMoving();
                game.TriggerMovingNPCMove(0);
            }
        }
    }
}
