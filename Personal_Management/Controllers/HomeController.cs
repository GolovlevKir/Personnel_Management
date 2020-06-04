using Newtonsoft.Json;
using Personal_Management.Models;
using Shifr;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class HomeController : Controller
    {

        PersonalContext db = new PersonalContext();
        [HttpGet]
        public ActionResult Index()
        {
            //Обновление испытательных сроков
            Program.update();
            ViewBag.isp = Program.colIsp;
            if (User.Identity.IsAuthenticated)
            {
                //Получение данных о пользователе
                var s = db.Sotrs.Where(sot => sot.Login_Acc == User.Identity.Name).FirstOrDefault();
                Session["FIO"] = s.Surname_Sot + " " + s.Name_Sot + " " + s.Petronumic_Sot;
                Session["Email"] = s.Email;
                Session["Num_Phone"] = s.Num_Phone;
                Session["Address"] = Shifrovanie.Decryption(s.Address);
                Session["Day_Of_Birth"] = s.Day_Of_Birth;
                Session["Photo"] = s.Photo;
                Session["Dolj"] = "Статус: Гость";
                Session["log"] = "Ваш логин: " + User.Identity.Name;
                Session["Guest"] = s.Guest;
                //Если не гость, не заблокирован, не уволен
                if ((bool)Session["Guest"] == false && s.Accounts.Block == false && s.fired == false)
                {
                    Session["Manip_Sotrs"] = s.Accounts.Roles.Manip_Sotrs;
                    Session["Manip_Roles"] = s.Accounts.Roles.Manip_Roles;
                    Session["Manip_Tests"] = s.Accounts.Roles.Manip_Tests;
                    Session["Manip_Department"] = s.Accounts.Roles.Manip_Department;
                    Session["Dolj"] = "Статус: Сотрудник";
                    var p = db.Posit_Responsibilities.Where(po => po.Sotrs.Login_Acc == User.Identity.Name).FirstOrDefault();
                    if (p != null)
                    {
                        //Данные испытательного срока
                        var isp = db.Isp_Sroki.Where(i => i.Pos_Res_ID == p.ID_Pos_Res).FirstOrDefault();
                        if (isp != null)
                        {
                            switch (isp.Status_ID)
                            {
                                case 1:
                                    ViewBag.isp = 100;
                                    break;
                                case 2:
                                    ViewBag.isp = 50;
                                    break;
                                case 3:
                                    ViewBag.isp = 75;
                                    break;
                                case 4:
                                    ViewBag.isp = 0;
                                    break;
                            }
                        }
                        else
                        {
                            ViewBag.isp = 0;
                        }
                        //Этапы принятия
                        var step = db.Steps.Where(st => st.Sotrs.Login_Acc == User.Identity.Name);
                        if (step != null)
                        {
                            ViewBag.steps = step.ToList();
                        }
                    }
                    return View();
                }
                else
                {
                    return View();
                }
            }
            //Не авторизованный пользователь
            return RedirectToAction("NotAuth", "Error");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Suc = false;
                ViewBag.SucMes = "";
                //получение данных аккаунта
                var s = db.Sotrs.Where(sot => sot.Login_Acc == User.Identity.Name).FirstOrDefault();
                Sotrs sotrs = new Sotrs();
                sotrs = db.Sotrs.Find(s.ID_Sotr);
                sotrs.Address = Shifrovanie.Decryption(sotrs.Address);
                if (sotrs == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(sotrs);
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpPost]
        public ActionResult Settings(Sotrs sotrs)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Если картинка загружена
                if (sotrs.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(sotrs.ImageUpload.FileName);
                    string extension = Path.GetExtension(sotrs.ImageUpload.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    sotrs.Photo = "/Content/Photo/st/" + filename;
                    sotrs.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Photo/st/"), filename));
                }
                try
                {
                    //Обновление данных
                    //Изменение данных
                    sotrs.Address = Shifrovanie.Encryption(sotrs.Address);
                    db.Entry(sotrs).State = EntityState.Modified;
                    Session["FIO"] = sotrs.Surname_Sot + " " + sotrs.Name_Sot + " " + sotrs.Petronumic_Sot;
                    Session["Email"] = sotrs.Email;
                    Session["Num_Phone"] = sotrs.Num_Phone;
                    Session["Address"] = sotrs.Address;
                    Session["Day_Of_Birth"] = sotrs.Day_Of_Birth;
                    Session["Photo"] = sotrs.Photo;
                    Session["Dolj"] = "Статус: Гость";
                    var s = db.Sotrs.Include(sot => sot.Accounts).Where(sot => sot.Login_Acc == User.Identity.Name).FirstOrDefault();
                    Session["Guest"] = s.Guest;
                    //Если не гость, не заблокирован, не уволен
                    if ((bool)Session["Guest"] == false && s.Accounts.Block == false && s.fired == false)
                    {
                        Session["Manip_Sotrs"] = s.Accounts.Roles.Manip_Sotrs;
                        Session["Manip_Roles"] = s.Accounts.Roles.Manip_Roles;
                        Session["Manip_Tests"] = s.Accounts.Roles.Manip_Tests;
                        Session["Manip_Department"] = s.Accounts.Roles.Manip_Department;
                    }
                    //Сохранение
                    db.SaveChanges();
                    //сообщение об изменении данных
                    ViewBag.Suc = true;
                    ViewBag.SucMes = "Ваши данные аккаунта успешно изменены";
                    return View(sotrs);
                }

                catch
                {
                    //список аккаунтов
                    ViewBag.Login_Acc = new SelectList(db.Accounts, "Login", "Password", sotrs.Login_Acc);
                    return View(sotrs);
                }
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpGet]
        public ActionResult SettPersDannie()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpGet]
        public ActionResult PoiskZay()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpPost]
        public ActionResult PoiskZay(nomSearch search)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Проверка заявки на назначение собеседования
                var result = db.Obrabotka.Where(r => r.nomerzay == search.nomerzay).FirstOrDefault();
                if (result != null)
                {
                    return Redirect("/Home/Zay/" + search.nomerzay);
                }
                else
                {
                    //Иначе поиск заявки в заявках
                    var res = db.ZayavkaNaSobes.Where(r => r.nomerzay == search.nomerzay).FirstOrDefault();
                    if (res != null)
                    {
                        return Redirect("/Home/ZayObr/" + search.nomerzay);
                    }
                    else
                    {
                        //Заявка не найдена
                        return View("NotZay");
                    }
                }
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpGet]
        public ActionResult NotZay()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpGet]
        public ActionResult Zay(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Получение данных заявки
                var zay = db.ZayavkaNaSobes.Where(z => z.nomerzay == id).FirstOrDefault();
                //Получение ответов соискателя
                ViewBag.zay = db.ZayavkaNaSobes.Where(z => z.nomerzay == id).ToList();
                //Получение данных по собеседованию
                ViewBag.sob = db.Obrabotka.Where(z => z.nomerzay == id).ToList();
                return View(zay);
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }
        [HttpGet]
        public ActionResult ZayObr(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Полчение данных заявки
                var zay = db.ZayavkaNaSobes.Where(z => z.nomerzay == id).FirstOrDefault();
                //Список ответов соискателя
                ViewBag.zay = db.ZayavkaNaSobes.Where(z => z.nomerzay == id).ToList();
                return View(zay);
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        public ActionResult SettPersDannie(newPass mod)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    //Хеширование старого пароля
                    string psh = Shifrovanie.Hash(mod.password.ToString());
                    //Хеширование нового пароля
                    string pshnew = Shifrovanie.Hash(mod.password1.ToString());
                    SqlCommand command = new SqlCommand("", Program.SqlConnection);
                    //сравнение паролей
                    command.CommandText = "SELECT count(*) FROM dbo.Accounts where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "' and [dbo].[Accounts].[Password_Shifr] = '" + psh + "'";
                    Program.SqlConnection.Open();
                    int co = Convert.ToInt32(command.ExecuteScalar());
                    Program.SqlConnection.Close();
                    if (co > 0)
                    {
                        if (mod.password1 == mod.password2 && mod.password1 != "" && mod.password2 != "")
                        {
                            //Смена пароля
                            ViewBag.New = "";
                            ViewBag.NewPass = "";
                            ViewBag.OldPass = "";
                            command.CommandText = "update Accounts " +
                            "set " +
                            "Password = '" + mod.password1 + "', " +
                            "Password_Shifr = '" + pshnew + "' " +
                            "where Login = '" + User.Identity.Name + "'";
                            Program.SqlConnection.Open();
                            command.ExecuteScalar();
                            Program.SqlConnection.Close();
                            ViewBag.SucMes = "Пароль изменен!";
                            ViewBag.Suc = true;
                            return View(mod);
                        }
                        else
                        {
                            //Если новые пароли не совпадают
                            ModelState.AddModelError("password1", "Новые пароли не совпадают");
                            ModelState.AddModelError("password2", "Новые пароли не совпадают");
                            return View(mod);
                        }
                    }
                    else
                    {
                        //Если пароль старый указан неверно
                        ModelState.AddModelError("password", "Старый пароль указан неверно");
                        return View(mod);
                    }
                }
                else
                {
                    //Если поля пустые
                    ModelState.AddModelError("password", "Поля не заполнены");
                    ModelState.AddModelError("password1", "Поля не заполнены");
                    ModelState.AddModelError("password2", "Поля не заполнены");
                    return View(mod);
                }
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        [HttpGet]
        public ActionResult Zayavki()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotAuth");
            }
        }

        //информация о компании
        public ActionResult OKomp()
        {
            SqlCommand command = new SqlCommand("", Program.SqlConnection);
            var dep = db.Positions;
            var shtat = db.Posit_Responsibilities;
            foreach (Positions item in dep)
            {
                command.CommandText = "Select Count(*) from [dbo].[Posit_Responsibilities] where [Positions_ID] = " + item.ID_Positions;
                Program.SqlConnection.Open();
                int? i = (int)command.ExecuteScalar();
                Program.SqlConnection.Close();
                if (i != 0)
                {
                    command.CommandText = "Update [dbo].[Positions] set [Vak_Mest] = [Kol_Vo_Pers] - " + i.ToString() + " where [ID_Positions] = " + item.ID_Positions.ToString();
                    Program.SqlConnection.Open();
                    command.ExecuteNonQuery();
                    Program.SqlConnection.Close();
                }
            }
            //Список среднесписочного состава сотрудников, которые не уволены
            command.CommandText = "select count(*) from Sotrs where guest = 'false' and fired = 'false'";
            Program.SqlConnection.Open();
            int vsego = (int)command.ExecuteScalar();
            //Список уволенных за год
            command.CommandText = "select count(*) from Sotrs where guest = 'false' and YEAR(CONVERT(datetime,Date_of_adoption,105)) = YEAR(GETDATE()) and fired = 'true'";
            int yvoleno = (int)command.ExecuteScalar();
            Program.SqlConnection.Close();
            int tekuch;
            if (vsego != 0)
                //Текучесть кадров = количество уволенных за год * 100 / количество работющих сотрудников 
                tekuch = (yvoleno * 100) / vsego;
            else
                tekuch = 0;
            //Список данных для диаграммы
            List<DataPoint> dataPoints = new List<DataPoint>();
            //Добавление данных процента текучести
            dataPoints.Add(new DataPoint("Процент текучести", tekuch));
            //Добавление процента стабильности
            dataPoints.Add(new DataPoint("Стабильный состав", 100 - tekuch));
            //Загрузка данных в переменную
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);


            List<DataPoint2> dataPoints2 = new List<DataPoint2>();
            var p = db.Positions.Where(pos => pos.Vak_Mest > 0).ToList();
            foreach (Positions item in p)
            {
                dataPoints2.Add(new DataPoint2(item.Naim_Posit, item.Vak_Mest));
            }

            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            return View();
        }

    }
}