using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleWebForms
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GreetButton_Click(object sender, EventArgs e)
        {
            var name = NameTextBox.Text == null ? string.Empty : NameTextBox.Text.Trim();
            ViewState["UserName"] = name;

            if (string.IsNullOrEmpty(name))
            {
                GreetingLabel.Text = "Please enter a name.";
            }
            else
            {
                // HtmlEncode to avoid XSS if user enters markup
                GreetingLabel.Text = "hello " + Server.HtmlEncode(name);
            }
        }
    }
}