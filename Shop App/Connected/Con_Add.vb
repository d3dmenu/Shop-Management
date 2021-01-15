Imports System.Security.Cryptography
Imports System.IO
Imports MySql.Data.MySqlClient

Module Con_Add
    'Public usrnameFriend As String
    Public Conn As New MySqlConnection
    Public ConTemp As New MySqlConnection
    Public MsgSubject As String = String.Empty

    Sub ConTempOpen()
        ConTemp.ConnectionString = "server=" & My.Settings.Host & "; user id=" & My.Settings.HUser & "; password=" & My.Settings.HPass & "; database= " & My.Settings.HName & "; charset=" & My.Settings.Charset
        Try
            ConTemp.Open()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Application.Exit()
        End Try
    End Sub

    Sub OpenConnection()
        Conn.ConnectionString = "server=" & My.Settings.Host & "; user id=" & My.Settings.HUser & "; password=" & My.Settings.HPass & "; database= " & My.Settings.HName & "; charset=" & My.Settings.Charset
        Try
            Conn.Open()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "ไม่สามารถเชื่อมต่อระบบฐานข้อมูลได้ กรุณาลองใหม่ภายหลัง", MsgBoxStyle.Critical, "ระบบแจ้งเตือน")
            Application.Exit()
        End Try

    End Sub

    Function MD5Encrypter(ByVal strPass As String) As String
        'Credit goes to dreamincode.net

        Dim Hasher As New MD5CryptoServiceProvider
        Dim PasswordBytes As Byte() = New Byte(strPass.Length + 3) {}
        Dim HashBytes As Byte()

        For i As Integer = 0 To strPass.Length - 1
            PasswordBytes(i) = CByte(Asc(strPass(i)))
        Next
        PasswordBytes(strPass.Length) = CByte(90)
        PasswordBytes(strPass.Length + 1) = CByte(85)
        PasswordBytes(strPass.Length + 2) = CByte(66)
        PasswordBytes(strPass.Length + 3) = CByte(73)

        HashBytes = Hasher.ComputeHash(PasswordBytes)

        Dim NewHashBytes As Byte() = New Byte(HashBytes.Length + 3) {}

        For j As Integer = 0 To hashBytes.Length - 1
            newhashBytes(j) = hashBytes(j)
        Next
        newhashBytes(hashBytes.Length) = CByte(90)
        newhashBytes(hashBytes.Length + 1) = CByte(85)
        newhashBytes(hashBytes.Length + 2) = CByte(66)
        newhashBytes(hashBytes.Length + 3) = CByte(73)

        strPass = Convert.ToBase64String(newhashBytes)
        Return strPass
    End Function

    Function ImageToBytes(ByVal imgPath As String) As String
        Try
            Dim Image_DP As Image = Image.FromFile(imgPath)
            Dim Memories As MemoryStream = New MemoryStream()
            Image_DP.Save(Memories, System.Drawing.Imaging.ImageFormat.Png)
            Dim ImageBytes As Byte() = Memories.ToArray()

            Dim StringImage As String = Convert.ToBase64String(ImageBytes)

            Return StringImage


        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "Failed to convert image to bytes.", MsgBoxStyle.Critical, "Error Occurred")
        End Try

    End Function
End Module
