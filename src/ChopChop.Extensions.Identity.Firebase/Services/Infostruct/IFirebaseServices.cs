using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChopChop.Extensions.Identity.Firebase.Models;

namespace ChopChop.Extensions.Identity.Firebase.Services.Infostruct;

public interface IFirebaseServices
{
    Task<string> SendVerifyEmail(string idToken, CancellationToken cancellationToken);
    Task<bool> ConfirmVerifyEmail(string oobCode, CancellationToken cancellationToken);

    Task<VerifyCustomTokenModel> VerifyCustomToken(string token, bool returnSecureToken, CancellationToken cancellationToken);

    Task<LoginModel> SignupNewUser(string email, string password,bool returnSecureToken=false, CancellationToken cancellationToken);
    
    Task<bool> ResetPassword(string email, string password, CancellationToken cancellationToken);

    Task<LoginModel> Login(string username, string password,CancellationToken cancellationToken);
}
