using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual Departments Departments { get; set; }
        public Positions()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
    }

    public class Isp_Sroki
    {
        [Key]
        //Первичный ключ
        public int ID_Isp { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
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
        public virtual Sotrs Sotrs { get; set; }
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
        public string Rate { get; set; }
        public Rates()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
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
        [Required(ErrorMessage = "Если документа нет, укажите прочерк (-)")]
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

    public class Work_Schedule
    {
        [Key]
        //Первичный ключ
        public int ID_Schedule { get; set; }
        //Отображение на странице
        [Display(Name = "Наименование графика работы")]
        //Ограничения ввода
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 50 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Naim_Sche { get; set; }
        public Work_Schedule()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
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
        //Регулярное выражение
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Пароль должен иметь заглавные (A-Z), прописные (a-z) буквы и цифры (0-9)")]
        //Ограничения ввода
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина Пароля должна быть от 6 до 20 символов")]
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password { get; set; }
        [ForeignKey("Roles")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Роль")]
        public int Role_ID { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Sotrs Sotrs { get; set; }
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
        [Display(Name = "Разрешить права доступа для администратора")]

        public bool Manip_Roles { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные сотрудников")]
        public bool Manip_Sotrs { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные структуры организации")]
        public bool Manip_Department { get; set; }
        //Отображение на странице
        [Display(Name = "Разрешить просматривать данные бухгалтерского учета")]
        public bool Buh_Ych { get; set; }
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
        [Display(Name = "Этап 1 \"Добавление данных сотрудника\"")]
        public bool AddSotrInIS { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 2 \"Загрузка резюме\"")]
        public bool AddRezume { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 3 \"Запись данных, полученных на собеседовании, сотрудника в поле описания\"")]
        public bool AddSobesedovanie { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 4 \"Назначение испытательного срока\"")]
        public bool AddIspSrok { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 5 \"Ожидание решения о принятии\"")]
        public bool RezimOzidaniya { get; set; }
        //Отображение на странице
        [Display(Name = "Этап 6 \"Итог\"")]
        public bool Reshenie { get; set; }
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
        //Вывод ошибки
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
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
        

        [ForeignKey("Positions")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Должность")]
        public int Positions_ID { get; set; }
        [ForeignKey("Rates")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Ставка")]
        public int Rate_ID { get; set; }
        [ForeignKey("Work_Schedule")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "График работы")]
        public int Schedule_ID { get; set; }
        public string Date_of_adoption { get; set; }
        public string Opisanie { get; set; }
        public string rezume { get; set; }
        public virtual Positions Positions { get; set; }
        public virtual Rates Rates { get; set; }
        public virtual Work_Schedule Work_Schedule { get; set; }
        public string FullName
        {
            get { return Surname_Sot + " " + Name_Sot + " " + Petronumic_Sot + " (" + Positions.Naim_Posit + ")"; }
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

        [ForeignKey("Roles")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "Роль")]
        public int Role_ID { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //Отображение на странице
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Sotrs Sotrs { get; set; }
    }

    public class SotrsListViewModel
    {
        public IEnumerable<Sotrs> Sotrs { get; set; }
        public SelectList Positions { get; set; }
        public SelectList Rates { get; set; }
        public SelectList Work_Schedule { get; set; }
    }
}