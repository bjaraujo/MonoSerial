using System;
using Gtk;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Diagnostics;

public partial class MainWindow: Gtk.Window
{

	SerialPort _serialPort;
	Thread t = null;
	bool exitApp = false;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{

		Build ();
					
		this.textview1.ModifyFont(Pango.FontDescription.FromString("Courier 12"));
		this.textview1.ModifyFont(Pango.FontDescription.FromString("Courier 12"));

		this.textview1.ModifyBase(StateType.Normal, new Gdk.Color(0x00,0x00,0x00)); 
		this.textview1.ModifyText(StateType.Normal, new Gdk.Color(0xff,0xff,0xff)); 

		this.txtPort.Text = "/dev/ttyS0";
		this.txtBaudRate.Text = "115200";

		this.cmbParity.AppendText ("None");
		this.cmbParity.AppendText ("Odd");
		this.cmbParity.AppendText ("Even");
		this.cmbParity.AppendText ("Mark");
		this.cmbParity.AppendText ("Space");
		this.cmbParity.Active = 0;

		this.cmbStopBits.AppendText ("None");
		this.cmbStopBits.AppendText ("One");
		this.cmbStopBits.AppendText ("Two");
		this.cmbStopBits.AppendText ("OnePointFive");
		this.cmbStopBits.Active = 1;

		this.textview1.IsFocus = true;

		_serialPort = new SerialPort();

	}
		
	public void ReadData()
	{

		while (_serialPort.IsOpen) {

			try {
				
				string txt = _serialPort.ReadLine ();

				Gtk.Application.Invoke (delegate {				

					// Clear buffer
					if (this.textview1.Buffer.LineCount > 1000)
						this.textview1.Buffer.Clear();

					this.textview1.Buffer.Text += txt + Environment.NewLine;
					this.textview1.ScrollToIter(textview1.Buffer.EndIter, 0, false, 0, 0);

				});

			}
			catch(Exception e)
			{
				Debug.WriteLine("Exception: " + e.Message);
			}

			Thread.Sleep (20);

			if (exitApp)
				break;
			
        };
			
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{

		_serialPort.Close ();
		exitApp = true;

		t.Join ();

		Application.Quit ();
		a.RetVal = true;
	}
				
	protected void OnTxtCommandKeyReleaseEvent (object o, KeyReleaseEventArgs args)
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

			this.textview1.Buffer.Text += "Unable to write data to serial port." + Environment.NewLine;
			this.textview1.ScrollToIter(this.textview1.Buffer.EndIter, 0, false, 0, 0);

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

	protected void OnCmdConnectClicked (object sender, EventArgs e)
	{

		if (!_serialPort.IsOpen) {

			try
			{

				_serialPort.PortName = txtPort.Text;
				_serialPort.BaudRate = Int32.Parse(this.txtBaudRate.Text);
				_serialPort.Parity = GetParity(this.cmbParity.ActiveText);
				_serialPort.StopBits = GetStopBits(this.cmbStopBits.ActiveText);
				_serialPort.ReadTimeout = 200;

				_serialPort.Open();
				this.cmdConnect.Label = "Close";
				this.textview1.Buffer.Text += "Connected to port: " + txtPort.Text + Environment.NewLine;
				this.textview1.ScrollToIter(this.textview1.Buffer.EndIter, 0, false, 0, 0);

				t = new Thread(ReadData);
				t.Start();

			}
			catch (Exception)
			{

				this.textview1.Buffer.Text += "Unable to open port: " + txtPort.Text + Environment.NewLine;
				this.textview1.ScrollToIter(this.textview1.Buffer.EndIter, 0, false, 0, 0);

			}

		} else {

			_serialPort.Close ();
			this.cmdConnect.Label = "Open";
			this.textview1.Buffer.Text += "Connection closed." + Environment.NewLine;
			this.textview1.ScrollToIter(this.textview1.Buffer.EndIter, 0, false, 0, 0);
			t.Join ();

		}
			
		QueueDraw();

	}
}