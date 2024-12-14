import sys
import pandas as pd

def add(a, b):
    return a + b

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python main.py <num1> <num2>")
        sys.exit(1)

    # Создание DataFrame с входными числами
    data = {'num1': [sys.argv[1]], 'num2': [sys.argv[2]]}
    df = pd.DataFrame(data)

    # Преобразование колонок в тип float
    df = df.astype(float)

    # Выполнение сложения
    result = add(df['num1'].iloc[0], df['num2'].iloc[0])
    
    print(result)
