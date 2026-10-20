using System;

namespace KalkulatorWPF
{
    public class CalculatorEngine
    {
        public double PerformOperation(double operand1, double operand2, string op)
        {
            double result = 0;
            switch (op)
            {
                case "+": result = operand1 + operand2; break;
                case "-": result = operand1 - operand2; break;
                case "*": result = operand1 * operand2; break;
                case "/":
                    if (operand2 == 0) throw new DivideByZeroException("Tidak dapat membagi dengan nol.");
                    result = operand1 / operand2;
                    break;
                case "^":
                    if (operand1 == 0 && operand2 < 0) throw new DivideByZeroException("Tidak dapat membagi dengan nol.");
                    result = Math.Pow(operand1, operand2);
                    break;
                default:
                    throw new InvalidOperationException("Operasi tidak valid");
            }

            if (double.IsNaN(result) || double.IsInfinity(result))
                throw new OverflowException("Hasil melampaui batas atau tidak terdefinisi.");

            return result;
        }

        public double PerformUnaryOperation(double operand, string op)
        {
            if (op == "√")
            {
                if (operand < 0) throw new ArgumentException("Tidak dapat menghitung akar dari bilangan negatif.");
                return Math.Sqrt(operand);
            }
            return operand;
        }
    }
}