using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class Instruction
    {
        public Instruction(string line)
        {
            string[] nameAndArg = line.Split(' ');
            name = nameAndArg[0];
            arg = Int32.Parse(nameAndArg[1]);
        }

        public string name;
        public int arg;
    }

    class IntComputer
    {
        public List<Instruction> instructions;
        public List<int> executionCounters;
        public int currentInstruction;
        public int globalValue;
        public IntComputer(string code)
        {
            globalValue = 0;
            currentInstruction = 0;

            instructions = new List<Instruction>();
            executionCounters = new List<int>();
            string[] lines = code.Split('\n');
            foreach(string line in lines)
            {
                instructions.Add(new Instruction(line));
                executionCounters.Add(0);
            }
        }

        public void Reset()
        {
            currentInstruction = 0;
            globalValue = 0;
            for (int i = 0; i < executionCounters.Count; i++)
                executionCounters[i] = 0;
        }
        public bool Step()
        {
            Instruction instruction = instructions[currentInstruction];

            switch(instruction.name)
            {
                case "acc":
                    globalValue += instruction.arg;
                    executionCounters[currentInstruction]++;
                    currentInstruction++;
                    break;
                case "jmp":
                    executionCounters[currentInstruction]++;
                    currentInstruction += instruction.arg;
                    break;
                case "nop":
                    executionCounters[currentInstruction]++;
                    currentInstruction++;
                    break;
            }

            return currentInstruction == instructions.Count;
        }

        public int Run(int maxSteps)
        {
            for (int i = 0; i < maxSteps; i++)
            {
                if (Step())
                    return i;
            }

            return maxSteps;
        }
    }
}
