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

public class rotationVector implements SensorEventListener {
    private SensorManager mSensorManager;
    private Sensor linAcc;
    private Context context;
    private float[] rotation;

    public rotationVector(Context context) {
        this.context = context;
        rotation = new float[4];
        mSensorManager = (SensorManager) context.getSystemService(Context.SENSOR_SERVICE);
        linAcc = mSensorManager.getDefaultSensor(Sensor.TYPE_ROTATION_VECTOR);
        mSensorManager.registerListener(this, linAcc, SensorManager.SENSOR_DELAY_GAME);
    }

    @Override
    public final void onSensorChanged(SensorEvent event) {
        rotation[0] = event.values[0];
        rotation[1] = event.values[1];
        rotation[2] = event.values[2];
        rotation[3] = event.values[3];
    }

    @Override
    public final void onAccuracyChanged(Sensor sensor, int accuracy) {

    }

    public float[] getRotation() {
        return rotation;
    }

}
