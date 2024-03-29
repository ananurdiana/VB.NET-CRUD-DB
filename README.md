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
Yang dibutuhakan untuk membuat CRUD pada SQLite adalah:
- [DB Browser for SQLite](https://sqlitebrowser.org/dl/), silahkan download dari [https://sqlitebrowser.org/dl/](https://sqlitebrowser.org/dl/)
- Install ```System.Data.SQLite``` dari ```Manage NuGet Packages```
### Install System.Data.SQLite
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/11-Install%20SQLite%201.png "Install System.Data.SQLite")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/12-Install%20SQLite%202.png "Install System.Data.SQLite")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/13-Install%20SQLite%203.png "Install System.Data.SQLite")
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/14-Install%20SQLite%204.png "Install System.Data.SQLite")
### Membuat file Database SQLite
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/15-Membuat%20File%20Database.png "Membuat file Database SQLite")
### Desain tampilan / UI untuk SQLite
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/16-Desain%20UI%20SQLite.png "Desain tampilan / UI untuk SQLite")
### VB.NET SQLite CRUD
![alt text](https://raw.githubusercontent.com/ananurdiana/vbnet_cruddb/master/dokumen/17-Script%20UI%20SQLite.png "VB.NET SQLite CRUD")
File ```frmSQLite.vb``` :
```vb
Imports System.Data.SQLite

Public Class frmSQLite

    Private sql_con As SQLiteConnection
    Private sql_cmd As SQLiteCommand
    Private DB As SQLiteDataAdapter
    Private DS As DataSet = New DataSet()
    Private DT As DataTable = New DataTable()
    Private dataInsert As ArrayList = New ArrayList()

    Private Sub txtSimpan_Click(sender As Object, e As EventArgs) Handles txtSimpan.Click
        'MsgBox(FileSystem.CurDir)
        Dim txtQuery As String = "insert into config_mysql (host, username, password, database) values(@host, @username, @password, @database)"
        dataInsert.Clear()
        dataInsert.Add("@host")
        dataInsert.Add(txtHost.Text)
        dataInsert.Add("@username")
        dataInsert.Add(txtUsername.Text)
        dataInsert.Add("@password")
        dataInsert.Add(txtPassword.Text)
        dataInsert.Add("@database")
        dataInsert.Add(txtDatabase.Text)
        executeCommand(txtQuery, dataInsert)
        loadData()
        bersihkanInput()
    End Sub

    Private Sub executeCommand(txtQuery As String, dataInsert As ArrayList)
        Me.setConnection()
        Me.sql_con.Open()
        Me.sql_cmd = Me.sql_con.CreateCommand()
        Me.sql_cmd.CommandText = txtQuery
        Me.sql_cmd.Prepare()
        For i As Integer = 0 To dataInsert.Count - 1 Step 2
            Me.sql_cmd.Parameters.AddWithValue(dataInsert(i).ToString(), dataInsert(i + 1).ToString())
        Next
        Try
            Me.sql_cmd.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Me.sql_con.Close()
    End Sub

    Private Sub setConnection()

        'If System.IO.File.Exists("mydbs.db") Then
        '    MsgBox("ada")
        'Else
        '    MsgBox("File " + FileSystem.CurDir + "\mydbs.db tidak ditemukan")
        '    Application.Exit()
        'End If

        Me.sql_con = New SQLiteConnection("Data Source=mydb.db;Version=3;New=false;Compress=True;")
    End Sub

    Private Sub loadData()
        Me.setConnection()
        Me.sql_con.Open()
        Me.sql_cmd = Me.sql_con.CreateCommand()
        Dim commandText As String = "select * from config_mysql"
        Me.DB = New SQLiteDataAdapter(commandText, Me.sql_con)
        Me.DS.Reset()
        Me.DB.Fill(Me.DS)
        Me.DT = Me.DS.Tables(0)
        DataGridView1.DataSource = DT
        Me.sql_con.Close()
    End Sub

    Private Sub loadTables()
        Me.setConnection()
        Me.sql_con.Open()
        Me.sql_cmd = Me.sql_con.CreateCommand()
        Dim commandText As String = "select * from sqlite_master"
        Me.DB = New SQLiteDataAdapter(commandText, Me.sql_con)
        Me.DS.Reset()
        Me.DB.Fill(Me.DS)
        Me.DT = Me.DS.Tables(0)
        DataGridView1.DataSource = DT
        Me.sql_con.Close()
    End Sub

    Private Sub frmSQLite_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadData()
    End Sub

    Private Sub bersihkanInput()
        txtHost.Text = ""
        txtUsername.Text = ""
        txtPassword.Text = ""
        txtDatabase.Text = ""
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.txtHost.Text = .Cells(0).Value
            Me.txtUsername.Text = .Cells(1).Value
            Me.txtPassword.Text = .Cells(2).Value
            Me.txtDatabase.Text = .Cells(3).Value
        End With
    End Sub

    Private Sub btnUbah_Click(sender As Object, e As EventArgs) Handles btnUbah.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        loadData()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        loadTables()
    End Sub

    Private Sub btnRunQuery_Click(sender As Object, e As EventArgs) Handles btnRunQuery.Click
        Me.setConnection()
        Me.sql_con.Open()
        Me.sql_cmd = Me.sql_con.CreateCommand()
        Dim commandText As String = txtQuery.Text
        Me.DB = New SQLiteDataAdapter(commandText, Me.sql_con)
        Me.DS.Reset()
        Me.DB.Fill(Me.DS)
        Me.DT = Me.DS.Tables(0)
        DataGridView1.DataSource = DT
        Me.sql_con.Close()
    End Sub
End Class
```
## WebRequest
Untuk melakukan request ke API
```vb
Private Sub btnLamp_Click(sender As Object, e As EventArgs)
    If Me.btnLamp.Text = "On" Then
        Try
            Dim req As WebRequest = WebRequest.Create("https://towering-sword.glitch.me/api/lampu/insert/1/" + txtDeviceName.Text)
            Dim res As WebResponse = req.GetResponse()
            Dim resDS As Stream = res.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(resDS)
            Dim resServer As String = reader.ReadToEnd()
            Me.txtResponse.Text = resServer
            res.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Throw
        End Try
        Me.btnLamp.Text = "Off"
    Else If Me.btnLamp.Text = "Off" Then
        Try
            Dim req As WebRequest = WebRequest.Create("https://towering-sword.glitch.me/api/lampu/insert/0/" + txtDeviceName.Text)
            Dim res As WebResponse = req.GetResponse()
            Dim resDS As Stream = res.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(resDS)
            Dim resServer As String = reader.ReadToEnd()
            Me.txtResponse.Text = resServer
            res.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Throw
        End Try
        Me.btnLamp.Text = "On"
    End If
End Sub
```

## SQL Server Express

## Oracle DataAccess
https://www.oracle.com/database/technologies/net-downloads.html
https://developer.oracle.com/dotnet/cook-dotnet.html

### Prasyarat
1. Install Oracle Database XE
2. Install [Oracle Data Access Components - .NET Downloads](https://www.oracle.com/database/technologies/net-downloads.html)

### Membuat Form baru
1. From Project menu, select Add Reference...
2. Scroll down the list of Component Names and select Oracle.DataAccess. Click OK.
3. Script Koneksi
```vb
        Dim dt As DataTable

        Dim oradb As String = "Data Source=(DESCRIPTION=" _
         + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=xxx.xxx.xxx.xxx)(PORT=1521)))" _
         + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=IDUMSD)));" _
         + "User Id=user;Password=inipassword;"
        Dim conn As New OracleConnection(oradb)
        conn.Open()
        Dim cmd As New OracleCommand
        cmd.Connection = conn
        cmd.CommandText = "SELECT * FROM EMPLOYEE"
        cmd.CommandType = CommandType.Text
        Dim dr As OracleDataReader = cmd.ExecuteReader()
        dr.Read()
        dt = New DataTable
        dt.Load(dr)
        DataGridView1.DataSource = dt
        'Label1.Text = dr.Item("jml")
        conn.Dispose()
```

## Membuat MDI Form

## Membuat Report
Untuk membuat laporan menggunakan crystal report di visual studio (install `CRforVS_13_0_12.exe` sebelum mengikuti langkah-langkah berikutnya), dan database MySQL, terlebih dahulu harus menmbuat ODBC Data Source.
Untuk membuat ODBC Data Source, harus install `mysql-connector-odbc` yang dapat di download dari [Web MySQL](https://dev.mysql.com/downloads/connector/odbc/).
Setelah selesai install `mysql-connector-odbc`, selanjutnya membuat ODBC Data Source. 

### Membuat ODBC Data Source
1. Buka `Control Panel` > `Administrative Tools` > `Data Sources (ODBC)`
2. Klik tombol `Add`
3. Pilih `MySQL ODBC x.x ANSI Driver`
4. Klik tombol `Finish`
5. Isi isian untuk konek ke database, kemudian klik tombol `OK`
![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/18-BuatKoneksiODBC.png)

### Desain Crystal Report
1. Buka project
2. Klik kanan pada project, kemudian `Add` > `New Item`
3. Pilih `Reporting`
4. Pilih `Crystal Report`
5. Berinama file sesuai kebutuhan
6. Klik tombol `Add`
    ![alt text](https://github.com/ananurdiana/VB.NET-CRUD-DB/blob/master/dokumen/19-MembuatCrystalReport-01.png)
7. Pilih `As a Blank Report`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/20-MembuatCrystalReport-02.png)
8. Setelah itu akan muncul tampilan Crystal Report kosong. Klik kanan pada `Data Fields`, kemudian klik `Log Off or On Server...`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/21-MembuatCrystalReport-03.png)
9. Pada `Create New Connection`, klik `ODBC (RDO)`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/22-MembuatCrystalReport-04.png)
10. Pilih nama ODBC Data Source yang tadi di atur di `Control Panel`, kemudian klik tombol `Next`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/23-MembuatCrystalReport-05.png)
11. Isi User ID dan Password untuk login ke database, kemudian klik tombol `Finish`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/24-MembuatCrystalReport-06.png)
12. Kemudian klik tombol `Close`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/25-MembuatCrystalReport-07.png)
13. Klik kanan pada `Data Fields`, kemudian klik `Database Expert...`. 
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/26-MembuatCrystalReport-08.png)
14. Pada `Data Expert` pilih tabel yang akan di jadikan report, kemudian tambahkan ke kolom sebelah kanan. Kemudian klik tombol `OK`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/27-MembuatCrystalReport-09.png)
15. Tambahkan Field dari `Field Explorer` ke `Sections 3 (Details)` pada Crystal Report
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/28-MembuatCrystalReport-10.png)

