namespace MiniSupermarket.WinForms
{
    partial class FormCategoryManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvCategories = new DataGridView();
            lblRole = new Label();
            lblName = new Label();
            lblDesc = new Label();
            txtName = new TextBox();
            txtDesc = new TextBox();
            btnAdd = new Button();
            btnDelete = new Button();
            btnReload = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            SuspendLayout();

            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRole.Location = new Point(15, 12);
            lblRole.Text = "Vai trò hiện tại:";

            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.Location = new Point(15, 45);
            dgvCategories.MultiSelect = false;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersWidth = 51;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.Size = new Size(650, 240);

            lblName.AutoSize = true;
            lblName.Location = new Point(15, 305);
            lblName.Text = "Tên danh mục:";

            txtName.Location = new Point(125, 302);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 27);

            lblDesc.AutoSize = true;
            lblDesc.Location = new Point(340, 305);
            lblDesc.Text = "Mô tả:";

            txtDesc.Location = new Point(395, 302);
            txtDesc.Name = "txtDesc";
            txtDesc.Size = new Size(270, 27);

            btnAdd.Location = new Point(15, 350);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 36);
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;

            btnDelete.Location = new Point(145, 350);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 36);
            btnDelete.Text = "Xóa (Admin)";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;

            btnReload.Location = new Point(275, 350);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(120, 36);
            btnReload.Text = "Tải lại";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(685, 405);
            Controls.Add(lblRole);
            Controls.Add(dgvCategories);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblDesc);
            Controls.Add(txtDesc);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(btnReload);
            Name = "FormCategoryManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý danh mục - MiniSupermarket";
            Load += FormCategoryManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvCategories;
        private Label lblRole;
        private Label lblName;
        private Label lblDesc;
        private TextBox txtName;
        private TextBox txtDesc;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnReload;
    }
}
