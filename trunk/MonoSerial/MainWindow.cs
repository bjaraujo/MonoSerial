using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using Gtk;
using IniParser;
using IniParser.Model;

public partial class MainWindow : Gtk.Window
{

	SerialPort _serialPort;
	bool _exitApp = false;
	string _iniFileName = "MonoSerial.ini";
	Thread _t = null;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{

		Build();

		this.txtSerialData.ModifyFont(Pango.FontDescription.FromString("Courier 12"));
		this.txtSerialData.ModifyFont(Pango.FontDescription.FromString("Courier 12"));

		this.txtSerialData.ModifyBase(StateType.Normal, new Gdk.Color(0x00, 0x00, 0x00));
		this.txtSerialData.ModifyText(StateType.Normal, new Gdk.Color(0xff, 0xff, 0xff));

		//Debug.WriteLine(System.Environment.OSVersion);

		if (System.Environment.OSVersion.Platform == PlatformID.Unix)
			this.txtPort.Text = "/dev/ttyS0";
		else
			this.txtPort.Text = "COM1";

		this.txtBaudRate.Text = "115200";

		this.cmbParity.AppendText("None");
		this.cmbParity.AppendText("Odd");
		this.cmbParity.AppendText("Even");
		this.cmbParity.AppendText("Mark");
		this.cmbParity.AppendText("Space");
		this.cmbParity.Active = 0;

		this.cmbStopBits.AppendText("None");
		this.cmbStopBits.AppendText("One");
		this.cmbStopBits.AppendText("Two");
		this.cmbStopBits.AppendText("OnePointFive");
		this.cmbStopBits.Active = 1;

		this.txtSerialData.IsFocus = true;

		LoadSettings();

		_serialPort = new SerialPort();

	}

	protected void ReadData()
	{

		while (_serialPort.IsOpen)
		{

			try
			{

				string aText = _serialPort.ReadExisting();

				if (aText.Length > 0)
				{
					Gtk.Application.Invoke(delegate
					{

						// Clear buffer
						if (this.txtSerialData.Buffer.LineCount > 1000)
							this.txtSerialData.Buffer.Clear();

						AppendText(aText);

					});
				}

			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception: " + e.Message);
			}

			Thread.Sleep(20);

			if (_exitApp)
				break;

		};

	}

	protected void AppendText(string aText)
	{

		this.txtSerialData.Buffer.Text += aText;
		this.txtSerialData.ScrollToIter(txtSerialData.Buffer.EndIter, 0, false, 0, 0);

	}

	protected void LoadSettings()
	{

		try
		{

			var aParser = new FileIniDataParser();

			IniData data = aParser.ReadFile(_iniFileName);

			this.txtPort.Text = data["SerialPort"]["Name"];
			this.txtBaudRate.Text = data["SerialPort"]["Speed"];
			this.cmbParity.Active = Int32.Parse(data["SerialPort"]["Parity"]);
			this.cmbStopBits.Active = Int32.Parse(data["SerialPort"]["Stopbit"]);

		}
		catch (Exception e)
		{
			Debug.WriteLine("Exception: " + e.Message);
		}

	}

	protected void SaveSettings()
	{

		try
		{

			var aParser = new FileIniDataParser();

			IniData data = aParser.ReadFile(_iniFileName);

			data["SerialPort"]["Name"] = this.txtPort.Text;
			data["SerialPort"]["Speed"] = this.txtBaudRate.Text;
			data["SerialPort"]["Parity"] = this.cmbParity.Active.ToString();
			data["SerialPort"]["Stopbit"] = this.cmbStopBits.Active.ToString();

			aParser.WriteFile(_iniFileName, data);

		}
		catch (Exception e)
		{
			Debug.WriteLine("Exception: " + e.Message);
		}

	}

	protected void ExitApplication()
	{

		SaveSettings();

		_serialPort.Close();
		_exitApp = true;

		if (_t != null)
			_t.Join();

		Application.Quit();

	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{

		ExitApplication();
		a.RetVal = true;

	}

	protected void OnTxtCommandKeyReleaseEvent(object o, KeyReleaseEventArgs args)
	{

		try
		{

			if (args.Event.Key == Gdk.Key.Return)
			{
				_serialPort.WriteLine(this.txtCommand.Text);
				this.txtCommand.Text = "";
			}

		}
		catch (Exception)
		{

			this.txtSerialData.Buffer.Text += "Error: Unable to write data to serial port." + Environment.NewLine;
			this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);

		}

		QueueDraw();

	}

	protected Parity GetParity(string text)
	{

		if (text == "None")
			return Parity.None;
		else if (text == "Odd")
			return Parity.Odd;
		else if (text == "Even")
			return Parity.Even;
		else if (text == "Mark")
			return Parity.Mark;
		else if (text == "Space")
			return Parity.Space;

		return Parity.None;

	}

	protected StopBits GetStopBits(string text)
	{

		if (text == "None")
			return StopBits.None;
		else if (text == "One")
			return StopBits.One;
		else if (text == "Two")
			return StopBits.Two;
		else if (text == "OnePointFive")
			return StopBits.OnePointFive;

		return StopBits.None;

	}

	protected void OnCmdConnectClicked(object sender, EventArgs e)
	{

		if (!_serialPort.IsOpen)
		{

			try
			{

				_serialPort.PortName = txtPort.Text;
				_serialPort.BaudRate = Int32.Parse(this.txtBaudRate.Text);
				_serialPort.Parity = GetParity(this.cmbParity.ActiveText);
				_serialPort.StopBits = GetStopBits(this.cmbStopBits.ActiveText);
				_serialPort.ReadTimeout = 200;

				_serialPort.Open();
				this.cmdConnect.Label = "Close";

				AppendText("<Connected to port: " + txtPort.Text + ">" + Environment.NewLine);

				this.txtCommand.IsFocus = true;

				_t = new Thread(ReadData);
				_t.Start();

			}
			catch (Exception)
			{

				AppendText("Error: Unable to open port: " + txtPort.Text + Environment.NewLine);

			}

		}
		else {

			try
			{

				_serialPort.Close();
				this.cmdConnect.Label = "Open";

				AppendText("<Connection closed.>" + Environment.NewLine);

				_t.Join();

			}
			catch (Exception)
			{

				AppendText("Error: closing connection." + Environment.NewLine);

			}

		}

		QueueDraw();

	}

	protected void OnExitActionActivated(object sender, EventArgs e)
	{
		ExitApplication();
	}

	protected void OnSendFileActionActivated(object sender, EventArgs e)
	{

		Gtk.FileChooserDialog aFileChooser = new Gtk.FileChooserDialog("Choose the file to open",
				this,
				FileChooserAction.Open,
				"Cancel", ResponseType.Cancel,
				"Open", ResponseType.Accept);

		if (aFileChooser.Run() == (int)ResponseType.Accept)
		{

			try
			{

				_serialPort.Write(File.ReadAllText(aFileChooser.Filename));

				AppendText("File sent: " + aFileChooser.Filename + Environment.NewLine);

			}
			catch (Exception)
			{

				AppendText("Error: Unable to write data to serial port." + Environment.NewLine);

			}

		}

		aFileChooser.Destroy();

	}
}