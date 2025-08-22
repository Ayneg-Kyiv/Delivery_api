using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class NextAuthController(IAccountService accountService) : ControllerBase
    {
        [HttpGet("csrf")]
        public IActionResult GetCsrfToken()
        {
            // Generate a simple CSRF token for NextAuth.js compatibility
            var token = GenerateCsrfToken();
            return Ok(new { csrfToken = token });
        }

        [HttpGet("session")]
        public async Task<IActionResult> GetSession()
        {
            // For now, return empty session - you can implement session validation here
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
            {
                return Ok(new { user = (object?)null });
            }

            // You can validate JWT token here and return user info
            return Ok(new { user = (object?)null });
        }

        [HttpPost("callback/credentials")]
        public async Task<IActionResult> CredentialsCallback([FromBody] SigninDTO request)
        {
            try
            {
                // Log incoming request for debugging
                Console.WriteLine($"NextAuth Credentials - Email: {request?.Email}, Password present: {!string.IsNullOrEmpty(request?.Password)}");
                
                if (request == null)
                {
                    Console.WriteLine("Request is null");
                    return BadRequest(new { error = "Request body is empty" });
                }

                var result = await accountService.SigninAsync(request, HttpContext);
                
                Console.WriteLine($"SigninAsync result - Success: {result.Success}, Message: {result.Message}");

                if (result.Success && result.Data != null)
                {
                    // Use reflection to access anonymous object properties
                    var dataType = result.Data.GetType();
                    var idProp = dataType.GetProperty("id");
                    var nameProp = dataType.GetProperty("name");
                    var emailProp = dataType.GetProperty("email");
                    var tokenProp = dataType.GetProperty("token");
                    var rolesProp = dataType.GetProperty("roles");
                    
                    var userId = idProp?.GetValue(result.Data)?.ToString();
                    var userName = nameProp?.GetValue(result.Data)?.ToString();
                    var userEmail = emailProp?.GetValue(result.Data)?.ToString();
                    var userToken = tokenProp?.GetValue(result.Data)?.ToString();
                    var userRoles = rolesProp?.GetValue(result.Data) as IEnumerable<string> ?? new List<string>();
                    
                    Console.WriteLine($"User data retrieved - ID: {userId}");
                    
                    // Return in format expected by NextAuth.js
                    return Ok(new
                    {
                        token = userToken,  // Direct token access for testing
                        user = new
                        {
                            id = userId,
                            name = userName,
                            email = userEmail,
                            token = userToken,
                            roles = userRoles
                        }
                    });
                }

                Console.WriteLine($"Login failed: {result.Message}");
                return Unauthorized(new { error = result.Message ?? "Invalid credentials" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CredentialsCallback: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("signout")]
        public async Task<IActionResult> NextAuthSignout()
        {
            var result = await accountService.SignoutAsync(HttpContext);
            
            if (result.Success) 
                return Ok(new { url = "/" });

            return BadRequest(result);
        }

        [HttpGet("userdata")]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var result = await accountService.GetUserDataAsync(HttpContext, CancellationToken.None);
                
                if (result.Success && result.Data != null)
                {
                    // Використовуємо рефлексію для отримання даних користувача
                    var dataType = result.Data.GetType();
                    var firstNameProp = dataType.GetProperty("firstName") ?? dataType.GetProperty("FirstName");
                    var lastNameProp = dataType.GetProperty("lastName") ?? dataType.GetProperty("LastName");
                    var middleNameProp = dataType.GetProperty("middleName") ?? dataType.GetProperty("MiddleName");
                    var emailProp = dataType.GetProperty("email") ?? dataType.GetProperty("Email");
                    var idProp = dataType.GetProperty("id") ?? dataType.GetProperty("Id");
                    
                    var firstName = firstNameProp?.GetValue(result.Data)?.ToString() ?? "";
                    var lastName = lastNameProp?.GetValue(result.Data)?.ToString() ?? "";
                    var middleName = middleNameProp?.GetValue(result.Data)?.ToString() ?? "";
                    var email = emailProp?.GetValue(result.Data)?.ToString() ?? "";
                    var userId = idProp?.GetValue(result.Data)?.ToString() ?? "";
                    
                    Console.WriteLine($"User data retrieved - FirstName: {firstName}, LastName: {lastName}, MiddleName: {middleName}");
                    
                    return Ok(new
                    {
                        id = userId,
                        email = email,
                        firstName = firstName,
                        lastName = lastName,
                        middleName = middleName,
                        fullName = $"{lastName} {firstName} {middleName}".Trim()
                    });
                }

                return Unauthorized("User not authenticated or user data not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetUserData: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }

        private string GenerateCsrfToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
