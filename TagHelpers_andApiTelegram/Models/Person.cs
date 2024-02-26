namespace TagHelpers_andApiTelegram.Models
{
    public class Person
    {
        public Person(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login {  get; set; }
        public string Password { get; set; }
    }
}
