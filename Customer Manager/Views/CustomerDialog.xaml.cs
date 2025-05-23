using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
    }

    public void SetInitialData(string name, string email, string sme, string sv)
    {
        NameTextBox.Text = name;
        EmailTextBox.Text = email;
        SmeSwitch.IsOn = sme == "IG";
        SvSwitch.IsOn = sv == "HC";
    }
}
