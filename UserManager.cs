
namespace ATMUygulamasi
{
    public class UserManager
    {
        public List<User> UserList { get; set; }


        public UserManager()
        {
            UserList = new List<User>();
        }

        public void Add(User user)
        {
            user.UserId = GetNewId();
            UserList.Add(user);


        }
        private int GetNewId()
        {
            if (UserList.Any())
            {
                int id = UserList.Max(a => a.UserId) + 1;
                return id;
            }
            else
                return 1;


        }
        public decimal GetBalance(int userId)
        {
            decimal balance = UserList.First(a => a.UserId == userId).Balance;
            return balance;
        }
        public User Login(string userName, string password)
        {
            User user = UserList.FirstOrDefault(a => a.UserName.ToLower() == userName.ToLower() && a.Password == password);
            return user;
        }
        public User Find(string userName)
        {
            User user = UserList.FirstOrDefault(a => a.UserName == userName);
            return user;
        }
        public void UpdateBalance(decimal balance, int userId)
        {
            UserList.First(a => a.UserId == userId).Balance = balance;

        }
    }

}