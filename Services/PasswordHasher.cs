using System.Security.Cryptography;
using System.Text;

namespace MedicineProject.Services
{
    /// <summary>
    /// Сервис для безопасного хеширования паролей
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 128 бит
        private const int HashSize = 32; // 256 бит
        private const int Iterations = 10000; // Количество итераций PBKDF2

        /// <summary>
        /// Хеширует пароль с использованием PBKDF2
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым", nameof(password));

            // Генерируем случайную соль
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Хешируем пароль с использованием PBKDF2
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            // Объединяем соль и хеш в одну строку
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Проверяет, соответствует ли пароль хешу
        /// </summary>
        public static bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                // Декодируем хеш из Base64
                byte[] hashBytes = Convert.FromBase64String(passwordHash);

                // Извлекаем соль
                byte[] salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                // Вычисляем хеш для введенного пароля
                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    HashSize
                );

                // Сравниваем хеши
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
