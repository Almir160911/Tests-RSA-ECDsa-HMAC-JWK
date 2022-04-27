using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace TestingJsonWebKeys
{
    public class RsaExample
    {
        private static DateTime Now = DateTime.Now;
        //private string scope;
        private static SecurityTokenDescriptor Jwt = new SecurityTokenDescriptor
        {
            Issuer = "viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io",
            Audience = "https://identityhomolog.acesso.io",
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim("scope", "*"),
              //  new Claim(JwtRegisteredClaimNames.GivenName, "almirfernandes"),
              //  new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            }),
            IssuedAt = Now,
            //    NotBefore = Now,
            Expires = Now.AddHours(1),

        };


        private static TokenValidationParameters TokenValidationParams = new TokenValidationParameters
        {
            ValidIssuer = "viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io",
            ValidAudience = "https://identityhomolog.acesso.io",
        };


        //public static void Run()
        //{
        //    var tokenHandler = new JsonWebTokenHandler();
        //    var key = new RsaSecurityKey(RSA.Create(2048))
        //    {
        //        //  KeyId = Guid.NewGuid().ToString()
        //        //                KeyId = "MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDPQvNh1v7XNEkj" +
        //        //"1vzc3wm7R0eA+9HYKbOqJH3XaZBIuN5GODeiXCr2bm3u7Ym1jjOSpEC7xEmbrrCw" +
        //        //"uyHyUK9n9OC9ijo+DxGkU8SIRdwk6X6V0E4w/2itl3aQz3/KchP13Wv7fJtCSrbo" +
        //        //"1jMRGTi3ArPMvW0A6So9Nspym/L0xzpkXHeuN9jH1Z5Os17555ylrmH7XB3g50Rf" +
        //        //"yFtUmpT8f0aj9MtGRHLNptLCb1FECsOPqaitt/mPiyv1QdHwbt7Qo5TlBdH7XJmb" +
        //        //"FCnDbNg3Sw+PIk8VHqBYS9EMFNTcmFNpjW1R0CSuGs9iGxsA+T49w03Uwr5QEZNT" +
        //        //"fb1ULLv5AgMBAAECggEBAJ9ykC5q6e7BiKsLz8hTxfbPpVCzagJ0QoXYQP+hdKCd" +
        //        //"qFI5GYPMiDjBR82R7GlCFqbvueU54q7kO4Ya0MjMDcVZ/DOUza3Ehfv3fbCgnfW/" +
        //        //"s+SpNh43v14csdhB6bvi+zwgtdvpRsA/HmwNrYev8ZPJfm06JUuYFSi2qBR3qPdp" +
        //        //"kutHfhKlHMiw6YBg7YS/smTwMlDBky4h7gR5f+Eb5bo12w8zvx0hQKJnB45m+BHq" +
        //        //"p4fDitr8SCTqbnCRvNxubZxhmydCG4riXKxXShRDmzbdUUOE+tDevRik6tQeLUQk" +
        //        //"RRpfxL7+naGAWTqT0jE+o9BBH1nGEPOgMbw7n6AJZwECgYEA79w3eASRb62KQfPz" +
        //        //"aP8zTNW3t095vhHSgf4QXWU5emtSnsyW0zwLk9azg9rKr3dqa0fFdvn3cvLnOLPj" +
        //        //"ihC6n1Lx0I/Lj5R6UAIrxfaKKn/l3VvXxbVe1j4vOKNMlJKjiNj11S6LwzZrmmLw" +
        //        //"/T4NmzJjObMUucChnnQBQbxQDPECgYEA3TUyD7o0/blA9gE6/Y6BdZilTDGTpvea" +
        //        //"vray9p7MuSs/8aIPD+j+J8SxzECO+pyB/nokvzssu52NLPOZf4CtHnEyi9C82ZYG" +
        //        //"P7ut1VjIpL8+E/bgkQBS7UrlkUkK3JBsweEBiWtM7Zt7MvNrfavDOW/djp4sk8JM" +
        //        //"ZYgTCwaOv4kCgYEAis9PzoYeQJoWxBpQyHfAEXwxSMWWt9eKkq4cfGBDRkRr2Pif" +
        //        //"ZHmAXbtHwkvRNoCONq2faj445O4Lv/Xi/Zi219NhnAhLjrknm7vV50fJJ9VCirtH" +
        //        //"AOT4kFYGb+mZPPYr69tbUVcRjRSmav9NZdEOgL1FTTQFD4aaQZ/yxj8buJECgYEA" +
        //        //"km5fWEfkQjz6wp5n5WcLTcQhUAHVgwaPOkYIy/atALqflp23qMmMTNkI9XOa8kze" +
        //        //"RI2oe6p5XyzZQnNzHhMZBnjag/FGwiZsto3PcdZW46/xMRgQ6guz2X7l3rniZcV5" +
        //        //"KRKAiO4tu3BcKos2kbc9AW2K1ZjhW2KYXfGo4XiS0DkCgYAJ9N7d7+JP3gRzuGWH" +
        //        //"cBPQTRI+57qAMRRsnpOSyA4v9FExBBN1nMT2YjOs1Z/ZcwqIZj5+BIajcDHiY8RZ" +
        //        //"cB9n1yOA2LNpjYrjgzA/mfSNLAcJ/AD3g8WcEXO7LciYwjrS2CrvrsqhIYqHVD9j" +
        //        //"l1WGv/C5/xMnPIG7uaXdSDWoDg=="
        //        //            };

        //        KeyId = "-----BEGIN PRIVATE KEY-----MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDPQvNh1v7XNEkj1vzc3wm7R0eA+9HYKbOqJH3XaZBIuN5GODeiXCr2bm3u7Ym1jjOSpEC7xEmbrrCwuyHyUK9n9OC9ijo+DxGkU8SIRdwk6X6V0E4w/2itl3aQz3/KchP13Wv7fJtCSrbo1jMRGTi3ArPMvW0A6So9Nspym/L0xzpkXHeuN9jH1Z5Os17555ylrmH7XB3g50RfyFtUmpT8f0aj9MtGRHLNptLCb1FECsOPqaitt/mPiyv1QdHwbt7Qo5TlBdH7XJmbFCnDbNg3Sw+PIk8VHqBYS9EMFNTcmFNpjW1R0CSuGs9iGxsA+T49w03Uwr5QEZNTfb1ULLv5AgMBAAECggEBAJ9ykC5q6e7BiKsLz8hTxfbPpVCzagJ0QoXYQP+hdKCdqFI5GYPMiDjBR82R7GlCFqbvueU54q7kO4Ya0MjMDcVZ/DOUza3Ehfv3fbCgnfW/s+SpNh43v14csdhB6bvi+zwgtdvpRsA/HmwNrYev8ZPJfm06JUuYFSi2qBR3qPdpkutHfhKlHMiw6YBg7YS/smTwMlDBky4h7gR5f+Eb5bo12w8zvx0hQKJnB45m+BHqp4fDitr8SCTqbnCRvNxubZxhmydCG4riXKxXShRDmzbdUUOE+tDevRik6tQeLUQkRRpfxL7+naGAWTqT0jE+o9BBH1nGEPOgMbw7n6AJZwECgYEA79w3eASRb62KQfPzaP8zTNW3t095vhHSgf4QXWU5emtSnsyW0zwLk9azg9rKr3dqa0fFdvn3cvLnOLPjihC6n1Lx0I/Lj5R6UAIrxfaKKn/l3VvXxbVe1j4vOKNMlJKjiNj11S6LwzZrmmLw/T4NmzJjObMUucChnnQBQbxQDPECgYEA3TUyD7o0/blA9gE6/Y6BdZilTDGTpveavray9p7MuSs/8aIPD+j+J8SxzECO+pyB/nokvzssu52NLPOZf4CtHnEyi9C82ZYGP7ut1VjIpL8+E/bgkQBS7UrlkUkK3JBsweEBiWtM7Zt7MvNrfavDOW/djp4sk8JMZYgTCwaOv4kCgYEAis9PzoYeQJoWxBpQyHfAEXwxSMWWt9eKkq4cfGBDRkRr2PifZHmAXbtHwkvRNoCONq2faj445O4Lv/Xi/Zi219NhnAhLjrknm7vV50fJJ9VCirtHAOT4kFYGb+mZPPYr69tbUVcRjRSmav9NZdEOgL1FTTQFD4aaQZ/yxj8buJECgYEAkm5fWEfkQjz6wp5n5WcLTcQhUAHVgwaPOkYIy/atALqflp23qMmMTNkI9XOa8kzeRI2oe6p5XyzZQnNzHhMZBnjag/FGwiZsto3PcdZW46/xMRgQ6guz2X7l3rniZcV5KRKAiO4tu3BcKos2kbc9AW2K1ZjhW2KYXfGo4XiS0DkCgYAJ9N7d7+JP3gRzuGWHcBPQTRI+57qAMRRsnpOSyA4v9FExBBN1nMT2YjOs1Z/ZcwqIZj5+BIajcDHiY8RZcB9n1yOA2LNpjYrjgzA/mfSNLAcJ/AD3g8WcEXO7LciYwjrS2CrvrsqhIYqHVD9jl1WGv/C5/xMnPIG7uaXdSDWoDg==-----END PRIVATE KEY-----"
        //    };

        //    Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSsaPssSha256);

        //   // Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);
        //    var lastJws = tokenHandler.CreateToken(Jwt);

        //    Console.WriteLine($"{lastJws}{Environment.NewLine}");

        //    // Store in filesystem
        //    // Store HMAC os Filesystem, recover and test if it's valid
        //    var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
        //    File.WriteAllText("current-rsa.key", JsonConvert.SerializeObject(jwk));


        //    var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("current-rsa.key"));
        //    TokenValidationParams.IssuerSigningKey = storedJwk;
        //    var validationResult = tokenHandler.ValidateToken(lastJws, TokenValidationParams);

        //    Console.WriteLine(validationResult.IsValid);
        //}

        public static void Run()
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = new RsaSecurityKey(RSA.Create(2048))
            {

                KeyId = "-----BEGIN PRIVATE KEY-----MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDPQvNh1v7XNEkj1vzc3wm7R0eA+9HYKbOqJH3XaZBIuN5GODeiXCr2bm3u7Ym1jjOSpEC7xEmbrrCwuyHyUK9n9OC9ijo+DxGkU8SIRdwk6X6V0E4w/2itl3aQz3/KchP13Wv7fJtCSrbo1jMRGTi3ArPMvW0A6So9Nspym/L0xzpkXHeuN9jH1Z5Os17555ylrmH7XB3g50RfyFtUmpT8f0aj9MtGRHLNptLCb1FECsOPqaitt/mPiyv1QdHwbt7Qo5TlBdH7XJmbFCnDbNg3Sw+PIk8VHqBYS9EMFNTcmFNpjW1R0CSuGs9iGxsA+T49w03Uwr5QEZNTfb1ULLv5AgMBAAECggEBAJ9ykC5q6e7BiKsLz8hTxfbPpVCzagJ0QoXYQP+hdKCdqFI5GYPMiDjBR82R7GlCFqbvueU54q7kO4Ya0MjMDcVZ/DOUza3Ehfv3fbCgnfW/s+SpNh43v14csdhB6bvi+zwgtdvpRsA/HmwNrYev8ZPJfm06JUuYFSi2qBR3qPdpkutHfhKlHMiw6YBg7YS/smTwMlDBky4h7gR5f+Eb5bo12w8zvx0hQKJnB45m+BHqp4fDitr8SCTqbnCRvNxubZxhmydCG4riXKxXShRDmzbdUUOE+tDevRik6tQeLUQkRRpfxL7+naGAWTqT0jE+o9BBH1nGEPOgMbw7n6AJZwECgYEA79w3eASRb62KQfPzaP8zTNW3t095vhHSgf4QXWU5emtSnsyW0zwLk9azg9rKr3dqa0fFdvn3cvLnOLPjihC6n1Lx0I/Lj5R6UAIrxfaKKn/l3VvXxbVe1j4vOKNMlJKjiNj11S6LwzZrmmLw/T4NmzJjObMUucChnnQBQbxQDPECgYEA3TUyD7o0/blA9gE6/Y6BdZilTDGTpveavray9p7MuSs/8aIPD+j+J8SxzECO+pyB/nokvzssu52NLPOZf4CtHnEyi9C82ZYGP7ut1VjIpL8+E/bgkQBS7UrlkUkK3JBsweEBiWtM7Zt7MvNrfavDOW/djp4sk8JMZYgTCwaOv4kCgYEAis9PzoYeQJoWxBpQyHfAEXwxSMWWt9eKkq4cfGBDRkRr2PifZHmAXbtHwkvRNoCONq2faj445O4Lv/Xi/Zi219NhnAhLjrknm7vV50fJJ9VCirtHAOT4kFYGb+mZPPYr69tbUVcRjRSmav9NZdEOgL1FTTQFD4aaQZ/yxj8buJECgYEAkm5fWEfkQjz6wp5n5WcLTcQhUAHVgwaPOkYIy/atALqflp23qMmMTNkI9XOa8kzeRI2oe6p5XyzZQnNzHhMZBnjag/FGwiZsto3PcdZW46/xMRgQ6guz2X7l3rniZcV5KRKAiO4tu3BcKos2kbc9AW2K1ZjhW2KYXfGo4XiS0DkCgYAJ9N7d7+JP3gRzuGWHcBPQTRI+57qAMRRsnpOSyA4v9FExBBN1nMT2YjOs1Z/ZcwqIZj5+BIajcDHiY8RZcB9n1yOA2LNpjYrjgzA/mfSNLAcJ/AD3g8WcEXO7LciYwjrS2CrvrsqhIYqHVD9jl1WGv/C5/xMnPIG7uaXdSDWoDg==-----END PRIVATE KEY-----"

            };

            //  string json = @"{'d':{'media':12.108320606149539,'lote':'','Opcao':[{'__type':'Model','leitura':70,'producao':1579981660130}],'sinal':'Up'}}";

            //string payload = @"{'iss': 'viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io',
            //                    'aud': 'https://identityhomolog.acesso.io',
            //                    'scope': '*',
            //                    'iat': 1649945130,    
            //                    'exp': 1649948730}";

            //  string json = @"{'alg': 'RS256',  'typ': 'JWT'}";

            //IDictionary<string, string> openWith = new Dictionary<string, string>();

            //openWith.Add("alg", "RS256");
            //openWith.Add("typ", "JWT");

            //string json = openWith;


            // var cert = new X509Certificate2(Convert.FromBase64String(certPem));
            // can be combined with the private key from the previous section var certWithKey = cert.CopyWithPrivateKey(key);



            // var cabecalho = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9";

            var claims = new List<System.Security.Claims.Claim>
            {
              //  new System.Security.Claims.Claim("alg", "RS256", ClaimValueTypes.String),
              //  new System.Security.Claims.Claim("typ","JWT", ClaimValueTypes.String),
                new System.Security.Claims.Claim("iss", "viav_sac_hml@ffb5c609-81fc-420c-8b40-89a699d6feed.iam.acesso.io", ClaimValueTypes.String),
                new System.Security.Claims.Claim("aud", "https://identityhomolog.acesso.io", ClaimValueTypes.String),
                new System.Security.Claims.Claim("scope", "*", ClaimValueTypes.String),
                new System.Security.Claims.Claim("iat", EpochTime.GetIntDate( DateTime.Now).ToString(), ClaimValueTypes.Integer64),
                new System.Security.Claims.Claim("exp", EpochTime.GetIntDate(DateTime.Now.AddMinutes(120)).ToString(), ClaimValueTypes.Integer64),

            };

            JwtPayload payload = new JwtPayload(claims);
            var jsonPayload = payload.SerializeToJson();

            //  Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            //Jwt.SigningCredentials = new SigningCredentials(cabecalho, jsonPayload);

            // Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

            //var keyfinal = new RsaSecurityKey(RSA.Create(2048))
            //{

            //    KeyId = "-----BEGIN PRIVATE KEY-----MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDPQvNh1v7XNEkj1vzc3wm7R0eA+9HYKbOqJH3XaZBIuN5GODeiXCr2bm3u7Ym1jjOSpEC7xEmbrrCwuyHyUK9n9OC9ijo+DxGkU8SIRdwk6X6V0E4w/2itl3aQz3/KchP13Wv7fJtCSrbo1jMRGTi3ArPMvW0A6So9Nspym/L0xzpkXHeuN9jH1Z5Os17555ylrmH7XB3g50RfyFtUmpT8f0aj9MtGRHLNptLCb1FECsOPqaitt/mPiyv1QdHwbt7Qo5TlBdH7XJmbFCnDbNg3Sw+PIk8VHqBYS9EMFNTcmFNpjW1R0CSuGs9iGxsA+T49w03Uwr5QEZNTfb1ULLv5AgMBAAECggEBAJ9ykC5q6e7BiKsLz8hTxfbPpVCzagJ0QoXYQP+hdKCdqFI5GYPMiDjBR82R7GlCFqbvueU54q7kO4Ya0MjMDcVZ/DOUza3Ehfv3fbCgnfW/s+SpNh43v14csdhB6bvi+zwgtdvpRsA/HmwNrYev8ZPJfm06JUuYFSi2qBR3qPdpkutHfhKlHMiw6YBg7YS/smTwMlDBky4h7gR5f+Eb5bo12w8zvx0hQKJnB45m+BHqp4fDitr8SCTqbnCRvNxubZxhmydCG4riXKxXShRDmzbdUUOE+tDevRik6tQeLUQkRRpfxL7+naGAWTqT0jE+o9BBH1nGEPOgMbw7n6AJZwECgYEA79w3eASRb62KQfPzaP8zTNW3t095vhHSgf4QXWU5emtSnsyW0zwLk9azg9rKr3dqa0fFdvn3cvLnOLPjihC6n1Lx0I/Lj5R6UAIrxfaKKn/l3VvXxbVe1j4vOKNMlJKjiNj11S6LwzZrmmLw/T4NmzJjObMUucChnnQBQbxQDPECgYEA3TUyD7o0/blA9gE6/Y6BdZilTDGTpveavray9p7MuSs/8aIPD+j+J8SxzECO+pyB/nokvzssu52NLPOZf4CtHnEyi9C82ZYGP7ut1VjIpL8+E/bgkQBS7UrlkUkK3JBsweEBiWtM7Zt7MvNrfavDOW/djp4sk8JMZYgTCwaOv4kCgYEAis9PzoYeQJoWxBpQyHfAEXwxSMWWt9eKkq4cfGBDRkRr2PifZHmAXbtHwkvRNoCONq2faj445O4Lv/Xi/Zi219NhnAhLjrknm7vV50fJJ9VCirtHAOT4kFYGb+mZPPYr69tbUVcRjRSmav9NZdEOgL1FTTQFD4aaQZ/yxj8buJECgYEAkm5fWEfkQjz6wp5n5WcLTcQhUAHVgwaPOkYIy/atALqflp23qMmMTNkI9XOa8kzeRI2oe6p5XyzZQnNzHhMZBnjag/FGwiZsto3PcdZW46/xMRgQ6guz2X7l3rniZcV5KRKAiO4tu3BcKos2kbc9AW2K1ZjhW2KYXfGo4XiS0DkCgYAJ9N7d7+JP3gRzuGWHcBPQTRI+57qAMRRsnpOSyA4v9FExBBN1nMT2YjOs1Z/ZcwqIZj5+BIajcDHiY8RZcB9n1yOA2LNpjYrjgzA/mfSNLAcJ/AD3g8WcEXO7LciYwjrS2CrvrsqhIYqHVD9jl1WGv/C5/xMnPIG7uaXdSDWoDg==-----END PRIVATE KEY-----"

            //};

            //  var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            

            var lastJws = tokenHandler.CreateToken(jsonPayload, Jwt.SigningCredentials);

            Console.WriteLine($"{lastJws}{Environment.NewLine}");

            // Store in filesystem
            // Store HMAC os Filesystem, recover and test if it's valid
            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
            File.WriteAllText("current-rsa.key", JsonConvert.SerializeObject(jwk));


            var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("current-rsa.key"));
            TokenValidationParams.IssuerSigningKey = storedJwk;
            var validationResult = tokenHandler.ValidateToken(lastJws, TokenValidationParams);

            Console.WriteLine(validationResult.IsValid);
        }
    }
}