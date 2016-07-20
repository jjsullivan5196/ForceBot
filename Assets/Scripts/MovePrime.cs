using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using JNIAssist;
using AccStuff;

public class MovePrime : MonoBehaviour {
	private InitJNI jinit;
	private GameObject move;
	private Rigidbody capsule;
	private TextMesh debug;

	private LinearAcceleration lacc;

	private Vector3 velocity;
	private Vector3 zero;

	private bool start;

	// Use this for initialization
	void Start () {
		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		capsule.constraints = /*RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX |*/ RigidbodyConstraints.FreezeRotation;
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();

		lacc = new LinearAcceleration(jinit.getContext());

		velocity = new Vector3(0, 0, 0);
		zero = new Vector3(0.75f, 0.75f, 0.75f);

		start = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire2"))
			start = !start;
		if(Input.GetButtonDown("Fire1")) {
			SceneManager.LoadScene(0);
			//float[] accZero = lacc.accelerationRaw();
			//zero = new Vector3(Math.Abs(accZero[0]), Math.Abs(accZero[1]), Math.Abs(accZero[2]));
		}

		Vector3 acc = lacc.accelerationVec();
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}", acc.x, acc.y, acc.z);

		if(start) capsule.velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
		else capsule.velocity = new Vector3(0, 0, 0);

		if((Math.Abs(acc.x) > zero.x) || (Math.Abs(acc.y) > zero.y) || (Math.Abs(acc.z) > zero.z)) {
			velocity = velocity + (acc * Time.deltaTime) * 10;
			debug.text += "\nMOVE";
		}
		else
			velocity = velocity + -(velocity/(Time.deltaTime * 1000));
	}
}
