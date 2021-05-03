using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using WolvenKit.Common;
using Path = System.IO.Path;

namespace WolvenKit.RED4.CR2W.Archive
{
    [ProtoContract]
    public class ArchiveManager : CyberArchiveManager
    {
        #region Fields

        public static string SerializationVersion = "1.1";

        private DirectoryInfo _parentDirectoryInfo;

        #endregion Fields

        #region Constructors

        public ArchiveManager()
        {
        }

        public ArchiveManager(DirectoryInfo indir)
            : this()
        {
            _parentDirectoryInfo = indir;
            Reload(indir);
        }

        #endregion Constructors

        #region properties

        //[ProtoMember(1)] public Dictionary<string, Archive> Archives { get; set; }
        [ProtoMember(1)] public override Dictionary<string, IGameArchive> Archives { get; set; } = new();

        public Dictionary<string, IEnumerable<FileEntry>> GroupedFiles =>
            Archives.Values
                .SelectMany(_ => _.Files.Values)
                .GroupBy(_ => _.Extension)
                .ToDictionary(_ => _.Key, _ => _.Select(x => x as FileEntry));


        #endregion properties

        #region methods

        [ProtoAfterDeserialization]
        public void AfterDeserializationCallback()
        {
            foreach (var archive in Archives.Values)
            {
                foreach (var file in archive.Files.Values)
                {
                    file.Archive = archive;
                }

            }

            RebuildRootNode();
        }

        public override EArchiveType TypeName => EArchiveType.Archive;

        /// <summary>
        ///     Load every non-mod bundle it can find in ..\..\content and ..\..\DLC, also calls RebuildRootNode()
        /// </summary>
        /// <param name="exedir">Path to executable directory</param>
        public override void LoadAll(string exedir)
        {
            var di = new DirectoryInfo(exedir);
            if (!di.Exists)
            {
                return;
            }

            var archivedir = Path.Combine(di.Parent.Parent.FullName, "archive", "pc", "content");

            foreach (var file in Directory.GetFiles(archivedir, "*.archive"))
            {
                LoadArchive(file);
            }
            RebuildRootNode();
        }

        /// <summary>
        /// Load a single bundle
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ispatch"></param>
        public override void LoadArchive(string filename, bool ispatch = false)
        {
            if (Archives.ContainsKey(filename))
            {
                return;
            }

            var bundle = new Archive(filename);
            //foreach (var (key, value) in bundle.Files)
            //{
            //    // add new key if the file isn't already in another bundle
            //    if (!Items.ContainsKey(value.Name))
            //    {
            //        Items.Add(value.Name, new List<IGameFile>());
            //    }

            //    Items[value.Name].Add(value);
            //}
            Archives.Add(bundle.ArchiveAbsolutePath, bundle);
        }

        /// <summary>
        ///     Load a single mod bundle
        /// </summary>
        /// <param name="filename">
        ///     file to process
        /// </param>
        public override void LoadModArchive(string filename)
        {
            return;
            /*if (Archives.ContainsKey(filename))
                return;

            var bundle = new Archive(filename);

            foreach (var item in bundle.Files)
            {
                if (!Items.ContainsKey(GetModFolder(filename) + "\\" + item.Key))
                    Items.Add(GetModFolder(filename) + "\\" + item.Key, new List<IGameFile>());

                Items[GetModFolder(filename) + "\\" + item.Key].Add(item.Value);
            }

            Archives.Add(filename, bundle);*/
        }

        /// <summary>
        /// Loads bundles from specified mods and dlc folder
        /// </summary>
        /// <param name="mods"></param>
        /// <param name="dlc"></param>
        public override void LoadModsArchives(string mods, string dlc)
        {
            return;
            /*if (!Directory.Exists(mods))
                Directory.CreateDirectory(mods);
            var modsdirs = new List<string>(Directory.GetDirectories(mods));
            modsdirs.Sort(new AlphanumComparator<string>());
            var modbundles = modsdirs.SelectMany(dir => Directory.GetFiles(dir, "*.bundle", SearchOption.AllDirectories)).ToList();
            foreach (var file in modbundles)
            {
                LoadModArchive(file);
            }

            if (Directory.Exists(dlc))
            {
                var dlcdirs = new List<string>(Directory.GetDirectories(dlc));
                dlcdirs.Sort(new AlphanumComparator<string>());

                var tmp = dlcdirs.Where(_ => !VanillaDlClist.Contains(new DirectoryInfo(_).Name)).ToList();
                foreach (var file in tmp.SelectMany(dir => Directory.GetFiles(dir ?? "", "*.bundle", SearchOption.AllDirectories).OrderBy(k => k)))
                {
                    LoadModArchive(file);
                }
            }
            RebuildRootNode();*/
        }

        /// <summary>
        /// Reload the ArchiveManager from given directory (optional).
        /// </summary>
        /// <param name="indir"></param>
        public void Reload(DirectoryInfo indir = null)
        {
            if (indir != null)
            {
                _parentDirectoryInfo = indir;
            }

            var archives = _parentDirectoryInfo.GetFiles("*.archive").ToList();
            Archives.Clear();
            foreach (var fi in archives)
            {
                Archives.Add(fi.FullName, new Archive(fi.FullName));
            }

        }

        #endregion methods
    }
}
