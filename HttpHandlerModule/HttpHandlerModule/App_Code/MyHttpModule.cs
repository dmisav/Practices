using System;
using System.Web;

namespace HttpHandlerModule.App_Code
{
    public class MyHttpModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
            context.EndRequest += new EventHandler(context_EndRequest);
            context.AuthorizeRequest += new EventHandler(context_AuthorizeRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }

        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            //We change uri for invoking correct handler
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.Request.RawUrl.Contains(".bspx"))
            {
                string url = context.Request.RawUrl.Replace(".bspx", ".aspx");
                context.RewritePath(url);
            }
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //We set back the original url on browser
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.Items["originalUrl"] != null)
            {
                context.RewritePath((string)context.Items["originalUrl"]);
            }
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            //We processed the request
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            //We received a request, so we save the original URL here
            HttpContext context = ((HttpApplication)sender).Context;

            if (context.Request.RawUrl.Contains(".bspx"))
            {
                context.Items["originalUrl"] = context.Request.RawUrl;
            }
        }

    }
}
