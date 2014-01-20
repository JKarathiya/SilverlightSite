using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ContactForm
{
	public partial class ContactFormControl : UserControl
	{
		[System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Gets/sets the smtp server")]
        public string smtp
        {
            get;set;
        }

		[System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Gets/sets an email address whom to mail")]
        public string mailTo
        {
            get;set;
        }
        private Helper helpMe  = new Helper();
        public ContactFormControl()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                helpMe.Init();
                helpMe.MailSendResponse += new EventHandler<MailServerResponseArgs>(helpMe_MailSendResponse);
                helpMe.MailServerErrorReceived += new EventHandler<MailServerEventArgs>(helpMe_MailServerErrorReceived);
                helpMe.SendingErrorOccured += new EventHandler<MailServerResponseArgs>(helpMe_SendingErrorOccured);
            }
        }

        void helpMe_SendingErrorOccured(object sender, MailServerResponseArgs e)
        {
            successMessage.Text = "Mail transport error";
        }
        void helpMe_MailServerErrorReceived(object sender, MailServerEventArgs e)
        {
            successMessage.Text = "Mail transport error";
        }
        void helpMe_MailSendResponse(object sender, MailServerResponseArgs e)
        {
           switch(e.response) {
               case "1": successMessage.Text = "No data sent."; break;
               case "2": successMessage.Text = "Message has been sent."; break;
               case "3": successMessage.Text = "There have been problems sending your message."; break;
               default: successMessage.Text = "Uknown error occured."; break;
            }
        }
		private void SEND_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			 if ((name.Text != "") && (email.Text != "") && (Regex.IsMatch(email.Text, @"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,6})$")) && (subject.Text != "") && (message.Text != ""))
            {
                successMessage.Text = "Sending...";
                successMessage.Visibility = Visibility.Visible; 
                helpMe.data.Add(new MailData("to", mailTo));
                helpMe.data.Add(new MailData("smtp", smtp));
                helpMe.data.Add(new MailData("name", name.Text));
                helpMe.data.Add(new MailData("email", email.Text));
                helpMe.data.Add(new MailData("subject", subject.Text));
                helpMe.data.Add(new MailData("message", message.Text));
                helpMe.ToSendMessage();
            }
            else {
                successMessage.Visibility = Visibility.Visible;
                successMessage.Text = "Please correct all mistakes";
            }
		}
		private void CLEAR_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			name.Text = "";
			email.Text = "";
			subject.Text = "";
			message.Text = "";
		}
	}
}
