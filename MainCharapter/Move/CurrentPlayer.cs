using UnityEngine;
using System.Collections;

public class CurrentPlayer : PlayerBase {
	
	// Update is called once per frame
	new void Update() {
        base.Update();

        if (Input.GetAxis("Horizontal") != 0)
        {
            Moving((int)Input.GetAxis("Horizontal"));
        }

        if (Input.GetButtonDown("Jump"))
        {
           Jumping();
        }

        if (Input.GetButton("Fire1"))
        {
            Attacking(true);
        }
        else
        {
            Attacking(false);
        }
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
