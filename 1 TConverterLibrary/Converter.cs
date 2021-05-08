namespace TConverterLibrary {
  public class Converter {
    private double ToCelsius(double fahrenheit) {
      return (fahrenheit - 32) / 1.8;
    }
    public double ToFahrenheit(double celsius) {
      return (celsius * 9 / 5) + 32;
    }
  }
}
