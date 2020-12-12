using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class Day8
    {
        IntComputer comp;

        public Day8()
        {
            comp = new IntComputer(File.ReadAllText("day8.txt"));
        }

        public void Part1()
        {
            int savedGlobalValue = 0;

            while (comp.executionCounters.IndexOf(2) == -1)
            {
                savedGlobalValue = comp.globalValue;
                comp.Step();
            }

            Console.WriteLine($"Day8 Part1 {savedGlobalValue}");
        }

        public void Part2()
        {
            bool goodRun = false;

            for (int i = 0; i < comp.instructions.Count; i++)
            {
                Instruction inst = comp.instructions[i];
                if (inst.name != "jmp" && inst.name != "nop")
                    continue;

                string oldName = inst.name;
                if (inst.name == "jmp")
                    inst.name = "nop";
                else if (inst.name == "nop")
                    inst.name = "jmp";

                while (comp.executionCounters.IndexOf(2) == -1)
                {
                    if (comp.Step())
                    {
                        goodRun = true;
                        break; 
                    }                       
                }

                if (goodRun)
                    break;
                else
                    comp.instructions[i].name = oldName;

                comp.Reset();
            }

            Console.WriteLine($"Day8 Part2 {comp.globalValue}");
        }
    }
}
