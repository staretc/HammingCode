using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammingCodeLib
{
    public static class HammingCode
    {
        /// <summary>
        /// Кодирование битов кодом Хэмминга
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <returns>Закодированная строка</returns>
        public static string Encode(string text)
        {
            // проверяем входную строку на содержание битов
            if (!CheckInputBits(text))
            {
                throw new ArgumentException();
            }

            // считаем количество контрольных битов
            var controlBitsCount = ComputeCountOfControlBits_InputText(text);

            // вставляем контрольные биты во входную строку
            for(int indicator = 0; indicator < controlBitsCount; indicator++)
            {
                var pos = (int)Math.Pow(2, indicator) - 1;
                text = text.Insert(pos, "0");
            }

            // определяем значения контрольных битов
            text = ComputeControlBits_InputText(text, controlBitsCount);

            return text;
        }
        /// <summary>
        /// Декодирование битов кодом Хэмминга с проверкой на ошибку
        /// </summary>
        /// <param name="text">Закодированные биты</param>
        /// <returns>Правильно раскодированная строка</returns>
        public static string Decode(string text)
        {
            // проверяем входную строку на содержание битов
            if (!CheckInputBits(text))
            {
                throw new ArgumentException();
            }

            // считаем количество контрольных битов в закодированной строке
            var controlBitsCount = ComputeCountOfControlBits_EncodedText(text);

            // пересчитываем контрольные биты
            var controlBits = ComputeControlBits_EncodedText(text, controlBitsCount);

            // переменная для подсчёта позиции ошибки (сумма позиций несовпавших битов)
            var errorPos = 0;

            // сравниваем пересчитанные контрольные биты с битами в тексте
            for (int indicator = 0; indicator < controlBitsCount; indicator++)
            {
                var pos = (int)Math.Pow(2, indicator) - 1;
                // если не совпали
                if (text[pos].ToString() != controlBits[indicator])
                {
                    // добавляем позицию в сумму
                    errorPos += pos;
                }
            }

            // проверяем позицию ошибки (если ненулевая, значит в таком бите находится ошибка)
            if (errorPos != 0)
            {
                Console.WriteLine($"Detected Error in: \t {errorPos}");
                var newBit = text[errorPos] == '0' ? "1" : "0";
                text = text.Remove(errorPos, 1);
                text = text.Insert(errorPos, newBit);
            }

            // удаляем контроьные символы из строки
            for (int indicator = controlBitsCount - 1; indicator >= 0; indicator--)
            {
                var pos = (int)Math.Pow(2, indicator) - 1;
                text = text.Remove(pos, 1);
            }

            return text;
        }
        /// <summary>
        /// Подсчёт контрольных битов для входящей строки
        /// </summary>
        /// <param name="text">Входящая строка</param>
        /// <returns>Количество битов для кодирования строки</returns>
        private static int ComputeCountOfControlBits_InputText(string text)
        {
            var count = 1;
            while (Math.Pow(2, count) < count + text.Length + 1)
            {
                count++;
            }
            return count;
        }
        /// <summary>
        /// Подсчёт контрольных битов в закодированной строке
        /// </summary>
        /// <param name="text">Закодированная строка</param>
        /// <returns>Количество контрольных битов в закодированной строке</returns>
        private static int ComputeCountOfControlBits_EncodedText(string text)
        {
            var count = 1;
            while (Math.Pow(2, count) < text.Length)
            {
                count++;
            }
            return count - 1;
        }
        /// <summary>
        /// Определение значений каждого контрольного бита входной строки
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <param name="controlBitsCount">Количество контрольных битов</param>
        /// <returns>Строка с подсчитанными контрольными битами</returns>
        private static string ComputeControlBits_InputText(string text, int controlBitsCount)
        {
            // для каждого контрольного бита
            for (int indicator = 0; indicator < controlBitsCount; indicator++)
            {
                var controlBit = GetControlBit(text, indicator);
                // заносим в position значение controlBit по модулю 2
                var pos = (int)Math.Pow(2, indicator) - 1;
                text = text.Remove(pos, 1);
                text = text.Insert(pos, controlBit);
            }
            return text;
        }
        /// <summary>
        /// Определение значений каждого контрольного бита закодированной строки
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <param name="controlBitsCount">Количество контрольных битов</param>
        /// <returns>Список контрольных битов</returns>
        private static List<string> ComputeControlBits_EncodedText(string text, int controlBitsCount)
        {
            var controlBits = new List<string>();
            // для каждого контрольного бита
            for (int indicator = 0; indicator < controlBitsCount; indicator++)
            {
                // получаем контрольный бит
                var controlBit = GetControlBit(text, indicator);
                // заносим в position значение controlBit по модулю 2
                controlBits.Add(controlBit);
            }
            return controlBits;
        }
        private static string GetControlBit(string text, int indicator)
        {
            var position = (int)Math.Pow(2, indicator) - 1; // позиция контрольного бита
            var shift = 0; // сдвиг по строке
            var index = 0; // индексатор для прохода по строке
            var controlBit = 0; // значение контрольного бита

            // проходим биты начиная с position + 1
            while (true)
            {
                // берём следующие position + 1 битов
                while (index < position + 1)
                {
                    // проверка если вышли за пределы строки
                    if (position + 2 * (position + 1) * shift + index >= text.Length)
                    {
                        break;
                    }
                    // добавляем бит для суммы в контрольный бит
                    controlBit += int.Parse(text[position + 2 * (position + 1) * shift + index].ToString());
                    index++;
                }
                // проверка если вышли за пределы строки
                if (position + 2 * (position + 1) * shift + index >= text.Length)
                {
                    break;
                }
                // перепрыгиваем через следующие position + 1 битов
                shift++;
                index = 0;
            }
            return $"{controlBit % 2}";
        }
        /// <summary>
        /// Проверка строки на содержание битов
        /// </summary>
        /// <param name="text">Строка</param>
        /// <returns>True, если в строке находятся только символы "0" и "1", иначе False</returns>
        private static bool CheckInputBits(string text)
        {
            var validChars = new char[] { '0', '1' };
            foreach(char chr in text)
            {
                if (!validChars.Contains(chr))
                {
                    return false;
                }
            }
            return true;
        }
    }
    public static class ErrorMaker
    {
        public static string SetRandomError(string code)
        {
            var random = new Random();
            var errorPos = random.Next(0, code.Length);
            Console.WriteLine($"Set Error in: \t\t {errorPos}");
            var newBit = code[errorPos] == '0' ? "1" : "0";
            code = code.Remove(errorPos, 1);
            code = code.Insert(errorPos, newBit);

            return code;
        }
    }
}
