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
using System.Data.Sql;
using REGISTROALUMNOS.Entidades;

namespace REGISTROALUMNOS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conexion = new SqlConnection();
        SqlCommand comando = new SqlCommand();
        SqlCommand comando2 = new SqlCommand();
        SqlCommand comando3 = new SqlCommand();
        DataTable tabla;
        SqlDataAdapter ad = new SqlDataAdapter();


        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox7.Visible = false;
            groupBox1.Visible = false;
            Btmodificar.Visible = false;
            label9.Visible = false;

            groupBox6.Visible = false;
            dataGridView1.Columns[0].Visible = false;
            groupBox8.Visible = false;
            radmasculino.Checked = true;
            #region CONEXION
            try
            {

                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";
                conexion.Open();
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            #endregion CONEXION
            Combosencar();
            Combosparient();


        }

        public void MostrarDatos()
        {
            try
            {
                conexion.Open();
                SqlDataAdapter ad = new SqlDataAdapter("select*from ALUMNOS", conexion);
                tabla = new DataTable();
                ad.Fill(tabla);
                dataGridView1.DataSource = tabla;
                conexion.Close();
                MessageBox.Show(Convert.ToString(tabla.Rows.Count));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
        public void Eliminar(int codigo)
        {
            try
            {
                SqlConnection conexion = new SqlConnection();
                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";

                conexion.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM ALUMNOS WHERE IdAlumno =" + codigo + "", conexion);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dato Eliminado Correctamente");
                conexion.Close();

            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }




        }

        public void cargardatos()
        {


           


        }




        #region funciones
        public void Nuevo()
        {
            txtnombre1.Text = "";
            txtnombre2.Text = "";
            txtapellido1.Text = "";
            txtapellido2.Text = "";
            dateNacimiento.Text = "";
            txtencargado.Text = "";
            compariente.Text = "";
            comEncargado.Text = "";
            radmasculino.Checked = false;
            radFemenino.Checked = false;
            BtGuardar.Visible = true;

        }
        public void Buscar()
        {
            try
            {
                SqlConnection conexion = new SqlConnection();
                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";
                conexion.Open();
                if (radApellido.Checked == true)
                {
                    SqlDataAdapter ad = new SqlDataAdapter("select * from ALUMNOS where PrimerApellido like'%" + txtbuscar.Text + "%'", conexion);
                    tabla = new DataTable();
                    ad.Fill(tabla);
                    dataGridView1.DataSource = tabla;

                }
                else if (radNombre.Checked == true)
                {
                    SqlDataAdapter ad = new SqlDataAdapter("select * from ALUMNOS where PrimerNombre like '" + txtbuscar.Text + "%'", conexion);
                    tabla = new DataTable();
                    ad.Fill(tabla);
                    dataGridView1.DataSource = tabla;

                }
                else
                { MessageBox.Show("No se a Seleccionado ninguna opcion,MessageBoxButtons.OK,MessageBoxIcon.Error"); }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            conexion.Close();

        }
        public void Modificar()
        {
            try
            {
                SqlConnection conexion = new SqlConnection();
                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";
                conexion.Open();
                groupBox3.Visible = false;
                int genero;
                if (radmasculino.Checked == true)
                {
                    genero = 1;
                }
                else if (radFemenino.Checked == true)
                {
                    genero = 2;
                }
                else
                {
                    genero = 0;
                    MessageBox.Show("Seleccionar el Genero");
                    return;
                }



                SqlCommand comd = new SqlCommand("ActualizarAlumno", conexion);
                comd.CommandType = CommandType.StoredProcedure;
                comd.Parameters.AddWithValue("@IdAlumno", Convert.ToInt32(lblCodigoAlumno.Text));
                comd.Parameters.AddWithValue("@PrimerNombre", txtnombre1.Text);
                comd.Parameters.AddWithValue("@SegundoNombre", txtnombre2.Text);
                comd.Parameters.AddWithValue("@PrimerApellido", txtapellido1.Text);
                comd.Parameters.AddWithValue("@SegundoApellido", txtapellido2.Text);
                comd.Parameters.AddWithValue("@FechaNacimiento", dateNacimiento.Value);
                comd.Parameters.AddWithValue("@CodigoEnacargado", comEncargado.SelectedValue);
                comd.Parameters.AddWithValue("@CodigoGenero", genero);
                comd.Parameters.AddWithValue("@CodigoPariente", compariente.SelectedValue);
                int resultado = Convert.ToInt32(comd.ExecuteNonQuery());
                if (resultado > 0)
                {
                    MessageBox.Show("Datos Actualizados Exitosamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MostrarDatos();
                    conexion.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        public void Combosencar()
        {


            conexion.Open();
            SqlDataAdapter ad = new SqlDataAdapter("select CodigoEncargado, NomEncargado from ENCARGADO", conexion);
            tabla = new DataTable();
            ad.Fill(tabla);


            comEncargado.DataSource = null;
            List<Encargado> listadoEncargado = new List<Encargado>();

            listadoEncargado = (from DataRow row in tabla.Rows

                                select new Encargado
                                {
                                    CodigoEncargado = Int32.Parse(row["CodigoEncargado"].ToString()),
                                    NombreEncargado = row["NomEncargado"].ToString(),

                                }).ToList();



            comEncargado.DataSource = listadoEncargado;
            comEncargado.DisplayMember = "NombreEncargado";
            comEncargado.ValueMember = "CodigoEncargado";

            conexion.Close();

        }
        public void Combosparient()
        {


            conexion.Open();
            SqlDataAdapter ad = new SqlDataAdapter("select CodigoPariente, NomPariente from PARIENTE", conexion);
            tabla = new DataTable();
            ad.Fill(tabla);

            compariente.DataSource = null;
            List<Pariente> listadoParientes = new List<Pariente>();

            listadoParientes = (from DataRow row in tabla.Rows

                                select new Pariente
                                {
                                    CodigoPariente = Int32.Parse(row["CodigoPariente"].ToString()),
                                    NombrePariente = row["NomPariente"].ToString()

                                }).ToList();



            if (listadoParientes.Count > 0)
            {
                compariente.DataSource = listadoParientes;
                compariente.DisplayMember = "NombrePariente";
                compariente.ValueMember = "CodigoPariente";
            }

            conexion.Close();

        }
        public void CREARPARIENTE()
        {
            if (txtpariente.Text == "")
            {
                MessageBox.Show("Ingrese nombre de encargado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                conexion.Open();
                comando3 = new SqlCommand("INSERT INTO PARIENTE(NomPariente) VALUES('" + txtpariente.Text + "');", conexion);
                groupBox5.Visible = true;
                groupBox6.Visible = false;
                compariente.Items.Add(txtpariente.Text);
                comando3.ExecuteNonQuery();
                conexion.Close();
                Combosparient();


            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }



        }
        public void CREARENCARGADO()
        {
            if (txtencargado.Text == "")
            {
                MessageBox.Show("Ingrese nombre de encargado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";
                conexion.Open();

                groupBox7.Visible = false;

                comando2 = new SqlCommand("INSERT INTO ENCARGADO(NomEncargado) VALUES('" + txtencargado.Text + "');", conexion);

                comEncargado.Items.Add(txtencargado.Text);
                comando2.ExecuteNonQuery();

                groupBox8.Visible = false;
                groupBox4.Visible = true;
                conexion.Close();

                Combosencar();


            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }



        }
        public void HISTORIAL()
        {
            groupBox7.Visible = true;
            MostrarDatos();
      

        }
        private void rEGISTROToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compariente.Text = "";
            comEncargado.Text = "";
            groupBox1.Visible = true;

        }
        public void enabled()
        {
            txtnombre1.Enabled = false;
            txtnombre2.Enabled = false;
            txtapellido1.Enabled = false;
            txtapellido2.Enabled = false;
            dateNacimiento.Enabled = false;
            compariente.Enabled = false;
            comEncargado.Enabled = false;
            BtGuardar.Enabled = false;

        }
        public void desenabled()
        {
            txtnombre1.Enabled = true;
            txtnombre2.Enabled = true;
            txtapellido1.Enabled = true;
            txtapellido2.Enabled = true;
            dateNacimiento.Enabled = true;
            compariente.Enabled = true;
            comEncargado.Enabled = true;
            radmasculino.Checked = true;
            

        }
        public void INSERTAR()
        {
            try
            {
                conexion.ConnectionString = @"Persist Security Info=False;User ID=practica;Password=practica;Initial Catalog=bdalumnos;Server=PC19\SQLEXPRESS";
                conexion.Open();
                int genero;
                if (radmasculino.Checked == true)
                {
                    genero = 1;

                }
                else if (radFemenino.Checked == true)
                {
                    genero = 2;

                }
                else
                {
                    genero = 0;
                    MessageBox.Show("Seleccionar el Genero");
                    return;
                }



                SqlCommand comando = new SqlCommand("InsertarAlumnos", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@PrimerNombre", txtnombre1.Text);
                comando.Parameters.AddWithValue("@SegundoNombre", txtnombre2.Text);
                comando.Parameters.AddWithValue("@PrimerApellido", txtapellido1.Text);
                comando.Parameters.AddWithValue("@SegundoApellido", txtapellido2.Text);
                comando.Parameters.AddWithValue("@FechaNacimiento", dateNacimiento.Value);
                comando.Parameters.AddWithValue("@CodigoEnacargado", comEncargado.SelectedValue);
                comando.Parameters.AddWithValue("@CodigoGenero", genero);
                comando.Parameters.AddWithValue("@CodigoPariente", compariente.SelectedValue);
                int resultado = Convert.ToInt32(comando.ExecuteNonQuery());
                if (resultado > 0)
                {
                    MessageBox.Show("Exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                conexion.Close();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        #endregion funciones
        #region botones
        private void BtNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
            desenabled();
        }

        private void Bthistorial_Click(object sender, EventArgs e)
        {
            Bteliminar.Visible = true;
            txtbuscar.Enabled = false;
            Btbuscar.Visible = false;
            radNombre.Visible = false;
            radApellido.Visible = false;
           
            HISTORIAL();
        }

        private void Btpariente_Click(object sender, EventArgs e)
        {
            txtpariente.Focus();
            CREARPARIENTE();

        }

        private void Btmodificar_Click(object sender, EventArgs e)
        {
            BtNuevo.Visible = false;
            Modificar();
        }

        private void Btbuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Bteliminar_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult opcion;
                opcion = MessageBox.Show("Realmente desea eliminar los registros?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    string codigo;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            codigo = Convert.ToString(row.Cells[1].Value);
                            Eliminar(Convert.ToInt32(codigo));
                        }
                    }
                    MostrarDatos();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
            //Eliminar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtencargado.Focus();
            CREARENCARGADO();
        }

        private void BTencargado_Click(object sender, EventArgs e)
        {
            groupBox8.Visible = true;

        }

        private void BtGuardar_Click(object sender, EventArgs e)
        {
            INSERTAR();
            Nuevo();
            enabled();

        }

        private void Btnuevopariente_Click(object sender, EventArgs e)
        {
            groupBox6.Visible = true;
            groupBox5.Visible = false;
        }

        private void BtCancelencar_Click(object sender, EventArgs e)
        {
            txtencargado.Clear();
            groupBox8.Visible = false;
            groupBox4.Visible = true;
        }

        private void BtCancelParien_Click(object sender, EventArgs e)
        {
            txtpariente.Text = "";
            groupBox6.Visible = false;
            groupBox5.Visible = true;
        }


        private void Btbusqueda_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Seleccion una opcion de busqueda");
            radApellido.Visible = true;
            radApellido.Checked = true;
            radNombre.Visible = true;

            Bteliminar.Enabled = true;
            Btbuscar.Visible = true;
            Btbusqueda.Visible = false;
            txtbuscar.Enabled = true;

        }

        private void radApellido_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void radNombre_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void BtEliminado_Click(object sender, EventArgs e)
        {

        }

        private void txtbuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            Buscar();

        }
        #endregion botones

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            desenabled();
            BtGuardar.Visible = false;
            label9.Visible = true;
            Btmodificar.Visible = true;
            groupBox1.Visible = true;

            lblCodigoAlumno.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["idAlumno"].Value);
            txtnombre1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["PrimerNombre"].Value);
            txtnombre2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["SegundoNombre"].Value);
            txtapellido1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["PrimerApellido"].Value);
            txtapellido2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["SegundoApellido"].Value);
            dateNacimiento.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["FechaNacimiento"].Value);
            int codigoGenero = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CodigoGenero"].Value);
            if (codigoGenero == 1)
            {
                radmasculino.Checked = true;
            }
            else
            {
                radFemenino.Checked = true;
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radApellido_CheckedChanged(object sender, EventArgs e)
        {
            if (radApellido.Checked == true)
            {
                MessageBox.Show("INGRESE DATO");
                txtbuscar.Enabled = false;

            }
            MessageBox.Show("INGRESE DATO");
            txtbuscar.Enabled = false;
        }

        private void radNombre_CheckedChanged(object sender, EventArgs e)
        {
            if (radNombre.Checked == true)
            {
                MessageBox.Show("INGRESE DATO");
                txtbuscar.Enabled = false;

            }
        }

        private void iNICIOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chkEliminar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEliminar.Checked)
            {
                dataGridView1.Columns[0].Visible = true;
            }
            else
            {
                dataGridView1.Columns[0].Visible = false;
            }
        }
    }


}
