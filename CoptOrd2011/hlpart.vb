Public Class hlpart
    Dim testosel As New Articolo
    Public Structure Articolo
        Dim codart As String
        Dim desart As String
        Dim unimis As String
    End Structure
    Dim wArticolo As Articolo
    Private Sub hlpart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.carica_griglia("")
    End Sub
    Public ReadOnly Property ValRet() As ValoreRitorno
        Get
            Try
                Dim ic As New ValoreRitorno
                If testosel.codart Is Nothing Then
                    ic.valore.codart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(0).Value
                    ic.valore.desart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(1).Value
                    ic.valore.unimis = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(2).Value
                Else
                    ic.valore.codart = testosel.codart
                    ic.valore.desart = testosel.desart
                    ic.valore.unimis = testosel.unimis
                End If
                Return ic
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
    End Property
    Private Sub grihlp_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles griart.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            testosel.codart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(0).Value
            testosel.desart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(1).Value
            testosel.unimis = CTran(griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(2).Value, "")
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub carica_griglia(ByVal pCriterio As String)

        Dim ds As New DataSet
        Dim ii As Integer = 0
        Dim strsql As String = "SELECT CODART, DESART, UNMIS FROM ARTICOLI WHERE CODART IN (SELECT DISTINCT COMPLES FROM DISTBASE)"
        If pCriterio.Trim <> "" Then
            strsql = strsql & pCriterio
        End If
        Dim NRecord As Long = opConn.esegui_query(strsql, Globale.cn_dbf, ds)
        If NRecord = 0 Then
            Exit Sub
        End If
        griart.RowCount = NRecord
        For Each dr As DataRow In ds.Tables(0).Rows
            griart.Item(0, ii).Value = dr("CODART")
            griart.Item(1, ii).Value = dr("DESART")
            griart.Item(2, ii).Value = dr("UNMIS")
            ii = ii + 1
        Next

    End Sub
    Private Sub griart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles griart.DoubleClick

        testosel.codart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(0).Value
        testosel.desart = griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(1).Value
        testosel.unimis = CTran(griart.Rows.Item(griart.SelectedCells.Item(0).RowIndex).Cells(2).Value, "")
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub btnCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerca.Click
        Dim strCriterio As String = ""
        If Me.txtComples.Text.Trim <> "" Then
            strCriterio = " AND CODART LIKE '" & Me.txtComples.Text.Trim & "%'"
        End If
        If Me.txtDesart.Text.Trim <> "" Then
            strCriterio = " AND DESART LIKE '" & Me.txtDesart.Text.Trim & "%'"
        End If
        Me.carica_griglia(strCriterio)
    End Sub
End Class
Public Class ValoreRitorno
    Public valore As Articolo
End Class