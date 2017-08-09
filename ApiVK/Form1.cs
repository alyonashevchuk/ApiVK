using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ApiVK
{
    public partial class Form1 : Form
    {
        string accessToken;
        int userId;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(String.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", Program.appId, Program.scope));
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            accessToken = (from x in myReg.Matches(e.Url.ToString()).Cast<Match>()
                                  where x.Groups["name"].Value == "access_token"
                                  select x.Groups["value"].Value).FirstOrDefault();
            userId = Convert.ToInt32((from x in myReg.Matches(e.Url.ToString()).Cast<Match>()
                                          where x.Groups["name"].Value == "user_id"
                                          select x.Groups["value"].Value).FirstOrDefault());
            if (accessToken != null)
            {
                Form2 f = new Form2(accessToken, userId);
                f.ShowDialog();
            }
        }
    }
}
