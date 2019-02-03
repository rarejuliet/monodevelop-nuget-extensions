
// This file has been generated by the GUI designer. Do not modify.
using MonoDevelop.Components;
using MonoDevelop.Core;

namespace MonoDevelop.PackageManagement
{
	partial class PackageConsoleToolbarWidget
	{
		private global::Gtk.HBox mainHBox;
		private global::Gtk.Label packageSourceLabel;
		private global::Gtk.ComboBox packageSourcesComboBox;
		private global::Gtk.Label projectLabel;
		private global::Gtk.ComboBox projectsComboBox;
		Gtk.Button configurePackageSourceButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MonoDevelop.PackageManagement.PackageConsoleToolbarWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "MonoDevelop.PackageManagement.PackageConsoleToolbarWidget";
			// Container child MonoDevelop.PackageManagement.PackageConsoleToolbarWidget.Gtk.Container+ContainerChild
			this.mainHBox = new global::Gtk.HBox ();
			this.mainHBox.Name = "mainHBox";
			this.mainHBox.Spacing = 6;
			// Container child mainHBox.Gtk.Box+BoxChild
			this.packageSourceLabel = new global::Gtk.Label ();
			this.packageSourceLabel.Name = "packageSourceLabel";
			this.packageSourceLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Package Source:");
			this.mainHBox.Add (this.packageSourceLabel);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.mainHBox [this.packageSourceLabel]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child mainHBox.Gtk.Box+BoxChild
			this.packageSourcesComboBox = global::Gtk.ComboBox.NewText ();
			this.packageSourcesComboBox.WidthRequest = 200;
			this.packageSourcesComboBox.Name = "packageSourcesComboBox";
			this.mainHBox.Add (this.packageSourcesComboBox);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.mainHBox [this.packageSourcesComboBox]));
			w2.Position = 1;
			var configurePackageImage = new ImageView ("md-prefs-generic", Gtk.IconSize.Menu);
			configurePackageSourceButton = new Gtk.Button (configurePackageImage);
			configurePackageSourceButton.TooltipText = GettextCatalog.GetString ("Configure Package Sources");
			mainHBox.Add (configurePackageSourceButton);
			var boxChild = ((Gtk.Box.BoxChild)(mainHBox[configurePackageSourceButton]));
			boxChild.Position = 2;
			boxChild.Expand = false;
			boxChild.Fill = false;
			// Container child mainHBox.Gtk.Box+BoxChild
			this.projectLabel = new global::Gtk.Label ();
			this.projectLabel.Name = "projectLabel";
			this.projectLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("Default Project:");
			this.mainHBox.Add (this.projectLabel);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.mainHBox [this.projectLabel]));
			w3.Position = 3;
			w3.Expand = false;
			w3.Fill = false;
			// Container child mainHBox.Gtk.Box+BoxChild
			this.projectsComboBox = global::Gtk.ComboBox.NewText ();
			this.projectsComboBox.WidthRequest = 200;
			this.projectsComboBox.Name = "projectsComboBox";
			this.mainHBox.Add (this.projectsComboBox);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.mainHBox [this.projectsComboBox]));
			w4.Position = 4;
			this.Add (this.mainHBox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
