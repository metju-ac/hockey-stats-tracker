namespace Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider {
    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        return await Task.FromResult(new AuthenticationState(AnonymousUser));
    }

    private ClaimsPrincipal AnonymousUser => new(new ClaimsIdentity(Array.Empty<Claim>()));
    private ClaimsPrincipal FakedUser {
        get {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "john"),
                new Claim(ClaimTypes.Role, "user"),
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            return new ClaimsPrincipal(identity);
        }
    }
    private ClaimsPrincipal FakedAdmin {
        get {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "john"),
                new Claim(ClaimTypes.Role, "admin"),
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            var user =  new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            return user;
        }
    }
    public void FakedSignIn() {
        var result = Task.FromResult(new AuthenticationState(FakedUser));
        NotifyAuthenticationStateChanged(result);
    }
    public void FakedAdminSignIn() {
        var result = Task.FromResult(new AuthenticationState(FakedAdmin));
        NotifyAuthenticationStateChanged(result);
    }
    public void FakedSignOut() {
        var result = Task.FromResult(new AuthenticationState(AnonymousUser));
        NotifyAuthenticationStateChanged(result);
    }
    
    public async Task SignIn()
    {
        var claims = new[] 
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "admin")
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var user = new ClaimsPrincipal(identity);

        // Notify the system that the user's sign-in state has changed
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        // Validate the username and password
        // This is just a simple example, you should replace this with your own validation logic
    }
}
