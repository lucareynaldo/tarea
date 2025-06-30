using System;
using System.Data;
using System.Windows.Forms;

public partial class FormNotas : Form
{
    private GestorBaseDatos gestor;
    private DataGridView dgvNotas;
    private ComboBox cmbEstudiantes, cmbMaterias, cmbTipoEvaluacion;
    private TextBox txtCalificacion;
    private DateTimePicker dtpFechaEvaluacion;
    private Button btnAgregar, btnModificar, btnEliminar, btnLimpiar, btnCerrar;
    private int notaSeleccionadaId = -1;

    public FormNotas()
    {
        InitializeComponent();
        gestor = new GestorBaseDatos();
        ConfigurarFormulario();
        CargarDatos();
    }

    private void ConfigurarFormulario()
    {
        this.Text = "Gestión de Notas";
        this.Size = new System.Drawing.Size(900, 550);
        this.StartPosition = FormStartPosition.CenterScreen;

        dgvNotas = new DataGridView();
        dgvNotas.Location = new System.Drawing.Point(20, 20);
        dgvNotas.Size = new System.Drawing.Size(600, 350);
        dgvNotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvNotas.MultiSelect = false;
        dgvNotas.SelectionChanged += DgvNotas_SelectionChanged;

        Label lblEstudiante = new Label();
        lblEstudiante.Text = "Estudiante:";
        lblEstudiante.Location = new System.Drawing.Point(650, 50);

        cmbEstudiantes = new ComboBox();
        cmbEstudiantes.Location = new System.Drawing.Point(650, 70);
        cmbEstudiantes.Size = new System.Drawing.Size(200, 20);
        cmbEstudiantes.DropDownStyle = ComboBoxStyle.DropDownList;

        Label lblMateria = new Label();
        lblMateria.Text = "Materia:";
        lblMateria.Location = new System.Drawing.Point(650, 100);

        cmbMaterias = new ComboBox();
        cmbMaterias.Location = new System.Drawing.Point(650, 120);
        cmbMaterias.Size = new System.Drawing.Size(200, 20);
        cmbMaterias.DropDownStyle = ComboBoxStyle.DropDownList;

        Label lblCalificacion = new Label();
        lblCalificacion.Text = "Calificación (0-10):";
        lblCalificacion.Location = new System.Drawing.Point(650, 150);

        txtCalificacion = new TextBox();
        txtCalificacion.Location = new System.Drawing.Point(650, 170);
        txtCalificacion.Size = new System.Drawing.Size(200, 20);

        Label lblTipoEvaluacion = new Label();
        lblTipoEvaluacion.Text = "Tipo de Evaluación:";
        lblTipoEvaluacion.Location = new System.Drawing.Point(650, 200);

        cmbTipoEvaluacion = new ComboBox();
        cmbTipoEvaluacion.Location = new System.Drawing.Point(650, 220);
        cmbTipoEvaluacion.Size = new System.Drawing.Size(200, 20);
        cmbTipoEvaluacion.DropDownStyle = ComboBoxStyle.DropDownList;

        cmbTipoEvaluacion.Items.AddRange(new string[] {
            "Primer Parcial", "Segundo Parcial", "Examen Final",
            "Trabajo Práctico", "Evaluación Oral", "Proyecto"
        });

        Label lblFecha = new Label();
        lblFecha.Text = "Fecha de Evaluación:";
        lblFecha.Location = new System.Drawing.Point(650, 250);

        dtpFechaEvaluacion = new DateTimePicker();
        dtpFechaEvaluacion.Location = new System.Drawing.Point(650, 270);
        dtpFechaEvaluacion.Size = new System.Drawing.Size(200, 20);
        dtpFechaEvaluacion.Format = DateTimePickerFormat.Short;

        btnAgregar = new Button();
        btnAgregar.Text = "Agregar";
        btnAgregar.Location = new System.Drawing.Point(650, 310);
        btnAgregar.Size = new System.Drawing.Size(90, 30);
        btnAgregar.Click += BtnAgregar_Click;

        btnModificar = new Button();
        btnModificar.Text = "Modificar";
        btnModificar.Location = new System.Drawing.Point(750, 310);
        btnModificar.Size = new System.Drawing.Size(90, 30);
        btnModificar.Click += BtnModificar_Click;

        btnEliminar = new Button();
        btnEliminar.Text = "Eliminar";
        btnEliminar.Location = new System.Drawing.Point(650, 350);
        btnEliminar.Size = new System.Drawing.Size(90, 30);
        btnEliminar.Click += BtnEliminar_Click;

        btnLimpiar = new Button();
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Location = new System.Drawing.Point(750, 350);
        btnLimpiar.Size = new System.Drawing.Size(90, 30);
        btnLimpiar.Click += BtnLimpiar_Click;

        btnCerrar = new Button();
        btnCerrar.Text = "Cerrar";
        btnCerrar.Location = new System.Drawing.Point(650, 390);
        btnCerrar.Size = new System.Drawing.Size(90, 30);
        btnCerrar.Click += BtnCerrar_Click;

        this.Controls.Add(dgvNotas);
        this.Controls.Add(lblEstudiante);
        this.Controls.Add(cmbEstudiantes);
        this.Controls.Add(lblMateria);
        this.Controls.Add(cmbMaterias);
        this.Controls.Add(lblCalificacion);
        this.Controls.Add(txtCalificacion);
        this.Controls.Add(lblTipoEvaluacion);
        this.Controls.Add(cmbTipoEvaluacion);
        this.Controls.Add(lblFecha);
        this.Controls.Add(dtpFechaEvaluacion);
        this.Controls.Add(btnAgregar);
        this.Controls.Add(btnModificar);
        this.Controls.Add(btnEliminar);
        this.Controls.Add(btnLimpiar);
        this.Controls.Add(btnCerrar);
    }

