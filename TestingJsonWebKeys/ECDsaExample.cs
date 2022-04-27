using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace TestingJsonWebKeys
{
    public class ECDsaExample
    {
        private static DateTime Now = DateTime.Now;
        private static SecurityTokenDescriptor Jwt = new SecurityTokenDescriptor
        {
            Issuer = "viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io",
            Audience = "https://identityhomolog.acesso.io",
            IssuedAt = Now,
            NotBefore = Now,
            Expires = Now.AddHours(1),
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, "fernandesalmir31@gmail.com", ClaimValueTypes.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, "Almir Fernandes"),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            })
        };
        private static TokenValidationParameters TokenValidationParams = new TokenValidationParameters
        {
            ValidIssuer = "viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io",
            ValidAudience = "https://identityhomolog.acesso.io",
        };


        public static void Run()
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256))
            {
                // KeyId = Guid.NewGuid().ToString()
                KeyId = "MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDPQvNh1v7XNEkj1vzc3wm7R0eA+9HYKbOqJH3XaZBIuN5GODeiXCr2bm3u7Ym1jjOSpEC7xEmbrrCwuyHyUK9n9OC9ijo+DxGkU8SIRdwk6X6V0E4w/2itl3aQz3/KchP13Wv7fJtCSrbo1jMRGTi3ArPMvW0A6So9Nspym/L0xzpkXHeuN9jH1Z5Os17555ylrmH7XB3g50RfyFtUmpT8f0aj9MtGRHLNptLCb1FECsOPqaitt/mPiyv1QdHwbt7Qo5TlBdH7XJmbFCnDbNg3Sw+PIk8VHqBYS9EMFNTcmFNpjW1R0CSuGs9iGxsA+T49w03Uwr5QEZNTfb1ULLv5AgMBAAECggEBAJ9ykC5q6e7BiKsLz8hTxfbPpVCzagJ0QoXYQP+hdKCdqFI5GYPMiDjBR82R7GlCFqbvueU54q7kO4Ya0MjMDcVZ/DOUza3Ehfv3fbCgnfW/s+SpNh43v14csdhB6bvi+zwgtdvpRsA/HmwNrYev8ZPJfm06JUuYFSi2qBR3qPdpkutHfhKlHMiw6YBg7YS/smTwMlDBky4h7gR5f+Eb5bo12w8zvx0hQKJnB45m+BHqp4fDitr8SCTqbnCRvNxubZxhmydCG4riXKxXShRDmzbdUUOE+tDevRik6tQeLUQkRRpfxL7+naGAWTqT0jE+o9BBH1nGEPOgMbw7n6AJZwECgYEA79w3eASRb62KQfPzaP8zTNW3t095vhHSgf4QXWU5emtSnsyW0zwLk9azg9rKr3dqa0fFdvn3cvLnOLPjihC6n1Lx0I/Lj5R6UAIrxfaKKn/l3VvXxbVe1j4vOKNMlJKjiNj11S6LwzZrmmLw/T4NmzJjObMUucChnnQBQbxQDPECgYEA3TUyD7o0/blA9gE6/Y6BdZilTDGTpveavray9p7MuSs/8aIPD+j+J8SxzECO+pyB/nokvzssu52NLPOZf4CtHnEyi9C82ZYgkQBS7UrlkUkK3JBsweEBiWtM7Zt7MvNrfavDOW/djp4sk8JMZYgTCwaOv4kCgYEAis9PzoYeQJoWxBpQyHfAEXwxSMWWt9eKkq4cfGBDRkRr2PifZHmAXbtHwkvRNoCONq2faj445O4Lv/Xi/Zi219NhnAhLjrknm7vV50fJJ9VCirtHAOT4kFYGb+mZPPYr69tbUVcRjRSmav9NZdEOgL1FTTQFD4aaQZ/yxj8buJECgYEAkm5fWEfkQjz6wp5n5WcLTcQhUAHVgwaPOkYIy/atALqflp23qMmMTNkI9XOa8kzeRI2oe6p5XyzZQnNzHhMZBnjag/FGwiZsto3PcdZW46/xMRgQ6guz2X7l3rniZcV5KRKAiO4tu3BcKos2kbc9AW2K1ZjhW2KYXfGo4XiS0DkCgYAJ9N7d7+JP3gRzuGWHcBPQTRI+57qAMRRsnpOSyA4v9FExBBN1nMT2YjOs1Z/ZcwqIZj5+BIajcDHiY8RZcB9n1yOA2LNpjYrjgzA/mfSNLAcJ/AD3g8WcEXO7LciYwjrS2CrvrsqhIYqHVD9jl1WGv/C5/xMnPIG7uaXdSDWoDg=="
            };



            Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            var lastJws = tokenHandler.CreateToken(Jwt);
            Console.WriteLine($"{lastJws}{Environment.NewLine}");


            // Store in filesystem
            // Store HMAC os Filesystem, recover and test if it's valid
            var parameters = key.ECDsa.ExportParameters(true);
            var jwk = new JsonWebKey()
            {
                Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                Use = "sig",
                Kid = key.KeyId,
                KeyId = key.KeyId,
                X = Base64UrlEncoder.Encode(parameters.Q.X),
                Y = Base64UrlEncoder.Encode(parameters.Q.Y),
                D = Base64UrlEncoder.Encode(parameters.D),
                Crv = JsonWebKeyECTypes.P256,
                Alg = "ES256"
            };

            File.WriteAllText("current-ecdsa.key", JsonConvert.SerializeObject(jwk));

            var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("current-ecdsa.key"));
            TokenValidationParams.IssuerSigningKey = storedJwk;
            var validationResult = tokenHandler.ValidateToken(lastJws, TokenValidationParams);

            Console.WriteLine(validationResult.IsValid);
        }
    }
}