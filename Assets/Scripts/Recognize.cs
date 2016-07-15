using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FricDTW;
using JNIAssist;
using AccStuff;

public class Recognize : MonoBehaviour {
	private TextMesh debug;
	private InitJNI jinit;
	private LinearAcceleration linacc;
	private List<tPoint> input;
	private RecognizerDTW[] acts;
	private double[] scores;

	private float timeElapsed;
	private const int ACT_STEP = 0;
	private const int ACT_STAND = 1;
	private const int ACT_SIT = 2;
	private const int ACT_JUMP = 3;
	private const int ACT_FLAT = 4;
	private const int ACT_MAX = 5;

	private string[] action_names = {
		"step",
		"standup",
		"sitdown",
		"jump",
		"flatline"
	};

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		jinit = new InitJNI();
		linacc = new LinearAcceleration(jinit.getContext());
		acts = new RecognizerDTW[ACT_MAX];
		scores = new double[ACT_MAX];
		input = new List<tPoint>();

		string dataurl = "http://10.12.174.214/data/training/";
		for(int i = 0; i < ACT_MAX; i++) {
			WWW dlact = new WWW(dataurl + action_names[i] + ".csv");
			debug.text = dlact.bytesDownloaded.ToString();
			acts[i] = new RecognizerDTW(dlact.text, RecognizerDTW.DATA_Y);
			scores[i] = 0;
		}

		timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		float[] acc = linacc.accelerationRaw();

		input.Add(new tPoint(acc[RecognizerDTW.DATA_Y], timeElapsed));

		if(input.Count >= 3) {
			for(int i = 0; i < ACT_MAX; i++)
				scores[i] = acts[i].DTWDistanceWindow(input, 2);

			debug.text = action_names[Array.IndexOf(scores, scores.Min())];

			if(input.Count > 35)
				input.RemoveAt(0);
		}
	}
}