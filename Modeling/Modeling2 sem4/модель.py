import subprocess
import sys
import numpy as np
import cv2
import math
import matplotlib.pyplot as plt

PI = math.pi

# Функция для установки библиотек
def install_package(package):
    subprocess.check_call([sys.executable, "-m", "pip", "install", package])

# Проверка и установка необходимых библиотек
try:
    import numpy
except ImportError:
    print("Установка numpy...")
    install_package("numpy")

try:
    import cv2
except ImportError:
    print("Установка opencv-python...")
    install_package("opencv-python")

try:
    import matplotlib.pyplot as plt
except ImportError:
    print("Установка matplotlib...")
    install_package("matplotlib")

# Функция вычисления интенсивности для заданной длины волны
def compute_I(x, lambda_, a, d, N, D):
    beta = PI * a * x / (lambda_ * D)
    alpha = PI * d * x / (lambda_ * D)
    sinc_beta = 1.0 if abs(beta) < 1e-6 else math.sin(beta) / beta
    interference = N * N if abs(alpha) < 1e-6 else (math.sin(N * alpha) / math.sin(alpha)) ** 2
    return sinc_beta ** 2 * interference

def main():
    # Параметры
    N = 2              # Количество щелей (1 <= N <= 10)
    a = 10             # Ширина щели в мкм
    d = 50             # Период в мкм
    light_type = 0     # 0: монохроматический, 1: квазимонохроматический
    lambda_ = 500      # Длина волны в нм (или центральная для квазимонохроматического)
    delta_lambda = 10  # Ширина спектра в нм
    D = 1.0            # Расстояние до экрана в м
    x_min = -0.05      # Минимальная координата x в м
    x_max = 0.05       # Максимальная координата x в м
    num_points = 1000  # Количество точек
    M = 100            # Количество длин волн для усреднения

    # Конвертация единиц
    a_m = a * 1e-6           # мкм -> м
    d_m = d * 1e-6           # мкм -> м
    lambda_m = lambda_ * 1e-9 # нм -> м
    delta_lambda_m = delta_lambda * 1e-9 # нм -> м

    # Вычисление I(x)
    x_values = np.linspace(x_min, x_max, num_points)
    I_values = np.zeros(num_points)
    I_max = 0

    for i, x in enumerate(x_values):
        if light_type == 0:
            # Монохроматический свет
            I = compute_I(x, lambda_m, a_m, d_m, N, D)
        else:
            # Квазимонохроматический свет
            lambda_min = lambda_m - delta_lambda_m / 2
            lambda_max = lambda_m + delta_lambda_m / 2
            lambdas = np.linspace(lambda_min, lambda_max, M)
            I = np.mean([compute_I(x, lambda_k, a_m, d_m, N, D) for lambda_k in lambdas])
        I_values[i] = I
        if I > I_max:
            I_max = I

    # Нормализация интенсивности
    I_values /= I_max

    # Создание цветного изображения распределения интенсивности
    height = 100
    width = num_points
    image = np.zeros((height, width, 3), dtype=np.uint8)
    for i in range(width):
        # Интерполяция цвета от синего (низкая интенсивность) до красного (высокая)
        intensity = I_values[i]
        blue = int(255 * (1 - intensity))  # Меньше интенсивность -> больше синего
        red = int(255 * intensity)         # Больше интенсивность -> больше красного
        green = 0                          # Зеленый компонент всегда 0
        for j in range(height):
            image[j, i] = [blue, green, red]  # Формат BGR для OpenCV
    cv2.imwrite("intensity_distribution.png", image)

    # Построение графика зависимости интенсивности от координаты
    x_mm = x_values * 1000  # Переводим x в мм
    plt.figure(figsize=(8, 6))
    plt.plot(x_mm, I_values, 'b-', label='Интенсивность')
    plt.xlabel("x (мм)")
    plt.ylabel("Нормализованная интенсивность")
    plt.title(f"Интерференция от {N} щелей (λ={lambda_} нм, D={D} м)")
    plt.grid(True)
    plt.legend()
    plt.savefig("intensity_plot.png")
    plt.show()

if __name__ == "__main__":
    main()
