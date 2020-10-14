using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public class KetNoi
    {
        string connString = null;
        SqlConnection conn = null;
        SqlDataAdapter da = null;
        DataSet ds = null;

        public KetNoi() {
            connString = @"Data Source=.\SQLEXPRESS;Initial Catalog=QLSV_5;User=sa;Password=sql2014";
            conn = new SqlConnection(connString);
            conn.Open();
        }

        public string layMatKhau(string username)
        {
            da = new SqlDataAdapter("SELECT MatKhau FROM TAIKHOAN WHERE TenDangNhap='" + username + "'", conn);
            ds = new DataSet();
            da.Fill(ds, "TAIKHOAN");
            //kiem tra xem query co tra ve row nào khong, neu khong tra ve null
            if (da.Fill(ds, "TAIKHOAN") != 0)
                return ds.Tables["TAIKHOAN"].Rows[0]["MatKhau"].ToString();
            else
                return null;
        }

        public string layMotDuLieuDon(string sql)
        {
            da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows[0][0].ToString();
        }

        public Object layBangDuLieu(string sql)
        {
            try
            {
                // Lấy dữ liệu
                da = new SqlDataAdapter(sql, conn); // sql chứa SELECT
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        public SqlException thucThiKhongLayDuLieu(string sql)
        {
            try
            {
                // Không lấy dữ liệu
                SqlCommand cmd = new SqlCommand(sql, conn); // sql chứa INSERT hoặc UPDATE hoặc DELETE
                cmd.ExecuteNonQuery();
                return null;
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        public void Close()
        {
            conn.Close();
        }
    }
}
