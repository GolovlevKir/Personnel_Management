using Personal_Management.Models;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Shifr;
using Email;

namespace Personal_Management.Controllers
{
    public class AccountController : Controller
    {
        //Ссылква на контекст данных
        PersonalContext db = new PersonalContext();
        public ActionResult Login()
        {
            //Обновление статусов
            Program.update();
            ViewBag.isp = Program.colIsp;
            //Если не авторизованный пользователь
            if (User.Identity.IsAuthenticated)
            {
                //Вернуть начальную страницу
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Данные пользователя обнуляются
                Session["FIO"] = "";
                Session["Email"] = "";
                Session["Num_Phone"] = "";
                Session["Address"] = "";
                Session["Day_Of_Birth"] = "";
                Session["Photo"] = "";
                Session["Dolj"] = "";
                return View();
            }

        }

        public ActionResult Lic()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            //Если не авторизован
            if (User.Identity.IsAuthenticated)
            {
                //Переход на начальную страницу
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    // поиск пользователя в бд
                    Accounts user = null;
                    string pa = Shifrovanie.Hash(model.Password);
                    //Поиск аккаунта по логину и паролю
                    user = db.Accounts.FirstOrDefault(u => u.Login == model.Login && u.Password_Shifr == pa);
                    //Если найден, то открыть домашнюю страницу
                    if (user != null)
                    {
                        ViewBag.noacc = false;
                        //Указываем логин для пользователя
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        //Поиск данных пользователя
                        var s = db.Sotrs.Where(sot => sot.Login_Acc == model.Login).FirstOrDefault();
                        //Необходимо присвоить данные для отображения в личном кабинете
                        Session["FIO"] = s.Surname_Sot + " " + s.Name_Sot + " " + s.Petronumic_Sot;
                        Session["Email"] = s.Email;
                        Session["Num_Phone"] = s.Num_Phone;
                        Session["Address"] = Shifrovanie.Decryption(s.Address);
                        Session["Day_Of_Birth"] = s.Day_Of_Birth;
                        Session["Photo"] = s.Photo;
                        Session["Dolj"] = "Статус: Гость";
                        Session["Guest"] = s.Guest;
                        //Если не гость, не заблокирован и не уволенный
                        if ((bool)Session["Guest"] == false && s.Accounts.Block == false && s.fired == false)
                        {
                            //Получение прав доступа
                            Session["Manip_Sotrs"] = s.Accounts.Roles.Manip_Sotrs;
                            Session["Manip_Roles"] = s.Accounts.Roles.Manip_Roles;
                            Session["Manip_Tests"] = s.Accounts.Roles.Manip_Tests;
                            Session["Manip_Department"] = s.Accounts.Roles.Manip_Department;
                            Session["Dolj"] = "Статус: Сотрудник ";
                        }
                        //Переход в личный кабинет
                        return RedirectToAction("Index", "Home");
                    }
                    //Иначе вывести надпись, что пользователь отсутствует
                    else
                    {
                        //Вывод блока с сообщением, что данного пользователя нет
                        ViewBag.noacc = true;
                        ViewBag.noacct = "Пользователя с данным логином и паролем не существует";
                        return View(model);
                    }
                }
            }
            return View(model);
        }
        public ActionResult Register()
        {
            Session["LogGeve"] = "";
            Session["War"] = false;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            //Если значение поля для загрузки фото не пустое
            if (model.ImageUpload != null)
            {
                //Получение имени файла
                string filename = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                //Расширение файла
                string extension = Path.GetExtension(model.ImageUpload.FileName);
                //Новое уникальное наименование, которое будет занесено в БД
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                //Значение для поля Фото, которое будет добавлено в БД с путем к файлу
                model.Photo = "/Content/Photo/st/" + filename;
                //Загрузка фото в папку
                model.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Photo/st/"), filename));
            }
            else
            {
                //Иначе дефолтное значение
                model.Photo = "/Content/Photo/st/default.png";
            }
            Accounts user = null;
            //Поиск пльзователей с таким логином
            user = db.Accounts.FirstOrDefault(u => u.Login == model.Login);
            //Если не найдено, то добавляем нового
            if (user == null)
            {
                //Если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Создаем команду
                    SqlCommand command = new SqlCommand("", Program.SqlConnection);
                    //Запрос для команды (проверка на возраст)
                    command.CommandText = "select Datediff(year, '" + model.Day_Of_Birth + "', getdate())";
                    int? voz = 0;
                    Program.SqlConnection.Open();
                    // Выполнение команды
                    voz = (int)command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    //В соответствии со ст. 63 ТК РФ, Официально можно работать в России с 16 лет
                    //Проверка, есть ли пользователю 16 лет и более
                    if (voz <= 16)
                    {
                        ModelState.AddModelError("Day_Of_Birth", "Возрастные ограничения 18+");
                        return View(model);
                    }
                    Session["War"] = false;
                    //Добавление нового аккаунта
                    db.Accounts.Add(new Accounts { Login = model.Login, Password = model.Password, Password_Shifr = Shifrovanie.Hash(model.Password), Role_ID = 4, Logical_Delete = false, Block = false });
                    //Сохранение данных
                    db.SaveChanges();
                    //Добавление сотрудника
                    db.Sotrs.Add(new Sotrs
                    {
                        Surname_Sot = model.Surname_Sot,
                        Name_Sot = model.Name_Sot,
                        Petronumic_Sot = model.Petronumic_Sot,
                        Day_Of_Birth = model.Day_Of_Birth,
                        Address = Shifrovanie.Encryption(model.Address),
                        Num_Phone = model.Num_Phone,
                        Email = model.Email,
                        Photo = model.Photo,
                        Date_of_adoption = null,
                        fired = false,
                        Guest = true,
                        Logical_Delete = false,
                        Login_Acc = model.Login
                    });
                    //Сохранение данных
                    db.SaveChanges();
                    //Отправка письма на почту
                    EmailTo.MySendMail("" +

                        "Приветствуем, " + model.Surname_Sot + " " + model.Name_Sot + " " + model.Petronumic_Sot + "! <br>Мы рады, " +
                        "что вы зарегистрировались в нашей системе. Мы надеемся, что не разочаруем Вас. С радостью ответим на Ваши вопросы." +
                        "<br><br>Ваш Логин: " + model.Login + " <br>Пароль: " + model.Password,
                         model.Email, "Компания CMS", "Регистрация на сайте компании CMS");
                    //Поиск пользователя
                    user = db.Accounts.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    //Если пользователь найден
                    if (user != null)
                    {
                        //Авторизовываем пользователя
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        //Получение данных о пользователе
                        var s = db.Sotrs.Where(sot => sot.Login_Acc == model.Login);
                        Session["FIO"] = model.Surname_Sot + " " + model.Name_Sot + " " + model.Petronumic_Sot;
                        Session["Email"] = model.Email;
                        Session["Num_Phone"] = model.Num_Phone;
                        Session["Address"] = model.Address;
                        Session["Day_Of_Birth"] = model.Day_Of_Birth;
                        Session["Photo"] = model.Photo;
                        Session["Dolj"] = "Статус: Гость";
                        //Открытие личного кабинета
                        return RedirectToAction("Index", "Home");
                    }
                    //Иначе выводим надпись, данные введены некоррекно
                    else
                    {
                        ModelState.AddModelError("", "Попробуйте зарегистрироваться снова");
                        Session["War"] = true;
                        return View(model);
                    }
                }
            }
            else
            {
                //Сообщение, что пользователь с таким логином уже существует
                ModelState.AddModelError("", "Пользователь с данным логином уже существует");
                Session["War"] = true;
                return View(model);
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            //Выход из аккаунта
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult LogOut()
        {
            //Выход из аккаунта
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Them(string tema)
        {
            //Если чекбокс равен true
            if (tema == "on")
            {
                //Изменение стиля на темную версию
                Session["style"] = "bootstrapdark.min.css";
                return RedirectToAction("Settings", "Home");
            }
            else
            {
                //Иначе изменение стиля на светный
                Session["style"] = "bootstrap_min.css";
                return RedirectToAction("Settings", "Home");
            }

        }

    }
}