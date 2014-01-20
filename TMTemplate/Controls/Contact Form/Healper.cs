using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Controls;
using TMTemplate;


namespace ContactForm
{
    public class Helper : Panel
    {
        private WebClient client;
        private HttpWebRequest request;
        public enum ServerType {PHP, ASP, NON};
        public ServerType serverType;
        public string transportUrl;
        public int trys = 0;
        public List<MailData> data = new List<MailData>();    
 
        public event EventHandler<MailServerEventArgs> MailServerResponseReceived;
        public event EventHandler<MailServerEventArgs> MailServerErrorReceived;
        public event EventHandler<EventArgs> NoMailTransportDetermined;
        public event EventHandler<MailServerResponseArgs> MailSendResponse;
        public event EventHandler<MailServerResponseArgs> SendingErrorOccured;

        public Helper() {
            this.NoMailTransportDetermined += new EventHandler<EventArgs>((s, e) =>{ });//??
        }

        public void Init() {

            string docRoot = App.Current.Host.Source.OriginalString;
            AssemblyName an = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            docRoot = docRoot.Replace("ClientBin/" + an.Name + ".xap", "");

            transportUrl = docRoot;
            client = new WebClient();
            serverType = ServerType.PHP;

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            MailServerErrorReceived += new EventHandler<MailServerEventArgs>(Helper_MailServerErrorReceived);
            MailServerResponseReceived += new EventHandler<MailServerEventArgs>(Helper_MailServerResponseReceived);
            GetServerType();
        }

        void Helper_MailServerResponseReceived(object sender, MailServerEventArgs e)
        {
            
            if (trys < 2)
             {
                switch (e.response)
                {
                    case "PHP Confirmed": serverType = ServerType.PHP; break;
                    case "ASP Confirmed": serverType = ServerType.ASP; break;
                    default:
                        if (serverType == ServerType.ASP) serverType = ServerType.PHP;
                        else serverType = ServerType.ASP;
                        GetServerType();
                        trys++;
                    break;
                }
            }
            else serverType = ServerType.NON;
        }

        void Helper_MailServerErrorReceived(object sender, MailServerEventArgs e)
        {
                if (trys > 2)
                {
                    NoMailTransportDetermined.Invoke(this, new EventArgs());
                }
                else
                {
                    if (serverType == ServerType.ASP) serverType = ServerType.PHP;
                    else serverType = ServerType.ASP;
                    GetServerType();
                    trys++;
                }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null) Console.Write("Using WebClient: " + e.Result);
            else
            {
                if (trys > 2) {
                    NoMailTransportDetermined.Invoke(this, new EventArgs());
                }
                else
                {
                    if (serverType == ServerType.ASP) serverType = ServerType.PHP;
                    else serverType = ServerType.ASP;
                    GetServerType();
                    trys++;
                }
            } 
        }
       
        public void GetServerType(){
            switch (serverType) {
                case ServerType.ASP:
                    Dispatcher.BeginInvoke(delegate()
                    {
                        try
                        {
                            request = (HttpWebRequest)HttpWebRequest.Create(transportUrl + "MailHandler.ashx?server=check");
                            request.BeginGetResponse(new AsyncCallback(ReadMailServerCallBack), request);
                        }
                        catch (Exception e) {
                            MailServerErrorReceived.Invoke(request, new MailServerEventArgs(e));
                        }
                    });
                    break;
                case ServerType.PHP:
                    Dispatcher.BeginInvoke(delegate()
                    {
                        try
                        {
                            request = (HttpWebRequest)HttpWebRequest.Create(transportUrl + "MailHandler.php?server=check");
                            request.BeginGetResponse(new AsyncCallback(ReadMailServerCallBack), request);
                        }
                        catch (Exception e) {
                            MailServerErrorReceived.Invoke(request, new MailServerEventArgs(e));
                        }
                    });

                    break;
                default:
                    Dispatcher.BeginInvoke(delegate()
                    {
                        try
                        {
                            client.DownloadStringAsync(new Uri(transportUrl + "MailHandler.php?server=check", UriKind.Absolute));
                            request = (HttpWebRequest)HttpWebRequest.Create(transportUrl + "MailHandler.php?server=check");
                            request.BeginGetResponse(new AsyncCallback(ReadMailServerCallBack), request);
                        }
                        catch (Exception e)
                        {
                            MailServerErrorReceived.Invoke(request, new MailServerEventArgs(e));
                        }
                    });
                    break;
            }
        }

        private void ReadMailServerCallBack(IAsyncResult asynchronousResult)
        {
            try
            {
                request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string resultString = streamReader.ReadToEnd();
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        MailServerResponseReceived.Invoke(this, new MailServerEventArgs(resultString));
                    });
                }
            }
            catch (Exception e)
            {
                MailServerErrorReceived.Invoke(request, new MailServerEventArgs(e));
            }
        }

        public void ToSendMessage()
        {
            string transportUri = "";
            if (serverType == ServerType.ASP) transportUri = transportUrl + "MailHandler.ashx?server=send";
            else if (serverType == ServerType.PHP) transportUri = transportUrl + "MailHandler.php?server=send";
            try
            {
                WebRequest request = (WebRequest)WebRequest.Create(new Uri(transportUri, UriKind.Absolute));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream(new AsyncCallback(delegate(IAsyncResult asyncResult)
                {  
                    StreamWriter writer = new StreamWriter(request.EndGetRequestStream(asyncResult));  
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (i != 0) writer.Write("&"); 
                        writer.Write(data[i].key + "=" + data[i].value);
                    }
                    writer.Flush();
                    writer.Close();
                    request.BeginGetResponse(new AsyncCallback(delegate(IAsyncResult asyncResult2)
                    {
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult2);
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                string resultString = streamReader.ReadToEnd();
                                this.Dispatcher.BeginInvoke(delegate()
                                {
                                    MailSendResponse.Invoke(this, new MailServerResponseArgs(resultString));
                                });
                            }
                        }
                        catch (Exception exc) {

                            SendingErrorOccured.Invoke(this, new MailServerResponseArgs(exc));
                        }
                    }), request);
                }), request);
            }
            catch (Exception exc)
            {
                SendingErrorOccured.Invoke(this, new MailServerResponseArgs(exc));
            }
        }
    }

    public class MailServerResponseArgs : EventArgs {
        public string response = "";
        public Exception error = new Exception();
        public MailServerResponseArgs(string rsp)
        {
            response = rsp;
        }

        public MailServerResponseArgs(Exception exc)
        {
            error = exc;
        }
    }

    public class MailServerEventArgs : EventArgs {
        public Exception error;
        public string response;
        public MailServerEventArgs(Exception serverError)
        {
            error = serverError;
        }

        public MailServerEventArgs(string sResponse)
        {
            response = sResponse;
        }
    }

    public class MailData {
        public string key {get; set;}
        public string value { get; set; }
        public MailData(string k, string v) {
            key = k;
            value = v;
        }
    }
}
