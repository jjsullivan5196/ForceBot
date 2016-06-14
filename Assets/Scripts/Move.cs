using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Move : MonoBehaviour {
	private Rigidbody capsule;
	private TextMesh debug;
	private Vector3 lowbound;
	private GameObject move;
	private bool start;
	private float timeElapsed;
	private Vector3 oldAcc;
	private Vector3 oldVel;

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		debug.text = "HELLO!";
		lowbound = new Vector3(0, 0, 0);
		start = false;
		timeElapsed = 0;
		capsule.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		oldAcc = new Vector3(0, 0, 0);
		oldVel = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		if(Input.GetButtonDown("Fire1")) {
			SceneManager.LoadScene(0);
		}
		if(Input.GetButtonDown("Fire2")) {
			start = !start;
		}
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}\nMOVE: {3}", Input.acceleration.x, Input.acceleration.y, Input.acceleration.z, (start ? "TRUE" : "FALSE"));

		if(start) move.transform.Translate((oldVel * Time.deltaTime) + move.transform.position);

		oldVel = ((Input.acceleration - Input.gyro.gravity) - oldAcc) * Time.deltaTime;
		oldAcc = Input.acceleration - Input.gyro.gravity;

		/*if(start) {
			Vector3 trans = Input.acceleration - lowbound;
			trans = new Vector3(trans.x, 0, -trans.y);
			move.transform.Translate(move.transform.position + trans);
		}
		if(timeElapsed > 4) {
			lowbound = Input.acceleration;
			timeElapsed = 0;
		}*/
	}


}
