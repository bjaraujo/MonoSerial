
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.HBox hbox2;
	
	private global::Gtk.Label lblPort;
	
	private global::Gtk.Entry txtPort;
	
	private global::Gtk.Label lblBaudRate;
	
	private global::Gtk.Entry txtBaudRate;
	
	private global::Gtk.Label lblParity;
	
	private global::Gtk.ComboBox cmbParity;
	
	private global::Gtk.Label lblStopBits;
	
	private global::Gtk.ComboBox cmbStopBits;
	
	private global::Gtk.Button cmdConnect;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TextView txtSerialData;
	
	private global::Gtk.Entry txtCommand;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.lblPort = new global::Gtk.Label ();
		this.lblPort.Name = "lblPort";
		this.lblPort.LabelProp = global::Mono.Unix.Catalog.GetString ("Port:");
		this.hbox2.Add (this.lblPort);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.lblPort]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.txtPort = new global::Gtk.Entry ();
		this.txtPort.CanFocus = true;
		this.txtPort.Name = "txtPort";
		this.txtPort.IsEditable = true;
		this.txtPort.InvisibleChar = '•';
		this.hbox2.Add (this.txtPort);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.txtPort]));
		w2.Position = 1;
		// Container child hbox2.Gtk.Box+BoxChild
		this.lblBaudRate = new global::Gtk.Label ();
		this.lblBaudRate.Name = "lblBaudRate";
		this.lblBaudRate.LabelProp = global::Mono.Unix.Catalog.GetString ("Baud Rate:");
		this.hbox2.Add (this.lblBaudRate);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.lblBaudRate]));
		w3.Position = 2;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.txtBaudRate = new global::Gtk.Entry ();
		this.txtBaudRate.CanFocus = true;
		this.txtBaudRate.Name = "txtBaudRate";
		this.txtBaudRate.IsEditable = true;
		this.txtBaudRate.InvisibleChar = '•';
		this.hbox2.Add (this.txtBaudRate);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.txtBaudRate]));
		w4.Position = 3;
		// Container child hbox2.Gtk.Box+BoxChild
		this.lblParity = new global::Gtk.Label ();
		this.lblParity.Name = "lblParity";
		this.lblParity.LabelProp = global::Mono.Unix.Catalog.GetString ("Parity:");
		this.hbox2.Add (this.lblParity);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.lblParity]));
		w5.Position = 4;
		w5.Expand = false;
		w5.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.cmbParity = global::Gtk.ComboBox.NewText ();
		this.cmbParity.Name = "cmbParity";
		this.hbox2.Add (this.cmbParity);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.cmbParity]));
		w6.Position = 5;
		w6.Expand = false;
		w6.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.lblStopBits = new global::Gtk.Label ();
		this.lblStopBits.Name = "lblStopBits";
		this.lblStopBits.LabelProp = global::Mono.Unix.Catalog.GetString ("Stop Bits:");
		this.hbox2.Add (this.lblStopBits);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.lblStopBits]));
		w7.Position = 6;
		w7.Expand = false;
		w7.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.cmbStopBits = global::Gtk.ComboBox.NewText ();
		this.cmbStopBits.Name = "cmbStopBits";
		this.hbox2.Add (this.cmbStopBits);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.cmbStopBits]));
		w8.Position = 7;
		w8.Expand = false;
		w8.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.cmdConnect = new global::Gtk.Button ();
		this.cmdConnect.CanFocus = true;
		this.cmdConnect.Name = "cmdConnect";
		this.cmdConnect.UseUnderline = true;
		this.cmdConnect.Label = global::Mono.Unix.Catalog.GetString ("Connect");
		this.hbox2.Add (this.cmdConnect);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.cmdConnect]));
		w9.Position = 9;
		w9.Expand = false;
		w9.Fill = false;
		this.vbox1.Add (this.hbox2);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
		w10.Position = 0;
		w10.Expand = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.txtSerialData = new global::Gtk.TextView ();
		this.txtSerialData.CanFocus = true;
		this.txtSerialData.Name = "txtSerialData";
		this.txtSerialData.Editable = false;
		this.GtkScrolledWindow.Add (this.txtSerialData);
		this.vbox1.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
		w12.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.txtCommand = new global::Gtk.Entry ();
		this.txtCommand.CanFocus = true;
		this.txtCommand.Name = "txtCommand";
		this.txtCommand.IsEditable = true;
		this.txtCommand.InvisibleChar = '•';
		this.vbox1.Add (this.txtCommand);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.txtCommand]));
		w13.Position = 2;
		w13.Expand = false;
		w13.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 1024;
		this.DefaultHeight = 715;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.cmdConnect.Clicked += new global::System.EventHandler (this.OnCmdConnectClicked);
		this.txtCommand.KeyReleaseEvent += new global::Gtk.KeyReleaseEventHandler (this.OnTxtCommandKeyReleaseEvent);
	}
}