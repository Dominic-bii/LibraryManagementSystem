Imports System.Data.SqlClient

Public Class Form1
    Private connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Hp\source\repos\LibraryManagementSystem\LibraryManagementSystem\LibraryDB.mdf;Integrated Security=True"
    Private connection As New SqlConnection(connectionString)
    Private command As New SqlCommand
    Private adapter As New SqlDataAdapter
    Private dataTable As New DataTable

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load book records into DataGridView
        LoadBooks()
    End Sub

    Private Sub LoadBooks()
        dataTable.Clear() ' Clear the dataTable
        ExecuteQuery("SELECT * FROM Books", CommandType.Text)
        DataGridView1.DataSource = dataTable
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles Add.Click
        ' Add new book record
        Dim titleParam As New SqlParameter("@Title", TextBox1.Text)
        Dim authorParam As New SqlParameter("@Author", TextBox2.Text)
        Dim yearPublishedParam As New SqlParameter("@YearPublished", TextBox3.Text)
        Dim genreParam As New SqlParameter("@Genre", TextBox4.Text)

        ExecuteQuery("INSERT INTO Books (Title, Author, YearPublished, Genre) VALUES (@Title, @Author, @YearPublished, @Genre)", CommandType.Text,
                     titleParam,
                     authorParam,
                     yearPublishedParam,
                     genreParam)
        LoadBooks()
        ClearFields()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles Update.Click
        ' Update existing book record
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Populate TextBox controls with selected row's values
            TextBox1.Text = DataGridView1.SelectedRows(0).Cells(1).Value.ToString()
            TextBox2.Text = DataGridView1.SelectedRows(0).Cells(2).Value.ToString()
            TextBox3.Text = DataGridView1.SelectedRows(0).Cells(3).Value.ToString()
            TextBox4.Text = DataGridView1.SelectedRows(0).Cells(4).Value.ToString()

            Dim idParam As New SqlParameter("@ID", DataGridView1.SelectedRows(0).Cells(0).Value)
            Dim titleParam As New SqlParameter("@Title", TextBox1.Text)
            Dim authorParam As New SqlParameter("@Author", TextBox2.Text)
            Dim yearPublishedParam As New SqlParameter("@YearPublished", TextBox3.Text)
            Dim genreParam As New SqlParameter("@Genre", TextBox4.Text)

            ExecuteQuery("UPDATE Books SET Title = @Title, Author = @Author, YearPublished = @YearPublished, Genre = @Genre WHERE ID = @ID", CommandType.Text,
                         idParam,
                         titleParam,
                         authorParam,
                         yearPublishedParam,
                         genreParam)
            LoadBooks()
            ClearFields()
        Else
            MessageBox.Show("Please select a row to update.")
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles Delete.Click
        ' Delete book record
        Dim idParam As New SqlParameter("@ID", DataGridView1.SelectedRows(0).Cells(0).Value)

        ExecuteQuery("DELETE FROM Books WHERE ID = @ID", CommandType.Text,
                     idParam)
        LoadBooks()
        ClearFields()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles Clear.Click
        ' Clear fields
        ClearFields()
    End Sub

    Private Sub ClearFields()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
    End Sub

    Private Sub ExecuteQuery(query As String, commandType As CommandType, ParamArray parameters As SqlParameter())
        Try
            connection.Open()
            command.CommandText = query
            command.Connection = connection
            command.CommandType = commandType
            For Each parameter As SqlParameter In parameters
                command.Parameters.Add(parameter)
            Next
            If commandType = CommandType.Text Then
                adapter.SelectCommand = command
                adapter.Fill(dataTable)
            Else
                command.ExecuteNonQuery()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub
End Class
