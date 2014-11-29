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
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(robotManager);
            if (robotManager.GetRobots() > 0)
                geneticAlgorithm.Execute();
            Console.ReadKey();
        }
    }
}
