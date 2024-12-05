using System;

class GyroscopicModeling
{
    static void Main(string[] args)
    {
        double масса = 5000;     // Масса (кг)
        double радиус = 0.1;     // Радиус (м)
        double n = 3000;         // Скорость вращения ротора (об/мин)
        double L = 5;            // Расстояние между подшипниками (м)

        // Расчет угловой скорости вращения ротора
        double omega = 2 * Math.PI * n / 60; // рад/с

        // Переменные для отслеживания максимального давления
        double maxPressure = 0;
        int maxAmplitude = 0;
        int maxPeriod = 0;

        Console.WriteLine("Амплитуда (°)\tПериод (с)\tP (Н/м²)");

        for (int амплитуда = 3; амплитуда <= 8; амплитуда++)
        {
            for (int период = 10; период <= 15; период++)
            {
                // Момент инерции
                double I = 0.5 * масса * Math.Pow(радиус, 2);

                // Угловая скорость прецессии
                double Omega = (2 * Math.PI / период) * Math.Sin(амплитуда * Math.PI / 180);

                // Гироскопический момент
                double M_g = I * omega * Omega;

                // Давление на подшипники
                double P = M_g / L;

                // Проверка на максимальное давление
                if (P > maxPressure)
                {
                    maxPressure = P;
                    maxAmplitude = амплитуда;
                    maxPeriod = период;
                }

                // Вывод текущего давления
                Console.WriteLine($"{амплитуда}\t\t{период}\t\t{P:F2}");
            }
        }

        // Вывод максимального давления
        Console.WriteLine($"\nМаксимальное давление: {maxPressure:F2} Н/м²");
        Console.WriteLine($"Амплитуда: {maxAmplitude}°, Период: {maxPeriod} с");
    }
}