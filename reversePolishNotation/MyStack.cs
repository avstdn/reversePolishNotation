using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPN
{
    class MyStack
    {
        string[] stack;
        int tos;

        public MyStack(int size)
        {
            stack = new string[size];
            tos = 0;
        }

        public void Push(string str)
        {
            if (tos == stack.Length)
            {
                Console.WriteLine("\n*** Стек полон ***\n");
                return;
            }
            stack[tos] = str;
            tos++;
        }

        public string Pop()
        {
            if (tos == 0)
            {
                Console.WriteLine("\n*** Стек пуст ***\n");
                return "0";
            }
            tos--;
            return stack[tos];
        }

        public string Peek()
        {
            if (tos == 0)
            {
                Console.WriteLine("\n*** Стек пуст ***\n");
                return "0";
            }
            return stack[tos - 1];
        }

        public int Count()
        {
            return tos;
        }
    }
}