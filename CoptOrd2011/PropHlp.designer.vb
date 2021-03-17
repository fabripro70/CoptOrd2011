<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropHlp
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim LinkDati2 As ClSLib.LinkDati = New ClSLib.LinkDati
        Me.txtTesto = New System.Windows.Forms.TextBox
        Me.txtLunghezza = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkVisibile = New System.Windows.Forms.CheckBox
        Me.lstCampi = New System.Windows.Forms.ListBox
        Me.btnSalva = New System.Windows.Forms.Button
        Me.btnEsci = New System.Windows.Forms.Button
        Me.txtTitolo = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtQuery = New ClSLib.CTextBoxEx
        Me.btnGeneara = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtTesto
        '
        Me.txtTesto.Location = New System.Drawing.Point(210, 46)
        Me.txtTesto.MaxLength = 25
        Me.txtTesto.Name = "txtTesto"
        Me.txtTesto.Size = New System.Drawing.Size(190, 20)
        Me.txtTesto.TabIndex = 1
        '
        'txtLunghezza
        '
        Me.txtLunghezza.Location = New System.Drawing.Point(211, 85)
        Me.txtLunghezza.MaxLength = 4
        Me.txtLunghezza.Name = "txtLunghezza"
        Me.txtLunghezza.Size = New System.Drawing.Size(39, 20)
        Me.txtLunghezza.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(208, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Testo"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(208, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Lungh."
        '
        'chkVisibile
        '
        Me.chkVisibile.AutoSize = True
        Me.chkVisibile.Location = New System.Drawing.Point(211, 111)
        Me.chkVisibile.Name = "chkVisibile"
        Me.chkVisibile.Size = New System.Drawing.Size(58, 17)
        Me.chkVisibile.TabIndex = 4
        Me.chkVisibile.Text = "Visibile"
        Me.chkVisibile.UseVisualStyleBackColor = True
        '
        'lstCampi
        '
        Me.lstCampi.FormattingEnabled = True
        Me.lstCampi.Location = New System.Drawing.Point(6, 29)
        Me.lstCampi.Name = "lstCampi"
        Me.lstCampi.Size = New System.Drawing.Size(183, 147)
        Me.lstCampi.TabIndex = 7
        Me.lstCampi.TabStop = False
        '
        'btnSalva
        '
        Me.btnSalva.Location = New System.Drawing.Point(281, 164)
        Me.btnSalva.Name = "btnSalva"
        Me.btnSalva.Size = New System.Drawing.Size(55, 21)
        Me.btnSalva.TabIndex = 8
        Me.btnSalva.TabStop = False
        Me.btnSalva.Text = "Salva"
        Me.btnSalva.UseVisualStyleBackColor = True
        '
        'btnEsci
        '
        Me.btnEsci.Location = New System.Drawing.Point(342, 164)
        Me.btnEsci.Name = "btnEsci"
        Me.btnEsci.Size = New System.Drawing.Size(55, 21)
        Me.btnEsci.TabIndex = 9
        Me.btnEsci.TabStop = False
        Me.btnEsci.Text = "Esci"
        Me.btnEsci.UseVisualStyleBackColor = True
        '
        'txtTitolo
        '
        Me.txtTitolo.Location = New System.Drawing.Point(82, 5)
        Me.txtTitolo.MaxLength = 40
        Me.txtTitolo.Name = "txtTitolo"
        Me.txtTitolo.Size = New System.Drawing.Size(318, 20)
        Me.txtTitolo.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Titolo Help :"
        '
        'txtQuery
        '
        Me.txtQuery.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtQuery.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati2.ChiaveSelezione = Nothing
        LinkDati2.LinkCampi = Nothing
        LinkDati2.TabellaCollegata = "CLIFOR"
        Me.txtQuery.LinkDati = LinkDati2
        Me.txtQuery.Location = New System.Drawing.Point(6, 193)
        Me.txtQuery.Messaggio = Nothing
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Obbligatorio = ClSLib.CTextBoxEx.SiNO.SI
        Me.txtQuery.Size = New System.Drawing.Size(394, 108)
        Me.txtQuery.Tabella = Nothing
        Me.txtQuery.TabIndex = 12
        Me.txtQuery.TabStop = False
        Me.txtQuery.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'btnGeneara
        '
        Me.btnGeneara.Location = New System.Drawing.Point(195, 164)
        Me.btnGeneara.Name = "btnGeneara"
        Me.btnGeneara.Size = New System.Drawing.Size(74, 21)
        Me.btnGeneara.TabIndex = 13
        Me.btnGeneara.TabStop = False
        Me.btnGeneara.Text = "Gen. query"
        Me.btnGeneara.UseVisualStyleBackColor = True
        '
        'PropHlp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(412, 306)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnGeneara)
        Me.Controls.Add(Me.txtQuery)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTitolo)
        Me.Controls.Add(Me.btnEsci)
        Me.Controls.Add(Me.btnSalva)
        Me.Controls.Add(Me.lstCampi)
        Me.Controls.Add(Me.chkVisibile)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtLunghezza)
        Me.Controls.Add(Me.txtTesto)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(295, 95)
        Me.Name = "PropHlp"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTesto As System.Windows.Forms.TextBox
    Friend WithEvents txtLunghezza As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkVisibile As System.Windows.Forms.CheckBox
    Friend WithEvents lstCampi As System.Windows.Forms.ListBox
    Friend WithEvents btnSalva As System.Windows.Forms.Button
    Friend WithEvents btnEsci As System.Windows.Forms.Button
    Friend WithEvents txtTitolo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtQuery As ClSLib.CTextBoxEx
    Friend WithEvents btnGeneara As System.Windows.Forms.Button
End Class
