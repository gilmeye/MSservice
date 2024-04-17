using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace WebService1
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Bank : System.Web.Services.WebService
    {

        const String FileName = "Web_Service.accdb";
        public static string GetConnectionString()
        {
            string location = HttpContext.Current.Server.MapPath("~/Service_Data/" + FileName);
            string connectionString = "provider=Microsoft.ACE.OleDb.12.0;data source=" + location;
            return connectionString;
        }


        public static DataTable selectFromDb(string sqlStr)
        {
            OleDbConnection myConnection = new OleDbConnection();
            myConnection.ConnectionString = GetConnectionString();
            OleDbCommand myCmd = new OleDbCommand(sqlStr, myConnection);
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = myCmd;
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }


        public static void sqlUdi(string sqlStr)
        {
            OleDbConnection myConnection = new OleDbConnection();
            myConnection.ConnectionString = GetConnectionString();
            OleDbCommand myCmd = new OleDbCommand(sqlStr, myConnection);
            myConnection.Open();
            myCmd.ExecuteNonQuery();
            myConnection.Close();
        }



        [WebMethod]
        public void AddAccount(int id, string cnam, int cvc, int balance)
        {
            string st = string.Format(@"Insert Into tblBank(ID, CardNum, CVC, Balance) 
            Values({0}, '{1}', {2}, {3})", id, cnam, cvc, balance);
            sqlUdi(st);
        }

        [WebMethod]
        public bool DoesExist(int id, string cnum, int cvc)
        {
            string st = string.Format(@"Select tblBank.CardNum, tblBank.CVC From tblBank Where tblBank.ID = {0}", id);
            DataTable dt = selectFromDb(st);
            if (dt.Rows[0][0] == null) { return false; }
            string n1 = dt.Rows[0][0].ToString();
            int c1 = int.Parse(dt.Rows[0][1].ToString());
            return (n1 == cnum)&&(c1 == cvc);
        }

        [WebMethod]
        public bool HasAmount(int id, int price)
        {
            int balance = GetBalance(id);
            return balance >= price;
        }

        [WebMethod]
        public void Pay(int id, int price) 
        {
            string st = string.Format(@"Update tblBank set tblBank.Balance = tblBank.Balance - {0} Where tblBank.ID = {1}", price, id);
            sqlUdi(st);
        }

        [WebMethod]
        public int GetBalance(int id)
        {
            string st = string.Format(@"Select tblBank.Balance From tblBank Where tblBank.ID = {0}", id);
            DataTable dt = selectFromDb(st);
            int balance = int.Parse(dt.Rows[0][0].ToString());
            return balance;

        }

        [WebMethod]
        public void AddToBalance(int id, int mulah)
        {
            string st = string.Format(@"Update tblBank set tblBank.Balance = tblBank.Balance + {0} Where tblBank.ID = {1}", mulah, id);
            sqlUdi(st);
        }   

    }
}
