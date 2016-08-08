using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JNIAssist;
using FricDTW;
using AccStuff;

public class StupidityTwo : MonoBehaviour {
	private InitJNI jinit;
	private LinearAcceleration lacc;
	private TextMesh debug;
	private List<double> input;

	bool start = false;
	int min_index = 0;
	int old_min_index = 0;
	int timeout = 0;

	int threshold = -1;

	// Use this for initialization
	void Start () {
		jinit = new InitJNI();
		lacc = new LinearAcceleration(jinit.getContext());
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		input = new List<double>();
	}
	
	// Update is called once per frame
	void Update () {
		float[] acc = lacc.accelerationRaw();
		input.Add(acc[RecognizerDTW.DATA_Y]);

		if(acc[RecognizerDTW.DATA_Y] < threshold && timeout <= 0) {
			debug.text = "VALLEY";
			start = true;
			if(input[min_index] < acc[RecognizerDTW.DATA_Y]) {
				min_index = input.Count - 1;
			}
		}

		if(acc[RecognizerDTW.DATA_Y] >= threshold && start) {
			debug.text = "OUT VALLEY";
			if(input[old_min_index] < 0) {
				GetComponent<AudioSource>().Play();
			}
			timeout = 15;
			start = false;
			old_min_index = min_index;
		}

		timeout--;
	}
}
