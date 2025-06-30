using System;
using System.Data;
using System.Windows.Forms;

public partial class FormEstudiantes : Form
{
    private GestorBaseDatos gestor;
    private DataGridView dgvEstudiantes;
    private TextBox txtNombre, txtApellido, txtDNI;
    private DateTimePicker dtpFechaNacimiento;
    private Button btnAgregar, btnModificar, btnEliminar, btnLimpiar, btnCerrar;
    private int estudianteSeleccionadoId = -1; // -1 significa que no hay nada seleccionado

    public FormEstudiantes()
    {
        InitializeComponent();
        gestor = new GestorBaseDatos();
        ConfigurarFormulario();
        CargarEstudiantes();
    }

    private void ConfigurarFormulario()
    {
        this.Text = "Gestión de Estudiantes";
        this.Size = new System.Drawing.Size(800, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        dgvEstudiantes = new DataGridView();
        dgvEstudiantes.Location = new System.Drawing.Point(20, 20);
        dgvEstudiantes.Size = new System.Drawing.Size(500, 300);
        dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEstudiantes.MultiSelect = false;
        dgvEstudiantes.SelectionChanged += DgvEstudiantes_SelectionChanged;

        Label lblNombre = new Label();
        lblNombre.Text = "Nombre:";
        lblNombre.Location = new System.Drawing.Point(550, 50);

        txtNombre = new TextBox();
        txtNombre.Location = new System.Drawing.Point(550, 70);
        txtNombre.Size = new System.Drawing.Size(200, 20);

        Label lblApellido = new Label();
        lblApellido.Text = "Apellido:";
        lblApellido.Location = new System.Drawing.Point(550, 100);

        txtApellido = new TextBox();
        txtApellido.Location = new System.Drawing.Point(550, 120);
        txtApellido.Size = new System.Drawing.Size(200, 20);

        Label lblDNI = new Label();
        lblDNI.Text = "DNI:";
        lblDNI.Location = new System.Drawing.Point(550, 150);

        txtDNI = new TextBox();
        txtDNI.Location = new System.Drawing.Point(550, 170);
        txtDNI.Size = new System.Drawing.Size(200, 20);

        Label lblFecha = new Label();
        lblFecha.Text = "Fecha de Nacimiento:";
        lblFecha.Location = new System.Drawing.Point(550, 200);

        dtpFechaNacimiento = new DateTimePicker();
        dtpFechaNacimiento.Location = new System.Drawing.Point(550, 220);
        dtpFechaNacimiento.Size = new System.Drawing.Size(200, 20);
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;

        btnAgregar = new Button();
        btnAgregar.Text = "Agregar";
        btnAgregar.Location = new System.Drawing.Point(550, 260);
        btnAgregar.Size = new System.Drawing.Size(90, 30);
        btnAgregar.Click += BtnAgregar_Click;

        btnModificar = new Button();
        btnModificar.Text = "Modificar";
        btnModificar.Location = new System.Drawing.Point(650, 260);
        btnModificar.Size = new System.Drawing.Size(90, 30);
        btnModificar.Click += BtnModificar_Click;

        btnEliminar = new Button();
        btnEliminar.Text = "Eliminar";
        btnEliminar.Location = new System.Drawing.Point(550, 300);
        btnEliminar.Size = new System.Drawing.Size(90, 30);
        btnEliminar.Click += BtnEliminar_Click;

        btnLimpiar = new Button();
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Location = new System.Drawing.Point(650, 300);
        btnLimpiar.Size = new System.Drawing.Size(90, 30);
        btnLimpiar.Click += BtnLimpiar_Click;

        btnCerrar = new Button();
        btnCerrar.Text = "Cerrar";
        btnCerrar.Location = new System.Drawing.Point(550, 340);
        btnCerrar.Size = new System.Drawing.Size(90, 30);
        btnCerrar.Click += BtnCerrar_Click;

        this.Controls.Add(dgvEstudiantes);
        this.Controls.Add(lblNombre);
        this.Controls.Add(txtNombre);
        this.Controls.Add(lblApellido);
        this.Controls.Add(txtApellido);
        this.Controls.Add(lblDNI);
        this.Controls.Add(txtDNI);
        this.Controls.Add(lblFecha);
        this.Controls.Add(dtpFechaNacimiento);
        this.Controls.Add(btnAgregar);
        this.Controls.Add(btnModificar);
        this.Controls.Add(btnEliminar);
        this.Controls.Add(btnLimpiar);
        this.Controls.Add(btnCerrar);
    }
    private void CargarEstudiantes()
    {
        DataTable estudiantes = gestor.ObtenerEstudiantes();
        dgvEstudiantes.DataSource = estudiantes;

        if (dgvEstudiantes.Columns["ID_Estudiante"] != null)
            dgvEstudiantes.Columns["ID_Estudiante"].Visible = false;
    }
    private void DgvEstudiantes_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvEstudiantes.SelectedRows.Count > 0)
        {
            DataGridViewRow fila = dgvEstudiantes.SelectedRows[0];

            estudianteSeleccionadoId = Convert.ToInt32(fila.Cells["ID_Estudiante"].Value);
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
            txtDNI.Text = fila.Cells["DNI"].Value.ToString();
            dtpFechaNacimiento.Value = Convert.ToDateTime(fila.Cells["FechaNacimiento"].Value);
        }
    }

    private void BtnAgregar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtDNI.Text))
        {
            MessageBox.Show("Por favor complete todos los campos obligatorios.", "Campos requeridos");
            return;
        }

        bool resultado = gestor.AgregarEstudiante(txtNombre.Text, txtApellido.Text, txtDNI.Text, dtpFechaNacimiento.Value);

        if (resultado)
        {
            MessageBox.Show("Estudiante agregado exitosamente.", "Éxito");
            CargarEstudiantes();
            LimpiarCampos();
        } else
        {
            MessageBox.Show("Error al agregar Estudiante.", "Error");
        }
    }

    private void BtnModificar_Click(object sender, EventArgs e)
    {
        if (estudianteSeleccionadoId == -1)
        {
            MessageBox.Show("Por favor seleccione un estudiante para modificar.", "Selección requerida");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtDNI.Text))
        {
            MessageBox.Show("Por favor complete todos los campos obligatorios.", "Campos requeridos");
            return;
        }

        bool resultado = gestor.ModificarEstudiante(estudianteSeleccionadoId, txtNombre.Text, txtApellido.Text, txtDNI.Text, dtpFechaNacimiento.Value);

        if (resultado)
        {
            MessageBox.Show("Estudiante modificado exitosamente.", "Éxito");
            CargarEstudiantes();
            LimpiarCampos();
        } else
        {
            MessageBox.Show("Error al modificar Estudiante.", "Error");
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (estudianteSeleccionadoId == -1)
        {
            MessageBox.Show("Por favor seleccione un estudiante para eliminar.", "Selección requerida");
            return;
        }

        DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar este estudiante?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirmacion == DialogResult.Yes)
        {
            bool resultado = gestor.EliminarEstudiante(estudianteSeleccionadoId);

            if (resultado)
            {
                MessageBox.Show("Estudiante eliminado exitosamente.", "Éxito");
                CargarEstudiantes();
                LimpiarCampos();
            } else
            {
                MessageBox.Show("Error al eliminar Estudiante.", "Error");
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
        txtNombre.Clear();
        txtApellido.Clear();
        txtDNI.Clear();
        dtpFechaNacimiento.Value = DateTime.Today;
        estudianteSeleccionadoId = -1;
    }
}