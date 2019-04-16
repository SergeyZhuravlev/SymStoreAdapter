# SymStoreAdapter

symstore doesn't support concurrent access to PDB repository:
https://docs.microsoft.com/en-us/windows/desktop/debug/using-symstore
> SymStore does not support simultaneous transactions from multiple users. 

1) SymStoreAdapter protect debug symbols PDB repository from concurrent access.
2) SymStoreAdapter supporting cleaning symbol repository for transactions older then specified.

Example of adding PDBs from directory to repository:
> symstoreadapter add /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /SymStoreArgs /r /o /t "MyProduct x86" /f "D:\PDBSource\Debug" /s "D:\PDBserver"

Example of delete PDB transaction 0000000019 from PDB repository:
> symstoreadapter del /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /SymStoreArgs /i 0000000019 /o /s "D:\PDBserver"

Example of delete PDB transactions from PDB repository older then 90 days:
> symstoreadapter CleanOldTransactions /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /After 90.00:00:00 /s "D:\PDBserver"
