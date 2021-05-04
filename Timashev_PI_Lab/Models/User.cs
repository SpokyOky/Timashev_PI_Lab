using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class User
    {
        public int? Id { get; set; }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("ФИО")]
        public string FIO { get; set; }
    }
}
