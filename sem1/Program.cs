using System;
using System.Text;

namespace RaceQualificationAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка кодировки для корректного отображения символов
            Console.OutputEncoding = Encoding.UTF8;

            // Этап 1: Ввод данных
            Console.WriteLine("=== АНАЛИЗ РЕЗУЛЬТАТОВ КВАЛИФИКАЦИИ ГОНОЧНЫХ БОЛИДОВ ===\n");

            int n;
            while (true)
            {
                Console.Write("Введите количество участников заезда: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out n) && n > 0)
                    break;

                Console.WriteLine("Ошибка! Введите положительное целое число.");
            }

            // Создание массивов
            string[] teams = new string[n];
            double[] avgSpeeds = new double[n];

            Console.WriteLine();

            // Ввод данных для каждого участника
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Участник #{i + 1}");

                Console.Write("Команда: ");
                teams[i] = Console.ReadLine();

                while (true)
                {
                    Console.Write("Средняя скорость (км/ч): ");
                    string speedInput = Console.ReadLine().Replace('.', ',');

                    if (double.TryParse(speedInput, out double speed) && speed > 0)
                    {
                        avgSpeeds[i] = speed;
                        break;
                    }

                    Console.WriteLine("Ошибка! Введите положительное число.");
                }

                Console.WriteLine();
            }

            // Этап 2: Статистический анализ
            PrintStatistics(teams, avgSpeeds);

            // Этап 3: Визуализация в виде форматированной таблицы
            PrintQualificationTable(teams, avgSpeeds, "ПРОТОКОЛ КВАЛИФИКАЦИИ");

            // Этап 4: Сортировка (Турнирная таблица)
            SortBySpeedDescending(teams, avgSpeeds);
            PrintQualificationTable(teams, avgSpeeds, "ИТОГОВЫЙ ПРОТОКОЛ КВАЛИФИКАЦИИ", includePosition: true);

            // Этап 5: Фильтр по минимальной скорости
            FilterByMinSpeed(teams, avgSpeeds);

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        /// <summary>
        /// Этап 2: Вывод статистики квалификации
        /// </summary>
        static void PrintStatistics(string[] teams, double[] speeds)
        {
            Console.WriteLine("--- СТАТИСТИКА КВАЛИФИКАЦИИ ---");

            // Средняя скорость
            double sum = 0;
            foreach (double speed in speeds)
                sum += speed;

            double averageSpeed = sum / speeds.Length;
            Console.WriteLine($"Средняя скорость: {averageSpeed:F2} км/ч");

            // Поиск лидера и аутсайдера
            int maxIndex = 0;
            int minIndex = 0;

            for (int i = 1; i < speeds.Length; i++)
            {
                if (speeds[i] > speeds[maxIndex])
                    maxIndex = i;

                if (speeds[i] < speeds[minIndex])
                    minIndex = i;
            }

            Console.WriteLine($"Лидер: {teams[maxIndex]} ({speeds[maxIndex]:F2} км/ч)");
            Console.WriteLine($"Самый медленный: {teams[minIndex]} ({speeds[minIndex]:F2} км/ч)");
            Console.WriteLine($"Разница темпа: {speeds[maxIndex] - speeds[minIndex]:F2} км/ч\n");
        }

        /// <summary>
        /// Этап 3 и 4: Вывод таблицы с результатами
        /// </summary>
        static void PrintQualificationTable(string[] teams, double[] speeds, string title, bool includePosition = false)
        {
            Console.WriteLine(title);
            Console.WriteLine(new string('-', 47));

            // Заголовок таблицы
            if (includePosition)
                Console.WriteLine($"| {"Поз.",-4} | {"Команда",-20} | {"Скорость",-10} |");
            else
                Console.WriteLine($"| {"Команда",-20} | {"Скорость (км/ч)",-18} |");

            Console.WriteLine(new string('-', 47));

            // Вывод данных
            for (int i = 0; i < teams.Length; i++)
            {
                if (includePosition)
                {
                    Console.WriteLine($"| {i + 1,-4} | {teams[i],-20} | {speeds[i],-10:F2} |");
                }
                else
                {
                    Console.WriteLine($"| {teams[i],-20} | {speeds[i],-18:F2} |");
                }
            }

            Console.WriteLine(new string('-', 47) + "\n");
        }

        /// <summary>
        /// Этап 4: Сортировка пузырьком по убыванию скорости
        /// Использован алгоритм сортировки пузырьком для наглядности и образовательных целей
        /// </summary>
        static void SortBySpeedDescending(string[] teams, double[] speeds)
        {
            int n = speeds.Length;

            // Сортировка пузырьком
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (speeds[j] < speeds[j + 1])
                    {
                        // Обмен скоростей
                        double tempSpeed = speeds[j];
                        speeds[j] = speeds[j + 1];
                        speeds[j + 1] = tempSpeed;

                        // Обмен названий команд
                        string tempTeam = teams[j];
                        teams[j] = teams[j + 1];
                        teams[j + 1] = tempTeam;
                    }
                }
            }
        }

        /// <summary>
        /// Этап 5: Фильтрация по минимальной скорости
        /// </summary>
        static void FilterByMinSpeed(string[] teams, double[] speeds)
        {
            Console.Write("Введите минимальную скорость для отбора (км/ч): ");

            if (double.TryParse(Console.ReadLine(), out double minSpeed))
            {
                Console.WriteLine($"\nКоманды со скоростью >= {minSpeed:F2} км/ч:");

                int count = 0;
                for (int i = 0; i < speeds.Length; i++)
                {
                    if (speeds[i] >= minSpeed)
                    {
                        Console.WriteLine($"- {teams[i]} ({speeds[i]:F2} км/ч)");
                        count++;
                    }
                }

                Console.WriteLine($"\nОтобрано команд: {count}\n");
            }
            else
            {
                Console.WriteLine("Ошибка! Введено некорректное число.\n");
            }
        }
    }
}
