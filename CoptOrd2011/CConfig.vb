Imports System.Configuration
Imports System.Security.Cryptography
Public Class CConfig
    Public Sub New()
        Try
            Dim config As Configuration
            Dim elm As SettingElement
            Dim grpapp As ConfigurationSectionGroup
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            grpapp = config.GetSectionGroup("applicationSettings")
            Dim secPropSet As ClientSettingsSection
            secPropSet = CType(grpapp.Sections("adhocsync.My.MySettings"), ClientSettingsSection)
            '
            elm = secPropSet.Settings.Get("TipoDb")
            Globale.TipoDb = elm.Value.ValueXml.InnerText
            '
            elm = secPropSet.Settings.Get("ConnectionString")
            Globale.ConnectionString = elm.Value.ValueXml.InnerText
            '
            elm = secPropSet.Settings.Get("CodAzi")
            Globale.CodAzi = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("CartellaAggLocale")
            Globale.CartellaAggLocale = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("CartellaImport")
            Globale.ImportFolder = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("DocType")
            Globale.DocType = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("DocType_P")
            Globale.DocType_P = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("Store")
            Globale.Store = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("CatCon")
            Globale.CatCon = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("MastroCli")
            Globale.MastroCli = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("ListDef")
            Globale.ListDef = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("Compress")
            Globale.gCompres = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("Fasce")
            Globale.gFasce = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("ExtendLog")
            Globale.gExtendLog = elm.Value.ValueXml.InnerText
            elm = secPropSet.Settings.Get("UTF-8")
            Globale.gUTF8 = elm.Value.ValueXml.InnerText
            '
            Try
                elm = secPropSet.Settings.Get("TableAgg")
                Globale.gTableAgg = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                Globale.gTableAgg = "aggiornamenti"
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("ExpOrdStatus")
                Globale.gExpOrdStatus = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                Globale.gExpOrdStatus = "S"
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("DefPayCode")
                Globale.gDefPayCode = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                Globale.gDefPayCode = ""
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("CodNaz")
                Globale.gCodNaz = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                Globale.gCodNaz = "ITA"
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("LenCodCli")
                Globale.gLenCodCli = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Specificare la lunghezza del codice cliente, parametro : LenCodCLi, nel file di configurazione!")
                Application.Exit()
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("CauCorrisp")
                Globale.gCauCorrisp = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Specificare la causale per i corrispettivi, nel file di configurazione!")
                Application.Exit()
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("emailTo")
                g_emailTo = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_emailTo = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("emailCC")
                g_emailCC = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_emailCC = ""
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("Mittente")
                g_Mittente = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Mittente = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("SMTP")
                g_SMTP = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_SMTP = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("Auth")
                g_Auth = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Auth = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("Ssl")
                g_Ssl = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Ssl = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("Utente")
                g_Utente = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Utente = ""
            End Try
            Try
                elm = secPropSet.Settings.Get("Password")
                g_Password = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Password = ""
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("SmtpPort")
                g_Port = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                g_Port = ""
            End Try
            '
            Try
                elm = secPropSet.Settings.Get("CatCom")
                Globale.g_CatCom = elm.Value.ValueXml.InnerText
            Catch ex As Exception
                Globale.g_CatCom = "1"
            End Try
            '
        Catch ex As System.Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Lettura configurazione")
        End Try
    End Sub
    Public Sub LeggeConfig()
    End Sub
    Public Sub ScriviConfig()
        Try

            Dim config As Configuration
            Dim elm As SettingElement
            Dim grpapp As ConfigurationSectionGroup
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            grpapp = config.GetSectionGroup("applicationSettings")
            Dim secPropSet As ClientSettingsSection
            secPropSet = CType(grpapp.Sections("adhocsync.My.MySettings"), ClientSettingsSection)
            secPropSet.SectionInformation.ForceSave = True
            '
            elm = secPropSet.Settings.Get("ConnectionString")
            elm.Value.ValueXml.InnerText = Globale.ConnectionString
            '
             config.Save()
        Catch ex As ConfigurationErrorsException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Scrittura configurazione")
        End Try

    End Sub
End Class
