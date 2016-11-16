
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;

	private global::Gtk.Action FileAction;

	private global::Gtk.Action SendFileAction;

	private global::Gtk.Action ExitAction;

	private global::Gtk.VBox vboxMain;

	private global::Gtk.MenuBar mnuMain;

	private global::Gtk.HBox hboxMain;

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

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup("Default");
		this.FileAction = new global::Gtk.Action("FileAction", global::Mono.Unix.Catalog.GetString("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString("File");
		w1.Add(this.FileAction, null);
		this.SendFileAction = new global::Gtk.Action("SendFileAction", global::Mono.Unix.Catalog.GetString("Send File..."), null, null);
		this.SendFileAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Send File...");
		w1.Add(this.SendFileAction, null);
		this.ExitAction = new global::Gtk.Action("ExitAction", global::Mono.Unix.Catalog.GetString("Exit"), null, null);
		this.ExitAction.ShortLabel = global::Mono.Unix.Catalog.GetString("Exit");
		w1.Add(this.ExitAction, null);
		this.UIManager.InsertActionGroup(w1, 0);
		this.AddAccelGroup(this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vboxMain = new global::Gtk.VBox();
		this.vboxMain.Name = "vboxMain";
		this.vboxMain.Spacing = 6;
		// Container child vboxMain.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString("<ui><menubar name=\'mnuMain\'><menu name=\'FileAction\' action=\'FileAction\'><menuitem" +
				" name=\'SendFileAction\' action=\'SendFileAction\'/><menuitem name=\'ExitAction\' acti" +
				"on=\'ExitAction\'/></menu></menubar></ui>");
		this.mnuMain = ((global::Gtk.MenuBar)(this.UIManager.GetWidget("/mnuMain")));
		this.mnuMain.Name = "mnuMain";
		this.vboxMain.Add(this.mnuMain);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vboxMain[this.mnuMain]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vboxMain.Gtk.Box+BoxChild
		this.hboxMain = new global::Gtk.HBox();
		this.hboxMain.Name = "hboxMain";
		this.hboxMain.Spacing = 6;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.lblPort = new global::Gtk.Label();
		this.lblPort.Name = "lblPort";
		this.lblPort.LabelProp = global::Mono.Unix.Catalog.GetString("Port:");
		this.hboxMain.Add(this.lblPort);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.lblPort]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.txtPort = new global::Gtk.Entry();
		this.txtPort.CanFocus = true;
		this.txtPort.Name = "txtPort";
		this.txtPort.IsEditable = true;
		this.txtPort.InvisibleChar = '•';
		this.hboxMain.Add(this.txtPort);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.txtPort]));
		w4.Position = 1;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.lblBaudRate = new global::Gtk.Label();
		this.lblBaudRate.Name = "lblBaudRate";
		this.lblBaudRate.LabelProp = global::Mono.Unix.Catalog.GetString("Baud Rate:");
		this.hboxMain.Add(this.lblBaudRate);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.lblBaudRate]));
		w5.Position = 2;
		w5.Expand = false;
		w5.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.txtBaudRate = new global::Gtk.Entry();
		this.txtBaudRate.CanFocus = true;
		this.txtBaudRate.Name = "txtBaudRate";
		this.txtBaudRate.IsEditable = true;
		this.txtBaudRate.InvisibleChar = '•';
		this.hboxMain.Add(this.txtBaudRate);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.txtBaudRate]));
		w6.Position = 3;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.lblParity = new global::Gtk.Label();
		this.lblParity.Name = "lblParity";
		this.lblParity.LabelProp = global::Mono.Unix.Catalog.GetString("Parity:");
		this.hboxMain.Add(this.lblParity);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.lblParity]));
		w7.Position = 4;
		w7.Expand = false;
		w7.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.cmbParity = global::Gtk.ComboBox.NewText();
		this.cmbParity.Name = "cmbParity";
		this.hboxMain.Add(this.cmbParity);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.cmbParity]));
		w8.Position = 5;
		w8.Expand = false;
		w8.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.lblStopBits = new global::Gtk.Label();
		this.lblStopBits.Name = "lblStopBits";
		this.lblStopBits.LabelProp = global::Mono.Unix.Catalog.GetString("Stop Bits:");
		this.hboxMain.Add(this.lblStopBits);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.lblStopBits]));
		w9.Position = 6;
		w9.Expand = false;
		w9.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.cmbStopBits = global::Gtk.ComboBox.NewText();
		this.cmbStopBits.Name = "cmbStopBits";
		this.hboxMain.Add(this.cmbStopBits);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.cmbStopBits]));
		w10.Position = 7;
		w10.Expand = false;
		w10.Fill = false;
		// Container child hboxMain.Gtk.Box+BoxChild
		this.cmdConnect = new global::Gtk.Button();
		this.cmdConnect.CanFocus = true;
		this.cmdConnect.Name = "cmdConnect";
		this.cmdConnect.UseUnderline = true;
		this.cmdConnect.Label = global::Mono.Unix.Catalog.GetString("Connect");
		this.hboxMain.Add(this.cmdConnect);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hboxMain[this.cmdConnect]));
		w11.Position = 9;
		w11.Expand = false;
		w11.Fill = false;
		this.vboxMain.Add(this.hboxMain);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vboxMain[this.hboxMain]));
		w12.Position = 1;
		w12.Expand = false;
		// Container child vboxMain.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.txtSerialData = new global::Gtk.TextView();
		this.txtSerialData.CanFocus = true;
		this.txtSerialData.Name = "txtSerialData";
		this.txtSerialData.Editable = false;
		this.GtkScrolledWindow.Add(this.txtSerialData);
		this.vboxMain.Add(this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vboxMain[this.GtkScrolledWindow]));
		w14.Position = 2;
		// Container child vboxMain.Gtk.Box+BoxChild
		this.txtCommand = new global::Gtk.Entry();
		this.txtCommand.CanFocus = true;
		this.txtCommand.Name = "txtCommand";
		this.txtCommand.IsEditable = true;
		this.txtCommand.InvisibleChar = '•';
		this.vboxMain.Add(this.txtCommand);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vboxMain[this.txtCommand]));
		w15.Position = 3;
		w15.Expand = false;
		w15.Fill = false;
		this.Add(this.vboxMain);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 1023;
		this.DefaultHeight = 715;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.SendFileAction.Activated += new global::System.EventHandler(this.OnSendFileActionActivated);
		this.ExitAction.Activated += new global::System.EventHandler(this.OnExitActionActivated);
		this.cmdConnect.Clicked += new global::System.EventHandler(this.OnCmdConnectClicked);
		this.txtCommand.KeyReleaseEvent += new global::Gtk.KeyReleaseEventHandler(this.OnTxtCommandKeyReleaseEvent);
	}
}
