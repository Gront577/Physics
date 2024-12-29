using System;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

class ElectronInCapacitor
{
    const double e = 1.6e-19; // заряд частицы, Кл (положительный)
    const double m = 9.11e-31; // масса электрона, кг

    static void Main()
    {
        // Создаём папку "graphs", если она не существует
        string directoryPath = "graphs";
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Console.WriteLine("Папка 'graphs' создана.");
        }

        // Исходные данные
        double r = 0.03; // внутренний радиус, м
        double R = 0.07; // внешний радиус, м
        double V0 = 7e6; // начальная скорость, м/с
        double L = 0.15; // длина конденсатора, м

        // Расчёт минимальной разности потенциалов
        double d = R - r; // расстояние между обкладками, м
        double E = e / (4 * Math.PI * 8.85e-12 * d); // поле внутри конденсатора
        double U_min = m * V0 * V0 * d / (2 * e * L); // минимальная разность потенциалов, В

        Console.WriteLine($"Минимальная разность потенциалов: {U_min:F2} В");

        // Расчёт траектории и времени полёта
        double t_max = L / V0; // время полёта, с
        double dt = 1e-10; // шаг времени, с

        List<double> xList = new List<double>();
        List<double> yList = new List<double>();
        List<double> vyList = new List<double>();
        List<double> ayList = new List<double>();
        List<double> tList = new List<double>();

        double y = 0;
        double vy = 0;
        double ay = e * U_min / (m * d); // ускорение, м/с^2

        for (double t = 0; t <= t_max; t += dt)
        {
            y += vy * dt;
            vy += ay * dt;

            xList.Add(V0 * t);
            yList.Add(y);
            vyList.Add(vy);
            ayList.Add(ay);
            tList.Add(t);

            // Если y выходит за пределы, прекращаем расчёт
            if (Math.Abs(y) >= d / 2)
                break;
        }

        Console.WriteLine($"Время полёта: {tList.Last():F9} с");
        Console.WriteLine($"Конечная скорость Vy: {vyList.Last():F2} м/с");

        // Построение графиков
        PlotGraph(xList, yList, "График y(x)", "x (м)", "y (м)", directoryPath);
        PlotGraph(tList, vyList, "График Vy(t)", "t (с)", "Vy (м/с)", directoryPath);
        PlotGraph(tList, ayList, "График ay(t)", "t (с)", "ay (м/с^2)", directoryPath);
        PlotGraph(tList, yList, "График y(t)", "t (с)", "y (м)", directoryPath);
    }

    static void PlotGraph(List<double> x, List<double> y, string title, string xLabel, string yLabel, string directoryPath)
    {
        var plotModel = new PlotModel { Title = title };
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = xLabel });
        plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = yLabel });

        var series = new LineSeries();
        for (int i = 0; i < x.Count; i++)
        {
            series.Points.Add(new DataPoint(x[i], y[i]));
        }
        plotModel.Series.Add(series);

        var exporter = new SvgExporter { Width = 600, Height = 400 };
        string filePath = Path.Combine(directoryPath, $"{title.Replace(' ', '_')}.svg");

        using (var stream = File.Create(filePath))
        {
            exporter.Export(plotModel, stream);
        }

        Console.WriteLine($"График '{title}' сохранён в файл: {filePath}");
    }
}
