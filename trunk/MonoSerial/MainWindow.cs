using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Timers;
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
    char[] _outBuffer = new char[1];
    int _windowLeft, _windowTop, _windowWidth, _windowHeight;
    System.Timers.Timer _scrollTimer;
	bool _keyPressed = true;

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

        _serialPort = new SerialPort();

        _settings = new MonoSerial.SettingsDialog();
        _settings.Hide();

        _scrollTimer = new System.Timers.Timer(500);

        _scrollTimer.Elapsed += new ElapsedEventHandler(OnTimeEvent);

        _scrollTimer.AutoReset = true;

        _scrollTimer.Start();

		this.txtSerialData.IsFocus = true;
		this.txtSerialData.AcceptsTab = true;
		this.txtSerialData.Editable = false;

        this.Show();

        LoadSettings();

    }

    private void OnTimeEvent(object source, ElapsedEventArgs e)
    {

        Gtk.Application.Invoke(delegate
        {

            this.txtSerialData.ScrollToIter(txtSerialData.Buffer.EndIter, 0, false, 0, 0);
            this.txtSerialData.Buffer.PlaceCursor(this.txtSerialData.Buffer.EndIter);

        });

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

        TextIter end = this.txtSerialData.Buffer.EndIter;
        this.txtSerialData.Buffer.Insert(ref end, aText);

    }

	protected void OutputMessage(string aText)
	{

		this.txtSerialData.Buffer.Text += aText;
		this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);

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
            _windowLeft = Int32.Parse(data["View"]["Left"]);
            _windowTop = Int32.Parse(data["View"]["Top"]);
            _windowWidth = Int32.Parse(data["View"]["Width"]);
            _windowHeight = Int32.Parse(data["View"]["Height"]);

            /*
			int cx, cy;
			this.GetPosition(out cx, out cy);
			this.Move(_windowLeft - cx, _windowTop - cy);
			this.SetDefaultSize(_windowWidth, _windowHeight);
			*/

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
            int width, height;
            this.GdkWindow.GetSize(out width, out height);

            int left, top;
            this.GdkWindow.GetPosition(out left, out top);

            data["View"]["Left"] = left.ToString();
            data["View"]["Top"] = top.ToString();
            data["View"]["Width"] = width.ToString();
            data["View"]["Height"] = height.ToString();
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

                this.txtSerialData.IsFocus = true;

                OutputText("<Connected to port: " + txtPort.Text + ">" + Environment.NewLine);

                _t1 = new Thread(ReadData);
                _t1.Start();

            }
            catch (Exception)
            {

                OutputText("Error: Unable to open port: " + txtPort.Text + Environment.NewLine);

            }

        }
        else
        {

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

                OutputMessage("File sent: " + aFileChooser.Filename + Environment.NewLine);

            }
            catch (Exception)
            {

                OutputMessage("Error: Unable to write data to serial port." + Environment.NewLine);

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

    protected void OnTxtSerialDataFocused(object o, FocusedArgs args)
    {

        this.txtSerialData.Buffer.PlaceCursor(this.txtSerialData.Buffer.EndIter);

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

			OutputMessage("Error: Unable to write data to serial port." + Environment.NewLine);

		}

		QueueDraw();

	}

	protected void OnTxtSerialDataFocusOutEvent(object o, FocusOutEventArgs args)
	{

		if (_keyPressed)
		{
			this.txtSerialData.IsFocus = true;
			_keyPressed = false;
		}

	}

	protected void OnTxtSerialDataKeyReleaseEvent(object o, KeyReleaseEventArgs args)
	{

		try
		{

			_keyPressed = true;

			if (args.Event.Key == Gdk.Key.Return)
			{
				_outBuffer[0] = '\r';
				_serialPort.Write(_outBuffer, 0, 1);
			}
			else if (args.Event.Key == Gdk.Key.BackSpace)
			{
				_outBuffer[0] = (char)8;
				_serialPort.Write(_outBuffer, 0, 1);
			}
			else if (args.Event.Key == Gdk.Key.Tab)
			{
				_outBuffer[0] = (char)9;
				_serialPort.Write(_outBuffer, 0, 1);
			}
			else if (args.Event.Key == Gdk.Key.Shift_R ||
				args.Event.Key == Gdk.Key.Shift_L ||
				args.Event.Key == Gdk.Key.Control_R ||
				args.Event.Key == Gdk.Key.Control_L
			   )
			{
				// Do nothing
			}
			else
			{
				_outBuffer[0] = (char)args.Event.KeyValue;
				_serialPort.Write(_outBuffer, 0, 1);
			}

		}
		catch (Exception)
		{

			OutputMessage("Error: Unable to write data to serial port." + Environment.NewLine);

		}
		finally
		{

			this.txtSerialData.ScrollToIter(txtSerialData.Buffer.EndIter, 0, false, 0, 0);
			this.txtSerialData.Buffer.PlaceCursor(this.txtSerialData.Buffer.EndIter);

			QueueDraw();

		}

	}

}