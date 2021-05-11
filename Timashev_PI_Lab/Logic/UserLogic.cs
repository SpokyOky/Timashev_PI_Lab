using Timashev_PI_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Timashev_PI_Lab.Logic
{
    public class UserLogic
    {
        private Database context;

        public UserLogic(Database _context)
        {
            context = _context;
        }

        public bool CreateOrUpdate(User user)
        {
            User tempUser = user.Id.HasValue ? null : new User();

            if (user.Id.HasValue)
            {
                tempUser = context.Users.FirstOrDefault(rec => rec.Id == user.Id);

            }
            List<User> result = new List<User>();
            result.AddRange(context.Users
                .Where(rec => rec.Login == user.Login)
                .Select(rec => rec));
            if (result.Count > 0)
            {
                return false;
            }

            if (user.Id.HasValue)
            {
                if (tempUser == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempUser.Id = user.Id;
                tempUser.Login = user.Login;
                tempUser.Password = user.Password;
                tempUser.FIO = user.FIO;
            }
            else
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
            return true;
        }

        public void Delete(User user)
        {
            User element = context.Users.FirstOrDefault(rec => rec.Id == user.Id.Value);

            if (element != null)
            {
                context.Users.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<User> Read(User user)
        {
            List<User> result = new List<User>();

            if (user != null)
            {
                result.AddRange(context.Users
                    .Where(rec => (rec.Id == user.Id) || ((rec.Login == user.Login || rec.FIO == user.FIO)
                    && (user.Password != null && rec.Password == user.Password)))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Users);
            }

            return result;
        }
    }
}
