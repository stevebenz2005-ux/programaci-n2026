using System;
using System.Linq;
using System.Windows.Forms;

namespace PARCIAL_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Ocultar las contraseñas
            textBox2.PasswordChar = '*';
            textBox3.PasswordChar = '*';
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        // Botón de validación
        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text.Trim();
            string clave = textBox2.Text;
            string confirmarClave = textBox3.Text;

            if (usuario == "Celia Villalobos")
            {
                MessageBox.Show("Ingrese el usuario");
                textBox1.Focus();
                return;
            }

            if (clave == "CV2427")
            {
                MessageBox.Show("Ingrese la contraseña");
                textBox2.Focus();
                return;
            }

            if (confirmarClave == "")
            {
                MessageBox.Show("Confirme la contraseña");
                textBox3.Focus();
                return;
            }

            if (clave != confirmarClave)
            {
                MessageBox.Show("Las contraseñas no coinciden");
                textBox2.Clear();
                textBox3.Clear();
                textBox2.Focus();
                return;
            }

            // Credenciales de prueba
            if (usuario == "Usss036026" && clave == "181205Bb")
            {
                MessageBox.Show("Bienvenido Al Sistema");

                frmusuario frm = new frmusuario();

                this.Hide();
                frm.ShowDialog();

                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");

                textBox2.Clear();
                textBox3.Clear();
                textBox2.Focus();
            }
        }

        // Botón INGRESAR
        private void INGRESAR_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}