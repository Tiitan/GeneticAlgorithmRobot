using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GeneticAlgorithmRobot
{
    class Individual
    {
        
        public int IndividualID { get; private set; }
        private static int currentIndividualID = 0;

        private RobotManager robotManager;
        private double distanceMoved = -1;
        public double DistanceMoved { get { return distanceMoved; } }

        internal static Individual GenerateRandom(RobotManager robotManager)
        {
            return new Individual(robotManager);
        }

        private Individual(RobotManager robotManager)
        {
            IndividualID = currentIndividualID++;
            this.robotManager = robotManager;
        }

        internal void Evaluate(Object threadContext)
        {
            ManualResetEvent manualResetEvent = threadContext as ManualResetEvent;
            Debug.Assert(manualResetEvent != null);

            Log("Evaluation requested, waiting for an aviable robot...");
            Robot robot = robotManager.WaitGetAvailableRobot();
            if (robot != null)
            {
                Log("Available robot found, evaluation started...");
                robot.Reset();
                Moda.Vector3 initialPosition = robot.GetPosition();

                Thread.Sleep(5000); //placeholder robot evaluation

                distanceMoved = (initialPosition - robot.GetPosition()).Length();
                Log("Evaluation finished (" + distanceMoved + ").");

                robotManager.ReleaseRobot(robot);
            }
            else
                Log("Error: robotManager.WaitGetAvailableRobot() didn't return any robot :(");
            manualResetEvent.Set();
        }
        void Log(string message)
        {
            Console.WriteLine("Individual " + IndividualID + ": " + message);
        }
    }
}
