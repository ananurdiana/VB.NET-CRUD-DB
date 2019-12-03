<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLaporanMahasiswa
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.crtLaporanMahasiswa = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.rptMahasiswa1 = New VB.NET_CRUD_DB.rptMahasiswa()
        Me.SuspendLayout()
        '
        'crtLaporanMahasiswa
        '
        Me.crtLaporanMahasiswa.ActiveViewIndex = 0
        Me.crtLaporanMahasiswa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crtLaporanMahasiswa.Cursor = System.Windows.Forms.Cursors.Default
        Me.crtLaporanMahasiswa.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crtLaporanMahasiswa.Location = New System.Drawing.Point(0, 0)
        Me.crtLaporanMahasiswa.Name = "crtLaporanMahasiswa"
        Me.crtLaporanMahasiswa.ReportSource = Me.rptMahasiswa1
        Me.crtLaporanMahasiswa.Size = New System.Drawing.Size(674, 354)
        Me.crtLaporanMahasiswa.TabIndex = 0
        '
        'frmLaporanMahasiswa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 354)
        Me.Controls.Add(Me.crtLaporanMahasiswa)
        Me.Name = "frmLaporanMahasiswa"
        Me.Text = "frmLaporanMahasiswa"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents crtLaporanMahasiswa As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents rptMahasiswa1 As VB.NET_CRUD_DB.rptMahasiswa
End Class
