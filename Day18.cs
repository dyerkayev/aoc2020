using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace aoc2020
{   
    class Day18
    {
        string[] expressions;
        public Day18()
        {
            expressions = File.ReadAllLines("day18.txt");
        }

        public BigInteger EvalOp(char op, BigInteger val1, BigInteger val2)
        {
            if (op == '*')
                return val1 * val2;
            else if (op == '+')
                return val1 + val2;
            else
                return val2;
        }

        public BigInteger EvalExpr1(string expression, ref int start)
        {
            BigInteger val = 0;
            char op = 'n';
            while (start < expression.Length)
            {
                char c = expression[start];

                if (char.IsNumber(c))
                    val = EvalOp(op, val, (int)char.GetNumericValue(c));
                else if (c == '+' || c == '*')
                    op = c;
                else if (c == '(')
                {
                    start++;
                    BigInteger val1 = EvalExpr1(expression, ref start);
                    val = EvalOp(op, val, val1);
                }
                else if (c == ')')
                    return val;

                start++;
            }

            return val;
        }

        public void EvalOp2(char op, BigInteger val2, List<BigInteger> expr)
        {
            expr.Add(val2);
            if (op == '+')
            {
                int lastIndex = expr.Count - 1;
                expr[lastIndex - 1] = expr[lastIndex] + expr[lastIndex - 1];
                expr.RemoveAt(lastIndex);
            }
  
        }
        public BigInteger EvalExpr3(List<BigInteger> expr)
        {
            if (expr.Count == 1)
                return expr[0];

            BigInteger result = 1;
            for (int i = 0; i < expr.Count; i++)
            {
                result *= expr[i];
            }

            return result;
        }

        public BigInteger EvalExpr2(string expression, ref int start)
        {
            char op = 'n';
            List<BigInteger> expr = new List<BigInteger>();
            while (start < expression.Length)
            {
                char c = expression[start];

                if (char.IsNumber(c))
                    EvalOp2(op, (int)char.GetNumericValue(c), expr);
                else if (c == '+' || c == '*')
                    op = c;
                else if (c == '(')
                {
                    start++;
                    BigInteger val = EvalExpr2(expression, ref start);
                    EvalOp2(op, val, expr);
                }
                else if (c == ')')
                    return EvalExpr3(expr);

                start++;
            }

            return EvalExpr3(expr);
        }

        public void Part1()
        {
            BigInteger sum = 0;
            foreach (string l in expressions)
            {
                int pos = 0;
                sum += EvalExpr1(l, ref pos);
            }

            Console.WriteLine($"Day18 Part 1 {sum}");
        }

        public void Part2()
        {
            BigInteger sum = 0;
            foreach (string l in expressions)
            {
                int pos = 0;
                sum += EvalExpr2(l, ref pos);
            }

            Console.WriteLine($"Day18 Part 2 {sum}");        
        }
    }
}
