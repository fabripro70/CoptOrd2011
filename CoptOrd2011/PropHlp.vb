Imports System.Windows.Forms
Public Class PropHlp
    Private l_HlpStruct As HlpStruct
    Private HashCol As New Hashtable
    Private w_NomeColonna As String = ""
    Private SumLarghCelle As Integer
    Private GapForm As Integer = 26
    Private LarghForm As Integer
    Public NomeChiave As String
    Public Chiave As String
    Public l_struttura As New Schede
    Public grd As System.Windows.Forms.DataGridView
    Public tQuery As String

    Private Sub PropHlp_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        For ii As Integer = 0 To grd.Columns.Count - 1
            Me.l_HlpStruct.IndiceColonna = ii
            Me.l_HlpStruct.NomeColonna = grd.Columns.Item(ii).Name
            Me.l_HlpStruct.LunghezzaColonna = grd.Columns.Item(ii).Width
            Me.l_HlpStruct.TitoloColonna = grd.Columns.Item(ii).HeaderText
            Me.l_HlpStruct.visibile = grd.Columns.Item(ii).Visible
            If grd.Columns.Item(ii).Visible Then
                Me.SumLarghCelle = Me.SumLarghCelle + grd.Columns.Item(ii).Width
            End If
            Me.HashCol.Add(Me.l_HlpStruct.NomeColonna, Me.l_HlpStruct)
            Me.lstCampi.Items.Add(Me.l_HlpStruct.NomeColonna)
        Next ii
        AddHandler txtLunghezza.KeyPress, AddressOf ControllaCarattere
        Me.LarghForm = Me.SumLarghCelle + Me.GapForm
        'Me.txtTitolo.Text = Me.Text
    End Sub
    Sub ControllaCarattere(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Char.IsControl(e.KeyChar) Then
            If Not Char.IsNumber(e.KeyChar) Then
                e.Handled = True   'accetta digitazione
            End If
        End If
    End Sub

    Private Sub CalcolaLarghezzaForm()
        Me.SumLarghCelle = 0
        For ii As Integer = 0 To grd.Columns.Count - 1
            Me.l_HlpStruct = Me.HashCol(grd.Columns.Item(ii).Name)
            If Me.l_HlpStruct.visibile Then
                Me.SumLarghCelle = Me.SumLarghCelle + Me.l_HlpStruct.LunghezzaColonna
            End If
        Next ii
        Me.LarghForm = Me.SumLarghCelle + Me.GapForm
    End Sub
    Private Sub lstCampi_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstCampi.SelectedIndexChanged

        If Trim(Me.w_NomeColonna) <> "" Then
            Me.HashCol.Remove(Me.w_NomeColonna)
            Me.l_HlpStruct.TitoloColonna = Me.txtTesto.Text
            Me.l_HlpStruct.LunghezzaColonna = Me.txtLunghezza.Text
            Me.l_HlpStruct.visibile = IIf(Me.chkVisibile.CheckState = CheckState.Checked, True, False)
            Me.HashCol.Add(Me.w_NomeColonna, Me.l_HlpStruct)
        End If
        Me.l_HlpStruct = Me.HashCol(Me.lstCampi.SelectedItem.ToString)
        Me.w_NomeColonna = Me.l_HlpStruct.NomeColonna.ToString
        Me.txtLunghezza.Text = Me.l_HlpStruct.LunghezzaColonna
        Me.txtTesto.Text = Me.l_HlpStruct.TitoloColonna
        Me.chkVisibile.CheckState = IIf(Me.l_HlpStruct.visibile = True, Windows.Forms.CheckState.Checked, Windows.Forms.CheckState.Unchecked)
        Me.txtTesto.Focus()
    End Sub
    Private Sub ScriviFileConfig()
        Dim stringa As String
        Dim obj As Object = frmhlp

        Dim sw As System.IO.FileStream
        Dim fileConfig As String = Globale.percorsoApp & "\" & Me.l_struttura.Tabella & ".hlp"

        sw = New System.IO.FileStream(fileConfig, IO.FileMode.Create)
        Dim filewriter As New System.IO.StreamWriter(sw)
        stringa = "<MAIN>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<TITOLO>" & Trim(Me.txtTitolo.Text) & "</TITOLO>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<QUERY>" & Trim(Me.txtQuery.Text) & "</QUERY>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<LARGHEZZA>" & Me.LarghForm & "</LARGHEZZA>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<ALTEZZA>" & obj.height & "</ALTEZZA>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<NOMECHIAVE>" & NomeChiave & "</NOMECHIAVE>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = vbTab & "<CHIAVE>" & Chiave & "</CHIAVE>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        stringa = "</MAIN>"
        filewriter.WriteLine(stringa)
        filewriter.Flush()
        For Each colonna As String In Me.lstCampi.Items
            Me.l_HlpStruct = Me.HashCol(colonna)
            stringa = "<COLONNA>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<INDICE>" & Me.l_HlpStruct.IndiceColonna.ToString & "</INDICE>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<TESTO>" & Me.l_HlpStruct.TitoloColonna.ToString & "</TESTO>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<LUNGHEZZA>" & Me.l_HlpStruct.LunghezzaColonna.ToString & "</LUNGHEZZA>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<VISIBILE>" & Me.l_HlpStruct.visibile.ToString & "</VISIBILE>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = "</COLONNA>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
        Next
        filewriter.Close()
        sw.Close()
    End Sub

    Private Sub btnEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEsci.Click
        Me.Close()
    End Sub

    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        If Trim(Me.w_NomeColonna) <> "" Then
            Me.HashCol.Remove(Me.w_NomeColonna)
            Me.l_HlpStruct.TitoloColonna = Me.txtTesto.Text
            Me.l_HlpStruct.LunghezzaColonna = Me.txtLunghezza.Text
            Me.l_HlpStruct.visibile = IIf(Me.chkVisibile.CheckState = CheckState.Checked, True, False)
            Me.HashCol.Add(Me.w_NomeColonna, Me.l_HlpStruct)
        End If
        Me.CalcolaLarghezzaForm()
        Me.ScriviFileConfig()
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub btnGeneara_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGeneara.Click
        Me.txtQuery.Text = tQuery
    End Sub
End Class