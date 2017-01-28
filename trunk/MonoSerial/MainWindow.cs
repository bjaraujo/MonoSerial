﻿using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Text;
using Gtk;
using IniParser;
using IniParser.Model;

public partial class MainWindow : Gtk.Window
{

	MonoSerial.SettingsDialog _settings;

	SerialPort _serialPort;
	bool _exitApp = false;
	string _iniFileName = "MonoSerial.ini";
	Gdk.Color _bgColor = new Gdk.Color(0, 0, 0);
	Gdk.Color _textColor = new Gdk.Color(0, 255, 0);
	Pango.FontDescription _font = Pango.FontDescription.FromString("Courier 12");
	Thread _t1 = null;
		
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{

		Build();

		this.txtSerialData.ModifyFont(_font);
		this.txtSerialData.ModifyBase(StateType.Normal, _bgColor);
		this.txtSerialData.ModifyText(StateType.Normal, _textColor);

		//Debug.WriteLine(System.Environment.OSVersion);

		if (System.Environment.OSVersion.Platform == PlatformID.Unix)
			this.txtPort.Text = "/dev/ttyS0";
		else
			this.txtPort.Text = "COM1";

		this.txtBaudRate.Text = "9600";

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

		_settings = new MonoSerial.SettingsDialog();
		_settings.Hide();

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
						if (this.txtSerialData.Buffer.LineCount > 10000)
							this.txtSerialData.Buffer.Clear();

						OutputText(aText);

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

	protected void OutputText(string aText)
	{

		if (_settings.AppendOption == MonoSerial.SettingsDialog.Append.CR)
			aText += "\r";
		else if (_settings.AppendOption == MonoSerial.SettingsDialog.Append.LF)
			aText += "\n";
		if (_settings.AppendOption == MonoSerial.SettingsDialog.Append.CRLF)
			aText += "\r\n";

		this.txtSerialData.Buffer.Text += aText;
		this.txtSerialData.ScrollToIter(txtSerialData.Buffer.EndIter, 0, false, 0, 0);

	}

	protected void LoadSettings()
	{

		try
		{

			var aParser = new FileIniDataParser();

			IniData data = aParser.ReadFile(_iniFileName);

			// SerialPort
			this.txtPort.Text = data["SerialPort"]["Name"];
			this.txtBaudRate.Text = data["SerialPort"]["Speed"];
			this.cmbParity.Active = Int32.Parse(data["SerialPort"]["Parity"]);
			this.cmbStopBits.Active = Int32.Parse(data["SerialPort"]["Stopbit"]);

			// View
			string fontName = data["View"]["FontName"];
			string fontSize = data["View"]["FontSize"];

			_font = Pango.FontDescription.FromString(fontName + " " + fontSize);
			this.txtSerialData.ModifyFont(_font);

			string[] bgColor = data["View"]["BackgroundColor"].Split(',');
			string[] textColor = data["View"]["TextColor"].Split(',');

			if (bgColor.Length > 2)
			{
				_bgColor = new Gdk.Color(Byte.Parse(bgColor[0]), Byte.Parse(bgColor[1]), Byte.Parse(bgColor[2]));
				this.txtSerialData.ModifyBase(StateType.Normal, _bgColor);
			}

			if (textColor.Length > 2)
			{
				_textColor = new Gdk.Color(Byte.Parse(textColor[0]), Byte.Parse(textColor[1]), Byte.Parse(textColor[2]));
				this.txtSerialData.ModifyText(StateType.Normal, _textColor);
			}

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

			// SerialPort
			data["SerialPort"]["Name"] = this.txtPort.Text;
			data["SerialPort"]["Speed"] = this.txtBaudRate.Text;
			data["SerialPort"]["Parity"] = this.cmbParity.Active.ToString();
			data["SerialPort"]["Stopbit"] = this.cmbStopBits.Active.ToString();

			// View
			data["View"]["FontName"] = _font.Family;
			data["View"]["FontSize"] = (_font.Size / 1024).ToString();
			data["View"]["BackgroundColor"] = (_bgColor.Red / 65535 * 255).ToString() + "," + (_bgColor.Green / 65535 * 255).ToString() + "," + (_bgColor.Blue / 65535 * 255).ToString();
			data["View"]["TextColor"] = (_textColor.Red / 65535 * 255).ToString() + "," + (_textColor.Green / 65535 * 255).ToString() + "," + (_textColor.Blue / 65535 * 255).ToString();

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

		if (_t1 != null)
			_t1.Join();

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
				_serialPort.WriteTimeout = 1000;
				_serialPort.Encoding = Encoding.UTF8;

				_serialPort.Open();

				this.cmdConnect.Label = "Close";

				OutputText("<Connected to port: " + txtPort.Text + ">" + Environment.NewLine);

				this.txtCommand.IsFocus = true;

				_t1 = new Thread(ReadData);
				_t1.Start();

			}
			catch (Exception)
			{

				OutputText("Error: Unable to open port: " + txtPort.Text + Environment.NewLine);

			}

		}
		else {

			try
			{

				_serialPort.Close();
				this.cmdConnect.Label = "Open";

				OutputText("<Connection closed.>" + Environment.NewLine);

				_t1.Join();

			}
			catch (Exception)
			{

				OutputText("Error: closing connection." + Environment.NewLine);

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

				const int bufferLength = 1024;
				byte[] buffer = new byte[bufferLength];
				int bytesRead = 0;

				using (FileStream source = new FileStream(aFileChooser.Filename, FileMode.Open, FileAccess.Read))
				{
					while ((bytesRead = source.Read(buffer, 0, bufferLength)) > 0)
					{
						_serialPort.Write(buffer, 0, bytesRead);
					}
				}

				OutputText("File sent: " + aFileChooser.Filename + Environment.NewLine);

			}
			catch (Exception)
			{

				OutputText("Error: Unable to write data to serial port." + Environment.NewLine);

			}

		}

		aFileChooser.Destroy();

	}

	protected void OnClearActionActivated(object sender, EventArgs e)
	{

		this.txtSerialData.Buffer.Clear();
		             
	}

	protected void OnSettingsActionActivated(object sender, EventArgs e)
	{

		_settings.Modal = true;
		_settings.Show();
						
	}
}