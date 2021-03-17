Imports System
Imports System.io
Imports System.Net
Imports System.Net.Sockets
Imports System.Globalization
Imports System.Text
Imports Microsoft.VisualBasic
Imports iTextSharp
Imports iTextSharp.text.pdf


Public Class formImport

    'For moving form by dragging
    Dim loca As New Point(0, 0)
    Dim curs As New Point(System.Windows.Forms.Cursor.Position)
    Dim curpos As New Point(0, 0)
    '
    Private Structure Ive
        Dim _impIva As Decimal
        Dim _imponi As Decimal
        Dim _codcon As String
    End Structure
    '
    Private Structure RifPar
        Dim _pnserial As String
        Dim _cproword As Integer
        Dim _cprownum As Integer
    End Structure
    Private Structure pnt_dett
        Dim _Numdoc As String
        Dim _alfdoc As String
        Dim _DatDoc As String
        Dim _Codese As String
        Dim _DatReg As String
        Dim _tipcon As String
        Dim _Codcli As String
        Dim _Segno As String
        Dim _Importo As Decimal
        Dim _Conto As String
        Dim _Codiva As String
        Dim _impIva As Decimal
        Dim _Imponi As Decimal
        Dim _Codpag As String
    End Structure

    Dim cf As New CConfig
    Dim op As New COpeFile
    '
    Dim _hspn As New Hashtable
    Dim _h_hspn As New CHashGest(_hspn)
    '
    Dim _aKey_pn As New ArrayList
    '
    Dim _hCli As New Hashtable ' Contains the traslation from GECIM customer code to ADHOC customer code.
    Dim _h_hCli As New CHashGest(_hCli)
    '
    Dim importedFolder As String = ""
    Private Sub formEva_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        cn_dbf.Close()
    End Sub

    Private Sub FormExport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If String.IsNullOrEmpty(Globale.FileCli) Or _
           String.IsNullOrEmpty(Globale.CodAzi) Or _
           String.IsNullOrEmpty(Globale.PathFile) Or _
           String.IsNullOrEmpty(Globale.FileEmiFat) Or _
           String.IsNullOrEmpty(Globale.FileIncFat) Or _
           String.IsNullOrEmpty(Globale.CauEmi) Or _
           String.IsNullOrEmpty(Globale.CauInc) Or _
           String.IsNullOrEmpty(Globale.CauNC) Or _
           String.IsNullOrEmpty(Globale.MastroCli) Then
            Config.ShowDialog()
            Exit Sub
        End If
        Me.txtEmifile.Text = Globale.FileEmiFat
        Me.txtIncFile.Text = Globale.FileIncFat
        Me.txtCliFile.Text = Globale.FileCli
        Me.txtFolder.Text = Globale.PathFile
        Me.txtCodAzi.Text = Globale.CodAzi
        '
        importedFolder = Globale.PathFile.TrimEnd("\") & "\importati\"

        If Not System.IO.Directory.Exists(importedFolder) Then
            System.IO.Directory.CreateDirectory(importedFolder)
        End If
        op.imposta_connessione(Globale.ConnectionString)
        'cn_ext.ConnectionString = Globale.ConnectionString
        'cn_ext.Open()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Config.Show()
    End Sub

     Private Sub shpMain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles shpMain.Click
        curpos = Cursor.Position
    End Sub
    Private Sub Position()
        loca = Me.Location
        curs = System.Windows.Forms.Cursor.Position
    End Sub

    Private Sub shpMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles shpMain.MouseDown
        Timer1.Start()
        Position()
    End Sub

    Private Sub shpmain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles shpMain.MouseUp
        Timer1.Stop()
        Position()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Location = loca - curs + System.Windows.Forms.Cursor.Position
    End Sub
    Private Sub btnUscita_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUscita.Click
        Application.Exit()
    End Sub
    Private Sub btnRiduci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRiduci.Click
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        'Questa funzione serve per intercettare il tasto F10 e fare in modo che quando si preme non si abiliti il menu

        Select Case keyData
            Case Keys.Escape 'Uscita
                Application.Exit()
            Case Else
                Return MyBase.ProcessCmdKey(msg, keyData)
        End Select
        Return True
        'Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFolder.Click
    End Sub

    Private Sub btnChangeFold_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeFold.Click
        Dim savePath As String = System.IO.Directory.GetCurrentDirectory
        With FBD1
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtFolder.Text = .SelectedPath
            End If
        End With
        'With OFD1
        '.Filter = "Tutti i files|*.*"
        'If .ShowDialog = Windows.Forms.DialogResult.OK Then
        'For Each oFile As String In .FileNames
        'lstSpedire.Items.Add(oFile)
        'Next
        'End If
        'End With
        'Me.btnSave_Click(Nothing, Nothing)
        System.IO.Directory.SetCurrentDirectory(savePath)
    End Sub

    Private Sub btnImporta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImporta.Click
        _hCli.Clear()
        Me.ImportaCli()
        Me.ImportaFatture()
        MsgBox("Importazione terminata", MsgBoxStyle.Exclamation)
        'Me.ImportaIncassi()
    End Sub
    Private Sub ImportaCli()

        Try
            Dim rigafile As String = ""
            Dim wFileName As String = "*" & Me.txtCliFile.Text.Trim
            Dim di As New DirectoryInfo(Me.txtFolder.Text.TrimEnd)
            Dim fi As FileInfo
            For Each fi In di.GetFiles(wFileName)
                '
                Dim sr As System.IO.FileStream
                sr = New System.IO.FileStream(Me.txtFolder.Text.TrimEnd("\") & "\" & fi.Name, IO.FileMode.Open, IO.FileAccess.Read)
                Dim filereader As New System.IO.StreamReader(sr)
                While filereader.Peek > -1
                    rigafile = filereader.ReadLine
                    Dim wCodFisc As String = rigafile.Substring(155, 16)
                    Dim wPiva As String = rigafile.Substring(143, 12).Trim
                    Dim wTipCon As String = "C"
                    Dim wCodice As String = rigafile.Substring(19, 10).Trim
                    Dim wRagsoc As String = rigafile.Substring(31, 40).Trim
                    Dim wIndiri As String = rigafile.Substring(71, 30).Trim
                    Dim wCap As String = rigafile.Substring(101, 5).Trim
                    Dim wLocali As String = rigafile.Substring(106, 30).Trim
                    Dim WProvin As String = rigafile.Substring(136, 2).Trim
                    Dim wPerFis As String = IIf(String.IsNullOrEmpty(wPiva), "S", "")
                    Dim wStrsql As String
                    If wCodFisc.Trim <> "" Then
                        wStrsql = "select ancodice from " & Me.GetTableNameLocal("CONTI") & " where ancodfis = " & op.ValAdapter(wCodFisc, TipoCampo.TChar)
                    Else
                        wStrsql = "select ancodice from " & Me.GetTableNameLocal("CONTI") & " where anpariva = " & op.ValAdapter(wPiva, TipoCampo.TChar)
                    End If
                    Dim ds As DataSet = op.esegui_query(wStrsql)
                    If ds.Tables(0).Rows.Count = 0 Then
                        wStrsql = "insert into " & Me.GetTableNameLocal("CONTI") & _
                               "(ANTIPCON, ANCODICE, ANDESCRI, ANINDIRI, AN___CAP, ANLOCALI, ANPROVIN, ANCODFIS, ANPARIVA, ANCODPAG, " & _
                               "ANCONSUP, ANPERFIS, ANPARTSN, ANBOLFAT, ANPREBOL, ANSCORPO, ANCODLIN, ANFLESIG) VALUES (" & _
                               op.ValAdapter(wTipCon, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wCodice, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wRagsoc, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wIndiri, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wCap, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wLocali, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(WProvin, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wCodFisc, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wPiva, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(Globale.codPag, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(Globale.MastroCli, TipoCampo.TChar) & ", " & _
                               op.ValAdapter(wPerFis, TipoCampo.TChar) & ", " & _
                               "'S', 'N', 'N', 'N', 'IT', 'N')"
                        op.esegui_query(wStrsql)
                        Me._h_hCli.CHGput(wCodice.Trim, wCodice.Trim)
                    Else
                        Me._h_hCli.CHGput(wCodice.Trim, ds.Tables(0).Rows(0).Item("ANCODICE").trim)
                    End If
                End While
                '
                sr.Close()
                '
                'Spostamento file
                '
                Dim RenFile As String = "CLI-" & Now.Year & "-" & Now.DayOfYear & "-" & Now.TimeOfDay.Hours & Now.TimeOfDay.Minutes & Now.TimeOfDay.Seconds & ".txt"
                Dim RenPathfilename As String = importedFolder.TrimEnd("\") & "\" & RenFile
                Dim pathFileNameToRemove As String = Me.txtFolder.Text.TrimEnd("\") & "\" & fi.Name
                '
                System.IO.File.Move(pathFileNameToRemove, RenPathfilename)
                '
            Next
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import Clienti")
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import Clienti")
        End Try
    End Sub
    Private Sub ImportaFatture()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim _flNC As Boolean = False
            Dim wFileName As String = "*" & Me.txtEmifile.Text.Trim
            'Lettura file

            'Dim wFileName As String = "*" & Me.txtCliFile.Text.Trim
            Dim di As New DirectoryInfo(Me.txtFolder.Text.TrimEnd)
            Dim fi As FileInfo
            For Each fi In di.GetFiles(wFileName)
                '
                Dim sr As System.IO.FileStream
                If fi.Name.Substring(0, 1) = "N" Then
                    _flNC = True
                Else
                    _flNC = False
                End If
                sr = New System.IO.FileStream(Me.txtFolder.Text.TrimEnd("\") & "\" & fi.Name, IO.FileMode.Open, IO.FileAccess.Read)
                Dim filereader As New System.IO.StreamReader(sr)
                Dim _Index As Integer = 1
                While filereader.Peek > -1
                    rigafile = filereader.ReadLine
                    '
                    _prog = _Index.ToString.PadLeft(4, "0")
                    With _pnt_dett
                        ._Numdoc = rigafile.Substring(14, 6).Trim
                        ._alfdoc = rigafile.Substring(20, 2).Trim
                        ._DatDoc = rigafile.Substring(28, 2) & "-" & rigafile.Substring(26, 2) & "-" & rigafile.Substring(22, 4)
                        ._Codese = rigafile.Substring(22, 4)
                        ._DatReg = rigafile.Substring(36, 2) & "-" & rigafile.Substring(34, 2) & "-" & rigafile.Substring(30, 4)
                        ._tipcon = rigafile.Substring(38, 1)
                        ._Codcli = rigafile.Substring(39, 6).Trim
                        ._Segno = rigafile.Substring(51, 1)
                        ._Importo = CTran(rigafile.Substring(52, 13), 0) / 100
                        If Not String.IsNullOrEmpty(Globale.codPag) Then
                            ._Codpag = Globale.codPag.Trim
                        Else
                            ._Codpag = rigafile.Substring(65, 5).Trim
                        End If
                        ._Conto = rigafile.Substring(116, 13).Trim
                        If ._Segno = "D" Then
                            ._Codcli = Me._h_hCli.CHGGet(._Codcli.Trim)
                            ._Conto = ._Codcli
                        End If
                        ._Codiva = rigafile.Substring(70, 5).Trim
                        ._Imponi = CTran(rigafile.Substring(75, 13), 0) / 100
                        ._impIva = CTran(rigafile.Substring(88, 13), 0) / 100
                        _a_key = ._Numdoc & "#" & ._alfdoc & "#" & ._Codese & "#" & _prog
                    End With
                    Me._aKey_pn.Add(_a_key)
                    Me._h_hspn.CHGput(_a_key, _pnt_dett)
                    _Index = _Index + 1
                End While
                sr.Close()
                '
                'Controllo conti, codici pagamenti e causali
                '
                Dim _flError As Boolean = False
                'elimina il file
                System.IO.File.Delete(Environment.CurrentDirectory & "\" & "errori.txt")
                '
                Dim sw As System.IO.FileStream
                sw = New System.IO.FileStream(Environment.CurrentDirectory & "\" & "errori.txt", IO.FileMode.Create, IO.FileAccess.Write)
                Dim filewriter As New System.IO.StreamWriter(sw)
                '
                'Codici causali
                Dim wStrsql = "select CCCODICE AS MODPAG from " & Me.GetTableNameLocal("CAU_CONT") & " where CCCODICE = " & op.ValAdapter(Globale.CauEmi, TipoCampo.TChar)
                Dim ds As DataSet = op.esegui_query(wStrsql)
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim _strError As String = "Causale emissione fatture < " & Globale.CauEmi & " > sconosciuta!"
                    filewriter.WriteLine(_strError)
                    _flError = True
                End If
                '
                wStrsql = "select CCCODICE AS MODPAG from " & Me.GetTableNameLocal("CAU_CONT") & " where CCCODICE = " & op.ValAdapter(Globale.CauEmi, TipoCampo.TChar)
                ds = op.esegui_query(wStrsql)
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim _strError As String = "Causale incassi < " & Globale.CauInc & " > sconosciuta!"
                    filewriter.WriteLine(_strError)
                    _flError = True
                End If
                '
                wStrsql = "select CCCODICE AS MODPAG from " & Me.GetTableNameLocal("CAU_CONT") & " where CCCODICE = " & op.ValAdapter(Globale.CauNC, TipoCampo.TChar)
                ds = op.esegui_query(wStrsql)
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim _strError As String = "Causale note di credito cliente < " & Globale.CauNC & " > sconosciuta!"
                    filewriter.WriteLine(_strError)
                    _flError = True
                End If
                '
                For Each element As String In Me._aKey_pn
                    _pnt_dett = Me._h_hspn.CHGGet(element)
                    With _pnt_dett
                        'Conti
                        Dim strsql As String = "select ancodice from " & Me.GetTableNameLocal("CONTI") & _
                                               " where antipcon = " & op.ValAdapter(._tipcon, TipoCampo.TChar) & _
                                               " and ancodice = " & op.ValAdapter(._Conto, TipoCampo.TChar)
                        ds = op.esegui_query(strsql)
                        If ds.Tables(0).Rows.Count = 0 Then
                            Dim _strError As String = "Conto < " & ._Conto & " > sconosciuto! Documento n. " & ._Numdoc & " del " & ._DatDoc
                            filewriter.WriteLine(_strError)
                            _flError = True
                        End If
                        'Codici pagamento
                        If ._tipcon = "C" Then
                            wStrsql = "select distinct(P2MODPAG) AS MODPAG from " & Me.GetTableNameLocal("PAG_2AME") & " where P2CODICE = " & op.ValAdapter(._Codpag, TipoCampo.TChar)
                            ds = op.esegui_query(wStrsql)
                            If ds.Tables(0).Rows.Count = 0 Then
                                Dim _strError As String = "Codice pagamento < " & ._Codpag & " > sconosciuto!"
                                filewriter.WriteLine(_strError)
                                _flError = True
                            End If
                        End If
                    End With
                Next
                filewriter.Flush()
                filewriter.Close()
                sw.Close()
                filewriter.Dispose()
                If _flError Then
                    MsgBox("Riscontrati errori!", MsgBoxStyle.Critical)
                    Me.fileLaunch()
                    Exit Sub
                End If
                '
                '
                'Spostamento file
                '
                Dim RenFile As String = "EMI-" & Now.Year & "-" & Now.DayOfYear & "-" & Now.TimeOfDay.Hours & Now.TimeOfDay.Minutes & Now.TimeOfDay.Seconds & ".txt"
                Dim RenPathfilename As String = importedFolder.TrimEnd("\") & "\" & RenFile
                Dim pathFileNameToRemove As String = Me.txtFolder.Text.TrimEnd("\") & "\" & fi.Name
                '
                System.IO.File.Move(pathFileNameToRemove, RenPathfilename)
                '
                'Scrittura documenti
                '
                Dim _hsIve As New Hashtable
                Dim _h_hsive As New CHashGest(_hsIve)
                '
                Dim _PairFldVal As New Hashtable
                Dim _t_numdoc As String = ""
                Dim _t_alfdoc As String = ""
                Dim _cprownum As Integer = 0
                Dim _impdar As Decimal = 0
                Dim _impave As Decimal = 0
                Dim _pnserial As String = ""
                Dim _TotimpDare As Decimal = 0
                Dim _TotimpAver As Decimal = 0
                Dim _pnProg As Integer = 0
                _aKey_pn.Sort()
                op.BeginTrans()
                Try
                    For Each element As String In Me._aKey_pn
                        '
                        _pnt_dett = Me._h_hspn.CHGGet(element)
                        If _t_numdoc = "" Or _t_numdoc <> _pnt_dett._Numdoc Or _t_alfdoc <> _pnt_dett._alfdoc Then
                            'Scrittura della testata primanota
                            If _t_numdoc <> "" Then
                                If Not Me.FineReg(_pnserial, IIf(_flNC, _TotimpAver, _TotimpDare), _hsIve) Then
                                    Throw New Exception("Fallita fine registrazione")
                                End If
                                _hsIve.Clear()
                            End If
                            _TotimpDare = 0
                            _TotimpAver = 0
                            _t_numdoc = _pnt_dett._Numdoc
                            _t_alfdoc = _pnt_dett._alfdoc
                            _pnserial = Me.pnSerial().ToString.PadLeft(10, "0")
                            _cprownum = 1
                            With _pnt_dett
                                _pnProg = Me.pnProg(._Codese)
                                Dim _Month As Integer = Convert.ToDateTime(._DatDoc).Month
                                _PairFldVal.Clear()
                                _PairFldVal.Add("PNSERIAL", op.ValAdapter(_pnserial, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODESE", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODUTE", "0")
                                _PairFldVal.Add("PNNUMRER", op.ValAdapter(_pnProg, TipoCampo.TInt))
                                _PairFldVal.Add("PNDATREG", op.ValAdapter(._DatReg, TipoCampo.TData))
                                _PairFldVal.Add("PNCODCAU", op.ValAdapter(IIf(_flNC, Globale.CauNC, Globale.CauEmi), TipoCampo.TChar))
                                _PairFldVal.Add("PNCOMPET", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPREG", op.ValAdapter("V", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLIVDF", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLGDIF", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMREG", op.ValAdapter("1", TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPDOC", op.ValAdapter(IIf(_flNC, "NC", "FA"), TipoCampo.TChar))
                                _PairFldVal.Add("PNPRG", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNPRP", op.ValAdapter("NN", TipoCampo.TChar))
                                _PairFldVal.Add("PNPRD", op.ValAdapter("FV", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMDOC", op.ValAdapter(._Numdoc, TipoCampo.TChar))
                                _PairFldVal.Add("PNALFDOC", op.ValAdapter(._alfdoc, TipoCampo.TChar))
                                _PairFldVal.Add("PNALFPRO", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNDATDOC", op.ValAdapter(._DatDoc, TipoCampo.TData))
                                _PairFldVal.Add("PNANNPRO", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNANNDOC", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODVAL", op.ValAdapter("EUR", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMPRO", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPCLF", op.ValAdapter("C", TipoCampo.TChar))
                                _PairFldVal.Add("PNDESSUP", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNTOTDOC", op.ValAdapter("0", TipoCampo.TCur))
                                _PairFldVal.Add("PNCODCLF", op.ValAdapter(._Conto, TipoCampo.TChar))
                                _PairFldVal.Add("UTCV", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("UTCC", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("UTDC", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                                _PairFldVal.Add("UTDV", op.ValAdapter("", TipoCampo.TData))
                                _PairFldVal.Add("PNCOMIVA", op.ValAdapter(._DatDoc, TipoCampo.TData))
                                _PairFldVal.Add("PNDATPLA", op.ValAdapter(._DatDoc, TipoCampo.TData))
                                _PairFldVal.Add("PNFLPROV", op.ValAdapter("N", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLREGI", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFINC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNCAOVAL", op.ValAdapter("1", TipoCampo.TCur))
                                _PairFldVal.Add("PNRIFDIS", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFDOC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFCES", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNVALNAZ", op.ValAdapter("EUR", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMTRA", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMTR2", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFACC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFSAL", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                                _PairFldVal.Add("PNSCRASS", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLGSTO", op.ValAdapter("S", TipoCampo.TChar))
                                _PairFldVal.Add("PNTOTENA", op.ValAdapter("0", TipoCampo.TCur))
                                _PairFldVal.Add("PN__ANNO", op.ValAdapter(._Codese, TipoCampo.TCur))
                                _PairFldVal.Add("PN__MESE", op.ValAdapter(_Month, TipoCampo.TChar))

                                Dim _strFld As String = ""
                                Dim _strVal As String = ""
                                Dim strsql As String = "insert into " & Me.GetTableNameLocal("PNT_MAST") & " ( "

                                For Each _element As String In _PairFldVal.Keys
                                    _strFld = _strFld & _element & ","
                                    _strVal = _strVal & _PairFldVal(_element) & ","
                                Next
                                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                                '
                                op.esegui_query(strsql)
                                '
                            End With
                            '
                        End If
                        _PairFldVal.Clear()
                        'Fine scrittura testata primanota
                        '
                        'Scrittura dettaglio
                        '
                        With _pnt_dett
                            If ._Segno = "D" Then
                                _impave = 0
                                _impdar = ._Importo
                                _TotimpDare = _TotimpDare + ._Importo
                            Else
                                _impdar = 0
                                _impave = ._Importo
                                _TotimpAver = _TotimpAver + ._Importo
                            End If
                            '
                            If ._Codiva <> "00000" Then
                                Dim _h_ive As New Ive
                                _h_ive = _h_hsive.CHGGet(._Codiva)
                                _h_ive._impIva = _h_ive._impIva + ._impIva
                                _h_ive._imponi = _h_ive._imponi + ._Imponi
                                _h_ive._codcon = ._Conto
                                _h_hsive.CHGput(._Codiva, _h_ive)
                            End If
                            '
                            _PairFldVal.Clear()
                            _PairFldVal.Add("PNSERIAL", op.ValAdapter(_pnserial, TipoCampo.TChar))
                            _PairFldVal.Add("CPROWORD", op.ValAdapter(_cprownum * 10, TipoCampo.TInt))
                            _PairFldVal.Add("PNIMPAVE", op.ValAdapter(_impave, TipoCampo.TCur))
                            _PairFldVal.Add("PNCODCON", op.ValAdapter(._Conto, TipoCampo.TChar))
                            _PairFldVal.Add("PNTIPCON", op.ValAdapter(._tipcon, TipoCampo.TChar))
                            If ._tipcon = "C" Then
                                _PairFldVal.Add("PNFLPART", op.ValAdapter("C", TipoCampo.TChar))
                            Else
                                _PairFldVal.Add("PNFLPART", op.ValAdapter("N", TipoCampo.TChar))
                            End If
                            _PairFldVal.Add("PNIMPDAR", op.ValAdapter(_impdar, TipoCampo.TCur))
                            _PairFldVal.Add("CPROWNUM", op.ValAdapter(_cprownum, TipoCampo.TInt))
                            _PairFldVal.Add("PNFLZERO", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNDESRIG", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNCAURIG", op.ValAdapter(IIf(_flNC, Globale.CauNC, Globale.CauEmi), TipoCampo.TChar))
                            If ._tipcon = "C" Then
                                _PairFldVal.Add("PNCODPAG", op.ValAdapter(._Codpag, TipoCampo.TChar))
                            End If
                            _PairFldVal.Add("PNFLSALD", op.ValAdapter("+", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLSALI", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLSALF", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNLIBGIO", op.ValAdapter("0", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLABAN", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNIMPIND", op.ValAdapter("0", TipoCampo.TCur))
                            _PairFldVal.Add("PNFLVABD", op.ValAdapter(IIf(._tipcon = "C", "S", ""), TipoCampo.TChar))
                            _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                            '
                            Dim _strFld As String = ""
                            Dim _strVal As String = ""
                            Dim strsql As String = "insert into " & Me.GetTableNameLocal("PNT_DETT") & " ( "

                            For Each _element As String In _PairFldVal.Keys
                                _strFld = _strFld & _element & ","
                                _strVal = _strVal & _PairFldVal(_element) & ","
                            Next
                            strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                            '
                            op.esegui_query(strsql)
                            '
                            If ._tipcon = "C" Then
                                If Not Me.GeneraPartite(_pnt_dett, _pnserial, _pnProg, _cprownum, "EMISSIONE") Then
                                    Throw New Exception("Fallita generazione partite")
                                End If
                            End If
                            '
                            _cprownum = _cprownum + 1
                        End With
                        '
                    Next
                    '
                    If Not Me.FineReg(_pnserial, IIf(_flNC, _TotimpAver, _TotimpDare), _hsIve) Then
                        Throw New Exception("Fallita fine registrazione")
                    End If
                    '
                    op.CommitTrans()
                Catch ex As System.Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                    op.RollbackTrans()
                End Try
                sr.Close()

            Next
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import documenti")
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import documenti")
        End Try
    End Sub
    Private Sub ImportaIncassi()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim wFileName As String = "*" & Me.txtIncFile.Text.Trim
            'Lettura file
            '
            Dim di As New DirectoryInfo(Me.txtFolder.Text.TrimEnd)
            Dim fi As FileInfo
            For Each fi In di.GetFiles(wFileName)
                'Lettura file
                Dim sr As System.IO.FileStream
                sr = New System.IO.FileStream(Me.txtFolder.Text.TrimEnd("\") & "\" & fi.Name, IO.FileMode.Open, IO.FileAccess.Read)
                Dim filereader As New System.IO.StreamReader(sr)
                Dim _Index As Integer = 1
                While filereader.Peek > -1
                    rigafile = filereader.ReadLine
                    '
                    _prog = _Index.ToString.PadLeft(4, "0")
                    With _pnt_dett
                        ._Numdoc = rigafile.Substring(14, 6).Trim
                        ._alfdoc = rigafile.Substring(20, 2).Trim
                        ._DatDoc = rigafile.Substring(28, 2) & "-" & rigafile.Substring(26, 2) & "-" & rigafile.Substring(22, 4)
                        ._Codese = rigafile.Substring(22, 4)
                        ._DatReg = rigafile.Substring(36, 2) & "-" & rigafile.Substring(34, 2) & "-" & rigafile.Substring(30, 4)
                        ._tipcon = rigafile.Substring(38, 1)
                        ._Codcli = rigafile.Substring(39, 6).Trim
                        ._Segno = rigafile.Substring(51, 1)
                        ._Importo = CTran(rigafile.Substring(52, 13), 0) / 100
                        If Not String.IsNullOrEmpty(Globale.codPag) Then
                            ._Codpag = Globale.codPag.Trim
                        Else
                            ._Codpag = rigafile.Substring(65, 5).Trim
                        End If
                        ._Conto = rigafile.Substring(116, 13).Trim
                        ._Codiva = rigafile.Substring(70, 5).Trim
                        ._Imponi = CTran(rigafile.Substring(75, 13), 0) / 100
                        ._impIva = CTran(rigafile.Substring(88, 13), 0) / 100
                        _a_key = ._Numdoc & "#" & ._alfdoc & "#" & ._Codese & "#" & _prog
                    End With
                    Me._aKey_pn.Add(_a_key)
                    Me._h_hspn.CHGput(_a_key, _pnt_dett)
                    _Index = _Index + 1
                End While
                sr.Close()
                '
                'Controllo conti
                '
                Dim _flError As Boolean = False
                'elimina il file
                System.IO.File.Delete(Environment.CurrentDirectory & "\" & "errori.txt")
                '
                Dim sw As System.IO.FileStream
                sw = New System.IO.FileStream(Environment.CurrentDirectory & "\" & "errori.txt", IO.FileMode.Create, IO.FileAccess.Write)
                Dim filewriter As New System.IO.StreamWriter(sw)
                '
                For Each element As String In Me._aKey_pn
                    _pnt_dett = Me._h_hspn.CHGGet(element)
                    With _pnt_dett
                        Dim strsql As String = "select ancodice from " & Me.GetTableNameLocal("CONTI") & _
                                               " where antipcon = " & op.ValAdapter(._tipcon, TipoCampo.TChar) & _
                                               " and ancodice = " & op.ValAdapter(._Conto, TipoCampo.TChar)
                        Dim ds As DataSet = op.esegui_query(strsql)
                        If ds.Tables(0).Rows.Count = 0 Then
                            Dim _strError As String = "Conto < " & ._Conto & " > sconosciuto!"
                            filewriter.WriteLine(_strError)
                            _flError = True
                        End If
                    End With
                Next
                filewriter.Flush()
                filewriter.Close()
                sw.Close()
                filewriter.Dispose()
                If _flError Then
                    MsgBox("Riscontrati errori!", MsgBoxStyle.Critical)
                    Me.fileLaunch()
                    Exit Sub
                End If

                '
                'Scrittura documenti
                '
                Dim _hsIve As New Hashtable
                Dim _h_hsive As New CHashGest(_hsIve)
                '
                Dim _PairFldVal As New Hashtable
                Dim _t_numdoc As String = ""
                Dim _t_alfdoc As String = ""
                Dim _cprownum As Integer = 0
                Dim _impdar As Decimal = 0
                Dim _impave As Decimal = 0
                Dim _pnserial As String = ""
                Dim _Totimp As Decimal = 0
                Dim _pnProg As Integer = 0
                _aKey_pn.Sort()
                op.BeginTrans()
                Try
                    For Each element As String In Me._aKey_pn
                        '
                        _pnt_dett = Me._h_hspn.CHGGet(element)
                        If _t_numdoc = "" Or _t_numdoc <> _pnt_dett._Numdoc Or _t_alfdoc = "" Or _t_alfdoc <> _pnt_dett._alfdoc Then
                            'Scrittura della testata primanota
                            If _t_numdoc <> "" Then
                                If Not Me.FineReg(_pnserial, _Totimp, _hsIve) Then
                                    Throw New Exception("Fallita fine registrazione")
                                End If
                                _hsIve.Clear()
                            End If
                            _t_numdoc = _pnt_dett._Numdoc
                            _t_alfdoc = _pnt_dett._alfdoc
                            _pnserial = Me.pnSerial().ToString.PadLeft(10, "0")
                            _cprownum = 1
                            With _pnt_dett
                                _pnProg = Me.pnProg(._Codese)
                                Dim _Month As Integer = Convert.ToDateTime(._DatDoc).Month
                                _PairFldVal.Clear()
                                _PairFldVal.Add("PNSERIAL", op.ValAdapter(_pnserial, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODESE", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODUTE", "0")
                                _PairFldVal.Add("PNNUMRER", op.ValAdapter(_pnProg, TipoCampo.TInt))
                                _PairFldVal.Add("PNDATREG", op.ValAdapter(._DatReg, TipoCampo.TData))
                                _PairFldVal.Add("PNCODCAU", op.ValAdapter(Globale.CauInc, TipoCampo.TChar))
                                _PairFldVal.Add("PNCOMPET", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPREG", op.ValAdapter("N", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLIVDF", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLGDIF", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMREG", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPDOC", op.ValAdapter("NO", TipoCampo.TChar))
                                _PairFldVal.Add("PNPRG", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNPRP", op.ValAdapter("NN", TipoCampo.TChar))
                                _PairFldVal.Add("PNPRD", op.ValAdapter("NN", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMDOC", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNALFDOC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNALFPRO", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNDATDOC", op.ValAdapter("", TipoCampo.TData))
                                _PairFldVal.Add("PNANNPRO", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNANNDOC", op.ValAdapter(._Codese, TipoCampo.TChar))
                                _PairFldVal.Add("PNCODVAL", op.ValAdapter("EUR", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMPRO", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNTIPCLF", op.ValAdapter("N", TipoCampo.TChar))
                                _PairFldVal.Add("PNDESSUP", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNTOTDOC", op.ValAdapter("0", TipoCampo.TCur))
                                '_PairFldVal.Add("PNCODCLF", op.ValAdapter(._Conto, TipoCampo.TChar))
                                _PairFldVal.Add("UTCV", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("UTCC", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("UTDC", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                                _PairFldVal.Add("UTDV", op.ValAdapter("", TipoCampo.TData))
                                _PairFldVal.Add("PNCOMIVA", op.ValAdapter(._DatDoc, TipoCampo.TData))
                                _PairFldVal.Add("PNDATPLA", op.ValAdapter(._DatDoc, TipoCampo.TData))
                                _PairFldVal.Add("PNFLPROV", op.ValAdapter("N", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLREGI", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFINC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNCAOVAL", op.ValAdapter("1", TipoCampo.TCur))
                                _PairFldVal.Add("PNRIFDIS", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFDOC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFCES", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNVALNAZ", op.ValAdapter("EUR", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMTRA", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNNUMTR2", op.ValAdapter("0", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFACC", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNRIFSAL", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                                _PairFldVal.Add("PNSCRASS", op.ValAdapter("", TipoCampo.TChar))
                                _PairFldVal.Add("PNFLGSTO", op.ValAdapter("S", TipoCampo.TChar))
                                _PairFldVal.Add("PNTOTENA", op.ValAdapter("0", TipoCampo.TCur))
                                _PairFldVal.Add("PN__ANNO", op.ValAdapter(._Codese, TipoCampo.TCur))
                                _PairFldVal.Add("PN__MESE", op.ValAdapter(_Month, TipoCampo.TChar))

                                Dim _strFld As String = ""
                                Dim _strVal As String = ""
                                Dim strsql As String = "insert into " & Me.GetTableNameLocal("PNT_MAST") & " ( "

                                For Each _element As String In _PairFldVal.Keys
                                    _strFld = _strFld & _element & ","
                                    _strVal = _strVal & _PairFldVal(_element) & ","
                                Next
                                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                                '
                                op.esegui_query(strsql)
                                '
                            End With
                            '
                        End If
                        _PairFldVal.Clear()
                        'Fine scrittura testata primanota
                        '
                        'Scrittura dettaglio
                        '
                        With _pnt_dett
                            If ._Segno = "D" Then
                                _impave = 0
                                _impdar = ._Importo
                                _Totimp = _Totimp + ._Importo
                            Else
                                _impave = ._Importo
                                _impdar = 0
                            End If
                            '
                            If ._Codiva <> "00" Then
                                Dim _h_ive As New Ive
                                _h_ive = _h_hsive.CHGGet(._Codiva)
                                _h_ive._impIva = _h_ive._impIva + ._impIva
                                _h_ive._imponi = _h_ive._imponi + ._Imponi
                                _h_ive._codcon = ._Conto
                                _h_hsive.CHGput(._Codiva, _h_ive)
                            End If
                            '
                            _PairFldVal.Clear()
                            _PairFldVal.Add("PNSERIAL", op.ValAdapter(_pnserial, TipoCampo.TChar))
                            _PairFldVal.Add("CPROWORD", op.ValAdapter(_cprownum * 10, TipoCampo.TInt))
                            _PairFldVal.Add("PNIMPAVE", op.ValAdapter(_impave, TipoCampo.TCur))
                            _PairFldVal.Add("PNCODCON", op.ValAdapter(._Conto, TipoCampo.TChar))
                            _PairFldVal.Add("PNTIPCON", op.ValAdapter(._tipcon, TipoCampo.TChar))
                            If ._tipcon = "C" Then
                                _PairFldVal.Add("PNFLPART", op.ValAdapter("S", TipoCampo.TChar))
                            Else
                                _PairFldVal.Add("PNFLPART", op.ValAdapter("N", TipoCampo.TChar))
                            End If
                            _PairFldVal.Add("PNIMPDAR", op.ValAdapter(_impdar, TipoCampo.TCur))
                            _PairFldVal.Add("CPROWNUM", op.ValAdapter(_cprownum, TipoCampo.TInt))
                            _PairFldVal.Add("PNFLZERO", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNDESRIG", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNCAURIG", op.ValAdapter(Globale.CauInc, TipoCampo.TChar))
                            If ._tipcon = "C" Then
                                '_PairFldVal.Add("PNCODPAG", op.ValAdapter(._Codpag, TipoCampo.TChar))
                            End If
                            _PairFldVal.Add("PNFLSALD", op.ValAdapter("+", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLSALI", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLSALF", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNLIBGIO", op.ValAdapter("0", TipoCampo.TChar))
                            _PairFldVal.Add("PNFLABAN", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("PNIMPIND", op.ValAdapter("0", TipoCampo.TCur))
                            _PairFldVal.Add("PNFLVABD", op.ValAdapter("", TipoCampo.TChar))
                            _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                            '
                            Dim _strFld As String = ""
                            Dim _strVal As String = ""
                            Dim strsql As String = "insert into " & Me.GetTableNameLocal("PNT_DETT") & " ( "

                            For Each _element As String In _PairFldVal.Keys
                                _strFld = _strFld & _element & ","
                                _strVal = _strVal & _PairFldVal(_element) & ","
                            Next
                            strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                            '
                            op.esegui_query(strsql)
                            '
                            If ._tipcon = "C" Then
                                If Not Me.GeneraPartite(_pnt_dett, _pnserial, _pnProg, _cprownum, "INCASSO") Then
                                    Throw New Exception("Fallita generazione partite")
                                End If
                            End If
                            '
                            _cprownum = _cprownum + 1
                        End With
                        '
                    Next
                    '
                    op.CommitTrans()
                Catch ex As System.Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                    op.RollbackTrans()
                End Try
            Next
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import documenti")
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import documenti")
        End Try
    End Sub
    Private Function GeneraPartite(ByVal pPnt_dett As pnt_dett, ByVal pSerial As String, ByVal pNumrer As Integer, ByVal pCprownum As Integer, ByVal pTipo As String) As Boolean


        Try
            Dim _PairFldVal As New Hashtable
            Dim modpag As String = Me.LeggiPagamento(pPnt_dett._Codpag)
            With pPnt_dett
                _PairFldVal.Clear()
                _PairFldVal.Add("PTSERIAL", op.ValAdapter(pSerial, TipoCampo.TChar))
                _PairFldVal.Add("PTROWORD", op.ValAdapter(pCprownum, TipoCampo.TInt))
                _PairFldVal.Add("CPROWNUM", op.ValAdapter(1, TipoCampo.TInt))
                _PairFldVal.Add("PTNUMPAR", op.ValAdapter(._Codese.Trim & "/" & ._Numdoc.PadLeft(6, "0"), TipoCampo.TChar))
                _PairFldVal.Add("PTDATSCA", op.ValAdapter(calcScad(._DatDoc, ._Codpag), TipoCampo.TData))
                _PairFldVal.Add("PTTIPCON", op.ValAdapter(._tipcon, TipoCampo.TChar))
                _PairFldVal.Add("PTCODCON", op.ValAdapter(._Conto, TipoCampo.TChar))
                _PairFldVal.Add("PT_SEGNO", op.ValAdapter(._Segno, TipoCampo.TChar))
                _PairFldVal.Add("PTTOTIMP", op.ValAdapter(._Importo, TipoCampo.TCur))
                _PairFldVal.Add("PTCODVAL", op.ValAdapter("EUR", TipoCampo.TChar))
                _PairFldVal.Add("PTCAOVAL", op.ValAdapter(1, TipoCampo.TCur))
                _PairFldVal.Add("PTCAOAPE", op.ValAdapter(1, TipoCampo.TCur))
                _PairFldVal.Add("PTDATAPE", op.ValAdapter(._DatDoc, TipoCampo.TData))
                _PairFldVal.Add("PTNUMDOC", op.ValAdapter(._Numdoc, TipoCampo.TInt))
                _PairFldVal.Add("PTALFDOC", op.ValAdapter(._alfdoc, TipoCampo.TChar))
                _PairFldVal.Add("PTDATDOC", op.ValAdapter(._DatDoc, TipoCampo.TData))
                _PairFldVal.Add("PTIMPDOC", op.ValAdapter(._Importo, TipoCampo.TCur))
                _PairFldVal.Add("PTMODPAG", op.ValAdapter(modpag, TipoCampo.TChar)) ''''''?????????
                _PairFldVal.Add("PTFLSOSP", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTBANAPP", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTBANNOS", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTFLRAGG", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTFLRITE", op.ValAdapter("0", TipoCampo.TChar))
                _PairFldVal.Add("PTTOTABB", op.ValAdapter("0", TipoCampo.TCur))
                Select Case pTipo
                    Case "EMISSIONE"
                        _PairFldVal.Add("PTFLCRSA", op.ValAdapter("C", TipoCampo.TChar))
                    Case "INCASSO"
                        _PairFldVal.Add("PTFLCRSA", op.ValAdapter("S", TipoCampo.TChar))
                End Select
                _PairFldVal.Add("PTFLIMPE", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTNUMDIS", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTNUMEFF", op.ValAdapter("0", TipoCampo.TInt))
                _PairFldVal.Add("PTFLINDI", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTDESRIG", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTNUMPRO", op.ValAdapter("0", TipoCampo.TInt))
                '
                Select Case pTipo
                    Case "EMISSIONE"
                        _PairFldVal.Add("PTSERRIF", op.ValAdapter("", TipoCampo.TChar))
                        _PairFldVal.Add("PTORDRIF", op.ValAdapter("0", TipoCampo.TInt))
                        _PairFldVal.Add("PTNUMRIF", op.ValAdapter("0", TipoCampo.TInt))
                    Case "INCASSO"
                        Dim _rifpar As RifPar = Me.LeggiPartitaApe(._Codese.Trim & "/" & ._Numdoc.PadLeft(6, "0"), ._Conto)
                        _PairFldVal.Add("PTSERRIF", op.ValAdapter(_rifpar._pnserial, TipoCampo.TChar))
                        _PairFldVal.Add("PTORDRIF", op.ValAdapter(_rifpar._cproword, TipoCampo.TInt))
                        _PairFldVal.Add("PTNUMRIF", op.ValAdapter(_rifpar._cprownum, TipoCampo.TInt))
                End Select
                '
                _PairFldVal.Add("PTFLVABD", op.ValAdapter("S", TipoCampo.TChar))
                _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                _PairFldVal.Add("PTDATREG", op.ValAdapter(._DatReg, TipoCampo.TData))
                _PairFldVal.Add("PTNUMCOR", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("PTCODRAG", op.ValAdapter("", TipoCampo.TChar))
                '
                Dim _strFld As String = ""
                Dim _strVal As String = ""
                Dim strsql As String = "insert into " & Me.GetTableNameLocal("PAR_TITE") & " ( "

                For Each _element As String In _PairFldVal.Keys
                    _strFld = _strFld & _element & ","
                    _strVal = _strVal & _PairFldVal(_element) & ","
                Next
                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                '
                op.esegui_query(strsql)
                '
            End With
            Return True
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Scrivi partite")
            Return False
        End Try

    End Function
    Private Function calcScad(ByVal pdata As String, ByVal pcodpag As String) As String
        Try
            Dim wDatsca As Date = pdata.Trim
            Dim wGiorni As Integer = 0
            Dim wTipsca As String = ""

            Dim wStrsql As String = "select P2GIOSCA, P2SCADEN from " & Me.GetTableNameLocal("PAG_2AME") & " where P2CODICE = " & op.ValAdapter(pcodpag, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(wStrsql)
            If ds.Tables(0).Rows.Count > 0 Then
                wGiorni = ds.Tables(0).Rows(0).Item("P2GIOSCA")
                wTipsca = ds.Tables(0).Rows(0).Item("P2SCADEN")
            End If
            '
            wDatsca = wDatsca.AddDays(wGiorni)
            If wTipsca.Trim = "FM" Then
                Dim wLastDay As Integer = Date.DaysInMonth(wDatsca.Year, wDatsca.Month)
                Dim wDayexp As Integer = wDatsca.Day
                If wDayexp > wLastDay Then
                    wDayexp = 0
                End If
                wDatsca = wDatsca.AddDays(wLastDay - wDayexp) ' Aggiungo il numero di giorni che manca a fine mese
            End If
            '
            Return wDatsca
            '
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "calcScad")
        End Try
    End Function
    Private Function LeggiPartitaApe(ByVal pnumpar As String, ByVal pConto As String) As RifPar

        Dim _rifpar As RifPar
        _rifpar._pnserial = ""
        _rifpar._cproword = 0
        _rifpar._cprownum = 0
        '
        Try
            Dim wStrsql = "select ptserial, ptroword, cprownum from " & Me.GetTableNameLocal("PAR_TITE") & " where ptnumpar = " & op.ValAdapter(pnumpar, TipoCampo.TChar) & _
                          " and ptcodcon = " & op.ValAdapter(pConto, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(wStrsql)
            If ds.Tables(0).Rows.Count > 0 Then
                _rifpar._pnserial = ds.Tables(0).Rows(0).Item("PTSERIAL").ToString.Trim
                _rifpar._cproword = ds.Tables(0).Rows(0).Item("PTROWORD").ToString.Trim
                _rifpar._cprownum = ds.Tables(0).Rows(0).Item("CPROWNUM").ToString.Trim
            End If
            Return _rifpar
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi partita aperta")
            Return _rifpar
        End Try

    End Function
    Private Function LeggiPagamento(ByVal pCodpag As String) As String

        Try
            Dim wStrsql = "select distinct(P2MODPAG) AS MODPAG from " & Me.GetTableNameLocal("PAG_2AME") & " where P2CODICE = " & op.ValAdapter(pCodpag, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(wStrsql)
            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0).Item("MODPAG").ToString.Trim
            Else
                Return ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Leggi pagamento")
            Return ""
        End Try

    End Function
    Private Function FineReg(ByVal pPnserial As String, ByVal pTotdoc As Decimal, ByVal pHashIve As Hashtable)

        Try
            Dim _h_ive As New Ive
            Dim _PairFldVal As New Hashtable
            Dim _cprownum As Integer = 1
            'Registra importo totale testata
            Dim strupd As String = "update " & Me.GetTableNameLocal("PNT_MAST") & " set pntotdoc = " & op.ValAdapter(pTotdoc, TipoCampo.TCur) & _
                                   " where pnserial = " & op.ValAdapter(pPnserial, TipoCampo.TChar)
            op.esegui_query(strupd)
            '
            'Registra riepilogo ive
            For Each element As String In pHashIve.Keys
                '
                _h_ive = pHashIve(element)
                '
                _PairFldVal.Add("IVSERIAL", op.ValAdapter(pPnserial, TipoCampo.TChar))
                _PairFldVal.Add("CPROWNUM", op.ValAdapter(_cprownum * 10, TipoCampo.TInt))
                _PairFldVal.Add("IVCODIVA", op.ValAdapter(element, TipoCampo.TChar))
                _PairFldVal.Add("IVTIPCON", op.ValAdapter("G", TipoCampo.TChar))
                _PairFldVal.Add("IVCODCON", op.ValAdapter(_h_ive._codcon, TipoCampo.TChar))
                _PairFldVal.Add("IVCODCOI", op.ValAdapter("", TipoCampo.TChar))
                _PairFldVal.Add("IVPERIND", op.ValAdapter("0", TipoCampo.TInt))
                _PairFldVal.Add("IVTIPREG", op.ValAdapter("V", TipoCampo.TChar))
                _PairFldVal.Add("IVNUMREG", op.ValAdapter("1", TipoCampo.TInt))
                _PairFldVal.Add("IVIMPONI", op.ValAdapter(_h_ive._imponi, TipoCampo.TCur))
                _PairFldVal.Add("IVIMPIVA", op.ValAdapter(_h_ive._impIva, TipoCampo.TCur))
                _PairFldVal.Add("IVFLOMAG", op.ValAdapter("X", TipoCampo.TChar))
                _PairFldVal.Add("IVCFLOMA", op.ValAdapter("X", TipoCampo.TChar))
                _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                _PairFldVal.Add("IVTIPCOP", op.ValAdapter("G", TipoCampo.TChar))
                '
                Dim _strFld As String = ""
                Dim _strVal As String = ""
                Dim strsql As String = "insert into " & Me.GetTableNameLocal("PNT_IVA") & " ( "
                '
                For Each _element As String In _PairFldVal.Keys
                    _strFld = _strFld & _element & ","
                    _strVal = _strVal & _PairFldVal(_element) & ","
                Next
                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                '
                op.esegui_query(strsql)
                _PairFldVal.Clear()
                '
                _cprownum = _cprownum + 1
            Next
            '
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Fine reg")
            Return False
        End Try

    End Function
    Private Function GetTableNameLocal(ByVal pTablename) As String
        Return Globale.CodAzi.Trim & pTablename
    End Function

    Private Sub btnConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfig.Click
        Config.ShowDialog()
        '
        Me.txtEmifile.Text = Globale.FileEmiFat
        Me.txtIncFile.Text = Globale.FileIncFat
        Me.txtCliFile.Text = Globale.FileCli
        Me.txtFolder.Text = Globale.PathFile
        Me.txtCodAzi.Text = Globale.CodAzi
        '
    End Sub
    Private Sub fileLaunch()
        Dim pInf As New System.Diagnostics.ProcessStartInfo
        pInf.FileName = "notepad.exe"
        pInf.Arguments = Environment.CurrentDirectory & "\" & "errori.txt"
        Process.Start(pInf)
    End Sub
    Private Function pnSerial() As Long
        '
        Dim _Autonum As Long
        Dim ds As DataSet
        Dim _key As String = "prog\SEPNT\'" & Globale.CodAzi.Trim & "'"
        Dim strsql As String = "select autonum from cpwarn where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        If ds.Tables(0).Rows.Count > 0 Then
            _Autonum = ds.Tables(0).Rows(0).Item("autonum") + 1
        End If
        strsql = "update cpwarn set autonum = " & op.ValAdapter(_Autonum, TipoCampo.TLong) & " where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        Return _Autonum
    End Function
    Private Function pnProg(ByVal pCodese As String) As Integer
        '
        Dim _Prog As Integer
        Dim ds As DataSet
        Dim _key As String = "prog\PRPNT\'" & Globale.CodAzi.Trim & "'\'" & pCodese & "'\0\'        ' "
        Dim strsql As String = "select autonum from cpwarn where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        If ds.Tables(0).Rows.Count > 0 Then
            _Prog = ds.Tables(0).Rows(0).Item("autonum") + 1
        End If
        strsql = "update cpwarn set autonum = " & op.ValAdapter(_Prog, TipoCampo.TLong) & " where tablecode = " & op.ValAdapter(_key, TipoCampo.TChar)
        ds = op.esegui_query(strsql)
        Return _Prog
    End Function

End Class