using System.Windows;
using StudentManagent.Data;

namespace StudentManagent;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Database.Initialize();
    }
}