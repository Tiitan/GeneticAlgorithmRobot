using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmRobot
{
    class Action
    {
        const int minShoudlerPosition = 250;
        const int maxShoudlerPosition = 790;

        const int minArmPosition = 200;
        const int maxArmPosition = 750;

        const int minFingerPosition = 400;
        const int maxFingerPosition = 1024;

        const int minDelayPosition = 30;
        const int maxDelayPosition = 300;

        public int shoudlerPosition;
        public int armPosition;
        public int fingerPosision;
        public int delay;

        static Random random = new Random();

        public Action(int shoudlerPosition, int armPosition, int fingerPosisiotn, int delay)
        {
            this.shoudlerPosition = shoudlerPosition;
            this.armPosition = armPosition;
            this.fingerPosision = fingerPosisiotn;
            this.delay = delay;
        }

        public static Action getRandomAction()
        {
            return new Action(getRandomRange(minShoudlerPosition, maxShoudlerPosition),
                              getRandomRange(minArmPosition, maxArmPosition),
                              getRandomRange(minFingerPosition, maxFingerPosition),
                              getRandomRange(minDelayPosition, maxDelayPosition));
        }

        public static int getRandomRange(int min, int max)
        {
            return random.Next(max - min) + min;
        }

        public override string ToString()
        {
            return "Shoulder: " + shoudlerPosition +
                ",\t Arm: " + armPosition +
                ",\t Finger: " + fingerPosision +
                ",\t Delay: " + delay;
        }
    }
}
