using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace GeneticAlgorithmRobot
{
    class PlotDisplay
    {
        const string FILE_NAME = "GenAlgoRobotData.txt";

        Process gnuplot = new Process();
        StreamWriter gnupStWr;

        public void Init(bool isLoaded)
        {
            if (/*!isLoaded &&*/ File.Exists(FILE_NAME))
                File.Delete(FILE_NAME);

            gnuplot.StartInfo.FileName = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
            gnuplot.StartInfo.UseShellExecute = false;
            gnuplot.StartInfo.RedirectStandardInput = true;
            try
            {
                gnuplot.Start();
                gnupStWr = gnuplot.StandardInput;
            }
            catch
            {

            }
        }

        ~PlotDisplay()
        {
            if (!gnuplot.HasExited)
                gnuplot.Close();
            gnuplot.Dispose();
        }

        public void Refresh()
        {
            try
            {
                gnupStWr.WriteLine("plot \"" + FILE_NAME + "\" u 1:2 w lines, \"" + FILE_NAME + "\" u 1:3 w lines");
            }
            catch
            {

            }
        }
        public void LogData(int generation, List<Individual> population)
        {
            double average = 0;
            foreach (Individual individual in population)
                average += individual.DistanceMoved;
            average /= population.Count;

            using (StreamWriter file = File.AppendText(FILE_NAME))
            {
                file.WriteLine(generation + " " + population[0].DistanceMoved + " " + average);
            }
        }
    }
}
