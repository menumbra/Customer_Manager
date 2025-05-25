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
}
