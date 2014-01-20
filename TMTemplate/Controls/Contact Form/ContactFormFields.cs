using System;
using System.Text.RegularExpressions;


namespace ContactForm
{
    public class ContactFormFields 
    {
 		private string fieldName;
        private string fieldEmail;
        private string fieldSubject;
        private string fieldMessage;

        public string namefld
        {

            get { return fieldName; }
            set
            {
                if (value == "" || value == null)
                {
                    throw new Exception("This is a requiered field");
                }
                fieldName = value;
            }
        }

        public string emailfld
        {
            get { return fieldEmail; }
            set
            {
                if (value == "" || value == null)
                {
                    throw new Exception("This is a requiered field");
                }
                else if (!Regex.IsMatch(value, @"^([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,6})$"))
                {
                    throw new Exception("This is not an email address");
                }
                fieldEmail = value;
            }
        }
        public string subjectfld
        {
            get { return fieldSubject; }
            set
            {
                if (value == "" || value == null)
                {
                    throw new Exception("This is a requiered field");
                }
                fieldSubject = value;
            }
        }

        public string messagefld
        {
            get { return fieldMessage; }
            set
            {
                if (value == "" || value == null)
                {
                    throw new Exception("This is a requiered field");
                }
                fieldMessage = value;
            }
        }
    }
}
