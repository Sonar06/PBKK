using System.Windows;
using StudentManagent.ViewModels;

namespace StudentManagent;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new StudentViewModel();
    }
}