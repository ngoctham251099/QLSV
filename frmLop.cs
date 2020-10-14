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
    public partial class frmLop : Form
    {

        KetNoi ketNoi = new KetNoi();
        int khiLuu = 00;
        int Them = 10;
        int Sua = 20;
        private void layDuLieu()
        {
            string cauTruyVan = "SELECT MaLop, TenLop, SiSo, MaKhoa FROM LOP";
            object duLieu = ketNoi.layBangDuLieu(cauTruyVan);
            dgvLop.DataSource = duLieu;
            string cauTruyVanKhoa = "SELECT MaKhoa, TenKhoa FROM KHOA";
            cboMaKhoa.DataSource = ketNoi.layBangDuLieu(cauTruyVanKhoa);
            cboMaKhoa.DisplayMember = "TenKhoa";
            cboMaKhoa.ValueMember = "MaKhoa";

            // Việt hóa tiêu đề các cột
            dgvLop.Columns["MaLop"].HeaderText = "Mã lớp";
            dgvLop.Columns["TenLop"].HeaderText = "Tên lớp";
            dgvLop.Columns["SiSo"].HeaderText = "Sỉ Số";
            dgvLop.Columns["MaKhoa"].HeaderText = "Mã khoa";
            dgvLop.Columns["TenLop"].Width =260;
        }

        private void choPhepNhapDuLieu(bool trangThai)
        {
            txtMaLop.Enabled = trangThai;
            txtTenLop.Enabled = trangThai;
          //  txtSiSo.Enabled = trangThai;
            cboMaKhoa.Enabled = trangThai;

            btnLuu.Enabled = trangThai;
            btnHuy.Enabled = trangThai;

            btnThem.Enabled = !trangThai;
            btnSua.Enabled = !trangThai;
        }

        public frmLop()
        {
            InitializeComponent();

        }

        private void btbThem_Click(object sender, EventArgs e)
        {
            khiLuu = Them;
            choPhepNhapDuLieu(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            khiLuu = Sua;
            choPhepNhapDuLieu(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string cauTruyVan = "DELETE LOP WHERE MaLop = '" + txtMaLop.Text + "'";
            SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
            if (ex != null)
            {
                if (ex.Number == 547)
                {
                    string cauTruyVanLayDSSV = "SELECT * FROM SINHVIEN WHERE MaLop = '" + txtMaLop.Text + "'";
                    object objSV = ketNoi.layBangDuLieu(cauTruyVanLayDSSV);
                    if (objSV != null)
                    {
                        DataTable dtSV = (DataTable)objSV;
                        string dsTenSV = "[ ";
                        foreach (DataRow dong in dtSV.Rows)
                        {
                            dsTenSV += dong["MaSV"].ToString() + ", ";
                        }
                        dsTenSV = dsTenSV.Substring(0, dsTenSV.Length - 2) + " ]";
                        MessageBox.Show("Không xóa được lớp do còn có các sinh viên trực thuộc lớp này:\n" + dsTenSV + "\n Vui lòng xóa các sinh viên đó trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Không xóa được lớp do còn có các sinh viên trực thuộc lớp này.\n Vui lòng xóa các sinh viên đó trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }else
                    MessageBox.Show("Không xóa được lop do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                layDuLieu();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Text != "")
            {
                if (khiLuu == Them)
                {
                    // Thêm khoa vào CSDL
                    string cauTruyVan = "INSERT INTO LOP (MaLop, TenLop, MaKhoa) VALUES ('" + txtMaLop.Text + "', N'" + txtTenLop.Text +"','"+cboMaKhoa.SelectedValue+"')";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                            MessageBox.Show("Không thêm được lớp do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        layDuLieu();
                    }
                }
                if (khiLuu == Sua)
                {
                    // Sửa khoa trên CSDL
                    txtMaLop.ReadOnly = true;
                    string cauTruyVan = "UPDATE LOP SET TenLop = N'" + txtTenLop.Text +"', MaKhoa = '"+cboMaKhoa.SelectedValue+"' WHERE MaLop = '" + txtMaLop.Text + "'";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                        MessageBox.Show("Không sửa được lớp do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        layDuLieu();
                    }
                }
            }
            else
                MessageBox.Show("Mã khoa không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            choPhepNhapDuLieu(false);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            //txtMaKhoa.Clear();
            choPhepNhapDuLieu(false);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLop.Text = dgvLop.Rows[dgvLop.CurrentRow.Index].Cells["MaLop"].Value.ToString();
            txtTenLop.Text = dgvLop.Rows[dgvLop.CurrentRow.Index].Cells["TenLop"].Value.ToString();
            cboMaKhoa.SelectedValue = dgvLop.Rows[dgvLop.CurrentRow.Index].Cells["MaKhoa"].Value;
        }

        private void frmLop_Load(object sender, EventArgs e)
        {
            layDuLieu();
            // Ẩn cột tiêu đề của các dòng
            dgvLop.RowHeadersVisible = false;
            // Chỉ cho phép dgv chọn theo dòng
            dgvLop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Chỉ cho phép xem, không cho sửa dữ liệu trong dgv
            dgvLop.ReadOnly = true;
            // Không cho sửa kích thước dòng dgv
            dgvLop.AllowUserToResizeRows = false;
            // Không cho sửa kích thước cột dgv
            dgvLop.AllowUserToResizeColumns = false;
            // Không cho thêm dòng mới
            dgvLop.AllowUserToAddRows = false;
            // Không cho chọn nhiều dòng cùng một lúc
            dgvLop.MultiSelect = false;

            choPhepNhapDuLieu(false);
        }

        private void lbTenLop_Click(object sender, EventArgs e)
        {

        }

       // private void 
    }
}
