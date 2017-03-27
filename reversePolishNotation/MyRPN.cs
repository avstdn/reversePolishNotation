using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPN
{
    class Program
    {

        static int digit;

        static void Main(string[] args)
        {
            Console.WriteLine("Допустимые символы:\n" +
                    "\n{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'} \n{'+', '-', '*', '/'} \n{'(', ')'}\n" +
                    "\nДля выхода ведите \"0\"");
            while (true)
            {
                Console.WriteLine("\n**********************************************************");
                Console.WriteLine("\nВведите выражение:\n");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (input == "0")
                {
                    break;
                }

                List<string> result = new List<string>();

                // Режем строку по пробелам и помещаем результат в массив строк input
                input = input.Replace(" ", string.Empty);

                // В цикле посимвольно анализируем входную строку и помещаем в каждый элемент списка result число, либо символ, либо скобку
                for (int i = 0; i < input.Length;)
                {
                    string str = null;
                    if (IsInteger(Convert.ToString(input[i])))
                    {
                        str += input[i];
                        i++;
                        while (i < input.Length && IsInteger(Convert.ToString(input[i])))
                        {
                            str += input[i];
                            i++;
                        }
                        result.Add(str);
                    }
                    else if (i < input.Length)
                    {
                        str += input[i];
                        result.Add(str);
                        i++;
                    }
                }

                if (IsCorrect(result) && Parenthesis(result))
                {

                    MyStack operators = new MyStack(result.Count);
                    List<string> finalResult = new List<string>();

                    // Непосредственная реализация ОПЗ, поэлементно считываем обработанный список result и получаем ОПЗ в finalResult
                    foreach (string temp in result)
                    {
                        // Если число, то добавляем в список
                        if (IsInteger(temp))
                        {
                            finalResult.Add(temp);
                            continue;
                        }

                        // Если допустимый символ, то в стек
                        if (Priority(temp) != 0)
                        {
                            if (operators.Count() == 0)
                            {
                                operators.Push(temp);

                            }
                            else
                            {
                                if (Priority(temp) > Priority(operators.Peek()))
                                {
                                    operators.Push(temp);
                                }
                                else
                                {
                                    while (Priority(temp) <= Priority(operators.Peek()))
                                    {
                                        finalResult.Add(operators.Pop());
                                        if (operators.Count() == 0)
                                        {
                                            break;
                                        }
                                    }
                                    operators.Push(temp);
                                }

                            }

                        }

                        // Отработка ситуации с приоритетом скобок
                        if (temp == "(")
                        {
                            operators.Push(temp);
                        }
                        if (temp == ")")
                        {
                            while (operators.Peek() != "(")
                            {
                                finalResult.Add(operators.Pop());
                            }
                            operators.Pop();
                        }

                    }
                    // Выталкиваем все оставшиеся операторы из стека, после цикла foreach()
                    while (operators.Count() != 0)
                    {
                        finalResult.Add(operators.Pop());
                    }

                    // Вывод на экран ОПЗ
                    Console.WriteLine("Обратная польская запись:\n");
                    finalResult.ForEach(i => Console.Write("{0} ", i));
                    Console.WriteLine();


                    // Вывод результата вычислений
                    Console.WriteLine("\nРезультат: {0}", Calculate(finalResult));

                }
            }
        }

        // Метод для проверки приоритета операций
        static private int Priority(string a)
        {
            switch (a)
            {
                case "*": return 2;
                case "/": return 2;
                case "+": return 1;
                case "-": return 1;
                default:
                    {
                        return 0;
                    }
            }
        }

        // Метод для проверки числовых значений
        static private bool IsInteger(string b)
        {
            // Дополнительная проверка на унарный минус
            if (b[0] == '!')
            {
                return true;
            }
            return int.TryParse(b, out digit);
        }

        // Метод для проверки скобок
        static private bool Parenthesis(List<string> result)
        {
            int opBrack, clBrack, count;
            opBrack = clBrack = count = 0;

            for (int i = 0; i < result.Count; i++)
            {
                if (count == 0 && result[i] == ")")
                {
                    Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                    return false;
                }
                else if (result[i] == "(")
                {
                    opBrack++;
                    count++;
                }
                else if (result[i] == ")")
                {
                    clBrack++;
                    count++;
                }
                if (i == result.Count - 1 && count % 2 != 0)
                {
                    Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                    return false;
                }

            }
            // Возврат при успехе
            return true;
        }

        // Метод для проверки на некорректный ввод символов во входную строку
        static private bool IsCorrect(List<string> result)
        {
            int dig, sign, leftBrack, rightBrack, countOp, um;
            dig = sign = leftBrack = rightBrack = countOp = um = 0;


            for (int i = 0; i < result.Count; i++)
            {
                if (IsInteger(result[i]))
                {
                    dig++;
                    if (rightBrack == 1)
                    {
                        Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                        return false;
                    }
                    // Проверка на наличие унарного минуса, вставка в строку с числовым значением и удаление его из result 
                    if (um == 1)
                    {
                        result[i] = "!" + result[i];
                        result.RemoveAt(i - 1);
                        i--;
                    }

                    sign = leftBrack = rightBrack = um = 0;
                }

                // Дополнительная проверка знаков
                else if (Priority(result[i]) != 0)
                {
                    if (i == 0 && result[i] == "-")
                    {
                        um++;
                        countOp--;
                    }
                    else if (um == 1)
                    {
                        Console.WriteLine("Ошибка! Два оператора расположены рядом!");
                        return false;
                    }
                    else if (i == 0)
                    {
                        Console.WriteLine("Ошибка! Оператор расположен неверно!");
                        return false;
                    }
                    else if (result[i - 1] == "(" && result[i] == "-")
                    {
                        um++;
                        countOp--;
                        dig = sign = leftBrack = rightBrack = 0;
                    }

                    else
                    {
                        sign++;
                        if (sign == 2)
                        {
                            Console.WriteLine("Ошибка! Два оператора расположены рядом!");
                            return false;
                        }
                        else if (leftBrack == 1 && um == 0)
                        {
                            Console.WriteLine("Ошибка! Скобка поставлена некорректно!");
                            return false;
                        }
                        else if (i == result.Count - 1 && sign == 1)
                        {
                            Console.WriteLine("Ошибка! Оператор расположен некорректно!");
                            return false;
                        }

                        dig = leftBrack = rightBrack = um = 0;
                    }
                    countOp++;
                }

                // Проверка скобок
                else if (result[i] == "(" || result[i] == ")")
                {

                    // Проверка на ")("
                    if (rightBrack == 1 && result[i] == "(")
                    {
                        Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                        return false;
                    }
                    else if (result[i] == "(" && um == 1)
                    {
                        Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                        return false;
                    }

                    // Проверка левой скобки
                    else if (result[i] == "(")
                    {
                        leftBrack++;

                        if (dig == 1)
                        {
                            Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                            return false;
                        }


                        dig = sign = rightBrack = um = 0;
                    }

                    // Проверка пустых скобок
                    else if (result[i] == ")" && leftBrack == 1)
                    {
                        Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                        return false;
                    }

                    // Проверка правой скобки
                    else if (result[i] == ")")
                    {
                        rightBrack++;
                        if (sign == 1)
                        {
                            Console.WriteLine("Ошибка! Проверьте расстановку скобок!");
                            return false;
                        }
                        dig = sign = leftBrack = um = 0;
                    }
                }

                else
                {
                    Console.WriteLine("Ошибка! Введенные Вами символы не допустимы!");
                    return false;
                }
            }

            // Проверка на количество введенных операторов
            if (countOp == 0)
            {
                Console.WriteLine("Ошибка! Вы не ввели ни одного оператора!");
                return false;
            }
            // Возврат при успехе
            return true;
        }

        // Метод для вычисления результата
        static private double Calculate(List<string> finalResult)
        {
            double summary = 0.0;

            for (int i = 0; i < finalResult.Count; i++)
            {
                if (finalResult[i][0] == '!')
                {
                    finalResult[i] = finalResult[i].Replace('!', '-');
                }

                if (Priority(finalResult[i]) != 0)
                {
                    switch (finalResult[i])
                    {
                        case "+":
                            {
                                summary = Convert.ToDouble(finalResult[i - 2]) + Convert.ToDouble(finalResult[i - 1]);
                                break;
                            }
                        case "-":
                            {
                                summary = Convert.ToDouble(finalResult[i - 2]) - Convert.ToDouble(finalResult[i - 1]);
                                break;
                            }
                        case "*":
                            {
                                summary = Convert.ToDouble(finalResult[i - 2]) * Convert.ToDouble(finalResult[i - 1]);
                                break;
                            }
                        case "/":
                            {
                                summary = Convert.ToDouble(finalResult[i - 2]) / Convert.ToDouble(finalResult[i - 1]);
                                break;
                            }

                    }

                    finalResult[i - 1] = Convert.ToString(summary);
                    finalResult.RemoveAt(i - 2);
                    finalResult.RemoveAt(i - 1);

                    i = 0;
                }
            }
            return summary;
        }
    }
}
