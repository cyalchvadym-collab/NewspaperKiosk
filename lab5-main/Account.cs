namespace NewspaperKioskLab5
{
    class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public Account() { }

        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}

