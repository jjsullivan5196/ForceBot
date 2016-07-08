using UnityEngine;
using System.Collections;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

public class ConnectServer : MonoBehaviour {
	private SftpClient cli;
	private TextMesh debug;

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		debug.text = "NOT DEFAULT";
		try {
			cli = new SftpClient("10.12.174.48", 22, "poser", "poser");
			cli.Connect();
			debug.text = "CONNECT OK";
		}
		catch(System.Exception e) {
			Debug.Log(e.ToString());
			debug.text = e.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
