using System.Windows.Controls;

namespace Calculator.Views.Controls
{
  /// <summary>
  /// Interaction logic for ButtonGrid.xaml
  /// </summary>
  public partial class ButtonGrid : UserControl
  {
    public ButtonGrid()
    {
      InitializeComponent();
      SetCommandParameters();
    }

    private void SetCommandParameters()
    {
      #region Non-Mathematical Commands
      History.CommandParameter = History.Name;
      ClearEntry.CommandParameter = ClearEntry.Name;
      ClearAll.CommandParameter = ClearAll.Name;
      Backspace.CommandParameter = Backspace.Name;
      #endregion

      #region Operators
      MemoryPlus.CommandParameter = MemoryPlus.Name;
      MemoryMinus.CommandParameter = MemoryMinus.Name;
      MemoryRecall.CommandParameter = MemoryRecall.Name;
      Divide.CommandParameter = Divide.Name;
      Multiply.CommandParameter = Multiply.Name;
      Minus.CommandParameter = Minus.Name;
      Add.CommandParameter = Add.Name;
      PlusMinus.CommandParameter = PlusMinus.Name;
      Calculate.CommandParameter = Calculate.Name;
      #endregion

      #region Inputs
      Number0.CommandParameter = Number0.Name;
      Number1.CommandParameter = Number1.Name;
      Number2.CommandParameter = Number2.Name;
      Number3.CommandParameter = Number3.Name;
      Number4.CommandParameter = Number4.Name;
      Number5.CommandParameter = Number5.Name;
      Number6.CommandParameter = Number6.Name;
      Number7.CommandParameter = Number7.Name;
      Number8.CommandParameter = Number8.Name;
      Number9.CommandParameter = Number9.Name;
      Comma.CommandParameter = Comma.Name;
      #endregion
    }
  }
}
