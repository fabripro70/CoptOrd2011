Imports System.Windows.Forms
Public Class frmhlp
    Public l_campo As New StructCampi
    Public oCampo As ClSLib.CTextBoxEx
    Public Titolo As String
    Public PosX As Long
    Public PosY As Long
    Public FixedKey As String
    Public zoomOnzoom As String
    Private Structure TabJoin
        Dim NomeTabella As String
        Dim AliasTabella As String
        Dim CampoSinistra As String
        Dim CampoDestra As String
    End Structure
    Private l_TabJoin As New TabJoin
    Private l_struttura As New Schede
    Private l_strucTabJ As New Schede
    Private ListaTabJoin As New ArrayList
    Private LstColonne As ArrayList
    Private GapForm As Integer = 26
    Private LarghGrid As Integer
    Private op As New COpeFile
    Private MainQuery As String = ""
    Private testoSel As String = ""
    Private MapField As New Hashtable

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Public ReadOnly Property ValoreRitorno() As ValoreRitorno
        Get
            Try
                Dim ic As New ValoreRitorno
                ic.valore = ""
                Return ic
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
    End Property
    Private Sub puli_griglia()

        Dim hAlias As New Hashtable
        Dim fieldAlias As New Hashtable
        Dim AliasFldName As String = ""
        Dim stringa_sql As String = ""
        MapField.Clear()
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                stringa_sql = "SELECT [" & Me.l_struttura.Tabella & "].NUMREC"
                For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                    '
                    AliasFldName = fld.SNome
                    fieldAlias.Add(fld.SNome, 0)
                    '
                    MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                    '
                    stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                    'stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & fld.SNome
                    If fld.STabellaJoin.Trim <> "" Then
                        l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                        l_TabJoin.CampoSinistra = fld.SNome
                        l_TabJoin.CampoDestra = fld.SChiaveJoin
                        If hAlias.Contains(l_TabJoin.NomeTabella) Or l_TabJoin.NomeTabella = Me.l_struttura.Tabella Then
                            Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                            Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                            l_TabJoin.AliasTabella = nomeAlias
                            hAlias(l_TabJoin.NomeTabella) = prog + 1
                        Else
                            hAlias.Add(l_TabJoin.NomeTabella, 0)
                            l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                        End If
                        Me.ListaTabJoin.Add(l_TabJoin)
                    End If
                Next
                Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                'Legge la struttura delle tabelle collegate e le mette in join
                For Each element As TabJoin In Me.ListaTabJoin
                    Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                    For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella
                        '
                        If fieldAlias.ContainsKey(fld.SNome) Then
                            Dim prgf As Integer = fieldAlias(fld.SNome)
                            AliasFldName = fld.SNome & prgf.ToString.Trim
                            fieldAlias(fld.SNome) = AliasFldName
                        Else
                            AliasFldName = fld.SNome
                            fieldAlias.Add(fld.SNome, 0)
                        End If
                        '
                        'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                        stringa_sql = stringa_sql & ", [" & element.AliasTabella & "]." & fld.SNome & " AS " & AliasFldName
                        MapField.Add(AliasFldName, element.AliasTabella & "." & fld.SNome)
                    Next
                    stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON [" & _
                                                  Me.l_struttura.Tabella & "]." & element.CampoSinistra & " = [" & _
                                                  element.AliasTabella & "]." & element.CampoDestra
                Next

                stringa_sql = stringa_sql & stringa_from & " WHERE 1=0"
            Case "ORACLE"
                stringa_sql = "SELECT " & Me.l_struttura.Tabella & ".NUMREC"
                For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                    '
                    AliasFldName = fld.SNome
                    fieldAlias.Add(fld.SNome, 0)
                    '
                    MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                    '
                    stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                    'stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & fld.SNome
                    If fld.STabellaJoin.Trim <> "" Then
                        l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                        l_TabJoin.CampoSinistra = fld.SNome
                        l_TabJoin.CampoDestra = fld.SChiaveJoin
                        If hAlias.Contains(l_TabJoin.NomeTabella) Or l_TabJoin.NomeTabella = Me.l_struttura.Tabella Then
                            Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                            Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                            l_TabJoin.AliasTabella = nomeAlias
                            hAlias(l_TabJoin.NomeTabella) = prog + 1
                        Else
                            hAlias.Add(l_TabJoin.NomeTabella, 0)
                            l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                        End If
                        Me.ListaTabJoin.Add(l_TabJoin)
                    End If
                Next
                Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                'Legge la struttura delle tabelle collegate e le mette in join
                For Each element As TabJoin In Me.ListaTabJoin
                    Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                    For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella

                        '
                        If fieldAlias.ContainsKey(fld.SNome) Then
                            Dim prgf As Integer = fieldAlias(fld.SNome)
                            AliasFldName = fld.SNome & prgf.ToString.Trim
                            fieldAlias(fld.SNome) = AliasFldName
                        Else
                            AliasFldName = fld.SNome
                            fieldAlias.Add(fld.SNome, 0)
                        End If
                        '
                        'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                        stringa_sql = stringa_sql & ", " & element.AliasTabella & "." & fld.SNome & " AS " & AliasFldName
                        MapField.Add(AliasFldName, element.AliasTabella & "." & fld.SNome)
                    Next
                    stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON " & _
                                                  Me.l_struttura.Tabella & "." & element.CampoSinistra & " = " & _
                                                  element.AliasTabella & "." & element.CampoDestra
                Next
                stringa_sql = stringa_sql & stringa_from & " WHERE 1=0"
            Case "MYSQL"
                stringa_sql = "SELECT " & Me.l_struttura.Tabella & ".NUMREC"
                For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                    '
                    AliasFldName = fld.SNome
                    fieldAlias.Add(fld.SNome, 0)
                    '
                    MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                    '
                    stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                    'stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & fld.SNome
                    If fld.STabellaJoin.Trim <> "" Then
                        l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                        l_TabJoin.CampoSinistra = fld.SNome
                        l_TabJoin.CampoDestra = fld.SChiaveJoin
                        If hAlias.Contains(l_TabJoin.NomeTabella) Or l_TabJoin.NomeTabella = Me.l_struttura.Tabella Then
                            Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                            Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                            l_TabJoin.AliasTabella = nomeAlias
                            hAlias(l_TabJoin.NomeTabella) = prog + 1
                        Else
                            hAlias.Add(l_TabJoin.NomeTabella, 0)
                            l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                        End If
                        Me.ListaTabJoin.Add(l_TabJoin)
                    End If
                Next
                Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                'Legge la struttura delle tabelle collegate e le mette in join
                For Each element As TabJoin In Me.ListaTabJoin
                    Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                    For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella
                        '
                        If fieldAlias.ContainsKey(fld.SNome) Then
                            Dim prgf As Integer = fieldAlias(fld.SNome)
                            AliasFldName = fld.SNome & prgf.ToString.Trim
                            fieldAlias(fld.SNome) = AliasFldName
                        Else
                            AliasFldName = fld.SNome
                            fieldAlias.Add(fld.SNome, 0)
                        End If
                        '
                        'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                        stringa_sql = stringa_sql & ", " & element.AliasTabella & "." & fld.SNome & " AS " & AliasFldName
                        MapField.Add(AliasFldName, element.AliasTabella & "." & fld.SNome)
                    Next
                    stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON " & _
                                                  Me.l_struttura.Tabella & "." & element.CampoSinistra & " = " & _
                                                  element.AliasTabella & "." & element.CampoDestra
                Next
                stringa_sql = stringa_sql & stringa_from & " WHERE 1=0"
        End Select
        '
        Dim ds As DataSet = op.esegui_query(stringa_sql)
        grihlp.DataSource = ds.Tables(0)
        Me.ListaTabJoin.Clear()
    End Sub
    Private Sub carica_griglia()

        Dim hAlias As New Hashtable
        Dim fieldAlias As New Hashtable
        Dim AliasFldName As String = ""
        Dim stringa_sql As String = ""
        'MapField.Clear()
        Select Case Globale.TipoDb
            Case "SQLSERVER"
                MainQuery = ""
                If MainQuery.Trim() = "" Then
                    stringa_sql = "SELECT [" & Me.l_struttura.Tabella & "].NUMREC"
                    For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                        '
                        AliasFldName = fld.SNome
                        fieldAlias.Add(fld.SNome, 0)
                        'MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                        '
                        stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                        If fld.STabellaJoin.Trim <> "" Then
                            l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                            l_TabJoin.CampoSinistra = fld.SNome
                            l_TabJoin.CampoDestra = fld.SChiaveJoin
                            If hAlias.Contains(l_TabJoin.NomeTabella) Then
                                Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                                Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                                l_TabJoin.AliasTabella = nomeAlias
                                hAlias(l_TabJoin.NomeTabella) = prog + 1
                            Else
                                hAlias.Add(l_TabJoin.NomeTabella, 0)
                                l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                            End If
                            Me.ListaTabJoin.Add(l_TabJoin)
                        End If
                    Next
                    Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                    'Legge la struttura delle tabelle collegate e le mette in join
                    For Each element As TabJoin In Me.ListaTabJoin
                        Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                        For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella
                            '
                            If fieldAlias.ContainsKey(fld.SNome) Then
                                Dim prgf As Integer = fieldAlias(fld.SNome)
                                AliasFldName = fld.SNome & prgf.ToString.Trim
                                fieldAlias(fld.SNome) = AliasFldName
                            Else
                                AliasFldName = fld.SNome
                                fieldAlias.Add(fld.SNome, 0)
                            End If
                            '
                            'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                            stringa_sql = stringa_sql & ", " & element.AliasTabella & "." & fld.SNome & " AS " & AliasFldName
                        Next
                        stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON [" & _
                                                      Me.l_struttura.Tabella & "]." & element.CampoSinistra & " = [" & _
                                                      element.AliasTabella & "].NUMREC" ' & element.CampoDestra
                    Next
                    '
                    stringa_sql = stringa_sql & stringa_from
                    stringa_sql = stringa_sql & " WHERE 1=1"
                    If Not String.IsNullOrEmpty(FixedKey) Then
                        stringa_sql = stringa_sql & " AND " & FixedKey.Trim()
                    End If
                    If Trim(Me.lbChiave.Text) <> "" And Trim(Me.txtCerca.Text) <> "" Then
                        stringa_sql = stringa_sql & " AND " & Trim(Me.lbChiave.Text) & " like '" & trasforma_stringa(Trim(Me.txtCerca.Text)) & "%'"
                        stringa_sql = stringa_sql & " ORDER BY " & Trim(Me.lbChiave.Text)
                    End If
                    Me.MainQuery = stringa_sql
                Else
                    stringa_sql = Me.MainQuery.Trim
                End If
            Case "ORACLE"
                MainQuery = ""
                If MainQuery.Trim() = "" Then
                    stringa_sql = "SELECT " & Me.l_struttura.Tabella & ".NUMREC"
                    For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                        '
                        AliasFldName = fld.SNome
                        fieldAlias.Add(fld.SNome, 0)
                        'MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                        '
                        stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                        If fld.STabellaJoin.Trim <> "" Then
                            l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                            l_TabJoin.CampoSinistra = fld.SNome
                            l_TabJoin.CampoDestra = fld.SChiaveJoin
                            If hAlias.Contains(l_TabJoin.NomeTabella) Then
                                Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                                Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                                l_TabJoin.AliasTabella = nomeAlias
                                hAlias(l_TabJoin.NomeTabella) = prog + 1
                            Else
                                hAlias.Add(l_TabJoin.NomeTabella, 0)
                                l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                            End If
                            Me.ListaTabJoin.Add(l_TabJoin)
                        End If
                    Next
                    Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                    'Legge la struttura delle tabelle collegate e le mette in join
                    For Each element As TabJoin In Me.ListaTabJoin
                        Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                        For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella
                            '
                            If fieldAlias.ContainsKey(fld.SNome) Then
                                Dim prgf As Integer = fieldAlias(fld.SNome)
                                AliasFldName = fld.SNome & prgf.ToString.Trim
                                fieldAlias(fld.SNome) = AliasFldName
                            Else
                                AliasFldName = fld.SNome
                                fieldAlias.Add(fld.SNome, 0)
                            End If
                            '
                            'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                            stringa_sql = stringa_sql & ", " & element.AliasTabella & "." & fld.SNome & " AS " & AliasFldName
                        Next
                        stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON " & _
                                                      Me.l_struttura.Tabella & "." & element.CampoSinistra & " = " & _
                                                      element.AliasTabella & ".NUMREC" ' & element.CampoDestra
                    Next
                    '
                    stringa_sql = stringa_sql & stringa_from
                    stringa_sql = stringa_sql & " WHERE 1=1"
                    If Not String.IsNullOrEmpty(FixedKey) Then
                        stringa_sql = stringa_sql & " AND " & FixedKey.Trim()
                    End If
                    If Trim(Me.lbChiave.Text) <> "" And Trim(Me.txtCerca.Text) <> "" Then
                        stringa_sql = stringa_sql & " AND " & Trim(Me.lbChiave.Text) & " like '" & trasforma_stringa(Trim(Me.txtCerca.Text)) & "%'"
                        stringa_sql = stringa_sql & " ORDER BY " & Trim(Me.lbChiave.Text)
                    End If
                    Me.MainQuery = stringa_sql
                Else
                    stringa_sql = Me.MainQuery.Trim
                End If
            Case "MYSQL"
                MainQuery = ""
                If MainQuery.Trim() = "" Then
                    stringa_sql = "SELECT " & Me.l_struttura.Tabella & ".NUMREC"
                    For Each fld As StructCampi In Me.l_struttura.StrutturaTabella
                        '
                        AliasFldName = fld.SNome
                        fieldAlias.Add(fld.SNome, 0)
                        'MapField.Add(AliasFldName, Me.l_struttura.Tabella & "." & fld.SNome)
                        '
                        stringa_sql = stringa_sql & ", " & Me.l_struttura.Tabella & "." & fld.SNome & " AS " & AliasFldName
                        If fld.STabellaJoin.Trim <> "" Then
                            l_TabJoin.NomeTabella = fld.STabellaJoin.ToString
                            l_TabJoin.CampoSinistra = fld.SNome
                            l_TabJoin.CampoDestra = fld.SChiaveJoin
                            If hAlias.Contains(l_TabJoin.NomeTabella) Then
                                Dim prog As Integer = hAlias(l_TabJoin.NomeTabella)
                                Dim nomeAlias As String = l_TabJoin.NomeTabella & prog.ToString.Trim
                                l_TabJoin.AliasTabella = nomeAlias
                                hAlias(l_TabJoin.NomeTabella) = prog + 1
                            Else
                                hAlias.Add(l_TabJoin.NomeTabella, 0)
                                l_TabJoin.AliasTabella = l_TabJoin.NomeTabella
                            End If
                            Me.ListaTabJoin.Add(l_TabJoin)
                        End If
                    Next
                    Dim stringa_from As String = " FROM " & op.getTablename(Me.l_struttura.Tabella) & " " & Me.l_struttura.Tabella
                    'Legge la struttura delle tabelle collegate e le mette in join
                    For Each element As TabJoin In Me.ListaTabJoin
                        Me.l_strucTabJ.StrutturaTabella = op.leggi_struttura(element.NomeTabella)
                        '
                        For Each fld As StructCampi In Me.l_strucTabJ.StrutturaTabella
                            '
                            If fieldAlias.ContainsKey(fld.SNome) Then
                                Dim prgf As Integer = fieldAlias(fld.SNome)
                                AliasFldName = fld.SNome & prgf.ToString.Trim
                                fieldAlias(fld.SNome) = AliasFldName
                            Else
                                AliasFldName = fld.SNome
                                fieldAlias.Add(fld.SNome, 0)
                            End If
                            '
                            'stringa_sql = stringa_sql & ", " & element.NomeTabella & "." & fld.SNome & " AS " & fld.SNome
                            stringa_sql = stringa_sql & ", " & element.AliasTabella & "." & fld.SNome & " AS " & AliasFldName
                            'MapField.Add(AliasFldName, element.AliasTabella & "." & fld.SNome)
                        Next
                        '
                        stringa_from = stringa_from & " LEFT JOIN " & op.getTablename(element.NomeTabella) & " " & element.AliasTabella & " ON " & _
                                                      Me.l_struttura.Tabella & "." & element.CampoSinistra & " = " & _
                                                      element.AliasTabella & ".NUMREC" ' & element.CampoDestra
                    Next
                    '
                    stringa_sql = stringa_sql & stringa_from
                    stringa_sql = stringa_sql & " WHERE 1=1"
                    If Not String.IsNullOrEmpty(FixedKey) Then
                        stringa_sql = stringa_sql & " AND " & FixedKey.Trim()
                    End If
                    If Trim(Me.lbChiave.Text) <> "" And Trim(Me.txtCerca.Text) <> "" Then
                        Dim criteriaField As String = MapField(Me.lbChiave.Text)
                        stringa_sql = stringa_sql & " AND " & criteriaField.Trim & " like '" & trasforma_stringa(Trim(Me.txtCerca.Text)) & "%'"
                        stringa_sql = stringa_sql & " ORDER BY " & Trim(Me.lbChiave.Text)
                    End If
                    Me.MainQuery = stringa_sql
                Else
                    stringa_sql = Me.MainQuery.Trim
                End If
        End Select
        Dim ds As DataSet = op.esegui_query(stringa_sql)
        '
        grihlp.DataSource = ds.Tables(0)
        Me.ListaTabJoin.Clear()
    End Sub
    Private Sub ScriviFileConfig()
        Dim stringa As String

        Dim sw As System.IO.FileStream
        Dim fileConfig As String = Globale.percorsoApp & "\" & Me.l_struttura.Tabella & ".hlp"
        Try
            sw = New System.IO.FileStream(fileConfig, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            stringa = "<MAIN>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<TITOLO>" & Trim(Me.Text) & "</TITOLO>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<QUERY>" & Trim(Me.MainQuery) & "</QUERY>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<LARGHEZZA>" & Me.Width & "</LARGHEZZA>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<ALTEZZA>" & Me.Height & "</ALTEZZA>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<NOMECHIAVE>" & Me.lb_cerca.Text & "</NOMECHIAVE>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = vbTab & "<CHIAVE>" & Me.lbChiave.Text & "</CHIAVE>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            stringa = "</MAIN>"
            filewriter.WriteLine(stringa)
            filewriter.Flush()
            For ii As Integer = 0 To grihlp.Columns.Count - 1
                stringa = "<COLONNA>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
                stringa = vbTab & "<INDICE>" & ii.ToString & "</INDICE>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
                stringa = vbTab & "<TESTO>" & grihlp.Columns.Item(ii).HeaderText & "</TESTO>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
                stringa = vbTab & "<LUNGHEZZA>" & grihlp.Columns.Item(ii).Width & "</LUNGHEZZA>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
                stringa = vbTab & "<VISIBILE>" & grihlp.Columns.Item(ii).Visible.ToString & "</VISIBILE>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
                stringa = "</COLONNA>"
                filewriter.WriteLine(stringa)
                filewriter.Flush()
            Next
            filewriter.Close()
            sw.Close()
        Catch ex As IO.IOException
        End Try
    End Sub
    Private Sub frmhlp_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Left = 25
        Me.Top = 180
        Try
            If l_campo.STabellaJoin.ToString <> "" Then
                Me.l_struttura.Tabella = l_campo.STabellaJoin.ToString
                Me.l_struttura.StrutturaTabella = op.leggi_struttura(Me.l_struttura.Tabella)
            End If
            'If Not IsNothing(oCampo) Then
            'If Not IsNothing(oCampo.LinkDati.Criteriofisso) Then
            'Me.FixedKey = oCampo.LinkDati.Criteriofisso
            'End If
            'End If
            If Me.FixedKey Is Nothing Then
                Me.FixedKey = ""
            End If
            puli_griglia()
            Me.LeggiFileConfig()
            Me.carica_griglia()
            Me.Refresh()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Help")
        End Try
    End Sub
    Private Sub grihlp_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles grihlp.ColumnHeaderMouseClick

        Try
            Dim grd As New System.Windows.Forms.DataGridView
            Dim col As System.Windows.Forms.DataGridViewColumn
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Dim PH As New PropHlp
                grd = sender
                PH.grd = Me.grihlp
                PH.l_struttura = Me.l_struttura
                PH.NomeChiave = Trim(Me.lb_cerca.Text)
                PH.Chiave = Trim(Me.lbChiave.Text)
                PH.Top = Me.Top + Me.grihlp.Location.Y + 200
                PH.Left = Me.Left + Me.grihlp.Location.X + 200
                PH.txtTitolo.Text = Me.Text.Trim
                PH.tQuery = Me.MainQuery.Trim
                PH.ShowDialog()
                Me.LeggiFileConfig()
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                grd = sender
                col = grd.SortedColumn
                Me.lb_cerca.Text = Trim(col.HeaderText.ToString)
                Me.lbChiave.Text = Trim(col.Name.ToString)
                Me.ScriviFileConfig()
            End If
        Catch ex As NullReferenceException

        End Try

    End Sub
    Private Sub LeggiFileConfig()

        Dim RColonna As Boolean = False
        Dim RMain As Boolean = False
        Dim stringa As String
        Dim posIni As Integer
        Dim posFin As Integer
        Dim Indice As Integer

        Try
            Dim fileConfig As String = Globale.percorsoApp & "\" & Me.l_struttura.Tabella & ".hlp"
            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(fileConfig, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            While filereader.Peek > -1
                stringa = filereader.ReadLine
                If stringa.Contains("<MAIN>") Then
                    RMain = True
                    Continue While
                End If
                If stringa.Contains("<TITOLO>") And RMain Then
                    posIni = stringa.IndexOf("<TITOLO>") + 8
                    posFin = stringa.IndexOf("</TITOLO>")
                    If posFin - posIni > 0 Then
                        Me.Text = Trim(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<QUERY>") And RMain Then
                    posIni = stringa.IndexOf("<QUERY>") + 7
                    posFin = stringa.IndexOf("</QUERY>")
                    If posFin - posIni > 0 Then
                        Me.MainQuery = Trim(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<LARGHEZZA>") And RMain Then
                    posIni = stringa.IndexOf("<LARGHEZZA>") + 11
                    posFin = stringa.IndexOf("</LARGHEZZA>")
                    If posFin - posIni > 0 Then
                        Me.Width = CInt(stringa.Substring(posIni, posFin - posIni))
                        Me.grihlp.Width = Me.Width - Me.GapForm + 3
                    End If
                    Continue While
                End If
                If stringa.Contains("<ALTEZZA>") And RMain Then
                    posIni = stringa.IndexOf("<ALTEZZA>") + 9
                    posFin = stringa.IndexOf("</ALTEZZA>")
                    If posFin - posIni > 0 Then
                        Me.Height = CInt(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<NOMECHIAVE>") And RMain Then
                    posIni = stringa.IndexOf("<NOMECHIAVE>") + 12
                    posFin = stringa.IndexOf("</NOMECHIAVE>")
                    If posFin - posIni > 0 Then
                        Me.lb_cerca.Text = Trim(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<CHIAVE>") And RMain Then
                    posIni = stringa.IndexOf("<CHIAVE>") + 8
                    posFin = stringa.IndexOf("</CHIAVE>")
                    If posFin - posIni > 0 Then
                        Me.lbChiave.Text = Trim(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("</MAIN>") Then
                    RMain = False
                    Continue While
                End If
                If stringa.Contains("<COLONNA>") Then
                    RColonna = True
                    Continue While
                End If
                If stringa.Contains("<INDICE>") And RColonna Then
                    posIni = stringa.IndexOf("<INDICE>") + 8
                    posFin = stringa.IndexOf("</INDICE>")
                    If posFin - posIni > 0 Then
                        Indice = CInt(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<TESTO>") And RColonna Then
                    posIni = stringa.IndexOf("<TESTO>") + 7
                    posFin = stringa.IndexOf("</TESTO>")
                    If posFin - posIni > 0 Then
                        grihlp.Columns.Item(Indice).HeaderText = Trim(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<LUNGHEZZA>") And RColonna Then
                    posIni = stringa.IndexOf("<LUNGHEZZA>") + 11
                    posFin = stringa.IndexOf("</LUNGHEZZA>")
                    If posFin - posIni > 0 Then
                        grihlp.Columns.Item(Indice).Width = CInt(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("<VISIBILE>") And RColonna Then
                    posIni = stringa.IndexOf("<VISIBILE>") + 10
                    posFin = stringa.IndexOf("</VISIBILE>")
                    If posFin - posIni > 0 Then
                        grihlp.Columns.Item(Indice).Visible = Convert.ToBoolean(stringa.Substring(posIni, posFin - posIni))
                    End If
                    Continue While
                End If
                If stringa.Contains("</COLONNA>") Then
                    RColonna = False
                    Continue While
                End If
            End While
            filereader.Close()
            sr.Close()
        Catch ex As IO.FileNotFoundException
        End Try

    End Sub
    Public ReadOnly Property ValRet() As ValoreRitorno
        Get
            Try
                Dim ic As New ValoreRitorno
                If testoSel.Trim = "" Then
                    ic.valore = grihlp.Rows.Item(grihlp.SelectedCells.Item(0).RowIndex).Cells(0).Value
                Else
                    ic.valore = testoSel.Trim
                End If
                Return ic
            Catch ex As SystemException
                MsgBox(ex.Message)
            End Try
        End Get
    End Property
    Private Sub grihlp_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grihlp.DoubleClick
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub txtCerca_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCerca.KeyPress
        If e.KeyChar = vbCr Then
            carica_griglia()
        End If
    End Sub

    Private Sub grihlp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles grihlp.KeyPress

        If e.KeyChar = ControlChars.Cr Then
            e.Handled = False
        End If
    End Sub

    Private Sub grihlp_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles grihlp.PreviewKeyDown
        If e.KeyCode = Keys.Enter Then
            testoSel = grihlp.Rows.Item(grihlp.SelectedCells.Item(0).RowIndex).Cells(0).Value
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
        If e.KeyCode = Keys.F9 Then
            Dim formCall As Form = Globale.getFormByName(Me.zoomOnzoom, MDIParent1)
            formCall.Show()
            'Me.carica_griglia()
            'Me.Refresh()
        End If
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub txtCerca_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles txtCerca.PreviewKeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnAggiorna_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAggiorna.Click
        Me.carica_griglia()
        Me.Refresh()
    End Sub
End Class

Public Class ValoreRitorno
    Public valore As String

End Class
