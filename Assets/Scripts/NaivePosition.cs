using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using JNIAssist;
using AccStuff;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using SimpleWebServer;

public class NaivePosition : MonoBehaviour {
	private InitJNI jinit;
	private LinearAcceleration linacc;
	private TextMesh debug;
	private GameObject move;
	private Vector3 vel_now;
	private Vector3 vel_pre;
	private Vector3 acc_now;
	private Vector3 acc_pre;
	private Vector3 pos_now;
	private Vector3 init_pos;
	private int frameCount;
	private float timeElapsed;

	private float magGate;
	private float scaleFrac;

	private WebServer ws;

	private bool lock_x;
	private bool lock_y;
	private bool lock_z;
	private bool mFilter;
	private string lstring;

	public string wsResponse(HttpListenerRequest request) {
		string settings = "";
		if(request.QueryString["mg"] != null) {
			magGate = float.Parse(request.QueryString["mg"]);
			settings += "Set magnitude gate to: " + magGate.ToString() + "<br>";
		}
		if(request.QueryString["sf"] != null) {
			scaleFrac = float.Parse(request.QueryString["sf"]);
			settings += "Set scale factor to: " + scaleFrac.ToString() + "<br>";
		}
		if(request.QueryString["lock"] != null) {
			lstring = request.QueryString["lock"];
			lock_x = lstring.Contains("x");
			lock_y = lstring.Contains("y");
			lock_z = lstring.Contains("z");

			settings += "Locked on axes: " + lstring + "<br>";
		}
		if(request.QueryString["mfilter"] != null) {
			mFilter = true;
			settings += "Zeroing acceleration below gate<br>";
		}
		else {
			mFilter = false;
		}
		string form = @"
			<html>
				<head>
					<title>ForceBot Remote</title>
				</head>
				<body>
					<form action=""remote"" method=""get"">
						Magnitude Gate:<br>
						<input type=""number"" step=""any"" name=""mg"" value=""{0}""><br>
						Scale Factor:<br>
						<input type=""number"" step=""any"" name=""sf"" value=""{1}""><br>
						Lock on axes:<br>
						<input type=""text"" name=""lock"" value=""{2}""><br>
						Zero acceleration values below magnitude gate: 
						<input type=""checkbox"" name=""mfilter"" value=""true""{3}><br>
						<input type=""submit"" value=""Update""><br>
					</form>
					{4}
				</body>
			</html>
		";

		return string.Format(form, magGate.ToString(), scaleFrac.ToString(), lstring, mFilter ? "checked" : "", settings);
	}

	private void wsSetup() {
		ws = new WebServer(wsResponse, "http://localhost:8080/remote/");
		ws.Run();
	}

	// Use this for initialization
	void Start () {
		jinit = new InitJNI();
		linacc = new LinearAcceleration(jinit.getContext());
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		move = GameObject.Find("Capsule");

		vel_now = new Vector3(0, 0, 0);
		vel_pre = new Vector3(0, 0, 0);
		acc_now = new Vector3(0, 0, 0);
		acc_pre = new Vector3(0, 0, 0);
		pos_now = init_pos = move.transform.position;

		frameCount = 0;
		timeElapsed = 0f;

		magGate = 0.75f;
		scaleFrac = 0.25f;

		lock_x = false;
		lock_y = true;
		lock_z = false;
		mFilter = false;
		lstring = "y";

		wsSetup();
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = 
			"ACC: " + acc_pre.ToString() + 
			"\nVEL: " + vel_pre.ToString() + 
			"\nMG: " + magGate.ToString() + 
			"\nSF: " + scaleFrac.ToString() + 
			"\nLOCK: " + lstring +
			"\nMFILTER: " + (mFilter ? "TRUE" : "FALSE");

		if(Input.GetButtonDown("Fire1")) {
			vel_now = new Vector3(0, 0, 0);
			vel_pre = new Vector3(0, 0, 0);
			acc_now = new Vector3(0, 0, 0);
			acc_pre = new Vector3(0, 0, 0);
			timeElapsed = 0f;
			frameCount = 0;
			move.transform.position = init_pos;
			pos_now = move.transform.position;
		}

		//Filter acc data
		if(timeElapsed <= 0.15f) {
			acc_now += linacc.accelerationVec();
			timeElapsed += Time.deltaTime;
			frameCount++;
		}
		else {
			acc_now = acc_now/frameCount;
			frameCount = 0;

			//Mechanical Filter
			if(mFilter) {
				acc_now = new Vector3(
					Math.Abs(acc_now.x) >= magGate ? acc_now.x : 0,
					Math.Abs(acc_now.y) >= magGate ? acc_now.y : 0,
					Math.Abs(acc_now.z) >= magGate ? acc_now.z : 0
				);
			}

			//First Integration, assume deceleration if under acc thres
			if(Math.Abs(acc_now.x) >= magGate || Math.Abs(acc_now.y) >= magGate || Math.Abs(acc_now.z) >= magGate) {
				//Scale Input
				acc_now = acc_now * scaleFrac;
				vel_now = vel_pre + acc_pre + ((acc_now - acc_pre)/2) * timeElapsed;
			}
			else {
				vel_now = vel_pre + -(vel_pre * timeElapsed);
			}

			Vector3 vel_apply_now = new Vector3(-vel_now.x, vel_now.y, -vel_now.z);
			Vector3 vel_apply_pre = new Vector3(-vel_pre.x, vel_pre.y, -vel_pre.z);

			//Second Integration
			pos_now = pos_now + vel_apply_pre + ((vel_apply_now - vel_apply_pre)/2) * timeElapsed;

			vel_pre = vel_now;
			acc_pre = acc_now;

			vel_now = new Vector3(0, 0, 0);
			acc_now = new Vector3(0, 0, 0);

			timeElapsed = 0f;
		}

		Vector3 pos_cur = move.transform.position;
		move.transform.position = Vector3.Lerp(move.transform.position, new Vector3(lock_x ? pos_cur.x : pos_now.x, lock_y ? pos_cur.y : pos_now.y, lock_z ? pos_cur.z : pos_now.z), 0.02f);
	}
}