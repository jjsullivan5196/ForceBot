using UnityEngine;
using System.Collections;
using System;

namespace AccStuff {
	public class LinearAcceleration {
		private AndroidJavaObject sensor;

		public LinearAcceleration(AndroidJavaObject context) {
			sensor = new AndroidJavaObject("hci.csumb.edu.usensors.linearAcceleration", context);
		}

		public Vector3 accelerationVec() {
			float[] acc = sensor.Call<float[]>("getAcceleration");
			return new Vector3(acc[1], acc[0], acc[2]);
		}

		public float[] accelerationRaw() {
			float[] acc = sensor.Call<float[]>("getAcceleration");
			return new float[] {acc[1], acc[0], acc[2]};
		}
	}

	public class rotationVector {
		private AndroidJavaObject sensor;

		public rotationVector(AndroidJavaObject context) {
			sensor = new AndroidJavaObject("hci.csumb.edu.usensors.rotationVector", context);
		}

		public Quaternion rotationVec() {
			float[] rot = sensor.Call<float[]>("getRotation");
			return new Quaternion(rot[1], rot[0], rot[2], rot[3]);
		}

		public float[] accelerationRaw() {
			float[] rot = sensor.Call<float[]>("getRotation");
			return new float[] {rot[1], rot[0], rot[2], rot[3]};
		}
	}

	public class kalman_filter {
		private float q, r, x, p, k;

		public kalman_filter(float q, float r, float p, float init_val) {
			this.q = q;
			this.r = r;
			this.p = p;
			this.x = init_val;
		}

		public kalman_filter() {
			this.q = 0.1f;
			this.r = 0.1f;
			this.p = 0.1f;
			this.x = 0.1f;
		}

		public float update(float m) {
			//predict
			p = p + q;

			//update
			k = p / (p + r);
			x = x + k * (m - x);
			p = (1 - k) * p;

			return x;
		}

		public float getVal() {
			return x;
		}
	}

	public class filtered_acceleration : LinearAcceleration {
		private kalman_filter[] facc;

		public filtered_acceleration(AndroidJavaObject context) : base(context) {
			facc = new kalman_filter[] {new kalman_filter(), new kalman_filter(), new kalman_filter()};
		}

		public Vector3 update() {
			float[] newAcc = accelerationRaw();
			return new Vector3(facc[0].update(newAcc[0]),facc[1].update(newAcc[1]),facc[2].update(newAcc[2]));
		}

		public Vector3 acceleration() {
			return new Vector3(facc[0].getVal(),facc[1].getVal(),facc[2].getVal());
		}
	}
		
}