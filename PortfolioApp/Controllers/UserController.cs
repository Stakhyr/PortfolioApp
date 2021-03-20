using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using PortfolioApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace PortfolioApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UserController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select 
                            UsersId, 
                            FullName, 
                            Email,  
                            PhoneNumber,
                            convert(varchar(10),BirthDate,120) as BirthDate,
                            PhotoFileName,
                            UserRole

                        from dbo.Users";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PortfolioAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string query = @"
                        insert into dbo.Users
                        (FullName,Email,PhoneNumber,BirthDate,PhotoFileName, UserRole)
                        values 
                            (
                            '" + user.FullName + @"',
                            '" + user.Email + @"',
                            '" + user.PhoneNumber + @"',
                            '" + user.BirthDate + @"',
                            '" + user.PhotoFileName + @"',
                            '" + user.UserRole + @"'
                            )
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PortfolioAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfuly");
        }


        [HttpPut]
        public JsonResult Put(User user)
        {
            string query = @"
                        update dbo.Users set
                        FullName = '" + user.FullName + @"',    
                        Email = '" + user.Email + @"',
                        PhoneNumber = '" + user.PhoneNumber + @"',
                        BirthDate = '" + user.BirthDate + @"',
                        PhotoFileName = '" + user.PhotoFileName + @"',
                        UserRole = '" + user.UserRole + @"'
                        where UsersId = " + user.UserId + @"
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PortfolioAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfuly");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from dbo.Users 
                        where UsersId = " + id + @"
                        ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PortfolioAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successfuly");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() 
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception) 
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}

