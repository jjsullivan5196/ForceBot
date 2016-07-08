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

		activity = new RecognizerDTW[3];
		input = new List<tPoint>[3];
		for(int i = 0; i < RecognizerDTW.DATA_T; i++) {
			activity[i] = new RecognizerDTW(dlact.text, i);
			input[i] = new List<tPoint>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		string temp = "";
		//Update
		float[] acc = linacc.accelerationRaw();
		for(int i = 0; i < RecognizerDTW.DATA_T; i++)
			input[i].Add(new tPoint(acc[i], timeElapsed));

		//Compare
		for(int i = 0; i < RecognizerDTW.DATA_T; i++) {
			double score = activity[i].DTWDistance(input[i]);
			temp += string.Format("{0}: {1:0.0000}\n", i, score);

			//Trim
			if(score < 1)
				input[i].Clear();
			if(score > 5)
				input[i].RemoveAt(0);
		}

		debug.text = temp;
	}
}
