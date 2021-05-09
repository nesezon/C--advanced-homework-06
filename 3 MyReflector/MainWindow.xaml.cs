using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace MyReflector {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void FileOpen_Click(object sender, RoutedEventArgs e) {

      // диалог запроса имени файла
      OpenFileDialog openFileDialog = new OpenFileDialog(){
        Filter = "Assembly files (*.exe;*.dll)|*.exe;*.dll|All files (*.*)|*.*"
      };
      if (openFileDialog.ShowDialog() == true) {
        FileInfo file = new FileInfo(openFileDialog.FileName);

        // Разбор сборки

        // Построение дерева




      }
    }
  }
}
