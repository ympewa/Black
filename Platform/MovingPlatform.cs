using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    private bool comeBack = false;


    [Header("PLATFORM SETTING")]
    public bool move = true;
    public bool lerp = false;
    public float speed = 1;

    public GameObject platform;
    public Transform[] border;
    private Transform currentPosition;
    private Transform nextPosition;
    private int index = 0;


	// Use this for initialization
	void Awake () {
	if(platform == null || border == null)
        {
            Debug.Log("ERROR! SOME OBJECT IN MOVING PLATFORM MISSING!");
        }   
    }
	
	// Update is called once per frame

    void FixedUpdate()
    {
        if (move)
        {
            StartCoroutine(MovePlatform());
        }
    }


    IEnumerator MovePlatform()
    {
        nextPosition = border[index];

        if (platform.transform.localPosition != nextPosition.localPosition)
        {
            if (lerp)
            {
                platform.transform.localPosition = Vector2.Lerp(platform.transform.localPosition, nextPosition.localPosition, 1f * speed*Time.deltaTime);
            }
            else
            {
                platform.transform.localPosition = Vector2.MoveTowards(platform.transform.localPosition, nextPosition.localPosition, 1f * speed*Time.deltaTime);
            }
        }
        else if (platform.transform.localPosition == nextPosition.localPosition)
        {
            yield return StartCoroutine(Position());
        }
    }

    IEnumerator Position()
    {
        if (!comeBack)
        {
            if (index < border.Length - 1)
            {
                index++;
            }
            else
            {
                comeBack = true;
            }
        }

        if (comeBack)
        {
            if (index > 0)
            {
                index--;
            }
            else
            {
                comeBack = false;
            }
        }

        yield return null;
    }

}
