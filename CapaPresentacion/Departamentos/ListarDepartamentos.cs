﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;

namespace CapaPresentacion.Departamentos
{
    public partial class ListarDepartamentos : Form
    {


        public ListarDepartamentos()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            
            try
            {
                CNDepartamento lista = new CNDepartamento();
                dataGridViewDepartamentos.DataSource = lista.ObtenerDatos();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
