USE [ahr65copt]
GO
/****** Object:  StoredProcedure [dbo].[sp_legge_vendite_adhoc]    Script Date: 17/03/2021 08:12:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER   PROC [dbo].[sp_legge_vendite_adhoc] 
AS
   DECLARE @ANNO         CHAR(4)
   DECLARE @CODCLI       CHAR(15)
   DECLARE @DESCLI       CHAR(40)
   DECLARE @PIVA           CHAR(18)
   DECLARE @GEN            MONEY
   DECLARE @FEB            MONEY
   DECLARE @MAR            MONEY
   DECLARE @APR            MONEY
   DECLARE @MAG            MONEY
   DECLARE @GIU            MONEY
   DECLARE @LUG            MONEY
   DECLARE @AGO            MONEY
   DECLARE @SETT            MONEY
   DECLARE @OTT            MONEY
   DECLARE @NOV            MONEY
   DECLARE @DIC            MONEY
   DECLARE @SERIALE     CHAR(10)
   DECLARE @CMD            VARCHAR(8000)
   DECLARE @AHRAZ         CHAR(5)
   DECLARE @TIPART         CHAR(2)
   DECLARE @CAUMAG        CHAR(5)
   DECLARE @FLOMAG1        CHAR(1)
   DECLARE @FLOMAG2        CHAR(1)
   DECLARE @FLOMAG3        CHAR(1)
   DECLARE @FLOMAG4        CHAR(1)
   DECLARE @MVAIMPN1       MONEY
   DECLARE @MVAIMPN2       MONEY
   DECLARE @MVAIMPN3       MONEY
   DECLARE @MVAIMPN4       MONEY
   DECLARE @MVSPEINC       MONEY
   DECLARE @MVSPEBOL       MONEY
   DECLARE @VALTMP         MONEY
   DECLARE @MESE           INT

   SET @AHRAZ='coptg'
   SET @TIPART='FO'
   SET @ANNO=YEAR(GETDATE())
--   SET @ANNO='2009'
   --
   CREATE TABLE #FATTURATO 
   (
          SERIALE CHAR(10),
          CLIENTE CHAR(15),
		  ANNO CHAR(4),
          GEN       MONEY,
          FEB       MONEY,
          MAR       MONEY,
          APR       MONEY,
          MAG       MONEY,
          GIU       MONEY,
          LUG       MONEY,
          AGO       MONEY,
          SETT       MONEY,
          OTT       MONEY,
          NOV       MONEY,
          DIC       MONEY
   ) 	
   --
   DELETE FROM coptgFATTMENS WHERE ANNO = @ANNO

   --Vado a leggere tutti i record relativi alle vendite : tm_tipork = 'B' per ddt , 'A' per datture immediate, 'N' per note di accredito 
   DECLARE CURSORE CURSOR FOR
		select mvserial, 
					  mvcodcon, 
					  andescri, 
					  MONTH(MVDATDOC) AS MESE,
					  MVAIMPN1,MVAIMPN2,MVAIMPN3,MVAIMPN4,
					  TDCAUMAG, 
					  MVAFLOM1, MVAFLOM2, MVAFLOM3, MVAFLOM4,
					  MVSPEINC, MVSPEBOL  
				  from (coptgdoc_mast a inner join coptgasysuteweb d on a.mvcodcon = d.utcodcli left join coptgconti b on a.mvcodcon = b.ancodice and a.mvtipcon = b.antipcon)
													left join coptgtip_docu c on a.mvtipdoc = c.tdtipdoc
				  where a.mvcodese = @ANNO AND (A.MVTIPDOC = 'DDTVE' 
											 OR A.MVTIPDOC = 'FATAC' 
											 OR A.MVTIPDOC = 'DDTRC') 
				  --      and a.mvcodcon = '0002085' AND MONTH(A.MVDATDOC)=2
				 order by MVSERIAL   OPEN CURSORE 
   FETCH NEXT FROM CURSORE INTO @SERIALE, @CODCLI, @DESCLI, @MESE, @MVAIMPN1, @MVAIMPN2, @MVAIMPN3, 
                                @MVAIMPN4, @CAUMAG, @FLOMAG1, @FLOMAG2, @FLOMAG3, @FLOMAG4,
                                @MVSPEINC, @MVSPEBOL
   WHILE(@@FETCH_STATUS <> -1)
   BEGIN
				   SET @GEN=0
				   SET @FEB=0
				   SET @MAR=0
				   SET @APR=0
				   SET @MAG=0
				   SET @GIU=0
				   SET @LUG=0
				   SET @AGO=0
				   SET @SETT=0
				   SET @OTT=0
				   SET @NOV=0
				   SET @DIC=0
                   SET @VALTMP=0
                   --
                   --PRENDO GLI IMPORTI SOLO SE NON SONO OMAGGI
                   --
                   IF @FLOMAG1='X' BEGIN SET @VALTMP=@MVAIMPN1 END
                   IF @FLOMAG2='X' BEGIN SET @VALTMP=@VALTMP+@MVAIMPN2 END
                   IF @FLOMAG3='X' BEGIN SET @VALTMP=@VALTMP+@MVAIMPN3 END
                   IF @FLOMAG4='X' BEGIN SET @VALTMP=@VALTMP+@MVAIMPN4 END
                   --
                   --PRINT @VALTMP
                   SET @VALTMP=@VALTMP-@MVSPEINC-@MVSPEBOL
                   --PRINT @VALTMP
				   --	
                   IF @MESE='1' BEGIN SET @GEN=@VALTMP END 
                   IF @MESE='2' BEGIN SET @FEB=@VALTMP END 
                   IF @MESE='3' BEGIN SET @MAR=@VALTMP END 
                   IF @MESE='4' BEGIN SET @APR=@VALTMP END 
                   IF @MESE='5' BEGIN SET @MAG=@VALTMP END 
                   IF @MESE='6' BEGIN SET @GIU=@VALTMP END 
                   IF @MESE='7' BEGIN SET @LUG=@VALTMP END 
                   IF @MESE='8' BEGIN SET @AGO=@VALTMP END 
                   IF @MESE='9' BEGIN SET @SETT=@VALTMP END 
                   IF @MESE='10' BEGIN SET @OTT=@VALTMP END 
                   IF @MESE='11' BEGIN SET @NOV=@VALTMP END 
                   IF @MESE='12' BEGIN SET @DIC=@VALTMP END 
                   --
                   IF @CAUMAG = 'RESCO'  --Nota di accredito
                       BEGIN
                           SET @GEN=@GEN*-1
                           SET @FEB=@FEB*-1
                           SET @MAR=@MAR*-1
                           SET @APR=@APR*-1
                           SET @MAG=@MAG*-1
                           SET @GIU=@GIU*-1
                           SET @LUG=@LUG*-1
                           SET @AGO=@AGO*-1
                           SET @SETT=@SETT*-1
                           SET @OTT=@OTT*-1
                           SET @NOV=@NOV*-1
                           SET @DIC=@DIC*-1
                       END
                       BEGIN TRY
                           INSERT INTO coptgFATTMENS (FBCLIENTE, GEN, FEB, MAR, APR, MAG, GIU, LUG, AGO, SETT, OTT, NOV, DIC, FBDESCLI, ANNO) 
                                                          VALUES (@CODCLI, @GEN, @FEB, @MAR, @APR, @MAG, @GIU, @LUG, @AGO, @SETT, @OTT, @NOV, @DIC, @DESCLI, @ANNO)
					   END TRY
					   BEGIN CATCH
							UPDATE coptgFATTMENS SET FBCLIENTE = @CODCLI, FBDESCLI = @DESCLI, ANNO = @ANNO,
                                                                         GEN = GEN + @GEN,
																		 FEB = FEB + @FEB,
																		 MAR = MAR + @MAR,
																		 APR = APR + @APR,
																		 MAG = MAG + @MAG,
																		 GIU = GIU + @GIU,
																		 LUG = LUG + @LUG,
																		 AGO = AGO + @AGO,
																		 SETT = SETT + @SETT,
																		 OTT = OTT + @OTT,
																		 NOV = NOV + @NOV,
																		 DIC = DIC + @DIC
								 WHERE FBCLIENTE = @CODCLI AND ANNO = @ANNO
                       END CATCH
   FETCH NEXT FROM CURSORE INTO @SERIALE, @CODCLI, @DESCLI, @MESE, @MVAIMPN1, @MVAIMPN2, @MVAIMPN3, 
                                @MVAIMPN4, @CAUMAG, @FLOMAG1, @FLOMAG2, @FLOMAG3, @FLOMAG4,
                                @MVSPEINC, @MVSPEBOL
   END
   CLOSE CURSORE 
   DEALLOCATE CURSORE
   SELECT FBCLIENTE,FBDESCLI,UT_EMAIL,GEN,FEB,MAR,APR,MAG,GIU,LUG,AGO,SETT,OTT,NOV,DIC,ANNO FROM COPTGFATTMENS INNER JOIN COPTGASYSUTEWEB ON FBCLIENTE=UTCODCLI WHERE ANNO = @ANNO