using Project.CrossCutting.Base;
using Project.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Project.Service
{
    public class GenericApplicationService
    {
        protected readonly UnitOfWork _uow;
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10;

        public GenericApplicationService(UnitOfWork uow)
        {
            _uow = uow;
        }

        public static string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA512);
            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Iterations}.{salt}.{key}";
        }

        public static (bool Verified, bool NeedsUpgrade) CheckHash(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
                throw new FormatException("Unexpected hash format. " + "Should be formatted as `{iterations}.{salt}.{hash}`");

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Iterations;

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA512);
            var keyToCheck = algorithm.GetBytes(KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return (verified, needsUpgrade);
        }

        public string ToJson(object obj) => JsonConvert.SerializeObject(obj);

        public void VerifyExists(object obj, string name, string msg = "{0} não foi encontrado.")
        {
            if (obj == null)
                Error(string.Format(msg, name));
        }

        public void AssertIsNotEmpty(string field, string value)
        {
            if (string.IsNullOrEmpty(value))
                Error(field + " é obrigatório");
        }

        public void AssertIsNotZero(string field, int value)
        {
            if (value == 0)
                Error(field + " deve ser maior que zero");
        }

        public void Error(string msg)
        {
            throw new DomainException(msg);
        }
    }
}