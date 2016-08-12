using UnityEngine;
using System.Collections;

public class WeaponPosition : MonoBehaviour {

 //   private Vector2 defaultPosition = new Vector2(1.5f, 0);
 //   private Vector2 position;
 //   private int direction =1;

	//// Use this for initialization
	//void Awake () {
 //       position = defaultPosition;
 //       this.transform.localPosition = position;
 //   }

 //   // Update is called once per frame
 //   void Update()
 //   {
 //       var v = (Vector2)this.transform.forward;
 //       Debug.Log(v);
 //       SetDirection();
 //       SetPosition();

 //       this.transform.localPosition = position;
 //   }

 //   private void SetDirection()
 //   {
 //       if (Input.GetAxis("Horizontal") != 0 && direction != (int)Input.GetAxis("Horizontal"))
 //       {
 //           direction = (int)Input.GetAxis("Horizontal");
 //       }
 //   }

 //   private void SetPosition()
 //   {
 //       Vector2 inputPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 100;
 //       if (inputPosition != Vector2.zero)
 //       {
 //           position = Vector2.ClampMagnitude(inputPosition, 1.5f);
 //       }
 //       else
 //       {
 //           position = new Vector2(defaultPosition.x * direction, defaultPosition.y);
 //       }
 //   }
}
