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

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		debug.text = "HELLO!";
		start = false;
		timeElapsed = 0;
		capsule.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		linacc = new AndroidJavaObject("hci.csumb.edu.usensors.linearAcceleration", jinit.getContext());
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

		Vector3 acc = Input.acceleration;// - Input.gyro.gravity;
		if(start) move.transform.position = Vector3.Lerp(move.transform.position, (move.transform.position + acc.normalized), 0.5f);

		float[] lacc = linacc.Call<float[]>("getAcceleration");
	
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}\n MOVE: {3}", lacc[0], lacc[1], lacc[2], (start ? "TRUE" : "FALSE"));
	}


}
