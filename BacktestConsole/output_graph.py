import pandas as pd
import json
import matplotlib.pyplot as plt

fd = r"C:\Users\oumai\Downloads\output_file.json"
with open(fd, 'r') as file:
    data = json.load(file)

df = pd.DataFrame(data)

df['date'] = pd.to_datetime(df['date'])

plt.figure(figsize=(10, 6))
plt.plot(df['date'], df['value'], marker='o', linestyle='-', color='b', label = 'Value')
plt.plot(df['date'], df['price'], marker='x', linestyle='--', color='r', label='Price')
plt.xlabel('Date')
plt.ylabel('Value / Price')
plt.title('Value and Price over Time')
plt.grid(True)
plt.xticks(rotation=45)
plt.legend()
plt.show()
plt.savefig(r'C:\Users\oumai\Downloads\output_plot.png')
