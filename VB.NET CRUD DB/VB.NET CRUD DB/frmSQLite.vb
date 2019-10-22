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