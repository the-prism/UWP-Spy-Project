using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SQLite.Net.Attributes;
using System.Collections;
using Windows.UI.Text;

namespace CampExploUWP
{
    class ASI
    {
        string path;
        SQLite.Net.SQLiteConnection conn;
        User current;

        public ASI()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
            "db_user.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new
               SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            conn.CreateTable<User>();
            conn.CreateTable<Message>();
        }

        public User connect(string name, string password)
        {
            var query = conn.Table<User>();

            foreach (var user_row in query)
            {
                if (user_row.Name == name && user_row.Pwd == password)
                {
                    current = user_row;
                    return user_row;
                }
            }
            return null;
        }

        public User get_user_by_id(int i)
        {
            var query = conn.Table<User>();

            foreach (var user_row in query)
            {
                if (i == user_row.Id)
                {
                    return user_row;
                }
            }
            return null;
        }

        public Message get_message_id(int i)
        {
            var query = conn.Table<Message>();

            foreach (var item in query)
            {
                if(i == item.Id)
                {
                    return item;
                }
            }

            return null;
        }

        public string get_current_permisssion()
        {
            return current.Permission;
        }

        public User get_current()
        {
            return current;
        }

        public void add_user(string n, string p)
        {
            var s = conn.Insert(new User()
            {
                Name = n,
                Pwd = p,
                Permission = "none"
            });
        }

        public void add_user_perm(string n, string p, string mod)
        {
            var s = conn.Insert(new User()
            {
                Name = n,
                Pwd = p,
                Permission = mod
            });
        }

        public IList get_user_list()
        {
            var ls = conn.Table<User>();
            return ls.ToList();
        }

        public IList get_message_list_admin()
        {
            var ls = conn.Table<Message>();
            return ls.ToList();
        }

        public IList get_message_curent()
        {
            List<Message> msg_list = new List<Message>();
            var query = conn.Table<Message>();
            foreach (var message in query)
            {
                if (message.destination == current.Name)
                {
                    msg_list.Add(message);
                }
            }
            msg_list.Reverse();
            return msg_list;
        }

        public void clear_user_table()
        {
            conn.DropTable<User>();
            conn.CreateTable<User>();
        }

        public void clear_message_table()
        {
            conn.DropTable<Message>();
            conn.CreateTable<Message>();
        }

        public void admin_send_new_msg(string from, string to, string title, string msg)
        {
            var s = conn.Insert(new Message()
            {
                sender = from,
                destination = to,
                titre = title,
                message = msg,
                font = "bold"
            });
        }

        public void send_message(Message new_mess)
        {
            conn.Insert(new_mess);
        }

        public void admin_rm_msg_id(int id)
        {
            conn.Delete<Message>(id);
        }

        public void update_msg_id(int id_ed)
        {
            var query = from msg in conn.Table<Message>()
                        where msg.Id == id_ed
                        select msg;
            foreach (var msg in query)
            {
                msg.font = "normal";
                conn.InsertOrReplace(msg);
            }
            
        }

        public void update_msg(Message msg)
        {
            conn.InsertOrReplace(msg);
        }

        public void update_user(User nw_data)
        {
            conn.InsertOrReplace(nw_data);
        }
    }

    class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Permission { get; set; }
    }

    class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string sender { get; set; }
        public string destination { get; set; }
        public string titre { get; set; }
        public string message { get; set; }
        public string font { get; set; }
    }
}
