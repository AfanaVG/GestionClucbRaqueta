using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGClubRaquetaJoseAntonio
{
    public partial class FormReservas : Form
    {

        private List<String> listaDNI = new List<String>();
        private List<int> listapistaID = new List<int>();

        public FormReservas()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormReservas_Load(object sender, EventArgs e)
        {
            cargarSocios();
            cargarPistas();

        }

        private void cargarSocios()
        {
            /**
            RaquetaDSTableAdapters.sociosTableAdapter tbSocios = new RaquetaDSTableAdapters.sociosTableAdapter();
            tbSocios.Fill(this.raquetaDS.socios);
            for (int i = 0; i < raquetaDS.socios.Count; i++)
            {
                cbSocio.Items.Add(raquetaDS.socios[i].apellidos+" "+
                    raquetaDS.socios[i].nombre);
                listaDNI.Add(raquetaDS.socios[i].DNI);
            }**/

            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {
                var qSocios = from c in objBD.socios
                              orderby c.apellidos, c.nombre ascending
                              select new { c.nombre, c.apellidos, c.DNI };
                var listaSoc = qSocios.ToList();
                if (listaSoc.Count > 0)
                {

                    for (int i = 0; i < listaSoc.Count; i++)
                    {
                        cbSocio.Items.Add(listaSoc[i].apellidos + ", " + listaSoc[i].nombre);
                        listaDNI.Add(listaSoc[i].DNI);
                    }


                }
            }
        }

        private void cbSocio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = cbSocio.SelectedIndex;


            /*
            RaquetaDSTableAdapters.sociosTableAdapter tbSocios = new RaquetaDSTableAdapters.sociosTableAdapter();
            if (cbSocio.SelectedIndex != -1)
            {

                panel2.Enabled = true;
                panel3.Enabled = true;
                tbSocios.FillByDNI(this.raquetaDS.socios, listaDNI[n]);
                for (int i = 0; i < raquetaDS.socios.Count; i++)
                {
                    txtApellidos.Text = raquetaDS.socios[i].apellidos;
                    txtNombre.Text = raquetaDS.socios[i].nombre;
                    txtEmail.Text = raquetaDS.socios[i].email;
                    txtDireccion.Text = raquetaDS.socios[i].domicilio;
                    txtCCC.Text = raquetaDS.socios[i].cuentaCorriente;
                    txtTelf.Text = raquetaDS.socios[i].telefono;
                    lblDNI.Text = raquetaDS.socios[i].DNI;
                }
                cargarDgvReservas();
            }*/


            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {
                if (cbSocio.SelectedIndex != -1)
                {
                    panel2.Enabled = true;
                    panel3.Enabled = true;
                    String dni = listaDNI[n];

                    var qSocios = from c in objBD.socios
                                  where c.DNI == dni
                                  select new { c.nombre, c.apellidos, c.email, c.telefono, c.DNI, c.cuentaCorriente, c.domicilio };
                    var listaSoc = qSocios.ToList();
                    if (listaSoc.Count > 0)
                    {

                        for (int i = 0; i < listaSoc.Count; i++)
                        {
                            txtApellidos.Text = listaSoc[i].apellidos;
                            txtNombre.Text = listaSoc[i].nombre;
                            txtEmail.Text = listaSoc[i].email;
                            txtDireccion.Text = listaSoc[i].domicilio;
                            txtCCC.Text = listaSoc[i].cuentaCorriente;
                            txtTelf.Text = listaSoc[i].telefono;
                            lblDNI.Text = listaSoc[i].DNI;
                        }
                    }
                    cargarDgvReservas();
                }

            }





        }

        private void cargarPistas()
        {
            cbPista.Items.Clear();

            /*
            RaquetaDSTableAdapters.pistasTableAdapter taPistas = new RaquetaDSTableAdapters.pistasTableAdapter();
            
            taPistas.Fill(raquetaDS.pistas);

            for (int i = 0; i < raquetaDS.pistas.Count; i++)
            {
                cbPista.Items.Add(raquetaDS.pistas[i].nombre);
                listapistaID.Add(raquetaDS.pistas[i].idPista);
            }*/



            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {
                var qPistas = from p in objBD.pistas
                              orderby p.nombre ascending
                              select new { p.nombre, p.idPista };
                var listaPist = qPistas.ToList();
                if (listaPist.Count > 0)
                {

                    for (int i = 0; i < listaPist.Count; i++)
                    {
                        cbPista.Items.Add(listaPist[i].nombre);
                        listapistaID.Add(listaPist[i].idPista);
                    }


                }
            }




        }

        private void cargarDgvReservas()
        {
            /**
            RaquetaDSTableAdapters.reservasTableAdapter taReservas = new RaquetaDSTableAdapters.reservasTableAdapter();
            taReservas.FillBySocio(this.raquetaDS.reservas, lblDNI.Text);

            dgvReserva.DataSource = raquetaDS.reservas;
            **/

            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {
                String dni = lblDNI.Text;

                var qReservas = from c in objBD.reservas
                                orderby c.fecha, c.hora ascending
                                where c.socio == dni
                                select new { c.fecha, c.hora, c.pista, c.pagado, Euros = c.cantidad };

                dgvReserva.DataSource = qReservas.ToList();
            }





        }

        private bool comparaHoras(TimeSpan horaBD, TimeSpan strHoraReserva)
        {
            bool boo_correcto;

            //TimeSpan horaReserva = DateTime.Parse(strHoraReserva).TimeOfDay;
            TimeSpan intervalo = strHoraReserva - horaBD;

            if ((Math.Abs(intervalo.Hours)) < 1)
            {
                boo_correcto = false;
            }
            else if ((Math.Abs(intervalo.Hours)) >= 2)
            {
                boo_correcto = true;
            }
            else if ((Math.Abs(intervalo.Minutes)) >= 30)
            {
                boo_correcto = true;
            }
            else
            {
                boo_correcto = false;
            }


            return boo_correcto;
        }

        private void cbPista_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = cbPista.SelectedIndex;
            btnReservar.Enabled = true;

            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {
                int id = listapistaID[n];

                var qPistas = from c in objBD.pistas
                              where c.idPista == id
                              select new { c.foto };

                var listaPist = qPistas.ToList();
                if (listaPist.Count > 0)
                {
                    for (int i = 0; i < listaPist.Count; i++)
                    {
                        byte[] image = listaPist[i].foto;
                        MemoryStream ms = new MemoryStream(image);
                        picPista.Image = Image.FromStream(ms);
                    }
                }
            }

            /*
            RaquetaDSTableAdapters.pistasTableAdapter taPistas = new RaquetaDSTableAdapters.pistasTableAdapter();
            taPistas.FillByID(this.raquetaDS.pistas, listapistaID[n]);

            for (int i = 0; i < raquetaDS.pistas.Count; i++)
            {
                byte[] image = raquetaDS.pistas[i].foto;
                MemoryStream ms = new MemoryStream(image);
                picPista.Image = Image.FromStream(ms);
            }*/





        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            string dni = lblDNI.Text;
            int n = cbPista.SelectedIndex;
            int pistaID = listapistaID[n];
            bool valido = true;

            using (clubraquetaEntities objBD = new clubraquetaEntities())
            {


                var qSocios = from c in objBD.reservas
                              where c.socio == dni && c.pagado.Equals("No")
                              select new { c.pagado };

                var qPistas = from c in objBD.reservas
                              where c.pista == pistaID
                              select new { c.hora, c.fecha };

                var listaSoc = qSocios.ToList();
                var listaPist = qPistas.ToList();



                if (listaSoc.Count > 0)
                {
                    MessageBox.Show("Tiene reservas sin pagar", "PAGO PENDIENTE",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    valido = false;

                }
                else
                {
                    if (listaPist.Count > 0)
                    {
                        for (int i = 0; i < listaPist.Count; i++)
                        {
                            if (listaPist[i].fecha == dateFecha.Value.Date && !comparaHoras(listaPist[0].hora, new TimeSpan((int)numHora.Value, (int)numMinutos.Value, 0)))
                            {

                                valido = false;
                                MessageBox.Show("La pista esta cogida ese dia a esa hora", "OCUPADA",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (valido)
                            {

                                addReserva();
                                valido = false;


                            }
                        }
                    }
                    else
                    {

                        addReserva();


                    }
                }
            }
        }

        private void addReserva()
        {

            DialogResult dr = MessageBox.Show("La pista esta libre ¿Seguro que desea realizar la reserva?", "RESERVAR",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string dni = lblDNI.Text;
                int n = cbPista.SelectedIndex;
                int pistaID = listapistaID[n];

                using (clubraquetaEntities objBD = new clubraquetaEntities())
                {
                    reservas objRes = new reservas();
                    objRes.fecha = dateFecha.Value.Date;
                    objRes.hora = new TimeSpan((int)numHora.Value, (int)numMinutos.Value, 0);
                    objRes.pista = pistaID;
                    objRes.socio = dni;
                    objRes.pagado = "No";
                    objRes.cantidad = 12;

                    objBD.reservas.Add(objRes);

                    objBD.SaveChanges();
                    MessageBox.Show("Reserva realizada  correctamente");
                    cargarDgvReservas();
                    dgvReserva.Refresh();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                string pagado = dgvReserva.SelectedRows[0].Cells[3].Value.ToString();

                if (pagado.Equals("No"))
                {
                    using (clubraquetaEntities objBD = new clubraquetaEntities())
                    {
                        var qReserva = from c in objBD.reservas
                                       where c.socio == lblDNI.Text && c.pagado.Equals("No")
                                       select new { c.idReserva };


                        var listReserca = qReserva.ToList();


                        reservas objCat = objBD.reservas.Find(listReserca[0].idReserva);


                        if (objCat != null)
                        {
                            objCat.pagado = "Si";

                            objBD.SaveChanges();
                            MessageBox.Show("Pago efectuado correctamente", "PAGO REALIZADO",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cargarDgvReservas();
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("La reserva ya esta pagada", "INFORMACION",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception)
            {

                MessageBox.Show("No hay una reserva seleccionada", "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
    }
}
