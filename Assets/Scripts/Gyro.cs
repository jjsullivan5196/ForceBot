using UnityEngine;
using System.Collections;
using JNIAssist;
using AccStuff;

public class Gyro : MonoBehaviour {
	private TextMesh debug;
	private InitJNI jinit;
	private rotationVector rot;

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		jinit = new InitJNI();
		rot = new rotationVector(jinit.getContext());
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = rot.rotationVec().eulerAngles.ToString();
	}
}
