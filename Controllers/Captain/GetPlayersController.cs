using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LOG_Automation.Models;

namespace LOG_Server.Controllers.Captain
{
    [Route("api/getplayers")]
    [ApiController]
    public class GetPlayersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GetPlayersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(PlayerModel user)
        {
            string query = string.Empty;
            query = @"select UserName from dbo.Players where TeamName='"+user.TeamName+"' and Sport='"+user.Sport+"'";
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
