using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            // Hiệu chỉnh cửa sổ đăng nhập
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
        }

        KetNoi ketNoi = new KetNoi();

        // Hàm xử lý mã hóa MD5 copy trên mạng
        private string getMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private string maHoa(string matKhau)
        {
            return getMd5(matKhau);
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // Xử lý ẩn mật khẩu
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin đăng nhập
            if (maHoa(txtMatKhau.Text) == ketNoi.layMatKhau(txtTenDangNhap.Text))
            {
                // Trả kết quả đăng nhập thành công về cho frmChinh
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // Khi sai thông tin đăng nhập, xóa mật khẩu cho người dùng nhập lại
                txtMatKhau.Clear();
                txtMatKhau.Focus();
                // Trả kết quả đăng nhập thất bại về cho frmChinh
                this.DialogResult = DialogResult.Retry;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Trả kết quả về cho frmChinh
            this.DialogResult = DialogResult.Ignore;
        }

        private void lblTenDangNhap_Click(object sender, EventArgs e)
        {

        }
    }
}
