using System;
using System.Data;
using System.Windows.Forms;

public partial class FormMaterias : Form
{
    private GestorBaseDatos gestor;
    private DataGridView dgvMaterias;
    private TextBox txtNombreMateria;
    private Button btnAgregar, btnModificar, btnEliminar, btnLimpiar, btnCerrar;
    private int materiaSeleccionadaId = -1; // Variable para rastrear qué materia está seleccionada

    public FormMaterias()
    {
        InitializeComponent();
        gestor = new GestorBaseDatos();
        ConfigurarFormulario();
        CargarMaterias();
    }

    private void ConfigurarFormulario()
    {
        this.Text = "Gestión de Materias";
        this.Size = new System.Drawing.Size(650, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        dgvMaterias = new DataGridView();
        dgvMaterias.Location = new System.Drawing.Point(20, 20);
        dgvMaterias.Size = new System.Drawing.Size(350, 300);
        dgvMaterias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMaterias.MultiSelect = false;
        dgvMaterias.SelectionChanged += DgvMaterias_SelectionChanged;

        Label lblNombreMateria = new Label();
        lblNombreMateria.Text = "Nombre de la Materia:";
        lblNombreMateria.Location = new System.Drawing.Point(400, 50);

        txtNombreMateria = new TextBox();
        txtNombreMateria.Location = new System.Drawing.Point(400, 70);
        txtNombreMateria.Size = new System.Drawing.Size(200, 20);

        btnAgregar = new Button();
        btnAgregar.Text = "Agregar";
        btnAgregar.Location = new System.Drawing.Point(400, 110);
        btnAgregar.Size = new System.Drawing.Size(90, 30);
        btnAgregar.Click += BtnAgregar_Click;

        btnModificar = new Button();
        btnModificar.Text = "Modificar";
        btnModificar.Location = new System.Drawing.Point(500, 110);
        btnModificar.Size = new System.Drawing.Size(90, 30);
        btnModificar.Click += BtnModificar_Click;

        btnEliminar = new Button();
        btnEliminar.Text = "Eliminar";
        btnEliminar.Location = new System.Drawing.Point(400, 150);
        btnEliminar.Size = new System.Drawing.Size(90, 30);
        btnEliminar.Click += BtnEliminar_Click;

        btnLimpiar = new Button();
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.Location = new System.Drawing.Point(500, 150);
        btnLimpiar.Size = new System.Drawing.Size(90, 30);
        btnLimpiar.Click += BtnLimpiar_Click;

        btnCerrar = new Button();
        btnCerrar.Text = "Cerrar";
        btnCerrar.Location = new System.Drawing.Point(400, 190);
        btnCerrar.Size = new System.Drawing.Size(90, 30);
        btnCerrar.Click += BtnCerrar_Click;

        this.Controls.Add(dgvMaterias);
        this.Controls.Add(lblNombreMateria);
        this.Controls.Add(txtNombreMateria);
        this.Controls.Add(btnAgregar);
        this.Controls.Add(btnModificar);
        this.Controls.Add(btnEliminar);
        this.Controls.Add(btnLimpiar);
        this.Controls.Add(btnCerrar);
    }

    private void CargarMaterias()
    {
        DataTable materias = gestor.ObtenerMaterias();
        dgvMaterias.DataSource = materias;

        if (dgvMaterias.Columns["ID_Materia"] != null)
            dgvMaterias.Columns["ID_Materia"].Visible = false;
    }

    private void DgvMaterias_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvMaterias.SelectedRows.Count > 0)
        {
            DataGridViewRow fila = dgvMaterias.SelectedRows[0];

            materiaSeleccionadaId = Convert.ToInt32(fila.Cells["ID_Materia"].Value);
            txtNombreMateria.Text = fila.Cells["NombreMateria"].Value.ToString();
        }
    }

    private void BtnAgregar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombreMateria.Text))
        {
            MessageBox.Show("Por favor ingrese el nombre de la materia.", "Campo requerido");
            return;
        }

        bool resultado = gestor.AgregarMateria(txtNombreMateria.Text.Trim());

        if (resultado)
        {
            MessageBox.Show("Materia agregada exitosamente.", "Éxito");
            CargarMaterias();
            LimpiarCampos();
        }
    }

    private void BtnModificar_Click(object sender, EventArgs e)
    {
        if (materiaSeleccionadaId == -1)
        {
            MessageBox.Show("Por favor seleccione una materia para modificar.", "Selección requerida");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtNombreMateria.Text))
        {
            MessageBox.Show("Por favor ingrese el nombre de la materia.", "Campo requerido");
            return;
        }

        bool resultado = gestor.ModificarMateria(materiaSeleccionadaId, txtNombreMateria.Text.Trim());

        if (resultado)
        {
            MessageBox.Show("Materia modificada exitosamente.", "Éxito");
            CargarMaterias();
            LimpiarCampos();
        }
    }

    private void BtnEliminar_Click(object sender, EventArgs e)
    {
        if (materiaSeleccionadaId == -1)
        {
            MessageBox.Show("Por favor seleccione una materia para eliminar.", "Selección requerida");
            return;
        }

        DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar esta materia?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirmacion == DialogResult.Yes)
        {
            bool resultado = gestor.EliminarMateria(materiaSeleccionadaId);

            if (resultado)
            {
                MessageBox.Show("Materia eliminada exitosamente.", "Éxito");
                CargarMaterias();
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
        txtNombreMateria.Clear();
        materiaSeleccionadaId = -1;
    }
}