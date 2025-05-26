using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Customer_Manager.Views;

public sealed partial class CustomerDialog : ContentDialog
{
    public string CustomerName => NameTextBox.Text.Trim();
    public string CustomerEmail => EmailTextBox.Text.Trim();
    public bool IsSme => SmeSwitch.IsOn;
    public bool IsSv => SvSwitch.IsOn;

    public CustomerDialog()
    {
        this.InitializeComponent();
        this.Loaded += CustomerDialog_Loaded;
    }

    private void CustomerDialog_Loaded(object sender, RoutedEventArgs e)
    {
        var storyboard = (Storyboard)this.Resources["EntranceStoryboard"];
        storyboard.Begin();
    }

    public void SetInitialData(string name, string email, string sme, string sv)
    {
        NameTextBox.Text = name;
        EmailTextBox.Text = email;
        SmeSwitch.IsOn = sme == "IG";
        SvSwitch.IsOn = sv == "HC";
    }

    private void CustomerDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string name = NameTextBox.Text.Trim();
        string email = EmailTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            // Prevent dialog from closing
            args.Cancel = true;

            ContentDialog errorDialog = new()
            {
                Title = "Missing Information",
                Content = "Both Name and Email fields are required.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = errorDialog.ShowAsync();
        }
    }

}
