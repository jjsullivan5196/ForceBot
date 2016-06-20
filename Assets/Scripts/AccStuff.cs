using UnityEngine;
using System.Collections;

namespace AccStuff {
	public class physObj {
		private Vector3 acceleration; //Linear acceleration from last frame
		private Vector3 velocity; //Estimated velocity from last frame
		private Vector3 lowBound; //Set resting acceleration

		public physObj() {
			acceleration = new Vector3(0, 0, 0);
			velocity = new Vector3(0, 0, 0);
			lowBound = new Vector3(0, 0, 0);
		}

		public void calcVelocity(Vector3 acc, float time) {
			velocity = velocity + (-acceleration * 10 * time); //Discrete time interval integration of acceleration
			acceleration = acc; //Set acceleration for next frame
		}

		public Vector3 getVelocity() {
			return velocity;
		}

		public void setLowBound(float[] rawAcc) {
			lowBound.x = rawAcc[1];
			lowBound.y = rawAcc[0];
			lowBound.z = rawAcc[2];
		}

		public void reset() {
			acceleration = new Vector3(0, 0, 0);
			velocity = new Vector3(0, 0, 0);
			lowBound = new Vector3(0, 0, 0);
		}
			
	}
}