### Membuat Form untuk menampilkan Crystal Report
1. Klik kanan pada project, kemudian `Add` > `New Item`
2. Pilih `Windows Forms` kemudian pilih `Windows Forms`, beri nama file sesuai kebutuhan kemudia klik tombol `Add`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/29-MembuatCrystalReport-11.png)
3. Tambahkan `CrystalReportViewer` dari `Toolbox` ke Form yang baru di buat
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/30-MembuatCrystalReport-12.png)
4. Pilih Component `CrystalReportViewer` kemudian klik segitiga kecil di sebelah kanan atas. Kemudian klik `Choose a Crystal Report...`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/31-MembuatCrystalReport-13.png)
5. Pilih file Crystal Report yang tadi dibuat, kemudian klik tombol `OK`
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/32-MembuatCrystalReport-14.png)
6. Selanjutnya akan muncul data sample yang di random oleh Visual Studio
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/33-MembuatCrystalReport-15.png)
    
### Menampilkan Crystal Report
1. Buka form `MySQL`, kemudian tambahkan combobox dan button seperti pada gambar
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/34-MenampilkanCrystalReport-01.png)
2. Tambahkan event click pada button
    ```vb
        frmLaporanMahasiswa.crtLaporanMahasiswa.SelectionFormula = "{mahasiswa1.jurusan}='" & cmbJurusan.Text & "'"
        frmLaporanMahasiswa.crtLaporanMahasiswa.Refresh()
        frmLaporanMahasiswa.Show()
    ```
    ![alt text](https://raw.githubusercontent.com/ananurdiana/VB.NET-CRUD-DB/master/dokumen/35-MenampilkanCrystalReport-05.png)


## Membuat Installer
## DataGridVeiw to CSV
```vb
Private Sub subExportDGVToCSV(ByVal strExportFileName As String, ByVal DataGridView As DataGridView, Optional ByVal blnWriteColumnHeaderNames As Boolean = False, Optional ByVal strDelimiterType As String = ",")

    Dim sr As StreamWriter = File.CreateText(strExportFileName)
    Dim strDelimiter As String = strDelimiterType
    Dim intColumnCount As Integer = DataGridView.Columns.Count - 1
    Dim strRowData As String = ""

    If blnWriteColumnHeaderNames Then
        For intX As Integer = 0 To intColumnCount
            strRowData += Replace(DataGridView.Columns(intX).Name, strDelimiter, "") & IIf(intX < intColumnCount, strDelimiter, "")
        Next intX
        sr.WriteLine(strRowData)
    End If

    For intX As Integer = 0 To DataGridView.Rows.Count - 1
        strRowData = ""
        For intRowData As Integer = 0 To intColumnCount
            strRowData += Replace(DataGridView.Rows(intX).Cells(intRowData).Value, strDelimiter, "") & IIf(intRowData < intColumnCount, strDelimiter, "") '''''''''highlights this row
        Next intRowData
        sr.WriteLine(strRowData)
    Next intX
    sr.Close()

End Sub
```
