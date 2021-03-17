<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class hlpart
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
        Dim LinkDati1 As ClSLib.LinkDati = New ClSLib.LinkDati
        Dim LinkDati2 As ClSLib.LinkDati = New ClSLib.LinkDati
        Me.griart = New System.Windows.Forms.DataGridView
        Me.Codice = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Descrizione = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UM = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtComples = New ClSLib.CTextBoxEx
        Me.txtDesart = New ClSLib.CTextBoxEx
        Me.btnCerca = New System.Windows.Forms.Button
        CType(Me.griart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'griart
        '
        Me.griart.AllowUserToAddRows = False
        Me.griart.AllowUserToDeleteRows = False
        Me.griart.AllowUserToResizeRows = False
        Me.griart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.griart.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Codice, Me.Descrizione, Me.UM})
        Me.griart.Location = New System.Drawing.Point(12, 34)
        Me.griart.Name = "griart"
        Me.griart.Size = New System.Drawing.Size(513, 243)
        Me.griart.TabIndex = 0
        '
        'Codice
        '
        Me.Codice.HeaderText = "Codice"
        Me.Codice.Name = "Codice"
        '
        'Descrizione
        '
        Me.Descrizione.HeaderText = "Descrizione"
        Me.Descrizione.Name = "Descrizione"
        Me.Descrizione.Width = 300
        '
        'UM
        '
        Me.UM.HeaderText = "U.M."
        Me.UM.Name = "UM"
        Me.UM.Width = 50
        '
        'txtComples
        '
        Me.txtComples.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtComples.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati1.campiDb = Nothing
        LinkDati1.campiForm = Nothing
        LinkDati1.ChiaveSelezione = Nothing
        LinkDati1.Criteriofisso = Nothing
        LinkDati1.elencoCampi = Nothing
        LinkDati1.LinkCampi = Nothing
        LinkDati1.NUMREC = CType(0, Long)
        LinkDati1.puntatoreDb = Nothing
        LinkDati1.TabellaCollegata = Nothing
        LinkDati1.UsaNumrec = ClSLib.SiNO.NO
        LinkDati1.ZoomOnZoom = Nothing
        Me.txtComples.LinkDati = LinkDati1
        Me.txtComples.Location = New System.Drawing.Point(52, 12)
        Me.txtComples.MaxLength = 50
        Me.txtComples.Messaggio = Nothing
        Me.txtComples.Name = "txtComples"
        Me.txtComples.NumeroDecimali = 0
        Me.txtComples.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtComples.Size = New System.Drawing.Size(101, 20)
        Me.txtComples.Tabella = Nothing
        Me.txtComples.TabIndex = 1
        Me.txtComples.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'txtDesart
        '
        Me.txtDesart.EnterAsTab = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtDesart.EventControlla = ClSLib.CTextBoxEx.SiNO.SI
        LinkDati2.campiDb = Nothing
        LinkDati2.campiForm = Nothing
        LinkDati2.ChiaveSelezione = Nothing
        LinkDati2.Criteriofisso = Nothing
        LinkDati2.elencoCampi = Nothing
        LinkDati2.LinkCampi = Nothing
        LinkDati2.NUMREC = CType(0, Long)
        LinkDati2.puntatoreDb = Nothing
        LinkDati2.TabellaCollegata = Nothing
        LinkDati2.UsaNumrec = ClSLib.SiNO.NO
        LinkDati2.ZoomOnZoom = Nothing
        Me.txtDesart.LinkDati = LinkDati2
        Me.txtDesart.Location = New System.Drawing.Point(155, 12)
        Me.txtDesart.MaxLength = 50
        Me.txtDesart.Messaggio = Nothing
        Me.txtDesart.Name = "txtDesart"
        Me.txtDesart.NumeroDecimali = 0
        Me.txtDesart.Obbligatorio = ClSLib.CTextBoxEx.SiNO.NO
        Me.txtDesart.Size = New System.Drawing.Size(298, 20)
        Me.txtDesart.Tabella = Nothing
        Me.txtDesart.TabIndex = 2
        Me.txtDesart.TipoCampo = ClSLib.CTextBoxEx.Tipi.Alfanumerico
        '
        'btnCerca
        '
        Me.btnCerca.Location = New System.Drawing.Point(453, 11)
        Me.btnCerca.Name = "btnCerca"
        Me.btnCerca.Size = New System.Drawing.Size(49, 21)
        Me.btnCerca.TabIndex = 3
        Me.btnCerca.Text = "Cerca"
        Me.btnCerca.UseVisualStyleBackColor = True
        '
        'hlpart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(538, 289)
        Me.Controls.Add(Me.btnCerca)
        Me.Controls.Add(Me.txtDesart)
        Me.Controls.Add(Me.txtComples)
        Me.Controls.Add(Me.griart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "hlpart"
        Me.Text = "Elenco Articoli"
        CType(Me.griart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents griart As System.Windows.Forms.DataGridView
    Friend WithEvents Codice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Descrizione As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UM As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtComples As ClSLib.CTextBoxEx
    Friend WithEvents txtDesart As ClSLib.CTextBoxEx
    Friend WithEvents btnCerca As System.Windows.Forms.Button
End Class
