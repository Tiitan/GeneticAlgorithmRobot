
using Moda;
using System;
using System.Threading;

namespace GeneticAlgorithmRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            RobotManager robotManager = new RobotManager();
            Console.WriteLine("Init");
            robotManager.Init();
            Console.WriteLine("TryGetAvailableRobot");
            Robot robot = robotManager.TryGetAvailableRobot();
            if (robot != null)
            {
                Console.WriteLine("------------ ROBOT ------------");
                Thread.Sleep(2000);
                Console.WriteLine("reset");
                robot.Reset();
                Thread.Sleep(2000);
                Console.WriteLine("move");
                robot.Move(ServoType.SHOULDER, 1024, 10);
            }
            else
                Console.WriteLine("robot null");
            Console.ReadKey();
        }
    }
}
