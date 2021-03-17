Imports System
Imports System.io
Imports System.Net
Imports System.Net.Sockets
Imports System.Globalization
Imports System.Text
Imports Microsoft.VisualBasic
Imports iTextSharp
Imports iTextSharp.text.pdf
Imports System.Xml


Public Class frmCopt
    Dim cf As New CConfig
    Dim op As New COpeFile
    Public Structure sKey
        Dim key1 As String
        Dim key2 As String
        Dim key3 As String
        Dim key1_o As String 'Contains the old key before modify
        Dim key2_o As String 'Contains the old key before modify
        Dim key3_o As String 'Contains the old key before modify 
        Dim operation As String
    End Structure
    Public Structure sManifest
        Dim hFields As Hashtable
        Dim hTargetFields As Hashtable
        Dim hkeys As Hashtable
        Dim hTargetKeys As Hashtable
        Dim mappingTable As String
        Dim queryString As String
        Dim queryAll As String
    End Structure
    Public _Container As New Hashtable ' Contains a list of Manifest entity for each table
    Dim _query As String
    Dim _hOperat As New Hashtable
    Protected gLogFile As String = "CoptOrd2011.log"
    Protected hLogFile As FileStream
    Dim _g_auto As Boolean
    Dim _g_end As Boolean = False

    Private Sub frmCopt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        op.imposta_connessione(Globale.ConnectionString)
        CAdhocDocVar.g_PlanTable = "copt\plan_tables.xml"
        CAdhocDocVar.g_AdhocAzi = Globale.CodAzi
        adhoc = New CAdhocDoc(CAdhocDocVar.g_PlanTable, op, CAdhocDocVar.g_AdhocAzi, "SQLSERVER")
        'adhoc.loadTableFromXml("copt\banche.xml", "BAN_CHE", True)
        adhoc.aTableList.Sort()
        If My.Application.CommandLineArgs.Count > 0 Then
            Dim a As String = My.Application.CommandLineArgs.Item(0)
            If a.ToUpper = "AUTO" Then
                _g_auto = True
                Me.fExport()
                While _g_end = False
                End While
                Application.Exit()
            Else
                _g_auto = False
            End If
        End If


    End Sub
    Private Sub addTableToExport(ByVal pTable As String, ByVal pComment As String)

        Try
            lstToExp.BeginUpdate()
            Dim item As New ListViewItem(pTable)
            item.SubItems.Add(pComment)
            lstToExp.Items.Add(item)
            lstToExp.EndUpdate()
            '
        Catch ex As System.SystemException
        End Try

    End Sub
    Private Sub addExportedTable(ByVal pTable As String, ByVal pComment As String)

        Try
            lstExported.BeginUpdate()
            Dim item As New ListViewItem(pTable)
            item.SubItems.Add(pComment)
            lstExported.Items.Add(item)
            lstExported.EndUpdate()
        Catch ex As System.SystemException
        End Try

    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Try
            Me.fExport()
        Catch ex As Exception

        End Try
    End Sub
    Private Function fExport() As Integer
        Try
            Dim _hTemp As New Hashtable
            Dim _manifest As New sManifest
            Dim _keys As New sKey
            Dim __keys As New sKey
            Dim _aOrd As New ArrayList
            Dim _key As String = ""
            If _g_auto Then
                Me.WriteLog("Start process - " & Now)
            End If
            _g_end = False
            If chkAll.Checked Then
                'export all record
                fexportAll(_aOrd, _hTemp)
            Else
                'export only updated record
                Dim strsql As String = "select numrec, codice, codsec, codter, codice_o, codsec_o, codter_o, tabella, operazione from aggiornamenti order by tabella, codice"
                Dim ds As DataSet = op.esegui_query(strsql)
                For Each _row As DataRow In ds.Tables(0).Rows
                    _key = _row.Item("tabella").ToString.Trim & "#" & _row.Item("codice") & "#" & _row.Item("codsec") & "#" & _row.Item("codter") & "#" & _row.Item("operazione")
                    With _keys
                        .key1 = _row.Item("codice").ToString.Trim
                        .key2 = _row.Item("codsec").ToString.Trim
                        .key3 = _row.Item("codter").ToString.Trim
                        .key1_o = _row.Item("codice_o").ToString.Trim
                        .key2_o = _row.Item("codsec_o").ToString.Trim
                        .key3_o = _row.Item("codter_o").ToString.Trim
                        .operation = _row.Item("operazione").ToString.Trim
                    End With
                    strsql = "delete from aggiornamenti where numrec = " & op.ValAdapter(_row.Item("numrec"), TipoCampo.TLong)
                    op.esegui_query(strsql)
                    If _hTemp.ContainsKey(_key) Then
                        Continue For
                    End If
                    _aOrd.Add(_key)
                    _hTemp.Add(_key, _keys)
                Next
            End If
            'Creates update file
            Dim stringa As String = ""
            Dim sw As System.IO.FileStream
            'Make file name by date and time
            Dim DateAndTime As String = Now.Year & Now.Month.ToString.PadLeft(2, "0") & Now.Day.ToString.PadLeft(2, "0") & Now.Hour.ToString.PadLeft(2, "0") & Now.Second.ToString.PadLeft(2, "0")
            Dim fileexp As String = "xml\\" & DateAndTime & ".xml"
            Dim filerar As String = "xml\\" & DateAndTime & ".rar"
            '
            sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            'Writes header file
            stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = "<dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            If Me.chkAll.Checked Then
                'insert here price list delete command 
                '
                If Me.chkListini.Checked Then
                    Dim _sqlString As String = "delete from palmlistinim"
                    stringa = Chr(9) & "<command>" & _sqlString & "</command>" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    _sqlString = "delete from palmlistinid"
                    stringa = Chr(9) & "<command>" & _sqlString & "</command>" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                End If
                '
            End If
            '
            For Each _element As String In _aOrd
                Me.txtRiga.Text = _element
                Me.txtRiga.Refresh()
                Dim _table As String = _element.Split("#").Clone(0).ToString.Trim
                _keys = _hTemp(_element)
                Dim _codice As String = _element.Split("#").Clone(1).ToString.Trim
                _manifest = New sManifest
                If Not _Container.ContainsKey(_table) Then
                    _manifest = Me.readManifest(_table)
                Else
                    _manifest = _Container(_table)
                End If
                Select Case _keys.operation
                    Case "UPDATE"
                        If _manifest.mappingTable <> "" Then
                            Me.writeXmlUpd(_table, filewriter, _manifest.queryString, _manifest, _keys)
                            Application.DoEvents()
                            If Not IsNothing(_keys.key1_o) Then
                                If _keys.key1_o.Trim() <> "" Then
                                    If _keys.key1 <> _keys.key1_o Or _keys.key2 <> _keys.key2_o Or _keys.key3 <> _keys.key3_o Then
                                        __keys.key1 = _keys.key1_o 'WriteXmlDel function look at only the first three keys, so i must fill up key1, key2 and key3   
                                        __keys.key2 = _keys.key2_o 'with : key1_o, key2_o and key3_o
                                        __keys.key3 = _keys.key3_o
                                        __keys.operation = "DELETE"
                                        Me.writeXmlDel(_table, filewriter, _manifest.queryString, _manifest, __keys)
                                    End If
                                End If
                            End If
                        End If
                    Case "DELETE"
                            Me.writeXmlDel(_table, filewriter, _manifest.queryString, _manifest, _keys)
                            Application.DoEvents()
                End Select
                '
            Next
            'put Here the call to export week-discount function
            If Globale.g_SysData.DayOfWeek >= 4 Or Me.chkFasce.Checked Then
                Me.ExpWeekDiscount(filewriter)
                'Me.expRoute(filewriter)
            End If
            '
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
            sw.Close()
            '
            Me.txtRiga.Text = "Compressing file..."
            Me.txtRiga.Refresh()
            Me.AddFileToRarArchive(filerar, fileexp)
            Me.txtRiga.Text = "Done!"
            Me.txtRiga.Refresh()
            '
            If chkNoftp.Checked = False Then
                ftpExport()
            End If
            If _g_auto Then
                Me.WriteLog("End process - " & Now)
            Else
                MsgBox("done!")
            End If
            _g_end = True
            Return 1
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - fexport()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information)
            End If
            Return 0
        End Try

    End Function
    Private Function fexportAll(ByRef _aOrd As ArrayList, ByRef _hTemp As Hashtable) As Boolean

        Try
            Dim _keys As New sKey
            Dim _key As String = ""
            Dim _manifest As New sManifest
            Dim _hashTable As New Hashtable
            Dim _column As New DataColumn
            _hashTable = Me.ReadManifestTables

            For Each element As String In _hashTable.Keys
                _manifest = Me.readManifest(_hashTable(element))
                _query = _manifest.queryAll
                Dim ds As DataSet = op.esegui_query(_manifest.queryAll)
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each _row As DataRow In ds.Tables(0).Rows
                        '
                        _key = _hashTable(element).ToString.Trim & "#" & _row(0)
                        Try
                            _key = _key & "#" & _row(1)
                            _key = _key & "#" & _row(2)
                        Catch ex As System.Exception
                        End Try
                        If Not _hTemp.ContainsKey(_key) Then
                            _aOrd.Add(_key)
                            '
                            Dim _ii As Integer = 1
                            For Each _column In ds.Tables(0).Columns
                                Select Case _ii
                                    Case 1
                                        _keys.key1 = CTran(_row(_column.Caption), "")
                                    Case 2
                                        _keys.key2 = CTran(_row(_column.Caption), "")
                                    Case 3
                                        _keys.key3 = CTran(_row(_column.Caption), "")
                                End Select
                                _ii += 1
                            Next
                            '
                            _keys.operation = "UPDATE"
                            _hTemp.Add(_key, _keys)
                        End If
                        '
                    Next
                End If
            Next
            '
            Return True
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & "-" & _query & " - fexportAll()")
            Else
                MsgBox(ex.Message & "-" & _query, MsgBoxStyle.Information, "fexportAll")
            End If
            Return False
        End Try

    End Function
    ''' <summary>
    ''' It return a hashtable that contains mapping table-name as key and adhoc table-name for value 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadManifestTables() As Hashtable


        Try
            Dim XmlNodo As Xml.XmlNodeList
            Dim manifestName As String = "updateManifest.xml"
            Dim _tempH As New Hashtable
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(manifestName)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)

            XmlNodo = Xmlfile.GetElementsByTagName("table")
            For Each nodo As XmlNode In XmlNodo
                If nodo.ChildNodes(1).InnerText.Trim <> "" Then
                    If _hOperat.Count > 0 Then
                        If _hOperat.ContainsKey(nodo.ChildNodes(0).InnerText) Then
                            _tempH.Add(nodo.ChildNodes(1).InnerText, nodo.ChildNodes(0).InnerText) 'Mapping tablename(adhoc), tablename (gesordcopt)
                        End If
                    Else
                        _tempH.Add(nodo.ChildNodes(1).InnerText, nodo.ChildNodes(0).InnerText) 'Mapping tablename(adhoc), tablename (gesordcopt)
                    End If
                End If
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return _tempH
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - ReadManifestTable()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "ReadManifestTable()")
            End If

        End Try

    End Function
    Private Function writeXmlDel(ByVal ptablename As String, ByVal filewriter As System.IO.StreamWriter, ByVal pquerystring As String, ByVal pManifest As sManifest, ByVal pKeys As sKey) As Boolean

        Try
            '
            '
            Dim stringa As String = ""
            stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<tablename>" & ptablename & "</tablename>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<operation>" & pKeys.operation & "</operation>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
            With pManifest
                Dim _fieldvalue As String = ""
                For Each _field As String In .hTargetKeys.Keys
                    Select Case _field
                        Case "1"
                            _fieldvalue = pKeys.key1
                        Case "2"
                            _fieldvalue = pKeys.key2
                        Case "3"
                            _fieldvalue = pKeys.key3
                    End Select
                    If .hTargetKeys(_field).ToString.Trim = "LSQUANTI" Then
                        _fieldvalue = _fieldvalue.Replace(".", ",")
                    End If
                    stringa = Chr(9) & Chr(9) & Chr(9) & "<" & .hTargetKeys(_field).ToString.Trim & ">" & adhoc.htmlEncode(_fieldvalue.ToString.Trim) & "</" & .hTargetKeys(_field).ToString.Trim & ">" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                Next
            End With
            '
            stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - writeXmlDelete()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "writeXmlDelete")
            End If
        End Try


    End Function
    Private Function writeXmlUpd(ByVal ptablename As String, ByVal filewriter As System.IO.StreamWriter, ByVal pquerystring As String, ByVal pManifest As sManifest, ByVal pKeys As sKey) As Integer

        Try
            '
            If ptablename = "CONT_SCA" Then
                Dim a As Integer = "1"
            End If
            Dim _queryString As String = pquerystring.ToUpper
            Dim _criteriaString As String = ""
            If _queryString.Contains("WHERE") Then
                _criteriaString = " AND " & pManifest.hkeys("1") & " = " & op.ValAdapter(pKeys.key1, TipoCampo.TChar)
            Else
                _criteriaString = " WHERE " & pManifest.hkeys("1") & " = " & op.ValAdapter(pKeys.key1, TipoCampo.TChar)
            End If
            If pManifest.hkeys("2") <> "" Then
                _criteriaString = _criteriaString & " AND " & pManifest.hkeys("2") & " = " & op.ValAdapter(pKeys.key2, TipoCampo.TChar)
            End If
            If pManifest.hkeys("3") <> "" Then
                _criteriaString = _criteriaString & " AND " & pManifest.hkeys("3") & " = " & op.ValAdapter(pKeys.key3, TipoCampo.TCur)
            End If
            '
            Dim stringa As String = ""
            _queryString = _queryString & _criteriaString
            Dim dsArt As DataSet = op.esegui_query(_queryString)
            For Each _row As DataRow In dsArt.Tables(0).Rows
                '
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>" & ptablename & "</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>" & pKeys.operation & "</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                With pManifest
                    For Each _field As String In .hFields.Keys
                        stringa = Chr(9) & Chr(9) & Chr(9) & "<" & .hFields(_field) & ">" & adhoc.htmlEncode(_row.Item(.hFields(_field)).ToString.Trim) & "</" & .hFields(_field) & ">" & Chr(13) & Chr(10)
                        filewriter.Write(stringa)
                        filewriter.Flush()
                    Next
                End With
                '
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
            Next
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - writeXmlUpload()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "writeXmlUpload")
            End If
        End Try

    End Function
    Private Function readManifest(ByVal pTablename As String) As sManifest

        Try
            Dim _manifest As New sManifest
            Dim XmlNodo As Xml.XmlNodeList
            Dim manifestName As String = "updateManifest.xml"
            Dim _source As String = ""
            Dim _target As String = ""
            _manifest.hFields = New Hashtable
            _manifest.hkeys = New Hashtable
            _manifest.htargetFields = New Hashtable
            _manifest.hTargetKeys = New Hashtable
            _manifest.mappingTable = ""
            _manifest.queryString = ""
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(manifestName)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)

            XmlNodo = Xmlfile.GetElementsByTagName("table")
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    Dim name As String = element.Name
                    Dim value As String = element.InnerText
                    If name = "tablename" And value <> ptablename Then
                        Exit For
                    End If
                    If name = "mappingtable" Then
                        _manifest.mappingTable = value
                    End If
                    If name = "queryfile" Then
                        _manifest.queryString = op.getQuery(CAdhocDocVar.g_AdhocAzi, value)
                    End If
                    If name = "queryall" Then
                        _manifest.queryAll = op.getQuery(CAdhocDocVar.g_AdhocAzi, value)
                    End If
                    If name = "fields" Then
                        For Each _childNode As XmlNode In element
                            '
                            Try
                                With _manifest
                                    _source = _childNode.InnerText.Split("|")(0).Trim
                                    _target = _childNode.InnerText.Split("|")(1).Trim
                                    If Not .hFields.ContainsKey(_source) And _source.ToString.Trim <> "" Then
                                        .hFields.Add(_source, _source)
                                    End If
                                    If Not .hTargetFields.ContainsKey(_target) And _target.ToString.Trim <> "" Then
                                        .hTargetFields.Add(_target, _target)
                                    End If
                                End With
                            Catch ex As Exception
                                MsgBox(ex.Message, MsgBoxStyle.Information, "Fill fields hashtable")
                            End Try
                            '
                        Next
                    End If
                    '
                    If name = "keys" Then
                        Dim ii As Integer = 1
                        For Each _childNode As XmlNode In element
                            '
                            Try
                                With _manifest
                                    If _childNode.InnerText.Trim <> "" Then
                                        _source = _childNode.InnerText.Split("|")(0).Trim
                                        _target = _childNode.InnerText.Split("|")(1).Trim
                                        If Not .hkeys.ContainsKey(_source) And _source.ToString.Trim <> "" Then
                                            .hkeys.Add(ii.ToString.Trim, _source)
                                            .hTargetKeys.Add(ii.ToString.Trim, _target)
                                            ii = ii + 1
                                        End If
                                        'If Not .hTargetKeys.ContainsKey(_target) And _target.ToString.Trim <> "" Then
                                        '.hTargetKeys.Add(ii.ToString.Trim, _target)
                                        'End If
                                    End If
                                End With
                            Catch ex As Exception
                                MsgBox(ex.Message, MsgBoxStyle.Information, "Fill fields hashtable")
                            End Try
                            '
                        Next
                    End If
                Next
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            With _manifest
                If .queryString = "" Then
                    Dim strsql As String = "select "
                    For Each element As String In .hFields.Keys
                        Dim _value As String = .hFields(element)
                        strsql = strsql & _value & ","
                    Next
                    strsql = strsql.TrimEnd(",") & " from " & adhoc.getTablename(.mappingTable)
                    .queryString = strsql
                End If
            End With
            Return _manifest

        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - readManifest()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Read Manifest")
            End If
        End Try

    End Function

    Private Function AddFileToRarArchive(ByVal pCurrentZip As String, ByVal filePath As String) As Boolean

        Try
            Dim _taskInfo As New ProcessStartInfo
            Dim _ds As New Diagnostics.Process
            '
            _taskInfo.FileName = "rar.exe"
            _taskInfo.Arguments = "a -ep " & pCurrentZip & " " & filePath
            _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
            _ds = Process.Start(_taskInfo)
            'wait for end process
            While Not _ds.HasExited
            End While
            Dim a As String = _ds.ExitCode
            IO.File.Delete(filePath)
            Return True
        Catch ex As System.Exception
            If _g_auto Then
                Me.WriteLog("Error in function AddFileToRarArchive while compacting " & filePath & " ,specific error : " & ex.Message)
            Else
                MsgBox("Error in function AddFileToRarArchive while compacting " & filePath & " ,specific error : " & ex.Message)
            End If

        End Try
    End Function

    Private Sub setTable(sender As System.Object, e As System.EventArgs) Handles chkClienti.CheckedChanged, chkArticoli.CheckedChanged, chkListini.CheckedChanged, chkSaldi.CheckedChanged, chkSedi.CheckedChanged, chkContratti.CheckedChanged
        Try
            If sender.checked Then
                Me.chkAll.Checked = True
                Select Case sender.name.ToString.ToLower
                    Case "chkclienti"
                        _hOperat.Add("CLIENTI", "CLIENTI")
                    Case "chklistini"
                        _hOperat.Add("LISTINIM", "LISTINIM")
                        _hOperat.Add("LISTINI", "LISTINI")
                    Case "chkarticoli"
                        _hOperat.Add("ARTICOLI", "ARTICOLI")
                        _hOperat.Add("KEYARTI", "KEYARTI")
                    Case "chksaldi"
                        _hOperat.Add("SALDIART", "SALDIART")
                    Case "chksedi"
                        _hOperat.Add("DES_DIVE", "DES_DIVE")
                    Case "chksedi"
                        _hOperat.Add("CON_TRAM", "CON_TRAM")
                        _hOperat.Add("CON_TRAD", "CON_TRAD")
                        _hOperat.Add("CONT_SCA", "CONT_SCA")
                End Select
            Else
                Select Case sender.name.ToString.ToLower
                    Case "chkclienti"
                        _hOperat.Remove("CLIENTI")
                    Case "chklistini"
                        _hOperat.Remove("LISTINIM")
                        _hOperat.Remove("LISTINI")
                    Case "chkarticoli"
                        _hOperat.Remove("ARTICOLI")
                        _hOperat.Remove("KEYARTI")
                    Case "chksaldi"
                        _hOperat.Remove("SALDIART")
                    Case "chksedi"
                        _hOperat.Remove("DES_DIVE")
                    Case "chkcontratti"
                        _hOperat.Remove("CON_TRAM")
                        _hOperat.Remove("CON_TRAD")
                        _hOperat.Remove("CONT_SCA")
                End Select
                If _hOperat.Count = 0 Then
                    Me.chkAll.Checked = False
                End If
            End If
            Dim _e As Object = e.ToString
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "setTable")
        End Try

    End Sub
    Private Sub ftpExport()
        Dim Grandezza As Long
        Dim ff As clsFTP
        lbattendi.Visible = True
        lbattendi.Refresh()
        Try
            'Pass values to the constructor. These values can be overridden by setting
            'the appropriate properties on the instance of the clsFTP class.
            'The third parameter is the user name. The FTP site is accessed with the user name.
            'If there is no specific user name, the user name can be anonymous.
            'The fourth parameter is the password. The FTP server is accessed with the password.
            'The fifth parameter is the port of the FTP server. The port of the FTP server is typically 21.

            lbattendi.Visible = True
            lbattendi.Refresh()
            If Directory.Exists(Globale.cartellaAggLocale) = False Then
                Directory.CreateDirectory(Globale.cartellaAggLocale)
            End If
            Dim _array As ArrayList = Me.readRemoteFolder()
            Dim _fileList As ArrayList = Me.ElencaFileAgg()
            Dim _localFold As New DirectoryInfo(Globale.cartellaAggLocale)
            If IsNothing(_array) Then
                Exit Sub
            End If
            If IsNothing(_fileList) Then
                Exit Sub
            End If
            '
            For Each _vendor As String In _array
                ff = New clsFTP(FtpServer, _
                                "/" & RemoteFolder & "/", _
                                FtpUser, _
                                FtpPwd, _
                                21)
                'Try to log on to the FTP server.
                If (ff.Login() = True) Then
                    '
                    'Change the directory on your FTP site.
                    If (ff.ChangeDirectory("/" & RemoteFolder & "/" & _vendor) = False) Then
                        'Successful changing the directory
                        If _g_auto Then
                            Me.WriteLog("Cambio directory fallito - ftpExport()")
                        Else
                            MsgBox("Cambio directory fallito", MsgBoxStyle.Critical, "ftpExport()")
                        End If
                        Continue For
                    End If
                    '

                    For Each _file As String In _fileList
                        ff.SetBinaryMode(True)
                        ProgressBar2.Maximum = _fileList.Count
                        ProgressBar2.Minimum = 0
                        ProgressBar2.Value = 0
                        lbAzione.Text = "Download dei files..."
                        lbAzione.Refresh()
                        Dim _fi As FileInfo() = _localFold.GetFiles(_file)
                        Grandezza = _fi.Length
                        lb_grandezza.Text = Grandezza
                        lb_grandezza.Refresh()
                        lb_progress.Text = 0
                        lb_progress.Refresh()
                        If (ff.UploadFile(Globale.cartellaAggLocale & "\" & _file, lb_progress) = True) Then
                        End If
                    Next

                End If
            Next
            DeleteFileAgg()
        Catch ex As System.Exception
            'Display the error message.
            If _g_auto Then
                Me.WriteLog("Specific Error=" & ex.Message + vbCrLf + "Press 'ENTER' to EXIT" & " - ftpExport()")
            Else
                MsgBox("Specific Error=" & ex.Message + vbCrLf + "Press 'ENTER' to EXIT")
            End If

        Finally
            'Always close the connection to make sure that there are not any not-in-use FTP connections.
            'Check if you are logged on to the FTP server and then close the connection.
            If Not IsNothing(ff) Then
                If ff.flag_bool = True Then
                    ff.CloseConnection()
                End If
            End If
            lbAzione.Text = ""
            lbAzione.Refresh()
            lbattendi.Visible = False
            lbattendi.Refresh()
            '
            lb_grandezza.Text = ""
            lb_grandezza.Refresh()
            lb_progress.Text = ""
            lb_progress.Refresh()

            '

            ProgressBar2.Value = 0
            ProgressBar2.Refresh()
        End Try

    End Sub
    Private Function readRemoteFolder() As ArrayList

        Try
            Dim _manifest As New sManifest
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim folderFile As String = "remoteFolder.xml"
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(folderFile)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)

            XmlNodo = Xmlfile.GetElementsByTagName("folder")
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    Dim name As String = element.Name
                    Dim value As String = element.InnerText
                    If name = "name" Then
                        If Not _array.Contains(value) Then
                            _array.Add(value)
                        End If
                    End If
                Next
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return _array
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - readRemoteFolder()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Read Remote Folder")
            End If
            Return Nothing
        End Try

    End Function
    Private Function ElencaFileAgg() As ArrayList

        Try
            Dim _array As New ArrayList
            If Directory.Exists(Globale.cartellaAggLocale) = False Then
                Directory.CreateDirectory(Globale.cartellaAggLocale)
            End If
            Dim di As New DirectoryInfo(Globale.cartellaAggLocale)
            Dim fi As FileInfo
            Try
                For Each fi In di.GetFiles()
                    Dim item As New ListViewItem(fi.Name)
                    _array.Add(fi.Name)
                Next
            Catch ex As SystemException
                If _g_auto Then
                    Me.WriteLog(ex.Message & " - ElencaFileAgg()")
                Else
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation, "ElencaFileAgg")
                End If
            Finally
                di = Nothing
                fi = Nothing
            End Try
            Return _array
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Elenca file")
            Return Nothing
        End Try
    End Function
    Private Function DeleteFileAgg() As ArrayList

        Try
            Dim _array As New ArrayList
            If Directory.Exists(Globale.cartellaAggLocale) = False Then
                Directory.CreateDirectory(Globale.cartellaAggLocale)
            End If
            Dim di As New DirectoryInfo(Globale.cartellaAggLocale)
            Dim fi As FileInfo
            Try
                For Each fi In di.GetFiles()
                    Dim item As New ListViewItem(fi.Name)
                    IO.File.Delete(Globale.cartellaAggLocale & "\" & fi.Name)
                Next
            Catch ex As SystemException
                If _g_auto Then
                    Me.WriteLog(ex.Message & " - DeleteFileAgg()")
                Else
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation, "DeleteFileAgg")
                End If
            Finally
                di = Nothing
                fi = Nothing
            End Try
            Return _array
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - DeleteFileAgg()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "DeletefileAgg")
            End If
            Return Nothing
        End Try
    End Function
    Private Function ExpWeekDiscount(ByVal filewriter As System.IO.StreamWriter) As Boolean

        Try
            Dim stringa As String = ""
            Dim _amount As Decimal = 0
            Dim _prevYear As String = (Now.Date.Year - 1).ToString
            Dim _nextWeek As Integer = 0
            If Me.chkFasce.Checked Then
                _nextWeek = DatePart(DateInterval.WeekOfYear, Date.Now)
            Else
                _nextWeek = DatePart(DateInterval.WeekOfYear, Date.Now) + 1
            End If
            Dim _sqlString As String = "select oscodcli from " & adhoc.getTablename("ordsett") & _
                                       " a inner join " & adhoc.getTablename("asysfasce") & " b" & _
                                       " on a.oscodcli = b.facodcli" & _
                                       " where a.osnumset = " & op.ValAdapter(_nextWeek, TipoCampo.TInt) & _
                                       " and a.oscodese = " & op.ValAdapter(_prevYear, TipoCampo.TChar) & _
                                       " and a.oscodese  = " & op.ValAdapter(_prevYear, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(_sqlString)
            For Each _row As DataRow In ds.Tables(0).Rows
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>FASCE</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>UPDATE</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                ' 4 and a.oscodcli = '0002612' order by osimpord
                'If _row.Item("oscodcli").ToString.Trim = "0004748" Then
                'Dim a As Integer = 1
                'End If
                _sqlString = "select sum(osimpord) as importo, max(faaument) as aument, max(fasconto) as sconto, max(fadatval) as datval, max(fadtobso) as dtobso from " & _
                             adhoc.getTablename("ordsett") & " a inner join " & adhoc.getTablename("asysfasce") & " b" & _
                             " on a.oscodcli = b.facodcli where a.osnumset = " & op.ValAdapter(_nextWeek, TipoCampo.TInt) & " and a.oscodese = " & op.ValAdapter(_prevYear, TipoCampo.TChar) & _
                             " and a.oscodcli = " & op.ValAdapter(_row.Item("oscodcli"), TipoCampo.TChar)
                Dim ds1 As DataSet = op.esegui_query(_sqlString)
                For Each _row1 As DataRow In ds1.Tables(0).Rows
                    _amount = CTran(_row1.Item("importo"), 0) * (1 + CTran(_row1.Item("AUMENT"), 0) / 100)
                    If _amount > 0 Then
                        stringa = Chr(9) & Chr(9) & Chr(9) & "<FACODCLI>" & adhoc.htmlEncode(_row.Item("OSCODCLI")) & "</FACODCLI>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FANUMSET>" & adhoc.htmlEncode(_nextWeek) & "</FANUMSET>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FACODESE>" & adhoc.htmlEncode(_prevYear) & "</FACODESE>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FAIMPORT>" & adhoc.htmlEncode(_amount) & "</FAIMPORT>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FASCONTO>" & adhoc.htmlEncode(_row1.Item("SCONTO")) & "</FASCONTO>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FADATINI>" & adhoc.htmlEncode(_row1.Item("DATVAL")) & "</FADATINI>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FADATFIN>" & adhoc.htmlEncode(_row1.Item("DTOBSO")) & "</FADATFIN>" & Chr(13) & Chr(10) & _
                                  Chr(9) & Chr(9) & Chr(9) & "<FAORDIMP>" & adhoc.htmlEncode(_row1.Item("IMPORTO")) & "</FAORDIMP>" & Chr(13) & Chr(10)

                        filewriter.Write(stringa)
                        filewriter.Flush()
                    End If
                Next
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next

        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - ExpWeekDiscount()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "ExpWeekDiscount()")
            End If
        End Try

    End Function
    Private Function expRoute(ByVal filewriter As System.IO.StreamWriter) As Boolean

        Try
            Dim _string As String = ""
            Dim _sqlString As String = op.getQuery(CAdhocDocVar.g_AdhocAzi, "EXPGIRI.VQR")
            Dim ds As DataSet = op.esegui_query(_sqlString)
            If ds.Tables(0).Rows.Count > 0 Then
                _string = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
                _string = Chr(9) & Chr(9) & "<tablename>ROUTE</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
                _string = Chr(9) & Chr(9) & "<operation>UPDATE</operation>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
                _string = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
            End If
            '
            For Each _row As DataRow In ds.Tables(0).Rows
                '
                _string = Chr(9) & Chr(9) & Chr(9) & "<GRCODCON>" & adhoc.htmlEncode(_row.Item("SECODCON")) & "</GRCODCON>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<GRGGVISIT>" & adhoc.htmlEncode(_row.Item("GRGGVISIT")) & "</GRGGVISIT>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<GRCODAGE>" & adhoc.htmlEncode(_row.Item("ANCODAG1")) & "</GRCODAGE>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<GRSEQUEN>" & adhoc.htmlEncode(_row.Item("SESEQUEN")) & "</GRSEQUEN>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
                '
            Next
            '
            If ds.Tables(0).Rows.Count > 0 Then
                _string = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
                _string = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(_string)
                filewriter.Flush()
            End If
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - expRoute()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "expRoute")
            End If
        End Try

    End Function
    Private Function WriteLog(ByVal pMessage As String) As Boolean
        Try
            'Open log file
            hLogFile = File.Open(gLogFile, FileMode.Append)
            Dim lHeader As String = Date.Now.ToString
            Dim lMessage As String = lHeader & " - " & pMessage & Constants.vbCrLf
            Dim lByteMess As Byte()
            lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
            hLogFile.Write(lByteMess, 0, lMessage.Length)
            Return True
        Catch ex As Exception
            Return False
        Finally
            hLogFile.Close()
            hLogFile = Nothing
        End Try
    End Function

    Private Sub chkSedi_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSedi.CheckedChanged

    End Sub

    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged

    End Sub
End Class