using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ApiVK
{
    public partial class Form2 : Form
    {
        string accessToken;
        Dictionary<int, string> sex = new Dictionary<int, string>()
        {
            {1, "жiночий" },
            {2, "чоловiчий" }
        };
        private XmlDocument ExecuteCommand(string name, NameValueCollection qs)
        {
            XmlDocument result = new XmlDocument();
            result.Load(String.Format("https://api.vk.com/method/{0}.xml?{2}&access_token={1}", name, accessToken, String.Join("&", from item in qs.AllKeys select item + "=" + qs[item])));
            return result;
        }
        public string GetDataFromXmlNode(XmlNode input)
        {
            if (input == null || String.IsNullOrEmpty(input.InnerText))
            {
                return "no information";
            }
            else
            {
                return input.InnerText;
            }
        }
        public XmlDocument GetProfile(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["user_ids"] = uid.ToString();
            qs["fields"] = "photo_200, sex, bdate, city, country, home_town, online, contacts, site, education, universities, schools, status, occupation, nickname, relatives, relation, personal, connections, activities, interests, music, movies, tv, books, games, about, timezone, screen_name, maiden_name, career, military";
            return ExecuteCommand("users.get", qs);
        }
        public string GetCity(int id)
        {
            if (id <= 0) { return "no information"; }
            NameValueCollection qs = new NameValueCollection();
            qs["api_id"] = Program.appId.ToString();
            qs["cids"] = id.ToString();
            XmlDocument city = ExecuteCommand("getCities", qs);
            return city.SelectSingleNode("response/city/name").InnerText;
        }
        public string GetCountry(int id)
        {
            if (id <= 0) { return "no information"; }
            NameValueCollection qs = new NameValueCollection();
            qs["api_id"] = Program.appId.ToString();
            qs["cids"] = id.ToString();
            XmlDocument country = ExecuteCommand("getCountries", qs);
            return country.SelectSingleNode("response/country/name").InnerText;
        }
        public XmlDocument GetFriends(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["user_id"] = uid.ToString();
            qs["order"] = "name";
            qs["count"] = "30";
            qs["fields"] = "nickname, sex, bdate, city, country, timezone, photo_100, contacts, education, online, relation, status, universities";
            return ExecuteCommand("friends.get", qs);
        }
        public Form2(string token, int id)
        {
            InitializeComponent();
            accessToken = token;
            textBox1.Text = id.ToString();
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            pictureBox1.Visible = false;
            listBox1.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument profile = GetProfile(Convert.ToInt32(textBox1.Text));
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            pictureBox1.Visible = true;
            listBox1.Visible = true;
            textBox2.Text = GetDataFromXmlNode(profile.SelectSingleNode("response/user/first_name"));
            textBox3.Text = GetDataFromXmlNode(profile.SelectSingleNode("response/user/last_name"));
            textBox4.Text = (from x in sex
                             where (Convert.ToInt32(GetDataFromXmlNode(profile.SelectSingleNode("response/user/sex"))) == x.Key)
                             select x.Value).FirstOrDefault();
            textBox5.Text = GetDataFromXmlNode(profile.SelectSingleNode("response/user/bdate"));
            textBox6.Text = GetCountry(Convert.ToInt32(GetDataFromXmlNode(profile.SelectSingleNode("response/user/country"))));
            textBox7.Text = GetCity(Convert.ToInt32(GetDataFromXmlNode(profile.SelectSingleNode("response/user/city"))));
            pictureBox1.Image = Image.FromStream(new MemoryStream(new WebClient().DownloadData(GetDataFromXmlNode(profile.SelectSingleNode("response/user/photo_200")))));
            XmlDocument friends = GetFriends(Convert.ToInt32(textBox1.Text));
            XmlNodeList nodeList = friends.SelectNodes("response/user/last_name");
            (from x in nodeList.Cast<XmlNode>()
             select GetDataFromXmlNode(x)).ToList().ForEach(elem => listBox1.Items.Add(elem));
        }
    }
}
