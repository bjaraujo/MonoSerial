﻿using System;
using Gtk;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Diagnostics;

public partial class MainWindow : Gtk.Window
{

	SerialPort _serialPort;
	Thread t = null;
	bool exitApp = false;

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

		_serialPort = new SerialPort();

	}

	public void ReadData()
	{

		while (_serialPort.IsOpen)
		{

			try
			{

				string txt = _serialPort.ReadExisting();

				if (txt.Length > 0)
				{
					Gtk.Application.Invoke(delegate
					{

						// Clear buffer
						if (this.txtSerialData.Buffer.LineCount > 1000)
							this.txtSerialData.Buffer.Clear();

						this.txtSerialData.Buffer.Text += txt; // + Environment.NewLine;
						this.txtSerialData.ScrollToIter(txtSerialData.Buffer.EndIter, 0, false, 0, 0);

					});
				}

			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception: " + e.Message);
			}

			Thread.Sleep(20);

			if (exitApp)
				break;

		};

	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{

		_serialPort.Close();
		exitApp = true;

		t.Join();

		Application.Quit();
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
				this.txtSerialData.Buffer.Text += "<Connected to port: " + txtPort.Text + ">" + Environment.NewLine;
				this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);

				this.txtCommand.IsFocus = true;

				t = new Thread(ReadData);
				t.Start();

			}
			catch (Exception)
			{

				this.txtSerialData.Buffer.Text += "Error: Unable to open port: " + txtPort.Text + Environment.NewLine;
				this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);

			}

		}
		else {

			try
			{

				_serialPort.Close();
				this.cmdConnect.Label = "Open";
				this.txtSerialData.Buffer.Text += "<Connection closed.>" + Environment.NewLine;
				this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);
				t.Join();
			}
			catch (Exception)
			{

				this.txtSerialData.Buffer.Text += "Error: closing connection." + Environment.NewLine;
				this.txtSerialData.ScrollToIter(this.txtSerialData.Buffer.EndIter, 0, false, 0, 0);

			}

		}

		QueueDraw();

	}
}