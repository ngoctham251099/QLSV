using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmChinh : Form
    {
        public frmChinh()
        {
            InitializeComponent();
        }

        // Tạo mới các đối tượng cửa sổ (forms)
        frmDangNhap fDN = new frmDangNhap();
        frmKhoa fKhoa = new frmKhoa();
        frmSinhVien fSV = new frmSinhVien();
        frmLop fLop = new frmLop();
        frmThongTin fTT = new frmThongTin();

        // Mở hoặc khóa các chức năng: trangThai = true thì mở
        private void moChucNang(bool trangThai)
        {
            dangNhapToolStripMenuItem.Enabled = !trangThai;
            dangXuatToolStripMenuItem.Enabled = trangThai;
            quanLyToolStripMenuItem.Enabled = trangThai;
        }

        public void closeAllForm()
        {
            fKhoa.Close();
            fSV.Close();
            fLop.Close();
        }

        private void frmChinh_Load(object sender, EventArgs e)
        {
            // Mở frmChinh kích cỡ tối đa
            this.WindowState = FormWindowState.Maximized;
            // Đặt frmChinh là cửa sổ cha có thể chứa các cửa sổ khác bên trong
            this.IsMdiContainer = true;
            
            // Tắt chức năng để chờ đăng nhập
            moChucNang(false);
           // dangNhapToolStripMenuItem.Enabled = false;
           // MessageBox.Show("Khánh chó");
            
            // Mở cửa sổ đăng nhập
            dangNhapToolStripMenuItem_Click(sender, e);

            // Tạm mở chức năng và bỏ qua đăng nhập
            moChucNang(true);
        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO Trước khi thoát, nhớ xử lý các vấn đề lưu trữ và đóng các cửa sổ con
            Application.Exit();
        }

        private void dangNhapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Xử lý đăng nhập
            fDN.StartPosition = FormStartPosition.CenterScreen;
            switch(fDN.ShowDialog())
            {
                case DialogResult.OK:
                    // Thông báo đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Khi đăng nhập thành công, mở khóa các chức năng
                    moChucNang(true);
                    break;
                case DialogResult.Retry:
                    // Thông báo đăng nhập thất bại
                    MessageBox.Show("Sai thông tin đăng nhập. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Khi đăng nhập thất bại, mở lại cửa sổ đăng nhập
                    dangNhapToolStripMenuItem_Click(sender, e);
                    break;
                case DialogResult.Ignore:
                    // Khi thoát đăng nhập, xử lý thoát chương trình
                    thoatToolStripMenuItem_Click(sender, e);
                    break;
            }
        }

        private void dangXuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllForm();
            // Khóa các chức năng
            moChucNang(false);
            // Mở cửa sổ đăng nhập
            dangNhapToolStripMenuItem_Click(sender, e);
        }

        private void khoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllForm();
            if (fKhoa.IsDisposed)
                fKhoa = new frmKhoa();
            fKhoa.MdiParent = this;
            fKhoa.WindowState = FormWindowState.Maximized;
            fKhoa.Show();           
        }

        private void sinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllForm();
            if (fSV.IsDisposed)
                fSV = new frmSinhVien();
            fSV.MdiParent = this;
            fSV.WindowState = FormWindowState.Maximized;
            fSV.Show(); 
        }

        private void lopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllForm();
            if (fLop.IsDisposed)
                fLop = new frmLop();
            fLop.MdiParent = this;
            fLop.WindowState = FormWindowState.Maximized;
            fLop.Show();
        }

        private void thongTinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllForm();
            if (fTT.IsDisposed)
                fTT = new frmThongTin();
            fTT.MdiParent = this;
         //   fTT.WindowState = FormWindowState.Maximized;
            fTT.Show();
        }
    }
}
