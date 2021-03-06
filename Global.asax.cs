﻿using CharityKitchen.CharityKitchenServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CharityKitchen
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            CharityKitchen.CharityKitchenServiceReference.User u = new CharityKitchen.CharityKitchenServiceReference.User(); //Creates a new dummy user
            u.Roles = new RoleCombo[0];

            Session["user"] = u; //Stores dummy user in the session
        }
    }
}