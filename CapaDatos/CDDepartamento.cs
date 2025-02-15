﻿using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDatos
{
    public class CDDepartamento
    {
        string conexion = ConfigurationManager.AppSettings["conn"];
        

        public void PruebaConexion()
        {
            OracleConnection conn = new OracleConnection(conexion);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No conecta a la BD" + ex.Message);
                return;
            }
            conn.Close();
            MessageBox.Show("Conectado :)");
        }

        public void EliminarDepartamento(CEDepartamento departamento)
        {
            OracleConnection conn = new OracleConnection(conexion);

            try
            {
                DialogResult result;
                result = MessageBox.Show("¿Está seguro elimianr Depto?", "Eliminar Departamento", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    conn.Open();
                    OracleCommand command = new OracleCommand("DP_eliminarDepartamento", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("ID_DEPARTAMENTO", OracleType.Number).Value = Convert.ToInt32(departamento.idDepto);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Departamento eliminado :)");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error eliminar depto :("+ ex.Message);
            }
            conn.Close();
        }

        

        public List<CETipoDepartamento> TipoDepto()
        {
            OracleConnection conn = new OracleConnection(conexion);
            OracleDataReader mostrarTabla;
            List<CETipoDepartamento> ListaTipoDepto = new List<CETipoDepartamento>();

            try
            {
                conn.Open();
                OracleCommand command = new OracleCommand("SP_GET_TIPO_DEPARTAMENTOS", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("V_RESULT", OracleType.Cursor).Direction = ParameterDirection.Output;
                mostrarTabla = command.ExecuteReader();
                while (mostrarTabla.Read())
                {
                    ListaTipoDepto.Add(new CETipoDepartamento
                    {
                        idTipoDepartamento = Convert.ToInt32(mostrarTabla["IDTIPODEPARTAMENTO"]),
                        tDDepartamento = Convert.ToString(mostrarTabla["TD_DESCRIPCION"].ToString())

                    });
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("No conectado :((" + ex.Message);
            }
            conn.Close();
            return ListaTipoDepto;
        }


        public List<CESysEstadoDepto> SysEstDepto()
        {
            OracleConnection conn = new OracleConnection(conexion);
            OracleDataReader mostrarTabla;

            List<CESysEstadoDepto> listarEstadoDepto = new List<CESysEstadoDepto>();

            try
            {
                conn.Open();
                OracleCommand command = new OracleCommand("SP_GET_TIPO_ESTADO_DEPARTAMENTO", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("V_RESULT", OracleType.Cursor).Direction = ParameterDirection.Output;
                mostrarTabla = command.ExecuteReader();

                while (mostrarTabla.Read())
                {
                    listarEstadoDepto.Add(new CESysEstadoDepto
                    {
                        IDEstDepto = Convert.ToInt32(mostrarTabla["IDESTADODEPARTAMENTO"]),
                        EdDescripcion = Convert.ToString(mostrarTabla["ED_DESCRIPCION"].ToString())
                    });
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("No conectado :´(" + ex.Message);
            }
            conn.Close();
            return listarEstadoDepto;
        }

        public void CrearDepartamento(CEDepartamento depto)
        {
            OracleConnection conn = new OracleConnection(conexion);

            try
            {
                conn.Open();
                OracleCommand command = new OracleCommand("DP_agregarDepartamento", conn);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("VA_DE_DESCRIPCION", OracleType.NVarChar).Value = depto.descripcionDepto;
                command.Parameters.Add("VA_DE_PRECIO", OracleType.Number).Value = depto.precioDepto;
                command.Parameters.Add("VA_IDTIPODEPARTAMENTO", OracleType.Number).Value = depto.idTipoDepto;
                command.Parameters.Add("VA_IDESTADODEPARTAMENTO", OracleType.Number).Value = depto.idEstadoDepto;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                MessageBox.Show("No conectado" + ex.Message);
            }
            conn.Close();
            MessageBox.Show("Departamento Agregado");
            
        }







        public DataTable Listar()
        {
            OracleConnection conn = new OracleConnection(conexion);
            OracleDataReader mostrarTabla;
            DataTable tablaDepartamento = new DataTable();

            try
            {
                conn.Open();
                OracleCommand command = new OracleCommand("DP_seleccionarDepartamentos", conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
                mostrarTabla = command.ExecuteReader();
                mostrarTabla.Read();
                OracleDataAdapter adaptador = new OracleDataAdapter();
                adaptador.SelectCommand = command;
                adaptador.Fill(tablaDepartamento);
            }
            catch (Exception ex)
            {

                MessageBox.Show("No conectado :(" + ex.Message);
            }

            conn.Close();
            return tablaDepartamento;

            #region procedimiento almacenado listar departamentos
                /* --------------
                create or replace procedure DP_seleccionarDepartamentos(registros out SYS_REFCURSOR)
                as
                Begin
                    open registros for select* from DEPARTAMENTO;
                End;
                --------------*/
            #endregion
        }


    }
}
