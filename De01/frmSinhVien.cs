using De01.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace De01
{
    
    public partial class Form1 : Form
    {
        Model1 db = new Model1();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.SinhViens.ToList();
            dataGridView1.Columns[3].Visible = false;
            List<Lop> listLop = db.Lops.ToList();
            fillDataCBB(listLop);
            cbbLop.SelectedItem = null;
        }
        private void fillDataCBB(List<Lop> classes)
        {
            cbbLop.DataSource = classes;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtHoTenSV.Text = dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            txtMaSV.Text = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            dtNgaySinh.Text = dataGridView1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            cbbLop.Text = dataGridView1.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
            btnLuu.Enabled = btnKhong.Enabled = false; 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien s = new SinhVien();
            if (cbbLop.SelectedItem == null)
            {
                MessageBox.Show("Chua chon lop cho sv", "Loi khi them",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtHoTenSV.Text.Count() > 0)
            {
                s.HoTen = txtHoTenSV.Text.Trim();
            }
            else
            {

                MessageBox.Show("Vui long nhap ten", "Lỗi khi thêm",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckID(txtMaSV.Text))
            {
                s.MaSV = txtMaSV.Text.Trim();
            }
            else
            {
                MessageBox.Show("Ma sinh vien da ton tai", "Loi khi them",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            s.NgaySinh = dtNgaySinh.Value;
            s.MaLop = cbbLop.SelectedValue.ToString();
            try
            {
                db.SinhViens.Add(s);
                MessageBox.Show("Da them thanh cong:  ",
                           "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);

                db.SaveChanges();
                Form1_Load(sender, e);
                reset();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
        private bool CheckID(string id)
        {
            var listStu = db.SinhViens.ToList();
            foreach (var s in listStu)
            {
                if (s.MaSV.Equals(id)) return false;
            }
            return true;
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            var rs = db.SinhViens.FirstOrDefault(s => s.MaSV == txtMaSV.Text.Trim());
            if (rs != null)
            {
                MessageBox.Show("Ban co muon xoa khong","Thong bao",MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.SinhViens.Remove(rs);
                MessageBox.Show("Da xoa:  ",
                          "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.SaveChanges();
                Form1_Load(sender, e);
                reset();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var rs = db.SinhViens.FirstOrDefault(s => s.MaSV == txtMaSV.Text.Trim());
            if (rs != null)
            {
               btnThem.Enabled = false;
               btnLuu.Enabled = btnKhong.Enabled = true;

            }
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var rs = db.SinhViens.FirstOrDefault(s => s.MaSV == txtMaSV.Text.Trim());
            if (cbbLop.SelectedItem == null)
            {
                MessageBox.Show("Chua chon lop cho sv", "Loi khi them",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtHoTenSV.Text.Count() > 0)
            {
                rs.HoTen = txtHoTenSV.Text.Trim();
            }
            else
            {

                MessageBox.Show("Vui long nhap ten", "Lỗi khi thêm",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            rs.NgaySinh = dtNgaySinh.Value;
            rs.MaLop = cbbLop.SelectedValue.ToString();
            try
            {
               
               
                db.SaveChanges();
                MessageBox.Show("Da sua thanh cong:  ",
                          "Thanh cong", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form1_Load(sender, e);
                reset();
                btnLuu.Enabled = btnKhong.Enabled =false;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnKhong.Enabled = btnLuu.Enabled= false;
            reset();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ban co muon thoat khong","Thong bao", MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }
        public void reset()
        {
            txtHoTenSV.Text = "";
            txtMaSV.Text = "";
            dtNgaySinh.Text = DateTime.Now.ToString();
            cbbLop.SelectedItem = null;
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string key = txtTim.Text.Trim();
            if(!string.IsNullOrEmpty(key) )
            {
                var query = db.SinhViens.Where(student => student.HoTen.Contains(key)).ToList();
                dataGridView1.DataSource = query;
            }
            else
            {
                return;
            }
        }
    }
}
