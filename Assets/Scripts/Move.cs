using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using JNIAssist;

public class Move : MonoBehaviour {
	private Rigidbody capsule;
	private TextMesh debug;
	private GameObject move;
	private bool start;
	private float timeElapsed;
	private int frameCount;
	private AndroidJavaObject linacc;
	private InitJNI jinit;
	private Vector3 oldVel;
	private Vector3 oldAcc;
	private Vector3 lowbound;

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		debug.text = "HELLO!";
		start = false;
		timeElapsed = 0;
		capsule.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		oldVel = new Vector3(0, 0, 0);
		oldAcc = new Vector3(0, 0, 0);
		lowbound = new Vector3(0, 0, 0);

		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		linacc = new AndroidJavaObject("hci.csumb.edu.usensors.linearAcceleration", jinit.getContext());
	}
	
	// Update is called once per frame
	void Update () {
		move.transform.rotation.eulerAngles.Set(move.transform.rotation.eulerAngles.x, this.transform.eulerAngles.y, move.transform.eulerAngles.z);
		timeElapsed += Time.deltaTime;
		if(Input.GetButtonDown("Fire1")) {
			SceneManager.LoadScene(0);
			lowbound = oldAcc;
		}
		if(Input.GetButtonDown("Fire2")) {
			start = !start;
		}

		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}\n MOVE: {3}", oldAcc.x, oldAcc.y, oldAcc.z, (start ? "TRUE" : "FALSE"));
		if(start) capsule.velocity = oldVel;
		else capsule.velocity = new Vector3(0, 0, 0);

		float[] lacc = linacc.Call<float[]>("getAcceleration");
		Vector3 acc = new Vector3(-lacc[1], 0, -lacc[2]);
		acc = acc - lowbound;

		oldVel = oldVel + (oldAcc * 10 * Time.deltaTime);
		oldAcc = acc;
	}
}