    private void CargarDatos()
    {
        CargarNotas();
        CargarEstudiantes();
        CargarMaterias();
    }

    private void CargarNotas()
    {
        DataTable notas = gestor.ObtenerNotas();
        dgvNotas.DataSource = notas;

        if (dgvNotas.Columns["ID_Nota"] != null)
            dgvNotas.Columns["ID_Nota"].Visible = false;
        if (dgvNotas.Columns["ID_Estudiante"] != null)
            dgvNotas.Columns["ID_Estudiante"].Visible = false;
        if (dgvNotas.Columns["ID_Materia"] != null)
            dgvNotas.Columns["ID_Materia"].Visible = false;
    }

    private void CargarEstudiantes()
    {
        DataTable estudiantes = gestor.ObtenerEstudiantes();

        cmbEstudiantes.Items.Clear();

        foreach (DataRow fila in estudiantes.Rows)
        {
            EstudianteComboItem item = new EstudianteComboItem
            {
                ID = Convert.ToInt32(fila["ID_Estudiante"]),
                Texto = fila["Apellido"].ToString() + ", " + fila["Nombre"].ToString()
            };
            cmbEstudiantes.Items.Add(item);
        }
    }

    private void CargarMaterias()
    {
        DataTable materias = gestor.ObtenerMaterias();

        cmbMaterias.Items.Clear();

        foreach (DataRow fila in materias.Rows)
        {
            MateriaComboItem item = new MateriaComboItem
            {
                ID = Convert.ToInt32(fila["ID_Materia"]),
                Texto = fila["NombreMateria"].ToString()
            };
            cmbMaterias.Items.Add(item);
        }
    }

