using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FricDTW;
using JNIAssist;
using AccStuff;

public class RecognizeLocal : MonoBehaviour {
	private TextMesh debug;
	private InitJNI jinit;
	private LinearAcceleration linacc;
	private List<double> input;
	private RecognizerDTW[] acts;
	private double[] scores;
	private double thres;

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
		input = new List<double>();

		for(int i = 0; i < ACT_MAX; i++) {
			TextAsset train = Resources.Load("training/" + action_names[i]) as TextAsset;
			acts[i] = new RecognizerDTW(train.text, RecognizerDTW.DATA_Y);
			scores[i] = Single.MaxValue;
		}

		timeElapsed = 0;
		thres = 1;
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = "ACTS:\n";
		timeElapsed += Time.deltaTime;
		float[] acc = linacc.accelerationRaw();

		input.Add(acc[RecognizerDTW.DATA_Y]);

		for(int i = 0; i < ACT_MAX; i++) {
			if((input.Max() >= acts[i].Max - thres && input.Max() <= acts[i].Max + thres) && (input.Min() >= acts[i].Min - thres && input.Min() <= acts[i].Min + thres)) {
				scores[i] = acts[i].DTWDistance(input);
				debug.text += string.Format("{0}: {1:0.0000}\n", action_names[i], scores[i]);
			}
			if((scores[i] < 1 || scores[i] > 8) && scores[i] != Single.MaxValue) {
				input.Clear();

			}
		}
	}
}