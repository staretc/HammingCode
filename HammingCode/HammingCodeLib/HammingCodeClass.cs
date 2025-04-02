using System;
using System.Collections.Generic;
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
                text = text.Insert((int)Math.Pow(2, indicator) - 1, "0");
            }

            // определяем значения контрольных битов
            text = ComputeControlBits(text, controlBitsCount);

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
            var controlBitsCount = ComputeCountOfControlBits_EncodedText(text);

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
        /// Определение значений каждого контрольного бита
        /// </summary>
        /// <param name="text">Входная строка</param>
        /// <param name="controlBitsCount">Количество контрольных битов</param>
        /// <returns>Строка с подсчитанными контрольными битами</returns>
        private static string ComputeControlBits(string text, int controlBitsCount)
        {
            // для каждого контрольного бита
            for (int indicator = 0; indicator < controlBitsCount; indicator++)
            {
                var position = (int)Math.Pow(2, indicator) - 1; // позиция контрольного бита
                var shift = 0; // сдвиг по строке
                var index = 0; // индексатор для прохода по строке
                var controlBit = 0; // значение контрольного бита

                // проходим биты начиная с position + 1
                while (true) // тут проблемки
                {
                    while (index > position)
                    {
                        if (position + (position + 1) * shift + index >= text.Length)
                        {
                            break;
                        }
                        controlBit += int.Parse(text[position + (position + 1) * shift + index].ToString());
                        index++;
                    }
                    if (position + (position + 1) * shift + index >= text.Length)
                    {
                        break;
                    }
                    shift++;
                    index = 0;
                }
                // заносим в position значение controlBit по модулю 2
                text = text.Remove(position, 1);
                text = text.Insert(position, $"{controlBit % 2}");
            }
            return text;
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
        public static void SetRandomError(string code)
        {

        }
    }
}
