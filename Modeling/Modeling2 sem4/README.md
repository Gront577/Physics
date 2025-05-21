<!DOCTYPE html>
<html>
<head>
    <title>Анализ моделирования</title>
    <script src="https://polyfill.io/v3/polyfill.min.js?features=es6"></script>
    <script src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS-MML_HTMLorMML"></script>
</head>
<body>
    \documentclass{article}
    \usepackage[utf8]{inputenc}
    \usepackage[russian]{babel}
    \usepackage{amsmath}
    \usepackage{amsfonts}
    \usepackage{graphicx}
    \usepackage{xcolor}
    \usepackage{hyperref}

    \begin{document}

    \section*{Анализ моделирования интерференции от $N$ щелей}

    \subsection*{Условие задачи}
    Требуется смоделировать интерференцию от $N$ ($1 \leq N \leq 10$) узких высоких щелей с изменяемыми параметрами: ширина щели $a$, период $d$. Рассмотреть два типа света:
    \begin{itemize}
        \item Монохроматический свет с длиной волны $\lambda$.
        \item Квазимонохроматический свет с центральной длиной волны $\lambda$ и шириной спектра $\Delta \lambda$ (в нанометрах).
    \end{itemize}
    Необходимо вывести:
    \begin{itemize}
        \item Цветное распределение интенсивности на заданном расстоянии $D$ от щелей.
        \item График зависимости интенсивности $I(x)$ от координаты $x$.
    \end{itemize}

    \subsection*{Код и его соответствие условиям}
    Код написан на Python с использованием библиотек \texttt{numpy}, \texttt{opencv-python} и \texttt{matplotlib}. Основная функция \texttt{compute\_I} вычисляет интенсивность по формуле:
    \[
    I(x) = \left( \frac{\sin \beta}{\beta} \right)^2 \cdot \left( \frac{\sin (N \alpha)}{\sin \alpha} \right)^2,
    \]
    где $\beta = \frac{\pi a x}{\lambda D}$, $\alpha = \frac{\pi d x}{\lambda D}$, а параметры:
    \begin{itemize}
        \item $N = 2$ — количество щелей,
        \item $a = 10 \, \mu\text{m}$ — ширина щели,
        \item $d = 50 \, \mu\text{m}$ — период,
        \item $\lambda = 500 \, \text{nm}$ — длина волны (монохроматический свет),
        \item $\Delta \lambda = 10 \, \text{nm}$ — ширина спектра (квазимонохроматический свет),
        \item $D = 1.0 \, \text{m}$ — расстояние до экрана,
        \item $x \in [-0.05, 0.05] \, \text{m}$ — диапазон координат.
    \end{itemize}

    Код поддерживает оба типа света через параметр \texttt{light\_type}:
    \begin{itemize}
        \item \texttt{light\_type = 0}: монохроматический свет (используется одна длина волны $\lambda$).
        \item \texttt{light\_type = 1}: квазимонохроматический свет (усреднение по диапазону длин волн $[\lambda - \Delta \lambda / 2, \lambda + \Delta \lambda / 2]$).
    \end{itemize}

    По умолчанию \texttt{light\_type = 0}, поэтому в текущем выводе используется монохроматический свет. Для квазимонохроматического света нужно установить \texttt{light\_type = 1}.

    \subsection*{Анализ вывода}

    \subsubsection*{График зависимости $I(x)$}
    График показывает интенсивность $I(x)$ для $N = 2$, $\lambda = 500 \, \text{nm}$, $D = 1.0 \, \text{m}$. Расстояние между интерференционными максимумами:
    \[
    \Delta x = \frac{\lambda D}{d} = \frac{500 \times 10^{-9} \cdot 1.0}{50 \times 10^{-6}} = 0.01 \, \text{m} = 10 \, \text{mm}.
    \]
    Дифракционная огибающая определяется шириной щели $a$. Первый нуль огибающей:
    \[
    x = \frac{\lambda D}{a} = \frac{500 \times 10^{-9} \cdot 1.0}{10 \times 10^{-6}} = 0.05 \, \text{m} = 50 \, \text{mm}.
    \]
    На графике:
    \begin{itemize}
        \item Расстояние между пиками $\approx 10 \, \text{mm}$, что соответствует расчету.
        \item Интенсивность падает к краям ($x = \pm 50 \, \text{mm}$), что совпадает с нулем огибающей.
        \item График симметричен относительно $x = 0$, как ожидалось.
    \end{itemize}
    График полностью соответствует ожидаемой интерференционной картине.

    \subsubsection*{Цветное распределение интенсивности}
    Изображение интенсивности должно отображать цветовую шкалу от синего (низкая интенсивность) до красного (высокая интенсивность). В исправленном коде:
    \begin{itemize}
        \item Интенсивность $I = 0$: синий цвет (\texttt{[255, 0, 0]} в BGR).
        \item Интенсивность $I = 1$: красный цвет (\texttt{[0, 0, 255]} в BGR).
        \item Зеленый компонент исключен.
    \end{itemize}
    Предоставленное изображение имело зеленый цвет, что указывало на проблему с преобразованием HSV$\to$RGB. После исправления цвета строго соответствуют шкале (синий$\to$красный).

    \subsection*{Квазимонохроматический свет}
    Квазимонохроматический свет поддерживается, но не используется, так как \texttt{light\_type = 0}. Для его включения нужно установить:
    \[
    \texttt{light\_type = 1}.
    \]
    В этом случае интенсивность усредняется по диапазону $[\lambda - \Delta \lambda / 2, \lambda + \Delta \lambda / 2]$, что сгладит интерференционные пики.

    \subsection*{Заключение}
    Код полностью соответствует условиям задачи:
    \begin{itemize}
        \item Моделирование интерференции реализовано корректно.
        \item Поддерживаются оба типа света (монохроматический и квазимонохроматический).
        \item График $I(x)$ и цветное распределение интенсивности соответствуют ожидаемым результатам.
    \end{itemize}
    Для использования квазимонохроматического света измените \texttt{light\_type = 1} и перезапустите код.

    \end{document}
</body>
</html>
