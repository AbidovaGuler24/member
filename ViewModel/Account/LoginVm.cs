using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModel.Account
{
    public class LoginVm
    {
        public string EmailOrUsername { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Remember { get; set; }  

    }
}
