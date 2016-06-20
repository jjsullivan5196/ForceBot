using UnityEngine;
using System;
using System.Collections;
using System.IO;
using JNIAssist;
using AccStuff;

public class Record : MonoBehaviour {
	private AndroidJavaObject linacc;
	private InitJNI jinit;
	private TextMesh debug;
	private MemoryStream upload;
	private StreamWriter fsUpload;
	private bool record;
	private bool data_captured;
	private string uploadURL;

	public IEnumerator sendData(WWW sendTo) {
		yield return sendTo;
	}

	// Use this for initialization
	void Start () {
		AndroidJNI.AttachCurrentThread();
		jinit = new InitJNI();
		linacc = new AndroidJavaObject("hci.csumb.edu.usensors.linearAcceleration", jinit.getContext());

		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		debug.text = "NOT RECORD";

		upload = new MemoryStream();
		fsUpload = new StreamWriter(upload);
		record = false;
		data_captured = false;

		uploadURL = "http://10.12.174.79/upload.php";
	}
	
	// Update is called once per frame
	void Update () {
		if(!record && !(upload.Length > 0))
			debug.text = "NOT RECORD";
		else {
			if(!record && data_captured) {
				byte[] accBytes = upload.ToArray();
				WWWForm sendAcc = new WWWForm();
				sendAcc.AddField("frameCount", Time.frameCount.ToString());
				string datestring = DateTime.Now.ToString("MM-dd-yy H-mm-ss");
				sendAcc.AddBinaryData("file", accBytes, "acc" + datestring + ".csv", "text/plain");
				WWW uploadAcc = new WWW(uploadURL, sendAcc);
				sendData(uploadAcc);
				fsUpload.Flush();
				upload.SetLength(0);
				data_captured = false;
			}
			else {
				debug.text = "RECORD";
				float[] acc = linacc.Call<float[]>("getAcceleration");
				fsUpload.WriteLine(string.Format("{0:0.0000},{1:0.0000},{2:0.0000}", acc[1], acc[0], acc[2]));
				data_captured = true;
			}
		}
		if(Input.GetButtonDown("Fire1"))
			record = !record;
	}
}
