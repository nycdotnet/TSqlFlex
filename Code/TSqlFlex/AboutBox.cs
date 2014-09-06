using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TSqlFlex
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            string productName = "T-SQL Flex";
            string companyName = "LegendaryApps.com";
            this.Text = String.Format("About {0}", productName);
            this.labelProductName.Text = productName;
            this.labelVersion.Text = String.Format("Version {0}", TSqlFlex.Core.Info.Version());
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = companyName;
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTwitter_Click(object sender, EventArgs e)
        {
            launchUrl("https://twitter.com/nycdotnet/");
        }

        private void btnIssues_Click(object sender, EventArgs e)
        {
            launchUrl("https://github.com/nycdotnet/TSqlFlex/issues");
        }

        private void btnReleases_Click(object sender, EventArgs e)
        {
            launchUrl("https://github.com/nycdotnet/TSqlFlex/releases");
        }

        private void launchUrl(string url)
        {
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = url;
            proc.Start();
        }


    }
}
