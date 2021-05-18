# CoptOrd2011

Sintassi linea di comando:

AdhocSync.exe [DUMPVENDUTO|DUMPVENDUTOSTO|DUMPLISTINIAGENTI|DUMPLISTINI|DUMPCONTRATTI
               AUTO SALDIART,
               AUTO ARTICOLI,
               AUTO LISTINI]
               
DUMPVENDUTO : crea ed esporta file sql con il venduto del mese corrente, usa stored procedure sp_legge_vendite_adhoc
DUMPVENDUTOSTO : crea ed esporta file sql con il venduto dell' anno indicato nella stored proc : sp_legge_vendite_adhoc_storico
DUMPLISTINIAGENTI : crea file sql per esportazione completa listini di vendita
DUMPLISTINI: crea file sql per esportazione completa anagrafiche listini
DUMPCONTRATTI: crea file sql per esportazione completa contratti
AUTO SALDIART : esporta Saldi articoli modificati o creati via xml (usa trigger)
AUTO ARTICOLI : esporta anagrafiche articoli modificate o create via xml (usa trigger)
AUTO LISTINI : esporta listini (anagrafiche e prezzi) e contratti (anagrafiche e prezzi) creati o modificati. (usa trigger)

