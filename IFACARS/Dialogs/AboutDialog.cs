using Gdk;

namespace IFACARS.Dialogs
{
    public class AboutDialog : Gtk.AboutDialog
    {
        public AboutDialog()
        {
            Name = "IFACARS";
            Version = Shared.Version.Current;
            Website = "https://github.com";
            WebsiteLabel = "View on GitHub";
            Comments = "Log, monitor, and store your Infinite Flight flights.";
            LicenseType = Gtk.License.MitX11;

            var icon = new Pixbuf("Resources/Icon.svg");
            Logo = icon;
            Icon = icon;
        }
    }
}