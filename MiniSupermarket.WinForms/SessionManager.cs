namespace MiniSupermarket.WinForms
{
    public static class SessionManager
    {
        // JWT Token nhận từ Server
        public static string JwtToken { get; set; } = string.Empty;

        // Vai trò người dùng: Admin / Cashier
        public static string CurrentRole { get; set; } = string.Empty;

        // Địa chỉ API - chỉnh port nếu khác (xem launchSettings.json của API)
        public const string ApiBaseUrl = "https://localhost:7123/api/";
    }
}
