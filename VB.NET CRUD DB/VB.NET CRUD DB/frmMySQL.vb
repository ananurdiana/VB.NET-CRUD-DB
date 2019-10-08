Imports VB.NET_CRUD_DB.koneksiMySQL
Imports MySql.Data.MySqlClient

Public Class frmMySQL

    Dim conn As New MySqlConnection("Server=xxx.xxx.xxx.xxx; user=xxx; password=xxx; database=example")
    Dim perintah As New MySqlCommand
    Dim data As New MySqlDataAdapter
    Dim ds As New DataSet

    Private Sub tampilData()
        Dim dt As DataTable
        Dim adapter As MySqlDataAdapter
        Dim sqlstr As String
        Dim data As Integer

        sqlstr = "SELECT * FROM example.mahasiswa"
        adapter = New MySqlDataAdapter(sqlstr, conn)
        dt = New DataTable
        data = adapter.Fill(dt)

        If data > 0 Then
            DataGridView1.DataSource = dt
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill
            DataGridView1.Columns(0).HeaderText = "NIM"
            DataGridView1.Columns(1).HeaderText = "Nama"
            DataGridView1.Columns(2).HeaderText = "Jurusan"
        Else
            DataGridView1.DataSource = Nothing
        End If
        bersih()
    End Sub

    Private Sub bersih()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
    End Sub

    Private Sub frmMySQL_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        konek("xxx.xxx.xxx.xxx", "username", "password", "example")
        tampilData()
        statusTombol(True, False, False)
        statusInput(False, False, False)
    End Sub

    Private Sub statusTombol(tambah As Boolean, ubah As Boolean, hapus As Boolean)
        Button1.Enabled = tambah
        Button2.Enabled = ubah
        Button3.Enabled = hapus
    End Sub

    Private Sub statusInput(nim As Boolean, nama As Boolean, jurusan As Boolean)
        TextBox1.Enabled = nim
        TextBox2.Enabled = nama
        TextBox3.Enabled = jurusan
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Tambah" Then
            Button1.Text = "Simpan"
            Button3.Text = "Batal"
            bersih()
            statusInput(True, True, True)
            statusTombol(True, False, True)
            TextBox1.Focus()
        ElseIf Button1.Text = "Simpan" Then

            conn.Open()
            Try
                perintah.CommandType = CommandType.Text
                perintah.CommandText = "insert into mahasiswa (nim, nama, jurusan) values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "')"
                perintah.Connection = conn
                perintah.ExecuteNonQuery()
                MsgBox("Data berhasil di simpan", MsgBoxStyle.Information, "Informasi")

                tampilData()
                bersih()
                statusInput(False, False, False)
                statusTombol(True, False, False)
            Catch ex As Exception
                MsgBox("Gagal simpan" + ex.Message, MsgBoxStyle.Critical, "ERROR")
            End Try
            conn.Close()

            Button1.Text = "Tambah"
            Button3.Text = "Hapus"
            statusInput(False, False, False)
            statusTombol(True, False, False)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Hapus" Then
            Dim hapus As MsgBoxResult = MessageBox.Show("Yakin mau menghapus " & TextBox2.Text & " ?", "Konfirmasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
            If hapus = vbOK Then
                Try
                    conn.Open()
                    perintah.Connection = conn
                    perintah.CommandType = CommandType.Text
                    perintah.CommandText = "delete from mahasiswa where nim = '" & TextBox1.Text & "'"
                    perintah.ExecuteNonQuery()
                    conn.Close()
                Catch ex As Exception
                    MsgBox("Gagal hapus data", MsgBoxStyle.Exclamation, "Error")
                End Try

                bersih()
                statusInput(False, False, False)
                statusTombol(True, False, False)
                tampilData()
            End If
        ElseIf Button3.Text = "Batal" Then
            bersih()
            Button1.Text = "Tambah"
            Button3.Text = "Hapus"
            statusInput(False, False, False)
            statusTombol(True, False, False)
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox1.Text = .Cells(0).Value
            Me.TextBox2.Text = .Cells(1).Value
            Me.TextBox3.Text = .Cells(2).Value
        End With

        statusTombol(True, True, True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Ubah" Then
            Button1.Text = "Tambah"
            Button2.Text = "Simpan"
            Button3.Text = "Batal"
            statusInput(False, True, True)
            statusTombol(False, True, True)
            TextBox2.Focus()

        ElseIf Button2.Text = "Simpan" Then
            Try
                conn.Open()
                perintah.Connection = conn
                perintah.CommandType = CommandType.Text
                perintah.CommandText = "update mahasiswa set nama='" & TextBox2.Text & "', jurusan='" & TextBox3.Text & "' where nim='" & TextBox1.Text & "'"
                perintah.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                MsgBox("Gagal Update data", MsgBoxStyle.Exclamation, "Error")
            End Try

            bersih()
            Button1.Text = "Tambah"
            Button2.Text = "Ubah"
            Button3.Text = "Hapus"
            statusInput(False, False, False)
            statusTombol(True, False, False)
            tampilData()
        End If
    End Sub
End Class
