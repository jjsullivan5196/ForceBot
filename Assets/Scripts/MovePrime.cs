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
	private kalman_filter xacc;
	private kalman_filter yacc;
	private kalman_filter zacc;

	private Vector3 velocity;

	private bool start;

	// Use this for initialization
	void Start () {
		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		move = GameObject.Find("Capsule");
		capsule = move.GetComponent<Rigidbody>();
		capsule.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();

		lacc = new LinearAcceleration(jinit.getContext());
		xacc = new kalman_filter(0.1f, 2.8f, 0.1f, 0.0f);
		yacc = new kalman_filter(0.1f, 2.8f, 0.1f, 0.0f);
		zacc = new kalman_filter(0.1f, 2.8f, 0.1f, 0.0f);

		velocity  = new Vector3(0, 0, 0);

		start = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire2"))
			start = !start;
		if(Input.GetButtonDown("Fire1"))
			SceneManager.LoadScene(0);

		float[] acc = lacc.accelerationRaw();
		Vector3 acc_filtered = new Vector3(xacc.update(acc[0]),yacc.update(acc[1]),zacc.update(acc[2]));
		debug.text = string.Format("X: {0:0.0000}\nY: {1:0.0000}\nZ: {2:0.0000}", acc_filtered.x, acc_filtered.y, acc_filtered.z);

		if(start) capsule.velocity = -velocity;
		else capsule.velocity = new Vector3(0, 0, 0);

		velocity = velocity + (acc_filtered * 10 * Time.deltaTime);
	}
}
