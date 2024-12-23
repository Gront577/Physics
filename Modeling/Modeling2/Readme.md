﻿# Отчет по моделированию «Частица в конденсаторе»

Это программа для моделирования движения электрона в конденсаторе, расчёта минимальной разности потенциалов, траектории и времени полёта электрона, а также построения графиков зависимости различных величин от времени и координаты.

## Описание

Программа выполняет следующие шаги:

1. Рассчитывает минимальную разность потенциалов, необходимую для того, чтобы электрон мог покинуть конденсатор, используя данные о начальной скорости электрона и параметрах конденсатора (радиусах обкладок и длине).
2. Вычисляет траекторию движения электрона в поле конденсатора, а также время полёта и конечную скорость.
3. Строит графики, отображающие зависимость координаты от времени, скорости от времени, ускорения от времени и координаты от координаты. Все графики сохраняются в формате SVG в папке `graphs`.

## Формулы

### 1. Минимальная разность потенциалов

Минимальная разность потенциалов, необходимая для того, чтобы электрон мог покинуть конденсатор, вычисляется по следующей формуле:

**U_min = (m * V₀² * d) / (2 * e * L)**

где:
- **U_min** — минимальная разность потенциалов (В),
- **m** — масса электрона (9.11 × 10⁻³¹ кг),
- **V₀** — начальная скорость электрона (м/с),
- **e** — заряд электрона (1.6 × 10⁻¹⁹ Кл),
- **d** — расстояние между обкладками конденсатора (м),
- **L** — длина конденсатора (м).

### 2. Электрическое поле внутри конденсатора

Электрическое поле **E** между обкладками конденсатора вычисляется по формуле:

**E = (e) / (4 * π * ε₀ * d)**

где:
- **ε₀** — электрическая постоянная (8.85 × 10⁻¹² Ф/м),
- **d** — расстояние между обкладками конденсатора (м).

### 3. Ускорение электрона в электрическом поле

Ускорение электрона **aᵧ** в электрическом поле можно выразить через силу, действующую на него:

**aᵧ = (e * U_min) / (m * d)**

где:
- **aᵧ** — ускорение электрона (м/с²),
- **e** — заряд электрона (1.6 × 10⁻¹⁹ Кл),
- **U_min** — минимальная разность потенциалов (В),
- **m** — масса электрона (9.11 × 10⁻³¹ кг),
- **d** — расстояние между обкладками конденсатора (м).

### 4. Траектория электрона

Траектория электрона **y(t)** рассчитывается с использованием кинематического уравнения для перемещения и скорости:

**y(t) = y₀ + vᵧ * t + (1/2) * aᵧ * t²**

где:
- **y(t)** — позиция электрона по оси **y** в момент времени **t** (м),
- **vᵧ** — скорость электрона по оси **y** (м/с),
- **aᵧ** — ускорение электрона по оси **y** (м/с²),
- **y₀** — начальная позиция (в данном случае равна нулю).

---

## Данные задачи

### Таблица данных

| №  | Внутренний радиус r(см) | Внешний  Радиус R(м) | Начальная скорость V(м/с) | Длина конденсатора L(см) |
|----|-------------------------|----------------------|---------------------------|--------------------------|
| 1  | 1                       | 3                    | 9*10^6                    | 11                       |
| 2  | 1.5                     | 4                    | 8.5*10^6                  | 12                       |
| 3  | 2                       | 5                    | 8*10^6                    | 13                       |
| 4  | 2.5                     | 6                    | 7.5*10^6                  | 14                       |
| 5  | 3                       | 7                    | 5*10^6                    | 15                       | 
| ...| ...                     | ...                  | ...                       | ...                      |

## Исходный код на C#

### Мой вариант

| № | Внутренний радиус r(см) | Внешний  Радиус R(м) | Начальная скорость V(м/с) | Длина конденсатора L(см) |
|---|------------|------------|---------------|------------|
| 5  | 3                       | 7                    | 5*10^6                    | 15                       | | 

```csharp
using System;
using System.IO;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

class ElectronInCapacitor
{
    const double e = 1.6e-19; // заряд электрона, Кл
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
```
## Результаты работы кода:
![{B4E07C88-0597-47E2-8708-0FD5D6206E0E}](https://github.com/user-attachments/assets/368278b9-0833-4818-bc11-ed0f6370c101)

