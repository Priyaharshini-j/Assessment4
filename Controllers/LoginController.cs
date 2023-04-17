using LiveDocument.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace LiveDocument.Controllers
{
    public class LoginController : Controller

    {

        IConfiguration _configuration;
        SqlConnection _Connection;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("LogIn"));
        }


        public List<LoginModel> GetUsers()
        {
            List<LoginModel> allUsers = new();
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("ListUsers", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                LoginModel login_user = new();
                login_user.Id = (int)dr[0];
                login_user.UserName = (string)dr[1];
                login_user.Email = (string)(dr[2]);
                login_user.ContactNo = Convert.ToInt64(dr[3]);
                login_user.Password = dr.GetString(4);
                allUsers.Add(login_user);
            }
            dr.Close();
            _Connection.Close();
            return allUsers;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            return View(GetUsers());
        }

        //For SignUp Page
        // GET: AdminController/Create
        public ActionResult SignUp()
        {
            return View();
        }

        void CreateUser(LoginModel login_user)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("CreateUser", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@User_name", login_user.UserName);
            cmd.Parameters.AddWithValue("@Email", login_user.Email);
            cmd.Parameters.AddWithValue("@ContactNo", login_user.ContactNo);
            cmd.Parameters.AddWithValue("@Password", login_user.Password);

            cmd.ExecuteNonQuery();
            _Connection.Close();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(LoginModel login)
        {
            try
            {
                CreateUser(login);
                return RedirectToAction(nameof(LogIn));
            }
            catch (Exception e)
            {
                Console.WriteLine($"We have faced some issues {e}");
                return View();
            }
        }
        
        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult LogIn()
        {
            return View();
        }

        int RetriveId(LoginModel log)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("Fetch_UserId", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", log.Email);
            cmd.Parameters.AddWithValue("@ContactNo", log.ContactNo);
            cmd.Parameters.AddWithValue("@UserName", log.UserName);
            SqlDataReader dr = cmd.ExecuteReader();
            int id = 0;
            while (dr.Read())
            {
                id = (int)dr[0];
            }
            dr.Close();
            _Connection.Close();
            return id;
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel login)
        {
            try
            {
                if (login.UserName == "admin12345" && login.Password == "psw")
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    int log_user = RetriveId(login);
                    return RedirectToAction("Index", "Users", new { log = log_user });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
