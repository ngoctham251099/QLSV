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
    public partial class frmKhoa : Form
    {
        public frmKhoa()
        {
            InitializeComponent();
        }

        const int Them = 10;
        const int Sua = 20;
        int khiLuu = 00;

        KetNoi ketNoi = new KetNoi();

        private void layDuLieu()
        {
            string cauTruyVan = "SELECT MaKhoa, TenKhoa FROM KHOA";
            object duLieu = ketNoi.layBangDuLieu(cauTruyVan);
            dgvKhoa.DataSource = duLieu;

            // Việt hóa tiêu đề các cột
            dgvKhoa.Columns["MaKhoa"].HeaderText = "Mã khoa";
            dgvKhoa.Columns["TenKhoa"].HeaderText = "Tên khoa";
            dgvKhoa.Columns["TenKhoa"].Width = 402;
        }

        private void choPhepNhapDuLieu(bool trangThai)
        {
            txtMaKhoa.Enabled = trangThai;
            txtTenKhoa.Enabled = trangThai;

            btnLuu.Enabled = trangThai;
            btnHuy.Enabled = trangThai;

            btnThem.Enabled = !trangThai;
            btnSua.Enabled = !trangThai;
        }

        private void frmKhoa_Load(object sender, EventArgs e)
        {
            layDuLieu();
            // Ẩn cột tiêu đề của các dòng
            dgvKhoa.RowHeadersVisible = false;
            // Chỉ cho phép dgv chọn theo dòng
            dgvKhoa.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Chỉ cho phép xem, không cho sửa dữ liệu trong dgv
            dgvKhoa.ReadOnly = true;
            // Không cho sửa kích thước dòng dgv
            dgvKhoa.AllowUserToResizeRows = false;
            // Không cho sửa kích thước cột dgv
            dgvKhoa.AllowUserToResizeColumns = false;
            // Không cho thêm dòng mới
            dgvKhoa.AllowUserToAddRows = false;
            // Không cho chọn nhiều dòng cùng một lúc
            dgvKhoa.MultiSelect = false;

            choPhepNhapDuLieu(false);
        }

        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaKhoa.Text = dgvKhoa.Rows[dgvKhoa.CurrentRow.Index].Cells["MaKhoa"].Value.ToString();
            txtTenKhoa.Text = dgvKhoa.Rows[dgvKhoa.CurrentRow.Index].Cells["TenKhoa"].Value.ToString();
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
            choPhepNhapDuLieu(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaKhoa.Text != "")
            {
                if (khiLuu == Them)
                {
                    // Thêm khoa vào CSDL
                    string cauTruyVan = "INSERT INTO KHOA VALUES ('" + txtMaKhoa.Text + "', N'" + txtTenKhoa.Text + "')";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                        MessageBox.Show("Không thêm được khoa do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        layDuLieu();
                    }
                }
                if (khiLuu == Sua)
                {
                    // Sửa khoa trên CSDL
                    string cauTruyVan = "UPDATE KHOA SET TenKhoa = N'" + txtTenKhoa.Text + "' WHERE MaKhoa = '" + txtMaKhoa.Text + "'";
                    SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
                    if (ex != null)
                    {
                        MessageBox.Show("Không sửa được khoa do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtTenKhoa.Clear();
            choPhepNhapDuLieu(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            //
            string cauTruyVan = "DELETE KHOA WHERE MaKhoa = '" + txtMaKhoa.Text + "'";
            SqlException ex = ketNoi.thucThiKhongLayDuLieu(cauTruyVan);
            if (ex != null)
            {
                if (ex.Number == 547)
                {
                    string cauTruyVanLayDSLop = "SELECT MaLop, TenLop FROM LOP WHERE MaKhoa = '" + txtMaKhoa.Text + "'";
                    object objLop = ketNoi.layBangDuLieu(cauTruyVanLayDSLop);
                    if (objLop != null)
                    {
                        DataTable dtLop = (DataTable)objLop;
                        string dsTenLop = "[ ";
                        foreach (DataRow dong in dtLop.Rows)
                        {
                            dsTenLop += dong["MaLop"].ToString() + ", ";
                        }
                        dsTenLop = dsTenLop.Substring(0, dsTenLop.Length - 2) + " ]";
                        MessageBox.Show("Không xóa được khoa do còn có các lớp trực thuộc khoa này:\n" + dsTenLop + "\n Vui lòng xóa các lớp đó trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Không xóa được khoa do còn có các lớp trực thuộc khoa này.\n Vui lòng xóa các lớp đó trước!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không xóa được khoa do lỗi: (" + ex.Number + ") " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                layDuLieu();
            }
        }
    }
}
