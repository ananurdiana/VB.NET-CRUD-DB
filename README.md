# VB.NET CRUD DB

## Persiapan
- Install Visual Studio 2013
- Install Database Server MySQL (bisa menggunakan XAMPP, WAMPP, LAMPP)
- Install [Connector/NET 6.10.9](https://dev.mysql.com/downloads/connector/net/6.10.html)

## Initialize Project
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/01-New%20Project.png "New Project")
- Membuat project baru ```File -> New -> Project```
- Pada Window New Project, Pilih ```Tempalates -> Visual Basic -> Windows Desktop```
- Gunakan framework ```.NET Framework 4.5.2```
- Isi nama project, Pilih lokasi Project, kemudian klik OK

## Database MySQL
Menggunakan Connector/NET 6.10
### Membuat Database
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/03-Database.png "Database")
### Desain tampilan / UI
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/02-Desain%20UI%20Mahasiswa.png "UI Mahasiswa")
### Menambahkan Reference
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/04-Menambahan%20reference.png "Menambahkan Reference")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/05-Menambahan%20reference%202.png "Menambahkan Reference")
### Membuat Module
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/06-Menambahkan%20Module.png "Membuat Module")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/07-Menambahkan%20Module%202.png "Membuat Module")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/08-Menambahkan%20Module%203.png "Membuat Module")
file ```koneksiMySQL.vb``` :
```
Imports MySql.Data.MySqlClient

Module koneksiMySQL
    Public conn As New MySqlConnection
    Public cmd As New MySqlCommand
    Public MySQLReader As MySqlDataReader
    Public da As MySqlDataReader

    Public Sub konek(ByVal server As String, ByVal user As String, ByVal pass As String, ByVal db As String)
        If conn.State = ConnectionState.Closed Then
            Dim myString As String = "server=" & server & ";user=" & user & ";password=" & pass & ";database=" & db
            Try
                conn.ConnectionString = myString
                conn.Open()
            Catch ex As MySql.Data.MySqlClient.MySqlException
                MessageBox.Show("Koneksi Gagal" & vbCrLf & "Mohon cek apakah server sudah siap!", "Koneksi ke server", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End If
    End Sub

    Public Sub disconnect()
        Try
            conn.Open()
        Catch ex As MySql.Data.MySqlClient.MySqlException
        End Try
    End Sub

End Module
```
### VB.NET MySQL CRUD
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/09-Koding%20CRUD.png "VB.NET MySQL CRUD")
File ```Form1.vb``` :
```vb
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
```
### Menjalankan Program
Untuk menjalankan program, dapat menekan tombol ```F5``` di keyboard, atau dengan menu ```DEBUG -> Start Debugging```
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/10-Menjalankan%20program.png "Menjalankan Program")

## SQLite
## SQL Server Express
