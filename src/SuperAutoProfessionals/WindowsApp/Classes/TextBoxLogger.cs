using System.Text;
using System.Windows.Controls;

namespace SuperAutoProfessionals.WindowsApp;

class TextBoxLogger: ILogger
{
	public TextBoxLogger(TextBox tb)
	{
		_tb = tb;
	}

	readonly TextBox _tb;
	readonly StringBuilder _sb = new();

	public void Write(string log)
	{
		_sb.Append(log);
		_tb.Text = _sb.ToString();
		_tb.ScrollToEnd();
	}

	public void WriteLine(string log)
	{
		_sb.AppendLine(log);
		_tb.Text = _sb.ToString();
		_tb.ScrollToEnd();
	}
}
