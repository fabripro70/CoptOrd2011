<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCopt
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
        Me.components = New System.ComponentModel.Container()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.chkContratti = New System.Windows.Forms.CheckBox()
        Me.chkSedi = New System.Windows.Forms.CheckBox()
        Me.chkFasce = New System.Windows.Forms.CheckBox()
        Me.chkNoftp = New System.Windows.Forms.CheckBox()
        Me.lbAzione = New System.Windows.Forms.TextBox()
        Me.ProgressBar2 = New System.Windows.Forms.ProgressBar()
        Me.lbattendi = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lb_grandezza = New System.Windows.Forms.TextBox()
        Me.lb_progress = New System.Windows.Forms.TextBox()
        Me.chkSaldi = New System.Windows.Forms.CheckBox()
        Me.txtRiga = New System.Windows.Forms.TextBox()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.chkListini = New System.Windows.Forms.CheckBox()
        Me.chkArticoli = New System.Windows.Forms.CheckBox()
        Me.chkClienti = New System.Windows.Forms.CheckBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lstExported = New System.Windows.Forms.ListView()
        Me.lstToExp = New System.Windows.Forms.ListView()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabPage1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.chkContratti)
        Me.TabPage1.Controls.Add(Me.chkSedi)
        Me.TabPage1.Controls.Add(Me.chkFasce)
        Me.TabPage1.Controls.Add(Me.chkNoftp)
        Me.TabPage1.Controls.Add(Me.lbAzione)
        Me.TabPage1.Controls.Add(Me.ProgressBar2)
        Me.TabPage1.Controls.Add(Me.lbattendi)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.lb_grandezza)
        Me.TabPage1.Controls.Add(Me.lb_progress)
        Me.TabPage1.Controls.Add(Me.chkSaldi)
        Me.TabPage1.Controls.Add(Me.txtRiga)
        Me.TabPage1.Controls.Add(Me.chkAll)
        Me.TabPage1.Controls.Add(Me.chkListini)
        Me.TabPage1.Controls.Add(Me.chkArticoli)
        Me.TabPage1.Controls.Add(Me.chkClienti)
        Me.TabPage1.Controls.Add(Me.btnStart)
        Me.TabPage1.Controls.Add(Me.lstExported)
        Me.TabPage1.Controls.Add(Me.lstToExp)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(291, 316)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Esporta"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'chkContratti
        '
        Me.chkContratti.AutoSize = True
        Me.chkContratti.Location = New System.Drawing.Point(104, 81)
        Me.chkContratti.Name = "chkContratti"
        Me.chkContratti.Size = New System.Drawing.Size(77, 17)
        Me.chkContratti.TabIndex = 30
        Me.chkContratti.Text = "Contratti v."
        Me.chkContratti.UseVisualStyleBackColor = True
        '
        'chkSedi
        '
        Me.chkSedi.AutoSize = True
        Me.chkSedi.Location = New System.Drawing.Point(104, 126)
        Me.chkSedi.Name = "chkSedi"
        Me.chkSedi.Size = New System.Drawing.Size(47, 17)
        Me.chkSedi.TabIndex = 29
        Me.chkSedi.Text = "Sedi"
        Me.chkSedi.UseVisualStyleBackColor = True
        '
        'chkFasce
        '
        Me.chkFasce.AutoSize = True
        Me.chkFasce.Location = New System.Drawing.Point(182, 106)
        Me.chkFasce.Name = "chkFasce"
        Me.chkFasce.Size = New System.Drawing.Size(81, 17)
        Me.chkFasce.TabIndex = 28
        Me.chkFasce.Text = "Forza fasce"
        Me.ToolTip1.SetToolTip(Me.chkFasce, "Esporta gli sconti per la settimana corrente")
        Me.chkFasce.UseVisualStyleBackColor = True
        '
        'chkNoftp
        '
        Me.chkNoftp.AutoSize = True
        Me.chkNoftp.Location = New System.Drawing.Point(182, 83)
        Me.chkNoftp.Name = "chkNoftp"
        Me.chkNoftp.Size = New System.Drawing.Size(109, 17)
        Me.chkNoftp.TabIndex = 27
        Me.chkNoftp.Text = "Senza spedizione"
        Me.chkNoftp.UseVisualStyleBackColor = True
        '
        'lbAzione
        '
        Me.lbAzione.Location = New System.Drawing.Point(6, 253)
        Me.lbAzione.Name = "lbAzione"
        Me.lbAzione.ReadOnly = True
        Me.lbAzione.Size = New System.Drawing.Size(156, 20)
        Me.lbAzione.TabIndex = 26
        '
        'ProgressBar2
        '
        Me.ProgressBar2.Location = New System.Drawing.Point(6, 279)
        Me.ProgressBar2.Name = "ProgressBar2"
        Me.ProgressBar2.Size = New System.Drawing.Size(277, 22)
        Me.ProgressBar2.TabIndex = 25
        '
        'lbattendi
        '
        Me.lbattendi.AutoSize = True
        Me.lbattendi.Location = New System.Drawing.Point(51, 226)
        Me.lbattendi.Name = "lbattendi"
        Me.lbattendi.Size = New System.Drawing.Size(62, 13)
        Me.lbattendi.TabIndex = 24
        Me.lbattendi.Text = "Attendere..."
        Me.lbattendi.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(200, 222)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(12, 13)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "/"
        '
        'lb_grandezza
        '
        Me.lb_grandezza.Location = New System.Drawing.Point(211, 219)
        Me.lb_grandezza.Name = "lb_grandezza"
        Me.lb_grandezza.ReadOnly = True
        Me.lb_grandezza.Size = New System.Drawing.Size(71, 20)
        Me.lb_grandezza.TabIndex = 22
        '
        'lb_progress
        '
        Me.lb_progress.Location = New System.Drawing.Point(133, 219)
        Me.lb_progress.Name = "lb_progress"
        Me.lb_progress.ReadOnly = True
        Me.lb_progress.Size = New System.Drawing.Size(68, 20)
        Me.lb_progress.TabIndex = 21
        '
        'chkSaldi
        '
        Me.chkSaldi.AutoSize = True
        Me.chkSaldi.Location = New System.Drawing.Point(104, 103)
        Me.chkSaldi.Name = "chkSaldi"
        Me.chkSaldi.Size = New System.Drawing.Size(49, 17)
        Me.chkSaldi.TabIndex = 20
        Me.chkSaldi.Text = "Saldi"
        Me.chkSaldi.UseVisualStyleBackColor = True
        '
        'txtRiga
        '
        Me.txtRiga.Location = New System.Drawing.Point(6, 193)
        Me.txtRiga.Name = "txtRiga"
        Me.txtRiga.ReadOnly = True
        Me.txtRiga.Size = New System.Drawing.Size(277, 20)
        Me.txtRiga.TabIndex = 19
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(46, 14)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(47, 17)
        Me.chkAll.TabIndex = 18
        Me.chkAll.Text = "Tutti"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'chkListini
        '
        Me.chkListini.AutoSize = True
        Me.chkListini.Location = New System.Drawing.Point(104, 60)
        Me.chkListini.Name = "chkListini"
        Me.chkListini.Size = New System.Drawing.Size(52, 17)
        Me.chkListini.TabIndex = 17
        Me.chkListini.Text = "Listini"
        Me.chkListini.UseVisualStyleBackColor = True
        '
        'chkArticoli
        '
        Me.chkArticoli.AutoSize = True
        Me.chkArticoli.Location = New System.Drawing.Point(104, 37)
        Me.chkArticoli.Name = "chkArticoli"
        Me.chkArticoli.Size = New System.Drawing.Size(57, 17)
        Me.chkArticoli.TabIndex = 16
        Me.chkArticoli.Text = "Articoli"
        Me.chkArticoli.UseVisualStyleBackColor = True
        '
        'chkClienti
        '
        Me.chkClienti.AutoSize = True
        Me.chkClienti.Location = New System.Drawing.Point(104, 14)
        Me.chkClienti.Name = "chkClienti"
        Me.chkClienti.Size = New System.Drawing.Size(54, 17)
        Me.chkClienti.TabIndex = 15
        Me.chkClienti.Text = "Clienti"
        Me.chkClienti.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(38, 153)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 14
        Me.btnStart.Text = "Start export"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lstExported
        '
        Me.lstExported.Location = New System.Drawing.Point(247, 6)
        Me.lstExported.Name = "lstExported"
        Me.lstExported.Size = New System.Drawing.Size(34, 25)
        Me.lstExported.TabIndex = 12
        Me.lstExported.UseCompatibleStateImageBehavior = False
        Me.lstExported.Visible = False
        '
        'lstToExp
        '
        Me.lstToExp.Location = New System.Drawing.Point(247, 37)
        Me.lstToExp.Name = "lstToExp"
        Me.lstToExp.Size = New System.Drawing.Size(34, 26)
        Me.lstToExp.TabIndex = 11
        Me.lstToExp.UseCompatibleStateImageBehavior = False
        Me.lstToExp.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(4, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(299, 342)
        Me.TabControl1.TabIndex = 8
        '
        'frmCopt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 352)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "frmCopt"
        Me.Text = "frmCopt"
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents txtRiga As System.Windows.Forms.TextBox
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkListini As System.Windows.Forms.CheckBox
    Friend WithEvents chkArticoli As System.Windows.Forms.CheckBox
    Friend WithEvents chkClienti As System.Windows.Forms.CheckBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents lstExported As System.Windows.Forms.ListView
    Friend WithEvents lstToExp As System.Windows.Forms.ListView
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents chkSaldi As System.Windows.Forms.CheckBox
    Friend WithEvents ProgressBar2 As System.Windows.Forms.ProgressBar
    Friend WithEvents lbattendi As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lb_grandezza As System.Windows.Forms.TextBox
    Friend WithEvents lb_progress As System.Windows.Forms.TextBox
    Friend WithEvents lbAzione As System.Windows.Forms.TextBox
    Friend WithEvents chkNoftp As System.Windows.Forms.CheckBox
    Friend WithEvents chkFasce As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkSedi As System.Windows.Forms.CheckBox
    Friend WithEvents chkContratti As System.Windows.Forms.CheckBox
End Class