    private void DgvNotas_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvNotas.SelectedRows.Count > 0)
        {
            DataGridViewRow fila = dgvNotas.SelectedRows[0];

            notaSeleccionadaId = Convert.ToInt32(fila.Cells["ID_Nota"].Value);

            int idEstudiante = Convert.ToInt32(fila.Cells["ID_Estudiante"].Value);
            int idMateria = Convert.ToInt32(fila.Cells["ID_Materia"].Value);

            for (int i = 0; i < cmbEstudiantes.Items.Count; i++)
            {
                if (((EstudianteComboItem)cmbEstudiantes.Items[i]).ID == idEstudiante)
                {
                    cmbEstudiantes.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < cmbMaterias.Items.Count; i++)
            {
                if (((MateriaComboItem)cmbMaterias.Items[i]).ID == idMateria)
                {
                    cmbMaterias.SelectedIndex = i;
                    break;
                }
            }

            txtCalificacion.Text = fila.Cells["Calificacion"].Value.ToString();
            cmbTipoEvaluacion.Text = fila.Cells["TipoEvaluacion"].Value.ToString();
            dtpFechaEvaluacion.Value = Convert.ToDateTime(fila.Cells["FechaEvaluacion"].Value);
        }
    }

    private void BtnAgregar_Click(object sender, EventArgs e)
    {
        if (cmbEstudiantes.SelectedItem == null)
        {
            MessageBox.Show("Por favor seleccione un estudiante.", "Campo requerido");
            return;
        }

        if (cmbMaterias.SelectedItem == null)
        {
            MessageBox.Show("Por favor seleccione una materia.", "Campo requerido");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtCalificacion.Text))
        {
            MessageBox.Show("Por favor ingrese una calificación.", "Campo requerido");
            return;
        }

        if (!decimal.TryParse(txtCalificacion.Text, out decimal calificacion))
        {
            MessageBox.Show("La calificación debe ser un número válido.", "Formato incorrecto");
            return;
        }

        if (calificacion < 0 || calificacion > 10)
        {
            MessageBox.Show("La calificación debe estar entre 0 y 10.", "Valor fuera de rango");
            return;
        }

        if (cmbTipoEvaluacion.SelectedItem == null)
        {
            MessageBox.Show("Por favor seleccione un tipo de evaluación.", "Campo requerido");
            return;
        }

        int idEstudiante = ((EstudianteComboItem)cmbEstudiantes.SelectedItem).ID;
        int idMateria = ((MateriaComboItem)cmbMaterias.SelectedItem).ID;

        bool resultado = gestor.AgregarNota(idEstudiante, idMateria, calificacion,
                                           cmbTipoEvaluacion.Text, dtpFechaEvaluacion.Value);

        if (resultado)
        {
            MessageBox.Show("Nota agregada exitosamente.", "Éxito");
            CargarNotas();
            LimpiarCampos();
        }
    }

    private void BtnModificar_Click(object sender, EventArgs e)
    {
        if (notaSeleccionadaId == -1)
        {
            MessageBox.Show("Por favor seleccione una nota para modificar.", "Selección requerida");
            return;
        }

        if (cmbEstudiantes.SelectedItem == null || cmbMaterias.SelectedItem == null ||
            string.IsNullOrWhiteSpace(txtCalificacion.Text) || cmbTipoEvaluacion.SelectedItem == null)
        {
            MessageBox.Show("Por favor complete todos los campos.", "Campos requeridos");
            return;
        }

        if (!decimal.TryParse(txtCalificacion.Text, out decimal calificacion) ||
            calificacion < 0 || calificacion > 10)
        {
            MessageBox.Show("La calificación debe ser un número entre 0 y 10.", "Valor inválido");
            return;
        }

        int idEstudiante = ((EstudianteComboItem)cmbEstudiantes.SelectedItem).ID;
        int idMateria = ((MateriaComboItem)cmbMaterias.SelectedItem).ID;

        bool resultado = gestor.ModificarNota(notaSeleccionadaId, idEstudiante, idMateria,
                                             calificacion, cmbTipoEvaluacion.Text, dtpFechaEvaluacion.Value);

        if (resultado)
        {
            MessageBox.Show("Nota modificada exitosamente.", "Éxito");
            CargarNotas();
            LimpiarCampos();
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (notaSeleccionadaId == -1)
        {
            MessageBox.Show("Por favor seleccione una nota para eliminar.", "Selección requerida");
            return;
        }

        DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar esta nota?",
                                                   "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirmacion == DialogResult.Yes)
        {
            bool resultado = gestor.EliminarNota(notaSeleccionadaId);

            if (resultado)
            {
                MessageBox.Show("Nota eliminada exitosamente.", "Éxito");
                CargarNotas();
                LimpiarCampos();
            }
        }
    }

    private void BtnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarCampos();
    }

    private void BtnCerrar_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void LimpiarCampos()
    {
        cmbEstudiantes.SelectedIndex = -1;
        cmbMaterias.SelectedIndex = -1;
        txtCalificacion.Clear();
        cmbTipoEvaluacion.SelectedIndex = -1;
        dtpFechaEvaluacion.Value = DateTime.Today;
        notaSeleccionadaId = -1;
    }
}
public class EstudianteComboItem
{
    public int ID { get; set; }
    public string Texto { get; set; }

    public override string ToString()
    {
        return Texto;
    }
}

public class MateriaComboItem
{
    public int ID { get; set; }
    public string Texto { get; set; }

    public override string ToString()
    {
        return Texto;
    }
}