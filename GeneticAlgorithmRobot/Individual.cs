using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GeneticAlgorithmRobot
{
    class Individual : IComparable
    {
        const double SPEED_MULTIPLIER = 1.0f;
        const int minSequence = 2;
        const int maxSequence = 5;
        const double evaluationDuration = 20;

        private List<Action> sequence = new List<Action>();

        public int IndividualID { get; private set; }
        private static int currentIndividualID = 0;

        private RobotManager robotManager;

        private double bestDistanceMoved = -1;
        private double distanceMoved = -1;
        public double DistanceMoved { get { return distanceMoved; } }
        public double BestDistanceMoved { get { return bestDistanceMoved; } }

        private static Random random = new Random();

        internal static Individual GenerateRandom(RobotManager robotManager)
        {
            Individual outIndividiual = new Individual(robotManager);
            int sequenceSize = Action.getRandomRange(minSequence, maxSequence);
            for (int n = 0; n < sequenceSize; ++n)
                outIndividiual.sequence.Add(Action.getRandomAction());
            return outIndividiual;

        }

        internal static Individual GenerateFromParents(Individual a, Individual b)
        {
            Debug.Assert(a.sequence.Count > 0);
            Debug.Assert(b.sequence.Count > 0);

            Individual newIndividiual = new Individual(a.robotManager);

            // Swap random 2
            if (random.Next(2) == 1)
            {
                Individual tmp = a;
                a = b;
                b = tmp;
            }

            // Merge sequence
            int splitPositionA = random.Next(a.sequence.Count);
            for (int i = 0; i < splitPositionA; i++)
                newIndividiual.sequence.Add(a.sequence[i]);
            int splitPositionB = random.Next(b.sequence.Count);
            for (int i = splitPositionB; i < b.sequence.Count; i++)
                newIndividiual.sequence.Add(b.sequence[i]);

            return newIndividiual;
        }

        internal static Individual GenerateFromMutation(Individual a)
        {
            Debug.Assert(a.sequence.Count > 0);

            Individual newIndividiual = new Individual(a.robotManager);
            newIndividiual.sequence = a.sequence;
            newIndividiual.sequence[random.Next(newIndividiual.sequence.Count)] = Action.getRandomAction();
            return newIndividiual;
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

            //Log("Evaluation requested, waiting for an aviable robot...");
            Robot robot = robotManager.WaitGetAvailableRobot();
            if (robot != null)
            {
                //Log("Available robot found, evaluation started...");
                robot.Reset();
                Moda.Vector3 initialPosition = robot.GetPosition();
                Thread.Sleep((int)(1000 / SPEED_MULTIPLIER));

                while (robot.GetTime() < evaluationDuration)
                {
                    //Log("Sequence start:");
                    foreach (Action action in sequence)
                    {
                        //Log(action.ToString());
                        robot.Move(ServoType.SHOULDER, action.shoudlerPosition, 10);
                        robot.Move(ServoType.ARM, action.armPosition, 10);
                        robot.Move(ServoType.FINGER, action.fingerPosision, 10);
                        Thread.Sleep((int)(action.delay / SPEED_MULTIPLIER));
                        if (robot.GetTime() > evaluationDuration)
                            break;
                    }
                }
                distanceMoved = -(initialPosition.X - robot.GetPosition().X);
                if (distanceMoved > bestDistanceMoved)
                    bestDistanceMoved = distanceMoved;

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

        public int CompareTo(object obj)
        {
            if (((Individual)obj).distanceMoved < this.distanceMoved)
                return -1;
            if (((Individual)obj).distanceMoved > this.distanceMoved)
                return 1;
            return 0;
        }

        public string Serialize()
        {
            string output = "";
            foreach (Action action in sequence)
                output += action.Serialize() + " ";
            return output;
        }

        internal static Individual Deserialize(RobotManager robotManager, string line)
        {
            Individual output = new Individual(robotManager);
            string[] stringActionArray = line.Split(' ');
            foreach (string stringAction in stringActionArray)
            {
                Action a = Action.Deserialize(stringAction);
                if (a != null)
                    output.sequence.Add(a);
                //else
                    //Console.WriteLine("Load action error.");
            }
            return output;
        }
    }
}
