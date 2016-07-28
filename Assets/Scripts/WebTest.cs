using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using SimpleWebServer;

public class WebTest : MonoBehaviour {
	private WebServer ws;

	public static string Response(HttpListenerRequest request) {
		string name = request.QueryString["name"];
		return "<html><body>YOUR NAME IS " + name + "</body></html>";
	}

	// Use this for initialization
	void Start () {
		IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
		string localIP = "";

		foreach(IPAddress ip in localIPs) {
			if(ip.ToString().Contains("192.168."))
				localIP = ip.ToString();
		}

		ws = new WebServer(Response, "http://" + localIP + ":8080/remote/");
		ws.Run();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
