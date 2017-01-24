using System;
namespace MonoSerial
{
	public partial class SettingsDialog : Gtk.Dialog
	{
		public SettingsDialog()
		{
			this.Build();
		}

		public enum Append { None, CR, LF, CRLF };

		public Append AppendOption
		{
			get { 

				if (this.optAppendCR.Active)
					return Append.CR;
				else if (this.optAppendLF.Active)
					return Append.LF;
				else if (this.optAppendCRLF.Active)
					return Append.CRLF;

				return Append.None;
			}

			set {

				if (value == Append.CR)
					optAppendCR.Active = true;
				else if (value == Append.LF)
					optAppendLF.Active = true;
				else if (value == Append.CRLF)
					optAppendCRLF.Active = true;
				else	
					optAppendNothing.Active = true;

				}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			this.Hide();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Hide();
		}

	}
}
