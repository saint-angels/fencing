using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Body body = null;

    void Start()
    {
        
    }

    void Update()
    {
        bool upButtonDown = Input.GetButtonDown("Up");
        bool downButtonDown = Input.GetButtonDown("Down");

        if (upButtonDown)
        {
            body.MoveHand(true);
        }
        else if(downButtonDown)
        {
            body.MoveHand(false);
        }
    }
}
