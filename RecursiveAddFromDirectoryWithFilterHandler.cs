using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;

namespace SymStoreAdapter
{
    class RecursiveAddFromDirectoryWithFilterHandler : SymStoreExecuteBase
    {
        public RecursiveAddFromDirectoryWithFilterHandler(string[] cmdArgs) : base(cmdArgs)
        {
            _fileFilter = CmdParser.Value(CmdArgumentOptions.FileFilter);
            if (string.IsNullOrWhiteSpace(_fileFilter))
                _fileFilter = "*.pdb";

            if (!CmdParser.IsKey(CmdArgumentOptions.SymStoreArgs))
            {
                Help();
                throw new ArgumentException(
                    $"Not found {CmdArgumentOptions.SymStoreArgs} commandline argument");
            }

            var pdbSourcePath = CmdParser.Value(CmdArgumentOptions.PdbSourcePath);
            if (string.IsNullOrWhiteSpace(pdbSourcePath) || (!CmdParser.IsArgumentBefore(CmdArgumentOptions.PdbSourcePath,
                    CmdArgumentOptions.SymStoreArgs)))
            {
                Help();
                throw new ArgumentException(
                    $"{CmdArgumentOptions.PdbSourcePath} commandline argument must exist only before command line option {CmdArgumentOptions.SymStoreArgs}");
            }

            _pdbSourcePath = pdbSourcePath;
        }

        private void Help()
        {
            Console.Error.WriteLine($"This mode {GetType()} required {CmdArgumentOptions.PdbSourcePath} and {CmdArgumentOptions.SymStorePath} and {CmdArgumentOptions.PdbRepositoryPath} and {CmdArgumentOptions.SymStoreArgs} commandline arguments.");
            Console.Error.WriteLine($"Additionally supported command line options: {CmdArgumentOptions.FileFilter}");
        }

        public override int Execute()
        {
            using (var fileListPath = new TempFile())
            {
                if (!FillAddingFilesList(fileListPath.FileName))
                    return 0;
                return AddFiles(fileListPath.FileName);
            }
        }

        private bool FillAddingFilesList(string fileListPath)
        {
            var files = Directory.GetFiles(_pdbSourcePath, _fileFilter, SearchOption.AllDirectories).ToList();
            if (!files.Any())
            {
                Console.WriteLine(
                    $"Nothing to store PDB files from directory '{_pdbSourcePath}' with filter '{_fileFilter}'");
                return false;
            }

            File.WriteAllLines(fileListPath, files);
            return true;
        }

        protected override string SymStoreMode()
        {
            return "add";
        }

        protected override string SymStoreArgs()
        {
            return $"/f \"@{_fileListPath}\" {SymStoreAllArgs()}";
        }

        private int AddFiles(string fileListPath)
        {
            _fileListPath = fileListPath;
            return base.Execute();
        }

        public override bool AutoHandled()
        {
            return false;
        }

        private string _fileListPath;
        private readonly string _pdbSourcePath;
        private readonly string _fileFilter;
    }
}
