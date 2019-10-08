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