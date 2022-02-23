using LOG_Automation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LOG_Automation.Controllers
{
    [Route("api/registercaptain")]
    [ApiController]
    public class RegisterCaptainController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegisterCaptainController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(Captain user)
        {
            string query = string.Empty;
            query = @"INSERT INTO dbo.Captains(UserName,UserPassword,TeamName) values ('" + user.UserName + "','" + user.UserPassword + "','"+user.TeamName+"');";
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

            return new JsonResult("Added Successfuly");
        }
    }
}
