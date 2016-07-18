using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using JNIAssist;
using AccStuff;

public class InterpMove : MonoBehaviour {
	private InitJNI jinit;
	private LinearAcceleration linacc;
	private Vector3 velocity;
	private Vector3 newpos;
	private GameObject capsule;
	private bool move;
	private float timer;

	// Use this for initialization
	void Start () {
		jinit = new InitJNI();
		linacc = new LinearAcceleration(jinit.getContext());
		velocity = new Vector3(0, 0, 0);
		newpos = new Vector3(0, 0, 0);
		capsule = GameObject.Find("Capsule");
		move = false;
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
			move = !move;
		if(Input.GetButtonDown("Fire2"))
			SceneManager.LoadScene(0);
		if(move) {
			timer += Time.deltaTime;
			if(timer <= 0.25) {
				velocity += linacc.accelerationVec() * Time.deltaTime;
			}
			else {
				newpos = newpos + velocity;
				velocity = new Vector3(0, 0, 0);
				timer = 0;
			}
			capsule.transform.position = Vector3.Lerp(capsule.transform.position, newpos, 0.02f);
		}
	}
}
