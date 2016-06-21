using UnityEngine;
using System.Collections;
using System;

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
			acc = new Vector3(Math.Abs(acc.x) > 0 ? acc.x : 0, Math.Abs(acc.y) > 0 ? acc.y : 0, Math.Abs(acc.z) > 0 ? acc.z : 0);
			velocity = velocity + (((-acceleration) /*- (-lowBound)*/) * 10 * time); //Discrete time interval integration of acceleration
			velocity = new Vector3(Math.Abs(acc.x) > 0 ? velocity.x : 0, Math.Abs(acc.y) > 0 ? velocity.y : 0, Math.Abs(acc.z) > 0 ? velocity.z : 0);
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