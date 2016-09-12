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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CampExploUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    // Memory allocation issue with logout, login
    // Todo

    public sealed partial class Admin : Page
    {
        ASI sys_admin;

        public Admin()
        {
            this.InitializeComponent();
            
        }

        private void add_new_user(object sender, RoutedEventArgs e)
        {
            string name = new_user.Text;
            string password = new_pwd.Text;
            string permissions = new_perm.Text;

            sys_admin.add_user_perm(name, password, permissions);

            popup_msg.Text = "Utilisateur Créé";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            sys_admin = (ASI)e.Parameter;

            // parameters.Name
            // parameters.Text
            // ...

            login_as.Text = "Connecté en tant que : " + sys_admin.get_current().Name;

            /*
             * Get user list on page init
             */
            user_list.ItemsSource = sys_admin.get_user_list();

            /*
             * Get msg list admin (all msg) on init
             */
            msg_list_admin.ItemsSource = sys_admin.get_message_list_admin();
        }

        private void clear_all_user_db(object sender, RoutedEventArgs e)
        {

            sys_admin.clear_user_table();

            if (sys_admin.get_user_by_id(1) == null)
            {
                sys_admin.add_user_perm("Admin", "root", "admin");
            }

            popup_msg.Text = "Utilisateurs supprimés";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void update_admin_pivot(object sender, SelectionChangedEventArgs e)
        {

            user_list.ItemsSource = sys_admin.get_user_list();
            msg_list_admin.ItemsSource = sys_admin.get_message_list_admin();

        }



        private void admin_logout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), sys_admin);
        }

        private void admin_send_button_Click(object sender, RoutedEventArgs e)
        {
            sys_admin.admin_send_new_msg(admin_new_msg_sender.Text, admin_new_msg_destination.Text, admin_new_msg_title.Text, admin_new_msg_message.Text);

            popup_msg.Text = "Message Envoyé";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void admin_delete_msg_id_Click(object sender, RoutedEventArgs e)
        {
            string msg_id = admin_msg_to_remove.Text;
            sys_admin.admin_rm_msg_id(int.Parse(msg_id));

            popup_msg.Text = "Message Supprimé";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void msg_list_admin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView recu = (ListView)sender;

            Message selected = (Message)recu.SelectedItem;

            if (selected != null)
            {
                popup_msg.Text = selected.message;
                if (!StandardPopup.IsOpen)
                {
                    StandardPopup.IsOpen = true;

                }
            }

        }

        private void ClosePopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        private void mod_user_button_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(mod_user.Text);
            User mod = sys_admin.get_user_by_id(id);

            new_user.Text = mod.Name;
            new_pwd.Text = mod.Pwd;
            new_perm.Text = mod.Permission;
            user_id.Text = id.ToString();
            
        }

        private void mod_user_final_Click(object sender, RoutedEventArgs e)
        {
            User mod = new User() { Id = int.Parse(user_id.Text), Name = new_user.Text, Permission = new_perm.Text, Pwd = new_pwd.Text };
            sys_admin.update_user(mod);

            popup_msg.Text = "Utilisateur Modifié";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void admin_clear_all_message_Click(object sender, RoutedEventArgs e)
        {
            sys_admin.clear_message_table();

            popup_msg.Text = "Messages Supprimés";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }

        private void mod_msg_button_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(mod_msg_id.Text);
            Message msg = sys_admin.get_message_id(id);
            if (msg != null)
            {

                admin_new_msg_sender.Text = msg.sender;
                admin_new_msg_destination.Text = msg.destination;
                admin_new_msg_title.Text = msg.titre;
                admin_new_msg_message.Text = msg.message;
                message_id.Text = msg.Id.ToString();
                message_font.Text = msg.font;

            }
        }

        private void mod_message_Click(object sender, RoutedEventArgs e)
        {
            Message mod_msg = new Message() { font = message_font.Text, message = admin_new_msg_message.Text, Id = int.Parse(message_id.Text), titre = admin_new_msg_title.Text, destination = admin_new_msg_destination.Text, sender = admin_new_msg_sender.Text };
            sys_admin.update_msg(mod_msg);

            popup_msg.Text = "Message Modifié";
            if (!StandardPopup.IsOpen)
            {
                StandardPopup.IsOpen = true;
            }
        }
    }
}
