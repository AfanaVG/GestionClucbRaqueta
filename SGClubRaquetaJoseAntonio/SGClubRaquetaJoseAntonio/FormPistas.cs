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
    public partial class FormPistas : Form
    {
        public FormPistas()
        {
            InitializeComponent();
        }

        private void pistasBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.pistasBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.raquetaDS);

        }

        private void pistasBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.pistasBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.raquetaDS);

        }

        private void FormPistas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'raquetaDS.pistas' Puede moverla o quitarla según sea necesario.
            this.pistasTableAdapter.Fill(this.raquetaDS.pistas);

        }

        private void btnNuevaFoto_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Imagenes|*.jpg;*.png";
            openFileDialog1.Title = "Abriendo imagen";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fotoPictureBox.Load(openFileDialog1.FileName);
            }

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            RaquetaDS ds = new RaquetaDS();
            RaquetaDSTableAdapters.reservasTableAdapter taReservas = new RaquetaDSTableAdapters.reservasTableAdapter();
            taReservas.FillByPista(ds.reservas,int.Parse(idPistaLabel1.Text));

            if (ds.reservas.Count > 0)
            {

                MessageBox.Show("No se puede eliminar la pista porque tiene una reserva");

                DialogResult dr2 = MessageBox.Show("¿Desea elimanar la pista y sus reservas?", "ELIMINACION",
                 MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr2 == DialogResult.OK)
                {
                    taReservas.DeleteByPista(int.Parse(idPistaLabel1.Text));
                    this.pistasTableAdapter.Delete(int.Parse(idPistaLabel1.Text));
                    MessageBox.Show("Socio eliminado");
                    this.pistasTableAdapter.Fill(this.raquetaDS.pistas);


                }

            }
            else
            {


                DialogResult dr = MessageBox.Show("¿Esta seguro de eliminar la pista?", "ELIMINACION",
                 MessageBoxButtons.OK, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {

                    this.pistasTableAdapter.Delete(int.Parse(idPistaLabel1.Text));
                    MessageBox.Show("Socio eliminado");
                    this.pistasTableAdapter.Fill(this.raquetaDS.pistas);


                }
            }
        }
    }
}
