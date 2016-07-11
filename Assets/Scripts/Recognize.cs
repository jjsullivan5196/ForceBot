using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FricDTW;
using JNIAssist;
using AccStuff;

public class Recognize : MonoBehaviour {
	private TextMesh debug;
	private InitJNI jinit;
	private LinearAcceleration linacc;
	private RecognizerDTW[] activity;
	private List<tPoint>[] input;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		WWW dlact = new WWW("http://10.12.174.48/data/training/step.csv");
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		debug.text = dlact.bytesDownloaded.ToString();
		
		timeElapsed = 0;

		jinit = new InitJNI();
		linacc = new LinearAcceleration(jinit.getContext());

		//activity = new RecognizerDTW(dlact.text, RecognizerDTW.DATA_Y);
		activity = new RecognizerDTW[RecognizerDTW.DATA_T];
		input = new List<tPoint>[RecognizerDTW.DATA_T];

		for(int i = 0; i < RecognizerDTW.DATA_T; i++) {
			activity[i] = new RecognizerDTW(dlact.text, i);
			input[i] = new List<tPoint>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//debug.text = "";
		timeElapsed += Time.deltaTime;

		//Update
		float[] acc = linacc.accelerationRaw();
		for(int i = 0; i < RecognizerDTW.DATA_T; i++)
			input[i].Add(new tPoint(acc[i], timeElapsed));

		//Compare
		for(int i = 0; i < RecognizerDTW.DATA_T; i++) {
			if(input[i].Count >= 3) {
				double score = activity[i].DTWDistance(input[i]);
				//debug.text += string.Format("{0}: {1:0.0000}\n", i, score);

				//Trim
				if(Mathf.Abs((float)score) > 100 || input[i].Count > 50)
					input[i].Clear();
			}
		}
	}
}
