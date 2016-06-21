using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using JNIAssist;
using AccStuff;

public class Move : MonoBehaviour {
	private Rigidbody capsule;
	private TextMesh debug;
	private GameObject move;
	private AndroidJavaObject linacc;
	private InitJNI jinit;
	private physObj walk;
	private bool start;
	private float[] rawAcc;



	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		walk = new physObj();
		capsule.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
		debug.text = "HELLO!";
		start = false;
		rawAcc = new float[] {0, 0, 0};

		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		linacc = new AndroidJavaObject("hci.csumb.edu.usensors.linearAcceleration", jinit.getContext());
	}
	
	// Update is called once per frame
	void Update () {
		move.transform.rotation.eulerAngles.Set(move.transform.rotation.eulerAngles.x, this.transform.eulerAngles.y, move.transform.eulerAngles.z); //Lock y rotation of rigidbody to camera

		//Reset
		if(Input.GetButtonDown("Fire1")) {
			SceneManager.LoadScene(0);
			walk.reset();
		}
		if(Input.GetButtonUp("Fire1")) {
			walk.setLowBound(linacc.Call<float[]>("getAcceleration"));
		}

		//Start moving
		if(Input.GetButtonDown("Fire2")) {
			start = !start;
		}

		//Current velocity
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}\n MOVE: {3}", rawAcc[1], rawAcc[0], rawAcc[2], (start ? "TRUE" : "FALSE"));

		//Set new velocity
		if(start) capsule.velocity = walk.getVelocity();
		else capsule.velocity = new Vector3(0, 0, 0);

		//Priors for next frame
		rawAcc = linacc.Call<float[]>("getAcceleration");
		Vector3 acc = new Vector3(rawAcc[1], rawAcc[0], rawAcc[2]);
		walk.calcVelocity(acc, Time.deltaTime);
	}
}
