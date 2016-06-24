using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	private TextMesh debug;
	private WebCamDevice[] devices;
	private WebCamTexture phoneCam;
	private GameObject camPlane;

	// Use this for initialization
	void Start () {
		debug = GameObject.Find("Debug").GetComponent<TextMesh>();
		devices = WebCamTexture.devices;
		phoneCam = new WebCamTexture();
		camPlane = GameObject.Find("phoneCamera");
		camPlane.GetComponent<MeshRenderer>().material.mainTexture = phoneCam;
		phoneCam.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}