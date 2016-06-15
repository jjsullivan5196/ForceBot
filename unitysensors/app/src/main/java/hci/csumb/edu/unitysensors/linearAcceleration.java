package hci.csumb.edu.unitysensors;

/**
 * Created by student on 6/15/16.
 */

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.hardware.Sensor;
import android.os.IBinder;
import android.support.annotation.Nullable;

public class linearAcceleration extends Service implements SensorEventListener {
    private SensorManager mSensorManager;
    private Sensor linAcc;
    private float[] acceleration;

    public linearAcceleration() {
        acceleration = new float[3];
        mSensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        linAcc = mSensorManager.getDefaultSensor(Sensor.TYPE_LINEAR_ACCELERATION);
        mSensorManager.registerListener(this, linAcc, SensorManager.SENSOR_DELAY_GAME);
    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public final void onSensorChanged(SensorEvent event) {
        acceleration[0] = event.values[0];
        acceleration[1] = event.values[1];
        acceleration[2] = event.values[2];
    }

    @Override
    public final void onAccuracyChanged(Sensor sensor, int accuracy) {

    }

    public float[] getAcceleration() {
        return acceleration;
    }

}
