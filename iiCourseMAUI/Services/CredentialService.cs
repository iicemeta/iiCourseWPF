using System.Text.Json;
using System.Text;

namespace iiCourseMAUI.Services
{
    /// <summary>
    /// 跨平台安全凭据存储服务
    /// 使用 MAUI SecureStorage 实现跨平台安全存储
    /// Android: 使用 Android KeyStore + EncryptedSharedPreferences
    /// iOS: 使用 Keychain
    /// Windows: 使用 DPAPI
    /// </summary>
    public class CredentialService
    {
        private const string CredentialKey = "iicourse_credentials";
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("iiCourseMAUI_Credential_Protection_2024");

        /// <summary>
        /// 保存凭据到安全存储
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="rememberPassword">是否记住密码</param>
        public async Task SaveCredentialsAsync(string username, string password, bool rememberPassword)
        {
            try
            {
                var credential = new CredentialData
                {
                    Username = username,
                    Password = rememberPassword ? password : string.Empty,
                    RememberPassword = rememberPassword
                };

                var json = JsonSerializer.Serialize(credential);
                var encryptedData = EncryptData(json);
                var base64Data = Convert.ToBase64String(encryptedData);

                await SecureStorage.SetAsync(CredentialKey, base64Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存凭据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载保存的凭据
        /// </summary>
        public async Task<CredentialData?> LoadCredentialsAsync()
        {
            try
            {
                var base64Data = await SecureStorage.GetAsync(CredentialKey);
                if (string.IsNullOrEmpty(base64Data))
                {
                    return null;
                }

                var encryptedData = Convert.FromBase64String(base64Data);
                var json = DecryptData(encryptedData);

                return JsonSerializer.Deserialize<CredentialData>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载凭据失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 清除保存的凭据
        /// </summary>
        public void ClearCredentials()
        {
            try
            {
                SecureStorage.Remove(CredentialKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清除凭据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否存在保存的凭据
        /// </summary>
        public async Task<bool> HasSavedCredentialsAsync()
        {
            try
            {
                var data = await SecureStorage.GetAsync(CredentialKey);
                return !string.IsNullOrEmpty(data);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 简单的 XOR 加密（配合 SecureStorage 使用）
        /// </summary>
        private static byte[] EncryptData(string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var result = new byte[dataBytes.Length];

            for (int i = 0; i < dataBytes.Length; i++)
            {
                result[i] = (byte)(dataBytes[i] ^ Entropy[i % Entropy.Length]);
            }

            return result;
        }

        /// <summary>
        /// 简单的 XOR 解密
        /// </summary>
        private static string DecryptData(byte[] data)
        {
            var result = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ Entropy[i % Entropy.Length]);
            }

            return Encoding.UTF8.GetString(result);
        }
    }

    /// <summary>
    /// 凭据数据模型
    /// </summary>
    public class CredentialData
    {
        /// <summary>
        /// 用户名（学号）
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool RememberPassword { get; set; }
    }
}
