using UnityEngine;
using System;
using System.Collections;
using System.IO;
using JNIAssist;
using AccStuff;

public class Record : MonoBehaviour {
	private LinearAcceleration linacc;
	private InitJNI jinit;
	private TextMesh debug;
	private MemoryStream upload;
	private StreamWriter fsUpload;
	private bool record;
	private string uploadURL;
	private float timeElapsed;

	public IEnumerator sendData(WWW sendTo) {
		yield return sendTo;
	}

	// Use this for initialization
	void Start () {
		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		linacc = new LinearAcceleration(jinit.getContext());

		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		debug.text = "NOT RECORD";

		upload = new MemoryStream();
		fsUpload = new StreamWriter(upload);
		record = false;
		uploadURL = "http://10.12.174.48/upload.php";

		timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			record = !record;
			debug.text = "NOT RECORD";
			GetComponent<AudioSource>().pitch = record ? 2 : 1;
			GetComponent<AudioSource>().Play();

			if(record) {
				fsUpload.Flush();
				upload.SetLength(0);
				timeElapsed = 0;
			}
		}
		if(Input.GetButtonDown("Fire2")) {
			byte[] accBytes = upload.ToArray();
			WWWForm sendAcc = new WWWForm();
			sendAcc.AddField("frameCount", Time.frameCount.ToString());
			string datestring = DateTime.Now.ToString("MM-dd-yy_H-mm-ss");
			sendAcc.AddBinaryData("file", accBytes, "acc" + datestring + ".csv", "text/plain");
			WWW uploadAcc = new WWW(uploadURL, sendAcc);
			sendData(uploadAcc);

			fsUpload.Flush();
			upload.SetLength(0);
			timeElapsed = 0;

			GetComponent<AudioSource>().pitch = 3;
			GetComponent<AudioSource>().Play();
		}
		if(record) {
			debug.text = "RECORD";
			float[] acc = linacc.accelerationRaw();
			timeElapsed += Time.deltaTime;
			fsUpload.WriteLine(string.Format("{0:0.0000},{1:0.0000},{2:0.0000},{3:0.0000}", acc[0], acc[1], acc[2], timeElapsed));
		}
	}
}
