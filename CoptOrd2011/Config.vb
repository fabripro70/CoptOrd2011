Public Class Config
    Dim cf As New CConfig
    Private Sub btnDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.FoldDialog.ShowDialog()
    End Sub

    Private Sub Config_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.txtCauEmi.Text = Globale.CauEmi
        Me.txtCauinc.Text = Globale.CauInc
        Me.txtFolder.Text = Globale.PathFile
        Me.txtEmifile.Text = Globale.FileEmiFat
        Me.txtIncFile.Text = Globale.FileIncFat
        Me.txtCliFile.Text = Globale.FileCli
        Me.txtCodAzi.Text = Globale.CodAzi
        Me.txtMastroCli.Text = Globale.MastroCli
        Me.txtNotaAcc.Text = Globale.CauNC
        Me.txtCodpag.Text = Globale.codPag
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Globale.CauEmi = Me.txtCauEmi.Text
        Globale.CauInc = Me.txtCauinc.Text
        Globale.PathFile = Me.txtFolder.Text
        Globale.FileEmiFat = Me.txtEmifile.Text
        Globale.FileIncFat = Me.txtIncFile.Text
        Globale.FileCli = Me.txtCliFile.Text
        Globale.CodAzi = Me.txtCodAzi.Text
        Globale.MastroCli = Me.txtMastroCli.Text
        Globale.CauNC = Me.txtNotaAcc.Text
        Globale.codPag = Me.txtCodpag.Text
        cf.ScriviConfig()
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click

    End Sub

    Private Sub btnFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFolder.Click
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

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbCauinc.Click

    End Sub
End Class