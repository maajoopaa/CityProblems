namespace CityProblems.Models
{
    public class AccountViewModel
    {
        public SignInModel? signInModel { get; set; }

        public SignUpModel? signUpModel { get; set; }

        public string Error { get; set; } = null!;
    }

    public class SignInModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }

    public class SignUpModel
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
