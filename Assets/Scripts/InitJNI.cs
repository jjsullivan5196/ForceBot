using UnityEngine;
using System.Collections;

namespace JNIAssist {

	public class InitJNI {
		private AndroidJavaClass activityClass;
		private AndroidJavaObject activityContext;

		public InitJNI() {
			activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		public AndroidJavaClass getActivity() {
			return activityClass;
		}

		public AndroidJavaObject getContext() {
			return activityContext;
		}
	}

}