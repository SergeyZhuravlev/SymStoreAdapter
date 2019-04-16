# SymStoreAdapter

symstore doesn't support concurrent access to PDB repository:
https://docs.microsoft.com/en-us/windows/desktop/debug/using-symstore
> SymStore does not support simultaneous transactions from multiple users. 

1) SymStoreAdapter protect debug symbols PDB repository from concurrent access even when PDB repository located in network shared directory.
2) SymStoreAdapter supporting cleanup symbol repository for transactions older then specified.
3) SymStoreAdapter supporting of fast recursive directory debug symbols commit to PDB repository with file filter.


Example of adding all PDBs from directory and subdirectories to repository with file filter: *.pdb
> symstoreadapter RecursiveAddFromDirectoryWithFilter /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /f "D:\PDBSource\Debug" /FileFilter "*.pdb" /SymStoreArgs /o /t "MyProduct x86" /s "D:\PDBserver"

Example of adding test.pdb file from directory to repository:
> symstoreadapter add /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /SymStoreArgs /o /t "MyProduct x86" /f "D:\PDBSource\Debug\test.pdb" /s "D:\PDBserver"

Example of delete PDB transaction 0000000019 from PDB repository:
> symstoreadapter del /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /SymStoreArgs /i 0000000019 /o /s "D:\PDBserver"

Example of delete PDB transactions from PDB repository older then 90 days:
> symstoreadapter CleanOldTransactions /SymStorePath "C:\Program Files (x86)\Windows Kits\10\Debuggers\x64\symstore.exe" /After 90.00:00:00 /s "D:\PDBserver"
