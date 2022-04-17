using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyForum.Core.Interfaces.Repositories;
using MyForum.Data.Models;
using MyForum.Data.Repository.Repositories;
using MyForum.Extensions;
using MyForum.ViewModels;
using MyForum.Core.Models;
using System.Net.Mail;
using System.Net;

namespace MyForum.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _allUsers;
        private MyForumContext _context;
        private PostRepository _postRepository;
        private TopicRepository _topics;
        private MarkRepository _markRepository;
        private CommentRepository _commentRepository;
        private readonly UserRepository _userRepository;

        public UserController(IUserRepository iAllUsers, MyForumContext context)
        {
            _allUsers = iAllUsers;
            _context = context;
            _topics = new TopicRepository(_context);
            _userRepository = new UserRepository(_context);
            _postRepository = new PostRepository(_context);
            _markRepository = new MarkRepository(_context);
            _commentRepository = new CommentRepository(_context);
        }

        // Метод перехода на страницу для входа
        [HttpGet]
        [Route("~/")]
        [Route("~/User/Login")]
        public IActionResult Login()
        {
            return View();
        }

        // Метод 
        [Route("~/User/LoginUser")]
        public IActionResult LoginUser(User user)
        {
            if (HttpContext.Session.Keys.Contains("user") == true)
            {
                HttpContext.Session.Remove("user");
            }

            if(user.Email == null || !Regex.IsMatch(user.Email,@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}"))
            {
                ModelState.AddModelError("Email", "Uncorrect email");

                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (CheckExist(user.Email) == false)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if (CheckPass(user.Email, user.Password) == false || user.Password == null)
            {
                ModelState.AddModelError("Password", "Uncorrect password");

                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            user = _userRepository.GetAll().FirstOrDefault(u => 
                    string.Compare(u.Email, user.Email) == 0 
                    && 
                    string.Compare(u.Password, user.Password) == 0);

            HttpContext.Session.Set("user", user);

            ViewBag.UserLogined = user;

            return RedirectToRoute(new { controller = "Home", action = "TopicsList" });
        }

        [HttpGet]
        [Route("~/User/Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        // Метод 
        [Route("~/User/RegistrationUser")]
        public IActionResult RegistrationUser(User user)
        {
            if (user.Email.Length <= 0)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (CheckExist(user.Email) == true)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Password.Length < 8 || user.Password == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Address.Length <= 0 || user.Address == null)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            if (user.Age <= 0)
            {
                return RedirectToRoute(new { controller = "User", action = "Registration" });
            }

            user.Created = DateTime.Now.ToShortDateString();
            user.IsVerified = false;

            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [HttpGet]
        [Route("~/User/Profile")]
        public IActionResult Profile()
        {
            if (HttpContext.Session.Keys.Contains("user") != true)
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            if(HttpContext.Session.Get<User>("user").Picture == null)
            {
                string imagepath = Path.GetFullPath(@"wwwroot\images\default.png");
                FileStream fs = new FileStream(imagepath, FileMode.Open);
                byte[] byData = new byte[fs.Length];
                fs.Read(byData, 0, byData.Length);

                var base64 = Convert.ToBase64String(byData);
                ViewBag.Picture = String.Format("data:image/jpg;base64,{0}", base64);

                fs.Close();
            }
            else
            {
                var base64 = Convert.ToBase64String(HttpContext.Session.Get<User>("user").Picture);
                ViewBag.Picture = String.Format("data:image/jpg;base64,{0}", base64);
            }

            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;
            ViewBag.User = HttpContext.Session.Get<User>("user");
            ViewBag.UsersPosts = _postRepository.GetAll().Where(p => p.UserId == HttpContext.Session.Get<User>("user").Id);

            return View();
        }

        [HttpGet]
        [Route("~/User/ChangePass")]
        public IActionResult ChangePass()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            return View();
        }

        [Route("~/User/SentNewPass")]
        public IActionResult SentNewPass(ChangePassViewModel pass)
        {
            if(String.Compare(pass.oldPass, HttpContext.Session.Get<User>("user").Password, StringComparison.Ordinal) != 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if (pass.newPass.Length <= 8)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if(pass.newPassConfign.Length <= 8 || pass.newPassConfign.Length != pass.newPass.Length)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if (pass.newPass.CompareTo(pass.newPassConfign) != 0) 
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            if(pass.oldPass.CompareTo(pass.newPass) == 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangePass" });
            }

            User user = HttpContext.Session.Get<User>("user");

            user.Password = pass.newPass;

            _context.User.Update(user);
            _context.SaveChanges();

            HttpContext.Session.Remove("user");
            HttpContext.Session.Set("user", user);

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }

        [HttpPost]
        public ActionResult AddPicture(Picture pic, IFormFile uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.OpenReadStream()))
                {
                    long f = uploadImage.Length;
                    int c = Convert.ToInt32(uploadImage.Length);
                    imageData = binaryReader.ReadBytes(c);
                }

                User u = HttpContext.Session.Get<User>("user");
                u.Picture = imageData;

                _context.User.Update(u);
                _context.SaveChanges();

                HttpContext.Session.Set<User>("user", u);

                return RedirectToRoute(new { controller = "User", action = "Profile" });
            }

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }

        [HttpGet]
        [Route("~/User/ChangeOwnData")]
        public IActionResult ChangeOwnData()
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;
            ViewBag.UserData = HttpContext.Session.Get<User>("user");

            return View();
        }

        [Route("~/User/SentNewData")]
        public IActionResult SentNewData(EditUserViewModel newUser)
        {
            if(newUser.Name.Length == 0 || newUser.Surname.Length == 0 || newUser.Age < 0 || newUser.Email.Length == 0 || newUser.Address.Length == 0)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangeData" });
            }

            if(newUser.Email.Contains('@') == false)
            {
                return RedirectToRoute(new { controller = "User", action = "ChangeData" });
            }

            User user = HttpContext.Session.Get<User>("user");

            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.Age = newUser.Age;
            user.Email = newUser.Email;
            user.Address = newUser.Address;

            _context.User.Update(user);
            _context.SaveChanges();           

            HttpContext.Session.Remove("user");
            HttpContext.Session.Set("user", user);

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }

        [Route("~/User/UserPosts/{id?}")]
        public IActionResult UserPosts(int id)
        {
            ViewBag.IsAdmin = HttpContext.Session.Get<User>("user").IsAdmin;

            ViewBag.UserPosts = _postRepository.GetPostsByUserId(id);

            ViewBag.UsersName = _userRepository.GetUserById(id).Name;

            ViewBag.AverageMark = averageMark(id);

            return View();
        }

        [Route("~/User/PickupRights/{id?}")]
        public IActionResult PickupRights(int id)
        {
            if (HttpContext.Session.Get<User>("user").IsAdmin)
            {
                User user = _context.User.Where(u => u.Id == id).FirstOrDefault();

                user.IsAdmin = false;

                _context.User.Update(user);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Admin", action = "UsersList" });
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [Route("~/User/GiveRights/{id?}")]
        public IActionResult GiveRights(int id)
        {
            if (HttpContext.Session.Get<User>("user").IsAdmin) 
            {
                User user = _context.User.Where(u => u.Id == id).FirstOrDefault();

                user.IsAdmin = true;

                _context.User.Update(user);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Admin", action = "UsersList" });
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [Route("~/User/DeleteUser/{id?}")]
        public IActionResult DeleteUser(int id)
        {
            if(HttpContext.Session.Get<User>("user").IsAdmin)
            {
                User user = _context.User.Where(u => u.Id == id).FirstOrDefault();
                                
                do
                {
                    if(_commentRepository.GetAll().Where(c => c.UserId == id).FirstOrDefault() != null)
                    {
                        _context.Comment.Remove(_commentRepository.GetAll().Where(c => c.UserId == id).FirstOrDefault());
                        _context.SaveChanges();
                    }
                } while(_commentRepository.GetAll().Where(c => c.UserId == id).FirstOrDefault() != null);

                do
                {
                    if (_markRepository.GetAll().Where(m => m.UserId == id).FirstOrDefault() != null)
                    {
                        _context.Mark.Remove(_markRepository.GetAll().Where(m => m.UserId == id).FirstOrDefault());
                        _context.SaveChanges();
                    }
                } while (_markRepository.GetAll().Where(m => m.UserId == id).FirstOrDefault() != null);

                do
                {
                    if(_postRepository.GetAll().Where(p => p.UserId == id).FirstOrDefault() != null)
                    {
                        int pid = _postRepository.GetAll().Where(p => p.UserId == id).FirstOrDefault().PostId;

                        _context.Post.Remove(_postRepository.GetAll().Where(p => p.UserId == id).FirstOrDefault());

                        do
                        {
                            if (_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault() != null)
                            {
                                _context.Comment.Remove(_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault());
                                _context.SaveChanges();
                            }
                        } while (_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault() != null);

                        do
                        {
                            if (_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault() != null)
                            {
                                _context.Comment.Remove(_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault());
                                _context.SaveChanges();
                            }
                        } while (_commentRepository.GetAll().Where(c => c.PostId == pid).FirstOrDefault() != null);

                        _context.SaveChanges();
                    }
                } while (_postRepository.GetAll().Where(p => p.UserId == id).FirstOrDefault() != null);

                _context.User.Remove(user);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "Admin", action = "UsersList" });
            }

            return RedirectToRoute(new { controller = "User", action = "Login" });
        }

        [Route("~/User/Verification")]
        public IActionResult Verification()
        {
            int p = SendSecretPass(HttpContext.Session.Get<User>("user").Id);

            if (p == -1)
            {

            }

            HttpContext.Session.Set<int>("secret", p);

            return View();
        }

        [Route("~/User/ComplateVerification")]
        public IActionResult ComplateVerification()
        {
            User u = HttpContext.Session.Get<User>("user");

            if (Request.Form["pass"].ToString() == HttpContext.Session.Get<int>("secret").ToString())
            {
                u.IsVerified = true;
                HttpContext.Session.Remove("secret");

                _context.User.Update(u);
                _context.SaveChanges();

                return RedirectToRoute(new { controller = "User", action = "Profile" });
            }

            HttpContext.Session.Remove("secret");

            return RedirectToRoute(new { controller = "User", action = "Profile" });
        }

        private int SendSecretPass(int id)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("acseler16@gmail.com");
            msg.To.Add(_userRepository.GetUserById(id).Email.ToString());
            msg.Subject = "Verification";

            Random rand = new Random();
            int pass = rand.Next(100000, 999999);
            msg.Body = "Secret pass: " + pass.ToString();

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("acseler16@gmail.com", "Cthusq2002");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                try
                {
                    client.Send(msg);
                    return pass;
                }
                catch (Exception ex)
                {
                    
                }
            }

            return -1;
        }

        private float averageMark(int id)
        {
            int postsCount = _postRepository.GetPostsByUserId(id).Count();
            int allSum = 0;

            var posts = _postRepository.GetPostsByUserId(id);

            foreach (var i in posts)
            {
                allSum += _markRepository.GetPostMark(i.PostId);
            }

            float average = allSum / postsCount;

            return average;
        }

        public bool CheckExist(string email)
        {
            if(_userRepository.GetUserNameByEmail(email) == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckPass(string email, string pass)
        {
            if (_userRepository.GetAll().Where(u => u.Email.CompareTo(email) == 0).FirstOrDefault().Password.CompareTo(pass) == 0) 
            {
                return true;
            }

            return false;
        }
    }
}
