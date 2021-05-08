using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace TConverter {
  class Program {
    static void Main() {
      string number = default;
      char oper = default;

      double num;
      // ввод числа
      while (!DoubleParse(number, out num)) {
        Console.Write("Введите температуру: ");
        number = Console.ReadLine();
      }

      // запрос операции
      while (!is1or2(oper)) {
        Console.WriteLine("Выберите операцию: ");
        Console.WriteLine("1) Из градусов Фаренгейта в градусы Цельсия");
        Console.WriteLine("2) Из градусов Цельсия в градусы Фаренгейта");
        oper = Console.ReadKey().KeyChar;
      }
      Console.WriteLine();

      // Использую позднее связывание c TConverterLibrary
      try
      {
        var assembly = Assembly.Load("TConverterLibrary");
        //Type[] types = assembly.GetTypes();
        //foreach (Type t in types) {
        //  Console.WriteLine(t.Name);
        //}
        Type type = assembly.GetType("TConverterLibrary.Converter");
        object instance = Activator.CreateInstance(type);
        MethodInfo C2F = type.GetMethod("ToFahrenheit");
        MethodInfo F2C = type.GetMethod("ToCelsius", BindingFlags.Instance | BindingFlags.NonPublic);

        // вывожу результат
        if (oper == '1') ShowResult(F2C, instance, num);
        if (oper == '2') ShowResult(C2F, instance, num);
      }
      catch (FileNotFoundException e)
      {
        Console.WriteLine(e.Message);
      }

      Console.ReadKey();
    }

    private static void ShowResult(MethodInfo method, object instance, double num) {
      double result = default;
      object[] parameter = { num };
      result = (double)method.Invoke(instance, parameter);
      Console.WriteLine($"Результат: {result}");
    }

    private static bool is1or2(char oper) {
      if (Equals(oper, '1')) return true;
      if (Equals(oper, '2')) return true;
      return false;
    }

    /// <summary>
    /// Культуро-независимый TryParse для Double (нормально работает и с '.' и с ',').
    /// </summary>
    /// <param name="source">Исходная строка с числом.</param>
    /// <param name="result">Результат преобразования в double.</param>
    /// <returns>Успешность операции (true/false)</returns>
    static bool DoubleParse(string source, out double result) {
      result = default;
      if (source == null) return false;
      if (!double.TryParse(source, out double t_prm_1)) {
        char sepdec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        char sepdecInverse = (char)('.' + ',' - sepdec);
        IFormatProvider fp = new NumberFormatInfo { NumberDecimalSeparator = sepdecInverse.ToString() };
        if (double.TryParse(source, NumberStyles.Float, fp, out double t_prm_2)) {
          result = t_prm_2;
          return true;
        }
        return false;
      }
      result = t_prm_1;
      return true;
    }
  }
}
