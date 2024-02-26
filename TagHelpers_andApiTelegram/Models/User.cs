namespace TagHelpers_andApiTelegram.Models
{
    /*public class User
    {
        public int Id { get; set; } 
        public string Login { get; set; }
        public string Password {  get; set; }
    }*/

    //public record class User(int Id, string Email, string Password, int RoleId, Role Role);

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public User() { }
        public User(int id, string email, string password, int roleId, Role role)
        {
            Id = id;
            Email = email;
            Password = password;
            RoleId = roleId;
            Role = role;
        }

        public User(string email, string password, int roleId, Role role)
        {
            Email = email;
            Password = password;
            RoleId = roleId;
            Role = role;
        }
    }
    public record class Role
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public Role(int id, string name, List<User> users)
        {
            Id = id;
            Name = name;
            Users = users;
        }

        /*public Role(string name, List<User> users)
        {
            Name = name;
            Users = users;
        }*/

        public Role()
        {
            Users = new List<User>();
        }
    }
}
