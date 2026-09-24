using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategoryManagement : Form
    {
        public FormCategoryManagement()
        {
            InitializeComponent();
        }

        // Tạo HttpClient có đính kèm Bearer Token
        private HttpClient GetAuthenticatedClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(SessionManager.ApiBaseUrl)
            };

            if (!string.IsNullOrEmpty(SessionManager.JwtToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }
            return client;
        }

        private async void FormCategoryManagement_Load(object sender, EventArgs e)
        {
            lblRole.Text = $"Vai trò hiện tại: {SessionManager.CurrentRole}";
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient();
                var categories = await client.GetFromJsonAsync<List<CategoryDto>>("categories");
                dgvCategories.DataSource = categories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quyền truy cập hoặc mất kết nối: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnReload_Click(object sender, EventArgs e) => await LoadDataAsync();

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nhập tên danh mục!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var client = GetAuthenticatedClient();
                var dto = new CategoryDto { Name = txtName.Text.Trim(), Description = txtDesc.Text.Trim() };
                var res = await client.PostAsJsonAsync("categories", dto);
                if (res.IsSuccessStatusCode)
                {
                    txtName.Clear();
                    txtDesc.Clear();
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show($"Thêm thất bại: {(int)res.StatusCode} {res.StatusCode}", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow?.DataBoundItem is not CategoryDto selected)
            {
                MessageBox.Show("Chọn 1 dòng để xóa!");
                return;
            }

            try
            {
                using var client = GetAuthenticatedClient();
                var res = await client.DeleteAsync($"categories/{selected.Id}");

                if (res.IsSuccessStatusCode)
                {
                    await LoadDataAsync();
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("403 Forbidden: Chỉ Admin mới có quyền xóa danh mục!", "Không đủ quyền",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Phiên làm việc hết hạn hoặc chưa đăng nhập!");
                }
                else
                {
                    MessageBox.Show($"Xóa thất bại: {(int)res.StatusCode} {res.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
