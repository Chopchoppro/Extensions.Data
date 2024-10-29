using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ChopChop.Extensions.Identity.Firebase.Models;
using ChopChop.Extensions.Identity.Firebase.Services.Infostruct;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChopChop.Extensions.Identity.Firebase.Services;


public class FirebaseServices : IFirebaseServices
{
    protected readonly HttpClient HttpClient;
    protected readonly string apiKey;
    public FirebaseServices(HttpClient httpClient, string apiKey)
    {
        this.HttpClient = httpClient;
        this.apiKey = apiKey;
    }

    public async Task<LoginModel> Login(string username, string password, CancellationToken cancellationToken)
    {
        var uri = $"accounts:signInWithPassword?key={apiKey}";
        var request = new { email = username, password, returnSecureToken = true };
        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{uri}");
        using var response = await HttpClient.PostAsJsonAsync("", request); 
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<LoginModel>(json);
    }
 

    public Task<bool> ResetPassword(string email, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendVerifyEmail(string lang, string code)
    {
        var uri = $"__/auth/action?mode=verifyEmail&oobCode={code}&apiKey={apiKey}&lang={lang}";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = await HttpClient.SendAsync(req);
        if (!response.IsSuccessStatusCode)
            return false;
        var json = await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<string> SendVerifyEmail(string idToken,  CancellationToken cancellationToken)
    {
        var uri = $"accounts:sendOobCode?key={apiKey}";
        var request = new { idToken, requestType= "VERIFY_EMAIL" };
        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{uri}");
        using var response = await HttpClient.PostAsJsonAsync("", request);
        if (!response.IsSuccessStatusCode)
            return null;
        
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<LoginModel> SignupNewUser(string email, string password, bool returnSecureToken = false, CancellationToken cancellationToken = default)
    {
        var uri = $"accounts:signUp?key={apiKey}";
        var request = new { email,password, returnSecureToken };
        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{uri}");
        using var response = await HttpClient.PostAsJsonAsync("", request);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<LoginModel>(json);
    }

    public async Task<VerifyCustomTokenModel> VerifyCustomToken(string token, bool returnSecureToken, CancellationToken cancellationToken)
    {
        var uri = $"accounts:signInWithCustomToken?key={apiKey}";
        var request = new { token, returnSecureToken };
        HttpClient.BaseAddress = new Uri($"{HttpClient.BaseAddress}{uri}");
        using var response = await HttpClient.PostAsJsonAsync("", request);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<VerifyCustomTokenModel>(json);
    }
}
