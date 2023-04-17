using LiveDocument.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace LiveDocument.Controllers
{
    public class UsersController : Controller
    {
        IConfiguration _configuration;
        SqlConnection _Connection;
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("Document"));
        }

        public List<UsersModel> GetDocument(int login)
        {
            List<UsersModel> alldocs = new();
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("FetchDocuments", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", login);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                UsersModel user = new();
                user.DocId = (int)dr[0];
                user.UserName = (string)dr[2];
                user.Email = (string)(dr[3]);
                user.Title = ""+(dr[6]);
                alldocs.Add(user);
            }
            dr.Close();
            _Connection.Close();
            return alldocs;
        }
        // GET: UsersController
        public ActionResult Index(int log)
        {
            return View(GetDocument(log));
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View(GetDocumentbyId(id));
        }

        UsersModel GetDocumentbyId(int id)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("GetDocbyId", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocId", id);
            SqlDataReader dr = cmd.ExecuteReader();
            UsersModel user = new();
            while (dr.Read())
            {
                user.DocId = (int)dr[0];
                user.Id = (int)dr[1];
                user.UserName = (string)dr[2];
                user.Email = (string)(dr[3]);
                user.ContactNo = Convert.ToInt64(dr[4]);
                user.Password = dr.GetString(5);
                user.Title = (string)dr[6];
                user.Content = (string)dr[7];
            }
            dr.Close();
            _Connection.Close();
            return user;
        }
        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        void InsertAppointment(UsersModel user)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("InsertDocument", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Title", user.Title);
            cmd.Parameters.AddWithValue("@Content", user.Content);
            cmd.ExecuteNonQuery();
            _Connection.Close();
        }
        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersModel user)
        {
            try
            {
                InsertAppointment(user);
                return RedirectToAction("Index", "Users", new { log = user.Id });
            }
            catch
            {
                return View();
            }
        }
        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(GetDocumentbyId(id));
        }
        void UpdateDocument(int DocId, UsersModel user)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("UpdateDocument", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocId", DocId);
            cmd.Parameters.AddWithValue("@Title", user.Title);
            cmd.Parameters.AddWithValue("@Content", user.Content);
            cmd.ExecuteNonQuery();
            _Connection.Close();
        }
        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsersModel user)
        {
            try
            {
                UpdateDocument(id, user);
                return RedirectToAction("Index", "Users", new { log = user.Id });
            }
            catch
            {
                return View();
            }
        }


        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
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
