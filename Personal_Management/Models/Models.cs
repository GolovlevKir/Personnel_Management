using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Personal_Management.Models
{
    public class Departments
    {
        [Key]
        public int ID_Depart { get; set; }
        [Required(ErrorMessage = "Введите наименование отдела")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Введите от 0 до 30 символов")]
        [Display(Name = "Наименование отдела", Description = "desc")]
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
        public int ID_Positions { get; set; }
        [Display(Name = "Наименование должности")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования должности от 0 до 50 символов")]
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
        [Required(ErrorMessage = "Наименование не может быть пустым")]
        public string Naim_Posit { get; set; }
        [Display(Name = "Оклад")]
        [RegularExpression(@"^([0-9 .&'-]+)$", ErrorMessage = "Поле может содержать только следующие символы: 0-9 и .")]
        [Range(typeof(decimal), "0.0", "999999999999.9", ErrorMessage = "Наименьшая цена - 0.0 рублей")]
        [Required(ErrorMessage = "Указанный оклад введен неправильно (Должны содержаться числа и(или) .)")]
        public decimal Salary { get; set; }
        [ForeignKey("Departments")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Наименование отдела")]
        public int Depart_ID { get; set; }
        [Display(Name = "Количество сотрудников для должности")]
        [Range(typeof(int), "1", "10000", ErrorMessage = "Наименьшее количество человек = 1")]
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public int Kol_Vo_Pers { get; set; }
        [Display(Name = "Количество дней для испытательного срока")]
        [Range(typeof(int), "1", "365", ErrorMessage = "Испытательный срок может быть от 1 дня до года")]
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public int Kol_Vo_On_Isp { get; set; }
        public Positions()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
    }

    public class Isp_Sroki : IValidatableObject
    {
        [Key]
        public int ID_Isp { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [Display(Name = "Дата начала испытательного срока")]
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public string Date_Start { get; set; }
        [Display(Name = "Дата окончания испытательного срока")]
        [Required(ErrorMessage = "Поле заполнено не верно")]
        public string Date_Finish { get; set; }
        [ForeignKey("status_isp_sroka")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Статус испытательного срока")]
        public int Status_ID { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public virtual status_isp_sroka status_isp_sroka { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Convert.ToDateTime(Date_Start) < DateTime.Now)
            {
                results.Add(new ValidationResult("Дата начала не может быть меньше, чем сегодня", new[] { "Date_Start" }));
            }

            if (Convert.ToDateTime(Date_Finish) <= Convert.ToDateTime(Date_Start))
            {
                results.Add(new ValidationResult("Дата окончания не может быть меньше, чем дата начала", new[] { "Date_Finish" }));
            }

            return results;
        }
    }
    

    public class Rates
    {
        [Key]
        public int ID_Rate { get; set; }
        [Display(Name = "Ставка")]
        [RegularExpression(@"^([0-9 .&'-]+)$", ErrorMessage = "Поле может содержать только следующие символы: 0-9 и .")]
        [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Наименьшая ставка - 0.0, а наибольшая - 10.0")]
        [Required(ErrorMessage = "Указанная ставка введена неправильно (Должны содержаться числа и(или) .)")]
        public decimal Rate { get; set; }
        public Rates()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
    }

    public class Documents
    {
        [Key]
        public int ID_Doc { get; set; }
        [Display(Name = "Наименование документа")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 100 символов")]
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
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
        public int ID_Pass { get; set; }
        [Display(Name = "Серия паспорта")]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string S_Pas { get; set; }
        [Display(Name = "Номер паспорта")]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string N_Pas { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        public virtual Sotrs Sotrs { get; set; }
    }

    public class Sbor_Docum
    {
        [Key]
        public int ID_Sbora { get; set; }
        [ForeignKey("Documents")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Наименование документа")]
        public int Doc_ID { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [Display(Name = "Результат (Сдан / Не сдан)")]
        public int Itog { get; set; }
        [Display(Name = "Загрузить документ...")]
        [Required(ErrorMessage = "Если документа нет, укажите прочерк (-)")]
        public string Photo_Doc { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public virtual Documents Documents { get; set; }
    }

    public class status_isp_sroka
    {
        [Key]
        public int ID_St { get; set; }
        [Display(Name = "Наименование статуса")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 50 символов")]
        [RegularExpression(@"^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле наименования должно содержать только русские буквы")]
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
        public int ID_Schedule { get; set; }
        [Display(Name = "Наименование графика работы")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования документа от 0 до 50 символов")]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Naim_Sche { get; set; }
        public Work_Schedule()
        {
            this.Sotrs = new HashSet<Sotrs>();
        }
        public ICollection<Sotrs> Sotrs { get; set; }
    }

    public class Account
    {
        [Key]
        [Display(Name = "Логин аккаунта")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Логин должен иметь заглавные (A-Z), прописные (a-z) буквы и цифры (0-9)")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина Логина должна быть от 6 до 20 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Login { get; set; }
        [Display(Name = "Пароль аккаунта")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Пароль должен иметь заглавные (A-Z), прописные (a-z) буквы и цифры (0-9)")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина Пароля должна быть от 6 до 20 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Password { get; set; }
        [ForeignKey("Roles")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Роль")]
        public int Role_ID { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Sotrs Sotrs { get; set; }
    }

    public class Roles
    {
        [Key]
        public int ID_Role { get; set; }
        [Display(Name = "Наименование роли")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Длина наименования роли от 0 до 50 символов")]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Role_Naim { get; set; }
        [Display(Name = "Разрешить права доступа для администратора")]
        public int Manip_Roles { get; set; }
        [Display(Name = "Разрешить просматривать данные сотрудников")]
        public int Manip_Sotrs { get; set; }
        [Display(Name = "Разрешить просматривать данные структуры организации")]
        public int Manip_Department { get; set; }
        [Display(Name = "Разрешить просматривать данные бухгалтерского учета")]
        public int Buh_Ych { get; set; }
        public Roles()
        {
            this.Account = new HashSet<Account>();
        }
        public ICollection<Account> Account { get; set; }

    }

    public class Zar_Plata : IValidatableObject
    {
        [Key]
        public int ID_Zar { get; set; }
        [ForeignKey("Sotrs")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ФИО сотрудника")]
        public int Sotr_ID { get; set; }
        [Display(Name = "Дата выплаты заработной платы")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Data_Viplati { get; set; }
        [Display(Name = "Время выплаты заработной платы")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Vremya_Viplati { get; set; }
        [Display(Name = "НДС")]
        [RegularExpression(@"^([0-9 .&'-]+)$", ErrorMessage = "Поле может содержать только следующие символы: 0-9 и .")]
        [Required(ErrorMessage = "Указанный НДС введен неправильно (Должны содержаться числа и(или) .)")]
        public decimal NDS { get; set; }
        [Display(Name = "Заработная плата с вычетом НДС")]
        [RegularExpression(@"^([0-9 .&'-]+)$", ErrorMessage = "Поле может содержать только следующие символы: 0-9 и .")]
        [Required(ErrorMessage = "Указанный НДС введен неправильно (Должны содержаться числа и(или) .)")]
        public decimal Itog { get; set; }
        public virtual Sotrs Sotrs { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Convert.ToDateTime(Data_Viplati) < DateTime.Now)
            {
                results.Add(new ValidationResult("Дата выплаты не может быть меньше, чем сегодня", new[] { "Data_Viplati" }));
            }
            return results;
        }
    }

    public class Sotrs : IValidatableObject
    {
        [Key]
        public int ID_Sotr { get; set; }
        [Display(Name = "Фамилия")]
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Surname_Sot { get; set; }
        [Display(Name = "Имя")]
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Name_Sot { get; set; }
        [Display(Name = "Отчество")]
        [RegularExpression("^([а-яА-Я .&'-]+)$", ErrorMessage = "Поле должно иметь заглавные (А-Я), прописные (а-я) буквы")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 50 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Petronumic_Sot { get; set; }

        public string Day_Of_Birth { get; set; }
        [Display(Name = "Адрес")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина поля должна быть от 1 до 200 символов")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Address { get; set; }
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Num_Phone { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Адрес эл. почы введен неверно")]
        [Required(ErrorMessage = "Данное поле неверно заполнено")]
        public string Email { get; set; }
        [Display(Name = "Загрузить документ...")]
        [Required(ErrorMessage = "Если документа нет, укажите прочерк (-)")]
        public string Photo { get; set; }
        [ForeignKey("Positions")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Должность")]
        public int Positions_ID { get; set; }
        [ForeignKey("Rates")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Ставка")]
        public int Rate_ID { get; set; }
        [ForeignKey("Work_Schedule")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "График работы")]
        public int Schedule_ID { get; set; }
        public string Date_of_adoption { get; set; }
        public string Opisanie { get; set; }
        public virtual Positions Positions { get; set; }
        public virtual Rates Rates { get; set; }
        public virtual Work_Schedule Work_Schedule { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (Convert.ToDateTime(Day_Of_Birth) < DateTime.Now.AddYears(-18))
            {
                results.Add(new ValidationResult("Сотрудник должен быть старше 18 лет", new[] { "Day_Of_Birth" }));
            }
            return results;
        }
    }
}