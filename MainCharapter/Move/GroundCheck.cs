using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

    public GameObject currentPlatform;  //Текущая платформа(только для собых платформ).
    public bool onMovingPlatform;       //Стоит ли игрок на движущейся платформе?
    public bool onGround;               //Стоит ли игрок на земле?

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.CompareTag("mPlatform")){
            onMovingPlatform = true;
            currentPlatform = Other.gameObject;
        }
        if (Other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            onGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if (Other.CompareTag("mPlatform")){
            onMovingPlatform = false;
            currentPlatform = null;
        }

        if (Other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            onGround = false;
        }
    }
}
