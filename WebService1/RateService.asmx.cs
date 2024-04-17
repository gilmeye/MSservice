using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace WebService1
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class RateService : System.Web.Services.WebService
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
            DataTable dataTable = new DataTable{ TableName = "tblRate" };
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
        public void AddRating(string name, int design, int comfort, int products, int service)
        {
            string st = string.Format(@"INSERT INTO tblRate (UserName, Design, Comfort, Products, Service) 
            VALUES ('{0}', {1}, {2}, {3}, {4})", name, design, comfort, products, service);
            sqlUdi(st);
        }

        [WebMethod]
        
        public DataTable ShowRating()
        {
            string st = @"Select * From tblRate;";
            DataTable dt = selectFromDb(st);
            return dt;
        }

    }
}
