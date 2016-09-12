using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CampExploUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    // Memory allocation issue after logout login 
    // Todo

    public sealed partial class MainPage : Page
    {
        ASI sys_admin;
        User current;

        public MainPage()
        {
            this.InitializeComponent();
            sys_admin = new ASI();
        }

        private void action_login(object sender, RoutedEventArgs e)
        {
            user_login();
        }

        private void user_login()
        {
            current = sys_admin.connect(userInput.Text, passInput.Password);

            if (sys_admin.get_user_list().Count < 1)
            {
                sys_admin.add_user_perm("Admin", "root", "admin");
                current = sys_admin.connect(userInput.Text, passInput.Password);
            }
            if (current != null)
            {
                switch (current.Permission)
                {
                    case "admin":
                        loginError.Text = "Welcome Admin";
                        this.Frame.Navigate(typeof(Admin), sys_admin);
                        break;
                    case "explo":
                        this.Frame.Navigate(typeof(Explo), sys_admin);
                        break;
                    case "intrepide":
                        this.Frame.Navigate(typeof(Explo), sys_admin);
                        break;
                    case "explo_double":
                        // Agent double explo
                        break;
                    case "intrepide_double":
                        // Agent double intrépide
                        break;
                    default:
                        popup_msg.Text = "Accès Refusé !";
                        if (!StandardPopup.IsOpen)
                        {
                            StandardPopup.IsOpen = true;
                        }
                        break;

                }
            }
            else
            {
                popup_msg.Text = "Utilisateur ou mot de passe incorrect !";
                if (!StandardPopup.IsOpen)
                {
                    StandardPopup.IsOpen = true;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter.GetType() == typeof(ASI))
            {
                sys_admin = (ASI)e.Parameter;
            }
            

            // parameters.Name
            // parameters.Text
            // ...
        }

        private void passInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // If keypress enter, login
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                user_login();
            }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
    }
}
