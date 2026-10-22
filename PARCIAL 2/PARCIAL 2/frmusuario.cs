using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PARCIAL_2
{
    public partial class frmusuario : Form
    {
        private readonly string conexion =
            @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\usuariodb.mdf;Integrated Security=True;Connect Timeout=30;";

        private DataTable tabla = new DataTable();
        private int posicion = -1;
        private bool guardando = false;
        private bool navegando = false;

        public frmusuario()
        {
            InitializeComponent();

            // Conectar eventos una sola vez.
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            button3.Click -= button3_Click;
            button3.Click += button3_Click;

            button4.Click -= button4_Click;
            button4.Click += button4_Click;

            button5.Click -= button5_Click;
            button5.Click += button5_Click;

            button6.Click -= button6_Click;
            button6.Click += button6_Click;

            button7.Click -= button7_Click;
            button7.Click += button7_Click;

            dataGridView1.SelectionChanged -= dataGridView1_SelectionChanged;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            CargarDatos();
        }

        // Evento Load requerido por el diseñador.
        private void frmusuario_Load(object sender, EventArgs e)
        {
            // Los datos se cargan en el constructor.
        }

        // CARGAR USUARIOS DESDE LA BASE DE DATOS.
        private void CargarDatos()
        {
            try
            {
                DataTable nuevaTabla = new DataTable();

                using (SqlConnection cn = new SqlConnection(conexion))
                using (SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT idUsuario, usuario, clave, nombre, direccion, telefono " +
                    "FROM usuario_usss36026 ORDER BY idUsuario", cn))
                {
                    da.Fill(nuevaTabla);
                }

                navegando = true;
                tabla = nuevaTabla;

                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = tabla;

                if (tabla.Rows.Count > 0)
                {
                    MostrarRegistro(0);
                }
                else
                {
                    posicion = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los usuarios:\n" + ex.Message,
                    "Error SQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                navegando = false;
                ActualizarNavegacion();
            }
        }

        // MOSTRAR UN REGISTRO.
        private void MostrarRegistro(int indice)
        {
            if (dataGridView1.Rows.Count == 0 ||
                indice < 0 ||
                indice >= dataGridView1.Rows.Count)
            {
                posicion = -1;
                ActualizarNavegacion();
                return;
            }

            navegando = true;

            try
            {
                posicion = indice;
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell =
                    dataGridView1.Rows[indice].Cells[0];
                dataGridView1.Rows[indice].Selected = true;
            }
            finally
            {
                navegando = false;
            }

            ActualizarNavegacion();
        }

        // HABILITAR LOS BOTONES CUANDO HAY REGISTROS.
        private void ActualizarNavegacion()
        {
            bool hayRegistros = dataGridView1.Rows.Count > 0;

            button1.Enabled = hayRegistros;
            button2.Enabled = hayRegistros;
            button3.Enabled = hayRegistros;
            button4.Enabled = hayRegistros;

            button6.Enabled = hayRegistros;
            button7.Enabled = hayRegistros;
        }

        // BOTÓN 1: PRIMER REGISTRO.
        private void button1_Click(object sender, EventArgs e)
        {
            NavegarA(0);
        }

        // BOTÓN 2: REGISTRO ANTERIOR.
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
                NavegarA(dataGridView1.CurrentRow.Index - 1);
        }

        // BOTÓN 3: REGISTRO SIGUIENTE.
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
                NavegarA(dataGridView1.CurrentRow.Index + 1);
        }

        // BOTÓN 4: ÚLTIMO REGISTRO.
        private void button4_Click(object sender, EventArgs e)
        {
            NavegarA(dataGridView1.Rows.Count - 1);
        }

        // MÉTODO GENERAL PARA NAVEGAR.
        private void NavegarA(int indice)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay usuarios para navegar.");
                return;
            }

            if (indice < 0 || indice >= dataGridView1.Rows.Count)
                return;

            MostrarRegistro(indice);
        }

        // BOTÓN 5: AGREGAR USUARIO.
        private void button5_Click(object sender, EventArgs e)
        {
            using (Form f = CrearFormularioUsuario("Agregar usuario"))
            {
                TextBox[] cajas = ObtenerCajas(f);

                Button guardar = new Button
                {
                    Text = "Guardar",
                    Left = 125,
                    Top = 380,
                    Width = 190
                };

                f.Controls.Add(guardar);

                guardar.Click += (s, ev) =>
                {
                    if (guardando || !ValidarDatos(cajas))
                        return;

                    try
                    {
                        guardando = true;
                        guardar.Enabled = false;

                        using (SqlConnection cn = new SqlConnection(conexion))
                        using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO usuario_usss36026 " +
                            "(usuario, clave, nombre, direccion, telefono) " +
                            "VALUES (@usuario, @clave, @nombre, @direccion, @telefono)", cn))
                        {
                            AgregarParametros(cmd, cajas);
                            cn.Open();

                            int filas = cmd.ExecuteNonQuery();

                            if (filas == 0)
                            {
                                MessageBox.Show("No se guardó ningún usuario.");
                                guardar.Enabled = true;
                                return;
                            }
                        }

                        MessageBox.Show("Usuario guardado correctamente.");
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar:\n" + ex.Message);
                        guardar.Enabled = true;
                    }
                    finally
                    {
                        guardando = false;
                    }
                };

                if (f.ShowDialog(this) == DialogResult.OK)
                    CargarDatos();
            }
        }

        // BOTÓN 6: MODIFICAR USUARIO.
        private void button6_Click(object sender, EventArgs e)
        {
            if (posicion < 0 || posicion >= tabla.Rows.Count)
            {
                MessageBox.Show("Selecciona un usuario.");
                return;
            }

            DataRow fila = tabla.Rows[posicion];
            int id = Convert.ToInt32(fila["idUsuario"]);

            using (Form f = CrearFormularioUsuario("Modificar usuario"))
            {
                TextBox[] cajas = ObtenerCajas(f);

                cajas[0].Text = Convert.ToString(fila["usuario"]);
                cajas[1].Text = Convert.ToString(fila["clave"]);
                cajas[2].Text = Convert.ToString(fila["clave"]); // Confirmar clave precargada
                cajas[3].Text = Convert.ToString(fila["nombre"]);
                cajas[4].Text = Convert.ToString(fila["direccion"]);
                cajas[5].Text = Convert.ToString(fila["telefono"]);

                Button guardar = new Button
                {
                    Text = "Guardar cambios",
                    Left = 125,
                    Top = 380,
                    Width = 190
                };

                f.Controls.Add(guardar);

                guardar.Click += (s, ev) =>
                {
                    if (guardando || !ValidarDatos(cajas))
                        return;

                    try
                    {
                        guardando = true;
                        guardar.Enabled = false;

                        using (SqlConnection cn = new SqlConnection(conexion))
                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE usuario_usss36026 SET " +
                            "usuario=@usuario, clave=@clave, nombre=@nombre, " +
                            "direccion=@direccion, telefono=@telefono " +
                            "WHERE idUsuario=@id", cn))
                        {
                            AgregarParametros(cmd, cajas);
                            cmd.Parameters.AddWithValue("@id", id);

                            cn.Open();

                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                MessageBox.Show("No se encontró el usuario.");
                                guardar.Enabled = true;
                                return;
                            }
                        }

                        MessageBox.Show("Usuario modificado correctamente.");
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar:\n" + ex.Message);
                        guardar.Enabled = true;
                    }
                    finally
                    {
                        guardando = false;
                    }
                };

                if (f.ShowDialog(this) == DialogResult.OK)
                    CargarDatos();
            }
        }

        // BOTÓN 7: ELIMINAR USUARIO.
        private void button7_Click(object sender, EventArgs e)
        {
            if (posicion < 0 || posicion >= tabla.Rows.Count)
            {
                MessageBox.Show("Selecciona un usuario.");
                return;
            }

            int id = Convert.ToInt32(tabla.Rows[posicion]["idUsuario"]);

            DialogResult respuesta = MessageBox.Show(
                "¿Seguro que deseas eliminar este usuario?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection cn = new SqlConnection(conexion))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM usuario_usss36026 WHERE idUsuario=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("No se encontró el usuario.");
                        return;
                    }
                }

                MessageBox.Show("Usuario eliminado correctamente.");
                CargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar:\n" + ex.Message);
            }
        }

        // CREAR VENTANA PARA AGREGAR O MODIFICAR.
        private Form CrearFormularioUsuario(string titulo)
        {
            Form f = new Form
            {
                Text = titulo,
                Width = 360,
                Height = 460,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            string[] nombres =
            {
                "Usuario:", "Clave:", "Confirmar Clave:", "Nombre:",
                "Dirección:", "Teléfono:"
            };

            for (int i = 0; i < nombres.Length; i++)
            {
                Label lbl = new Label
                {
                    Text = nombres[i],
                    Left = 20,
                    Top = 20 + i * 52,
                    Width = 100
                };

                TextBox txt = new TextBox
                {
                    Name = "caja" + i,
                    Left = 125,
                    Top = 17 + i * 52,
                    Width = 190
                };

                // Ocultar caracteres en Clave (caja1) y Confirmar Clave (caja2)
                if (i == 1 || i == 2)
                {
                    txt.PasswordChar = '*';
                }

                f.Controls.Add(lbl);
                f.Controls.Add(txt);
            }

            return f;
        }

        // OBTENER LAS CAJAS DE TEXTO.
        private TextBox[] ObtenerCajas(Form f)
        {
            TextBox[] cajas = new TextBox[6];

            for (int i = 0; i < cajas.Length; i++)
                cajas[i] = f.Controls["caja" + i] as TextBox;

            return cajas;
        }

        // VALIDAR INFORMACIÓN.
        private bool ValidarDatos(TextBox[] cajas)
        {
            string usuario = cajas[0].Text.Trim();
            string clave = cajas[1].Text;
            string confirmarClave = cajas[2].Text;
            string nombre = cajas[3].Text.Trim();
            string direccion = cajas[4].Text.Trim();
            string telefono = cajas[5].Text.Trim();

            if (usuario.Length < 6 || usuario.Length > 16)
            {
                MessageBox.Show("El usuario debe tener entre 6 y 16 caracteres.");
                return false;
            }

            if (!Regex.IsMatch(clave, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
            {
                MessageBox.Show("La clave debe tener al menos una letra y un número.");
                return false;
            }

            if (clave != confirmarClave)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                cajas[1].Clear();
                cajas[2].Clear();
                cajas[1].Focus();
                return false;
            }

            if (nombre == "" || direccion == "")
            {
                MessageBox.Show("Completa el nombre y la dirección.");
                return false;
            }

            if (!Regex.IsMatch(telefono, @"^\d{8}$"))
            {
                MessageBox.Show("El teléfono debe tener exactamente 8 dígitos.");
                return false;
            }

            return true;
        }

        // PARÁMETROS DE SQL (Mapeados correctamente con las 5 columnas de la BD).
        private void AgregarParametros(SqlCommand cmd, TextBox[] cajas)
        {
            cmd.Parameters.AddWithValue("@usuario", cajas[0].Text.Trim());
            cmd.Parameters.AddWithValue("@clave", cajas[1].Text);
            cmd.Parameters.AddWithValue("@nombre", cajas[3].Text.Trim());
            cmd.Parameters.AddWithValue("@direccion", cajas[4].Text.Trim());
            cmd.Parameters.AddWithValue("@telefono", cajas[5].Text.Trim());
        }

        // ACTUALIZAR POSICIÓN AL SELECCIONAR UNA FILA.
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (navegando || dataGridView1.CurrentRow == null)
                return;

            int indice = dataGridView1.CurrentRow.Index;

            if (indice >= 0 && indice < tabla.Rows.Count)
            {
                posicion = indice;
                ActualizarNavegacion();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();

            this.Hide();
            frm.ShowDialog();

            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}