Imports System.IO
Imports System.Xml

Public Class frmFilter

    Dim cf As New CConfig
    Dim op As New COpeFile
    Private Structure sFilter
        Dim Inde As String
        Dim site As String
        Dim item As String
        Dim desc As String
        Dim type As String
        Dim indexSite As String
        Dim Tabella As String
        Dim Query As String
    End Structure
    Dim gFilter As New sFilter
    Dim gIndex As Integer = 0
    Dim gKeyItems As New ArrayList
    Dim gItems As New Hashtable
    Dim bGridchanged As Boolean = False

    Private Sub frmFilter_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If bGridchanged Then
                Select Case MsgBox("La sezione onfigurazione è cambiata, vuoi uscire senza salvarla ?", MsgBoxStyle.YesNo, "Exit application")
                    Case MsgBoxResult.No
                        e.Cancel = True
                End Select
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "formClosing")
        End Try

    End Sub

    Private Sub frmFilter_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Try

            op.imposta_connessione(Globale.ConnectionString)
            Globale.cn_dbext = Globale.cn
            CAdhocDocVar.g_PlanTable = "Plan\plan_tables.xml"
            CAdhocDocVar.g_AdhocAzi = Globale.CodAzi
            adhoc = New CAdhocDoc(CAdhocDocVar.g_PlanTable, op, CAdhocDocVar.g_AdhocAzi, "SQLSERVER")
            'adhoc.loadTableFromXml("copt\banche.xml", "BAN_CHE", True)
            adhoc.aTableList.Sort()


            Me.cleanFields()
            Me.cmbTipo.Text = "Articolo"
            Me.loadItems()
            Me.loadGrid()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Form load")
        End Try

    End Sub
    Private Function loadItems() As Boolean

        Try

            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = "filterGrid.xml"
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(_File)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)


            XmlNodo = Xmlfile.GetElementsByTagName("row")
            For Each nodo As XmlNode In XmlNodo
                     For Each element As XmlNode In nodo.ChildNodes
                        Dim name As String = element.Name
                        Dim value As String = element.InnerText
                    Select Case name.ToLower
                        Case "index"
                            gFilter.Inde = value
                        Case "site"
                            gFilter.site = value
                        Case "indexsite"
                            gFilter.indexSite = value
                        Case "item"
                            gFilter.item = value
                        Case "description"
                            gFilter.desc = value
                        Case "type"
                            gFilter.type = value
                        Case "tabella"
                            gFilter.Tabella = value
                        Case "query"
                            gFilter.Query = value
                    End Select
                Next
                    gItems.Add(gFilter.Inde, gFilter)
                    gKeyItems.Add(gFilter.Inde)
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return True
        Catch ex As IOException
            saveItems()
            Return Nothing
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadItems")
        End Try

    End Function
    Private Sub loadGrid()

        Try
            itemGrid.Rows.Clear()
            Dim _rowCount As Integer = gItems.Count
            gKeyItems.Sort()
            Dim _ii As Integer = 0
            For Each element As String In gKeyItems
                If element <> "" Then
                    gFilter = gItems(element)
                    gIndex = gFilter.Inde
                    If gFilter.indexSite = Me.TxtIndexSite.Text.Trim And gFilter.site = Me.lbSito.Text.Trim Then
                        With gFilter
                            itemGrid.RowCount += 1
                            itemGrid.Item("Index", _ii).Value = .Inde
                            itemGrid.Item("Codice", _ii).Value = .item
                            itemGrid.Item("Descrizione", _ii).Value = .desc
                            itemGrid.Item("Tipo", _ii).Value = .type
                            itemGrid.Item("Tabella", _ii).Value = .Tabella
                            itemGrid.Item("Query", _ii).Value = .Query
                        End With
                        _ii += 1
                    End If
                End If
            Next
            itemGrid.Refresh()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadgrid")
        End Try

    End Sub
    Private Function updateItems() As Boolean

        Try
            gFilter = New sFilter
            Dim _rowCount As Integer = itemGrid.RowCount
            If gItems.ContainsKey(Me.txtIndex.Text) Then
                gFilter = gItems(Me.txtIndex.Text)
                With gFilter
                    .Inde = Me.txtIndex.Text.Trim
                    .item = Me.txtCodice.Text.Trim
                    .desc = Me.txtDescrizione.Text.Trim
                    .site = Me.lbSito.Text.Trim
                    .indexSite = Me.TxtIndexSite.Text.Trim
                    .type = Me.cmbTipo.Text.Trim
                    .Tabella = Me.txTabella.Text.Trim
                    .Query = Me.txQuery.Text.Trim
                End With
                gItems(Me.txtIndex.Text.Trim) = gFilter
            Else
                gIndex += 1
                With gFilter
                    .Inde = gIndex.ToString.Trim
                    .item = Me.txtCodice.Text.Trim
                    .desc = Me.txtDescrizione.Text.Trim
                    .site = Me.lbSito.Text.Trim
                    .indexSite = Me.TxtIndexSite.Text.Trim
                    .type = Me.cmbTipo.Text.Trim
                    .Tabella = Me.txTabella.Text.Trim
                    .Query = Me.txQuery.Text.Trim
                End With
                gItems.Add(gIndex.ToString.Trim, gFilter)
                gKeyItems.Add(gIndex.ToString.Trim)
            End If
            Me.loadGrid()
            bGridchanged = True
            cleanFields()
            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "UpdateItems")
        End Try

    End Function
    Private Function cleanFields()

        Try
            Me.txtCodice.Text = ""
            Me.txtDescrizione.Text = ""
            'Me.cmbTipo.Text = "Articolo"
            Me.txtIndex.Text = ""
        Catch ex As Exception

        End Try

    End Function
    Private Function saveItems() As Boolean

        Try

            Dim sw As System.IO.FileStream
            Dim _file As String = "filterGrid.xml"
            Dim stringa As String = ""
            '
            sw = New System.IO.FileStream(_file, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            'Writes header file
            stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = "<dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            gKeyItems.Sort()
            For Each element As String In gKeyItems
                If element <> "" Then
                    gFilter = gItems(element)
                    With gFilter
                        stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                        filewriter.Write(stringa)
                        filewriter.Flush()
                        '
                        stringa = Chr(9) & Chr(9) & "<index>" & .Inde & "</index>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<site>" & .site & "</site>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<indexsite>" & .indexSite & "</indexsite>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<item>" & .item & "</item>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<description>" & .desc & "</description>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<type>" & .type & "</type>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<tabella>" & .Tabella & "</tabella>" & Chr(13) & Chr(10) &
                                  Chr(9) & Chr(9) & "<query>" & .Query & "</query>" & Chr(13) & Chr(10)
                        filewriter.Write(stringa)
                        filewriter.Flush()
                        '
                        stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                        filewriter.Write(stringa)
                        filewriter.Flush()
                    End With
                End If
            Next
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            filewriter.Close()
            sw.Dispose()
            bGridchanged = False
            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "saveItems")
        End Try

    End Function
    Private Function deleteItem()

        Try
            If gItems.ContainsKey(Me.txtIndex.Text) Then
                gItems.Remove(Me.txtIndex.Text)
                gKeyItems.Remove(Me.txtIndex.Text)
            End If
            'If Me.txtCodice.Text.Trim <> "" Then
            'End If
            Me.loadGrid()
            bGridchanged = True
            cleanFields()
            Return True

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "deleteItem")
        End Try

    End Function
    Private Function readRecord()

        Try
            If Me.txtCodice.Text.Trim <> "" Then
                Select Case Me.cmbTipo.Text.Trim
                    Case "Articolo"
                        Dim harticolo As New Hashtable
                        harticolo = adhoc.readAdhocTable("ART_ICOL", "ARCODART", Me.txtCodice.Text.Trim, False)
                        If harticolo.Count > 0 Then
                            Me.txtDescrizione.Text = adhoc.getVal(harticolo("ARDESART"))
                        Else
                            MsgBox("Codice sconosciuto!", MsgBoxStyle.Critical, "readRecord")
                        End If
                    Case "Gruppo merceologico"
                        Dim harticolo As New Hashtable
                        harticolo = adhoc.readAdhocTable("GRUMERC", "GMCODICE", Me.txtCodice.Text.Trim, False)
                        If harticolo.Count > 0 Then
                            Me.txtDescrizione.Text = adhoc.getVal(harticolo("GMDESCRI"))
                        Else
                            MsgBox("Codice sconosciuto!", MsgBoxStyle.Critical, "readRecord")
                        End If
                    Case "Famiglia"
                        Dim harticolo As New Hashtable
                        harticolo = adhoc.readAdhocTable("FAM_ARTI", "FACODICE", Me.txtCodice.Text.Trim, False)
                        If harticolo.Count > 0 Then
                            Me.txtDescrizione.Text = adhoc.getVal(harticolo("FADESCRI"))
                        Else
                            MsgBox("Codice sconosciuto!", MsgBoxStyle.Critical, "readRecord")
                        End If
                End Select
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readRecord")
        End Try

    End Function

    Private Sub txtCodice_LostFocus(sender As Object, e As System.EventArgs) Handles txtCodice.LostFocus
        Me.readRecord()
    End Sub

    Private Sub btnAggiorna_Click(sender As System.Object, e As System.EventArgs) Handles btnAggiorna.Click
        Me.updateItems()
    End Sub

    Private Sub btnSalva_Click(sender As System.Object, e As System.EventArgs) Handles btnSalva.Click
        Me.saveItems()
    End Sub

    Private Sub btnElimina_Click(sender As System.Object, e As System.EventArgs) Handles btnElimina.Click
        Me.deleteItem()
    End Sub

    Private Sub itemGrid_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles itemGrid.CellContentClick
        Try
            Me.cmbTipo.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Tipo").Value
            Me.txtIndex.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Index").Value
            Me.txtCodice.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Codice").Value
            Me.txtDescrizione.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Descrizione").Value
            Me.txTabella.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Tabella").Value
            Me.txQuery.Text = itemGrid.Rows.Item(itemGrid.SelectedCells.Item(0).RowIndex).Cells("Query").Value
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "itemgridClickIn")
        End Try

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub
End Class