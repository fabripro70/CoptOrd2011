Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Globalization
Imports System.Text
Imports Microsoft.VisualBasic
Imports iTextSharp
Imports iTextSharp.text.pdf
Imports System.Xml
Imports System.Net.Mail
Imports System.Text.RegularExpressions

Public Class frmAdhocSync
    Dim cf As New CConfig
    Dim op As New COpeFile
    Dim wStep As Integer 'Used into fexport and fexportAll functions
    Dim wRunning As Boolean = False 'Used into fexport and fexportAll functions
    Private Structure sListini
        Dim weblicodlis As String
        Dim webcprownum As String
        Dim webliprezzo As Decimal
        Dim weblicodart As String
        Dim webliquanti As Decimal
        Dim webliscont1 As Decimal
        Dim webliscont2 As Decimal
        Dim webliscont3 As Decimal
        Dim webliscont4 As Decimal
        Dim weblidatini As String
        Dim weblidatfin As String
        '
        Dim ahrlicodlis As String
        Dim ahrcprownum As String
        Dim ahrliprezzo As Decimal
        Dim ahrlicodart As String
        Dim ahrliquanti As Decimal
        Dim ahrliscont1 As Decimal
        Dim ahrliscont2 As Decimal
        Dim ahrliscont3 As Decimal
        Dim ahrliscont4 As Decimal
        Dim ahrlidatini As String
        Dim ahrlidatfin As String
        '
    End Structure
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
    Public Structure sSite
        Dim seq As String
        Dim host As String
        Dim folder As String
        Dim user As String
        Dim password As String
        Dim httppost As String
        Dim allItems As String
    End Structure
    Private Structure sOrdini
        Dim ODNUMORD As String
        Dim ODDATORD As String
        Dim ODCODCLI As String
        Dim ODPARIVA As String
        Dim ODCODFIS As String
        Dim ODCODART As String
        Dim ODTIPRIG As String
        Dim ODUNIMIS As String
        Dim ODQTAMOV As String
        Dim ODPREZZO As String
        Dim ODSCONT1 As String
        Dim ODSCONT2 As String
        Dim ODSCONT3 As String
        Dim ODSCONT4 As String
        Dim ODCODDES As String
        Dim ODDATEVA As String
        Dim ODNOTORD As String
        Dim ODNOTAGG As String
        Dim OD_EMAIL As String
        Dim ODSPETRA As String
    End Structure
    Private Structure sClienti
        Dim ANCODICE As String
        Dim ANDESCRI As String
        Dim ANDESCR1 As String
        Dim ANINDIRI As String
        Dim ANINDIR1 As String
        Dim AN___CAP As String
        Dim ANLOCALI As String
        Dim ANPROVIN As String
        Dim ANNAZION As String
        Dim ANTELEFO As String
        Dim ANTELFAX As String
        Dim ANNUMCEL As String
        Dim ANPARIVA As String
        Dim ANCODFIS As String
        Dim ANCODPAG As String
        Dim AN_EMAIL As String
        Dim ANPASSWD As String
    End Structure
    Private Structure sDesdive
        Dim DDCODICE As String
        Dim DDPARIVA As String
        Dim DDCODFIS As String
        Dim DDCODDES As String
        Dim DDDTOBSO As String
        Dim DDNOMDES As String
        Dim DDINDIRI As String
        Dim DD___CAP As String
        Dim DDLOCALI As String
        Dim DDPROVIN As String
        Dim DDTELEFO As String
        Dim DD_EMAIL As String
        Dim DD__NOTE As String
    End Structure
    Private Structure sPardocmast
        Dim serial As String
        Dim cladoc As String
        Dim codcli As String
        Dim numlis As String
        Dim tipcon As String
        Dim speinc As String
        Dim spetra As String
        Dim totdoc As Decimal
        Dim aciva1 As String
        Dim aciva2 As String
        Dim aciva3 As String
        Dim aciva4 As String
        Dim aciva5 As String
        Dim aciva6 As String
        Dim aimpn1 As Decimal
        Dim aimpn2 As Decimal
        Dim aimpn3 As Decimal
        Dim aimpn4 As Decimal
        Dim aimpn5 As Decimal
        Dim aimpn6 As Decimal
        Dim aimps1 As Decimal
        Dim aimps2 As Decimal
        Dim aimps3 As Decimal
        Dim aimps4 As Decimal
        Dim aimps5 As Decimal
        Dim aimps6 As Decimal
        Dim coddes As String
        Dim codpag As String
        Dim desdoc As String
        Dim ordweb As String
        Dim nomepriv As String
        Dim indiripriv As String
        Dim cappriv As String
        Dim localipriv As String
        Dim provinpriv As String
        Dim codfiscpriv As String
        Dim caucon As String
        Dim accont As Decimal
    End Structure
    Private Structure sPardocdett
        Dim serial As String
        Dim cpronum As Integer
        Dim codart As String
        Dim tiprig As String
        Dim qtamov As String
        Dim numlis As String
        Dim clifor As String
        Dim caumag As String
        Dim segno As String
        Dim dateva As Date
        Dim flordi As String
        Dim flimpe As String
        Dim flrise As String
        Dim prezzo As Decimal
        Dim scont1 As Decimal
        Dim scont2 As Decimal
        Dim scont3 As Decimal
        Dim scont4 As Decimal
    End Structure
    Private Structure sVat
        Dim VatCode As String
        Dim VatValue As Decimal
        Dim VatAmount As Decimal
    End Structure
    Private Structure sInstallment
        Dim Inst1 As Decimal
        Dim Inst2 As Decimal
        Dim Inst3 As Decimal
        Dim Inst4 As Decimal
    End Structure
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
    Private Structure sClipriv
        Dim CognomeNome As String
        Dim Indirizzo As String
        Dim Cap As String
        Dim locali As String
        Dim provin As String
        Dim codfis As String
        Dim codpag As String
    End Structure
    Private Structure sDesPriv
        Dim Nomdes As String
        Dim Indirizzo As String
        Dim Cap As String
        Dim locali As String
        Dim provin As String
        Dim codfis As String
    End Structure
    Private Structure sCriterio
        Dim Tabella As String
        Dim Filtro As String
    End Structure
    Private Structure sQuery
        Dim Tabella As String
        Dim Query As String
    End Structure

    Public _Container As New Hashtable ' Contains a list of Manifest entity for each table
    Dim _query As String
    Dim _hOperat As New Hashtable
    Protected gLogFile As String = "AdhocSync.log"
    Protected hLogFile As FileStream
    Protected gLogExpFile As String = "AdhocSyncExport.log"
    Protected hLogExpFile As FileStream
    Dim _g_auto As Boolean 'starts automatically, doesn't show form.
    Dim _g_end As Boolean = False
    Dim _g_all As Boolean = False 'uploads all records, used with _g_auto.
    Dim gCrypt As New CTripleDES
    Dim gSite As New sSite
    Dim gSiteList As New Hashtable
    Dim gKsiteList As New ArrayList
    Dim bFtpGridChanged As Boolean = False
    Dim gSiteIn As New sSite
    Dim gSiteListIn As New Hashtable
    Dim bFtpGridChangedIn As Boolean = False
    Dim gKeyCrypt As String = "KJYTR"
    Dim updfiles As New ArrayList
    Dim gOrd As New Hashtable
    Dim gk_Ord As New ArrayList 'array for sorting gOrd's elements
    Dim gNumOrd As New ArrayList
    Dim gCli As New Hashtable
    Dim gDes As New Hashtable
    Dim gFilter As New sFilter
    Dim gItemsFilter As New ArrayList  'Contains items to upload, if empty uploads all items.
    Dim gTableFilter As New ArrayList 'Contains tables name to upload, if empty it will upload all tables.
    Dim gCriteri As New ArrayList 'Contiene ulteriori ulteriori filtri in strile sql da accodare alla query di ricerca, va riempito con tipo dato sCriterio
    Dim gQuery As New ArrayList 'Contiene la query che deve sostituire quella specificata nel manifest, va riempito con tipo dato sQuery
    Dim gFldProp As New Hashtable 'Contains field's properties values 
    Dim gBodyMail As String = ""
    Dim gBsuccess As Boolean = False
    Dim hCliPriv As New Hashtable 'Hashtable clienti privati
    Dim hDesPriv As New Hashtable 'Hashtable destinazioni clienti privati
    Dim hImages As New Hashtable  'Hashtable contenente le immagini per articolo   16-06-2017

    Private Sub frmAdhocSync_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If bFtpGridChanged Then
                Select Case MsgBox("La sezione onfigurazione è cambiata, vuoi uscire senza salvarla ?", MsgBoxStyle.YesNo, "Exit application")
                    Case MsgBoxResult.No
                        e.Cancel = True
                End Select
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "formClosing")
        End Try
    End Sub
    Private Sub frmCopt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        op.imposta_connessione(Globale.ConnectionString)
        Globale.cn_dbext = Globale.cn
        CAdhocDocVar.g_PlanTable = "Plan\plan_tables.xml"
        CAdhocDocVar.g_AdhocAzi = Globale.CodAzi
        adhoc = New CAdhocDoc(CAdhocDocVar.g_PlanTable, op, CAdhocDocVar.g_AdhocAzi, "SQLSERVER")
        'adhoc.loadTableFromXml("copt\banche.xml", "BAN_CHE", True)
        adhoc.aTableList.Sort()
        Me.lb_tableAgg.Text = Globale.gTableAgg
        Me.readXmlFtpsite()
        Me.loadGrid()
        '
        Me.readXmlFtpsiteIn()
        Me.loadGridIn()
        '
        Me.readCheckHost()
        '
        If gFasce = "S" Then
            Me.chkFasce.Visible = True
        Else
            Me.chkFasce.Visible = False
            Me.chkFasce.Checked = False
        End If
        If My.Application.CommandLineArgs.Count > 0 Then
            Dim a As String = My.Application.CommandLineArgs.Item(0)
            Select Case a.ToUpper
                Case "DUMPVENDUTO"
                    _g_auto = True
                    ExportVendutoMensile()
                Case "DUMPVENDUTOSTO"
                    _g_auto = True
                    ExportVendutoMensileStorico()
                Case "DUMPLISTINIAGENTI"
                    _g_auto = True
                    ExportListiniFullAgenti()
                Case "DUMPLISTINI"
                    _g_auto = True
                    ExportListiniFull()
                    'La funzione ExportListiniFull esce
                    'While _g_end = False
                    'End While
                    'Application.Exit()
                Case "DUMPCONTRATTI"
                    _g_auto = True
                    ExportContrattiFull()
                Case "AUTO"
                    _g_auto = True
                    If My.Application.CommandLineArgs.Count >= 2 Then
                        If My.Application.CommandLineArgs.Item(1) = "ALL" Then
                            _g_all = True
                        End If
                    End If
                    If My.Application.CommandLineArgs.Count >= 2 Then
                        Select Case My.Application.CommandLineArgs.Item(1)
                            Case "SALDIART"
                                _g_all = True
                                _hOperat.Add("SALDIART", "SALDIART")
                            Case "ARTICOLI"
                                _g_all = True
                                _hOperat.Add("ARTICOLI", "ARTICOLI")
                                _hOperat.Add("KEYARTI", "KEYARTI")
                            Case "LISTINI"
                                _g_all = True
                                _hOperat.Add("LISTINIM", "LISTINIM")
                                _hOperat.Add("LISTINI", "LISTINI")
                                _hOperat.Add("CON_TRAM", "CON_TRAM")
                                _hOperat.Add("CON_TRAD", "CON_TRAD")
                                _hOperat.Add("CONT_SCA", "CONT_SCA")
                        End Select
                    End If
                    '
                    Me.floopExport()
                    While _g_end = False
                    End While
                    Application.Exit()
                Case Else
                    _g_auto = False
            End Select
            '
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
            Me.floopExport()
        Catch ex As Exception

        End Try
    End Sub
    Private Function floopExport() As Integer

        Dim _site As New sSite
        Dim _aOrd As New ArrayList
        Dim _hTemp As New Hashtable
        hImages.Clear()
        Try
            Me.getUpdateEl(_aOrd, _hTemp)
            '
            gKsiteList.Sort()
            For Each element As String In gKsiteList

                _site = gSiteList(element)
                Me.lb_sito.Text = _site.host & "/" & _site.folder
                Me.lb_sito.Refresh()
                'Retrieves item to export
                'If _site.allItems = "N" Then
                'getItemToFilter(_site)
                'Else
                'Reset both array filters
                'gItemsFilter = New ArrayList
                'gTableFilter = New ArrayList
                'End If
                '
                getItemToFilter(_site)
                '
                With _site
                    Me.fExport(.host, .folder, .user, .password, _aOrd, _hTemp, _site.seq)
                    If chkNoftp.Checked = False Then
                        Me.httpPost(.httppost)
                    End If
                End With
            Next
            If Not _g_auto Then
                Me.txtRiga.Text = ""
                Me.txtRiga.Refresh()
                Me.lb_grandezza.Text = ""
                Me.lb_grandezza.Refresh()
                Me.lb_progress.Text = ""
                Me.lb_progress.Refresh()
                Me.lbAzione.Text = ""
                Me.lbAzione.Refresh()
                Me.lb_sito.Text = ""
                Me.lb_sito.Refresh()
            End If
            gBsuccess = True
            If _g_auto Then
                Me.WriteLog("Done!")
            Else
                Me.txtRiga.Text = "Operazione terminata!"
                Me.txtRiga.Refresh()
            End If
            If g_emailTo.Trim <> "" Then
                Me.Sendmail("Sincronizzazione shop.copt.it terminata con successo!", gBodyMail)
            End If
        Catch ex As System.Exception
            Me.WriteLog(ex.Message)
            If g_emailTo.Trim <> "" Then
                Me.Sendmail("Sincronizzazione shop.copt.it terminata con anomalie!", gBodyMail)
            End If
        End Try

    End Function
    Private Function getItemToFilter(ByVal pSite As sSite) As Boolean
        Try

            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = "filterGrid.xml"
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument

            'carico il file
            Dim XmlLeggi As New XmlTextReader(_File)

            gItemsFilter = New ArrayList
            gTableFilter = New ArrayList
            gCriteri = New ArrayList
            gQuery = New ArrayList
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
                If gFilter.site.Trim <> pSite.host.ToString.Trim Then
                    Continue For
                End If
                Select Case gFilter.type
                    Case "Articolo"
                        gItemsFilter.Add(gFilter.item)
                    Case "Gruppo merceologico"
                        Dim strsql As String = "select arcodart from " & adhoc.getTablename("ART_ICOL") & " where argrumer = " & op.ValAdapter(gFilter.item, TipoCampo.TChar)
                        Dim ds As DataSet = op.esegui_query(strsql)
                        For Each dr As DataRow In ds.Tables(0).Rows
                            gItemsFilter.Add(dr("arcodart").ToString.Trim)
                        Next
                        ds.Dispose()
                    Case "Famiglia"
                        Dim strsql As String = "select arcodart from " & adhoc.getTablename("ART_ICOL") & " where arcodfam = " & op.ValAdapter(gFilter.item, TipoCampo.TChar)
                        Dim ds As DataSet = op.esegui_query(strsql)
                        For Each dr As DataRow In ds.Tables(0).Rows
                            gItemsFilter.Add(dr("arcodart").ToString.Trim)
                        Next
                        ds.Dispose()
                    Case "Tabella"
                        gTableFilter.Add(gFilter.Tabella)
                    Case "Filtro"
                        Dim _criterio As New sCriterio
                        _criterio.Tabella = gFilter.Tabella
                        _criterio.Filtro = gFilter.Query
                        gCriteri.Add(_criterio)
                    Case "Query"
                        Dim _query As New sQuery
                        _query.Tabella = gFilter.Tabella
                        _query.query = gFilter.Query
                        gQuery.Add(_query)
                End Select
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return True
        Catch ex As IOException
            Return Nothing
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog("getItemToFilter")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Critical, "getItemToFilter")
            End If
        End Try

    End Function
    Private Function getUpdateEl(ByRef _aOrd As ArrayList, ByRef _hTemp As Hashtable) As Integer

        Try
            Dim _keys As New sKey
            Dim _key As String = ""


            If _g_auto Then
                Me.WriteLog("Start process - " & Now)
            End If
            _g_end = False
            If chkAll.Checked Or _g_all Then
                'export all record
                fexportAll(_aOrd, _hTemp)
            Else
                'export only updated record
                'Dim strsql As String = "select numrec, codice, codsec, codter, codice_o, codsec_o, codter_o, tabella, operazione from " & Globale.gTableAgg & " order by tabella, codice"
                Dim strsql As String = "select numrec, codice, codsec, codter, codice_o, codsec_o, codter_o, tabella, operazione from " & Globale.gTableAgg & " order by numrec"
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
                    '
                    If Me.chkMantieni.CheckState = CheckState.Unchecked Then
                        strsql = "delete from " & Globale.gTableAgg & " where numrec = " & op.ValAdapter(_row.Item("numrec"), TipoCampo.TLong)
                        op.esegui_query(strsql)
                    End If
                    If _hTemp.ContainsKey(_key) Then
                        Continue For
                    End If
                    _aOrd.Add(_key)
                    _hTemp.Add(_key, _keys)
                Next
            End If

        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - getUpdateEl()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "getUpdateEl")
            End If
            Return 0
        End Try

    End Function
    Private Function ExportVendutoMensile() As Boolean
        Try




            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump listini" & " - ExportVendutoMensile")

                _site = gSiteList(element)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & _site.seq

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '

                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\expvenduto.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)

                Dim Mese As Integer = Now.Month
                'Dim Mese As Integer = 1
                Dim MeseStr As String = Mese.ToString.PadLeft(2, "0")
                'Dim MeseStr As String = "01"

                Dim ds As DataSet = op.esegui_sp("sp_legge_vendite_adhoc", cn)

                If ds.Tables(0).Rows.Count > 0 Then
                    Dim Anno As String = ds.Tables(0).Rows(0).Item("ANNO").ToString.Trim
                    Dim resultQuery = "DELETE FROM FATTURATO WHERE ANNO = " & op.ValAdapter(Anno, TipoCampo.TChar)
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                End If

                For Each row As DataRow In ds.Tables(0).Rows

                    '
                    Dim pariva As String = ""
                    Dim codfis As String = ""

                    'Aggiungere la lettura della p.iva o cf
                    Dim strqry As String = String.Format("SELECT ANPARIVA, ANCODFIS FROM {0}CONTI WHERE ANTIPCON = 'C' AND ANCODICE = '{1}'", Globale.CodAzi, CTran(row("FBCLIENTE").ToString.Trim, ""))
                    Dim dsCli As DataSet = op.esegui_query(strqry)
                    If dsCli.Tables(0).Rows.Count > 0 Then

                        pariva = CTran(dsCli.Tables(0).Rows(0).Item("ANPARIVA").ToString.Trim, "")
                        codfis = CTran(dsCli.Tables(0).Rows(0).Item("ANCODFIS").ToString.Trim, "")

                    End If
                    Dim ANPIVACF As String = ""
                    If pariva <> "" Then
                        ANPIVACF = pariva
                    Else
                        ANPIVACF = codfis
                    End If
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "01")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("GEN").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "02")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("FEB").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "03")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("MAR").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "04")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("APR").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "05")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("MAG").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "06")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("GIU").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "07")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("LUG").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "08")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("AGO").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "09")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("SETT").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "10")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("OTT").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "11")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("NOV").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", "12")
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("DIC").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()

                    '
                Next
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportListiniFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    Dim lMessage As String = "open ftp://" & _site.user & ":" & _site.password & "@" & _site.host & " -timeout=240" & Constants.vbCrLf &
                                             "put -filemask=""*.sql|*/"" " & "xml/" & _site.seq.TrimEnd("\") & "\*.sql" & Constants.vbCrLf &
                                             "exit" & Constants.vbCrLf
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportVendutoMensile")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportVendutoMensile")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "Xml\" & _site.seq.TrimEnd("\")
                Dim _Movefolder As String = "Xml\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump listini completato!" & " - ExportVendutoMensile")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportVendutoMensile")
            Application.Exit()
        End Try
    End Function

    Private Function ExportVendutoMensileStorico() As Boolean
        Try


            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump listini" & " - ExportVendutoMensile")

                _site = gSiteList(element)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & _site.seq

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '

                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\expvendutostorico.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)

                Dim ds As DataSet = op.esegui_sp("sp_legge_vendite_adhoc_storico", cn)
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim Anno As String = ds.Tables(0).Rows(0).Item("ANNO").ToString.Trim
                    Dim resultQuery = "DELETE FROM FATTURATO WHERE ANNO = " & op.ValAdapter(Anno, TipoCampo.TChar)
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                End If
                For Each row As DataRow In ds.Tables(0).Rows

                    '
                    Dim Mese As Integer = Now.Month

                    Dim pariva As String = ""
                    Dim codfis As String = ""

                    Dim MeseStr As String = Mese.ToString.PadLeft(2, "0")

                    'Aggiungere la lettura della p.iva o cf
                    Dim strqry As String = String.Format("SELECT ANPARIVA, ANCODFIS FROM {0}CONTI WHERE ANTIPCON = 'C' AND ANCODICE = '{1}'", Globale.CodAzi, CTran(row("FBCLIENTE").ToString.Trim, ""))
                    Dim dsCli As DataSet = op.esegui_query(strqry)
                    If dsCli.Tables(0).Rows.Count > 0 Then

                        pariva = CTran(dsCli.Tables(0).Rows(0).Item("ANPARIVA").ToString.Trim, "")
                        codfis = CTran(dsCli.Tables(0).Rows(0).Item("ANCODFIS").ToString.Trim, "")

                    End If
                    Dim ANPIVACF As String = ""
                    If pariva <> "" Then
                        ANPIVACF = pariva
                    Else
                        ANPIVACF = codfis
                    End If
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("01", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("GEN").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("02", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("FEB").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("03", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("MAR").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("04", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("APR").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("05", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("MAG").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("06", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("GIU").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("07", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("LUG").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("08", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("AGO").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("09", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("SETT").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("10", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("OTT").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("11", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("NOV").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANPIVACF", op.ValAdapter(ANPIVACF, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNO", op.ValAdapter(row("ANNO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("MESE", op.ValAdapter("12", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("TOTALE", op.ValAdapter(row("DIC").ToString.Trim, TipoCampo.TCur))
                    resultQuery = op.CreateInsertQuery("FATTURATO", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportListiniFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    Dim lMessage As String = "open ftp://" & _site.user & ":" & _site.password & "@" & _site.host & " -timeout=240" & Constants.vbCrLf &
                                             "put -filemask=""*.sql|*/"" " & "xml/" & _site.seq.TrimEnd("\") & "\*.sql" & Constants.vbCrLf &
                                             "exit" & Constants.vbCrLf
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportVendutoMensile")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportVendutoMensile")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "Xml\" & _site.seq.TrimEnd("\")
                Dim _Movefolder As String = "Xml\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump listini completato!" & " - ExportVendutoMensile")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportVendutoMensile")
            Application.Exit()
        End Try
    End Function
    Private Function ExportArticoliOrreaFull() As Boolean
        Try

            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump Articoli" & " - ExportArticoliOrreaFull")

                _site = gSiteList(element)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & _site.seq

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '
                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\exparticoli_orrea.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)
                '
                'Con_tram
                '
                '
                Dim deleteQuery As String = "delete from ARTICOLI;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                Dim sqlQuery As String = op.getQuery(Globale.CodAzi, "articoli_orrea.vqr")

                Dim ds As DataSet = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ARCODART", op.ValAdapter(row("ARCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARDESART", op.ValAdapter(row("ARDESART").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("ARUNMIS1", op.ValAdapter(row("ARUNMIS1").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARMOLTIP", op.ValAdapter(row("ARMOLTIP").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AROPERAT", op.ValAdapter(row("AROPERAT").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARUNMIS2", op.ValAdapter(row("ARUNMIS2").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARGRUMER", op.ValAdapter(row("ARGRUMER").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("ARCODFAM", op.ValAdapter(row("ARCODFAM").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("AR___IMG", op.ValAdapter(row("AR___IMG").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARDTOBSO", op.ValAdapter(row("ARDTOBSO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARDESSUP", op.ValAdapter(row("ARDESSUP").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARPUBBLICA", op.ValAdapter(row("ARPUBBLICA").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARLOTVEN", op.ValAdapter(row("ARLOTVEN").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARCODMAR", op.ValAdapter(row("ARCODMAR").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARCODSEZ", op.ValAdapter(row("ARCODSEZ").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARSCHEDA", op.ValAdapter(row("ARSCHEDA").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARCODIVA", op.ValAdapter(row("ARCODIVA").ToString.Trim, TipoCampo.TChar))

                    Dim resultQuery As String = op.CreateInsertQuery("ARTICOLI", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportArticoliOrreaFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    Dim lMessage As String = "open ftp://" & _site.user & ":" & _site.password & "@" & _site.host & " -timeout=240" & Constants.vbCrLf &
                                             "put -filemask=""*.sql|*/"" " & "xml/" & _site.seq.TrimEnd("\") & "\*.sql" & Constants.vbCrLf &
                                             "exit" & Constants.vbCrLf
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportArticoliOrreaFull")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportArticoliOrrea")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "Xml\" & _site.seq.TrimEnd("\")
                Dim _Movefolder As String = "Xml\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump articoli completato!" & " - ExportArticoliFull")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportContrattiFull")
            Application.Exit()
        End Try
    End Function
    Private Function ExportListiniFull() As Boolean
        Try

            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump listini" & " - ExportListiniFull")

                _site = gSiteList(element)
                getItemToFilter(_site)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & _site.seq

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '
                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\explistini.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)

                Dim sqlQuery As String = op.getQuery(Globale.CodAzi, "listini.vqr")

                For Each _query As sQuery In gQuery
                    If _query.Tabella = "LISTINI" Then
                        sqlQuery = op.getQuery(g_AdhocAzi, _query.Query)
                    End If
                Next

                'La query sotto l'ho commentata perchè il filtro l' ho scritto dentro il file listini.vqr
                'sqlQuery += " AND LIS_TINI.LICODLIS <> '2' AND LIS_TINI.LICODLIS <> 'ONLI' ORDER BY LIS_TINI.LICODART, LIS_TINI.LICODLIS"
                Dim ds As DataSet = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    If testCodart.Trim <> row("LICODART").ToString.Trim Then

                        Dim deleteQuery As String = "delete from LISTINI where licodart = '" & row("LICODART").ToString.Trim & "';" & Chr(13) & Chr(10)
                        filewriter.Write(deleteQuery)
                        filewriter.Flush()
                        '
                        testCodart = row("LICODART").ToString.Trim
                    End If
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("LICODART", op.ValAdapter(row("LICODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LICODLIS", op.ValAdapter(row("LICODLIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LIDATATT", op.ValAdapter(row("LIDATATT").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("LIDATDIS", op.ValAdapter(row("LIDATDIS").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("LIPREZZO", op.ValAdapter(row("LIPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LIQUANTI", op.ValAdapter(row("LIQUANTI").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LISCONT1", op.ValAdapter(row("LISCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LISCONT2", op.ValAdapter(row("LISCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LISCONT3", op.ValAdapter(row("LISCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LISCONT4", op.ValAdapter(row("LISCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("LISTINI", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportListiniFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    Dim lMessage As String = "open ftp://" & _site.user & ":" & _site.password & "@" & _site.host & " -timeout=240" & Constants.vbCrLf &
                                             "put -filemask=""*.sql|*/"" " & "xml/" & _site.seq.TrimEnd("\") & "\*.sql" & Constants.vbCrLf &
                                             "exit" & Constants.vbCrLf
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportListiniFull")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportListiniFull")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "Xml\" & _site.seq.TrimEnd("\")
                Dim _Movefolder As String = "Xml\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump listini completato!" & " - ExportListiniFull")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportListiniFull")
            Application.Exit()
        End Try
    End Function

    Private Function ExportContrattiFull() As Boolean
        Try

            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump Contratti" & " - ExportContrattiFull")

                _site = gSiteList(element)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & _site.seq

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '
                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\expcontratti.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)
                '
                'Con_tram
                '
                '
                Dim deleteQuery As String = "delete from CON_TRAM;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                Dim sqlQuery As String = op.getQuery(Globale.CodAzi, "con_tram.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                Dim ds As DataSet = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODATCON", op.ValAdapter(row("CODATCON").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("COTIPCLF", op.ValAdapter(row("COTIPCLF").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODCLF", op.ValAdapter(row("COCODCLF").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCATCOM", op.ValAdapter(row("COCATCOM").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODESCON", op.ValAdapter(row("CODESCON").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODATINI", op.ValAdapter(row("CODATINI").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("CODATFIN", op.ValAdapter(row("CODATFIN").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("COCODVAL", op.ValAdapter(row("COCODVAL").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COIVALIS", op.ValAdapter(row("COIVALIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COFLAQTA", op.ValAdapter(row("COFLAQTA").ToString.Trim, TipoCampo.TChar))
                    Dim resultQuery As String = op.CreateInsertQuery("CON_TRAM", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                'Con_tram
                '
                '
                deleteQuery = "delete from CON_TRAD;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                sqlQuery = op.getQuery(Globale.CodAzi, "con_trad.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TInt))
                    adhoc.hFieldVal.Add("COGRUMER", op.ValAdapter(row("COGRUMER").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODART", op.ValAdapter(row("COCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COPREZZO", op.ValAdapter(row("COPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT1", op.ValAdapter(row("COSCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT2", op.ValAdapter(row("COSCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT3", op.ValAdapter(row("COSCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT4", op.ValAdapter(row("COSCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("CON_TRAD", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                'Cont_sca
                '
                '
                deleteQuery = "delete from CONT_SCA;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                sqlQuery = op.getQuery(Globale.CodAzi, "con_cosc.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TInt))
                    adhoc.hFieldVal.Add("COGRUMER", op.ValAdapter(row("COGRUMER").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODART", op.ValAdapter(row("COCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COQUANTI", op.ValAdapter(row("COQUANTI").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COPREZZO", op.ValAdapter(row("COPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT1", op.ValAdapter(row("COSCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT2", op.ValAdapter(row("COSCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT3", op.ValAdapter(row("COSCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT4", op.ValAdapter(row("COSCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("CONT_SCA", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                '
                '
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportContrattiFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    Dim lMessage As String = "open ftp://" & _site.user & ":" & _site.password & "@" & _site.host & " -timeout=240" & Constants.vbCrLf & _
                                             "put -filemask=""*.sql|*/"" " & "xml/" & _site.seq.TrimEnd("\") & "\*.sql" & Constants.vbCrLf & _
                                             "exit" & Constants.vbCrLf
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportContrattiFull")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportContrattiFull")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "Xml\" & _site.seq.TrimEnd("\")
                Dim _Movefolder As String = "Xml\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump contratti completato!" & " - ExportContrattiFull")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportContrattiFull")
            Application.Exit()
        End Try
    End Function
    Private Function ExportListiniFullAgenti() As Boolean
        Try

            Dim _site As New sSite

            gKsiteList.Sort()
            For Each element As String In gKsiteList


                Me.WriteLog("Inizio dump listini" & " - ExportListiniFullAgenti")

                _site = gSiteList(element)


                Dim sw As System.IO.FileStream

                Dim _updFold As String = "listiniagenti"

                If Not Directory.Exists(_updFold) Then
                    Directory.CreateDirectory(_updFold)
                End If
                '
                Dim testCodart As String = ""
                Dim fileexp As String = _updFold & "\\explistini.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                Dim filewriter As New System.IO.StreamWriter(sw)

                Dim sqlQuery As String = op.getQuery(Globale.CodAzi, "listini.vqr")
                'sqlQuery += " AND LIS_TINI.LICODLIS <> '2' AND LIS_TINI.LICODLIS <> 'ONLI' ORDER BY LIS_TINI.LICODART, LIS_TINI.LICODLIS"
                Dim ds As DataSet = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    If testCodart.Trim <> row("LICODART").ToString.Trim Then

                        Dim deleteQuery_1 As String = "delete from PALMLISTINID where lscodart = '" & row("LICODART").ToString.Trim & "';" & Chr(13) & Chr(10)
                        filewriter.Write(deleteQuery_1)
                        filewriter.Flush()
                        '
                        testCodart = row("LICODART").ToString.Trim
                    End If
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("LSCODART", op.ValAdapter(row("LICODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LSCODLIS", op.ValAdapter(row("LICODLIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LSDATINI", op.ValAdapter(row("LIDATATT").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("LSDATFIN", op.ValAdapter(row("LIDATDIS").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("LSPREZZO", op.ValAdapter(row("LIPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LSQUANTI", op.ValAdapter(row("LIQUANTI").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LSSCONT1", op.ValAdapter(row("LISCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LSSCONT2", op.ValAdapter(row("LISCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LSSCONT3", op.ValAdapter(row("LISCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("LSSCONT4", op.ValAdapter(row("LISCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMLISTINID", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                'Prepara script
                '
                Me.WriteLog("Preparazione script WinScp" & " - ExportListiniFull")
                '
                Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
                Try
                    'hScriptFile = File.Open("script.txt", FileMode.Append)
                    Dim lByteMess As Byte()
                    '

                    'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
                    '
                    'Dim lMessage As String = "open ftp://808coptag:xawe6188r" & "@" & _site.host & " -timeout=240" & Constants.vbCrLf & _
                    '                         "put -filemask=""*.sql|*/"" " & "listini" & "\*.sql" & Constants.vbCrLf & _
                    '                         "exit" & Constants.vbCrLf
                    '
                    Dim lMessage As String = "open ftp://808coptag:xawe6188r" & "@" & "ftp.copt.it" & " -timeout=240" & Constants.vbCrLf & _
                                             "put -filemask=""*.sql|*/"" " & "listiniagenti" & "\*.sql www/listini/" & Constants.vbCrLf & _
                                             "exit" & Constants.vbCrLf
                    '
                    lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
                    hScriptFile.Write(lByteMess, 0, lMessage.Length)
                    '
                Catch ex As Exception
                    Me.WriteLog(ex.Message & " - ExportListiniFull")
                Finally
                    hScriptFile.Close()
                    hScriptFile = Nothing
                End Try
                '

                fileexp = _updFold & "\\expcontratti.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)
                '
                'Con_tram
                '
                '
                Dim deleteQuery As String = "delete from PALMCON_TRAM;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                sqlQuery = op.getQuery(Globale.CodAzi, "con_tram.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODATCON", op.ValAdapter(row("CODATCON").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("COTIPCLF", op.ValAdapter(row("COTIPCLF").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODCLF", op.ValAdapter(row("COCODCLF").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCATCOM", op.ValAdapter(row("COCATCOM").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODESCON", op.ValAdapter(row("CODESCON").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CODATINI", op.ValAdapter(row("CODATINI").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("CODATFIN", op.ValAdapter(row("CODATFIN").ToString.Trim, TipoCampo.TData))
                    adhoc.hFieldVal.Add("COCODVAL", op.ValAdapter(row("COCODVAL").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COIVALIS", op.ValAdapter(row("COIVALIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COFLAQTA", op.ValAdapter(row("COFLAQTA").ToString.Trim, TipoCampo.TChar))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMCON_TRAM", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                'Con_tram
                '
                '
                deleteQuery = "delete from PALMCON_TRAD;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                sqlQuery = op.getQuery(Globale.CodAzi, "con_trad.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TInt))
                    adhoc.hFieldVal.Add("COGRUMER", op.ValAdapter(row("COGRUMER").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODART", op.ValAdapter(row("COCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COPREZZO", op.ValAdapter(row("COPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT1", op.ValAdapter(row("COSCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT2", op.ValAdapter(row("COSCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT3", op.ValAdapter(row("COSCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT4", op.ValAdapter(row("COSCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMCON_TRAD", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                'Cont_sca
                '
                '
                deleteQuery = "delete from PALMCON_COSC;" & Chr(13) & Chr(10)
                filewriter.Write(deleteQuery)
                filewriter.Flush()
                '
                sqlQuery = op.getQuery(Globale.CodAzi, "con_cosc.vqr")
                sqlQuery += " AND CON_TRAM.CODATFIN >= getdate()"

                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CONUMERO", op.ValAdapter(row("CONUMERO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CPROWNUM", op.ValAdapter(row("CPROWNUM").ToString.Trim, TipoCampo.TInt))
                    adhoc.hFieldVal.Add("COGRUMER", op.ValAdapter(row("COGRUMER").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COCODART", op.ValAdapter(row("COCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("COQUANTI", op.ValAdapter(row("COQUANTI").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COPREZZO", op.ValAdapter(row("COPREZZO").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT1", op.ValAdapter(row("COSCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT2", op.ValAdapter(row("COSCONT2").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT3", op.ValAdapter(row("COSCONT3").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("COSCONT4", op.ValAdapter(row("COSCONT4").ToString.Trim, TipoCampo.TCur))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMCON_COSC", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                    '
                Next
                '
                '
                '
                filewriter.Close()
                '
                fileexp = _updFold & "\\exparticoli.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "articoli_agenti.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = "delete from PALMARTICOLI where arcodart = '" & row("arcodart").ToString.Trim & "';" & Chr(13) & Chr(10)
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ARCODART", op.ValAdapter(row("ARCODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARDESART", op.ValAdapter(row("ARDESART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARUNIMIS", op.ValAdapter(row("ARUNMIS1").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARMOLTIP", op.ValAdapter(row("ARMOLTIP").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("AROPERAT", op.ValAdapter(row("AROPERAT").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARUNIMI2", op.ValAdapter(row("ARUNMIS2").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARGRUMER", op.ValAdapter(row("ARGRUMER").ToString.Trim, TipoCampo.TChar))
                    If op.ValAdapter(row("ARDTOBSO").ToString.Trim, TipoCampo.TChar) <> "''" Then
                        adhoc.hFieldVal.Add("ARDTOBSO", op.ValAdapter(row("ARDTOBSO").ToString.Trim, TipoCampo.TData))
                    End If
                    adhoc.hFieldVal.Add("ARLOTVEN", op.ValAdapter(row("ARLOTVEN").ToString.Trim, TipoCampo.TInt))
                    adhoc.hFieldVal.Add("AR___IMG", op.ValAdapter(row("AR___IMG").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ARDESSUP", op.ValAdapter(row("ARDESSUP").ToString.Trim, TipoCampo.TChar))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMARTICOLI", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                fileexp = _updFold & "\\expkeyarti.sql"

                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "key_arti.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = "delete from PALMKEYARTI where cacodice = '" & row("cacodice").ToString.Trim & "';" & Chr(13) & Chr(10)
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CACODICE", op.ValAdapter(row("CACODICE").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CADESART", op.ValAdapter(row("CADESART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CACODART", op.ValAdapter(row("CACODART").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CAUNIMIS", op.ValAdapter(row("CAUNIMIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CAMOLTIP", op.ValAdapter(row("CAMOLTIP").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("CAOPERAT", op.ValAdapter(row("CAOPERAT").ToString.Trim, TipoCampo.TChar))
                    If op.ValAdapter(row("CADTOBSO").ToString.Trim, TipoCampo.TChar) <> "''" Then
                        adhoc.hFieldVal.Add("CADTOBSO", op.ValAdapter(row("CADTOBSO").ToString.Trim, TipoCampo.TData))
                    End If
                    Dim resultQuery As String = op.CreateInsertQuery("PALMKEYARTI", adhoc.hFieldVal, cn, "")
                        filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                fileexp = _updFold & "\\explistinim.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "listinim.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = "delete from PALMLISTINIM where LSCODLIS = '" & row("lscodlis").ToString.Trim & "';" & Chr(13) & Chr(10)
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("LSCODLIS", op.ValAdapter(row("LSCODLIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LSDESLIS", op.ValAdapter(row("LSDESLIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("LSFLLOVE", op.ValAdapter(row("LSFLLOVE").ToString.Trim, TipoCampo.TChar))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMLISTINIM", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()

                '
                fileexp = _updFold & "\\expclienti.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "conti_agenti.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = "delete from PALMCLIENTI where CLCODCLI = '" & row("CLCODCLI").ToString.Trim & "';" & Chr(13) & Chr(10)
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("CLCODCLI", op.ValAdapter(row("CLCODCLI").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLRAGSOC", op.ValAdapter(row("CLRAGSOC").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLRAGSO1", op.ValAdapter(row("CLRAGSO1").ToString.Trim, TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("CLINDIRI", op.ValAdapter(row("CLINDIRI").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLINDIR2", op.ValAdapter(row("CLINDIR2").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CL___CAP", op.ValAdapter(row("CL___CAP").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CL___LOC", op.ValAdapter(row("CL___LOC").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CL__PROV", op.ValAdapter(row("CL__PROV").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLTELEFO", op.ValAdapter(row("CLTELEFO").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLNUMFAX", op.ValAdapter(row("CLNUMFAX").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLNUMCEL", op.ValAdapter(row("CLNUMCEL").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLNUMIVA", op.ValAdapter(row("CLNUMIVA").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLCODFIS", op.ValAdapter(row("CLCODFIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLCODLIS", op.ValAdapter(row("CLCODLIS").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLNAZION", op.ValAdapter(row("CLNAZION").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLAGENTE", op.ValAdapter(row("CLAGENTE").ToString.Trim, TipoCampo.TChar))
                    If op.ValAdapter(row("CLDTOBSO").ToString.Trim, TipoCampo.TChar) <> "''" Then
                        adhoc.hFieldVal.Add("CLDTOBSO", op.ValAdapter(row("CLDTOBSO").ToString.Trim, TipoCampo.TChar))
                    End If
                    adhoc.hFieldVal.Add("CLCODPAG", op.ValAdapter(row("CLCODPAG").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLCATCOM", op.ValAdapter(row("CLCATCOM").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("CLSCONT1", op.ValAdapter(row("CLSCONT1").ToString.Trim, TipoCampo.TCur))
                    adhoc.hFieldVal.Add("CLSCONT2", op.ValAdapter(row("CLSCONT2").ToString.Trim, TipoCampo.TCur))
                    '
                    Dim resultQuery As String = op.CreateInsertQuery("PALMCLIENTI", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                fileexp = _updFold & "\\expdesdive.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "des_dive.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = ""
                    deleteQuery_1 = String.Format("delete from PALMDES_DIVE where DDCODICE = '{0}', DDCODDES = '{1}' and DDTIPCON = 'C'", row("DDCODICE").ToString.Trim, row("DDCODDES").ToString.Trim)
                    deleteQuery_1 &= "';" & Chr(13) & Chr(10)
                    '
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("DDCODICE", op.ValAdapter(row("DDCODICE").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("DDCODDES", op.ValAdapter(row("DDCODDES").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("DDINDIRI", op.ValAdapter(row("DDINDIRI").ToString.Trim, TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("DD___CAP", op.ValAdapter(row("DD___CAP").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("DDLOCALI", op.ValAdapter(row("DDLOCALI").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("DDPROVIN", op.ValAdapter(row("DDPROVIN").ToString.Trim, TipoCampo.TChar))
                    If op.ValAdapter(row("DDDTOBSO").ToString.Trim, TipoCampo.TChar) <> "''" Then
                        adhoc.hFieldVal.Add("DDDTOBSO", op.ValAdapter(row("DDDTOBSO").ToString.Trim, TipoCampo.TChar))
                    End If
                    '
                    Dim resultQuery As String = op.CreateInsertQuery("PALMDES_DIVE", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()

                '
                fileexp = _updFold & "\\expsaldiart.sql"
                '
                sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                filewriter = New System.IO.StreamWriter(sw)

                sqlQuery = op.getQuery(Globale.CodAzi, "saldiart.vqr")
                ds = op.esegui_query(sqlQuery)
                For Each row As DataRow In ds.Tables(0).Rows

                    Dim deleteQuery_1 As String = "delete from PALMSALDIART where SACODART = '" & row("SLCODICE").ToString.Trim & "';" & Chr(13) & Chr(10)
                    filewriter.Write(deleteQuery_1)
                    filewriter.Flush()
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("SACODART", op.ValAdapter(row("SLCODICE").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("SALDIART", op.ValAdapter(row("SLQTAPER").ToString.Trim, TipoCampo.TChar))
                    Dim resultQuery As String = op.CreateInsertQuery("PALMSALDIART", adhoc.hFieldVal, cn, "")
                    filewriter.Write(resultQuery & ";" & Chr(13) & Chr(10))
                    filewriter.Flush()
                Next
                filewriter.Close()
                '
                'Export ftp
                '
                Me.WriteLog("Export file via ftp" & " - ExportListiniFull")
                '
                Try
                    '
                    Me.WriteLog("Upload files " & " - CallWinScp")
                    '
                    Dim _taskInfo As New ProcessStartInfo
                    Dim _ds As New Diagnostics.Process
                    '
                    'WinScp\winscp.exe /log=ftp.log /script=script.txt
                    _taskInfo.FileName = "WinScp\winscp.exe"
                    _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
                    _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
                    _ds = Process.Start(_taskInfo)
                    'wait for end process
                    While Not _ds.HasExited
                    End While
                    Dim a As String = _ds.ExitCode
                    '
                Catch ex As System.Exception
                    Me.WriteLog(ex.Message & " - Export file via ftp")
                    wRunning = False
                End Try
                '
                'Sposta files
                '
                Dim _folder As String = "listiniagenti"
                Dim _Movefolder As String = "listiniagenti\" & _site.seq.TrimEnd("\") & "\esportati"
                Dim di As New DirectoryInfo(_folder)
                Dim fi As FileInfo
                Me.WriteLog("Move all files " & " - MoveFtpFiles")

                Try
                    If Not System.IO.Directory.Exists(_Movefolder) Then
                        System.IO.Directory.CreateDirectory(_Movefolder)
                    End If
                    '
                    For Each fi In di.GetFiles()
                        Try
                            If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                                IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                            End If
                            IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As System.IO.IOException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                Catch ex As SystemException
                    Me.WriteLog(ex.Message & " - MoveFtpFiles")
                    Return False
                Finally
                    di = Nothing
                    fi = Nothing
                End Try
                '
            Next
            Me.WriteLog("Dump listini Agenti completato!" & " - ExportListiniFull")
            Application.Exit()
        Catch ex As Exception
            Me.WriteLog(ex.Message & " - ExportListiniFull")
            Application.Exit()
        End Try
    End Function


    Private Function fExport(ByVal phost As String, ByVal pfolder As String, ByVal puser As String, ByVal ppassword As String, ByRef _aOrd As ArrayList, ByRef _hTemp As Hashtable, ByVal pSeq As String) As Integer
        Try
            Dim _manifest As New sManifest
            Dim _keys As New sKey
            Dim __keys As New sKey
            Dim _key As String = ""
            Dim strsql As String = ""
            Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & pSeq

            If Not Directory.Exists(_updFold) Then
                Directory.CreateDirectory(_updFold)
            End If
            '
            '
            Dim _ROWCOUNT As Integer = 0
            '''''''''''''''
            '''''''''''''''

            'Creates update file
            Dim stringa As String = ""
            lbAzione.Text = "creazione files..."
            lbAzione.Refresh()
            Dim sw As System.IO.FileStream
            'Make file name by date and time
            Dim DateAndTime As String = Now.Year & Now.Month.ToString.PadLeft(2, "0") & Now.Day.ToString.PadLeft(2, "0") & Now.Hour.ToString.PadLeft(2, "0") & Now.Second.ToString.PadLeft(2, "0")
            Dim fileexp As String = _updFold & "\\" & DateAndTime & ".xml"
            Dim filerar As String = _updFold & "\\" & DateAndTime & ".rar"
            '
            sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            'Writes header file
            If gUTF8 = "S" Then
                stringa = "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>" & Chr(13) & Chr(10)
            Else
                stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
            End If
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
                If _element.Trim = "ARTICOLI#CO00077" Then
                    Dim a As String = "1"
                End If

                Me.txtRiga.Text = _element
                Me.txtRiga.Refresh()
                Dim _table As String = _element.Split("#").Clone(0).ToString.Trim
                _keys = _hTemp(_element)
                Dim _codice As String = _element.Split("#").Clone(1).ToString.Trim
                'Filters Tables
                If gTableFilter.Count > 0 Then
                    If gTableFilter.Contains(_table) Then
                        Continue For
                    End If
                End If
                'Filters Items
                If gItemsFilter.Count > 0 Then
                    Select Case _table.ToString.ToLower
                        Case "articoli", "keyarti", "listini", "saldiart"
                            If Not gItemsFilter.Contains(_codice) Then 'If item not exists into gItemsFilter, includes it
                                '
                                Continue For
                                '
                            End If
                        Case Else
                    End Select
                End If
                '
                _manifest = New sManifest
                If _table = "CLIENTI" Or _table = "ARTICOLI" Then
                    Dim a As Integer = 1
                End If
                If Not _Container.ContainsKey(_table) Then
                    _manifest = Me.readManifest(_table)
                Else
                    _manifest = _Container(_table)
                End If
                For Each _query As sQuery In gQuery
                    If _query.Tabella = _table Then
                        _manifest.queryString = op.getQuery(g_AdhocAzi, _query.Query)
                    End If
                Next

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
                'Ogni 5000 righe crea un' altro file
                '
                _ROWCOUNT += 1
                If _ROWCOUNT > 5000 Then
                    stringa = "</dataroot>" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    '
                    sw.Close()
                    '
                    DateAndTime = Now.Year & Now.Month.ToString.PadLeft(2, "0") & Now.Day.ToString.PadLeft(2, "0") & Now.Hour.ToString.PadLeft(2, "0") & Now.Second.ToString.PadLeft(2, "0")
                    fileexp = _updFold & "\\" & DateAndTime & ".xml"
                    '
                    sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
                    filewriter = New System.IO.StreamWriter(sw)
                    'Writes header file
                    If gUTF8 = "S" Then
                        stringa = "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>" & Chr(13) & Chr(10)
                    Else
                        stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
                    End If
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    stringa = "<dataroot>" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    '
                    _ROWCOUNT = 0
                End If
                Application.DoEvents()
            Next
            '
            'Esporta immagini
            expImmagini(filewriter)
            '
            'put Here the call to export week-discount function
            If gFasce = "S" Then
                If Globale.g_SysData.DayOfWeek >= 4 Or Me.chkFasce.Checked Then
                    Me.ExpWeekDiscount(filewriter)
                    'Me.expRoute(filewriter)
                End If
            End If
            '
            If Me.chkAll.Checked = False And Globale.gExpOrdStatus = "S" Then
                If Not gTableFilter.Contains("STATORD") Then
                    Me.expOrdStatus(filewriter)
                End If
            End If
            '
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
            sw.Close()
            '
            wStep = 1
            Do
                Select Case wStep
                    Case 1
                        '
                        If wRunning Then Continue Do
                        '
                        Me.txtRiga.Text = ""
                        Me.txtRiga.Refresh()
                        Me.lb_grandezza.Text = ""
                        Me.lb_grandezza.Refresh()
                        Me.lb_progress.Text = ""
                        Me.lb_progress.Refresh()
                        Me.lbAzione.Text = ""
                        Me.lbAzione.Refresh()
                        '
                        'Send file by ftp
                        'pFolder is the remote host folder name
                        'Source directory is in pSeq variable
                        '
                        If chkNoftp.Checked = False Then
                            'crea script per il lnacio di WinScp
                            Dim _st As Boolean = ftpExport(phost, pfolder, puser, ppassword, pSeq)
                            If Not _st Then
                                Me.WriteLog("Previous task has genarated errors - exit")
                                wStep = 10
                            End If
                        Else
                            wStep += 1
                        End If
                        '
                    Case 2
                        '
                        If wRunning Then Continue Do
                        '
                        If chkNoftp.Checked = False Then
                            Dim _st As Boolean = CallWinScp(pfolder)
                            If Not _st Then
                                Me.WriteLog("Previous task has genarated errors - exit")
                                wStep = 10
                            End If
                        Else
                            wStep += 1
                        End If
                    Case 3
                        '
                        If wRunning Then Continue Do
                        '
                        If Not chkMantxml.Checked Then
                            'Dim _st As Boolean = DeleteFiles(pSeq)
                            Dim _st As Boolean = MoveFtpFiles(pSeq)
                            If Not _st Then
                                Me.WriteLog("Previous task has genarated errors - exit")
                                wStep = 10
                            End If
                        Else
                            wStep += 1
                        End If
                    Case 10
                        Exit Do
                    Case Else
                        If wStep <> 0 Then
                            wStep += 1
                        End If
                End Select
            Loop
            '
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
    Private Function fExport_bak(ByVal phost As String, ByVal pfolder As String, ByVal puser As String, ByVal ppassword As String, ByRef _aOrd As ArrayList, ByRef _hTemp As Hashtable, ByVal pSeq As String) As Integer
        Try
            Dim _manifest As New sManifest
            Dim _keys As New sKey
            Dim __keys As New sKey
            Dim _key As String = ""
            Dim strsql As String = ""
            Dim _updFold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & pSeq

            If Not Directory.Exists(_updFold) Then
                Directory.CreateDirectory(_updFold)
            End If

            '''''''''''''''
            '''''''''''''''

            'Creates update file
            Dim stringa As String = ""
            lbAzione.Text = "creazione files..."
            lbAzione.Refresh()
            Dim sw As System.IO.FileStream
            'Make file name by date and time
            Dim DateAndTime As String = Now.Year & Now.Month.ToString.PadLeft(2, "0") & Now.Day.ToString.PadLeft(2, "0") & Now.Hour.ToString.PadLeft(2, "0") & Now.Second.ToString.PadLeft(2, "0")
            Dim fileexp As String = _updFold & "\\" & DateAndTime & ".xml"
            Dim filerar As String = _updFold & "\\" & DateAndTime & ".rar"
            '
            sw = New System.IO.FileStream(fileexp, IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            'Writes header file
            If gUTF8 = "S" Then
                stringa = "<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>" & Chr(13) & Chr(10)
            Else
                stringa = "<?xml version=""1.0"" standalone=""yes""?>" & Chr(13) & Chr(10)
            End If
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
                'Filters Tables
                If gTableFilter.Count > 0 Then
                    If Not gTableFilter.Contains(_table) Then
                        Continue For
                    End If
                End If
                'Filters Items
                If gItemsFilter.Count > 0 Then
                    Select Case _table.ToString.ToLower
                        Case "articoli", "keyarti", "listini", "saldiart"
                            If Not gItemsFilter.Contains(_codice) Then 'If item not exists into gItemsFilter, includes it
                                '
                                Continue For
                                '
                            End If
                        Case Else
                    End Select
                End If
                '
                _manifest = New sManifest
                If _table = "CLIENTI" Or _table = "ARTICOLI" Then
                    Dim a As Integer = 1
                End If
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
                Application.DoEvents()
            Next
            'put Here the call to export week-discount function
            If gFasce = "S" Then
                If Globale.g_SysData.DayOfWeek >= 4 Or Me.chkFasce.Checked Then
                    Me.ExpWeekDiscount(filewriter)
                    'Me.expRoute(filewriter)
                End If
            End If
            '
            If Me.chkAll.Checked = False And Globale.gExpOrdStatus = "S" Then
                Me.expOrdStatus(filewriter)
            End If
            '
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            '
            sw.Close()
            '
            wStep = 1
            Do
                Select Case wStep
                    Case 1
                        If gCompres = "S" Then
                            If _g_auto Then
                                Me.WriteLog("Compressing file...")
                            Else
                                Me.WriteLog("Compressing file...")
                                Me.txtRiga.Text = "Compressing file..."
                                Me.txtRiga.Refresh()
                            End If
                            Me.AddFileToRarArchive(filerar, fileexp)
                        Else
                            wStep += 1
                        End If
                    Case 2
                        Me.txtRiga.Text = ""
                        Me.txtRiga.Refresh()
                        Me.lb_grandezza.Text = ""
                        Me.lb_grandezza.Refresh()
                        Me.lb_progress.Text = ""
                        Me.lb_progress.Refresh()
                        Me.lbAzione.Text = ""
                        Me.lbAzione.Refresh()
                        'Send file by ftp
                        If chkNoftp.Checked = False Then
                            ftpExport(phost, pfolder, puser, ppassword, pSeq)
                        Else
                            wStep += 1
                        End If
                        '
                    Case 10
                        Exit Do
                    Case Else
                        If wStep <> 0 Then
                            wStep += 1
                        End If
                End Select
            Loop
            '
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
                Me.txtRiga.Text = "Reading... " & _manifest.mappingTable.Trim
                Me.txtRiga.Refresh()
                _query = _manifest.queryAll
                Dim ds As DataSet = op.esegui_query(_manifest.queryAll)
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each _row As DataRow In ds.Tables(0).Rows
                        '
                        _key = _hashTable(element).ToString.Trim & "#" & _row(0)
                        If _key.Contains("CO00077") Then
                            Dim a As String = "1"
                        End If
                        '
                        Try
                            _key = _key & "#" & _row(1)
                            _key = _key & "#" & _row(2)
                        Catch ex As System.Exception
                        End Try
                        If Not _hTemp.ContainsKey(_key) Then
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
                            'Filters Tables
                            If gTableFilter.Count > 0 Then
                                If Not gTableFilter.Contains(_keys.key1) Then
                                    Continue For
                                End If
                            End If
                            '
                            If gItemsFilter.Count > 0 Then
                                Select Case _manifest.mappingTable.ToString.ToLower.Trim
                                    Case "art_icol", "key_arti", "lis_tini", "saldiart"
                                        If Not gItemsFilter.Contains(_keys.key1) Then 'If item not exists into gItemsFilter, doesn't includes it
                                            '
                                            Continue For
                                            '
                                        End If
                                    Case Else
                                End Select
                            End If
                            '
                            _aOrd.Add(_key)
                            _keys.operation = "UPDATE"
                            _hTemp.Add(_key, _keys)
                            '
                        End If
                        '
                    Next
                End If
                Application.DoEvents()
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
    Private Function expOrdStatus(ByVal filewriter As System.IO.StreamWriter) As Integer

        Dim stringa As String = ""
        Try
            Dim _datrif As New Date
            _datrif = Now.Date
            _datrif = _datrif.AddDays(-30)
            Dim _pparam As New Hashtable
            '
            _pparam.Add("@DATRIF", op.ValAdapter(_datrif, TipoCampo.TData))
            Dim strsql As String = op.getQuery(CAdhocDocVar.g_AdhocAzi, "ORDERSTATUS_R.vqr", _pparam)
            Dim ds As DataSet = op.esegui_query(strsql)
            '
            For Each _row As DataRow In ds.Tables(0).Rows
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>" & xmlEnc("STATORD") & "</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>" & xmlEnc("UPDATE") & "</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & Chr(9) & Chr(9) & "<OSNUMDOC>" & xmlEnc(_row.Item("MVNUMDOC")) & "</OSNUMDOC>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSCODESE>" & xmlEnc(_row.Item("MVCODESE")) & "</OSCODESE>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSDOCWEB>" & xmlEnc(CTran(_row.Item("MVDOCWEB"), " ")) & "</OSDOCWEB>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSSTATUS>" & xmlEnc(_row.Item("STATO").ToString.Trim) & "</OSSTATUS>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSCODCLI>" & xmlEnc(_row.Item("MVCODCON").ToString.Trim) & "</OSCODCLI>" & Chr(13) & Chr(10)
                Try
                    stringa = stringa & Chr(9) & Chr(9) & Chr(9) & "<OS_EMAIL>" & xmlEnc(_row.Item("UT_EMAIL").ToString.Trim) & "</OS_EMAIL>" & Chr(13) & Chr(10)
                Catch ex As Exception

                End Try
                '
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next
            '
            _pparam.Clear()
            _pparam.Add("@DATRIF", op.ValAdapter(_datrif, TipoCampo.TData))
            strsql = op.getQuery(CAdhocDocVar.g_AdhocAzi, "ORDERSTATUS_E.vqr", _pparam)
            ds = op.esegui_query(strsql)
            '
            For Each _row As DataRow In ds.Tables(0).Rows
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>STATORD</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>UPDATE</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & Chr(9) & Chr(9) & "<OSNUMDOC>" & xmlEnc(_row.Item("MVNUMDOC")) & "</OSNUMDOC>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSCODESE>" & xmlEnc(_row.Item("MVCODESE")) & "</OSCODESE>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSDOCWEB>" & xmlEnc(CTran(_row.Item("MVDOCWEB"), " ")) & "</OSDOCWEB>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSSTATUS>" & xmlEnc(_row.Item("STATO").ToString.Trim) & "</OSSTATUS>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<OSCODCLI>" & xmlEnc(_row.Item("MVCODCON").ToString.Trim) & "</OSCODCLI>" & Chr(13) & Chr(10)
                Try
                    stringa = stringa & Chr(9) & Chr(9) & Chr(9) & "<OS_EMAIL>" & xmlEnc(_row.Item("UT_EMAIL").ToString.Trim) & "</OS_EMAIL>" & Chr(13) & Chr(10)
                Catch ex As Exception

                End Try
                '
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next
        Catch ex As Exception
            Me.WriteLog(ex.Message & "-" & stringa & " - expOrdStatus")
            'MsgBox(ex.Message, MsgBoxStyle.Exclamation, "expOrdStatus")
        End Try

    End Function
    Private Function expImmagini(ByVal filewriter As System.IO.StreamWriter) As Integer

        Dim stringa As String = ""
        Dim iIndex As Integer = 0
        Try
            For Each _el As String In hImages.Keys
                Dim _articolo As String = _el.ToString.Trim()
                Dim _immagine As String = hImages(_el).ToString.Trim
                '
                Dim _prima As String = hImages(_el)
                iIndex += 1
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>" & xmlEnc("ARTICOLI_IMMAGINI") & "</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>" & xmlEnc("UPDATE") & "</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & Chr(9) & Chr(9) & "<ARCODART>" & xmlEnc(_articolo) & "</ARCODART>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & Chr(9) & "<AR___IMG>" & xmlEnc(_immagine) & "</AR___IMG>" & Chr(13) & Chr(10)
                '
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next
            '
        Catch ex As Exception
            Me.WriteLog(ex.Message & "-" & stringa & " - expImmagini")
            'MsgBox(ex.Message, MsgBoxStyle.Exclamation, "expOrdStatus")
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
            Dim manifestName As String = Globale.CodAzi & "\updateManifest.xml"
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
            Dim _logstring = ""
            stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<tablename>" & xmlEnc(ptablename) & "</tablename>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<operation>" & xmlEnc(pKeys.operation) & "</operation>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            _logstring = _logstring & "<tablename>" & xmlEnc(ptablename) & "</tablename>" & "<operation>" & xmlEnc(pKeys.operation) & "</operation>" & _
                        "<fields>"
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
                    'If .hTargetKeys(_field).ToString.Trim = "LIQUANTI" Then
                    '_fieldvalue = _fieldvalue.Replace(".", ",")
                    'End If
                    stringa = Chr(9) & Chr(9) & Chr(9) & "<" & .hTargetKeys(_field).ToString.Trim & ">" & xmlEnc(_fieldvalue.ToString.Trim) & "</" & .hTargetKeys(_field).ToString.Trim & ">" & Chr(13) & Chr(10)
                    filewriter.Write(stringa)
                    filewriter.Flush()
                    _logstring = _logstring & "<" & .hTargetKeys(_field).ToString.Trim & ">" & xmlEnc(_fieldvalue.ToString.Trim) & "</" & .hTargetKeys(_field).ToString.Trim & ">"
                Next
            End With
            '
            stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            _logstring = _logstring & "</fields>" & vbCr
            WriteExportLog(_logstring)
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
            Dim _rowValue As String = ""
            Dim _propValue As String = ""
            Dim _logstring As String = ""
            '
            If ptablename = "ARTICOLI" Then
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
            'Applica filtro
            _queryString = _queryString & _criteriaString
            For Each element As sCriterio In gCriteri
                If element.Tabella.Trim = ptablename.Trim Then
                    _queryString = _queryString & " AND " & element.Filtro
                End If
            Next
            'Fine applica filtro
            Dim dsArt As DataSet = op.esegui_query(_queryString)
            For Each _row As DataRow In dsArt.Tables(0).Rows
                '
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<tablename>" & xmlEnc(ptablename) & "</tablename>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<operation>" & xmlEnc(pKeys.operation) & "</operation>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & Chr(9) & "<fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                _logstring = _logstring & "<tablename>" & xmlEnc(ptablename) & "</tablename>" & "<operation>" & xmlEnc(pKeys.operation) & "</operation>" & _
                                        "<fields>"
                '
                If ptablename = "LISTINI" Then
                    Dim a As String = 1
                End If
                With pManifest
                    For Each _field As String In .hFields.Keys

                        _rowValue = _row.Item(.hFields(_field)).ToString.Trim()
                        'Reads field's properties

                        If gFldProp.ContainsKey(ptablename & _field & "nvl") Then
                            _propValue = gFldProp(ptablename & _field & "nvl")
                            _rowValue = CTran(_rowValue, _propValue)
                        End If
                        '
                        If _field = "LICODART" And _rowValue = "CO06435" Then
                            Dim a As String = 1
                        End If
                        If _field = "AR___IMG" Then
                            Dim a As String() = _rowValue.ToString.Trim.Split(";")
                            Dim iIndex As Integer = 0
                            Dim rString As String = ""
                            If a.Length > 0 Then
                                For Each el As String In a
                                    If iIndex > 0 Then
                                        Try
                                            rString &= el.ToString.Trim() & ";"
                                        Catch
                                        End Try
                                    End If

                                    iIndex += 1
                                Next
                                If rString.Trim() <> "" Then
                                    hImages.Add(_row.Item(pManifest.hFields("ARCODART")).ToString.Trim(), rString.TrimEnd(";"))
                                End If
                            End If
                            _rowValue = _rowValue.ToString.Trim.Split(";")(0)
                        End If
                        If _field = "ARDESSUP" Then
                            _rowValue = _rowValue.ToString.Replace(Chr(13), "_b")
                            _rowValue = _rowValue.Replace(Chr(10), "r_")

                        End If
                        '
                        stringa = Chr(9) & Chr(9) & Chr(9) & "<" & .hFields(_field) & ">" & xmlEnc(_rowValue) & "</" & .hFields(_field) & ">" & Chr(13) & Chr(10)
                        filewriter.Write(stringa)
                        filewriter.Flush()
                        _logstring = _logstring & "<" & .hFields(_field) & ">" & xmlEnc(_rowValue) & "</" & .hFields(_field) & ">"
                    Next
                End With
                '
                stringa = Chr(9) & Chr(9) & "</fields>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                _logstring = _logstring & "</fields>"
                WriteExportLog(_logstring)
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
            Dim manifestName As String = Globale.CodAzi & "\updateManifest.xml"
            Dim _source As String = ""
            Dim _target As String = ""
            _manifest.hFields = New Hashtable
            _manifest.hkeys = New Hashtable
            _manifest.hTargetFields = New Hashtable
            _manifest.hTargetKeys = New Hashtable
            _manifest.mappingTable = ""
            _manifest.queryString = ""
            gFldProp.Clear()
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
                    If name = "tablename" And value <> pTablename Then
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
                                    'Fill Properties structures
                                    If Not _childNode.ChildNodes(1) Is Nothing Then
                                        If _childNode.ChildNodes(1).Name.ToUpper = "PROPERTIES" Then
                                            For Each _subchild As XmlNode In _childNode.ChildNodes(1).ChildNodes
                                                Select Case _subchild.Name
                                                    Case "nvl"
                                                        gFldProp.Add(pTablename & _source & "nvl", _subchild.InnerText)
                                                    Case "replace"

                                                End Select
                                            Next
                                        End If
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

        Dim sStep As Integer = wStep 'Save step
        wStep = 0 'Disables all steps while executing this routine.
        Try
            Dim _taskInfo As New ProcessStartInfo
            Dim _ds As New Diagnostics.Process
            '
            _taskInfo.FileName = "rar.exe"
            _taskInfo.Arguments = "a -ep " & pCurrentZip & " " & filePath
            _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
            _taskInfo.WorkingDirectory = Environment.CurrentDirectory
            If Globale.gExtendLog = "S" Then
                _taskInfo.RedirectStandardOutput = True
                _taskInfo.RedirectStandardError = True
                _taskInfo.UseShellExecute = False
            Else
                _taskInfo.RedirectStandardOutput = False
                _taskInfo.RedirectStandardError = False
                _taskInfo.UseShellExecute = True
            End If
            Me.WriteLog(_taskInfo.FileName.Trim & " " & _taskInfo.Arguments.Trim)
            _ds = Process.Start(_taskInfo)
            _ds.WaitForExit()
            Dim sline As String = ""
            If Globale.gExtendLog = "S" Then
                Dim StreamOut As StreamReader = _ds.StandardOutput
                sline = StreamOut.ReadToEnd
                Me.WriteLog(sline)
                Dim StreamErr As StreamReader = _ds.StandardError
                sline = StreamErr.ReadToEnd
                Me.WriteLog(sline)
            End If
            'wait for end process
            'While Not _ds.HasExited
            'End While
            If Globale.gExtendLog = "S" Then
                Dim a As String = _ds.ExitCode
                Me.WriteLog("Exit Code : " & _ds.ExitCode)
            End If
            '
            IO.File.Delete(filePath)
            Return True
        Catch ex As System.Exception
            If _g_auto Then
                Me.WriteLog("Error in function AddFileToRarArchive while compacting " & filePath & " ,specific error : " & ex.Message)
            Else
                MsgBox("Error in function AddFileToRarArchive while compacting " & filePath & " ,specific error : " & ex.Message)
            End If
        Finally
            wStep = sStep
            wStep += 1
        End Try
    End Function

    Private Sub setTable(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClienti.CheckedChanged, chkArticoli.CheckedChanged, chkListini.CheckedChanged, chkSaldi.CheckedChanged, chkSedi.CheckedChanged, chkContratti.CheckedChanged, chkCatego.CheckedChanged, chkMarchi.CheckedChanged, chkIve.CheckedChanged, ChkPagamen.CheckedChanged
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
                        '
                        _hOperat.Add("CATEGOA", "CATEGOA")
                        _hOperat.Add("CATEGOB", "CATEGOB")
                        _hOperat.Add("CATEGOC", "CATEGOC")
                        _hOperat.Add("PAESEPRO", "PAESEPRO")
                        _hOperat.Add("NOMEPRO", "NOMEPRO")
                    Case "chksaldi"
                        _hOperat.Add("SALDIART", "SALDIART")
                    Case "chksedi"
                        _hOperat.Add("DES_DIVE", "DES_DIVE")
                    Case "chkcontratti"
                        _hOperat.Add("CON_TRAM", "CON_TRAM")
                        _hOperat.Add("CON_TRAD", "CON_TRAD")
                        _hOperat.Add("CONT_SCA", "CONT_SCA")
                    Case "chkcatego"
                        Select Case Globale.CodAzi
                            Case "ALTRQ"
                                _hOperat.Add("CATEGOA", "CATEGOA")
                                _hOperat.Add("CATEGOB", "CATEGOB")
                                _hOperat.Add("CATEGOC", "CATEGOC")
                            Case "COPTG"
                                _hOperat.Add("FAM_ARTI", "FAM_ARTI")
                                _hOperat.Add("GRUMERC", "GRUMERC")
                        End Select
                    Case "chkmarchi"
                        _hOperat.Add("MARCHI", "MARCHI")
                    Case "chkive"
                        _hOperat.Add("VOCIIVA", "VOCIIVA")
                    Case "chkpagamen"
                        _hOperat.Add("PAGAMEN", "PAGAMEN")
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
                        '
                        _hOperat.Remove("CATEGOA")
                        _hOperat.Remove("CATEGOB")
                        _hOperat.Remove("CATEGOC")
                        _hOperat.Remove("PAESEPRO")
                        _hOperat.Remove("NOMEPRO")
                    Case "chksaldi"
                        _hOperat.Remove("SALDIART")
                    Case "chksedi"
                        _hOperat.Remove("DES_DIVE")
                    Case "chkcontratti"
                        _hOperat.Remove("CON_TRAM")
                        _hOperat.Remove("CON_TRAD")
                        _hOperat.Remove("CONT_SCA")
                    Case "chkcatego"
                        Select Case Globale.CodAzi
                            Case "ALTRQ"
                                _hOperat.Remove("CATEGOA")
                                _hOperat.Remove("CATEGOB")
                                _hOperat.Remove("CATEGOC")
                            Case "COPTG"
                                _hOperat.Remove("FAM_ARTI")
                                _hOperat.Remove("GRUMERC")
                        End Select
                    Case "chkmarchi"
                        _hOperat.Remove("MARCHI")
                    Case "chkive"
                        _hOperat.Remove("VOCIIVA")
                    Case "chkpagamen"
                        _hOperat.Remove("PAGAMEN")
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
    Private Function ftpExport(ByVal pFtpserver As String, ByVal pRemoteFolder As String, ByVal pFtpUser As String, ByVal pFtpPwd As String, ByVal plocalfolder As String) As Boolean
        Try
            '
            Me.WriteLog("Prepare script " & " - ftpExport")
            '
            Dim pStatus As Boolean = WinScpScript(pFtpUser, pFtpPwd, plocalfolder, pFtpserver)
            wStep += 1
            wRunning = False
            Return True
        Catch ex As Exception
            'Display the error message.
            If _g_auto Then
                Me.WriteLog(ex.Message & " - ftpExport()")
            Else
                MsgBox(ex.Message & " - ftpExport()")
            End If
            Return False
        End Try
    End Function
    Private Function CallWinScp(ByVal pFolder As String) As Boolean

        Try
            '
            Me.WriteLog("Upload files " & " - CallWinScp")
            '
            Dim _taskInfo As New ProcessStartInfo
            Dim _ds As New Diagnostics.Process
            '
            'WinScp\winscp.exe /log=ftp.log /script=script.txt
            _taskInfo.FileName = "WinScp\winscp.exe"
            _taskInfo.Arguments = "/log=ftp.log /script=script.txt"
            _taskInfo.WindowStyle = ProcessWindowStyle.Hidden
            _ds = Process.Start(_taskInfo)
            'wait for end process
            While Not _ds.HasExited
            End While
            Dim a As String = _ds.ExitCode
            wRunning = False
            '
            wStep += 1
            Return True
        Catch ex As System.Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - CallWinScp")
            Else
                MsgBox(ex.Message & " - CallWinScp")
            End If
            wRunning = False
            Return False
        End Try
    End Function
    Private Function MoveFtpFiles(ByVal pFolder As String) As Boolean

        Dim _folder As String = "Xml\" & pFolder.TrimEnd("\")
        Dim _Movefolder As String = "Xml\" & pFolder.TrimEnd("\") & "\esportati"
        Dim di As New DirectoryInfo(_folder)
        Dim fi As FileInfo
        Me.WriteLog("Move all files " & " - MoveFtpFiles")

        Try
            If Not System.IO.Directory.Exists(_Movefolder) Then
                System.IO.Directory.CreateDirectory(_Movefolder)
            End If
            '
            For Each fi In di.GetFiles()
                Try
                    If IO.File.Exists(_Movefolder.TrimEnd & "\" & fi.Name) Then
                        IO.File.Delete(_Movefolder.TrimEnd & "\" & fi.Name)
                    End If
                    IO.File.Move(_folder.TrimEnd & "\" & fi.Name, _Movefolder.TrimEnd & "\" & fi.Name)
                Catch ex As Exception

                End Try
            Next
            wStep += 1
            wRunning = False
            Return True
        Catch ex As System.IO.IOException
            If _g_auto Then
                Me.WriteLog(ex.Message & " - MoveFtpFiles")
            Else
                MsgBox(ex.Message & " - MoveFtpFiles")
            End If
            wStep += 1
            wRunning = False
            Return False
        Catch ex As SystemException
            If _g_auto Then
                Me.WriteLog(ex.Message & " - MoveFtpFiles")
            Else
                MsgBox(ex.Message & " - MoveFtpFiles")
            End If
            wRunning = False
            Return False
        Finally
            di = Nothing
            fi = Nothing
        End Try
    End Function
    Private Function DeleteFiles(ByVal pFolder As String) As Boolean

        Dim _folder As String = "Xml\" & pFolder.TrimEnd("\")
        Dim di As New DirectoryInfo(_folder)
        Dim fi As FileInfo
        Me.WriteLog("Clear all files " & " - DeleteFiles")
        Try
            For Each fi In di.GetFiles()
                Try
                    IO.File.Delete(_folder.TrimEnd & "\" & fi.Name)
                Catch ex As Exception

                End Try
            Next
            wStep += 1
            wRunning = False
            Return True
        Catch ex As System.IO.IOException
            If _g_auto Then
                Me.WriteLog(ex.Message & " - DeleteFiles")
            Else
                MsgBox(ex.Message & " - DeleteFiles")
            End If
            wStep += 1
            wRunning = False
            Return False
        Catch ex As SystemException
            If _g_auto Then
                Me.WriteLog(ex.Message & " - DeleteFiles")
            Else
                MsgBox(ex.Message & " - DeleteFiles")
            End If
            wRunning = False
            Return False
        Finally
            di = Nothing
            fi = Nothing
        End Try


    End Function
    Private Sub ftpExport_1(ByVal pFtpserver As String, ByVal pRemoteFolder As String, ByVal pFtpUser As String, ByVal pFtpPwd As String, ByVal plocalfolder As String)
        Dim Grandezza As Long
        Dim ff As clsFTP

        lbattendi.Visible = True
        lbattendi.Refresh()
        '
        Dim sStep As Integer = wStep 'Save step
        wStep = 0 'Disables all steps while executing this routine.
        '
        Me.WriteLog("Uploading file...")
        Try
            'Pass values to the constructor. These values can be overridden by setting
            'the appropriate properties on the instance of the clsFTP class.
            'The third parameter is the user name. The FTP site is accessed with the user name.
            'If there is no specific user name, the user name can be anonymous.
            'The fourth parameter is the password. The FTP server is accessed with the password.
            'The fifth parameter is the port of the FTP server. The port of the FTP server is typically 21.

            lbattendi.Visible = True
            lbattendi.Refresh()

            Dim _tfold As String = Globale.cartellaAggLocale.TrimEnd("\") & "\\" & plocalfolder
            Me.WriteLog("Working directory for xml file : " & _tfold)
            Dim _fileList As ArrayList = Me.ElencaFileAgg(_tfold)
            Dim _localFold As New DirectoryInfo(_tfold)
            If IsNothing(_fileList) Then
                Exit Sub
            End If
            '
            ff = New clsFTP(pFtpserver, _
                                "/" & pRemoteFolder & "/", _
                                pFtpUser, _
                                pFtpPwd, _
                                21)
            'Try to log on to the FTP server.
            If (ff.Login() = True) Then
                '
                Me.WriteLog("Logged in...")
                'Change the directory on your FTP site.
                If (ff.ChangeDirectory("/" & pRemoteFolder) = False) Then
                    'Successful changing the directory
                    If _g_auto Then
                        Me.WriteLog("Cambio directory fallito - ftpExport()")
                    Else
                        MsgBox("Cambio directory fallito", MsgBoxStyle.Critical, "ftpExport()")
                    End If
                    ff.CloseConnection()
                    Exit Sub
                Else
                    If Globale.gExtendLog = "S" Then
                        Me.WriteLog("Directory changed to : " & pRemoteFolder)
                    End If
                End If
                '
                Me.WriteLog("File to be copying : " & _fileList.Count.ToString)
                '
                For Each _file As String In _fileList
                    ff.SetBinaryMode(True)
                    ProgressBar2.Maximum = _fileList.Count
                    ProgressBar2.Minimum = 0
                    ProgressBar2.Value = 0
                    lbAzione.Text = "upload dei files..."
                    lbAzione.Refresh()
                    Me.WriteLog("uploading file : " & _file)
                    Dim _fi As FileInfo() = _localFold.GetFiles(_file)
                    Grandezza = _fi(0).Length
                    lb_grandezza.Text = Grandezza
                    lb_grandezza.Refresh()
                    lb_progress.Text = 0
                    lb_progress.Refresh()
                    If (ff.UploadFile(_tfold & "\" & _file, lb_progress) = True) Then
                        Me.WriteLog("file " & _file & " uploaded")
                    Else
                        Me.WriteLog("uploaded " & _file & " failed!")
                    End If
                Next
            End If
            If Me.chkMantxml.Checked = False Then
                DeleteFileAgg(_tfold)
            End If
            Me.WriteLog("Closing connection")
            ff.CloseConnection()
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
            wStep = sStep
            wStep += 1
        End Try

    End Sub
    Private Function importData() As Boolean
        Try
            Dim _site As New sSite
            Me.lstUpd.Items.Clear()
            Me.lstUpd.Refresh()
            For Each element As String In gSiteListIn.Keys
                _site = gSiteListIn(element)
                With _site
                    Me.ftpimport(.host, .folder, .user, .password)
                End With
            Next
            Me.listXmlFileImp()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "exportData")
            Return False
        End Try
    End Function
    Private Sub ftpimport(ByVal pFtpserver As String, ByVal pRemoteFolder As String, ByVal pFtpUser As String, ByVal pFtpPwd As String)
        Dim Grandezza As Long
        Dim ff As clsFTP
        lbattendiin.Visible = True
        lbattendiin.Refresh()
        Try
            'Pass values to the constructor. These values can be overridden by setting
            'the appropriate properties on the instance of the clsFTP class.
            'The third parameter is the user name. The FTP site is accessed with the user name.
            'If there is no specific user name, the user name can be anonymous.
            'The fourth parameter is the password. The FTP server is accessed with the password.
            'The fifth parameter is the port of the FTP server. The port of the FTP server is typically 21.

            lbattendiin.Visible = True
            lbattendiin.Refresh()
            If Directory.Exists(Globale.ImportFolder) = False Then
                Directory.CreateDirectory(Globale.ImportFolder)
            End If
            '
            ff = New clsFTP(pFtpserver, _
                                "/" & pRemoteFolder & "/", _
                                pFtpUser, _
                                pFtpPwd, _
                                21)
            'Try to log on to the FTP server.
            If (ff.Login() = True) Then
                '
                'Change the directory on your FTP site.
                If (ff.ChangeDirectory("/" & pRemoteFolder) = False) Then
                    'Successful changing the directory
                    If _g_auto Then
                        Me.WriteLog("Cambio directory fallito - ftpExport()")
                    Else
                        MsgBox("Cambio directory fallito", MsgBoxStyle.Critical, "ftpExport()")
                    End If
                    ff.CloseConnection()
                    Exit Sub
                End If
                '
                ff.SetBinaryMode(True)
                '
                Dim stringa As String() = ff.GetFileList("*.xml")
                Dim ii As Integer
                ProgressBar2in.Maximum = stringa.Length
                ProgressBar2in.Minimum = 0
                ProgressBar2in.Value = 0
                lbazionein.Text = "Download dei files..."
                lbazionein.Refresh()
                '
                For ii = LBound(stringa) To UBound(stringa) - 1
                    'Upload a file from your local hard disk to the FTP site.
                    Dim _fileName As String = stringa(ii).TrimEnd(vbCr) ' tolgo il ritorno a capo finale
                    lstUpd.Items.Add(_fileName)
                    lstUpd.Refresh()
                    Grandezza = ff.GetFileSize(_fileName)
                    lb_grandezzain.Text = Grandezza
                    lb_grandezzain.Refresh()
                    lb_progressin.Text = 0
                    lb_progressin.Refresh()
                    ff.DownloadFile(_fileName, Globale.ImportFolder & "\" & _fileName, lb_progressin)
                    ' ff.DownloadFile("exp050421.txt", "\My Documents\aggiornamenti\exp050421.txt")
                    'ImportaAggiornamenti(Global.CartellaAggLocale & "\" & lstAggiornamenti.Items(ii).Text)
                    If ff.DeleteFile(_fileName) = False Then
                        If _g_auto Then
                            Me.WriteLog("Cancellazione fallita " & " - ftpImport()")
                        Else
                            MsgBox("Cancellazione " & _fileName & " fallita !")
                        End If
                    End If
                    ProgressBar2in.Value = ProgressBar2in.Value + 1
                    ProgressBar2in.Refresh()
                Next ii
                '
            End If
            ff.CloseConnection()
        Catch ex As System.Exception
            'Display the error message.
            If _g_auto Then
                Me.WriteLog("Specific Error=" & ex.Message + vbCrLf + "Press 'ENTER' to EXIT" & " - ftpIxport()")
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
            lbazionein.Text = ""
            lbazionein.Refresh()
            lbattendiin.Visible = False
            lbattendiin.Refresh()
            '
            lb_grandezzain.Text = ""
            lb_grandezzain.Refresh()
            lb_progressin.Text = ""
            lb_progressin.Refresh()

            '

            ProgressBar2in.Value = 0
            ProgressBar2in.Refresh()
        End Try

    End Sub


    Private Sub ftpExportOld()
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
            Dim _fileList As ArrayList = Me.ElencaFileAgg(Globale.cartellaAggLocale)
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
            DeleteFileAgg(Globale.cartellaAggLocale)
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
    Private Function ElencaFileAgg(ByVal pPath As String) As ArrayList

        Try
            Dim _array As New ArrayList
            If Globale.gExtendLog = "S" Then
                Me.WriteLog("Looking for file to send into : " & Environment.CurrentDirectory.TrimEnd("\") & "\" & pPath)
            End If
            If Directory.Exists(pPath) = False Then
                Directory.CreateDirectory(pPath)
            End If
            Dim di As New DirectoryInfo(pPath)
            Dim fi As FileInfo
            Try
                For Each fi In di.GetFiles("*.xml")
                    Dim item As New ListViewItem(pPath)
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
            If _g_auto Then
                Me.WriteLog(ex.Message & " - ElencaFileAgg()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "Elenca file")
                Return Nothing
            End If
        End Try
    End Function
    Private Function DeleteFileAgg(ByVal pFold As String) As ArrayList

        Try
            If Globale.gExtendLog = "S" Then
                Me.WriteLog("Removing file from folder: xml")
            End If
            Dim _array As New ArrayList
            'If Directory.Exists(pFold) = False Then
            'Directory.CreateDirectory(Globale.cartellaAggLocale)
            'End If
            Dim di As New DirectoryInfo(pFold)
            Dim fi As FileInfo
            Try
                For Each fi In di.GetFiles()
                    Dim item As New ListViewItem(fi.Name)
                    IO.File.Delete(pFold & "\" & fi.Name)
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
    Private Function WinScpScript(ByVal pUser As String, ByVal pPasswd As String, ByVal pLocalPath As String, ByVal pHost As String) As Boolean
        'Open log file
        '
        Dim hScriptFile As New FileStream("script.txt", FileMode.Create)
        Try
            'hScriptFile = File.Open("script.txt", FileMode.Append)
            Dim lByteMess As Byte()
            '

            'Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
            '                         "put " & "xml/" & pLocalPath.TrimEnd("\") & "\*.*" & Constants.vbCrLf & _
            '                         "exit" & Constants.vbCrLf
            'Questa sintassi mi trasferisce solo il contenuto della cartella e non delle sotto cartelle
            '
            Dim lMessage As String = "open ftp://" & pUser & ":" & pPasswd & "@" & pHost & " -timeout=240" & Constants.vbCrLf & _
                                     "put -filemask=""*.xml|*/"" " & "xml/" & pLocalPath.TrimEnd("\") & "\*.xml" & Constants.vbCrLf & _
                                     "exit" & Constants.vbCrLf
            lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
            hScriptFile.Write(lByteMess, 0, lMessage.Length)
            '
            Return True
        Catch ex As Exception
            Return False
        Finally
            hScriptFile.Close()
            hScriptFile = Nothing
        End Try
    End Function
    Private Function WriteLog(ByVal pMessage As String) As Boolean
        Try
            'Open log file
            '
            Dim fileRenamed As String = gLogFile.Replace(".log", "_1.log")
            Dim _file As New FileInfo(gLogFile)
            If _file.Length > 1024000 Then '1M
                System.IO.File.Delete(fileRenamed)
                System.IO.File.Move(gLogFile, fileRenamed)
            End If
            '
            hLogFile = File.Open(gLogFile, FileMode.Append)
            Dim lHeader As String = Date.Now.ToString
            Dim lMessage As String = lHeader & " - " & pMessage & Constants.vbCrLf
            gBodyMail = gBodyMail & lMessage & vbCr
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
    Private Function WriteExportLog(ByVal pMessage As String) As Boolean
        Try
            'Open log file
            Dim fileRenamed As String = gLogExpFile.Replace(".log", "_1.log")
            Dim _file As New FileInfo(gLogExpFile)
            If _file.Length > 419430400 Then '400M
                System.IO.File.Delete(fileRenamed)
                System.IO.File.Move(gLogExpFile, fileRenamed)
            End If
            '
            hLogExpFile = File.Open(gLogExpFile, FileMode.Append)
            Dim lHeader As String = Date.Now.ToString
            Dim lMessage As String = lHeader & " - " & pMessage & Constants.vbCrLf
            Dim lByteMess As Byte()
            lByteMess = System.Text.Encoding.ASCII.GetBytes(lMessage)
            hLogExpFile.Write(lByteMess, 0, lMessage.Length)
            Return True
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "WriteExportLog")
            Return False
        Finally
            hLogExpFile.Close()
            hLogExpFile = Nothing
        End Try
    End Function
    Private Function updateFtpGrid() As Boolean
        Try
            Dim _rowCount As Integer = siteGrid.RowCount
            Dim __site As New sSite
            If Me.txtSeq.Text.Trim <> "" Then
                If gSiteList.ContainsKey(Me.txtSeq.Text) Then
                    __site = gSiteList(Me.txtSeq.Text)
                    With __site
                        .seq = Me.txtSeq.Text.Trim
                        .host = Me.txtFtpsrv.Text.Trim
                        .folder = Me.txtFolder.Text.Trim
                        .user = Me.txtUser.Text.Trim
                        .password = Me.txtPwd.Text.Trim
                        .httppost = Me.txtHttp.Text.Trim
                        If Me.chkAllItems.Checked Then
                            .allItems = "S"
                        Else
                            .allItems = "N"
                        End If
                    End With
                    gSiteList(Me.txtSeq.Text) = __site
                Else
                    With __site
                        .seq = Me.txtSeq.Text.Trim
                        .host = Me.txtFtpsrv.Text.Trim
                        .folder = Me.txtFolder.Text.Trim
                        .user = Me.txtUser.Text.Trim
                        .password = Me.txtPwd.Text.Trim
                        .httppost = Me.txtHttp.Text.Trim
                        If Me.chkAllItems.Checked Then
                            .allItems = "S"
                        Else
                            .allItems = "N"
                        End If
                    End With
                    gSiteList.Add(Me.txtSeq.Text, __site)
                End If
            End If
            Me.loadGrid()
            bFtpGridChanged = True
            cleanFtpFields()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "updateFtpGrid")
            Return False
        End Try
    End Function
    Private Function updateFtpGridIn() As Boolean
        Try
            Dim _rowCount As Integer = siteGridIn.RowCount
            Dim __site As New sSite
            If Me.txtSeqIn.Text.Trim <> "" Then
                If gSiteListIn.ContainsKey(Me.txtSeqIn.Text) Then
                    __site = gSiteListIn(Me.txtSeqIn.Text)
                    With __site
                        .seq = Me.txtSeqIn.Text.Trim
                        .host = Me.txtFtpSrvIn.Text.Trim
                        .folder = Me.txtFolderIn.Text.Trim
                        .user = Me.txtUserIn.Text.Trim
                        .password = Me.txtPwdIn.Text.Trim
                        .httppost = Me.txtHttpIn.Text.Trim
                    End With
                    gSiteListIn(Me.txtSeqIn.Text) = __site
                Else
                    With __site
                        .seq = Me.txtSeqIn.Text.Trim
                        .host = Me.txtFtpSrvIn.Text.Trim
                        .folder = Me.txtFolderIn.Text.Trim
                        .user = Me.txtUserIn.Text.Trim
                        .password = Me.txtPwdIn.Text.Trim
                        .httppost = Me.txtHttpIn.Text.Trim
                    End With
                    gSiteListIn.Add(Me.txtSeqIn.Text, __site)
                End If
            End If
            Me.loadGridIn()
            bFtpGridChangedIn = True
            cleanFtpFieldsIn()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "updateFtpGridIn")
            Return False
        End Try
    End Function

    Private Function deleteRecordFtpGrid() As Boolean
        Try
            If Me.txtSeq.Text.Trim <> "" Then
                If gSiteList.ContainsKey(Me.txtSeq.Text) Then
                    gSiteList.Remove(Me.txtSeq.Text)
                End If
            End If
            Me.loadGrid()
            bFtpGridChanged = True
            cleanFtpFields()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "updateFtpGrid")
            Return False
        End Try
    End Function
    Private Function deleteRecordFtpGridIn() As Boolean
        Try
            If Me.txtSeqIn.Text.Trim <> "" Then
                If gSiteListIn.ContainsKey(Me.txtSeqIn.Text) Then
                    gSiteListIn.Remove(Me.txtSeqIn.Text)
                End If
            End If
            Me.loadGridIn()
            bFtpGridChangedIn = True
            cleanFtpFieldsIn()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "updateFtpGridIn")
            Return False
        End Try
    End Function

    Private Function cleanFtpFields() As Boolean

        Try
            Me.txtSeq.Text = ""
            Me.txtFtpsrv.Text = ""
            Me.txtFolder.Text = ""
            Me.txtUser.Text = ""
            Me.txtPwd.Text = ""
            Me.txtHttp.Text = ""
            Me.chkAllItems.Checked = False
            Me.btnFilter.Enabled = False
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function cleanFtpFieldsIn() As Boolean

        Try
            Me.txtSeqIn.Text = ""
            Me.txtFtpSrvIn.Text = ""
            Me.txtFolderIn.Text = ""
            Me.txtUserIn.Text = ""
            Me.txtPwdIn.Text = ""
            Me.txtHttpIn.Text = ""
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function saveFtpGrid() As Boolean
        Try
            Dim sw As System.IO.FileStream
            Dim _file As String = "siteGrid.xml"
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
            For _ii As Integer = 0 To siteGrid.RowCount - 1
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                'Dim _pwd As String = System.Text.Encoding.ASCII.GetString(gCrypt.(siteGrid.Item("password", _ii).Value))
                Dim _pwd As String = CTripleDES.EncryptData(siteGrid.Item("password", _ii).Value)
                '
                stringa = Chr(9) & Chr(9) & "<seq>" & siteGrid.Item("Seq", _ii).Value & "</seq>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<host>" & siteGrid.Item("Host", _ii).Value & "</host>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<folder>" & siteGrid.Item("folder", _ii).Value & "</folder>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<user>" & siteGrid.Item("User", _ii).Value & "</user>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<password>" & _pwd & "</password>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<allitems>" & siteGrid.Item("All", _ii).Value & "</allitems>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<httppost>" & siteGrid.Item("post", _ii).Value & "</httppost>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            filewriter.Close()
            sw.Dispose()
            bFtpGridChanged = False
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "saveFtpGrid")
            Return False
        End Try
    End Function
    Private Function saveFtpGridIn() As Boolean
        Try
            Dim sw As System.IO.FileStream
            Dim _file As String = "siteGridIn.xml"
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
            For _ii As Integer = 0 To siteGridIn.RowCount - 1
                stringa = Chr(9) & "<row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                'Dim _pwd As String = System.Text.Encoding.ASCII.GetString(gCrypt.(siteGrid.Item("password", _ii).Value))
                Dim _pwd As String = CTripleDES.EncryptData(siteGridIn.Item("passwordin", _ii).Value)
                '
                stringa = Chr(9) & Chr(9) & "<seq>" & siteGridIn.Item("Seqin", _ii).Value & "</seq>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<host>" & siteGridIn.Item("Hostin", _ii).Value & "</host>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<folder>" & siteGridIn.Item("folderin", _ii).Value & "</folder>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<user>" & siteGridIn.Item("Userin", _ii).Value & "</user>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<password>" & _pwd & "</password>" & Chr(13) & Chr(10) & _
                          Chr(9) & Chr(9) & "<httppost>" & siteGridIn.Item("postin", _ii).Value & "</httppost>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
                '
                stringa = Chr(9) & "</row>" & Chr(13) & Chr(10)
                filewriter.Write(stringa)
                filewriter.Flush()
            Next
            stringa = "</dataroot>" & Chr(13) & Chr(10)
            filewriter.Write(stringa)
            filewriter.Flush()
            filewriter.Close()
            sw.Dispose()
            bFtpGridChangedIn = False
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "saveFtpGridIn")
            Return False
        End Try
    End Function

    Private Function readXmlFtpsite() As ArrayList

        Try
            Dim _manifest As New sManifest
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = "siteGrid.xml"
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
                        Case "seq"
                            gSite.seq = value
                        Case "host"
                            gSite.host = value
                        Case "folder"
                            gSite.folder = value
                        Case "user"
                            gSite.user = value
                        Case "password"
                            gSite.password = CTripleDES.DecryptData(value)
                        Case "httppost"
                            gSite.httppost = value
                        Case "allitems"
                            gSite.allItems = value
                    End Select
                Next
                If Not gSiteList.Contains(gSite.seq) Then
                    gSiteList.Add(gSite.seq, gSite)
                    gKsiteList.Add(gSite.seq)
                End If
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return _array
        Catch ex As IOException
            saveFtpGrid()
            Return Nothing
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readxmlfile")
            Return Nothing
        End Try

    End Function
    Private Function readXmlFtpsiteIn() As ArrayList

        Try
            Dim _manifest As New sManifest
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = "siteGridIn.xml"
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
                        Case "seq"
                            gSite.seq = value
                        Case "host"
                            gSite.host = value
                        Case "folder"
                            gSite.folder = value
                        Case "user"
                            gSite.user = value
                        Case "password"
                            gSite.password = CTripleDES.DecryptData(value)
                        Case "httppost"
                            gSite.httppost = value
                    End Select
                Next
                If Not gSiteListIn.Contains(gSite.seq) Then
                    gSiteListIn.Add(gSite.seq, gSite)
                End If
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            Return _array
        Catch ex As IOException
            saveFtpGridIn()
            Return Nothing
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readxmlfileIn")
            Return Nothing
        End Try

    End Function
    Private Function listXmlFileImp()
        Try
            Dim _fileList As ArrayList = Me.ElencaFileAgg(Globale.ImportFolder)
            For Each element As String In _fileList
                readXmlFileImp(Globale.ImportFolder & "\" & element, element)
            Next
        Catch ex As Exception

        End Try
    End Function
    Private Function readXmlFileImp(ByVal pFilename As String, ByVal pabsoluteName As String)

        Try
            Dim _manifest As New sManifest
            Dim XmlNodo As Xml.XmlNodeList
            Dim _array As New ArrayList
            Dim _File As String = pFilename
            Dim _tablename As String = ""
            Dim _ord As New sOrdini
            Dim _cli As New sClienti
            Dim _des As New sDesdive
            Dim gIndexOrd As Integer = 1
            Dim gIndexCli As Integer = 1
            Dim gIndexDes As Integer = 1
            'oggetto per il file xml
            Dim Xmlfile As New XmlDocument
            '
            Dim _rowValue As String = ""
            Dim _propValue As String = ""
            Dim _bspese As Boolean = False
            '
            gOrd.Clear()
            gk_Ord.Clear()
            gCli.Clear()
            gDes.Clear()
            '
            'carico il file
            Dim XmlLeggi As New XmlTextReader(_File)

            XmlLeggi.WhitespaceHandling = WhitespaceHandling.None

            Xmlfile.Load(XmlLeggi)


            XmlNodo = Xmlfile.GetElementsByTagName("row")
            For Each nodo As XmlNode In XmlNodo
                For Each element As XmlNode In nodo.ChildNodes
                    '
                    Dim name As String = element.Name
                    Dim value As String = htmlDecode(element.InnerText)
                    If name = "tablename" Then
                        _tablename = value
                    End If
                    '
                    If name = "fields" Then
                        Select Case _tablename
                            Case "ORDINI"
                                For Each _field As XmlNode In element.ChildNodes
                                    With _ord
                                        Select Case _field.Name
                                            Case "ODNUMORD"
                                                .ODNUMORD = _field.InnerText
                                            Case "ODDATORD"
                                                .ODDATORD = _field.InnerText
                                            Case "ODCODCLI"
                                                .ODCODCLI = _field.InnerText
                                            Case "ODPARIVA"
                                                .ODPARIVA = _field.InnerText
                                            Case "ODCODFIS"
                                                .ODCODFIS = _field.InnerText
                                            Case "ODCODART"
                                                .ODCODART = _field.InnerText
                                            Case "ODUNIMIS"
                                                .ODUNIMIS = _field.InnerText
                                            Case "ODQTAMOV"
                                                .ODQTAMOV = _field.InnerText
                                            Case "ODPREZZO"
                                                .ODPREZZO = _field.InnerText
                                            Case "ODSCONT1"
                                                .ODSCONT1 = _field.InnerText
                                            Case "ODSCONT2"
                                                .ODSCONT2 = _field.InnerText
                                            Case "ODSCONT3"
                                                .ODSCONT3 = _field.InnerText
                                            Case "ODSCONT4"
                                                .ODSCONT4 = _field.InnerText
                                            Case "ODCODDES"
                                                .ODCODDES = _field.InnerText
                                            Case "ODDATEVA"
                                                .ODDATEVA = _field.InnerText
                                            Case "ODNOTORD"
                                                .ODNOTORD = _field.InnerText
                                            Case "ODNOTAGG"
                                                .ODNOTAGG = _field.InnerText
                                            Case "OD_EMAIL"
                                                .OD_EMAIL = _field.InnerText
                                            Case "ODSPETRA"
                                                .ODSPETRA = CTran(_field.InnerText, 0)
                                        End Select
                                    End With
                                Next
                                _ord.ODTIPRIG = "R"
                                Dim gIndexOrdStr As String = gIndexOrd.ToString.Trim.PadLeft(4, "0")
                                gOrd.Add(_ord.ODNUMORD & "-" & _ord.ODCODART & "_" & gIndexOrdStr.Trim, _ord)
                                gk_Ord.Add(_ord.ODNUMORD & "-" & _ord.ODCODART & "_" & gIndexOrdStr.Trim)
                                '
                                'This row (transportation charge) come be addded only if this is a private customer order
                                '31-03-2021 Questo lo tolgo pechè le spese di trasporto vengono indicate nella testata del documento
                                If Not _bspese And _ord.ODPARIVA.Trim = "" And 1 = 0 Then
                                    _ord.ODTIPRIG = "F"
                                    _ord.ODCODART = "SPESE DI TRASPORTO"
                                    _ord.ODQTAMOV = 1
                                    _ord.ODSCONT1 = 0
                                    _ord.ODSCONT2 = 0
                                    _ord.ODSCONT3 = 0
                                    _ord.ODSCONT4 = 0
                                    _ord.ODPREZZO = _ord.ODSPETRA
                                    '
                                    gOrd.Add(_ord.ODNUMORD & "-" & _ord.ODCODART & "_" & "9999", _ord)
                                    gk_Ord.Add(_ord.ODNUMORD & "-" & _ord.ODCODART & "_" & "9999")
                                    '
                                    _bspese = True
                                End If
                                '
                                If Not gNumOrd.Contains(_ord.ODNUMORD) Then
                                    gNumOrd.Add(_ord.ODNUMORD)
                                End If
                                gIndexOrd += 1
                            Case "CLIENTI"
                                For Each _field As XmlNode In element.ChildNodes
                                    With _cli
                                        Select Case _field.Name
                                            Case "ANDESCRI"
                                                .ANDESCRI = fCut(_field.InnerText, 60)
                                            Case "ANDESCR1"
                                                .ANDESCR1 = fCut(_field.InnerText, 60)
                                            Case "ANINDIRI"
                                                .ANINDIRI = fCut(_field.InnerText, 35)
                                            Case "ANINDIR1"
                                                .ANINDIR1 = fCut(_field.InnerText, 35)
                                            Case "AN___CAP"
                                                .AN___CAP = fCut(_field.InnerText, 9)
                                            Case "ANLOCALI"
                                                .ANLOCALI = fCut(_field.InnerText, 30)
                                            Case "ANPROVIN"
                                                .ANPROVIN = fCut(_field.InnerText, 2)
                                            Case "ANNAZION"
                                                .ANNAZION = fCut(_field.InnerText, 3)
                                            Case "ANTELEFO"
                                                .ANTELEFO = fCut(_field.InnerText, 18)
                                            Case "ANTELFAX"
                                                .ANTELFAX = fCut(_field.InnerText, 18)
                                            Case "ANNUMCEL"
                                                .ANNUMCEL = fCut(_field.InnerText, 18)
                                            Case "ANPARIVA"
                                                .ANPARIVA = fCut(_field.InnerText, 12)
                                            Case "ANCODFIS"
                                                .ANCODFIS = fCut(_field.InnerText, 16)
                                            Case "ANCODPAG"
                                                .ANCODPAG = fCut(_field.InnerText, 5)
                                            Case "AN_EMAIL"
                                                .AN_EMAIL = fCut(_field.InnerText, 254)
                                            Case "ANPASSWD"
                                                .ANPASSWD = fCut(_field.InnerText, 30)
                                        End Select
                                    End With
                                Next
                                Dim _email As String = element.ChildNodes.Item(10).InnerText.Trim()
                                If _email <> "##AN_EMAIL##" Then  'Contenuto non corretto, scarto il record
                                    gCli.Add(gIndexCli.ToString.Trim, _cli)
                                End If
                                gIndexCli += 1
                            Case "DES_DIVE"
                                For Each _field As XmlNode In element.ChildNodes
                                    With _des
                                        Select Case _field.Name
                                            Case "DD___CAP"
                                                .DD___CAP = fCut(_field.InnerText, 8)
                                            Case "DDCODDES"
                                                .DDCODDES = fCut(_field.InnerText, 5)
                                            Case "DDCODFIS"
                                                .DDCODFIS = _field.InnerText
                                            Case "DDCODICE"
                                                .DDCODICE = fCut(_field.InnerText, 15)
                                            Case "DDDTOBSO"
                                                .DDDTOBSO = _field.InnerText
                                            Case "DDINDIRI"
                                                .DDINDIRI = fCut(_field.InnerText, 35)
                                            Case "DDLOCALI"
                                                .DDLOCALI = fCut(_field.InnerText, 30)
                                            Case "DDNOMDES"
                                                .DDNOMDES = fCut(_field.InnerText, 40)
                                            Case "DDPARIVA"
                                                .DDPARIVA = _field.InnerText
                                            Case "DDPROVIN"
                                                .DDPROVIN = fCut(_field.InnerText, 2)
                                            Case "DDTELEFO"
                                                .DDTELEFO = fCut(_field.InnerText, 18)
                                            Case "DDEMAIL"
                                                .DD_EMAIL = fCut(_field.InnerText, 254).Trim
                                            Case "DD__NOTE"
                                                .DD__NOTE = fCut(_field.InnerText, 40)
                                        End Select
                                    End With
                                Next
                                gDes.Add(gIndexDes.ToString.Trim, _des)
                                gIndexDes += 1
                        End Select
                    End If
                Next
            Next
            Xmlfile = New XmlDocument
            XmlLeggi.Close()
            XmlLeggi = Nothing
            If Not Me.checkData(pFilename) Then
                Me.mkCLienti()
                '
                Dim _coddes As String = Me.mkDesdive()
                Me.MakeOrder(_coddes)
                If Not System.IO.Directory.Exists("importati") Then
                    System.IO.Directory.CreateDirectory("importati")
                End If
                Try
                    System.IO.File.Move(pFilename, "importati\\" & pabsoluteName)
                Catch ex As Exception
                Finally
                    System.IO.File.Delete(pFilename)
                End Try
            End If
            lstUpd.Items.Add(" ")
            lstUpd.Refresh()
            lstUpd.Items.Add("Operazione terminata!")
            lstUpd.Refresh()
        Catch ex As IOException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readxmlfileImp")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "readxmlfileImp")
        End Try

    End Function
    Private Function fCut(ByVal pString As String, ByVal plen As Integer) As String
        Try
            If pString.Length > plen Then
                Return pString.Substring(0, plen)
            Else
                Return pString
            End If
        Catch ex As Exception

        End Try
    End Function
    Private Function loadGrid() As Boolean
        Try
            siteGrid.Rows.Clear()
            Dim _rowCount As Integer = gSiteList.Count
            siteGrid.RowCount = _rowCount
            Dim _ii As Integer = 0
            For Each element As String In gSiteList.Keys
                gSite = gSiteList(element)
                With gSite
                    siteGrid.Item("Seq", _ii).Value = .seq
                    siteGrid.Item("Host", _ii).Value = .host
                    siteGrid.Item("folder", _ii).Value = .folder
                    siteGrid.Item("User", _ii).Value = .user
                    siteGrid.Item("password", _ii).Value = .password
                    siteGrid.Item("post", _ii).Value = .httppost
                    siteGrid.Item("All", _ii).Value = .allItems
                End With
                _ii += 1
            Next
            siteGrid.Sort(siteGrid.Columns(0), ComponentModel.ListSortDirection.Ascending)
            siteGrid.Refresh()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadGrid")
            Return False
        End Try
    End Function
    Private Function loadGridIn() As Boolean
        Try
            siteGridIn.Rows.Clear()
            Dim _rowCount As Integer = gSiteListIn.Count
            siteGridIn.RowCount = _rowCount
            Dim _ii As Integer = 0
            For Each element As String In gSiteListIn.Keys
                gSite = gSiteListIn(element)
                With gSite
                    siteGridIn.Item("Seqin", _ii).Value = .seq
                    siteGridIn.Item("Hostin", _ii).Value = .host
                    siteGridIn.Item("folderin", _ii).Value = .folder
                    siteGridIn.Item("Userin", _ii).Value = .user
                    siteGridIn.Item("passwordin", _ii).Value = .password
                    siteGridIn.Item("postin", _ii).Value = .httppost
                End With
                _ii += 1
            Next
            siteGridIn.Sort(siteGridIn.Columns(0), ComponentModel.ListSortDirection.Ascending)
            siteGridIn.Refresh()
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "loadGridIn")
            Return False
        End Try
    End Function

    Private Sub btnAggiorna_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAggiorna.Click
        updateFtpGrid()
    End Sub
    Private Sub btnAggiornaIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAggiornaIn.Click
        updateFtpGridIn()
    End Sub

    Private Sub siteGrid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles siteGrid.CellClick
        Try
            Me.txtSeq.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("Seq").Value
            Me.txtFtpsrv.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("Host").Value
            Me.txtFolder.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("folder").Value
            Me.txtUser.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("User").Value
            Me.txtPwd.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("password").Value
            Me.txtHttp.Text = siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("post").Value
            If siteGrid.Rows.Item(siteGrid.SelectedCells.Item(0).RowIndex).Cells("All").Value.ToString.Trim = "S" Then
                Me.chkAllItems.Checked = True
                Me.btnFilter.Enabled = False
            Else
                Me.chkAllItems.Checked = False
                Me.btnFilter.Enabled = True
            End If
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "sitegridClick")
        End Try
    End Sub
    Private Sub siteGridIn_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles siteGridIn.CellClick
        Try
            Me.txtSeqIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("Seqin").Value
            Me.txtFtpSrvIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("Hostin").Value
            Me.txtFolderIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("folderin").Value
            Me.txtUserIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("Userin").Value
            Me.txtPwdIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("passwordin").Value
            Me.txtHttpIn.Text = siteGridIn.Rows.Item(siteGridIn.SelectedCells.Item(0).RowIndex).Cells("postin").Value
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "sitegridClickIn")
        End Try
    End Sub
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Me.saveFtpGrid()
    End Sub

    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Me.deleteRecordFtpGrid()
    End Sub
    Private Sub btnSalvaIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalvaIn.Click
        Me.saveFtpGridIn()
    End Sub

    Private Sub btnEliminain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaIn.Click
        Me.deleteRecordFtpGridIn()
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub btnImporta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImporta.Click
        Me.importData()
    End Sub
    Private Function getCodeByEmail(ByVal pEmail As String) As String
        Try
            Dim _strret As String = ""
            Dim strsql As String = "SELECT UTCODCLI FROM " & op.getTablename("ASYSUTEWEB") & " WHERE UT_EMAIL = " & op.ValAdapter(pEmail, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                _strret = CTran(ds.Tables(0).Rows(0).Item("UTCODCLI"), "")
            End If
            Return _strret
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getCodeByEmail")
        End Try
    End Function
    Private Function getCodeByPIvaCf(ByVal pPiva As String, ByVal pCf As String) As String
        Try
            Dim _conto As String = ""
            If pPiva.Trim <> "" And pPiva.Trim <> "NULL" Then
                Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANPARIVA", "C" & "," & pPiva, False)
                Try
                    _conto = CTran(adhoc.getVal(hConti("ANCODICE")), "")
                Catch ex As Exception
                    _conto = ""
                End Try
            End If
            If _conto = "" Then
                If pCf.Trim <> "" And pCf.Trim <> "NULL" Then
                    Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODFIS", "C" & "," & pCf, False)
                    Try
                        _conto = CTran(adhoc.getVal(hConti("ANCODICE")), "")
                    Catch ex As Exception
                        _conto = ""
                    End Try
                End If
            End If
            Return _conto
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getCodeByPIvaCf")
        End Try
    End Function
    Private Sub MakeOrder(ByVal pCoddes As String)
        Try
            '
            Dim _ord As New sOrdini
            Dim _cli As New sClienti
            Dim _bMast As Boolean = False
            Dim _clipriv As New sClipriv
            Dim _despriv As New sDesPriv
            Dim htipdocu As New Hashtable
            Dim _parDett As New sPardocdett
            '
            Me.gk_Ord.Sort()
            Me.gNumOrd.Sort()
            '
            lstUpd.Items.Add("Inserimento ordini:")
            lstUpd.Refresh()
            For Each _order As String In gNumOrd
                '
                Dim _savecprown As Integer = 0
                Dim _mvserial As String = ""
                Dim _codcli As String = ""
                For Each element As String In gk_Ord
                    _ord = Me.gOrd(element)
                    If _ord.ODNUMORD = _order Then
                        Dim _parMast As New sPardocmast
                        '
                        If _ord.ODPARIVA.ToString.Trim <> "" Then
                            htipdocu = adhoc.readAdhocTable("TIP_DOCU", "TDTIPDOC", Globale.DocType, False)
                        Else
                            htipdocu = adhoc.readAdhocTable("TIP_DOCU", "TDTIPDOC", Globale.gCauCorrisp, False)
                        End If
                        '
                        If Not _bMast Then
                            'Make master document
                            If _ord.ODPARIVA.ToString.Trim <> "" Then
                                _parMast.tipcon = "C"
                                _parMast.cladoc = "OV"
                                _parMast.spetra = _ord.ODSPETRA
                            Else
                                _parMast.tipcon = "C"
                                _parMast.cladoc = "RF"
                                _parMast.spetra = 0
                            End If
                            'If Globale.CodAzi = "COPTG" Then
                            '_ord.ODCODCLI = Me.getOrInsertCust(_ord.OD_EMAIL)
                            'End If
                            Select Case Globale.CodAzi
                                Case "COPTG"
                                    '18-03-2021 Per copt i privati vengono trattati come i clienti con p.iva
                                    _parMast.tipcon = "C"
                                    _parMast.cladoc = "OV"
                                    _parMast.spetra = _ord.ODSPETRA
                                    '
                                    _ord.ODCODCLI = Me.getCodeByPIvaCf(_ord.ODPARIVA, _ord.ODCODFIS)
                                    If _ord.ODCODCLI = "" Or _ord.ODCODCLI.StartsWith("w") Or _ord.ODCODCLI.StartsWith("t") Then
                                        _ord.ODCODCLI = Me.getOrInsertCust(_ord.OD_EMAIL)
                                    End If
                                    _codcli = _ord.ODCODCLI
                                    _parMast.caucon = ""
                                    Dim hPagamen As New Hashtable
                                    Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODICE", "C" & "," & _codcli, False)
                                    Dim _codpag As String = CTran(CType(hConti("ANCODPAG"), s_tableValue).mValue, "")
                                    hPagamen = adhoc.readAdhocTable("PAG_AMEN", "PACODICE", _codpag, False)
                                    _parMast.speinc = adhoc.getVal(hPagamen("PASPEINC"))
                                    If 1 = 0 Then
                                        If _ord.ODPARIVA.ToString.Trim <> "" Then
                                            _ord.ODCODCLI = Me.getCodeByPIvaCf(_ord.ODPARIVA, _ord.ODCODFIS)
                                            If _ord.ODCODCLI = "" Or _ord.ODCODCLI.StartsWith("w") Or _ord.ODCODCLI.StartsWith("t") Then
                                                _ord.ODCODCLI = Me.getOrInsertCust(_ord.OD_EMAIL)
                                            End If
                                            _codcli = _ord.ODCODCLI
                                            _parMast.caucon = ""
                                            'Dim hPagamen As New Hashtable
                                            'Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODICE", "C" & "," & _codcli, False)
                                            'Dim _codpag As String = CTran(CType(hConti("ANCODPAG"), s_tableValue).mValue, "")
                                            'hPagamen = adhoc.readAdhocTable("PAG_AMEN", "PACODICE", _codpag, False)
                                            '_parMast.speinc = adhoc.getVal(hPagamen("PASPEINC"))
                                        Else
                                            'Cliente privato
                                            _ord.ODCODCLI = ""
                                            _codcli = ""
                                            _clipriv = hCliPriv(_ord.OD_EMAIL.ToString.Trim)
                                            _despriv = hDesPriv(_ord.OD_EMAIL.ToString.Trim)
                                            With _parMast
                                                .nomepriv = _clipriv.CognomeNome.ToString.PadRight(40, " ")
                                                .indiripriv = IIf(IsNothing(_despriv), _clipriv.Indirizzo, _despriv.Indirizzo).ToString.PadRight(35, " ")
                                                .cappriv = IIf(IsNothing(_despriv), _clipriv.Cap, _despriv.Cap).ToString.PadRight(8, " ")
                                                .localipriv = IIf(IsNothing(_despriv), _clipriv.locali, _despriv.locali).ToString.PadRight(30, " ")
                                                .provinpriv = IIf(IsNothing(_despriv), _clipriv.provin, _despriv.provin).ToString.PadRight(2, " ")
                                                .codfiscpriv = _clipriv.codfis.ToString.PadRight(16, " ")
                                                .codpag = _clipriv.codpag
                                            End With
                                            _parMast.caucon = adhoc.getVal(htipdocu("TDCAUCON"))
                                        End If
                                    End If

                                Case "ALTRQ"
                                    If _ord.ODCODCLI = "" Or _ord.ODCODCLI.StartsWith("w") Or _ord.ODCODCLI.StartsWith("t") Then
                                        _ord.ODCODCLI = Me.getCodeByPIvaCf(_ord.ODPARIVA, _ord.ODCODFIS)
                                    End If
                                    If _ord.ODCODCLI = "" Or _ord.ODCODCLI.StartsWith("w") Or _ord.ODCODCLI.StartsWith("t") Then
                                        MsgBox("Ordine : " & _ord.ODNUMORD & "; cliente sconosciuto, PIVA : " & _ord.ODPARIVA & " C.F. : " & _ord.ODCODFIS)
                                        Continue For
                                    End If
                                    _parMast.caucon = ""
                            End Select
                            '
                            _parMast.ordweb = _ord.ODNUMORD
                            _parMast.codcli = _ord.ODCODCLI
                            _parMast.coddes = _ord.ODCODDES
                            If _parMast.coddes = "" Then
                                _parMast.coddes = pCoddes ' If destination code does not exist it means that there is a new destinatian in the imported file
                            End If
                            _parMast.desdoc = _ord.ODNOTORD
                            '
                            Me.calcAmountDoc(_parMast, Me.gOrd, _order)
                            _parMast.serial = Me.mkDocMast(_parMast, True)
                            _mvserial = _parMast.serial
                            If _mvserial <> "" Then
                                Me.mkDocrate(_parMast)
                            Else
                                If _parMast.codcli.Trim <> "" Then
                                    lstUpd.Items.Add("Ordine : " & _ord.ODNUMORD.ToString.Trim & " non importato!")
                                    lstUpd.Refresh()
                                Else
                                    lstUpd.Items.Add("Corrispettivo : " & _ord.ODNUMORD.ToString.Trim & " non importato!")
                                    lstUpd.Refresh()
                                End If
                            End If
                            _bMast = True
                        End If
                        'Dim htipdocu As New Hashtable
                        If _mvserial <> "" Then
                            'Make document detail
                            'If _ord.ODPARIVA.ToString.Trim <> "" Then
                            'htipdocu = adhoc.readAdhocTable("TIP_DOCU", "TDTIPDOC", Globale.DocType, False)
                            'Else
                            'htipdocu = adhoc.readAdhocTable("TIP_DOCU", "TDTIPDOC", Globale.gCauCorrisp, False)
                            'End If
                            Dim hCauMag As Hashtable = adhoc.readAdhocTable("CAM_AGAZ", "CMCODICE", CType(htipdocu("TDCAUMAG"), s_tableValue).mValue, False)
                            '
                            'insert here the code to make detail document
                            _parDett = New sPardocdett
                            _parDett.serial = _mvserial
                            _parDett.cpronum = _savecprown
                            _parDett.flimpe = adhoc.getVal(hCauMag("CMFLIMPE"))
                            _parDett.flordi = adhoc.getVal(hCauMag("CMFLORDI"))
                            _parDett.flrise = adhoc.getVal(hCauMag("CMFLRISE"))
                            _parDett.codart = _ord.ODCODART
                            _parDett.caumag = adhoc.getVal(htipdocu("TDCAUMAG"))
                            _parDett.segno = adhoc.getVal(htipdocu("TD_SEGNO"))
                            _parDett.tiprig = _ord.ODTIPRIG
                            '_parDett.cpronum = 0
                            _parDett.qtamov = _ord.ODQTAMOV
                            _parDett.numlis = _parMast.numlis
                            _parDett.clifor = _ord.ODCODCLI
                            _parDett.dateva = Globale.g_SysData
                            _parDett.prezzo = _ord.ODPREZZO
                            _parDett.scont1 = _ord.ODSCONT1
                            _parDett.scont2 = _ord.ODSCONT2
                            _parDett.scont3 = _ord.ODSCONT3
                            _parDett.scont4 = _ord.ODSCONT4
                            '
                            'Temporaneo
                            'Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODICE", "C" & "," & _codcli, False)
                            'Dim _codage As String = CTran(adhoc.getVal(hConti("ANCODAG1")), "")
                            'If _codage.Trim = "13" Or _codage.Trim = "4" Then
                            'If (_parDett.scont1 * -1) < 5 Then
                            '_parDett.scont1 = -5
                            'End If
                            ''
                            ''If _parDett.scont1 = 0 Then
                            ''_parDett.scont1 = -5
                            ''ElseIf _parDett.scont2 = 0 Then
                            ''_parDett.scont2 = -5
                            ''ElseIf _parDett.scont3 = 0 Then
                            ''_parDett.scont3 = -5
                            ''ElseIf _parDett.scont4 = 0 Then
                            ''_parDett.scont4 = -5
                            'End If
                            '
                            'End If
                            '
                            _savecprown = Me.mkDocDett(_parDett)
                            If _savecprown <> 0 Then
                                Me.updateSaldiTab(_parDett)
                            End If
                            '
                        Else
                            lstUpd.Items.Add("Articolo : " & _ord.ODCODART & " non inserito nell' ordine!")
                            lstUpd.Refresh()
                        End If
                    End If
                Next
                _bMast = False
                '
            Next
            lstUpd.Items.Add("Operazione completata!")
            lstUpd.Refresh()
            '
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "MakeOrder")
        End Try
    End Sub

    Private Function mkDocMast(ByRef pPar As sPardocmast, ByVal pNewdoc As Boolean) As String
        Dim _tipdoc As String = ""
        Try
            adhoc.hFieldVal.Clear()
            If pNewdoc Then
                If pPar.codcli.ToString.Trim = "" Then
                    _tipdoc = Globale.gCauCorrisp
                Else
                    _tipdoc = Globale.DocType
                End If
                '
                Dim _codpag As String = ""
                Dim hTipdocu As Hashtable = adhoc.readAdhocTable("TIP_DOCU", "TDTIPDOC", _tipdoc, False)
                Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODICE", pPar.tipcon & "," & pPar.codcli, False)
                Dim hContropa As Hashtable = adhoc.readAdhocTable("CONTROPA", "COCODAZI", Globale.CodAzi, True)
                '
                Dim hPagamen As New Hashtable
                If pPar.codcli.ToString.Trim <> "" Then
                    _codpag = CTran(CType(hConti("ANCODPAG"), s_tableValue).mValue, "")
                    hPagamen = adhoc.readAdhocTable("PAG_AMEN", "PACODICE", _codpag, False)
                Else
                    hPagamen = adhoc.readAdhocTable("PAG_AMEN", "PACODICE", pPar.codpag, False)
                    _codpag = pPar.codpag
                End If
                '
                Dim hArticolo As New Hashtable
                'Getting serial number
                Dim _Serial As String = adhoc.getSerial()
                'Getting document number
                Dim _alfa As String = ""
                Dim _docType As String = ""
                Select Case Globale.CodAzi
                    Case "ALTRQ"
                        If adhoc.getVal(hConti("ANPARIVA")).ToString.Trim <> "" Then
                            _docType = Globale.DocType
                            _alfa = ""
                        Else
                            _docType = Globale.DocType_P
                            _alfa = "P"
                        End If
                    Case "COPTG"
                        _docType = _tipdoc
                        _alfa = ""
                End Select
                Dim _numdoc As Integer = 0
                If pPar.codcli.Trim <> "" Then
                    _numdoc = adhoc.getProg(Year(Globale.g_SysData), pPar.cladoc, _alfa)
                    lstUpd.Items.Add("ordine " & _numdoc & "/" & _alfa & "  cliente : " & pPar.codcli)
                    lstUpd.Refresh()
                Else
                    _numdoc = adhoc.getProg(Year(Globale.g_SysData), "RV", _alfa)
                    lstUpd.Items.Add("Corrispettivo " & _numdoc & "/" & _alfa & "  cliente : " & pPar.nomepriv)
                    lstUpd.Refresh()
                End If
                '
                Dim strsql As String = ""
                '
                pPar.numlis = CTran(adhoc.getVal(hConti("ANNUMLIS")), Globale.ListDef)
                '
                If _codpag <> "" Then
                    pPar.speinc = adhoc.getVal(hPagamen("PASPEINC"))
                    pPar.codpag = adhoc.getVal(hConti("ANCODPAG"))
                Else
                    pPar.speinc = 0
                    pPar.codpag = ""
                End If
                '
                If pPar.codcli.Trim <> "" Then
                    Dim hDesdive As Hashtable = adhoc.readAdhocTable("DES_DIVE", "DDTIPCON,DDCODICE,DDCODDES", "C" & "," & adhoc.getVal(hConti("ANCODICE")) & "," & pPar.coddes, False)
                    If IsNothing(hDesdive("DDCODDES")) Then
                        pPar.coddes = ""
                    End If
                    '
                    adhoc.hFieldVal.Add("MVCODCON", adhoc.getVal(hConti("ANCODICE")))
                    If CTran(adhoc.getVal(hConti("ANCODAG1")), "") <> "" Then
                        adhoc.hFieldVal.Add("MVCODAGE", adhoc.getVal(hConti("ANCODAG1")))
                    End If
                    If _codpag <> "" Then
                        adhoc.hFieldVal.Add("MVCODPAG", adhoc.getVal(hConti("ANCODPAG")))
                    End If
                    If CTran(adhoc.getVal(hConti("ANCODBAN")), "") <> "" Then
                        adhoc.hFieldVal.Add("MVCODBAN", adhoc.getVal(hConti("ANCODBAN")))
                    End If
                    If CTran(adhoc.getVal(hConti("ANCODBA2")), "") <> "" Then
                        adhoc.hFieldVal.Add("MVCODBA2", adhoc.getVal(hConti("ANCODBA2")))
                    End If
                    adhoc.hFieldVal.Add("MVNUMCOR", CTran(adhoc.getVal(hConti("ANNUMCOR")), ""))
                    If pPar.numlis <> "" Then
                        adhoc.hFieldVal.Add("MVTCOLIS", pPar.numlis)
                    End If
                    adhoc.hFieldVal.Add("MVSCOCL1", adhoc.getVal(hConti("AN1SCONT")))
                    'adhoc.hFieldVal.Add("MVSCOCL2", adhoc.getVal(hConti("AN1SCONT")))
                    If pPar.coddes <> "" Then
                        adhoc.hFieldVal.Add("MVCODDES", pPar.coddes)
                    End If
                    adhoc.hFieldVal.Add("MVFLSCOR", "N")
                Else
                    '
                    'Descrizione cliente privato
                    '
                    Dim _mv_note As String = pPar.nomepriv & pPar.indiripriv & pPar.cappriv & pPar.localipriv & pPar.provinpriv & pPar.codfiscpriv
                    adhoc.hFieldVal.Add("MV__NOTE", _mv_note)
                    adhoc.hFieldVal.Add("MVPRD", "RV")
                    '
                    Dim _accont As Decimal = pPar.aimpn1 + pPar.aimpn2 + pPar.aimpn3 + pPar.aimpn4 + pPar.aimpn5 + pPar.aimpn6 +
                                             pPar.aimps1 + pPar.aimps2 + pPar.aimps3 + pPar.aimps4 + pPar.aimps5 + pPar.aimps6
                    adhoc.hFieldVal.Add("MVACCONT", _accont)
                    adhoc.hFieldVal.Add("MVFLSCOR", "S")
                    adhoc.hFieldVal.Add("MVCAUCON", adhoc.getVal(hTipdocu("TDCAUCON")))
                    '
                End If
                '
                adhoc.hFieldVal.Add("MVTCAMAG", adhoc.getVal(hTipdocu("TDCAUMAG")))
                adhoc.hFieldVal.Add("MV_SEGNO", adhoc.getVal(hTipdocu("TD_SEGNO")))
                adhoc.hFieldVal.Add("MVCLADOC", adhoc.getVal(hTipdocu("TDCATDOC")))
                '
                adhoc.hFieldVal.Add("MVIVAINC", adhoc.getVal(hContropa("COCOIINC")))
                adhoc.hFieldVal.Add("MVIVATRA", adhoc.getVal(hContropa("COCOITRA")))
                adhoc.hFieldVal.Add("MVIVAIMB", adhoc.getVal(hContropa("COCOIIMB")))
                adhoc.hFieldVal.Add("MVIVABOL", adhoc.getVal(hContropa("COCOIBOL")))
                '
                adhoc.hFieldVal.Add("MVSPEINC", pPar.speinc)
                If _codpag <> "" Then
                    adhoc.hFieldVal.Add("MVSCOPAG", adhoc.getVal(hPagamen("PASCONTO")))
                End If
                '
                adhoc.hFieldVal.Add("MVSPETRA", pPar.spetra)
                adhoc.hFieldVal.Add("MVAIMPN1", pPar.aimpn1)
                adhoc.hFieldVal.Add("MVAFLOM1", IIf(pPar.aimpn1 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS1", pPar.aimps1)
                adhoc.hFieldVal.Add("MVACIVA1", pPar.aciva1)
                '
                adhoc.hFieldVal.Add("MVAIMPN2", pPar.aimpn2)
                adhoc.hFieldVal.Add("MVAFLOM2", IIf(pPar.aimpn2 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS2", pPar.aimps2)
                adhoc.hFieldVal.Add("MVACIVA2", pPar.aciva2)
                '
                adhoc.hFieldVal.Add("MVAIMPN3", pPar.aimpn3)
                adhoc.hFieldVal.Add("MVAFLOM3", IIf(pPar.aimpn3 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS3", pPar.aimps3)
                adhoc.hFieldVal.Add("MVACIVA3", pPar.aciva3)
                '
                adhoc.hFieldVal.Add("MVAIMPN4", pPar.aimpn4)
                adhoc.hFieldVal.Add("MVAFLOM4", IIf(pPar.aimpn4 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS4", pPar.aimps4)
                adhoc.hFieldVal.Add("MVACIVA4", pPar.aciva4)
                '
                adhoc.hFieldVal.Add("MVAIMPN5", pPar.aimpn5)
                adhoc.hFieldVal.Add("MVAFLOM5", IIf(pPar.aimpn5 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS5", pPar.aimps5)
                adhoc.hFieldVal.Add("MVACIVA5", pPar.aciva5)
                '
                adhoc.hFieldVal.Add("MVAIMPN6", pPar.aimpn6)
                adhoc.hFieldVal.Add("MVAFLOM6", IIf(pPar.aimpn6 > 0, "X", ""))
                adhoc.hFieldVal.Add("MVAIMPS6", pPar.aimps6)
                adhoc.hFieldVal.Add("MVACIVA6", pPar.aciva6)
                '
                '
                adhoc.hFieldVal.Add("MVSERIAL", _Serial)
                adhoc.hFieldVal.Add("MVNUMREG", _numdoc)
                adhoc.hFieldVal.Add("MVDATREG", Now.Date)
                adhoc.hFieldVal.Add("MVTIPDOC", _docType)
                adhoc.hFieldVal.Add("MVNUMDOC", _numdoc)
                adhoc.hFieldVal.Add("MVALFDOC", _alfa)
                adhoc.hFieldVal.Add("MVDATDOC", Globale.g_SysData)
                adhoc.hFieldVal.Add("MVDATPLA", Globale.g_SysData)
                adhoc.hFieldVal.Add("MVDATTRA", Globale.g_SysData)
                adhoc.hFieldVal.Add("MVANNDOC", Globale.g_SysData.Year.ToString)
                adhoc.hFieldVal.Add("MVDATCIV", Globale.g_SysData)
                adhoc.hFieldVal.Add("UTCC", "1")
                adhoc.hFieldVal.Add("UTDV", Globale.g_SysData)
                adhoc.hFieldVal.Add("MV__ANNO", Year(Globale.g_SysData))
                adhoc.hFieldVal.Add("MVCODESE", Year(Globale.g_SysData))
                adhoc.hFieldVal.Add("MVDESDOC", pPar.desdoc)
                adhoc.hFieldVal.Add("MVTCONTR", "")
                adhoc.hFieldVal.Add("MVDOCWEB", pPar.ordweb)
                '
                'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                'If _codsed.Trim <> "" Then
                'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                'End If
                '
                Dim _ret As Boolean = adhoc.writeDocument("DOC_MAST", adhoc.tplDoc_mast)
                If _ret Then
                    Return _Serial
                Else
                    Return ""
                End If
            Else
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkDocMast")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' return cprownum
    ''' </summary>
    ''' <param name="ppar"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mkDocDett(ByVal ppar As sPardocdett) As Integer
        Try
            adhoc.hFieldVal.Clear()
            Dim harticolo As New Hashtable
            Dim _cprownum As Integer
            If CTran(ppar.cpronum, 0) = 0 Then
                _cprownum = 1
            Else
                _cprownum = ppar.cpronum

            End If
            ' 
            harticolo = adhoc.readAdhocTable("ART_ICOL", "ARCODART", ppar.codart, False)
            Dim _rowAmount As Decimal = Math.Round(ppar.qtamov * ppar.prezzo, 2)
            _rowAmount = _rowAmount * (1 + ppar.scont1 / 100) * (1 + ppar.scont2 / 100) * (1 + ppar.scont3 / 100) * (1 + ppar.scont4 / 100)
            '
            Dim _desart As String = adhoc.getVal(harticolo("ARDESART"))
            Dim _dessup As String = CTran(adhoc.getVal(harticolo("ARDESSUP")), "")
            adhoc.hFieldVal.Add("MVSERIAL", ppar.serial)
            adhoc.hFieldVal.Add("CPROWNUM", _cprownum)
            adhoc.hFieldVal.Add("CPROWORD", _cprownum * 10)
            adhoc.hFieldVal.Add("MVNUMRIF", -20)
            adhoc.hFieldVal.Add("MVCODICE", ppar.codart)
            '
            adhoc.hFieldVal.Add("MVTIPRIG", ppar.tiprig)
            '
            adhoc.hFieldVal.Add("MVDESART", _desart)
            adhoc.hFieldVal.Add("MVDESSUP", _dessup)
            adhoc.hFieldVal.Add("MVCODART", ppar.codart)
            adhoc.hFieldVal.Add("MVUNIMIS", adhoc.getVal(harticolo("ARUNMIS1")))
            adhoc.hFieldVal.Add("MVCATCON", adhoc.getVal(harticolo("ARCATCON")))
            adhoc.hFieldVal.Add("MVCAUMAG", ppar.caumag)
            adhoc.hFieldVal.Add("MVCODLIS", ppar.numlis)
            adhoc.hFieldVal.Add("MVQTAMOV", ppar.qtamov)
            adhoc.hFieldVal.Add("MVQTAUM1", ppar.qtamov)
            adhoc.hFieldVal.Add("MVCODIVA", adhoc.getVal(harticolo("ARCODIVA")))
            '
            adhoc.hFieldVal.Add("MVPREZZO", ppar.prezzo)
            adhoc.hFieldVal.Add("MVSCONT1", ppar.scont1)
            adhoc.hFieldVal.Add("MVSCONT2", ppar.scont2)
            adhoc.hFieldVal.Add("MVSCONT3", ppar.scont3)
            adhoc.hFieldVal.Add("MVSCONT4", ppar.scont4)
            '
            adhoc.hFieldVal.Add("MVVALRIG", _rowAmount)
            adhoc.hFieldVal.Add("MVVALMAG", _rowAmount)
            adhoc.hFieldVal.Add("MVIMPNAZ", _rowAmount)
            adhoc.hFieldVal.Add("MVIMPSCO", 0)
            adhoc.hFieldVal.Add("MVKEYSAL", ppar.codart)
            adhoc.hFieldVal.Add("MVQTASAL", ppar.qtamov)
            adhoc.hFieldVal.Add("MVDATEVA", ppar.dateva)
            adhoc.hFieldVal.Add("MVSEGNO", ppar.segno)
            adhoc.hFieldVal.Add("MVSERRIF", "")
            adhoc.hFieldVal.Add("MVROWRIF", 0)
            adhoc.hFieldVal.Add("MVCODMAG", Globale.Store)
            adhoc.hFieldVal.Add("MVFLORDI", ppar.flordi)
            adhoc.hFieldVal.Add("MVFLIMPE", ppar.flimpe)
            adhoc.hFieldVal.Add("MVFLRISE", ppar.flrise)
            adhoc.hFieldVal.Add("MVFLEVAS", "")
            adhoc.hFieldVal.Add("MVCODCLA", "")
            '
            Dim _ret As Boolean = adhoc.writeDocument("DOC_DETT", adhoc.tplDoc_dett_d)
            _cprownum = _cprownum + 1
            '
            Return _cprownum
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkDocDett")
            Return 0
        End Try

    End Function
    Private Function mkDocDett_rif(ByVal ppar As sPardocdett) As Integer
        Try
            adhoc.hFieldVal.Clear()
            Dim harticolo As New Hashtable
            Dim _cprownum As Integer
            If CTran(ppar.cpronum, 0) = 0 Then
                _cprownum = 0
            Else
                _cprownum = ppar.cpronum

            End If
            ' 
            _cprownum = _cprownum + 1
            '
            Dim _rowAmount As Decimal = 0
            '
            harticolo = adhoc.readAdhocTable("ART_ICOL", "ARCODART", ppar.codart, False)
            '
            Dim _desart As String = "SPESE DI TRASPORTO"
            Dim _dessup As String = ""
            adhoc.hFieldVal.Add("MVSERIAL", ppar.serial)
            adhoc.hFieldVal.Add("CPROWNUM", _cprownum)
            adhoc.hFieldVal.Add("CPROWORD", _cprownum * 10)
            adhoc.hFieldVal.Add("MVNUMRIF", -20)
            adhoc.hFieldVal.Add("MVCODICE", ".")
            adhoc.hFieldVal.Add("MVTIPRIG", "F")
            adhoc.hFieldVal.Add("MVDESART", _desart)
            adhoc.hFieldVal.Add("MVDESSUP", _dessup)
            adhoc.hFieldVal.Add("MVCODART", ppar.codart)
            adhoc.hFieldVal.Add("MVUNIMIS", adhoc.getVal(harticolo("ARUNMIS1")))
            adhoc.hFieldVal.Add("MVCATCON", adhoc.getVal(harticolo("ARCATCON")))
            adhoc.hFieldVal.Add("MVCAUMAG", ppar.caumag)
            adhoc.hFieldVal.Add("MVCODLIS", "")
            adhoc.hFieldVal.Add("MVQTAMOV", 0)
            adhoc.hFieldVal.Add("MVQTAUM1", 0)
            adhoc.hFieldVal.Add("MVCODIVA", "null")
            adhoc.hFieldVal.Add("MVCODMAG", "null")
            '
            adhoc.hFieldVal.Add("MVPREZZO", 0)
            adhoc.hFieldVal.Add("MVSCONT1", 0)
            adhoc.hFieldVal.Add("MVSCONT2", 0)
            adhoc.hFieldVal.Add("MVSCONT3", 0)
            adhoc.hFieldVal.Add("MVSCONT4", 0)
            '
            adhoc.hFieldVal.Add("MVVALRIG", _rowAmount)
            adhoc.hFieldVal.Add("MVVALMAG", _rowAmount)
            adhoc.hFieldVal.Add("MVIMPNAZ", _rowAmount)
            adhoc.hFieldVal.Add("MVIMPSCO", 0)
            adhoc.hFieldVal.Add("MVKEYSAL", "")
            adhoc.hFieldVal.Add("MVQTASAL", 0)
            adhoc.hFieldVal.Add("MVDATEVA", ppar.dateva)
            adhoc.hFieldVal.Add("MVSEGNO", ppar.segno)
            adhoc.hFieldVal.Add("MVFLERIF", "") 'Flag evasione riga documento originale
            adhoc.hFieldVal.Add("MVFLELAN", "")
            'adhoc.hFieldVal.Add("MVCODMAG", Globale.Store)
            adhoc.hFieldVal.Add("MVFLCASC", "")
            adhoc.hFieldVal.Add("MVFLORDI", "")
            adhoc.hFieldVal.Add("MVFLIMPE", "")
            adhoc.hFieldVal.Add("MVFLRISE", "")
            adhoc.hFieldVal.Add("MVF2CASC", "")
            adhoc.hFieldVal.Add("MVF2ORDI", "")
            adhoc.hFieldVal.Add("MVF2IMPE", "")
            adhoc.hFieldVal.Add("MVF2RISE", "")
            '
            adhoc.hFieldVal.Add("MVFLEVAS", "")
            adhoc.hFieldVal.Add("MVCODCLA", "")
            '
            adhoc.hFieldVal.Add("MVCONTRA", "")
            adhoc.hFieldVal.Add("MVFLELGM", "")
            adhoc.hFieldVal.Add("MVSERRIF", "")
            adhoc.hFieldVal.Add("MVROWRIF", 0)
            adhoc.hFieldVal.Add("MVFLARIF", "+") 'Flag aggiorna evaso
            adhoc.hFieldVal.Add("MVFLULPV", "")
            adhoc.hFieldVal.Add("MVFLLOTT", "")
            adhoc.hFieldVal.Add("MVDATOAI", Globale.g_SysData)
            adhoc.hFieldVal.Add("MVLOTMAG", "")
            '
            adhoc.hFieldVal.Add("ARVOCCEN", "null")
            adhoc.hFieldVal.Add("ARCODCEN", "null")
            adhoc.hFieldVal.Add("ARVOCRIC", "null")
            '
            Dim _ret As Boolean = adhoc.writeDocument("DOC_DETT", adhoc.tplDoc_dett_r)
            '
            Return _cprownum
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkDocDett_rif")
            Return 0
        End Try

    End Function
    Private Function updateSaldiTab(ByVal ppar As sPardocdett) As Boolean

        Try
            Dim hBalance As New Hashtable
            Dim _strsql As String = ""
            hBalance = adhoc.readAdhocTable("SALDIART", "SLCODICE,SLCODMAG", ppar.codart & "," & Globale.Store, False)

            If hBalance.Count > 0 Then
                _strsql = "update " & adhoc.getTablename("SALDIART") & " set slqtiper = slqtiper + " & op.ValAdapter(ppar.qtamov, TipoCampo.TCur) & _
                                        " where slcodice = " & op.ValAdapter(ppar.codart, TipoCampo.TChar) & _
                                        " and slcodmag = " & op.ValAdapter(Globale.Store, TipoCampo.TChar)
            Else
                _strsql = "insert into " & adhoc.getTablename("SALDIART") & " (SLCODICE, SLCODMAG, SLQTOPER, SLCODVAV, CPCCCHK) values (" & _
                          op.ValAdapter(ppar.codart, TipoCampo.TChar) & ", " & _
                          op.ValAdapter(Globale.Store, TipoCampo.TChar) & ", " & _
                          op.ValAdapter(ppar.qtamov, TipoCampo.TCur) & ", " & _
                          op.ValAdapter("EUR", TipoCampo.TChar) & ", " & _
                          op.ValAdapter("ABCDEGHILM", TipoCampo.TChar) & ")"
            End If
            op.esegui_query(_strsql)
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "updateSaldiTab")
            Return False
        End Try

    End Function
    Private Function calcAmountDoc(ByRef ppar As sPardocmast, ByVal phItem As Hashtable, ByVal pOrder As String) As Boolean

        Try
            Dim harticolo As New Hashtable
            Dim hIve As New Hashtable
            Dim hListVat As New Hashtable
            Dim _ord As New sOrdini
            Dim _vat As New sVat
            Dim _hVat As New Hashtable
            Dim _codIva As String
            Dim _periva As Decimal
            Dim _rowAmount As Decimal
            '
            '
            Dim hCaucon As New Hashtable
            'hCaucon = adhoc.readAdhocTable("CAU_CONT", "CCCODICE", ppar.caucon, False)
            Dim _tipreg As String = CType(hCaucon("CCTIPREG"), s_tableValue).mValue
            '
            For Each _item As String In phItem.Keys
                _ord = phItem(_item)
                If _ord.ODNUMORD <> pOrder Then
                    Continue For
                End If
                '
                'Temporaneo
                'Dim hConti As Hashtable = adhoc.readAdhocTable("CONTI", "ANTIPCON,ANCODICE", "C" & "," & ppar.codcli.ToString.Trim, False)
                'Dim _codage As String = CTran(adhoc.getVal(hConti("ANCODAG1")), "")
                'If _codage.Trim = "13" Or _codage.Trim = "4" Then
                'If (_ord.ODSCONT1 * -1) < 5 Then
                '_ord.ODSCONT1 = -5
                'End If
                ''If _ord.ODSCONT1 = 0 Then
                ''_ord.ODSCONT1 = -5
                ''ElseIf _ord.ODSCONT2 = 0 Then
                ''_ord.ODSCONT2 = -5
                ''ElseIf _ord.ODSCONT3 = 0 Then
                ''_ord.ODSCONT3 = -5
                ''ElseIf _ord.ODSCONT4 = 0 Then
                ''_ord.ODSCONT4 = -5
                ''End If
                'End If
                '
                harticolo = adhoc.readAdhocTable("ART_ICOL", "ARCODART", _ord.ODCODART, False)
                hIve = adhoc.readAdhocTable("VOCIIVA", "IVCODIVA", CType(harticolo("ARCODIVA"), s_tableValue).mValue, False)
                _codIva = CType(harticolo("ARCODIVA"), s_tableValue).mValue
                _periva = CType(hIve("IVPERIVA"), s_tableValue).mValue
                _rowAmount = Math.Round(_ord.ODQTAMOV * _ord.ODPREZZO, 2)
                _rowAmount = _rowAmount * (1 + _ord.ODSCONT1 / 100) * (1 + _ord.ODSCONT2 / 100) * (1 + _ord.ODSCONT3 / 100) * (1 + _ord.ODSCONT4 / 100)
                '
                If hListVat.ContainsKey(_codIva) Then
                    _vat = hListVat(_codIva)
                    '
                    Select Case _tipreg
                        Case "C" 'Scorporo
                            Dim _vatValue = _rowAmount - Math.Round((_rowAmount * 100) / (_periva + 100), 2)
                            _vat.VatValue += _vatValue
                            _vat.VatAmount += _rowAmount - _vatValue
                        Case Else
                            _vat.VatValue += Math.Round((_rowAmount * _periva) / 100, 2)
                            _vat.VatAmount += _rowAmount
                    End Select
                    '
                    hListVat(_codIva) = _vat
                Else
                    _vat.VatCode = _codIva
                    '
                    Select Case _tipreg
                        Case "C" 'Scorporo
                            Dim _vatValue = _rowAmount - Math.Round((_rowAmount * 100) / (_periva + 100), 2)
                            _vat.VatValue = _vatValue
                            _vat.VatAmount = _rowAmount - _vatValue
                        Case Else
                            _vat.VatValue = Math.Round((_rowAmount * _periva) / 100, 2)
                            _vat.VatAmount = _rowAmount
                    End Select
                    '
                    hListVat.Add(_codIva, _vat)
                End If
            Next
            'Getting charges vat code
            Dim strsql = "SELECT COCOIINC, COCOITRA FROM CONTROPA WHERE COCODAZI = " & op.ValAdapter(Globale.CodAzi, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                'Getting charges vat value
                If CTran(ds.Tables(0).Rows(0).Item("COCOIINC"), "") <> "" Then
                    hIve = adhoc.readAdhocTable("VOCIIVA", "IVCODIVA", ds.Tables(0).Rows(0).Item("COCOIINC"), False)
                    'Adding charges
                    Dim _vatCode As String = ds.Tables(0).Rows(0).Item("COCOIINC")
                    Dim _percent As Integer = CType(hIve("IVPERIVA"), s_tableValue).mValue
                    _rowAmount = ppar.speinc
                    If hListVat.ContainsKey(_vatCode) Then
                        _vat = hListVat(_vatCode)
                        _vat.VatValue += Math.Round((_rowAmount * _percent) / 100, 2)
                        _vat.VatAmount += _rowAmount
                        hListVat(_vatCode) = _vat
                    Else
                        _vat.VatCode = _vatCode
                        _vat.VatValue = Math.Round((_rowAmount * _percent) / 100, 2)
                        _vat.VatAmount = _rowAmount
                        hListVat.Add(_vatCode, _vat)
                    End If
                    '

                End If
            End If
            'Getting transportation charges vat value
            If CTran(ds.Tables(0).Rows(0).Item("COCOITRA"), "") <> "" And CTran(ppar.spetra, "0") <> "0" Then
                hIve = adhoc.readAdhocTable("VOCIIVA", "IVCODIVA", ds.Tables(0).Rows(0).Item("COCOITRA"), False)
                'Adding charges
                Dim _vatCode As String = ds.Tables(0).Rows(0).Item("COCOITRA")
                Dim _percent As Integer = CType(hIve("IVPERIVA"), s_tableValue).mValue
                '
                _rowAmount = ppar.spetra
                '
                If hListVat.ContainsKey(_vatCode) Then
                    _vat = hListVat(_vatCode)
                    _vat.VatAmount += _rowAmount
                    _vat.VatValue = Math.Round((_vat.VatAmount * _percent) / 100, 2)
                    hListVat(_vatCode) = _vat
                Else
                    _vat.VatCode = _vatCode
                    _vat.VatValue = Math.Round((_rowAmount * _percent) / 100, 2)
                    _vat.VatAmount = _rowAmount
                    hListVat.Add(_vatCode, _vat)
                End If
                '
            End If
            '
            Dim _ii As Integer = 0
            For Each _element As String In hListVat.Keys
                _vat = hListVat(_element)
                Select Case _ii
                    Case 0
                        ppar.aciva1 = _vat.VatCode
                        ppar.aimpn1 = _vat.VatAmount
                        ppar.aimps1 = _vat.VatValue
                    Case 1
                        ppar.aciva2 = _vat.VatCode
                        ppar.aimpn2 = _vat.VatAmount
                        ppar.aimps2 = _vat.VatValue
                    Case 2
                        ppar.aciva3 = _vat.VatCode
                        ppar.aimpn3 = _vat.VatAmount
                        ppar.aimps3 = _vat.VatValue
                    Case 3
                        ppar.aciva4 = _vat.VatCode
                        ppar.aimpn4 = _vat.VatAmount
                        ppar.aimps4 = _vat.VatValue
                    Case 4
                        ppar.aciva5 = _vat.VatCode
                        ppar.aimpn5 = _vat.VatAmount
                        ppar.aimps5 = _vat.VatValue
                    Case 5
                        ppar.aciva6 = _vat.VatCode
                        ppar.aimpn6 = _vat.VatAmount
                        ppar.aimps6 = _vat.VatValue
                End Select
                _ii += 1
                If _ii > 5 Then
                    Exit For
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "CalcAmountDoc")
        End Try

    End Function
    Private Function mkDocrate(ByRef ppar As sPardocmast) As Boolean

        Try
            Dim _lastDayMonth As Integer = 0
            Dim _numrat As Integer = 0
            Dim _impRat As Decimal = 0
            Dim _ExpDate As Date = Now.Date
            Dim _expMonth As Integer = 0
            Dim mm As Integer
            Dim gg As Integer

            If ppar.codpag = "" Then
                Return False
            End If
            Dim pAmount As Decimal = ppar.aimpn1 + ppar.aimpn2 + ppar.aimpn3 + ppar.aimpn4 + ppar.aimpn5 + ppar.aimpn6
            pAmount = pAmount + ppar.aimps1 + ppar.aimps2 + ppar.aimps3 + ppar.aimps4 + ppar.aimps5 + ppar.aimps6

            '

            Dim _Sqlstring As String = "select p2numrat, p2modpag, p2giosca, p2scaden, p2giofis from " & adhoc.getTablename("PAG_2AME") & _
                                           " where p2codice = " & op.ValAdapter(ppar.codpag, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(_Sqlstring)
            If ds.Tables(0).Rows.Count > 0 Then
                _numrat = ds.Tables(0).Rows.Count
            Else
                _numrat = 1
            End If
            _impRat = pAmount / _numrat
            For Each _row As DataRow In ds.Tables(0).Rows
                '
                adhoc.hFieldVal.Clear()
                '
                mm = _row.Item("p2giosca") / 30
                gg = _row.Item("p2giosca") Mod 30
                _ExpDate = _ExpDate.AddMonths(mm)
                If gg > 0 Then
                    _ExpDate = _ExpDate.AddDays(gg)
                End If
                If _row.Item("p2scaden") = "FM" Or _row.Item("p2scaden") = "FF" Then
                    _lastDayMonth = Date.DaysInMonth(_ExpDate.Year, _ExpDate.Month)
                    _ExpDate = _ExpDate.AddDays(_lastDayMonth - _ExpDate.Day)
                    If CTran(_row.Item("p2giofis"), 0) > 0 Then
                        '_expMonth += 1
                        _ExpDate = _ExpDate.AddMonths(1)
                    End If
                End If
                adhoc.hFieldVal.Add("RSSERIAL", op.ValAdapter(ppar.serial, TipoCampo.TChar))
                adhoc.hFieldVal.Add("RSNUMRAT", op.ValAdapter(_row.Item("p2numrat"), TipoCampo.TChar))
                adhoc.hFieldVal.Add("RSDATRAT", op.ValAdapter(_ExpDate, TipoCampo.TData))
                adhoc.hFieldVal.Add("RSIMPRAT", op.ValAdapter(_impRat, TipoCampo.TCur))
                adhoc.hFieldVal.Add("RSMODPAG", op.ValAdapter(_row.Item("p2modpag"), TipoCampo.TChar))
                adhoc.hFieldVal.Add("RSFLSOSP", op.ValAdapter("", TipoCampo.TChar))
                adhoc.hFieldVal.Add("RSDESRIG", op.ValAdapter("", TipoCampo.TChar))
                adhoc.hFieldVal.Add("RSFLPROV", op.ValAdapter("", TipoCampo.TChar))
                adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("XHFJEKLOPR", TipoCampo.TChar))
                '
                op.insert_query(adhoc.getTablename("DOC_RATE"), adhoc.hFieldVal)
            Next
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "mkDocRate")
            Return False
        End Try
    End Function
    Private Function getOrInsertCust(ByVal pEmail As String) As String

        Try
            Dim _codcli As String = ""
            adhoc.hFieldVal.Clear()
            Dim querystr As String = "select ut_email, utpassw, utcodlis, utcodcli, utdescri, utindiri, utindir2, " & _
                                     "ut___cap, utlocali, utprovin, utnazion, uttelefo, uttelfax, utnumcel, utpariva, " & _
                                     "utcodfis from " & adhoc.getTablename("ASYSUTEWEB") & " where ut_email = " & op.ValAdapter(pEmail.Trim, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(querystr)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim _row As DataRow = ds.Tables(0).Rows(0)
                If CTran(_row.Item("utcodcli").ToString.Trim, "") = "" Then
                    '
                    _codcli = adhoc.getProgCli("").ToString.PadLeft(Globale.gLenCodCli, "0")
                    '
                    adhoc.hFieldVal.Add("ANTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODICE", op.ValAdapter(_codcli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCRI", op.ValAdapter(_row.Item("utdescri").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCR2", op.ValAdapter("", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIRI", op.ValAdapter(_row.Item("utindiri").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIR2", op.ValAdapter(_row.Item("utindir2").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN___CAP", op.ValAdapter(_row.Item("ut___cap").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANLOCALI", op.ValAdapter(_row.Item("utlocali").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPROVIN", op.ValAdapter(_row.Item("utprovin").ToString.Trim, TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("ANNAZION", op.ValAdapter(IIf(CTran(_row.Item("utnazion").ToString.Trim, "") = "", Globale.gCodNaz, _row.Item("utnazion").ToString.Trim), TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("ANTELEFO", op.ValAdapter(_row.Item("uttelefo").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELFAX", op.ValAdapter(_row.Item("uttelfax").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMCEL", op.ValAdapter(_row.Item("utnumcel").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPARIVA", op.ValAdapter(_row.Item("utpariva").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODFIS", op.ValAdapter(_row.Item("utcodfis").ToString.Trim, TipoCampo.TChar))
                    'adhoc.hFieldVal.Add("ANCODPAG", op.ValAdapter(Globale.gDefPayCode.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN_EMAIL", op.ValAdapter(_row.Item("ut_email").ToString.Trim, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCC", op.ValAdapter("1", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    adhoc.hFieldVal.Add("ANCATCON", op.ValAdapter(Globale.CatCon, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCATCOM", op.ValAdapter(Globale.g_CatCom, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCONSUP", op.ValAdapter(Globale.MastroCli, TipoCampo.TChar))
                    '
                    If CTran(_row.Item("utcodlis").ToString.Trim, "") = "" Then
                        adhoc.hFieldVal.Add("ANNUMLIS", op.ValAdapter(Globale.ListDef, TipoCampo.TChar))
                    Else
                        adhoc.hFieldVal.Add("ANNUMLIS", op.ValAdapter(_row.Item("utcodlis").ToString.Trim, TipoCampo.TChar))
                    End If
                    '
                    adhoc.hFieldVal.Add("ANCODLIN", op.ValAdapter(Globale.gCodNaz, TipoCampo.TChar))
                    'Flag
                    adhoc.hFieldVal.Add("ANPARTSN", op.ValAdapter("S", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIG", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAACC", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AFFLINTR", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIVASOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBLLS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLFIDO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLSGRE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLSOAL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBODO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLRITE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANBOLFAT", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANSCORPO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCLIPOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLIMBA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPREBOL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLCODI", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLGCPZ", op.ValAdapter("N", TipoCampo.TChar))
                    If CTran(_row.Item("utpariva").ToString.Trim, "") = "" Then
                        adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("S", TipoCampo.TChar))
                    Else
                        adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("N", TipoCampo.TChar))
                    End If
                    adhoc.hFieldVal.Add("ANFLAPCA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLINCA", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLGAVV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIDRIDY", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIM", op.ValAdapter("N", TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                    'If _codsed.Trim <> "" Then
                    'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                    'End If
                    '
                    op.insert_query(adhoc.getTablename("CONTI"), adhoc.hFieldVal)
                    lstUpd.Items.Add(_codcli & " - " & _row.Item("utdescri").ToString.Trim)
                    lstUpd.Refresh()
                    '
                    If CTran(_row.Item("utcodlis").ToString.Trim, "") = "" Then
                        '    
                        Dim _updquery As String = "update " & adhoc.getTablename("ASYSUTEWEB") & _
                                                      " set utcodcli = " & op.ValAdapter(_codcli.Trim, TipoCampo.TChar) & _
                                                      ", utcodlis = " & op.ValAdapter(Globale.ListDef, TipoCampo.TChar) & _
                                                      " where ut_email = " & op.ValAdapter(pEmail.Trim, TipoCampo.TChar)
                        op.esegui_query(_updquery)
                        '
                    End If
                Else
                    _codcli = _row.Item("utcodcli").ToString.Trim
                End If
            Else

            End If
            Return _codcli
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getOrInsertCust")
        End Try
    End Function
    Private Function mkCLienti() As String

        Try
            lstUpd.Items.Add("Inserimento/Aggiornamento clienti:")
            lstUpd.Refresh()
            '
            For Each element As String In gCli.Keys
                adhoc.hFieldVal.Clear()
                Dim _Codcli As String
                Dim ppar As New sClienti
                Dim clipriv As New sClipriv
                '
                ppar = gCli(element)
                If Globale.CodAzi = "COPTG" Then
                    '
                    'se cliente privato non registro niente
                    '
                    '17-03-2021 IL CLIENTE PRIVATO DEVE ESSERE REGISTRATO COME SUCCEDE CON L' AZIENDA TRANNE PER IL FATTO CHE CI VANNO I FLAG PER UTENTE PRIVATO.
                    If ppar.ANPARIVA.ToString.Trim = "" Then
                        With clipriv
                            .CognomeNome = CTran(ppar.ANDESCRI.ToString, "")
                            .Indirizzo = CTran(ppar.ANINDIRI, "")
                            .locali = CTran(ppar.ANLOCALI, "")
                            .provin = CTran(ppar.ANPROVIN, "")
                            .codfis = CTran(ppar.ANCODFIS, "")
                            .Cap = CTran(ppar.AN___CAP, "")
                            .codpag = CTran(ppar.ANCODPAG, "")
                        End With
                        hCliPriv.Add(ppar.AN_EMAIL.Trim, clipriv)
                        '18-03-2021 prendo ugualmente i dati anhe per il privato ma elimino il Continue per proseguire per inserire l' anagrafica anche di questi 
                        'Continue For
                    End If
                    '
                    ppar.ANCODICE = Me.getCodeByPIvaCf(ppar.ANPARIVA, ppar.ANCODFIS)
                    If CTran(ppar.ANCODICE, "") = "" Or CTran(ppar.ANCODICE, "").StartsWith("w") Or CTran(ppar.ANCODICE, "").StartsWith("t") Then
                        '
                        adhoc.hFieldVal.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                        If ppar.ANPASSWD.Trim <> "" Then
                            adhoc.hFieldVal.Add("UTPASSW", op.ValAdapter(ppar.ANPASSWD, TipoCampo.TChar))
                        End If
                        adhoc.hFieldVal.Add("UTDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UT___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                        adhoc.hFieldVal.Add("UTCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                        If ppar.ANPARIVA = "" Then
                            'L' utente privato è autorizzato subito
                            adhoc.hFieldVal.Add("UTAUTORI", op.ValAdapter("S", TipoCampo.TChar))
                        End If
                        '
                        Dim strsql As String = "select ut_email from " & adhoc.getTablename("ASYSUTEWEB") & " where ut_email = " & op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar)
                        Dim ds As DataSet = op.esegui_query(strsql)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim pKey As New Hashtable
                            pKey.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL.Trim, TipoCampo.TChar))
                            op.update_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal, pKey)
                            '
                            'below i have to add customer update istructions ("CONTI" table)
                        Else
                            op.insert_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal)
                        End If
                        '
                        lstUpd.Items.Add(ppar.AN_EMAIL)
                        lstUpd.Refresh()
                        '
                        '18-03-2021 ripristinato l' inserimento perchè COPT ha iniziato a vendere ai privati.
                        If ppar.ANPARIVA = "" Then
                            ppar.ANCODICE = Me.getOrInsertCust(ppar.AN_EMAIL)
                        End If
                    Else
                        Me.updateClienti(ppar.ANCODICE)
                    End If
                    '
                Else
                    '
                    'Getting customer number
                    '
                    If ppar.ANPARIVA.Trim <> "" Then
                        _Codcli = adhoc.getProgCli("").ToString.PadLeft(5, "0")
                    Else
                        _Codcli = "P" & adhoc.getProgCli("CLIP").ToString.PadLeft(5, "0")
                    End If
                    Dim strsql As String = ""
                    '
                    adhoc.hFieldVal.Add("ANTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODICE", op.ValAdapter(_Codcli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCR2", op.ValAdapter(ppar.ANDESCR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODPAG", op.ValAdapter(ppar.ANCODPAG, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCC", op.ValAdapter("1", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    adhoc.hFieldVal.Add("ANCATCON", op.ValAdapter(Globale.CatCon, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCONSUP", op.ValAdapter(Globale.MastroCli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMLIS", op.ValAdapter(Globale.ListDef, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODLIN", op.ValAdapter("IT", TipoCampo.TChar))
                    'Flag
                    adhoc.hFieldVal.Add("ANPARTSN", op.ValAdapter("S", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIG", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAACC", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AFFLINTR", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIVASOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBLLS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLFIDO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLSGRE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLSOAL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBODO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLRITE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANBOLFAT", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANSCORPO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCLIPOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLIMBA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPREBOL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLCODI", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLGCPZ", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAPCA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLINCA", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLGAVV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIDRIDY", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIM", op.ValAdapter("N", TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                    'If _codsed.Trim <> "" Then
                    'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                    'End If
                    '
                    op.insert_query(adhoc.getTablename("CONTI"), adhoc.hFieldVal)
                    lstUpd.Items.Add(_Codcli & " - " & ppar.ANDESCRI)
                    lstUpd.Refresh()
                    '
                End If
            Next
            Return ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkClienti")
        End Try
    End Function
    Private Function updateClienti(ByVal pCodcli As String)


        Try
            lstUpd.Items.Add("Inserimento/Aggiornamento clienti:")
            lstUpd.Refresh()
            '
            For Each element As String In gCli.Keys
                adhoc.hFieldVal.Clear()
                Dim _Codcli As String
                Dim ppar As New sClienti
                '
                ppar = gCli(element)
                If Globale.CodAzi = "COPTG" Then
                    '
                    adhoc.hFieldVal.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                    If ppar.ANPASSWD.Trim <> "" Then
                        adhoc.hFieldVal.Add("UTPASSW", op.ValAdapter(ppar.ANPASSWD, TipoCampo.TChar))
                    End If
                    adhoc.hFieldVal.Add("UTDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UT___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                    '
                    Dim strsql As String = "select ut_email from " & adhoc.getTablename("ASYSUTEWEB") & " where ut_email = " & op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar)
                    Dim ds As DataSet = op.esegui_query(strsql)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Dim pKeyUt As New Hashtable
                        pKeyUt.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL.Trim, TipoCampo.TChar))
                        op.update_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal, pKeyUt)
                        '
                        'below i have to add customer update istructions ("CONTI" table)
                    Else
                        op.insert_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal)
                    End If
                    '
                    Dim pKey As New Hashtable
                    pKey.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL.Trim, TipoCampo.TChar))
                    op.update_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal, pKey)
                    '
                    lstUpd.Items.Add(pCodcli & " - " & ppar.ANDESCRI)
                    lstUpd.Refresh()
                    Continue For
                    '
                    'Il pezzo sotto non lo deve eseguire 22/10/2015
                    '
                    '
                    '
                    adhoc.hFieldVal.Clear()
                    adhoc.hFieldVal.Add("ANTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODICE", op.ValAdapter(pCodcli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCR2", op.ValAdapter(ppar.ANDESCR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                    'adhoc.hFieldVal.Add("ANNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                    'adhoc.hFieldVal.Add("ANCODPAG", op.ValAdapter(ppar.ANCODPAG, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCC", op.ValAdapter("1", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    'adhoc.hFieldVal.Add("ANCATCON", op.ValAdapter(Globale.CatCon, TipoCampo.TChar))
                    'adhoc.hFieldVal.Add("ANCONSUP", op.ValAdapter(Globale.MastroCli, TipoCampo.TChar))
                    'adhoc.hFieldVal.Add("ANNUMLIS", op.ValAdapter(Globale.ListDef, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODLIN", op.ValAdapter("IT", TipoCampo.TChar))
                    'Flag
                    adhoc.hFieldVal.Add("ANPARTSN", op.ValAdapter("S", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIG", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAACC", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AFFLINTR", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIVASOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBLLS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLFIDO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLSGRE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLSOAL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBODO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLRITE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANBOLFAT", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANSCORPO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCLIPOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLIMBA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPREBOL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLCODI", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLGCPZ", op.ValAdapter("N", TipoCampo.TChar))
                    If CTran(ppar.ANPARIVA, "") = "" Then
                        adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("S", TipoCampo.TChar))
                    Else
                        adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("N", TipoCampo.TChar))
                    End If

                    adhoc.hFieldVal.Add("ANFLAPCA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLINCA", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLGAVV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIDRIDY", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIM", op.ValAdapter("N", TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                    'If _codsed.Trim <> "" Then
                    'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                    'End If
                    '
                    pKey = New Hashtable
                    pKey.Add("ANCODICE", op.ValAdapter(pCodcli.Trim, TipoCampo.TChar))
                    pKey.Add("ANTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                    op.update_query(adhoc.getTablename("CONTI"), adhoc.hFieldVal, pKey)
                    lstUpd.Items.Add(pCodcli & " - " & ppar.ANDESCRI)
                    lstUpd.Refresh()
                    '
                End If
            Next
            Return ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkClienti")
        End Try

    End Function
    Private Function mkCLienti_1() As String

        Try
            lstUpd.Items.Add("Inserimento/Aggiornamento clienti:")
            lstUpd.Refresh()
            '
            For Each element As String In gCli.Keys
                adhoc.hFieldVal.Clear()
                Dim _Codcli As String
                Dim ppar As New sClienti
                '
                ppar = gCli(element)
                If Globale.CodAzi = "COPTG" Then
                    '
                    adhoc.hFieldVal.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                    If ppar.ANPASSWD.Trim <> "" Then
                        adhoc.hFieldVal.Add("UTPASSW", op.ValAdapter(ppar.ANPASSWD, TipoCampo.TChar))
                    End If
                    adhoc.hFieldVal.Add("UTDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UT___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                    '
                    'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                    'If _codsed.Trim <> "" Then
                    'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                    'End If
                    '
                    Dim strsql As String = "select ut_email from " & adhoc.getTablename("ASYSUTEWEB") & " where ut_email = " & op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar)
                    Dim ds As DataSet = op.esegui_query(strsql)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Dim pKey As New Hashtable
                        pKey.Add("UT_EMAIL", op.ValAdapter(ppar.AN_EMAIL.Trim, TipoCampo.TChar))
                        op.update_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal, pKey)
                        '
                        'below i have to add customer update istructions ("CONTI" table)
                    Else
                        op.insert_query(adhoc.getTablename("ASYSUTEWEB"), adhoc.hFieldVal)
                    End If
                    '
                    lstUpd.Items.Add(ppar.AN_EMAIL)
                    lstUpd.Refresh()
                    '
                Else
                    '
                    'Getting customer number
                    '
                    If ppar.ANPARIVA.Trim <> "" Then
                        _Codcli = adhoc.getProgCli("").ToString.PadLeft(5, "0")
                    Else
                        _Codcli = "P" & adhoc.getProgCli("CLIP").ToString.PadLeft(5, "0")
                    End If
                    Dim strsql As String = ""
                    '
                    adhoc.hFieldVal.Add("ANTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODICE", op.ValAdapter(_Codcli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCRI", op.ValAdapter(ppar.ANDESCRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANDESCR2", op.ValAdapter(ppar.ANDESCR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIRI", op.ValAdapter(ppar.ANINDIRI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANINDIR2", op.ValAdapter(ppar.ANINDIR1, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN___CAP", op.ValAdapter(ppar.AN___CAP, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANLOCALI", op.ValAdapter(ppar.ANLOCALI, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPROVIN", op.ValAdapter(ppar.ANPROVIN, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNAZION", op.ValAdapter(ppar.ANNAZION, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELEFO", op.ValAdapter(ppar.ANTELEFO, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANTELFAX", op.ValAdapter(ppar.ANTELFAX, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMCEL", op.ValAdapter(ppar.ANNUMCEL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPARIVA", op.ValAdapter(ppar.ANPARIVA, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODFIS", op.ValAdapter(ppar.ANCODFIS, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODPAG", op.ValAdapter(ppar.ANCODPAG, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AN_EMAIL", op.ValAdapter(ppar.AN_EMAIL, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTCC", op.ValAdapter("1", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    adhoc.hFieldVal.Add("ANCATCON", op.ValAdapter(Globale.CatCon, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCONSUP", op.ValAdapter(Globale.MastroCli, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANNUMLIS", op.ValAdapter(Globale.ListDef, TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCODLIN", op.ValAdapter("IT", TipoCampo.TChar))
                    'Flag
                    adhoc.hFieldVal.Add("ANPARTSN", op.ValAdapter("S", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIG", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAACC", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("AFFLINTR", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIVASOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBLLS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLFIDO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLSGRE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLSOAL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLBODO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLRITE", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANBOLFAT", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANSCORPO", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANCLIPOS", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLIMBA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANPREBOL", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLCODI", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLGCPZ", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLPRIV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLAPCA", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLINCA", op.ValAdapter("N", TipoCampo.TChar))

                    adhoc.hFieldVal.Add("ANFLGAVV", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANIDRIDY", op.ValAdapter("N", TipoCampo.TChar))
                    adhoc.hFieldVal.Add("ANFLESIM", op.ValAdapter("N", TipoCampo.TChar))
                    '
                    adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                    'If _codsed.Trim <> "" Then
                    'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                    'End If
                    '
                    op.insert_query(adhoc.getTablename("CONTI"), adhoc.hFieldVal)
                    lstUpd.Items.Add(_Codcli & " - " & ppar.ANDESCRI)
                    lstUpd.Refresh()
                    '
                End If
            Next
            Return ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkClienti")
        End Try
    End Function
    Private Function mkDesdive() As String

        Try
            lstUpd.Items.Add("Inserimento destinazioni:")
            lstUpd.Refresh()
            '
            Dim _Codcli As String = ""
            Dim _Coddes As String = ""
            Dim _bInsert As Boolean = False
            Dim _clipriv As New sClipriv
            Dim _despriv As New sDesPriv
            '
            For Each element As String In gDes.Keys ' Here i'm doing a loop but it's not ncessary because there is only one other destination for each order.
                adhoc.hFieldVal.Clear()
                Dim ppar As New sDesdive
                '
                ppar = gDes(element)
                '
                '
                Select Case Globale.CodAzi
                    Case "COPTG"
                        '
                        'Se privato non registro niente
                        '
                        If hCliPriv.ContainsKey(ppar.DD_EMAIL.ToString.Trim) Then
                            With _despriv
                                .Nomdes = ppar.DDNOMDES
                                .Indirizzo = ppar.DDINDIRI
                                .Cap = ppar.DD___CAP
                                .locali = ppar.DDLOCALI
                                .provin = ppar.DDPROVIN
                            End With
                            hDesPriv.Add(ppar.DD_EMAIL.ToString.Trim, _despriv)
                            Continue For
                        End If
                        '
                        'If codazi is coptg, the table des_dive comes from website only if other destination is new
                        '
                        _Codcli = Me.getCodeByEmail(ppar.DD_EMAIL)
                        If _Codcli.Trim = "" Then
                            _Codcli = Me.getOrInsertCust(ppar.DD_EMAIL)
                        End If
                        'Looking for other destination code
                        Dim hConti As Hashtable = adhoc.readAdhocTable("DES_DIVE", "DDTIPCON,DDCODICE,DDCODDES", "C" & "," & _Codcli & "," & ppar.DDCODDES, False)
                        If Not IsNothing(hConti("DDCODDES")) Then
                            _Coddes = adhoc.getVal(hConti("DDCODDES"))
                            _bInsert = False
                        Else
                            _Coddes = ppar.DDCODDES
                            _bInsert = True
                        End If
                        '
                    Case "ALTRQ"
                        '
                        If ppar.DDCODDES.Trim <> "" Then
                            _Coddes = ppar.DDCODDES
                            _bInsert = False
                        Else
                            '
                            _Codcli = Me.getCodeByPIvaCf(ppar.DDPARIVA, ppar.DDCODFIS)
                            'Looking for other destination code
                            Dim hConti As Hashtable = adhoc.readAdhocTable("DES_DIVE", "DDTIPCON,DDCODICE,DDNOMDES", "C" & "," & _Codcli & "," & ppar.DDNOMDES, False)
                            If Not IsNothing(hConti("DDCODDES")) Then
                                _Coddes = adhoc.getVal(hConti("DDCODDES"))
                                _bInsert = False
                            Else
                                _Coddes = makeRandomCode(_Codcli)
                                _bInsert = True
                            End If
                            '
                        End If
                        '
                End Select
                If Globale.CodAzi = "COPTG" Then

                Else
                End If
                adhoc.hFieldVal.Add("DDTIPCON", op.ValAdapter("C", TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDCODICE", op.ValAdapter(_Codcli, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDCODDES", op.ValAdapter(_Coddes, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DD___CAP", op.ValAdapter(ppar.DD___CAP, TipoCampo.TChar))
                If ppar.DDDTOBSO <> "NULL" And ppar.DDDTOBSO <> "1970-01-01" Then
                    adhoc.hFieldVal.Add("DDDTOBSO", op.ValAdapter(ppar.DDDTOBSO, TipoCampo.TChar))
                End If
                adhoc.hFieldVal.Add("DDINDIRI", op.ValAdapter(ppar.DDINDIRI, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDTIPRIF", op.ValAdapter("CO", TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDLOCALI", op.ValAdapter(ppar.DDLOCALI, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDNOMDES", op.ValAdapter(ppar.DDNOMDES, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDPROVIN", op.ValAdapter(ppar.DDPROVIN, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DDTELEFO", op.ValAdapter(ppar.DDTELEFO, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DD_EMAIL", op.ValAdapter(ppar.DD_EMAIL, TipoCampo.TChar))
                adhoc.hFieldVal.Add("DD__NOTE", op.ValAdapter(ppar.DD__NOTE, TipoCampo.TChar))
                adhoc.hFieldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                '
                'Dim _codsed As String = Me.cmbDesdive.Text.Split(":")(0)
                'If _codsed.Trim <> "" Then
                'adhoc.hFieldVal.Add("MVCODDES", _codsed)
                'End If
                '
                If _bInsert Then
                    op.insert_query(adhoc.getTablename("DES_DIVE"), adhoc.hFieldVal)
                    lstUpd.Items.Add(_Codcli & " - " & _Coddes)
                    lstUpd.Refresh()
                Else
                    Dim _HKey As New Hashtable
                    _HKey.Add("DDTIPCON", "'C'")
                    _HKey.Add("DDCODICE", op.ValAdapter(_Codcli, TipoCampo.TChar))
                    _HKey.Add("DDCODDES", op.ValAdapter(_Coddes, TipoCampo.TChar))
                    op.update_query(adhoc.getTablename("DES_DIVE"), adhoc.hFieldVal, _HKey)
                    lstUpd.Items.Add(_Codcli & " - " & _Coddes)
                    lstUpd.Refresh()
                End If
                '
            Next
            '
            Return _Coddes
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "mkDesdive")
        End Try
    End Function
    Private Function getCodDesDive(ByVal pCodcli As String, ByVal pNomdes As String) As String

        Try
            Dim hConti As Hashtable = adhoc.readAdhocTable("DES_DIVE", "DDTIPCON,DDCODICE,DDNOMDES", "C" & "," & pCodcli & "," & pNomdes, False)
            If Not IsNothing(hConti("DDCODDES")) Then
                Return adhoc.getVal(hConti("DDCODDES"))
            Else
                Return makeRandomCode(pCodcli)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getCodDes")
        End Try
    End Function
    ''' <summary>
    ''' Choices code for other company destinations
    ''' </summary>
    ''' <param name="pcodcli"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function makeRandomCode(ByVal pcodcli) As String

        Try
            Dim strsql As String = "select ddcoddes from " & adhoc.getTablename("DES_DIVE") & " where ddtipcon = 'C' and ddcodice = " & op.ValAdapter(pcodcli, TipoCampo.TChar) & _
                                   " and ddcoddes = "
            Dim _random As String = ""
            Dim _rnd As New Random
            Do
                _random = _rnd.Next.ToString
                _random = _random.Substring(7)
                strsql = strsql & op.ValAdapter(_random.ToString, TipoCampo.TChar)
                Dim ds As DataSet = op.esegui_query(strsql)
                If ds.Tables(0).Rows.Count = 0 Then
                    Return _random
                End If
            Loop
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "makeRandomCode")
        End Try

    End Function

    Private Function checkData(ByVal pfilename As String) As Boolean
        Dim sw As System.IO.FileStream
        Dim ds As DataSet
        Dim wStrsql As String = ""
        Dim _cli As New sClienti
        Dim _ord As New sOrdini
        Dim _des As New sDesdive
        Dim _OrdList As New ArrayList

        Try
            Dim _flError As Boolean = False
            'elimina il file
            System.IO.File.Delete(Environment.CurrentDirectory & "\" & "errori.txt")
            '
            sw = New System.IO.FileStream(Environment.CurrentDirectory & "\" & "errori.txt", IO.FileMode.Create, IO.FileAccess.Write)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            Dim _strErrorHead As String = "File : < " & pfilename & " >"
            filewriter.WriteLine(_strErrorHead)
            '
            For Each element As String In gCli.Keys
                _cli = gCli(element)
                '
                With _cli
                    .ANPARIVA = IIf(String.IsNullOrEmpty(.ANPARIVA), "", .ANPARIVA)
                    .ANCODFIS = IIf(String.IsNullOrEmpty(.ANCODFIS), "", .ANCODFIS)
                End With
                If _cli.ANCODFIS.Trim = "" And _cli.ANPARIVA.Trim = "" Then
                    Dim _strError As String = "Cliente < " & _cli.ANDESCRI & " > senza p.iva e C.F.!"
                    filewriter.WriteLine(_strError)
                    _flError = True
                End If
                '
                If Not String.IsNullOrEmpty(_cli.ANCODPAG) Then
                    If _cli.ANCODPAG.Trim <> "" Then
                        wStrsql = "select distinct(P2MODPAG) AS MODPAG from " & adhoc.getTablename("PAG_2AME") & " where P2CODICE = " & op.ValAdapter(_cli.ANCODPAG, TipoCampo.TChar)
                        ds = op.esegui_query(wStrsql)
                        Dim codpag As String = IIf(CTran(_cli.ANCODPAG, "") = "", Globale.gDefPayCode, _cli.ANCODPAG)
                        '
                        If ds.Tables(0).Rows.Count = 0 And codpag.Trim = "" Then
                            Dim _strError As String = "Codice pagamento < " & codpag & " > sconosciuto, Cliente < " & _cli.ANDESCRI & " >!"
                            filewriter.WriteLine(_strError)
                            _flError = True
                        End If
                    End If
                    '
                End If
            Next
            '
            For Each element As String In gOrd.Keys
                _ord = gOrd(element)
                With _ord
                    .ODPARIVA = IIf(String.IsNullOrEmpty(.ODPARIVA), "", .ODPARIVA)
                    .ODCODFIS = IIf(String.IsNullOrEmpty(.ODCODFIS), "", .ODCODFIS)
                    If Not _OrdList.Contains(_ord.ODNUMORD) Then  ' If array already contains same order number, it don't check again
                        If .ODCODFIS.Trim = "" And .ODPARIVA = "" Then
                            Dim _strError As String = "Ordine < " & _ord.ODNUMORD & " > senza P.IVA e/o C.F. cliente >!"
                            filewriter.WriteLine(_strError)
                            _flError = True
                            _OrdList.Add(_ord.ODNUMORD)
                        End If
                    End If
                End With
            Next
            '
            For Each element As String In gDes.Keys
                _des = gDes(element)
                With _des
                    .DDPARIVA = IIf(String.IsNullOrEmpty(.DDPARIVA), "", .DDPARIVA)
                    .DDCODFIS = IIf(String.IsNullOrEmpty(.DDCODFIS), "", .DDCODFIS)
                    .DDNOMDES = IIf(String.IsNullOrEmpty(.DDNOMDES), "", .DDNOMDES)
                    Select Case Globale.CodAzi
                        Case "ALTRQ"
                            If .DDPARIVA.Trim = "" And .DDCODFIS = "" Then
                                Dim _strError As String = "Destinazione < " & _des.DDNOMDES & " > senza P.IVA e/o C.F. cliente >!"
                                filewriter.WriteLine(_strError)
                                _flError = True
                            End If
                        Case "COPTG"
                            If .DD_EMAIL.Trim = "" Then
                                Dim _strError As String = "Destinazione < " & _des.DDNOMDES & " > senza E-MAIL >!"
                                filewriter.WriteLine(_strError)
                                _flError = True
                            End If
                    End Select
                End With
            Next
            '
            filewriter.Flush()
            filewriter.Close()
            sw.Close()
            filewriter.Dispose()
            If _flError Then
                MsgBox("Riscontrati errori!", MsgBoxStyle.Critical)
                Me.fileLaunch()
            End If

            Return _flError
        Catch ex As Exception
            sw.Close()
            MsgBox(ex.Message, MsgBoxStyle.Critical, "checkData")
        End Try
    End Function
    Private Sub fileLaunch()
        Dim pInf As New System.Diagnostics.ProcessStartInfo
        pInf.FileName = "notepad.exe"
        pInf.Arguments = Environment.CurrentDirectory & "\" & "errori.txt"
        Process.Start(pInf)
    End Sub
    Private Function httpPost(ByVal pUrl As String) As Boolean


        Try
            If String.IsNullOrEmpty(pUrl.Trim) Then
                Return True
            End If
            Dim webStream As Stream
            Dim webResponse = ""
            Dim req As HttpWebRequest
            Dim res As HttpWebResponse
            Dim uriString As String = ""

            '
            req = HttpWebRequest.Create(pUrl)
            req.Method = "GET" ' Method of sending HTTP Request(GET/POST)
            res = req.GetResponse() ' Send Request
            webStream = res.GetResponseStream() ' Get Response
            Dim webStreamReader As New StreamReader(webStream)
            '
            ' READ Response in one Variable
            While webStreamReader.Peek >= 0

                'webResponse = webStreamReader.ReadToEnd()
                lb.Items.Add(webStreamReader.ReadToEnd())
                lb.Refresh()

            End While
        Catch ex As System.Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - httpPost()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "httpPost")
            End If
        End Try

    End Function

    Private Sub chkClienti_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClienti.CheckedChanged

    End Sub

    Private Sub btnFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilter.Click

        frmFilter.lbSito.Text = Me.txtFtpsrv.Text
        frmFilter.TxtIndexSite.Text = Me.txtSeq.Text
        frmFilter.Show()
    End Sub

    Private Sub chkAllItems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllItems.CheckedChanged
        If Me.chkAllItems.Checked Then
            Me.btnFilter.Enabled = False
        Else
            Me.btnFilter.Enabled = True
        End If
    End Sub
    Private Function xmlEnc(ByVal pstring As String) As String
        Try

            Dim sOutput As String = pstring
            'Dim sOutput As String = pstring.Replace("&", "&amp")
            'sOutput = sOutput.Replace("""", "&quot")
            'sOutput = sOutput.Replace("'", "&apos")
            'sOutput = sOutput.Replace("<", "&lt")
            'sOutput = sOutput.Replace(">", "&gt")
            '
            If gUTF8 = "S" Then
                'Return codifica(sOutput)
                Return htmlEncode(sOutput)
            Else
                Return sOutput
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "xmlEnc")
        End Try
    End Function
    Public Function htmlEncode(ByVal pStringa As Object) As String

        If pStringa Is DBNull.Value Then
            Return "NULL"
        End If
        If String.IsNullOrEmpty(pStringa) Then
            Return "NULL"
        End If
        Dim Testo As String = pStringa
        Dim lStringa As String = ""
        Try
            Testo = Testo.Replace("&", "&amp;")
            Testo = Testo.Replace("'", "&apos;")
            Testo = Testo.Replace("""", "&quot;")
            Testo = Testo.Replace("<", "&lt;")
            Testo = Testo.Replace(">", "&gt;")
            'Testo = Testo.Replace(" ", "&nbsp;")
            Testo = Testo.Replace("¡", "&iexcl;")
            Testo = Testo.Replace("¢", "&cent;")
            Testo = Testo.Replace("£", "&pound;")
            Testo = Testo.Replace("¤", "&curren;")
            Testo = Testo.Replace("¥", "&yen;")
            Testo = Testo.Replace("¦", "&brvbar;")
            Testo = Testo.Replace("§", "&sect;")
            Testo = Testo.Replace("¨", "&uml;")
            Testo = Testo.Replace("©", "&copy;")
            Testo = Testo.Replace("ª", "&ordf;")
            Testo = Testo.Replace("«", "&laquo;")
            Testo = Testo.Replace("¬", "&not;")
            'Testo = Testo.Replace(" ", "*")
            Testo = Testo.Replace("®", "&reg;")
            Testo = Testo.Replace("¯", "&macr;")
            Testo = Testo.Replace("°", "&deg;")
            Testo = Testo.Replace("±", "&plusmn;")
            Testo = Testo.Replace("²", "&sup2;")
            Testo = Testo.Replace("³", "&sup3;")
            Testo = Testo.Replace("´", "&acute;")
            Testo = Testo.Replace("µ", "&micro;")
            Testo = Testo.Replace("¶", "&para;")
            Testo = Testo.Replace("·", "&middot;")
            Testo = Testo.Replace("¸", "&cedil;")
            Testo = Testo.Replace("¹", "&sup1;")
            Testo = Testo.Replace("º", "&ordm;")
            Testo = Testo.Replace("»", "&raquo;")
            Testo = Testo.Replace("¼", "&frac14;")
            Testo = Testo.Replace("½", "&frac12;")
            Testo = Testo.Replace("¾", "&frac34;")
            Testo = Testo.Replace("¿", "&iquest;")
            Testo = Testo.Replace("À", "&Agrave;")
            Testo = Testo.Replace("Á", "&Aacute;")
            Testo = Testo.Replace("Â", "&Acirc;")
            Testo = Testo.Replace("Ã", "&Atilde;")
            Testo = Testo.Replace("Ä", "&Auml;")
            Testo = Testo.Replace("Å", "&Aring;")
            Testo = Testo.Replace("Æ", "&AElig;")
            Testo = Testo.Replace("Ç", "&Ccedil;")
            Testo = Testo.Replace("È", "&Egrave;")
            Testo = Testo.Replace("É", "&Eacute;")
            Testo = Testo.Replace("Ê", "&Ecirc;")
            Testo = Testo.Replace("Ë", "&Euml;")
            Testo = Testo.Replace("Ì", "&Igrave;")
            Testo = Testo.Replace("Í", "&Iacute;")
            Testo = Testo.Replace("Î", "&Icirc;")
            Testo = Testo.Replace("Ï", "&Iuml;")
            Testo = Testo.Replace("Ð", "&ETH;")
            Testo = Testo.Replace("Ñ", "&Ntilde;")
            Testo = Testo.Replace("Ò", "&Ograve;")
            Testo = Testo.Replace("Ó", "&Oacute;")
            Testo = Testo.Replace("Ô", "&Ocirc;")
            Testo = Testo.Replace("Õ", "&Otilde;")
            Testo = Testo.Replace("Ö", "&Ouml;")
            Testo = Testo.Replace("×", "&times;")
            Testo = Testo.Replace("Ø", "&Oslash;")
            Testo = Testo.Replace("Ù", "&Ugrave;")
            Testo = Testo.Replace("Ú", "&Uacute;")
            Testo = Testo.Replace("Û", "&Ucirc;")
            Testo = Testo.Replace("Ü", "&Uuml;")
            Testo = Testo.Replace("Ý", "&Yacute;")
            Testo = Testo.Replace("Þ", "&THORN;")
            Testo = Testo.Replace("ß", "&szlig;")
            Testo = Testo.Replace("à", "&agrave;")
            Testo = Testo.Replace("á", "&aacute;")
            Testo = Testo.Replace("â", "&acirc;")
            Testo = Testo.Replace("ã", "&atilde;")
            Testo = Testo.Replace("ä", "&auml;")
            Testo = Testo.Replace("å", "&aring;")
            Testo = Testo.Replace("æ", "&aelig;")
            Testo = Testo.Replace("ç", "&ccedil;")
            Testo = Testo.Replace("è", "&egrave;")
            Testo = Testo.Replace("é", "&eacute;")
            Testo = Testo.Replace("ê", "&ecirc;")
            Testo = Testo.Replace("ë", "&euml;")
            Testo = Testo.Replace("ì", "&igrave;")
            Testo = Testo.Replace("í", "&iacute;")
            Testo = Testo.Replace("î", "&icirc;")
            Testo = Testo.Replace("ï", "&iuml;")
            Testo = Testo.Replace("ð", "&eth;")
            Testo = Testo.Replace("ñ", "&ntilde;")
            Testo = Testo.Replace("ò", "&ograve;")
            Testo = Testo.Replace("ó", "&oacute;")
            Testo = Testo.Replace("ô", "&ocirc;")
            Testo = Testo.Replace("õ", "&otilde;")
            Testo = Testo.Replace("ö", "&ouml;")
            Testo = Testo.Replace("÷", "&divide;")
            Testo = Testo.Replace("ø", "&oslash;")
            Testo = Testo.Replace("ù", "&ugrave;")
            Testo = Testo.Replace("ú", "&uacute;")
            Testo = Testo.Replace("û", "&ucirc;")
            Testo = Testo.Replace("ü", "&uuml;")
            Testo = Testo.Replace("ý", "&yacute;")
            Testo = Testo.Replace("þ", "&thorn;")
            Testo = Testo.Replace("ÿ", "&yuml;")
            Testo = Testo.Replace("Œ", "&OElig;")
            Testo = Testo.Replace("œ", "&oelig;")
            Testo = Testo.Replace("Š", "&Scaron;")
            Testo = Testo.Replace("š", "&scaron;")
            Testo = Testo.Replace("Ÿ", "&Yuml;")
            Testo = Testo.Replace("ƒ", "&fnof;")
            Testo = Testo.Replace("ˆ", "&circ;")
            Testo = Testo.Replace("˜", "&tilde;")
            Testo = Testo.Replace("–", "&ndash;")
            Testo = Testo.Replace("—", "&mdash;")
            Testo = Testo.Replace("‘", "&lsquo;")
            Testo = Testo.Replace("’", "&rsquo;")
            Testo = Testo.Replace("‚", "&sbquo;")
            Testo = Testo.Replace("„", "&bdquo;")
            Testo = Testo.Replace("†", "&dagger;")
            Testo = Testo.Replace("‡", "&Dagger;")
            Testo = Testo.Replace("•", "&bull;")
            Testo = Testo.Replace("…", "&hellip;")
            Testo = Testo.Replace("‰", "&permil;")
            Testo = Testo.Replace("‹", "&lsaquo;")
            Testo = Testo.Replace("›", "&rsaquo;")
            Testo = Testo.Replace("€", "&euro;")
            Testo = Testo.Replace("™", "&trade;")

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
        Return Testo
    End Function
    Public Function htmlDecode(ByVal pStringa As String) As String


        Dim Testo As String = pStringa
        Dim lStringa As String = pStringa
        Try
            lStringa = lStringa.Replace("&quot;", """")
            lStringa = lStringa.Replace("&amp;", "&")
            lStringa = lStringa.Replace("&apos;", "'")
            lStringa = lStringa.Replace("&lt;", "<")
            lStringa = lStringa.Replace("&gt;", ">")
            lStringa = lStringa.Replace("&nbsp;", "")
            lStringa = lStringa.Replace("&iexcl;", "¡")
            lStringa = lStringa.Replace("&cent;", "¢")
            lStringa = lStringa.Replace("&pound;", "£")
            lStringa = lStringa.Replace("&curren;", "¤")
            lStringa = lStringa.Replace("&yen;", "¥")
            lStringa = lStringa.Replace("&brvbar;", "¦")
            lStringa = lStringa.Replace("&sect;", "§")
            lStringa = lStringa.Replace("&uml;", "¨")
            lStringa = lStringa.Replace("&copy;", "©")
            lStringa = lStringa.Replace("&ordf;", "ª")
            lStringa = lStringa.Replace("&laquo;", "«")
            lStringa = lStringa.Replace("&not;", "¬")
            'lStringa = lStringa.Replace("*", "")
            lStringa = lStringa.Replace("&reg;", "®")
            lStringa = lStringa.Replace("&macr;", "¯")
            lStringa = lStringa.Replace("&deg;", "°")
            lStringa = lStringa.Replace("&plusmn;", "±")
            lStringa = lStringa.Replace("&sup2;", "²")
            lStringa = lStringa.Replace("&sup3;", "³")
            lStringa = lStringa.Replace("&acute;", "´")
            lStringa = lStringa.Replace("&micro;", "µ")
            lStringa = lStringa.Replace("&para;", "¶")
            lStringa = lStringa.Replace("&middot;", "·")
            lStringa = lStringa.Replace("&cedil;", "¸")
            lStringa = lStringa.Replace("&sup1;", "¹")
            lStringa = lStringa.Replace("&ordm;", "º")
            lStringa = lStringa.Replace("&raquo;", "»")
            lStringa = lStringa.Replace("&frac14;", "¼")
            lStringa = lStringa.Replace("&frac12;", "½")
            lStringa = lStringa.Replace("&frac34;", "¾")
            lStringa = lStringa.Replace("&iquest;", "¿")
            lStringa = lStringa.Replace("&Agrave;", "À")
            lStringa = lStringa.Replace("&Aacute;", "Á")
            lStringa = lStringa.Replace("&Acirc;", "Â")
            lStringa = lStringa.Replace("&Atilde;", "Ã")
            lStringa = lStringa.Replace("&Auml;", "Ä")
            lStringa = lStringa.Replace("&Aring;", "Å")
            lStringa = lStringa.Replace("&AElig;", "Æ")
            lStringa = lStringa.Replace("&Ccedil;", "Ç")
            lStringa = lStringa.Replace("&Egrave;", "È")
            lStringa = lStringa.Replace("&Eacute;", "É")
            lStringa = lStringa.Replace("&Ecirc;", "Ê")
            lStringa = lStringa.Replace("&Euml;", "Ë")
            lStringa = lStringa.Replace("&Igrave;", "Ì")
            lStringa = lStringa.Replace("&Iacute;", "Í")
            lStringa = lStringa.Replace("&Icirc;", "Î")
            lStringa = lStringa.Replace("&Iuml;", "Ï")
            lStringa = lStringa.Replace("&ETH;", "Ð")
            lStringa = lStringa.Replace("&Ntilde;", "Ñ")
            lStringa = lStringa.Replace("&Ograve;", "Ò")
            lStringa = lStringa.Replace("&Oacute;", "Ó")
            lStringa = lStringa.Replace("&Ocirc;", "Ô")
            lStringa = lStringa.Replace("&Otilde;", "Õ")
            lStringa = lStringa.Replace("&Ouml;", "Ö")
            lStringa = lStringa.Replace("&times;", "×")
            lStringa = lStringa.Replace("&Oslash;", "Ø")
            lStringa = lStringa.Replace("&Ugrave;", "Ù")
            lStringa = lStringa.Replace("&Uacute;", "Ú")
            lStringa = lStringa.Replace("&Ucirc;", "Û")
            lStringa = lStringa.Replace("&Uuml;", "Ü")
            lStringa = lStringa.Replace("&Yacute;", "Ý")
            lStringa = lStringa.Replace("&THORN;", "Þ")
            lStringa = lStringa.Replace("&szlig;", "ß")
            lStringa = lStringa.Replace("&agrave;", "à")
            lStringa = lStringa.Replace("&aacute;", "á")
            lStringa = lStringa.Replace("&acirc;", "â")
            lStringa = lStringa.Replace("&atilde;", "ã")
            lStringa = lStringa.Replace("&auml;", "ä")
            lStringa = lStringa.Replace("&aring;", "å")
            lStringa = lStringa.Replace("&aelig;", "æ")
            lStringa = lStringa.Replace("&ccedil;", "ç")
            lStringa = lStringa.Replace("&egrave;", "è")
            lStringa = lStringa.Replace("&eacute;", "é")
            lStringa = lStringa.Replace("&ecirc;", "ê")
            lStringa = lStringa.Replace("&euml;", "ë")
            lStringa = lStringa.Replace("&igrave;", "ì")
            lStringa = lStringa.Replace("&iacute;", "í")
            lStringa = lStringa.Replace("&icirc;", "î")
            lStringa = lStringa.Replace("&iuml;", "ï")
            lStringa = lStringa.Replace("&eth;", "ð")
            lStringa = lStringa.Replace("&ntilde;", "ñ")
            lStringa = lStringa.Replace("&ograve;", "ò")
            lStringa = lStringa.Replace("&oacute;", "ó")
            lStringa = lStringa.Replace("&ocirc;", "ô")
            lStringa = lStringa.Replace("&otilde;", "õ")
            lStringa = lStringa.Replace("&ouml;", "ö")
            lStringa = lStringa.Replace("&divide;", "÷")
            lStringa = lStringa.Replace("&oslash;", "ø")
            lStringa = lStringa.Replace("&ugrave;", "ù")
            lStringa = lStringa.Replace("&uacute;", "ú")
            lStringa = lStringa.Replace("&ucirc;", "û")
            lStringa = lStringa.Replace("&uuml;", "ü")
            lStringa = lStringa.Replace("&yacute;", "ý")
            lStringa = lStringa.Replace("&thorn;", "þ")
            lStringa = lStringa.Replace("&yuml;", "ÿ")
            lStringa = lStringa.Replace("&OElig;", "Œ")
            lStringa = lStringa.Replace("&oelig;", "œ")
            lStringa = lStringa.Replace("&Scaron;", "Š")
            lStringa = lStringa.Replace("&scaron;", "š")
            lStringa = lStringa.Replace("&Yuml;", "Ÿ")
            lStringa = lStringa.Replace("&fnof;", "ƒ")
            lStringa = lStringa.Replace("&circ;", "ˆ")
            lStringa = lStringa.Replace("&tilde;", "˜")
            'lStringa = lStringa.Replace("&thinsp;", "")
            'lStringa = lStringa.Replace("&zwnj;", "")
            'lStringa = lStringa.Replace("&zwj;", "")
            'lStringa = lStringa.Replace("&lrm;", "")
            'lStringa = lStringa.Replace("&rlm;", "")
            lStringa = lStringa.Replace("&ndash;", "–")
            lStringa = lStringa.Replace("&mdash;", "—")
            lStringa = lStringa.Replace("&lsquo;", "‘")
            lStringa = lStringa.Replace("&rsquo;", "’")
            lStringa = lStringa.Replace("&sbquo;", "‚")
            lStringa = lStringa.Replace("&bdquo;", "„")
            lStringa = lStringa.Replace("&dagger;", "†")
            lStringa = lStringa.Replace("&Dagger;", "‡")
            lStringa = lStringa.Replace("&bull;", "•")
            lStringa = lStringa.Replace("&hellip;", "…")
            lStringa = lStringa.Replace("&permil;", "‰")
            lStringa = lStringa.Replace("&lsaquo;", "‹")
            lStringa = lStringa.Replace("&rsaquo;", "›")
            lStringa = lStringa.Replace("&euro;", "€")
            lStringa = lStringa.Replace("&trade;", "™")

            Return pStringa
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
        Return lStringa
    End Function
    Private Function codifica(ByVal pStringa As String) As String
        Dim encodedBytes As Byte()
        Dim value As Encoding
        Dim riga As String
        Dim decodedString As String = ""
        Dim sOutput As String = ""
        Try
            Dim utf8 As New UTF8Encoding()
            riga = pStringa

            riga = htmlEncode(riga)
            encodedBytes = utf8.GetBytes(riga)
            decodedString = utf8.GetString(encodedBytes)
            sOutput = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(decodedString))
            sOutput = Regex.Replace(decodedString, "[^\w\.,:;#@*/\-_+&!()?=<> \[\]%]", "")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Codifica")
        End Try
        Return sOutput
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        CheckPrice()

    End Sub
    Private Sub CheckPrice()

        Try
            Dim webStream As Stream
            Dim webResponse = ""
            Dim req As HttpWebRequest
            Dim res As HttpWebResponse
            Dim uriString As String = ""
            Dim _list As New sListini
            Dim _hlist As New Hashtable
            Dim _slist As New ArrayList
            '
            If Me.cmbSitecheck.Text.Trim = "" Then
                MsgBox("Indicare sito!")
                Exit Sub
            End If
            '
            'Reads adhoc prices list
            lbstatus.Items.Clear()
            lbstatus.Refresh()
            '
            lbstatus.Items.Add("Lettura listini AdHoc")
            lbstatus.Refresh()
            '
            Dim strsql As String = op.getQuery(Globale.CodAzi, "getlistini.vqr")
            Dim ds As DataSet = op.esegui_query(strsql)
            For Each dr As DataRow In ds.Tables(0).Rows
                _list = New sListini
                If IsNumeric(dr.Item("LICODLIS").ToString.Trim) Then
                    With _list
                        .ahrlicodart = dr.Item("LICODART").ToString.Trim
                        .ahrlicodlis = dr.Item("LICODLIS").ToString.Trim
                        .ahrcprownum = dr.Item("LIROWNUM")
                        .ahrliprezzo = Convert.ToDecimal(dr.Item("LIPREZZO").ToString.Replace(".", ","))
                        .ahrliquanti = Convert.ToDecimal(dr.Item("LIQUANTI").ToString.Replace(".", ","))
                        .ahrliscont1 = Convert.ToDecimal(dr.Item("LISCONT1").ToString.Replace(".", ","))
                        .ahrliscont2 = Convert.ToDecimal(dr.Item("LISCONT2").ToString.Replace(".", ","))
                        .ahrliscont3 = Convert.ToDecimal(dr.Item("LISCONT3").ToString.Replace(".", ","))
                        .ahrliscont4 = Convert.ToDecimal(dr.Item("LISCONT4").ToString.Replace(".", ","))
                        .ahrlidatini = dr.Item("LIDATATT")
                        .ahrlidatfin = dr.Item("LIDATDIS")
                        Dim _key As String = .ahrlicodart & "|" & .ahrcprownum & "|" & .ahrliquanti
                        _slist.Add(_key)
                        _hlist.Add(_key, _list)
                    End With
                End If
                Application.DoEvents()
            Next
            '
            'Reads web stream
            '
            '
            lbstatus.Items.Add("Lettura listini Sito")
            lbstatus.Refresh()
            '
            req = HttpWebRequest.Create(Me.cmbSitecheck.Text.Trim)
            req.Method = "GET" ' Method of sending HTTP Request(GET/POST)
            res = req.GetResponse() ' Send Request
            webStream = res.GetResponseStream() ' Get Response
            Dim webStreamReader As New StreamReader(webStream)
            '
            ' READ Response in one Variable
            Dim _ii As Integer = 1
            '
            While webStreamReader.Peek >= 0
                '
                webResponse = webStreamReader.ReadToEnd()
                '
            End While
            Dim _exists As Boolean = False
            For Each _string As String In webResponse.split(Chr(10))
                If _string.Split(";").Length < 11 Then
                    Continue For
                End If
                _list = New sListini
                Dim _key As String = _string.Split(";")(3) & "|" & _string.Split(";")(1) & "|" & Convert.ToDecimal(_string.Split(";")(4).Replace(".", ",")) 'codart,cprownum,quanti
                If _string.Split(";")(0) <> "0" Then 'licodlis
                    With _list
                        _exists = False
                        If _hlist.ContainsKey(_key) Then
                            _list = _hlist(_key)
                            _exists = True
                        End If
                        .weblicodlis = _string.Split(";")(0)
                        .webcprownum = _string.Split(";")(1)
                        .webliprezzo = Convert.ToDecimal(_string.Split(";")(2).Replace(".", ","))
                        .weblicodart = _string.Split(";")(3)
                        .webliquanti = Convert.ToDecimal(_string.Split(";")(4).Replace(".", ","))
                        .webliscont1 = Convert.ToDecimal(_string.Split(";")(5).Replace(".", ","))
                        .webliscont2 = Convert.ToDecimal(_string.Split(";")(6).Replace(".", ","))
                        .webliscont3 = Convert.ToDecimal(_string.Split(";")(7).Replace(".", ","))
                        .webliscont4 = Convert.ToDecimal(_string.Split(";")(8).Replace(".", ","))
                        .weblidatini = _string.Split(";")(9)
                        .weblidatfin = _string.Split(";")(10)
                    End With
                    With _list
                        If _exists Then
                            _hlist(_key) = _list
                        Else
                            _hlist.Add(_key, _list)
                            _slist.Add(_key)
                        End If
                    End With
                End If
                _ii = _ii + 1
                Application.DoEvents()
            Next
            '
            lbstatus.Items.Add("Confronto listini")
            lbstatus.Refresh()
            '
            'Writes check file
            '
            Dim _record As String = ""
            Dim sw As System.IO.FileStream = New System.IO.FileStream("checkFile.csv", IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            '
            _record = "Articolo AHR" & ";" & "Articolo WEB" & ";" & "Listino" & ";" & _
                      "Prz. AHR" & ";" & "Prz. WEB" & ";" & "Qta. AHR" & ";" & "Qta. WEB" & ";" & "Sc1 AHR" & ";" & "Sc1 WEB" & ";" & _
                      "Sc2 AHR" & ";" & "Sc2 WEB" & ";" & "Sc3 AHR" & ";" & "Sc3 WEB" & ";" & "Sc4 AHR" & ";" & "Sc4 WEB"
            '
            filewriter.WriteLine(_record)
            _slist.Sort()
            For Each _key As String In _slist
                _list = New sListini
                _list = _hlist(_key)
                With _list
                    If .ahrliprezzo <> .webliprezzo Or _
                       .ahrliscont1 <> .webliscont1 Or _
                       .ahrliscont2 <> .webliscont2 Or _
                       .ahrliscont3 <> .webliscont3 Or _
                       .ahrliscont4 <> .webliscont4 Or _
                       .ahrliquanti <> .webliquanti Then
                        _record = "'" & .ahrlicodart & "';'" & .weblicodart & "';" & _
                                  .ahrlicodlis & ";" & _
                                  .ahrliprezzo & ";" & .webliprezzo & ";" & _
                                  .ahrliquanti & ";" & .webliquanti & ";" & _
                                  .ahrliscont1 & ";" & .webliscont1 & ";" & _
                                  .ahrliscont2 & ";" & .webliscont2 & ";" & _
                                  .ahrliscont3 & ";" & .webliscont3 & ";" & _
                                  .ahrliscont4 & ";" & .webliscont4 & Chr(9)
                        filewriter.WriteLine(_record)
                    End If
                End With
                Application.DoEvents()
            Next
            filewriter.Close()
            sw = Nothing
            '
            lbstatus.Items.Add("Apertura file")
            lbstatus.Refresh()
            '
            '
            Dim pInf As New System.Diagnostics.ProcessStartInfo
            pInf.FileName = "checkfile.csv"
            'pInf.Arguments = Environment.CurrentDirectory & "\" & "errori.txt"
            Process.Start(pInf)
            '
        Catch ex As System.Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & " - httpPost()")
            Else
                MsgBox(ex.Message, MsgBoxStyle.Information, "httpPost")
            End If
        End Try
    End Sub
    Private Sub readCheckHost()

        Try
            '
            Dim sw As System.IO.FileStream = New System.IO.FileStream("checkhost.ini", FileMode.Open)
            Dim objReader As New StreamReader(sw)
            Dim sLine As String = ""

            Do
                sLine = objReader.ReadLine()
                Me.cmbSitecheck.Items.Add(sLine.Trim)
            Loop Until sLine Is Nothing
            objReader.Close()
            sw = Nothing
            '
        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try
            ' checks if the host typed already exists in cmbsitecheck
            For Each element As String In Me.cmbSitecheck.Items
                If element.Trim = Me.txtchechkhost.Text.Trim Then
                    Exit Sub
                End If
            Next
            Me.cmbSitecheck.Items.Add(Me.txtchechkhost.Text.Trim)
            'Saves combo into the file 
            Dim sw As System.IO.FileStream = New System.IO.FileStream("checkhost.ini", IO.FileMode.Create)
            Dim filewriter As New System.IO.StreamWriter(sw)
            For Each element As String In Me.cmbSitecheck.Items
                filewriter.WriteLine(element)
            Next
            filewriter.Close()
            sw = Nothing
            '
        Catch ex As Exception

        End Try

    End Sub
    Private Function Sendmail(ByVal subject As String, ByVal body As String) As Boolean

        Dim mittente As MailAddress
        'Dichiara un oggetto destinato a contenere l'indirizzo di posta del destinatario
        Dim destAddress As MailAddress

        'Dichiara un oggetto destinato a contenere l'indirizzo di posta di chi legge per conoscenza
        Dim ccAddress As MailAddress
        'Dim bccAddress As MailAddress
        'Dichiara un oggetto che conterrà un singolo allegato
        'Dim attachment As Attachment
        '
        Dim mex As MailMessage

        'Istanzia un nuovo indirizzo di posta, dato l'indirizzo e.mail e il nome completo dell'autore
        mittente = New MailAddress("adhocwebsync@copt.it", "adhocwebsync@copt.it")
        destAddress = New MailAddress(Globale.g_emailTo, Globale.g_emailTo)
        'Istanzia un nuovo oggetto relativo alla copia per conoscenza,dato l'indirizzo e.mail
        ccAddress = New MailAddress(Globale.g_emailCC, Globale.g_emailCC)
        'Istanzia un nuovo oggetto MailMessage,che contiene tutti  gli elementi necessari per
        'inviare il messaggio di posta
        mex = New MailMessage
        '
        With mex

            'Assegna agli elementi del messaggio ciò che è stato specificato nel form
            'oltre all'autore del messaggio, istanziato precedentemente
            .Subject = subject
            .Sender = mittente
            .Body = body
            .From = mittente
            .ReplyTo = mittente
            .To.Add(destAddress)
            .CC.Add(ccAddress)

            'Specifica quale messaggio di notifica deve essere inviato al mittente
            'In questo caso solo se l'invio del messaggio fallisce.
            .DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
        End With
        '
        'Esegue un ciclo sulla Listbox al fine di aggiungere alla Collection
        'che contiene gli allegati (Attachments) ogni file elencato.
        'If ml.Attach.Trim <> "" Then
        'attachment = New Attachment(ml.Attach)
        'mex.Attachments.Add(attachment)
        'attachment = Nothing
        'End If
        '
        Dim Client As New SmtpClient(Globale.g_SMTP)
        Try
            'abilita l'utilizzo della crittografia nell'invio dei messaggi
            'si tenga conto che non tutti i destinatari possono supportare questo metodo.
            'Client.EnableSsl = True

            'E' possibile specificare l'indirizzo IP dell'Host:
            'Client.Host = "210.333.3.1"
            If Globale.g_Ssl = "SI" Then
                Client.EnableSsl = True
            Else
                Client.EnableSsl = False
            End If
            'Client.Port = Convert.ToInt32(cf.g_Port)
            If Globale.g_Auth = "SI" Then
                Client.Credentials = New System.Net.NetworkCredential(Globale.g_Utente, Globale.g_Password)
            Else
                Client.Credentials = New System.Net.NetworkCredential("", "")
            End If
            Client.Send(mex)
            Return True
            'Eccezioni specifiche per l'invio di e-mail
        Catch ex As SmtpFailedRecipientException
            If _g_auto Then
                Me.WriteLog(ex.Message & vbCr & ex.InnerException.Message)
            Else
                MessageBox.Show(ex.Message & vbCr & ex.InnerException.Message)
            End If
            Return False
        Catch ex As SmtpException
            If _g_auto Then
                Me.WriteLog(ex.Message & vbCr & ex.InnerException.Message)
            Else
                MessageBox.Show(ex.Message & vbCr & ex.InnerException.Message)
            End If
            Return False
        Catch ex As InvalidOperationException
            If _g_auto Then
                Me.WriteLog(ex.Message & vbCr & ex.InnerException.Message)
            Else
                MessageBox.Show(ex.Message & vbCr & ex.InnerException.Message)
            End If
            Return False
        Catch ex As Exception
            If _g_auto Then
                Me.WriteLog(ex.Message & vbCr & ex.InnerException.Message)
            Else
                MessageBox.Show(ex.Message.ToString & vbCr & ex.InnerException.Message)
            End If
            Return False
        End Try
    End Function

    Private Sub chkAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged

    End Sub
End Class