using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGClubRaquetaJoseAntonio
{
    public partial class FormSocios : Form
    {
        public FormSocios()
        {
            InitializeComponent();
        }

        private void sociosBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.sociosBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.raquetaDS);
            DialogResult dr2 = MessageBox.Show("Datos del socio guardados", "Guardado",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void FormSocios_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'raquetaDS.socios' Puede moverla o quitarla según sea necesario.
            this.sociosTableAdapter.Fill(this.raquetaDS.socios);

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            RaquetaDS ds = new RaquetaDS();
            RaquetaDSTableAdapters.reservasTableAdapter taReservas = new RaquetaDSTableAdapters.reservasTableAdapter();
            taReservas.FillBySocio(ds.reservas, dNITextBox.Text);

            if (ds.reservas.Count > 0)
            {

                MessageBox.Show("No se puede eliminar al socio porque tiene una reserva");

                DialogResult dr2 = MessageBox.Show("¿Desea elimanar al socio y sus reservas?", "ELIMINACION",
                 MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr2 == DialogResult.OK)
                {
                    taReservas.Delete(dNITextBox.Text);
                    this.sociosTableAdapter.Delete(dNITextBox.Text);
                    MessageBox.Show("Socio eliminado");
                    this.sociosTableAdapter.Fill(this.raquetaDS.socios);


                }

            }
            else
            {


                DialogResult dr = MessageBox.Show("¿Esta seguro de eliminar el socio?", "ELIMINACION",
                 MessageBoxButtons.OK, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {

                    this.sociosTableAdapter.Delete(dNITextBox.Text);
                    MessageBox.Show("Socio eliminado");
                    this.sociosTableAdapter.Fill(this.raquetaDS.socios);


                }
            }
        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {

                var qSocios = from c in objBD.socios
                              orderby c.apellidos ascending
                              orderby c.nombre ascending                         
                              select c;

                var listaSoc = qSocios.ToList();

                if (listaSoc.Count > 0)
                {
                    dataGridView1.DataSource = listaSoc;
                    dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count-1);
                    dataGridView1.Refresh();

                }
                


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
