Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If My.Application.CommandLineArgs.Count > 0 Then
            Dim a As String = My.Application.CommandLineArgs.Item(0)
            Select Case a.ToUpper
                Case "PRIMANOTA"
                    formImport.Show()
                Case "ARTICOLI"
                    formImpArt.Show()
            End Select
        End If
    End Sub
End Class