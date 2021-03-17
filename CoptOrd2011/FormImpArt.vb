Imports System
Imports System.io
Imports System.Net
Imports System.Net.Sockets
Imports System.Globalization
Imports System.Text
Imports Microsoft.VisualBasic
Imports iTextSharp
Imports iTextSharp.text.pdf


Public Class formImpArt

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

        If String.IsNullOrEmpty(Globale.CodAzi) Then
            Config.ShowDialog()
            Exit Sub
        End If
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
        Dim savePath As String = System.IO.Directory.GetCurrentDirectory
        With OFD1
            .Filter = "All files|*.*"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                For Each oFile As String In .FileNames
                    txtFolder.Text = oFile.Trim
                    '
                Next
            End If
        End With
        'Me.btnSave_Click(Nothing, Nothing)
        System.IO.Directory.SetCurrentDirectory(savePath)
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
        Select Case cmbTipo.Text.Trim
            Case "ANAGRAFICA"
                Me.ImportaArticoli()
            Case "CHIAVI"
                Me.ImportaKeyArti()
            Case "LISTINI"
                Me.ImportaListini()
        End Select
        MsgBox("Importazione terminata", MsgBoxStyle.Exclamation)
        'Me.ImportaIncassi()
    End Sub
    Private Sub ImportaArticoli()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim _flNC As Boolean = False
            Dim wFileName As String = "*" & Me.txtFolder.Text.Trim
            Dim _PairFldVal As New Hashtable
            Dim _hCampi As New Hashtable

            'Lettura file

            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(Me.txtFolder.Text.Trim, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            Dim _Index As Integer = 1
            Dim _campi
            op.BeginTrans()
            While filereader.Peek > -1
                rigafile = filereader.ReadLine
                _campi = rigafile.Split(";")
                '
                Me.txtStatus.Text = "Articolo : " & _campi(0)
                Me.txtStatus.Refresh()
                '
                Dim _Codart As String = _campi(0).ToString.Trim
                Dim _Desart As String = _campi(1).ToString.Trim
                Dim _Dessup As String = ""
                Dim _Unimi1 As String = _campi(2).ToString.Trim
                Dim _Unimi2 As String = _campi(3).ToString.Trim
                Dim _Operat As String = _campi(4).ToString.Trim
                Dim _Moltip As String = _campi(5).ToString.Trim
                Dim _Codiva As String = _campi(6).ToString.Trim
                Dim _Catcon As String = _campi(7).ToString.Trim

                'ART_ICOL
                _PairFldVal.Clear()
                _PairFldVal.Add("ARCODART", op.ValAdapter(_Codart, TipoCampo.TChar))
                _PairFldVal.Add("ARDESART", op.ValAdapter(_Desart, TipoCampo.TChar))
                If Not _Dessup = "NULL" Then
                    _PairFldVal.Add("ARDESSUP", op.ValAdapter(_Dessup, TipoCampo.TChar))
                End If
                If Not _Unimi1 = "NULL" Then
                    _PairFldVal.Add("ARUNMIS1", op.ValAdapter(_Unimi1, TipoCampo.TChar))
                End If
                If Not _Unimi2 = "NULL" Then
                    _PairFldVal.Add("ARUNMIS2", op.ValAdapter(_Unimi2, TipoCampo.TData))
                End If
                If Not _Operat = "NULL" Then
                    _PairFldVal.Add("AROPERAT", op.ValAdapter(_Operat, TipoCampo.TChar))
                End If
                If Not _Moltip = "NULL" Then
                    _PairFldVal.Add("ARMOLTIP", op.ValAdapter(_Moltip, TipoCampo.TChar))
                End If
                If Not _Codiva = "NULL" Then
                    _PairFldVal.Add("ARCODIVA", op.ValAdapter(_Codiva, TipoCampo.TChar))
                End If
                _PairFldVal.Add("ARFLINVE", op.ValAdapter("S", TipoCampo.TChar))
                _PairFldVal.Add("ARFLESAU", op.ValAdapter("N", TipoCampo.TChar))
                If Not _Catcon = "NULL" Then
                    _PairFldVal.Add("ARCATCON", op.ValAdapter(_Catcon, TipoCampo.TChar))
                End If
                _PairFldVal.Add("ARTIPART", op.ValAdapter("PF", TipoCampo.TChar))
                _PairFldVal.Add("ARPESNET", op.ValAdapter(0, TipoCampo.TCur))
                _PairFldVal.Add("ARPESNE2", op.ValAdapter(0, TipoCampo.TCur))
                _PairFldVal.Add("ARPESLOR", op.ValAdapter(0, TipoCampo.TCur))
                _PairFldVal.Add("ARPESLO2", op.ValAdapter(0, TipoCampo.TCur))
                _PairFldVal.Add("UTCC", op.ValAdapter("1", TipoCampo.TChar))
                _PairFldVal.Add("UTCV", op.ValAdapter("1", TipoCampo.TChar))
                _PairFldVal.Add("UTDC", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                _PairFldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                _PairFldVal.Add("ARFLDISP", op.ValAdapter("N", TipoCampo.TChar))
                _PairFldVal.Add("ARFLLOTT", op.ValAdapter("N", TipoCampo.TChar))
                _PairFldVal.Add("ARTIPGES", op.ValAdapter("E", TipoCampo.TChar))
                _PairFldVal.Add("ARPROPRE", op.ValAdapter("E", TipoCampo.TChar))
                _PairFldVal.Add("ARFLPECO", op.ValAdapter("C", TipoCampo.TChar))
                _PairFldVal.Add("ARFLDISC", op.ValAdapter("N", TipoCampo.TChar))
                _PairFldVal.Add("ARCATOPE", op.ValAdapter("AR", TipoCampo.TChar))
                '
                Dim _strFld As String = ""
                Dim _strVal As String = ""
                Dim strsql As String = "insert into " & Me.GetTableNameLocal("ART_ICOL") & " ( "

                For Each _element As String In _PairFldVal.Keys
                    _strFld = _strFld & _element & ","
                    _strVal = _strVal & _PairFldVal(_element) & ","
                Next
                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                '
                Try
                    op.esegui_query(strsql)
                Catch ex As System.Exception
                    MsgBox(ex.Message & " " & _Codart)
                End Try
                '
            End While
            op.CommitTrans()
            sr.Close()
            '
            Me.txtStatus.Text = "Finito!"
            Me.txtStatus.Refresh()
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import articoli")
            op.RollbackTrans()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import articoli")
            op.RollbackTrans()
        End Try
    End Sub
    Private Sub ImportaKeyArti()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim _flNC As Boolean = False
            Dim wFileName As String = "*" & Me.txtFolder.Text.Trim
            Dim _PairFldVal As New Hashtable
            Dim _hCampi As New Hashtable

            'Lettura file

            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(Me.txtFolder.Text.Trim, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            Dim _Index As Integer = 1
            Dim _campi
            op.BeginTrans()
            While filereader.Peek > -1
                rigafile = filereader.ReadLine
                _campi = rigafile.Split(";")
                '
                'KEY_ARTI
                '
                Me.txtStatus.Text = "Chiave : " & _campi(0)
                Me.txtStatus.Refresh()
                _PairFldVal.Clear()
                If Not _campi(0) = "NULL" Then
                    _PairFldVal.Add("CACODICE", op.ValAdapter(_campi(0), TipoCampo.TChar))
                End If
                If Not _campi(1) = "NULL" Then
                    _PairFldVal.Add("CADESART", op.ValAdapter(_campi(1), TipoCampo.TChar))
                End If
                If Not _campi(2) = "NULL" Then
                    _PairFldVal.Add("CADESSUP", op.ValAdapter(_campi(2), TipoCampo.TChar))
                End If
                If Not _campi(3) = "NULL" Then
                    _PairFldVal.Add("CACODART", op.ValAdapter(_campi(3), TipoCampo.TChar))
                End If
                If Not _campi(4) = "NULL" Then
                    _PairFldVal.Add("CATIPCON", op.ValAdapter(_campi(4), TipoCampo.TChar))
                End If
                If Not _campi(5) = "NULL" Then
                    _PairFldVal.Add("CACODCON", op.ValAdapter(_campi(5), TipoCampo.TChar))
                End If
                If Not _campi(6) = "NULL" Then
                    _PairFldVal.Add("CA__TIPO", op.ValAdapter(_campi(6), TipoCampo.TChar))
                End If
                If Not _campi(7) = "NULL" Then
                    _PairFldVal.Add("CATIPBAR", op.ValAdapter(_campi(7), TipoCampo.TChar))
                End If
                If Not _campi(8) = "NULL" Then
                    _PairFldVal.Add("CAFLSTAM", op.ValAdapter(_campi(8), TipoCampo.TChar))
                End If
                If Not _campi(9) = "NULL" Then
                    _PairFldVal.Add("CAUNIMIS", op.ValAdapter(_campi(9), TipoCampo.TChar))
                End If
                If Not _campi(10) = "NULL" Then
                    _PairFldVal.Add("CAOPERAT", op.ValAdapter(_campi(10), TipoCampo.TChar))
                End If
                If Not _campi(11) = "NULL" Then
                    _PairFldVal.Add("CAMOLTIP", op.ValAdapter(_campi(11), TipoCampo.TChar))
                End If
                If Not _campi(12) = "NULL" Then
                    _PairFldVal.Add("CADTINVA", op.ValAdapter(revDat(_campi(12)), TipoCampo.TData))
                End If
                If Not _campi(13) = "NULL" Then
                    _PairFldVal.Add("CADTOBSO", op.ValAdapter(revDat(_campi(13)), TipoCampo.TData))
                End If
                If Not _campi(14) = "NULL" Then
                    _PairFldVal.Add("UTCC", op.ValAdapter(_campi(14), TipoCampo.TInt))
                End If
                If Not _campi(15) = "NULL" Then
                    _PairFldVal.Add("UTCV", op.ValAdapter(_campi(15), TipoCampo.TInt))
                End If
                If Not _campi(16) = "NULL" Then
                    _PairFldVal.Add("UTDC", op.ValAdapter(revDat(_campi(16)), TipoCampo.TData))
                End If
                If Not _campi(17) = "NULL" Then
                    _PairFldVal.Add("UTDV", op.ValAdapter(revDat(_campi(17)), TipoCampo.TData))
                End If
                If Not _campi(18) = "NULL" Then
                    _PairFldVal.Add("CATIPMA3", op.ValAdapter(_campi(18), TipoCampo.TChar))
                End If
                If Not _campi(19) = "NULL" Then
                    _PairFldVal.Add("CAPESNE3", op.ValAdapter(_campi(19), TipoCampo.TCur))
                End If
                If Not _campi(20) = "NULL" Then
                    _PairFldVal.Add("CAPESLO3", op.ValAdapter(_campi(20), TipoCampo.TCur))
                End If
                If Not _campi(21) = "NULL" Then
                    _PairFldVal.Add("CADESVO3", op.ValAdapter(_campi(21), TipoCampo.TChar))
                End If
                If Not _campi(22) = "NULL" Then
                    _PairFldVal.Add("CAUMVOL3", op.ValAdapter(_campi(22), TipoCampo.TChar))
                End If
                If Not _campi(23) = "NULL" Then
                    _PairFldVal.Add("CATPCON3", op.ValAdapter(_campi(23), TipoCampo.TChar))
                End If
                If Not _campi(24) = "NULL" Then
                    _PairFldVal.Add("CAPZCON3", op.ValAdapter(_campi(24), TipoCampo.TCur))
                End If
                If Not _campi(25) = "NULL" Then
                    _PairFldVal.Add("CACOCOL3", op.ValAdapter(_campi(25), TipoCampo.TInt))
                End If
                If Not _campi(26) = "NULL" Then
                    _PairFldVal.Add("CADIMLU3", op.ValAdapter(_campi(26), TipoCampo.TCur))
                End If
                If Not _campi(27) = "NULL" Then
                    _PairFldVal.Add("CADIMLA3", op.ValAdapter(_campi(27), TipoCampo.TCur))
                End If
                If Not _campi(28) = "NULL" Then
                    _PairFldVal.Add("CADIMAL3", op.ValAdapter(_campi(28), TipoCampo.TCur))
                End If
                If Not _campi(29) = "NULL" Then
                    _PairFldVal.Add("CAUMDIM3", op.ValAdapter(_campi(29), TipoCampo.TChar))
                End If
                If Not _campi(30) = "NULL" Then
                    _PairFldVal.Add("CAFLCON3", op.ValAdapter(_campi(30), TipoCampo.TChar))
                End If
                If Not _campi(31) = "NULL" Then
                    _PairFldVal.Add("CATIPCO3", op.ValAdapter(_campi(31), TipoCampo.TChar))
                End If
                If Not _campi(32) = "NULL" Then
                    _PairFldVal.Add("CACODVAR", op.ValAdapter(_campi(32), TipoCampo.TChar))
                End If
                If Not _campi(33) = "NULL" Then
                    _PairFldVal.Add("CALENSCF", op.ValAdapter(_campi(33), TipoCampo.TInt))
                End If
                If Not _campi(34) = "NULL" Then
                    _PairFldVal.Add("CAFLIMBA", op.ValAdapter(_campi(34), TipoCampo.TChar))
                End If
                If Not _campi(35) = "NULL" Then
                    _PairFldVal.Add("CAPUBWEB", op.ValAdapter(_campi(35), TipoCampo.TChar))
                End If
                If Not _campi(36) = "NULL" Then
                    _PairFldVal.Add("cpccchk", op.ValAdapter(_campi(36), TipoCampo.TChar))
                End If
                '
                Dim _strFld As String = ""
                Dim _strVal As String = ""
                Dim strsql As String = "insert into " & Me.GetTableNameLocal("KEY_ARTI") & " ( "

                For Each _element As String In _PairFldVal.Keys
                    _strFld = _strFld & _element & ","
                    _strVal = _strVal & _PairFldVal(_element) & ","
                Next
                strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                '
                Try
                    op.esegui_query(strsql)
                Catch ex As System.Exception
                    MsgBox(ex.Message & " " & _campi(0))
                End Try
                '
            End While
            op.CommitTrans()
            sr.Close()
            Me.txtStatus.Text = "Finito!"
            Me.txtStatus.Refresh()
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import chiavi")
            op.RollbackTrans()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import chiavi")
            op.RollbackTrans()
        End Try
    End Sub

    Private Sub ImportaListini()

        Try
            Dim _pnt_dett As New pnt_dett
            Dim _a_key As String = ""
            Dim _prog As String = ""
            Dim rigafile As String = ""
            Dim _flNC As Boolean = False
            Dim wFileName As String = "*" & Me.txtFolder.Text.Trim
            Dim _PairFldVal As New Hashtable

            'Lettura file

            Dim sr As System.IO.FileStream
            sr = New System.IO.FileStream(Me.txtFolder.Text.Trim, IO.FileMode.Open, IO.FileAccess.Read)
            Dim filereader As New System.IO.StreamReader(sr)
            Dim _Index As Integer = 1
            Dim _campi
            Dim _h_cprownum As New Hashtable
            Dim __key_lis_tini As String = ""
            Dim __key_lis_scag As String = ""
            Dim __key_listini As String = ""
            Dim _key_lis_tini As String = ""
            Dim _key_lis_scag As String = ""
            Dim _key_listini As String = ""
            op.BeginTrans()
            While filereader.Peek > -1
                rigafile = filereader.ReadLine
                _campi = rigafile.Split(";")
                '
                Dim _Codlis As String = _campi(0).trim
                Dim _Deslis As String = _campi(1).trim
                Dim _ivalis As String = _campi(2).trim
                Dim _vallis As String = _campi(3).trim
                Dim _datatt As String = _campi(4)
                Dim _datdis As String = _campi(5)
                Dim _unimis As String = _campi(6)
                Dim _codart As String = _campi(7).trim
                Dim _rownum As String = _campi(8)
                Dim _quanti As String = _campi(9)
                Dim _prezzo As Decimal = _campi(10).ToString.Replace(".", ",")
                Dim _scont1 As Decimal = _campi(11).ToString.Replace(".", ",")
                Dim _scont2 As Decimal = _campi(12).ToString.Replace(".", ",")
                Dim _scont3 As Decimal = _campi(13).ToString.Replace(".", ",")
                Dim _scont4 As Decimal = _campi(14).ToString.Replace(".", ",")
                'Rileva cprownum
                If Not _h_cprownum(_codart) Is Nothing Then
                    _rownum = _h_cprownum(_codart)
                Else
                    _rownum = Me.pnRownum(_codart)
                    _h_cprownum.Add(_codart, _rownum)
                End If
                '
                _key_listini = _Codlis
                _key_lis_tini = _Codlis.Trim & _codart.Trim & _rownum.ToString.Trim
                _key_lis_scag = _codart.Trim & _rownum.ToString.Trim & _quanti.ToString.Trim
                '
                'LISTINI
                '
                If _key_listini <> __key_listini Then
                    '
                    __key_listini = _Codlis
                    '
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LSCODLIS", op.ValAdapter(_Codlis, TipoCampo.TChar))
                    _PairFldVal.Add("LSDESLIS", op.ValAdapter(_Deslis, TipoCampo.TChar))
                    _PairFldVal.Add("LSIVALIS", op.ValAdapter(_ivalis, TipoCampo.TChar))
                    _PairFldVal.Add("LSVALLIS", op.ValAdapter(_vallis, TipoCampo.TChar))
                    _PairFldVal.Add("LSQUANTI", op.ValAdapter("S", TipoCampo.TData))
                    _PairFldVal.Add("UTDC", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    _PairFldVal.Add("UTDV", op.ValAdapter(Globale.g_SysData, TipoCampo.TData))
                    _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LISTINI") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    op.esegui_query(strsql)
                End If
                '
                'LIS_TINI
                '
                If _key_lis_tini <> __key_lis_tini Then
                    '
                    _key_lis_tini = _Codlis.Trim & _codart.Trim & _rownum.ToString.Trim
                    '
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LICODART", op.ValAdapter(_codart, TipoCampo.TChar))
                    _PairFldVal.Add("CPROWNUM", op.ValAdapter(_rownum, TipoCampo.TChar))
                    _PairFldVal.Add("LICODLIS", op.ValAdapter(_Codlis, TipoCampo.TChar))
                    _PairFldVal.Add("LIDATATT", op.ValAdapter(Me.revDat(_datatt), TipoCampo.TData))
                    _PairFldVal.Add("LIDATDIS", op.ValAdapter(Me.revDat(_datdis), TipoCampo.TData))
                    _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LIS_TINI") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    Try
                        op.esegui_query(strsql)
                    Catch ex As System.Exception
                        MsgBox(ex.Message & " " & _codart & " " & _rownum, MsgBoxStyle.Information, "LIS_TINI")
                    End Try
                End If
                '
                'LIS_SCAG
                '
                If _key_lis_scag <> __key_lis_scag Then
                    '
                    _key_lis_scag = _codart.Trim & _rownum.ToString.Trim & _quanti.ToString.Trim
                    '
                    Me.txtStatus.Text = "Listini : " & _codart
                    Me.txtStatus.Refresh()
                    _PairFldVal.Clear()
                    _PairFldVal.Add("LICODART", op.ValAdapter(_codart, TipoCampo.TChar))
                    _PairFldVal.Add("LIROWNUM", op.ValAdapter(_rownum, TipoCampo.TChar))
                    _PairFldVal.Add("LIQUANTI", op.ValAdapter(_quanti, TipoCampo.TChar))
                    _PairFldVal.Add("LIPREZZO", op.ValAdapter(_prezzo, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT1", op.ValAdapter(_scont1, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT2", op.ValAdapter(_scont2, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT3", op.ValAdapter(_scont3, TipoCampo.TCur))
                    _PairFldVal.Add("LISCONT4", op.ValAdapter(_scont4, TipoCampo.TCur))
                    _PairFldVal.Add("CPCCCHK", op.ValAdapter("abcdefghil", TipoCampo.TChar))
                    '
                    Dim _strFld As String = ""
                    Dim _strVal As String = ""
                    Dim strsql As String = "insert into " & Me.GetTableNameLocal("LIS_SCAG") & " ( "
                    '
                    For Each _element As String In _PairFldVal.Keys
                        _strFld = _strFld & _element & ","
                        _strVal = _strVal & _PairFldVal(_element) & ","
                    Next
                    strsql = strsql & _strFld.TrimEnd(",") & " ) VALUES ( " & _strVal.TrimEnd(",") & " )"
                    '
                    Try
                        op.esegui_query(strsql)
                    Catch ex As System.Exception
                        MsgBox(ex.Message & " " & _codart & " " & _rownum, MsgBoxStyle.Information, "LIS_SCAG")
                    End Try
                End If

            End While
            op.CommitTrans()
            sr.Close()
            Me.txtStatus.Text = "Finito!"
            Me.txtStatus.Refresh()
            '
        Catch ex As IO.IOException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import listini")
            op.RollbackTrans()
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Import listini")
            op.RollbackTrans()
        End Try
    End Sub
    Private Function GetTableNameLocal(ByVal pTablename) As String
        Return Globale.CodAzi.Trim & pTablename
    End Function

    Private Sub btnConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfig.Click
        Config.ShowDialog()
        '
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
    Private Function pnRownum(ByVal pCodart As String) As Integer


        Try
            Dim _rownum As Integer = 0
            Dim strsql As String = "select max(cprownum) from " & Me.GetTableNameLocal("LIS_TINI") & _
                           " where licodart = " & op.ValAdapter(pCodart.Trim, TipoCampo.TChar)
            Dim ds As DataSet = op.esegui_query(strsql)
            If ds.Tables(0).Rows.Count = 0 Then
                _rownum = 1
            Else
                _rownum = CTran(ds.Tables(0).Rows(0).Item(0), 0) + 1
            End If
            Return _rownum
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Numera righe")
        End Try

    End Function
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

    Private Function revDat(ByVal pData As String) As String

        Dim tokField() As String
        Dim tokData() As String
        Dim sReturn As String
        Dim pSeparator As String
        '
        If pData.Contains("-") Then
            pSeparator = "-"
        End If
        If pData.Contains("/") Then
            pSeparator = "/"
        End If
        '
        Try
            tokField = pData.Split(" ") 'Separa la data dall' ora
            tokData = tokField(0).Split(pSeparator) 'separa i pezzi della data
            sReturn = tokData(2) & pSeparator & tokData(1) & pSeparator & tokData(0)
            If tokField.Length > 1 Then
                Return sReturn & " " & tokField(1).Trim
            Else
                Return sReturn
            End If

        Catch ex As Exception

        End Try

    End Function


End Class