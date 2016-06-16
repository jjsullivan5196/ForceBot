package hci.csumb.edu.usensors;

/**
 * Created by student on 6/15/16.
 */

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.IBinder;
import android.support.annotation.Nullable;

public class linearAcceleration implements SensorEventListener {
    private SensorManager mSensorManager;
    private Sensor linAcc;
    private Context context;
    private float[] acceleration;

    public linearAcceleration(Context context) {
        this.context = context;
        acceleration = new float[3];
        mSensorManager = (SensorManager) context.getSystemService(Context.SENSOR_SERVICE);
        linAcc = mSensorManager.getDefaultSensor(Sensor.TYPE_LINEAR_ACCELERATION);
        mSensorManager.registerListener(this, linAcc, SensorManager.SENSOR_DELAY_GAME);
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
