//using System.Security.Cryptography;
//using System.Text;

//namespace Api.Middleware
//{
//    public class EncryptionMiddleware(RequestDelegate next, IConfiguration configuration)
//    {
//        private readonly RequestDelegate _next = next;
//        private readonly byte[] Key = Encoding.UTF8.GetBytes(configuration["Encryption:Key"] ?? throw new NullReferenceException()); // 32 bytes for AES-256
//        private readonly byte[] IV = Encoding.UTF8.GetBytes(configuration["Encryption:IV"] ?? throw new NullReferenceException()); // 16 bytes for AES

//        public async Task InvokeAsync(HttpContext context)
//        {
//            // Decrypt request body if present
//            if (context.Request.ContentLength > 0 && context.Request.ContentType == "application/octet-stream")
//            {
//                using var ms = new MemoryStream();
//                await context.Request.Body.CopyToAsync(ms);
//                var encryptedBytes = ms.ToArray();
//                var decryptedJson = Decrypt(encryptedBytes);

//                var newStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedJson));
//                context.Request.Body = newStream;
//                context.Request.ContentLength = newStream.Length;
//                context.Request.Body.Position = 0;
//                context.Request.ContentType = "application/json";
//            }

//            // Capture and encrypt response
//            //var originalBody = context.Response.Body;
//            //using var newBody = new MemoryStream();
//            //context.Response.Body = newBody;

//            await _next(context);

//            //if (context.Response.ContentType == "application/json")
//            //{
//            //    newBody.Seek(0, SeekOrigin.Begin);
//            //    var plainText = await new StreamReader(newBody).ReadToEndAsync();
//            //    var encrypted = Encrypt(plainText);
//            //    context.Response.ContentType = "application/octet-stream";
//            //    context.Response.ContentLength = encrypted.Length;
//            //    await originalBody.WriteAsync(encrypted, 0, encrypted.Length);
//            //}
//            //else
//            //{
//            //    newBody.Seek(0, SeekOrigin.Begin);
//            //    await newBody.CopyToAsync(originalBody);
//            //}
//        }

//        private byte[] Encrypt(string plainText)
//        {
//            using var aes = Aes.Create();
//            aes.Key = Key;
//            aes.IV = IV;
//            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
//            using var ms = new MemoryStream();
//            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
//            using (var sw = new StreamWriter(cs))
//                sw.Write(plainText);
//            return ms.ToArray();
//        }

//        private string Decrypt(byte[] cipherBytes)
//        {
//            using var aes = Aes.Create();
//            aes.Key = Key;
//            aes.IV = IV;
//            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
//            using var ms = new MemoryStream(cipherBytes);
//            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
//            using var sr = new StreamReader(cs);
//            return sr.ReadToEnd();
//        }
//    }

//    public static class EncryptionMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseEncryptionMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<EncryptionMiddleware>();
//        }
//    }
//}