using Moda;
using System.Diagnostics;

namespace GeneticAlgorithmRobot
{
    enum ServoType
    {
        FINGER = 0,
        ARM = 1,
        SHOULDER = 2
    }

    class Robot
    {
        private RobotPHX robotPHX;

        private DeviceServoMotor[] servoMotors = new DeviceServoMotor[3];
        private Geom core;
        private uint resetTime;
        private DeviceAccelGyro gyro;
        public Robot(RobotPHX robotPHX, string name)
        {
            this.robotPHX = robotPHX;
            servoMotors[(int)ServoType.FINGER] = robotPHX.QueryDeviceServoMotor(name + "_doigt/a1/servo");
            servoMotors[(int)ServoType.ARM] = robotPHX.QueryDeviceServoMotor(name + "_bras/a1/servo");
            servoMotors[(int)ServoType.SHOULDER] = robotPHX.QueryDeviceServoMotor(name + "_epaule/a1/servo");
            Debug.Assert(servoMotors[0] != null &&
                         servoMotors[1] != null &&
                         servoMotors[2] != null);

            core = robotPHX.QueryGeom("basPlatine");
            Debug.Assert(core != null);

            gyro = robotPHX.QueryDeviceAccelGyro("basPlatine/gyro");
            Debug.Assert(gyro != null);
        }
        
        public void Move(ServoType servoType, int newIndex, int speed)
        {
            servoMotors[(int)servoType].GoPositionIndex(newIndex, speed);
        }

        public void Reset()
        {
            robotPHX.GetConnection().ResetSimulation();
            resetTime = robotPHX.GetTime();
        }

        public Vector3 GetPosition()
        {
            return core.GetPosition();
        }

        public double GetTime()
        {
            uint deltaTimeMillisecond = robotPHX.GetTime() - resetTime;
            double deltaTimeSecond = ((double)deltaTimeMillisecond) / 1000f;
            return deltaTimeSecond;
        }

        public Vector3 GetAcceleration()
        {
            AXESXYZValues gyroValue = gyro.GetXYZInstantValues();
            Vector3 acceleration = new Vector3();

            acceleration.X = gyroValue.LinearAccelerations[0];
            acceleration.Y = gyroValue.LinearAccelerations[1];
            acceleration.Z = gyroValue.LinearAccelerations[2];

            return acceleration;
        }

        public Vector3 GetOrientation()
        {
            AXESXYZValues gyroValue = gyro.GetXYZInstantValues();
            Vector3 angles = new Vector3();

            angles.X = gyroValue.Angles[0];
            angles.Y = gyroValue.Angles[1];
            angles.Z = gyroValue.Angles[2];

            return angles;
        }
    }
}
