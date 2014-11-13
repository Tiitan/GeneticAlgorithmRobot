using Moda;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GeneticAlgorithmRobot
{
    class RobotManager
    {
        private List<Robot>     listRobot = new List<Robot>();
        private Stack<Robot>    availableRobot = new Stack<Robot>();
        private Mutex           mutex = new Mutex();

        public int Init()
        {
            Connection connection = new Connection(true);
            for (int i = 0; connection.Connect("localhost", 13000 + i, true); i++)
            {
                Console.WriteLine("Connection etablished to Moda Server , on port : " + (13000 + i));
                RobotPHX robotPHX = connection.QueryRobotPHX("/blli");
                if (robotPHX != null)
                {
                    Robot robot = new Robot(robotPHX, "p0");
                    mutex.WaitOne();
                    availableRobot.Push(robot);
                    listRobot.Add(robot);
                    mutex.ReleaseMutex();
                }
                else
                    Console.WriteLine("Error while retreiving robot, skiping");
                connection = new Connection(true);
            }
            return listRobot.Count;
        }

        public Robot TryGetAvailableRobot()
        {
            mutex.WaitOne();
            Robot outRobot = null;
            if (availableRobot.Count > 0)
                outRobot = availableRobot.Pop();
            mutex.ReleaseMutex();
            return outRobot;
        }
        public Robot WaitGetAvailableRobot()
        {
            Robot outRobot = null;
            while (true)
            {
                mutex.WaitOne();
                if (availableRobot.Count != 0 || listRobot.Count == 0)
                    break;
                mutex.ReleaseMutex();
                Thread.Sleep(50);
            }
            if (availableRobot.Count != 0)
                outRobot = availableRobot.Pop();
            mutex.ReleaseMutex();
            return outRobot;
        }

        public void ReleaseRobot(Robot robot)
        {
            Debug.Assert(robot != null);
            mutex.WaitOne();
            availableRobot.Push(robot);
            mutex.ReleaseMutex();
        }
    }
}
