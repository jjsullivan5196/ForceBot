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
	private Vector3 newAcc;
	private int frameCount;

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
		newAcc = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		move.transform.rotation.eulerAngles.Set(move.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, move.transform.rotation.eulerAngles.z);
		timeElapsed += Time.deltaTime;
		if(Input.GetButtonDown("Fire1")) {
			SceneManager.LoadScene(0);
		}
		if(Input.GetButtonDown("Fire2")) {
			start = !start;
		}
		if(timeElapsed >= 0.16) {
			oldAcc = newAcc / timeElapsed;
			oldAcc = new Vector3(oldAcc.x, 0, -oldAcc.z);
			newAcc = new Vector3(0, 0, 0);
			timeElapsed = 0;
			frameCount = 0;
		}

		if(start) capsule.AddForce(oldAcc * 100);
		newAcc = newAcc + (Input.acceleration - Input.gyro.gravity);
	
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}\nMOVE: {3}", Input.acceleration.x, Input.acceleration.y, Input.acceleration.z, (start ? "TRUE" : "FALSE"));
		oldAcc = Input.acceleration - Input.gyro.gravity;
	}


}
