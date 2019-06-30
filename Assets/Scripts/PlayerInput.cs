using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action OnFirstButtonPressed = () => { };

    [SerializeField] private BodyShell body = null;

    private bool playerPressedAnybutton = false;

    void Update()
    {
        bool upButtonDown = Input.GetButtonDown("Up");
        bool downButtonDown = Input.GetButtonDown("Down");

        bool firstButtonPress = playerPressedAnybutton == false && (upButtonDown || downButtonDown);
        if (firstButtonPress)
        {
            playerPressedAnybutton = true;
            OnFirstButtonPressed();
        }

        if (upButtonDown)
        {
            body.SetStance(true);
        }
        else if(downButtonDown)
        {
            body.SetStance(false);
        }
    }
}
