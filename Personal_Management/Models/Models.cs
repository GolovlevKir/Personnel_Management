using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Models
{
    public class Departments
    {
        [Key]
        //Первичный ключ
        public int ID_Depart { get; set; }
        //Вывод ошибки
        [Required(ErrorMessage = "Введите наименование отдела")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Введите от 0 до 30 символов")]
        //Отображение на странице
        [Display(Name = "Наименование отдела", Description = "desc")]
        //Регулярное выражение
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
        public string Naim_Depart { get; set; }
        //Отображение на странице
        [Display(Name = "Логическое удаление", Description = "desc")]
        public bool Logical_Delete { get; set; }
        public Departments()
        {
            this.Positions = new HashSet<Positions>();
        }
        public ICollection<Positions> Positions { get; set; }
    }

    public class Positions
    {
        [Key]
        //Первичный ключ
        public int ID_Positions { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование должности")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования должности от 0 до 50 символов")]
        //Регулярное выражение
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
        //Вывод ошибки
        [Required(ErrorMessage = "Наименование не может быть пустым")]
        public string Naim_Posit { get; set; }
        //Отображение на странице
        [Display(Name = "Оклад")]
        //Ограничения ввода
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public string Salary { get; set; }
        [ForeignKey("Departments")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Наименование отдела")]
        public int Depart_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Количество сотрудников для должности")]
        //Ограничения ввода
        [Range(typeof(int), "1", "10000", ErrorMessage = "Наименьшее количество человек = 1")]
        //Вывод ошибки
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public int Kol_Vo_Pers { get; set; }
        //Отображение на странице
        [Display(Name = "Количество дней для испытательного срока")]
        //Ограничения ввода
        [Range(typeof(int), "1", "365", ErrorMessage = "Испытательный срок может быть от 1 дня до года")]
        //Вывод ошибки
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public int Kol_Vo_On_Isp { get; set; }
        //Отображение на странице
        [Display(Name = "Количество вакантных мест")]
        //Ограничения ввода
        [Range(typeof(int), "0", "365", ErrorMessage = "Испытательный срок может быть от 1 дня до года")]
        //Вывод ошибки
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public int Vak_Mest { get; set; }
        //Отображение на странице
        [Display(Name = "Логическое удаление", Description = "desc")]
        public bool Logical_Delete { get; set; }
        public virtual Departments Departments { get; set; }
        public Positions()
        {
            this.Posit_Responsibilities = new HashSet<Posit_Responsibilities>();
        }
        public ICollection<Posit_Responsibilities> Posit_Responsibilities { get; set; }
    }

    public class Isp_Sroki
    {
        [Key]
        //Первичный ключ
        public int ID_Isp { get; set; }
        [ForeignKey("Posit_Responsibilities")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Pos_Res_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Дата начала испытательного срока")]
        //Вывод ошибки
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public string Date_Start { get; set; }
        //Отображение на странице
        [Display(Name = "Дата окончания испытательного срока")]
        //Вывод ошибки
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public string Date_Finish { get; set; }
        [ForeignKey("status_isp_sroka")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Статус испытательного срока")]
        public int Status_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Итог испытательного срока")]
        public string itog { get; set; }
        public virtual Posit_Responsibilities Posit_Responsibilities { get; set; }
        public virtual status_isp_sroka status_isp_sroka { get; set; }
    }

    public class Rates
    {
        [Key]
        //Первичный ключ
        public int ID_Rate { get; set; }
        //Отображение на странице
        [Display(Name = "Ставка")]
        //Ограничения ввода
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public string Rate { get; set; }
        //Отображение на странице
        [Display(Name = "Логическое удаление", Description = "desc")]
        public bool Logical_Delete { get; set; }
        public Rates()
        {
            this.Posit_Responsibilities = new HashSet<Posit_Responsibilities>();
        }
        public ICollection<Posit_Responsibilities> Posit_Responsibilities { get; set; }
    }

    public class Posit_Responsibilities
    {
        [Key]
        //Первичный ключ
        public int ID_Pos_Res { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [ForeignKey("Positions")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Должность")]
        public int Positions_ID { get; set; }
        [ForeignKey("Rates")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Ставка")]
        public int Rates_ID { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public virtual Positions Positions { get; set; }
        public virtual Rates Rates { get; set; }
        public Posit_Responsibilities()
        {
            this.Isp_Sroki = new HashSet<Isp_Sroki>();
        }
        public ICollection<Isp_Sroki> Isp_Sroki { get; set; }
        public string Shif
        {
            get { return Sotrs.FullName; }
        }
    }

    public class Documents
    {
        [Key]
        //Первичный ключ
        public int ID_Doc { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование документа")]
        //Ограничения ввода
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 100 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Наименование не может быть пустым")]
        public string Doc_Naim { get; set; }
        //Отображение на странице
        [Display(Name = "Логическое удаление", Description = "desc")]
        public bool Logical_Delete { get; set; }
        public Documents()
        {
            this.Sbor_Docum = new HashSet<Sbor_Docum>();
        }
        public ICollection<Sbor_Docum> Sbor_Docum { get; set; }
    }

    public class Pass_Dannie
    {
        [Key]
        //Первичный ключ
        public int ID_Pass { get; set; }
        //Отображение на странице
        [Display(Name = "Серия паспорта")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string S_Pas { get; set; }
        //Отображение на странице
        [Display(Name = "Номер паспорта")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string N_Pas { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        //public string Shif
        //{
        //    get { return Program.Hash(N_Pas); }
        //}

    }

    public class Sbor_Docum
    {
        [Key]
        //Первичный ключ
        public int ID_Sbora { get; set; }
        [ForeignKey("Documents")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Наименование документа")]
        public int Doc_ID { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Результат (Сдан / Не сдан)")]
        public bool Itog { get; set; }
        //Отображение на странице
        [Display(Name = "Загрузить документ...")]
        //Вывод ошибки
        [Required(ErrorMessage = "Вы не прикрепили документ")]
        public string Photo_Doc { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public virtual Documents Documents { get; set; }
    }

    public class status_isp_sroka
    {
        [Key]
        //Первичный ключ
        public int ID_St { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование статуса")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 50 символов")]
        //Регулярное выражение
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Name_St { get; set; }
        public status_isp_sroka()
        {
            this.Isp_Sroki = new HashSet<Isp_Sroki>();
        }
        public ICollection<Isp_Sroki> Isp_Sroki { get; set; }
    }

    public class DaysOfWeek
    {
        [Key]
        //Первичный ключ
        public int ID_Day { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование дня недели")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования дня недели от 0 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Naim_Day { get; set; }
        public DaysOfWeek()
        {
            this.work_Schedules = new HashSet<Work_Schedule>();
        }
        public ICollection<Work_Schedule> work_Schedules { get; set; }
    }

    public class Work_Schedule
    {
        [Key]
        //Первичный ключ
        public int ID_Schedule { get; set; }
        [ForeignKey("DaysOfWeek")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "День недели:")]
        public int Day_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Время начала работы:")]
        public string Vremya_Start { get; set; }
        //Отображение на странице
        [Display(Name = "Время окончания работы:")]
        public string Vremya_End { get; set; }
        //Отображение на странице
        [Display(Name = "Часов на перерыв:")]
        public string Break_time { get; set; }
        //Отображение на странице
        [Display(Name = "Время начала перерыва:")]
        public string Break_Start { get; set; }
        //Отображение на странице
        [Display(Name = "Время окончания перерыва:")]
        public string Break_End { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [Display(Name = "Является выходным днем")]
        public bool Vih { get; set; }
        public virtual DaysOfWeek DaysOfWeek { get; set; }
        public virtual Sotrs Sotrs { get; set; }
    }
    public class RoomViewModel
    {
        public IEnumerable<Questions> Questions { get; set; }
        public IEnumerable<ZayavkaNaSobes> ZayavkaNaSobes { get; set; }
    }
    public class Questions
    {
        [Key]
        //Первичный ключ
        public int ID_Quest { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование вопроса:")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле заполнено неверно")]
        public string Quest_Naim { get; set; }
        public Questions()
        {
            this.ZayavkaNaSobes = new HashSet<ZayavkaNaSobes>();
        }
        public ICollection<ZayavkaNaSobes> ZayavkaNaSobes { get; set; }
    }

    public class ZayavkaNaSobes
    {
        [Key]
        //Первичный ключ
        public int ID_Zay { get; set; }
        //Отображение на странице
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [ForeignKey("Questions")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Вопрос")]
        public int Qwe_ID { get; set; }
        [Display(Name = "Ответ на вопрос:")]
        public string otvet { get; set; }
        public string datazayavki { get; set; }
        public string nomerzay { get; set; }
        public bool itog { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public virtual Questions Questions { get; set; }
    }

    public class Obrabotka
    {
        [Key]
        //Первичный ключ
        public int ID_Obr { get; set; }
        //Отображение на странице
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО пользователя:")]
        public int Sotr_ID { get; set; }
        [Display(Name = "Назначить дату собеседования")]
        public string Data_Sob { get; set; }
        [Display(Name = "Назначить время собеседования")]
        public string Vremya_Sob { get; set; }
        [Display(Name = "Добавить комментарий")]
        public string Kommnt { get; set; }
        [Display(Name = "Принять на работу")]
        public bool reshenie { get; set; }
        [Display(Name = "Номер заявки")]
        public string nomerzay { get; set; }
        public virtual Sotrs Sotrs { get; set; }
    }

    public class nomSearch
    {
        [Display(Name = "Укажите номер заявки")]
        public string nomerzay { get; set; }
    }

    public class newPass
    {
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string password { get; set; }
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string password1 { get; set; }
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string password2 { get; set; }
    }

    public class Accounts
    {
        [Key]
        //Первичный ключ
        //Отображение на странице
        [Display(Name = "Логин аккаунта")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Login { get; set; }
        //Отображение на странице
        [Display(Name = "Пароль аккаунта")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password { get; set; }
        [Display(Name = "Хэшированный пароль")]
        //Регулярное выражение
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password_Shifr { get; set; }
        [ForeignKey("Roles")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Роль")]
        public int Role_ID { get; set; }
        [Display(Name = "Блокировка аккаунта")]
        public bool Block { get; set; }
        [Display(Name = "Логическое удалени")]
        public bool Logical_Delete { get; set; }
        public virtual Roles Roles { get; set; }
        public Accounts()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
    }

    public class Roles
    {
        [Key]
        //Первичный ключ
        public int ID_Role { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование роли")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования роли от 0 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Role_Naim { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить права доступа администратора")]

        public bool Manip_Roles { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные сотрудников")]
        public bool Manip_Sotrs { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные структуры организации")]
        public bool Manip_Department { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные тестирования")]
        public bool Manip_Tests { get; set; }
        [Display(Name = "Логическое удалени")]
        public bool Logical_Delete { get; set; }
        public Roles()
        {
            this.Accounts = new HashSet<Accounts>();
        }
        public ICollection<Accounts> Accounts { get; set; }

    }

    public class Steps
    {
        [Key]
        //Первичный ключ
        public int ID_Step { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 1 \"Прохождение собеседования\"")]
        public bool Sobesedovanie { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 2 \"Назначение должности\"")]
        public bool Dolznost { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 3 \"Добавление графика работы\"")]
        public bool Grafik_Raboti { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 4 \"Сбор документов\"")]
        public bool Sbor_Documentov { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 5 \"Прохождение испытательного срока\"")]
        public bool Isp_Srok { get; set; }
        [Display(Name = "Логическое удалени")]
        public bool Logical_Delete { get; set; }
        public virtual Sotrs Sotrs { get; set; }

    }

    public class Sotrs
    {
        [Key]
        //Первичный ключ
        public int ID_Sotr { get; set; }
        //Отображение на странице
        [Display(Name = "Фамилия")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Surname_Sot { get; set; }
        //Отображение на странице
        [Display(Name = "Имя")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Name_Sot { get; set; }
        //Отображение на странице
        [Display(Name = "Отчество")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        public string Petronumic_Sot { get; set; }
        public string Day_Of_Birth { get; set; }
        //Отображение на странице
        [Display(Name = "Адрес")]
        //Ограничения ввода
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 200 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Address { get; set; }
        //Отображение на странице
        [Display(Name = "Номер телефона")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Num_Phone { get; set; }
        //Отображение на странице
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Адрес эл. почы введен неверно")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Email { get; set; }
        //Отображение на странице
        [Display(Name = "Фото")]
        //Вывод ошибки
        [Required(ErrorMessage = "Если документа нет, укажите прочерк (-)")]
        public string Photo { get; set; }
        //Отображение на странице
        [Display(Name = "Дата приема на работу")]
        public string Date_of_adoption { get; set; }
        //Отображение на странице
        [Display(Name = "Статус Уволен")]
        public bool fired { get; set; }
        //Отображение на странице
        [Display(Name = "Статус Гость")]
        public bool Guest { get; set; }
        //Отображение на странице
        [Display(Name = "Логическое удаление")]
        public bool Logical_Delete { get; set; }
        [ForeignKey("Accounts")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Должность")]
        public string Login_Acc { get; set; }
        public ICollection<Sbor_Docum> Sbor_Docum { get; set; }
        public ICollection<Posit_Responsibilities> Posit_Responsibilities { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

        public Sotrs()
        {
            Photo = "~/Content/Photo/st/default.png";
            this.Sbor_Docum = new HashSet<Sbor_Docum>();
            this.Posit_Responsibilities = new HashSet<Posit_Responsibilities>();
        }
        public virtual Accounts Accounts { get; set; }

        //Получение ФИО
        public string FullName
        {
            get { return Surname_Sot + " " + Name_Sot + " " + Petronumic_Sot; }
        }
        public string Full
        {
            get { return Surname_Sot + " " + Name_Sot + " " + Petronumic_Sot; }
        }
    }

    public class LoginModel
    {
        //Отображение на странице
        [Display(Name = "Логин аккаунта")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Login { get; set; }
        //Отображение на странице
        [Display(Name = "Пароль аккаунта")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        //Отображение на странице
        [Display(Name = "Логин аккаунта")]
        //Регулярное выражение
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Логин должен иметь заглавные (A-Z), прописные (a-z) буквы и цифры (0-9)")]
        //Ограничения ввода
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина Логина должна быть от 6 до 20 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Login { get; set; }
        //Отображение на странице
        [Display(Name = "Пароль аккаунта")]
        //Регулярное выражение
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Пароль должен иметь заглавные (A-Z), прописные (a-z) буквы и цифры (0-9)")]
        //Ограничения ввода
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина Пароля должна быть от 6 до 20 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password { get; set; }
        [Display(Name = "Фамилия")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Surname_Sot { get; set; }
        //Отображение на странице
        [Display(Name = "Имя")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Name_Sot { get; set; }
        //Отображение на странице
        [Display(Name = "Отчество")]
        //Регулярное выражение
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        public string Petronumic_Sot { get; set; }
        [Display(Name = "Дата рождения")]
        public string Day_Of_Birth { get; set; }
        //Отображение на странице
        [Display(Name = "Адрес")]
        //Ограничения ввода
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 200 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Address { get; set; }
        //Отображение на странице
        [Display(Name = "Номер телефона")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Num_Phone { get; set; }
        //Отображение на странице
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Адрес эл. почы введен неверно")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Email { get; set; }
        //Отображение на странице
        [Display(Name = "Фото")]
        public string Photo { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        public RegisterModel()
        {
            Photo = "~/Content/Photo/st/default.png";
        }
    }

    public class SotrsListViewModel
    {
        public IEnumerable<Sotrs> Sotrs { get; set; }
        public SelectList Positions { get; set; }
        public SelectList Rates { get; set; }
        public SelectList Work_Schedule { get; set; }
    }

    public class MessageSrvices
    {
        public async static Task SendEmailAsynk(string email, string subject, string message)
        {
            try
            {
                var _email = "kirvik12122000@gmail.com";
                var _epass = ConfigurationManager.AppSettings["EmailPassword"];
                var _dispName = "Devsone";
                MailMessage myMessage = new MailMessage();
                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _dispName);
                myMessage.Subject = subject;
                myMessage.Body = message;
                myMessage.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _epass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMessage);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class SendEMailViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
        //Наименование поля
        [DataMember(Name = "label")]
        public string Label = "";
        //Значение Y по оси координат
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }

    [DataContract]
    public class DataPoint2
    {
        public DataPoint2(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Наименование поля
        [DataMember(Name = "label")]
        public string Label = "";

        //Значение Y по оси координат
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}                        
                 