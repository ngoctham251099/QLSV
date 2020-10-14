using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLySinhVien
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }

        KetNoi ketNoi = new KetNoi();
        SqlConnection conn = new SqlConnection();

        const int Them = 10;
        const int Sua = 20;
        int khiLuu = 00;

        string maLopCu;

        private void layDuLieu() 
        {
            string cauTruyVan = "SELECT MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, MaLop FROM SINHVIEN";
            dgvSinhVien.DataSource = ketNoi.layBangDuLieu(cauTruyVan);
            string cauTruyVanLop = "SELECT MaLop, TenLop FROM LOP";
            cboMaLop.DataSource = ketNoi.layBangDuLieu(cauTruyVanLop);
            cboMaLop.DisplayMember = "TenLop";
            cboMaLop.ValueMember = "MaLop";

            dgvSinhVien.Columns["MaSv"].HeaderText = "Mã sinh viên";
            dgvSinhVien.Columns["TenSV"].HeaderText = "Tên sinh viên";
            dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới tính";
            dgvSinhVien.Columns["QueQuan"].HeaderText = "Quê quán";
            dgvSinhVien.Columns["MaLop"].HeaderText = "Mã lớp";

            dgvSinhVien.Columns["TenSV"].Width = 120;
            
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            layDuLieu();

            // Ẩn cột tiêu đề của các dòng
            dgvSinhVien.RowHeadersVisible = false;
            // Chỉ cho phép dgv chọn theo dòng
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Chỉ cho phép xem, không cho sửa dữ liệu trong dgv
            dgvSinhVien.ReadOnly = true;
            // Không cho sửa kích thước dòng dgv
            dgvSinhVien.AllowUserToResizeRows = false;
            // Không cho sửa kích thước cột dgv
            dgvSinhVien.AllowUserToResizeColumns = false;
            // Không cho thêm dòng mới
            dgvSinhVien.AllowUserToAddRows = false;
            // Không cho chọn nhiều dòng cùng một lúc
            dgvSinhVien.MultiSelect = false;

            // Cấu hình định dạng của dtpNgaySinh
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd - MM - yyyy";

            choPhepNhapDuLieu(false);
            cboCachTim.Text = "Mã Sinh viên";
        }

        private void choPhepNhapDuLieu(bool trangThai)
        {
            txtMaSV.Enabled = trangThai;
            txtTenSV.Enabled = trangThai;
            txtQueQuan.Enabled = trangThai;
            cboMaLop.Enabled = trangThai;
            dtpNgaySinh.Enabled = trangThai;
            radNam.Enabled = trangThai;
            radNu.Enabled = trangThai;

            btnLuu.Enabled = trangThai;
            btnHuy.Enabled = trangThai;

            btnThem.Enabled = !trangThai;
            btnSua.Enabled = !trangThai;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            khiLuu = Them;
            choPhepNhapDuLieu(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            khiLuu = Sua;
            maLopCu = cboMaLop.SelectedValue.ToString();
            choPhepNhapDuLieu(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string cauTruyVan = "DELETE SINHVIEN WHERE MaSV = '" + txtMaSV.Text + "'";
            SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
            if (ex != null)
            {
                MessageBox.Show("Không xóa được khoa do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string cauTruyVanLop = "UPDATE LOP SET SiSo = SiSo - 1 WHERE MaLop = '" + cboMaLop.SelectedValue + "'";
                ketNoi.thucThiKhongLayDuLieu(cauTruyVanLop);
                layDuLieu();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text != "" && cboMaLop.SelectedValue != "")
            {
                if (khiLuu == Them)
                {
                    //// Thêm SinhVien vào CSDL
                    string strNgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
                    string strGioiTinh = (radNam.Checked) ? "Nam" : "Nữ";
                    string cauTruyVan = "INSERT INTO SINHVIEN VALUES ('" + txtMaSV.Text + "', N'" + txtTenSV.Text + "', '"+strNgaySinh+"', N'"+strGioiTinh+"', N'"+txtQueQuan.Text+"', '"+cboMaLop.SelectedValue+"')";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                        MessageBox.Show("Không thêm được sinh viên do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string cauTruyVanLop = "UPDATE LOP SET SiSo = SiSo + 1 WHERE MaLop = '"+cboMaLop.SelectedValue+"'";
                        ketNoi.thucThiKhongLayDuLieu(cauTruyVanLop);
                        layDuLieu();
                    }
                }
                if (khiLuu == Sua)
                {
                    // Sửa khoa trên CSDL
                    string strNgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
                    string strGioiTinh = (radNam.Checked) ? "Nam" : "Nữ";
                    string cauTruyVan = "UPDATE SINHVIEN SET TenSV = N'" + txtTenSV.Text + "', NgaySinh = '" + strNgaySinh + "', GioiTinh = '" + strGioiTinh + "', QueQuan = N'" + txtQueQuan.Text + "', MaLop = '" + cboMaLop.SelectedValue + "' WHERE MaSV = '" + txtMaSV.Text + "'";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                        MessageBox.Show("Không sửa được khoa do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                            if (maLopCu != cboMaLop.SelectedValue)
                            {
                                string cauTruyVanGiamSiSoLop = "UPDATE LOP SET SiSo = SiSo - 1 WHERE MaLop = '" + maLopCu + "'";
                                ketNoi.thucThiKhongLayDuLieu(cauTruyVanGiamSiSoLop);
                                string cauTruyVanTangSiSoLop = "UPDATE LOP SET SiSo = SiSo + 1 WHERE MaLop = '" + cboMaLop.SelectedValue + "'";
                                ketNoi.thucThiKhongLayDuLieu(cauTruyVanTangSiSoLop);
                            }
                        layDuLieu();
                    }
                }
            }
            else
                MessageBox.Show("Mã sinh viên và mã lớp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            //txtTimKiem.Clear();
            txtMaSV.Clear();
            txtTenSV.Clear();
            txtQueQuan.Clear();
            choPhepNhapDuLieu(false);
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells["MaSV"].Value.ToString();
            txtTenSV.Text = dgvSinhVien.Rows[e.RowIndex].Cells["TenSV"].Value.ToString();
            dtpNgaySinh.Value = (DateTime)dgvSinhVien.Rows[e.RowIndex].Cells["NgaySinh"].Value;
            if (dgvSinhVien.Rows[e.RowIndex].Cells["GioiTinh"].Value.ToString() == "Nam")
            {
                radNam.Checked = true;
            }
            else
            {
                radNu.Checked = true;
            }
            txtQueQuan.Text = dgvSinhVien.Rows[e.RowIndex].Cells["QueQuan"].Value.ToString();
            cboMaLop.SelectedValue = dgvSinhVien.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
        }

        private void lbTenSV_Click(object sender, EventArgs e)
        {
        }

       /* public DataTable HienDL(string )
        {
            string cauTruyVan = "SELECT MaSV, TenSV, NgaySinh, GioiTinh, QueQuan, MaLop FROM SINHVIEN";
            dgvSinhVien.DataSource = ketNoi.layBangDuLieu(cauTruyVan);
        }*/

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboCachTim.Text == "Mã Sinh viên")
            {
                dgvSinhVien.DataSource = ketNoi.layBangDuLieu("SELECT * FROM SINHVIEN where MaSV like'%" + txtTimKiem.Text.Trim() + "%'");
               // layDuLieu();
            }

            else
            {
                dgvSinhVien.DataSource = ketNoi.layBangDuLieu("SELECT * FROM SINHVIEN where TenSV like N'%" + txtTimKiem.Text.Trim() + "%'");
                //layDuLieu();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (cboCachTim.Text == "Mã Sinh viên")
            {
                dgvSinhVien.DataSource = ketNoi.layBangDuLieu("SELECT * FROM SINHVIEN where MaSV like'%" + txtTimKiem.Text.Trim() + "%'");
                //layDuLieu();
            }
            else
            {
                dgvSinhVien.DataSource =ketNoi.layBangDuLieu( "SELECT * FROM SINHVIEN where TenSV like N'%" + txtTimKiem.Text.Trim() + "%'");
               // layDuLieu();
            }
        }

        private void cboMaLop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
