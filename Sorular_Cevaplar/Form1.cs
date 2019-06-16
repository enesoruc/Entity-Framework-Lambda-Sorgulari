using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorular_Cevaplar
{
    public partial class Form1 : Form
    {
        NORTHWNDEntities db;
        public Form1()
        {
            InitializeComponent();
            db = new NORTHWNDEntities();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Soru1();
            //Soru2();
            //Soru3();
            //Soru4();
            //Soru5();
            //Soru6();
            //Soru7();
            //soru8();
            Soru9();
        }  

        private void Soru1()
        {
            var result = (from od in db.Order_Details
                          select ((float)od.UnitPrice * od.Quantity * (1 - od.Discount))).Sum();
            textBox1.Text = result.ToString();
        }

        private void Soru2()//1997 de tüm cirom ne kadar?
        {
            var result = (from od in db.Order_Details
                       join o in db.Orders on od.OrderID equals o.OrderID
                       where o.OrderDate.Value.Year == 1997
                       select ((float)od.UnitPrice * od.Quantity * (1 - od.Discount))).Sum();

            textBox1.Text=result.ToString();            
        }
        private void Soru3()//Bugün doğum günü olan çalışanlarım kimler?
        {
            var result = (from e in db.Employees
                          where e.BirthDate.Value.Date==DateTime.Now.Date
                          select e).ToList();
            dataGridView1.DataSource = result;
        }
        private void Soru4()//Hangi çalışanım hangi çalışanıma bağlı?
        {
            var result = (from e in db.Employees
                          group e by new { e.FirstName,ReportsTo=e.EmployeeID } into g
                          select new {Calisan=g.Key.FirstName,
                                      Rapor =(from e in db.Employees
                                               where e.ReportsTo==g.Key.ReportsTo
                                              select e.FirstName).FirstOrDefault()});

            dataGridView1.DataSource = result;
        }
        private void Soru5()//Çalışanlarım ne kadarlık satış yapmışlar?
        {
            var result = (from e in db.Employees
                          join o in db.Orders on e.EmployeeID equals o.EmployeeID
                          join od in db.Order_Details on o.OrderID equals od.OrderID
                          group od by new { calisan = e.FirstName + " " + e.LastName } into g
                          select new { g.Key.calisan, Tutar = g.Sum(x => (float)x.UnitPrice * x.Quantity * (1 - x.Discount)) }).ToList();
            dataGridView1.DataSource = result;
        }
        private void Soru6()//Hangi ülkelere ihracat yapıyorum?
        {
            var result = (from o in db.Orders
                          select new
                          { Ulke = o.ShipCountry }).Distinct().ToList();
            dataGridView1.DataSource = result;
        }

        private void Soru7()
        {
            var result = (from p in db.Products
                          join od in db.Order_Details on p.ProductID equals od.ProductID
                          select new { p.ProductName, Ciro = ((float)od.UnitPrice * od.Quantity * (1 - od.Discount)) }).ToList();
            dataGridView1.DataSource = result;
        }

        private void soru8()
        {
            var result = (from c in db.Categories
                          join p in db.Products on c.CategoryID equals p.CategoryID
                          join od in db.Order_Details on p.ProductID equals od.ProductID
                          group od by new { Kategori = c.CategoryName } into g
                          select new { g.Key.Kategori, Tutar = g.Sum(x => (float)x.UnitPrice * x.Quantity * (1 - x.Discount)) }).ToList();
            dataGridView1.DataSource = result;
        }

        private void Soru9()//Ürün kategorilerine göre satışlarım nasıl? (sayı bazında)
        {
            var result = (from c in db.Categories
                          join p in db.Products on c.CategoryID equals p.CategoryID
                          join od in db.Order_Details on p.ProductID equals od.ProductID
                          group od by new { c.CategoryName } into g
                          select new
                          {
                              kategori = g.Key.CategoryName,
                              SiparisSayisi = g.Count()
                          }).ToList();
            dataGridView1.DataSource = result;
        }
    }
}
