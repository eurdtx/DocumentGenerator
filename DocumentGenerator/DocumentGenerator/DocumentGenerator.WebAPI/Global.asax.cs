﻿using System;
using System.Web.Http;
using DocumentGenerator.WebAPI.Startup;

namespace DocumentGenerator.WebAPI
{
  public class Global : System.Web.HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);
    }
  }
}