import pandas as pd
import matplotlib.pyplot as plt
from datetime import datetime, timedelta
import re
import os

# Функция для чтения данных из файла
def read_data_from_file(file_path):
    with open(file_path, 'r') as file:
        data = file.read()
    return data

# Функция для сохранения графиков
def save_plot(plt, filename, folder='plots'):
    if not os.path.exists(folder):
        os.makedirs(folder)
    plt.savefig(os.path.join(folder, filename))

# Чтение данных из файла
file_path = 'gym_data.txt'  # Укажите путь к вашему файлу
data = read_data_from_file(file_path)

# Парсинг данных
pattern = r"\[(.*?)\] Current gym availability: The gym is (open|closed): (\d+)/(\d+)"
records = []
for line in data.strip().split("\n"):
    match = re.match(pattern, line.strip())
    if match:
        timestamp_str, status, current, total = match.groups()
        timestamp = datetime.strptime(timestamp_str, "%m/%d/%Y %I:%M:%S %p")
        if status == "open":
            records.append((timestamp, int(current), int(total)))

# Создание DataFrame
df = pd.DataFrame(records, columns=["timestamp", "current", "total"])

# Конвертация времени в GMT+3
df["timestamp"] = df["timestamp"] + timedelta(hours=3)

# Определение временного промежутка данных
start_date = df["timestamp"].min().strftime("%Y-%m-%d")
end_date = df["timestamp"].max().strftime("%Y-%m-%d")
folder_name = f"plots_{start_date}_to_{end_date}"

# Фильтрация данных по дням недели и времени работы зала
df["weekday"] = df["timestamp"].dt.weekday
df["time"] = df["timestamp"].dt.time

weekday_open_time = datetime.strptime("05:20", "%H:%M").time()
weekday_close_time = datetime.strptime("21:40", "%H:%M").time()
saturday_open_time = datetime.strptime("08:50", "%H:%M").time()
saturday_close_time = datetime.strptime("18:40", "%H:%M").time()

def is_within_working_hours(weekday, time):
    if weekday == 6:  # Воскресенье
        return False
    if weekday < 5:  # Понедельник - Пятница
        return weekday_open_time <= time <= weekday_close_time
    if weekday == 5:  # Суббота
        return saturday_open_time <= time <= saturday_close_time
    return False

# Применение фильтрации данных по рабочим часам
df = df[df.apply(lambda row: is_within_working_hours(row["weekday"], row["time"]), axis=1)]

# Группировка данных по дням недели и часам, вычисление среднего арифметического
df["hour"] = df["timestamp"].dt.hour
weekly_avg = df.groupby(["weekday", "hour"])["current"].mean().unstack(fill_value=0)

# Названия дней недели
days_of_week = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

# Построение отдельных графиков для каждого дня недели
for i in range(6):  # Понедельник - Суббота (0-5)
    plt.figure(figsize=(10, 6))
    weekly_avg.loc[i].reindex(range(6, 22), fill_value=0).plot(kind='bar', edgecolor='black')
    plt.xlabel("Hour of the day (GMT+3)")
    plt.ylabel("Average number of people in the gym")
    plt.title(f"Average Gym Occupancy by Hour on {days_of_week[i]} (GMT+3)")
    plt.grid(True)
    plt.xticks(range(0, 16))
    save_plot(plt, f"average_gym_occupancy_by_hour_{days_of_week[i].lower()}.png", folder_name)
    plt.close()

# Средний график для всех дней недели
overall_avg = weekly_avg.mean(axis=0).reindex(range(6, 22), fill_value=0)

plt.figure(figsize=(10, 6))
overall_avg.plot(kind='bar', edgecolor='black')
plt.xlabel("Hour of the day (GMT+3)")
plt.ylabel("Average number of people in the gym")
plt.title("Average Gym Occupancy by Hour (Overall) (GMT+3)")
plt.grid(True)
plt.xticks(range(0, 16))
save_plot(plt, "average_gym_occupancy_by_hour_overall.png", folder_name)
plt.close()

# Вычисление среднего количества людей для каждого дня недели
daily_avg = df.groupby("weekday")["current"].mean()

# Построение графика для среднего количества людей по дням недели
plt.figure(figsize=(10, 6))
daily_avg.plot(kind='bar', edgecolor='black')
plt.xlabel("Day of the Week")
plt.ylabel("Average number of people in the gym")
plt.title("Average Gym Occupancy by Day of the Week")
plt.grid(True)
plt.xticks(range(6), days_of_week)
save_plot(plt, "average_gym_occupancy_by_day_of_week.png", folder_name)
plt.close()
