using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiVK
{
    static class Program
    {
        public static int appId = 5895019;
        public enum VkontakteScopeList // список прав
        {
            notify = 1, // користувач дозволив надсилати йому сповіщення
            friends = 2, // доступ до друзів
            photos = 4,
            audio = 8,
            video = 16,
            offers = 32,
            questions = 64,
            pages = 128, // доступ до wiki-сторінок
            link = 256, // додавання посилання на додаток в меню зліва
            notes = 2048,
            messages = 4096, // доступ до розширених методів роботи з повідомленнями
            wall = 8192, // доступ до методів роботи зі стіною
            docs = 131072
        }
        public static int scope = (int)(VkontakteScopeList.audio | VkontakteScopeList.docs | VkontakteScopeList.friends | VkontakteScopeList.link | VkontakteScopeList.messages | VkontakteScopeList.notes | VkontakteScopeList.notify | VkontakteScopeList.offers | VkontakteScopeList.pages | VkontakteScopeList.photos | VkontakteScopeList.questions | VkontakteScopeList.video | VkontakteScopeList.wall);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
