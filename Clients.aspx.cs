﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityKitchen
{
    public partial class Clients : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:Maximise()", true);
            if (!IsPostBack)
            {
                AddClientsToTable();
                AddStatesToDropDown();
            }
            //this.Form.Target = "_blank";
        }

        #region events
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected void btnAddNewClient_Click(object sender, EventArgs e)
        {
            AddClientToDB();
            AddClientsToTable();
            ClearFields();
        }

        protected void btnEditClient_Click(object sender, EventArgs e)
        {
            OpenEditClientScreen();
        }

        protected void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        #endregion events

        #region methods
        protected void AddStatesToDropDown()
        {
            try
            {
                CharityKitchenServiceReference.CKServiceSoapClient svc = new CharityKitchenServiceReference.CKServiceSoapClient();
                drpState.DataSource = svc.GetStateList();
                drpState.DataTextField = "StateName";
                drpState.DataValueField = "StateID";
                drpState.DataBind();
            }
            catch (Exception ex)
            {
                lblInfo.Text = ex.ToString();
            }
        }

        protected void AddClientsToTable()
        {
            CharityKitchenServiceReference.CKServiceSoapClient svc = new CharityKitchenServiceReference.CKServiceSoapClient();
            gvClientList.DataSource = svc.GetClientList();
            gvClientList.DataBind();
        }

        protected void AddClientToDB()
        {
            int state = drpState.SelectedIndex;

            CharityKitchenServiceReference.CKServiceSoapClient svc = new CharityKitchenServiceReference.CKServiceSoapClient();
            try 
            {                
                svc.AddNewClient(txtFirstName.Text, txtLastName.Text, Convert.ToDateTime(txtDOB.Text), txtPhoneNumber.Text, txtEmail.Text, txtAddress.Text, state, txtSuburb.Text, txtPostcode.Text);
                lblInfo.ForeColor = System.Drawing.Color.Green;
                lblInfo.Text = "*Client successfully added";
            }
            catch
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text = "*Please check all fields are entered correctly.";
            }
            
        }

        protected void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtDOB.Text = "";
            txtPhoneNumber.Text = "";
            txtEmail.Text = "";
            txtPostcode.Text = "";
            txtSuburb.Text = "";
            drpState.SelectedIndex = 0;
        }

        protected void OpenEditClientScreen()
        {
            try
            {
                CharityKitchenServiceReference.CKServiceSoapClient svc = new CharityKitchenServiceReference.CKServiceSoapClient();
                if (svc.ValidateClientID(Convert.ToInt32(txtClientID.Text)))
                {
                    HttpContext.Current.Items["idData"] = txtClientID.Text;//holds data from the client ID wanting to be edited
                    Server.Transfer("ClientEdit.aspx"); //sends the data to the page and opens page
                }
                else
                {
                    lblClientIDErrors.Text = "*Client ID not found, please enter valid ID";
                }
            }
            catch
            {
                lblClientIDErrors.Text = "*Client ID not found, please enter valid ID";
            }
        }

        private void Search()
        {
            CharityKitchenServiceReference.CKServiceSoapClient svc = new CharityKitchenServiceReference.CKServiceSoapClient();
            gvClientList.DataSource = svc.SearchClient(txtFirstNameSearch.Text, txtLastNameSearch.Text);
            gvClientList.DataBind();
        }
        #endregion methods


        protected void btnShowHelp_Click(object sender, EventArgs e)
        {
            txtHelp.Visible = true;
            HelpStrings help = new HelpStrings();
            txtHelp.Text = help.Clients;
        }
    }
}
