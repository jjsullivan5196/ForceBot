using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JNIAssist;
using AccStuff;

public class Direction : MonoBehaviour {
	private InitJNI jinit;
	private LinearAcceleration lacc;
	private TextMesh debug;
	private GameObject arrow;
	private List<Vector3> history;
	private Vector3 constPos;

	// Use this for initialization
	void Start () {
		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		lacc = new LinearAcceleration(jinit.getContext());
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		arrow = GameObject.Find("ArrowPoint");
		debug.text = "GET PSYCHED!";
		history = new List<Vector3>();
		constPos = transform.forward * 15;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 acc = lacc.accelerationVec();
		if(history.Count >= 5) {
			Vector3 facc = (history[0] + (history[1] * 2) + (history[2] * 3) + (history[3] * 2) + history[4])/9;
			arrow.transform.rotation = Quaternion.LookRotation(new Vector3(facc.x, 0, facc.z).normalized);
			history.RemoveAt(0);
			history.Add(acc);
		}
		else
			history.Add(acc);
	}
}
