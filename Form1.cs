using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VS_ArbolBinarioBusqueda_ArtNav {
    public partial class Form1 : Form {
        ArbolBinarioDeBusqueda<FabricaArticulosNavidenios> miArbol = new ArbolBinarioDeBusqueda<FabricaArticulosNavidenios>(); 
        Random intRand = new Random();

        public Form1() {
            InitializeComponent();
            cboClasificacion.SelectedIndex = 0;
            dataGridView1.Columns.Add("Nombre", "Nombre");
            dataGridView1.Columns.Add("Tamaño del territorio", "Tamaño del territorio");
            dataGridView1.Columns.Add("Clasificacion", "Clasificacion");
            dataGridView1.Columns.Add("Fecha inauguracion", "Fecha inauguracion");
            dataGridView1.Columns.Add("Tiene credito", "Tiene credito");
            dataGridView1.Columns.Add("Cantidad empleados", "Cantidad empleados");
            dataGridView1.Columns.Add("Cantidad proveedores", "Cantidad proveedores");
            dataGridView1.Columns.Add("Tipo", "Tipo"); txtNombre.Focus();
        }

        private void btnAgregarFabrica_Click(object sender, EventArgs e) {
            FabricaArticulosNavidenios miFabrica = new FabricaArticulosNavidenios();
            try {
                miFabrica.Nombre = txtNombre.Text;
                miFabrica.TamanioTerritorio = int.Parse(txtTamanio.Text);
                miFabrica.FechaInauguracion = dtmFechaInauguracion.Value;
                miFabrica.TieneCredito = chkTieneCredito.Checked;
                miFabrica.CantidadEmpleados = int.Parse(txtCantidadEmpleados.Text);
                miFabrica.CantidadProveedores = int.Parse(txtCantidadProveedores.Text);
                miFabrica.ModificarImagen(txtImagen.Text);

                switch (cboClasificacion.SelectedIndex) {
                    case 0: miFabrica.Clasificacion = 'C'; break;
                    case 1: miFabrica.Clasificacion = 'B'; break;
                    case 2: miFabrica.Clasificacion = 'A'; break;
                    case 3: miFabrica.Clasificacion = 'S'; break;
                    default: miFabrica.Clasificacion = 'C'; break;
                }

                if (radPrivada.Checked) miFabrica.TipoSegunDependencia = "Privada";
                else if (radPublica.Checked) miFabrica.TipoSegunDependencia = "Publica";
                else miFabrica.TipoSegunDependencia = "Mixta";
            } catch (Exception ex) {
                MessageBox.Show("Favor de verificar los datos de la fabrica:\n" + ex.Message); return;
            }

            if (miFabrica.Nombre == "") { MessageBox.Show("Favor de agregar un nombre a la fabrica"); txtNombre.Focus(); return; }

            MessageBox.Show(miArbol.Insertar(miFabrica) ? "Se agrego la nueva fabrica." : "No se agrego la fabrica.");

            Mostrar(); Acciones();
        }

        private void Mostrar() {
            dataGridView1.Rows.Clear(); bool a = true;

            if (radPostOrden.Checked) {
                foreach (FabricaArticulosNavidenios f in miArbol.RecorrerPostOrden()) {
                    if (a) { try { pictureBox1.Image = Image.FromFile(f.ObtenerImagen()); a = false; } catch { pictureBox1.Image = Properties.Resources.FabricaIcono; } }
                    dataGridView1.Rows.Add(f.Nombre, f.TamanioTerritorio, f.Clasificacion, f.FechaInauguracion, f.TieneCredito ? "Si" : "No",
                        f.CantidadEmpleados, f.CantidadProveedores, f.TipoSegunDependencia);
                }
            } else if (radInOrden.Checked) {
                foreach (FabricaArticulosNavidenios f in miArbol.RecorrerInOrden()) {
                    if (a) { try { pictureBox1.Image = Image.FromFile(f.ObtenerImagen()); a = false; } catch { pictureBox1.Image = Properties.Resources.FabricaIcono; } }
                    dataGridView1.Rows.Add(f.Nombre, f.TamanioTerritorio, f.Clasificacion, f.FechaInauguracion, f.TieneCredito ? "Si" : "No",
                        f.CantidadEmpleados, f.CantidadProveedores, f.TipoSegunDependencia);
                }
            } else {
                foreach (FabricaArticulosNavidenios f in miArbol.RecorrerPreOrden()) {
                    if (a) { try { pictureBox1.Image = Image.FromFile(f.ObtenerImagen()); a = false; } catch { pictureBox1.Image = Properties.Resources.FabricaIcono; } }
                    dataGridView1.Rows.Add(f.Nombre, f.TamanioTerritorio, f.Clasificacion, f.FechaInauguracion, f.TieneCredito ? "Si" : "No",
                        f.CantidadEmpleados, f.CantidadProveedores, f.TipoSegunDependencia);
                }
            }
        }

        private void Acciones() {
            foreach (Control c in groupBox1.Controls) {
                if (c is TextBox)
                    c.Text = "";
            }

            txtNombre.Focus();
        }

        private void btnExaminar_Click(object sender, EventArgs e) {
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = "C://";
            op.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (op.ShowDialog() == DialogResult.OK) {
                pictureBox1.ImageLocation = op.FileName;
                txtImagen.Text = op.FileName;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (dataGridView1.CurrentRow == null) return;

            int x = dataGridView1.CurrentRow.Index; FabricaArticulosNavidenios fabricaAux = null;
            string selected = dataGridView1.Rows[x].Cells[0].Value.ToString();

            foreach (FabricaArticulosNavidenios f in miArbol.RecorrerInOrden()) {
                if (selected == f.Nombre) {
                    fabricaAux = f; break;
                }
            }

            if (fabricaAux != null) {
                string strAux = fabricaAux.ObtenerImagen();
                if (strAux == "")
                    pictureBox1.Image = Properties.Resources.FabricaIcono;
                else
                    pictureBox1.ImageLocation = strAux;

                txtNombre.Text = fabricaAux.Nombre;
                txtTamanio.Text = fabricaAux.TamanioTerritorio.ToString();
                switch (fabricaAux.Clasificacion) {
                    case 'C': cboClasificacion.Text = "C-Micro"; break;
                    case 'B': cboClasificacion.Text = "B-Pequeña"; break;
                    case 'A': cboClasificacion.Text = "A-Mediana"; break;
                    case 'S': cboClasificacion.Text = "S-Grande"; break;
                }
                dtmFechaInauguracion.Value = fabricaAux.FechaInauguracion;
                txtCantidadEmpleados.Text = fabricaAux.CantidadEmpleados.ToString();
                txtCantidadProveedores.Text = fabricaAux.CantidadProveedores.ToString();
                txtImagen.Text = fabricaAux.ObtenerImagen();
                if (fabricaAux.TipoSegunDependencia == "Privada") radPrivada.Checked = true;
                else if (fabricaAux.TipoSegunDependencia == "Publica") radPublica.Checked = true;
                else radMixta.Checked = true;
                chkTieneCredito.Checked = fabricaAux.TieneCredito;
            } else {
                MessageBox.Show("Error");
            }
        }

        private void btnEliminarTodo_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Esta seguro que desea eliminar toda la lista?", "Confirmacion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                MessageBox.Show("Se eliminaron todas las fabricas.");
                miArbol.Vaciar();
            } else {
                MessageBox.Show("No se elimino la lista");
            }
            Mostrar(); Acciones();
        }

        private void btnEliminarUltimo_Click(object sender, EventArgs e) {
            /*     
            if (MessageBox.Show("Esta seguro que desea eliminar la ultima fabrica agregada?", "Confirmacion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                FabricaArticulosNavidenios fabricaAux = miArbol.EliminarNodo();
                if (fabricaAux == null) { MessageBox.Show("No hay fabricas por eliminar"); return; }
                string aux = fabricaAux.TieneCredito?"Si":"No";

                MessageBox.Show("Se elimino la fabrica con datos:\n"
                + "Nombre: " + fabricaAux.Nombre + "\n"
                + "Tamaño del territorio: " + fabricaAux.TamanioTerritorio + "metros cuadrados" + "\n"
                + "Clasificacion: " + fabricaAux.Clasificacion + "\n"
                + "Fecha inauguracion: " + fabricaAux.FechaInauguracion + "\n"
                + "Tiene credito: " + aux + "\n"
                + "Cantidad empleados: " + fabricaAux.CantidadEmpleados + "\n"
                + "Cantidad proveedores: " + fabricaAux.CantidadProveedores + "\n"
                + "Tipo segun su dependencia: "+fabricaAux.TipoSegunDependencia + "\n");
            } else {
                MessageBox.Show("No se elimino la fabrica.");
            }

            Mostrar();Acciones();*/
        }

        private void btnLimpiar_Click(object sender, EventArgs e) {
            Acciones();
        }

        private void btnBuscar_Click(object sender, EventArgs e) {
            if (txtNombre.Text == "") { MessageBox.Show("Rellenar campo del nombre"); return; }

            FabricaArticulosNavidenios fabricaAux = new FabricaArticulosNavidenios() { Nombre = txtNombre.Text };

            fabricaAux = miArbol.Buscar(fabricaAux);

            if (fabricaAux != null) {
                string aux = fabricaAux.TieneCredito ? "Si" : "No", strFabrica = "\n"
                + "Nombre: " + fabricaAux.Nombre + "\n"
                + "Tamaño del territorio: " + fabricaAux.TamanioTerritorio + " metros cuadrados" + "\n"
                + "Clasificacion: " + fabricaAux.Clasificacion + "\n"
                + "Fecha inauguracion: " + fabricaAux.FechaInauguracion + "\n"
                + "Tiene credito: " + aux + "\n"
                + "Cantidad empleados: " + fabricaAux.CantidadEmpleados + "\n"
                + "Cantidad proveedores: " + fabricaAux.CantidadProveedores + "\n"
                + "Tipo segun su dependencia: " + fabricaAux.TipoSegunDependencia + "\n";

                MessageBox.Show("Se encontro la fabrica con los datos:" + strFabrica);
            } else
                MessageBox.Show("No se encontro la fabrica");

            txtNombre.Focus();
        }

        private void btnEliminarSeleccionado_Click_1(object sender, EventArgs e) {
            if (dataGridView1.CurrentRow == null) { MessageBox.Show("Favor de seleccionar un elemento."); return; }

            FabricaArticulosNavidenios fabricaAux = new FabricaArticulosNavidenios();
            fabricaAux.Nombre = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();

            if (MessageBox.Show("Esta seguro que desea eliminar la fabrica:\n" + fabricaAux.Nombre, "Confirmacion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                fabricaAux = miArbol.Eliminar(fabricaAux);
                string aux = fabricaAux.TieneCredito ? "Si" : "No";

                MessageBox.Show("Se elimino la fabrica con datos:\n"
                + "Nombre: " + fabricaAux.Nombre + "\n"
                + "Tamaño del territorio: " + fabricaAux.TamanioTerritorio + "metros cuadrados" + "\n"
                + "Clasificacion: " + fabricaAux.Clasificacion + "\n"
                + "Fecha inauguracion: " + fabricaAux.FechaInauguracion + "\n"
                + "Tiene credito: " + aux + "\n"
                + "Cantidad empleados: " + fabricaAux.CantidadEmpleados + "\n"
                + "Cantidad proveedores: " + fabricaAux.CantidadProveedores + "\n"
                + "Tipo segun su dependencia: " + fabricaAux.TipoSegunDependencia + "\n");
            } else {
                MessageBox.Show("No se elimino la fabrica.");
            }

            Mostrar(); Acciones(); 
        }

        private void btnGenerarAleatorio_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Se eliminara cualquier elemento generado anteriormente, desea continuar?", "Confirmacion", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                FabricaArticulosNavidenios miFabrica; miArbol.Vaciar();
                char[] chrClasificaciones = { 'C', 'B', 'A', 'S' };
                string[] strDependencias = { "Privada", "Publica", "Mixta" };

                for (int x = 0; x < 10; x++) {
                    miFabrica = new FabricaArticulosNavidenios();

                    miFabrica.Nombre = GenerarNombreAleatorio();
                    miFabrica.TamanioTerritorio = intRand.Next(10, 1000);
                    miFabrica.FechaInauguracion = GenerarFechaAleatoria();
                    miFabrica.TieneCredito = intRand.Next(0, 2) == 1 ? true : false;
                    miFabrica.CantidadEmpleados = intRand.Next(100, 100000);
                    miFabrica.CantidadProveedores = intRand.Next(1, 100);
                    miFabrica.ModificarImagen("");
                    miFabrica.Clasificacion = chrClasificaciones[intRand.Next(0, 4)];
                    miFabrica.TipoSegunDependencia = strDependencias[intRand.Next(0, 3)];

                    miArbol.Insertar(miFabrica);
                }

                MessageBox.Show("Se generaron 10 fabricas con datos aleatorios.");
            } else {
                MessageBox.Show("No se generaron fabricas aleatorias.");
            }
            Mostrar(); Acciones();
        }

        private string GenerarNombreAleatorio() {
            string strPosibles = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", strNombre="FAB-";

            int intAux = intRand.Next(0, 100);
            strNombre += intAux < 10 ? "0" + intAux.ToString() : intAux.ToString(); strNombre += "-";

            for(int x = 0; x < 4; x++) 
                strNombre += strPosibles[intRand.Next(0, 26)];

            return strNombre;
        }

        private DateTime GenerarFechaAleatoria() {
            DateTime dtmInicio = new DateTime(1900, 1, 1);
            int intRango = (DateTime.Today - dtmInicio).Days;
            return dtmInicio.AddDays(intRand.Next(intRango));
        }

        private void picArbol_Click(object sender, EventArgs e) {
            if (picArbol.Image != null) {
                CrearFormaConFigura(picArbol.Image);
            }
        }

        private void btnDibujar_Click(object sender, EventArgs e) {
            if (miArbol.Vacio) {
                MessageBox.Show("No hay ninguna fabrica en estos momentos"); return;
            }

            Dibujar();
        }

        private void Dibujar() {
            GenerarCodigoFigura();

            try {
                System.Diagnostics.Process.Start(RutasDeAcceso.Bat);

                Thread.Sleep(600);

                System.IO.FileStream miArchivo = new System.IO.FileStream(RutasDeAcceso.FiguraJpg, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                picArbol.Image = Image.FromStream(miArchivo); miArchivo.Close();
            } catch (Exception ex) {
                MessageBox.Show("Error bat: \n" + ex.Message);
            }
            picArbol.Refresh();
        }

        private void CrearFormaConFigura(Image img) {
            Form frmVentana = new Form();
            frmVentana.Width = 1400; frmVentana.Height = 1000;
            frmVentana.AutoScroll = true; frmVentana.Text = "Figura";
            frmVentana.StartPosition = FormStartPosition.CenterScreen;
            frmVentana.Show();

            PictureBox picABB = new PictureBox();
            picABB.Parent = frmVentana; picABB.Width = 990; picABB.Height = 565;
            picABB.SizeMode = PictureBoxSizeMode.Zoom;

            picABB.Image = img;
            picABB.Refresh();
        }

        private void GenerarCodigoFigura() {
            string strCodigoEnDOT = miArbol.GenerarCodigoDOT();

            try {
                System.IO.StreamWriter miArchivo = new System.IO.StreamWriter(RutasDeAcceso.FiguraSinExtension);
                miArchivo.WriteLine(strCodigoEnDOT);
                miArchivo.Close();
            } catch (Exception ex) {
                MessageBox.Show("Error: \n" + ex.Message);
            }
        }

        private void radPreOrden_CheckedChanged(object sender, EventArgs e) {
            Mostrar();
        }

        private void radInOrden_CheckedChanged(object sender, EventArgs e) {
            Mostrar();
        }

        private void radPostOrden_CheckedChanged(object sender, EventArgs e) {
            Mostrar();
        }
        private void btnActualizar_Click(object sender, EventArgs e) {
            Mostrar(); Dibujar();
        }

        private void btnCambiar_Click(object sender, EventArgs e) {
            Hide();
            Form2 f = new Form2();
            f.ShowDialog();
        }
    }
}
