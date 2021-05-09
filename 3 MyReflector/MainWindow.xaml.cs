using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace MyReflector {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void FileOpen_Click(object sender, RoutedEventArgs e) {

      // диалог запроса имени файла
      OpenFileDialog openFileDialog = new OpenFileDialog() {
        Filter = "Assembly files (*.exe;*.dll)|*.exe;*.dll|All files (*.*)|*.*"
      };
      if (openFileDialog.ShowDialog() == true) {
        FileInfo file = new FileInfo(openFileDialog.FileName);

        var assembly = Assembly.LoadFrom(file.FullName);
        // Информация о сборке
        TreeItem root = GetAssemblyInfo(assembly);

        Type[] types = assembly.GetTypes();
        foreach (Type type in types) {
          TreeItem typeItem = new TreeItem { Title = type.ToString() };
          // Методы типа
          MethodInfo[] methods = type.GetMethods();
          if (methods != null) {
            foreach (MethodInfo method in methods) {
              string MethodBody = getMethodBody(method);
              typeItem.Items.Add(new TreeItem { Title = method.Name, Description = MethodBody });
            }
            root.Items.Add(typeItem);
          }
        }

        // Отображаю в дереве
        Tree.Items.Add(root);
      }
    }

    private string getMethodBody(MethodInfo method) {
      string result = string.Empty;
      var methodBody = method.GetMethodBody();
      if (methodBody != null) {
        var byteArray = methodBody.GetILAsByteArray();
        foreach (var b in byteArray) {
          result += b + ":";
        }
      }
      return result;
    }

    private TreeItem GetAssemblyInfo(Assembly assembly) {
      var sb = new StringBuilder();
      Concatenate(ref sb, assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title, "Title");
      Concatenate(ref sb, assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version, "FileVersion");
      Concatenate(ref sb, assembly.GetCustomAttribute<GuidAttribute>()?.Value, "GUID");
      Concatenate(ref sb, assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName, "TagetFramework");
      Concatenate(ref sb, assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company, "Company");
      Concatenate(ref sb, assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright, "Copyright");
      Concatenate(ref sb, assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description, "Description");
      return new TreeItem {
        Title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title,
        Description = sb.ToString()
      };
    }

    void Concatenate(ref StringBuilder sb, string txt, string title) {
      if (txt != null) {
        if (sb.Length > 0) sb.Append(Environment.NewLine);
        sb.AppendFormat("{0,-25}", title + ":");
        sb.Append(txt);
      }
    }
  }

  public class TreeItem {
    public TreeItem() {
      this.Items = new ObservableCollection<TreeItem>();
    }
    public string Title { get; set; }
    public string Description { get; set; }
    public ObservableCollection<TreeItem> Items { get; set; }
  }
}
