﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaExtraescolares
{
    public partial class Administrador : Form
    {
        IComunicacion Delegado;
        Boolean Actividades_ConDatos = false;
        //Boolean Grupos_ConDatos = false;
        //Boolean Docentes_ConDatos = false;
       // Boolean Alumnos_ConDatos = false;
        Actividad[] Actividades;
        Int32 IndiceSeleccionado;

        public Administrador(IComunicacion Del)
        {
            InitializeComponent();
            Delegado = Del;
        }

        private void Button_Salir_Click(object sender, EventArgs e)
        {
            Delegado.Salir();
        }

        private void TabControl_Principal_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabControl_Principal.SelectedIndex)
            {
                case 1:
                    Cargar_Actividades();
                    break;
                case 2:
                    Cargar_Grupos();
                    break;
                case 3:
                    Cargar_Docentes();
                    break;
                case 4:
                    Cargar_Alumnos();
                    break;
            }
        }

        void Cargar_Actividades()
        {
            if (!Actividades_ConDatos)
            {
                Actividades = (new Actividad_Manejador()).GetList();
                var array_Act = from _Actividad in Actividades
                                orderby _Actividad.Nombre ascending
                                select _Actividad;
                Actividades = array_Act.ToArray();
            }
            ListBox_Actividades.Items.Clear();
            foreach (Actividad Act in Actividades)
            {
                ListBox_Actividades.Items.Add(Act);
            }
            Actividades_ConDatos = true;
        }

        void Cargar_Grupos()
        {

            Cargar_Actividades();
            comboBox_Actividades.Items.Clear();
            foreach (Actividad Act in Actividades)
            {
                comboBox_Actividades.Items.Add(Act);
            }
            
            comboBox_MesPrimero.SelectedIndex = comboBox_MesPrimero.FindStringExact("Enero");
            comboBox_MesUltimo.SelectedIndex = comboBox_MesPrimero.FindStringExact("Febrero");
            comboBox_Actividades.SelectedIndex = 0;
            
        }

        void Cargar_Docentes()
        {

        }

        void Cargar_Alumnos()
        {

        }

        private void TextBox_Buscar_TextChanged(object sender, EventArgs e)
        {
            String Texto = TextBox_Buscar.Text.ToLower();
            var array_Act = from _Actividad in Actividades
                            where (_Actividad.Nombre.ToLower()).StartsWith(Texto)
                            select _Actividad;
            ListBox_Actividades.Items.Clear();
            foreach (Actividad Act in array_Act)
            {
                ListBox_Actividades.Items.Add(Act);
            }
            if (array_Act.Count() != 0)
            {
                ListBox_Actividades.SelectedIndex = 0;
            }
        }

        private void ListBox_Actividades_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Int32 Index = ListBox_Actividades.IndexFromPoint(e.Location);
                if (Index != -1)
                {
                    IndiceSeleccionado = Index;
                    ListBox_Actividades.SelectedIndex = Index;
                    ListBox Lista = (ListBox)sender;
                    ContextMenuStrip_Actividad.Show(Lista.PointToScreen(e.Location));
                }
            }
        }

        private void ToolStripMenuItem_Actividad_Modificar_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Actividad_Borrar_Click(object sender, EventArgs e)
        {
            Int32 IDActividad = ((Actividad)ListBox_Actividades.Items[IndiceSeleccionado]).IDActividad;
            (new Actividad_Manejador()).Borrar(IDActividad);
            Actividades_ConDatos = false;
            Cargar_Actividades();
        }

        private void button_Agregar_Grupo_Click(object sender, EventArgs e)
        {

            Int32 Actividad_ID=((Actividad)comboBox_Actividades.SelectedItem).IDActividad;
            uC_Horario_Dia1.Obtener_Horario_Dia();
            String Horario=uC_Horario_Dia1.Horario;
            Int32 Ciclo_Escolar=Convert.ToInt32(numericUpDown_CicloEscolar_Anho.Value);
            Int32 Mes_Primero = Convert.ToInt32(comboBox_MesPrimero.SelectedValue)+1;
            Int32 Mes_Ultimo = Convert.ToInt32(comboBox_MesUltimo.SelectedValue)+1;
            Int32 Capacidad= Convert.ToInt32(numericUpDown_Capacidad_Grupo.Value);
            Grupo Nuevo_Grupo = new Grupo(Actividad_ID, Ciclo_Escolar, Mes_Primero, Mes_Ultimo, Capacidad, Horario);
            Grupo_Manejador Manejador = new Grupo_Manejador();
            
            Manejador.Agregar_Grupo(Nuevo_Grupo);
        }

    }
}
