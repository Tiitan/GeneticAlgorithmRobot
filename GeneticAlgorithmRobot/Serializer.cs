using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmRobot
{
    class Serializer
    {
        const string FILE_NAME = "lastPopulation.txt";
        static public void Save(List<Individual> population)
        {
            if (File.Exists(FILE_NAME))
                File.Delete(FILE_NAME);

            using (StreamWriter file = File.AppendText(FILE_NAME))
            {
                foreach (Individual individual in population)
                    file.WriteLine(individual.Serialize());
            }
        }

        internal static List<Individual> Load()
        {
            List<Individual> output = new List<Individual>();

            using (StreamReader file = new StreamReader("c:\\test.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    output.Add(Individual.Deserialize(line));
                }
            }

            return output;
        }
    }
}
