<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Net.Mail;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {


        switch (context.Request.QueryString.Get("server")) {

            case "check": context.Response.Write("ASP Confirmed"); break;
            case "send": {


                SmtpClient mailClient = new SmtpClient(context.Request.Form.Get("smtp"));
                string to = context.Request.Form.Get("to");
                string from = context.Request.Form.Get("email");
                string subject = context.Request.Form.Get("subject");
                string messageBody = "";
                
                
                for (int i = 0; i < context.Request.Form.Keys.Count; i++) {
                    if ((context.Request.Form.GetKey(i) == "to") || (context.Request.Form.GetKey(i) == "email") || (context.Request.Form.GetKey(i) == "smtp")) continue;
                    else messageBody += context.Request.Form.GetKey(i) +":<br/>" + context.Request.Form.Get(i) + "<br/>";
                }

                MailMessage message = new MailMessage();
                try
                {
                    message.From = new MailAddress(from.ToString());
                }
                catch (FormatException e) {
                    context.Response.Write(e.Message);
                }
                message.To.Add(to);
                message.Subject = subject;
                message.Body = messageBody;
                
                message.IsBodyHtml = true;
                try
                {
                    mailClient.Send(message);
                }
                catch (SmtpException e) {

                    context.Response.Write("3");
                
                }
                
                context.Response.Write("2");
           
                
                
            }   break;
            default: context.Response.Write("1"); break;
               
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    
   

}