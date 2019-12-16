using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamC_.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ExamC_.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context){
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");

        }
        [HttpPost("login")]
        public IActionResult Login(UsersWrapper user){
            LoginUsers SubUser=user.LoginUser;
            if(ModelState.IsValid)
            {
                Users userInDb = dbContext.users.FirstOrDefault(u => u.Email == SubUser.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginUser.Email", "Invalid Email");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUsers>();
                var result = hasher.VerifyHashedPassword(SubUser, userInDb.Password, SubUser.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginUser.Password", "Password is wrong");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userid",userInDb.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }
        }
            
        [HttpPost("register")]
        public IActionResult Register(UsersWrapper user){
            Users SubUser=user.NewUser;
            if( ModelState.IsValid){
                bool ExUser=dbContext.users.Any(t=>t.Email==SubUser.Email);
                if(ExUser==true){
                    ModelState.AddModelError("NewUser.Email","email already exists");
                    return View("Index");
                }
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                SubUser.Password = Hasher.HashPassword(SubUser, SubUser.Password);
                dbContext.users.Add(SubUser);
                dbContext.SaveChanges();
                Users CurUser=dbContext.users.Last();
                HttpContext.Session.SetInt32("userid",CurUser.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }          
        }
        // success is dashboard ***************************************************
        [HttpGet("success")]
        public IActionResult Success(){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            bool Exists=dbContext.users.Any(e=>e.UserId==(int)y);
            if(Exists==false){
                return RedirectToAction("Index");
            }
            Users CurUser=dbContext.users.FirstOrDefault(e=>e.UserId==(int)y);
            ViewBag.UserName=CurUser.Name;
            ViewBag.UserId=(int)y;
            // ViewBag.CurUser=CurUser;
            List<Acitivites> AllAct=dbContext.acitivites.Include(t=>t.Creator).Include(x=>x.UsersAtEvent).ThenInclude(l=>l.User).ToList();
            ViewBag.AllAct=dbContext.acitivites.Include(t=>t.Creator).Include(x=>x.UsersAtEvent).ThenInclude(l=>l.User).ToList();
            return View(AllAct);
        }
        [HttpGet("addactivity")]
        public IActionResult AddActivity(){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            ViewBag.UserId=(int)y;


            return View("AddActivity");
        }
        [HttpPost("createactivity")]
        public IActionResult CreateActivity(Acitivites NewActivity){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            ViewBag.UserId=(int)y;
            if(NewActivity==null){
                return RedirectToAction("AddActivity");
            }
            if(ModelState.IsValid){
                if(NewActivity.Date<DateTime.Today){
                    ModelState.AddModelError("Date","Well, Well, Well, look whos Doctor Who");
                    return View("AddActivity");
                }
                dbContext.acitivites.Add(NewActivity);
                dbContext.SaveChanges();
                Acitivites x=dbContext.acitivites.Last();
                return RedirectToAction("ActivityInfo", new{id=x.ActivityId});
            }



            return View("AddActivity");
        }
        [Route("join/{activityId}/{userID}")]
        public IActionResult Join(int activityId, int userID){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            if(userID!=(int)y){return RedirectToAction("Success");}
            bool Check=dbContext.guests.Any(x=>x.UserId==userID && x.ActivityId==activityId);
            if(Check==true){
                return RedirectToAction("Success");
            }
            Guests guest= new Guests{ActivityId=activityId,UserId=userID};
            dbContext.guests.Add(guest);
            dbContext.SaveChanges();


            return RedirectToAction("Success");
        }
        [Route("leave/{activityId}/{userID}")]
        public IActionResult Leave(int activityId, int userID){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            if(userID!=(int)y){return RedirectToAction("Success");}
            bool Check=dbContext.guests.Any(x=>x.UserId==userID && x.ActivityId==activityId);
            if(Check==false){
                return RedirectToAction("Success");
            }
            Guests guest= dbContext.guests.FirstOrDefault(x=>x.ActivityId==activityId && x.UserId==userID);
            dbContext.guests.Remove(guest);
            dbContext.SaveChanges();


            return RedirectToAction("Success");
        }
        [Route("delete/{activityId}/{userID}")]
        public IActionResult Delete(int activityId, int userID){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            if(userID!=(int)y){return RedirectToAction("Success");}
            bool Check=dbContext.acitivites.Any(x=>x.CreatorId==userID && x.ActivityId==activityId);
            if(Check==false){
                return RedirectToAction("Success");
            }
            Acitivites NotActivity= dbContext.acitivites.FirstOrDefault(x=>x.ActivityId==activityId && x.CreatorId==userID);
            dbContext.acitivites.Remove(NotActivity);
            dbContext.SaveChanges();


            return RedirectToAction("Success");
        }
        [HttpGet("activity/{id}")]
        public IActionResult ActivityInfo(int id){
            int? y=HttpContext.Session.GetInt32("userid");
            if (y==null){
                return RedirectToAction("Index");
            }
            ViewBag.UserId=(int)y;
            bool L=dbContext.acitivites.Any(m=>m.ActivityId==id);
            if(L==false){
                return RedirectToAction("Success");
            }
            Acitivites x=dbContext.acitivites.Include(t=>t.Creator).Include(o=>o.UsersAtEvent).ThenInclude(p=>p.User).FirstOrDefault(l=>l.ActivityId==id);
            
            
            return View("ActivityInfo",x);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
