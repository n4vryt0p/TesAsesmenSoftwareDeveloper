using static FrontEnd.Pages.Auth.LoginModel;

namespace FrontEnd.Models;

public class AuthViewModel
{
    //public UserDto AppUser { get; set; } = null!;
    public IEnumerable<string>? AppRoles { get; set; }
    public IEnumerable<PrivilegeVm>? Privileges { get; set; }
}

public class PrivilegeVm
{
}