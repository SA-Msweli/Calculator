using Calculator.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Calculator.ViewModels
{
  public class CalculatorViewModel : INotifyPropertyChanged
  {
    private Stack<string> expression;
    public string History { get; set; }
    private List<string> expList;
    private decimal result;
    private decimal memoryValue;
    public string ResultText
    {
      get => result.ToString("##########.##########");
      private set
      {
        if (decimal.TryParse(value, out decimal val))
        {
          result = val;
        }
        OnPropertyChanged(nameof(ResultText));
      }
    }
    private string input;
    public string InputText
    {
      get => input;
      set
      {
        input = value;
        OnPropertyChanged(nameof(InputText));
      }
    }

    private string output;
    public string OutputText
    {
      get => output;
      private set
      {
        output = value;
        OnPropertyChanged(nameof(OutputText));
      }
    }
    public ICommand? PressButton => new PressButtonCommand<string>(ProcessButtonInput);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private Dictionary<string, string> commandOperator;
    public CalculatorViewModel()
    {
      History = string.Empty;
      input = string.Empty;
      output = string.Empty;
      expList = new();
      expression = new();
      memoryValue = new();
      commandOperator = new()
      {
        { "Add", "+" },
        { "Minus", "-" },
        { "Multiply", "*" },
        { "Divide", "/" },
        { "Calculate", "=" }
      };
    }

    private void ProcessButtonInput(string text)
    {
      if (text.Contains("Number"))
      {
        InputText += Regex.Match(text, @"\d+").Value;
        OutputText += Regex.Match(text, @"\d+").Value;
      }

      if (commandOperator.TryGetValue(text, out string? opp))
      {
        ResolveOpp(opp);
        //Calculate();
      }

      switch (text)
      {
        case "Comma":
          InputText += ".";
          break;
        case "PlusMinus":
          InputText = !string.IsNullOrWhiteSpace(InputText) && InputText[0].Equals('-') ? InputText.Remove(0,1) : $"-{InputText}";
          break;
        case "MemoryRecall":
          ResultText = memoryValue.ToString();
          break;
        case "MemoryMinus":
          memoryValue -= string.IsNullOrWhiteSpace(InputText) ? 0 : Convert.ToDecimal(InputText);
          break;
        case "MemoryPlus":
          memoryValue += string.IsNullOrWhiteSpace(InputText) ? 0 : Convert.ToDecimal(InputText);
          break;
        case "Backspace":
          InputText = !InputText.Equals(string.Empty) ? InputText.Remove(InputText.Length - 1, 1) : InputText;
          OutputText = !OutputText.Equals(string.Empty) ? OutputText.Remove(OutputText.Length - 1, 1) : InputText;
          break;
        case "ClearAll":
          InputText = string.Empty;
          OutputText = string.Empty;
          ResultText = string.Empty;
          break;
        case "ClearEntry":
          InputText = string.Empty;
          break;
        case "History":
          ShowHistory();
          break;
      }
    }

    private void ResolveOpp(string opp)
    {
      if (!string.IsNullOrWhiteSpace(InputText))
      {
        expression.Push(InputText);
      }

      if (opp.Equals("="))
      {
        History += $"{OutputText}\n";
        Calculate();
        OutputText = string.Empty;
        InputText = string.Empty;
        return;
      }

      if (string.IsNullOrWhiteSpace(OutputText) && string.IsNullOrWhiteSpace(InputText) && string.Join("", [.. expression]) != ResultText)
      {
        expression.Push(ResultText);
      }
      else if (!string.IsNullOrWhiteSpace(OutputText) && commandOperator.ContainsValue(OutputText[^1].ToString()))
      {
        OutputText = OutputText.Remove(OutputText.Length - 1, 1) + opp;
        string _ = expression.Pop();
      }

      InputText = string.Empty;
      expression.Push(opp);
      expList = [.. expression];
      expList.Reverse();
      OutputText = string.Join("", expList);
    }

    private void Calculate()
    {
      //Implement BODMAS for calculation:
      string[] bodmas = ["*", "/", "+", "-"];
      foreach (string opp in bodmas)
      {
        while (expression.Contains(opp))
          Calculate(opp);
      }

      ResultText = expList.Count == 0 ? "0" : expList[0].ToString();
      History += $"= {ResultText}\n";
      expression.Clear();
    }

    private void Calculate(string opp)
    {
      expList = [.. expression];
      expList.Reverse();
      decimal result;

      int index = expList.IndexOf(opp);
      if (expList.Count > index + 1)
      {
        result = opp switch
        {
          "*" => Convert.ToDecimal(expList[index - 1]) * Convert.ToDecimal(expList[index + 1]),
          "/" => Convert.ToDecimal(expList[index - 1]) / Convert.ToDecimal(expList[index + 1]),
          "+" => Convert.ToDecimal(expList[index - 1]) + Convert.ToDecimal(expList[index + 1]),
          _ => Convert.ToDecimal(expList[index - 1]) - Convert.ToDecimal(expList[index + 1]),
        };
        expList.RemoveAt(index);
        expList.RemoveAt(index);
        expList[index - 1] = result.ToString();
      }
      expression.Clear();
      expList.ForEach(ex => {  expression.Push(ex); });
    }

    private void ShowHistory()
    {
      OnPropertyChanged(nameof(History));
    }
  }
}
