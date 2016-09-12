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

using SQLite.Net.Attributes;
using Windows.UI.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CampExploUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Explo : Page
    {
        ASI sys_admin;

        public Explo()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            sys_admin = (ASI)e.Parameter;

            login_as.Text = "Connecté en tant que : " + sys_admin.get_current().Name;

            if(sys_admin.get_current_permisssion() == "explo")
            {
                pane_titre.Text = "Exploratrice";
                pane_message.Text = "Bienvenue agent " + sys_admin.get_current().Name;
            } else if(sys_admin.get_current_permisssion() == "intrepide")
            {
                pane_titre.Text = "Intrépide";
                pane_message.Text = "Bienvenue agent " + sys_admin.get_current().Name;
            }
            // parameters.Name
            // parameters.Text
            // ...

            liste_message_exploratrice.ItemsSource = sys_admin.get_message_curent();
        }

        private void exploratrice_message_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            liste_message_exploratrice.ItemsSource = sys_admin.get_message_curent();
            
        }

        private void explo_logout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), sys_admin);
        }

        private void liste_message_exploratrice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /**/
            ListView recu = (ListView)sender;

            Message selected = (Message)recu.SelectedItem;

            if (selected != null)
            {
                pane_titre.Text = selected.titre;
                pane_message.Text = selected.message;
                sys_admin.update_msg_id(selected.Id);
            }

            //liste_message_exploratrice.ItemsSource = sys_admin.get_message_curent();
            /**/
        }

        private void explo_send_message_Click(object sender, RoutedEventArgs e)
        {
            User cur = sys_admin.get_current();
            Message msg = new Message() { sender = cur.Name, destination = explo_message_to.Text, message = explo_message_message.Text, titre = explo_message_titre.Text, font = "bold" };
            sys_admin.send_message(msg);

            popup_msg.Text = "Message Envoyé";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }
    }    

}
