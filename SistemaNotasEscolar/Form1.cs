using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        ConfigurarFormulario();
    }
    private void ConfigurarFormulario()
    {
        try
        {
            this.Text = "Sistema de Libreta Escolar - Menú Principal";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            Button btnEstudiantes = new Button();
            if (btnEstudiantes != null)
            {
                btnEstudiantes.Text = "Gestionar Estudiantes";
                btnEstudiantes.Size = new System.Drawing.Size(200, 40);
                btnEstudiantes.Location = new System.Drawing.Point(100, 50);
                btnEstudiantes.Click += BtnEstudiantes_Click;
                this.Controls.Add(btnEstudiantes);
            }

            Button btnMaterias = new Button();
            if (btnMaterias != null)
            {
                btnMaterias.Text = "Gestionar Materias";
                btnMaterias.Size = new System.Drawing.Size(200, 40);
                btnMaterias.Location = new System.Drawing.Point(100, 100);
                btnMaterias.Click += BtnMaterias_Click;
                this.Controls.Add(btnMaterias);
            }

            Button btnNotas = new Button();
            if (btnNotas != null)
            {
                btnNotas.Text = "Gestionar Notas";
                btnNotas.Size = new System.Drawing.Size(200, 40);
                btnNotas.Location = new System.Drawing.Point(100, 150);
                btnNotas.Click += BtnNotas_Click;
                this.Controls.Add(btnNotas);
            }

            Button btnSalir = new Button();
            if (btnSalir != null)
            {
                btnSalir.Text = "Salir";
                btnSalir.Size = new System.Drawing.Size(200, 40);
                btnSalir.Location = new System.Drawing.Point(100, 200);
                btnSalir.Click += BtnSalir_Click;
                this.Controls.Add(btnSalir);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al configurar el formulario: {ex.Message}", "Error de configuración");
        }
    }

    private void BtnEstudiantes_Click(object sender, EventArgs e)
    {
        FormEstudiantes formEstudiantes = new FormEstudiantes();
        formEstudiantes.ShowDialog();
    }

    private void BtnMaterias_Click(object sender, EventArgs e)
    {
        FormMaterias formMaterias = new FormMaterias();
        formMaterias.ShowDialog();
    }

    private void BtnNotas_Click(object sender, EventArgs e)
    {
        FormNotas formNotas = new FormNotas();
        formNotas.ShowDialog();
    }

    private void BtnSalir_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
}