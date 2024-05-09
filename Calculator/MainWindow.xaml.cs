using System.Windows;
using Calculator.ViewModels;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
  {
    private CalculatorViewModel calculator;
    public MainWindow()
    {
      calculator = new CalculatorViewModel();
      DataContext = calculator;
      InitializeComponent();
    }
  }
}