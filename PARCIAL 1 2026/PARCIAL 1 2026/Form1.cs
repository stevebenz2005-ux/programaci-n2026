using System;
using System.Windows.Forms;

namespace PARCIAL_1_2026
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // El resultado no se puede modificar
            textBox2.ReadOnly = true;
        }

        // BOTÓN CALCULAR
        private void button1_Click(object sender, EventArgs e)
        {
            double monto;
            double impuesto;

            // Validar que sea un número
            if (!double.TryParse(textBox1.Text, out monto))
            {
                MessageBox.Show(
                    "Ingrese un monto válido.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            // Validar que no sea negativo
            if (monto < 0)
            {
                MessageBox.Show(
                    "El monto no puede ser negativo.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            // Cálculo
            if (monto <= 1000)
            {
                impuesto = 3;
            }
            else
            {
                impuesto = ((monto - 1000.01) / 1000) * 3 + 3;
            }

            // Mostrar resultado
            textBox2.Text = "$" + impuesto.ToString("0.00");
        }

        // BOTÓN VER CÁLCULO
        private void button2_Click(object sender, EventArgs e)
        {
            double monto;
            double rango;
            double impuesto;

            // Validar monto
            if (!double.TryParse(textBox1.Text, out monto))
            {
                MessageBox.Show(
                    "Ingrese primero un monto.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Validar negativo
            if (monto < 0)
            {
                MessageBox.Show(
                    "El monto no puede ser negativo.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            // Si el monto es menor o igual a 1000
            if (monto <= 1000)
            {
                impuesto = 3;

                MessageBox.Show(
                    "MONTO: $" + monto.ToString("0.00") +
                    "\n\n" +
                    "Valor fijo: $3.00" +
                    "\n\n" +
                    "IMPUESTO A PAGAR: $" + impuesto.ToString("0.00"),
                    "Cálculo del impuesto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // Obtener el monto desde el rango
            rango = monto - 1000.01;

            // Calcular impuesto
            impuesto = (rango / 1000) * 3 + 3;

            // Mostrar TODO el desarrollo
            MessageBox.Show(
                "Funciona correctamente según lo especificado\n\n" +

                "Ejemplo:\n" +
                "Si un negocio tiene una actividad económica de $" +
                monto.ToString("0.00") +
                " su valor a pagar es: $" +
                impuesto.ToString("0.00") +

                "\n\nMonto - desde (rango): " +
                rango.ToString("0.00") +

                "\n\nFórmula:\n" +
                rango.ToString("0.00") +
                " / 1000 * 3 + 3 = $" +
                impuesto.ToString("0.00") +

                "\n\nDesarrollo:\n" +
                monto.ToString("0.00") +
                " - 1000.01 = " +
                rango.ToString("0.00") +
                "\n" +
                rango.ToString("0.00") +
                " / 1000 * 3 + 3 = $" +
                impuesto.ToString("0.00"),

                "Resultado del cálculo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // EVENTOS DEL DISEÑADOR

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
