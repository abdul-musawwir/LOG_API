using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using LOG_Automation.Models;
using System.Diagnostics;

namespace LOG_Automation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SignInController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            Debug.WriteLine(user.UserName);
            string query = string.Empty;
            if(user.Role == "admin")
                query = @"select UserName from  dbo.Admin_Users where UserName ='"+user.UserName+"' AND UserPassword ='"+user.UserPassword+"'";
            else if (user.Role == "captain")
                query = @"select UserName,TeamName from  dbo.Captains where UserName ='" + user.UserName + "' AND UserPassword ='" + user.UserPassword + "'";
            else if (user.Role == "player")
                query = @"select UserName,TeamName,CaptainName,Sport from  dbo.Players where UserName ='" + user.UserName + "' AND UserPassword ='" + user.UserPassword + "'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LOGAutomationCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count != 0)
            {

            table.Columns.Add("Token");
            table.Rows[0]["Token"] = "1234";
            }
            foreach (DataRow dataRow in table.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Debug.WriteLine(item);
                }
            }
            return new JsonResult(table);
        }
    }
}
