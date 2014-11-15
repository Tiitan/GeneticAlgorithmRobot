using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgorithmRobot
{
    class GeneticAlgorithm
    {
        const int MAX_GENERATION = 3;
        const int POPULATION_SIZE = 3;
        
        private int generation = 0;
        private List<Individual> population = new List<Individual>();
        private ManualResetEvent[] doneEvents = new ManualResetEvent[POPULATION_SIZE];
        private RobotManager robotManager;

        public GeneticAlgorithm(RobotManager robotManager)
        {
            this.robotManager = robotManager;
            for (int i = 0; i < POPULATION_SIZE; i++)
                doneEvents[i] = new ManualResetEvent(false);
        }

        public void Execute()
        {
            Init();
            while (generation < MAX_GENERATION)
            {
                EvaluateGeneration();
                LogResults();
                NextGeneration();
                ++generation;
            }
        }

        private void NextGeneration()
        {
            List<Individual> newPopulation = new List<Individual>();

            // placeholder random new generation
            foreach (Individual individual in population)
                newPopulation.Add(Individual.GenerateRandom(robotManager));

            population = newPopulation;
        }

        private void LogResults()
        {
            double maxDistanceMoved = -1;
            double averageDistanceMoved = 0;
            foreach (Individual individual in population)
            {
                averageDistanceMoved += individual.DistanceMoved;
                if (individual.DistanceMoved > maxDistanceMoved)
                    maxDistanceMoved = individual.DistanceMoved;
            }
            averageDistanceMoved /= population.Count;
            Console.WriteLine("Generation " + generation + "| Max: " + maxDistanceMoved + ", Average: " + averageDistanceMoved + ".");
        }

        private void EvaluateGeneration()
        {
            int i = 0;
            foreach (Individual individual in population)
            {
                doneEvents[i].Reset();
                ThreadPool.QueueUserWorkItem(individual.Evaluate, doneEvents[i]);
                i++;
            }
            WaitHandle.WaitAll(doneEvents);
        }

        private void Init()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                population.Add(Individual.GenerateRandom(robotManager));
            }
        }
    }
}
