using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GzsTool.Core.Utility
{
    public static class Hashing
    {
        private static readonly MD5 Md5 = MD5.Create();
        private static readonly Dictionary<ulong, string> HashNameDictionary = new Dictionary<ulong, string>();

        private static readonly Dictionary<byte[], string> Md5HashNameDictionary =
            new Dictionary<byte[], string>(new StructuralEqualityComparer<byte[]>());

        private static readonly List<string> FileExtensions = new List<string>
        {
            "1.ftexs",
            "1.nav2",
            "2.ftexs",
            "3.ftexs",
            "4.ftexs",
            "5.ftexs",
            "6.ftexs",
            "ag.evf",
            "aia",
            "aib",
            "aibc",
            "aig",
            "aigc",
            "aim",
            "aip",
            "ait",
            "atsh",
            "bnd",
            "bnk",
            "cc.evf",
            "clo",
            "csnav",
            "dat",
            "des",
            "dnav",
            "dnav2",
            "eng.lng",
            "ese",
            "evb",
            "evf",
            "fag",
            "fage",
            "fago",
            "fagp",
            "fagx",
            "fclo",
            "fcnp",
            "fcnpx",
            "fdes",
            "fdmg",
            "ffnt",
            "fmdl",
            "fmdlb",
            "fmtt",
            "fnt",
            "fova",
            "fox",
            "fox2",
            "fpk",
            "fpkd",
            "fpkl",
            "frdv",
            "fre.lng",
            "frig",
            "frt",
            "fsd",
            "fsm",
            "fsml",
            "fsop",
            "fstb",
            "ftex",
            "fv2",
            "fx.evf",
            "fxp",
            "gani",
            "geom",
            "ger.lng",
            "gpfp",
            "grxla",
            "grxoc",
            "gskl",
            "htre",
            "info",
            "ita.lng",
            "jpn.lng",
            "json",
            "lad",
            "ladb",
            "lani",
            "las",
            "lba",
            "lng",
            "lpsh",
            "lua",
            "mas",
            "mbl",
            "mog",
            "mtar",
            "mtl",
            "nav2",
            "nta",
            "obr",
            "obrb",
            "param",
            "parts",
            "path",
            "pftxs",
            "ph",
            "phep",
            "phsd",
            "por.lng",
            "qar",
            "rbs",
            "rdb",
            "rdf",
            "rnav",
            "rus.lng",
            "sad",
            "sand",
            "sani",
            "sbp",
            "sd.evf",
            "sdf",
            "sim",
            "simep",
            "snav",
            "spa.lng",
            "spch",
            "sub",
            "subp",
            "tgt",
            "tre2",
            "txt",
            "uia",
            "uif",
            "uig",
            "uigb",
            "uil",
            "uilb",
            "utxl",
            "veh",
            "vfx",
            "vfxbin",
            "vfxdb",
            "vnav",
            "vo.evf",
            "vpc",
            "wem",
            "wmv",
            "xml"
        };

        private static readonly Dictionary<int, string> TypeExtensions = new Dictionary<int, string>
        {
            {0, ""},
            {1, ".xml"},
            {2, ".json"},
            {3, ".ese"},
            {4, ".fxp"},
            {5, ".fpk"},
            {6, ".fpkd"},
            {7, ".fpkl"},
            {8, ".aib"},
            {9, ".frig"},
            {10, ".mtar"},
            {11, ".gani"},
            {12, ".evb"},
            {13, ".evf"},
            {14, ".ag.evf"},
            {15, ".cc.evf"},
            {16, ".fx.evf"},
            {17, ".sd.evf"},
            {18, ".vo.evf"},
            {19, ".fsd"},
            {20, ".fage"},
            {21, ".fago"},
            {22, ".fag"},
            {23, ".fagx"},
            {24, ".fagp"},
            {25, ".frdv"},
            {26, ".fdmg"},
            {27, ".des"},
            {28, ".fdes"},
            {29, ".aibc"},
            {30, ".mtl"},
            {31, ".fsml"},
            {32, ".fox"},
            {33, ".fox2"},
            {34, ".las"},
            {35, ".fstb"},
            {36, ".lua"},
            {37, ".fcnp"},
            {38, ".fcnpx"},
            {39, ".sub"},
            {40, ".fova"},
            {41, ".lad"},
            {42, ".lani"},
            {43, ".vfx"},
            {44, ".vfxbin"},
            {45, ".frt"},
            {46, ".gpfp"},
            {47, ".gskl"},
            {48, ".geom"},
            {49, ".tgt"},
            {50, ".path"},
            {51, ".fmdl"},
            {52, ".ftex"},
            {53, ".htre"},
            {54, ".tre2"},
            {55, ".grxla"},
            {56, ".grxoc"},
            {57, ".mog"},
            {58, ".pftxs"},
            {59, ".nav2"},
            {60, ".bnd"},
            {61, ".parts"},
            {62, ".phsd"},
            {63, ".ph"},
            {64, ".veh"},
            {65, ".sdf"},
            {66, ".sad"},
            {67, ".sim"},
            {68, ".fclo"},
            {69, ".clo"},
            {70, ".lng"},
            {71, ".uig"},
            {72, ".uil"},
            {73, ".uif"},
            {74, ".uia"},
            {75, ".fnt"},
            {76, ".utxl"},
            {77, ".uigb"},
            {78, ".vfxdb"},
            {79, ".rbs"},
            {80, ".aia"},
            {81, ".aim"},
            {82, ".aip"},
            {83, ".aigc"},
            {84, ".aig"},
            {85, ".ait"},
            {86, ".fsm"},
            {87, ".obr"},
            {88, ".obrb"},
            {89, ".lpsh"},
            {90, ".sani"},
            {91, ".rdb"},
            {92, ".phep"},
            {93, ".simep"},
            {94, ".atsh"},
            {95, ".txt"},
            {96, ".1.ftexs"},
            {97, ".2.ftexs"},
            {98, ".3.ftexs"},
            {99, ".4.ftexs"},
            {100, ".5.ftexs"},
            {101, ".sbp"},
            {102, ".mas"},
            {103, ".rdf"},
            {104, ".wem"},
            {105, ".lba"},
            {106, ".uilb"}
        };

        private static readonly Dictionary<ulong, string> ExtensionsMap = FileExtensions.ToDictionary(HashFileExtension);

        public const ulong MetaFlag = 0x4000000000000;

        public static ulong HashFileExtension(string fileExtension) //from private to public
        {
            return HashFileName(fileExtension, false) & 0x1FFF;
        }

        public static ulong HashFileName(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            bool metaFlag = false;
            const string assetsConstant = "/Assets/";
            if (text.StartsWith(assetsConstant))
            {
                text = text.Substring(assetsConstant.Length);

                if (text.StartsWith("tpptest"))
                {
                    metaFlag = true;
                }
            }
            else
            {
                metaFlag = true;
            }
            
            text = text.TrimStart('/');

            const ulong seed0 = 0x9ae16a3b2f90404f;
            byte[] seed1Bytes = new byte[sizeof(ulong)];
            for (int i = text.Length - 1, j = 0; i >= 0 && j < sizeof(ulong); i--, j++)
            {
                seed1Bytes[j] = Convert.ToByte(text[i]);
            }
            ulong seed1 = BitConverter.ToUInt64(seed1Bytes, 0);
            ulong maskedHash = CityHash.CityHash.CityHash64WithSeeds(text, seed0, seed1) & 0x3FFFFFFFFFFFF;

            return metaFlag
                ? maskedHash | MetaFlag
                : maskedHash;
        }
        
        public static ulong HashFileNameLegacy(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            const ulong seed0 = 0x9ae16a3b2f90404f;
            ulong seed1 = text.Length > 0 ? (uint)((text[0]) << 16) + (uint)text.Length : 0;
            return CityHash.CityHash.CityHash64WithSeeds(text + "\0", seed0, seed1) & 0xFFFFFFFFFFFF;
        }

        public static ulong HashFileNameWithExtension(string filePath)
        {
            filePath = DenormalizeFilePath(filePath);
            string hashablePart;
            string extensionPart;
            int extensionIndex = filePath.IndexOf(".", StringComparison.Ordinal);
            if (extensionIndex == -1)
            {
                hashablePart = filePath;
                extensionPart = "";
            }
            else
            {
                hashablePart = filePath.Substring(0, extensionIndex);
                extensionPart = filePath.Substring(extensionIndex + 1, filePath.Length - extensionIndex - 1);
            }

            //tex TODO: parse _unknown to recover typeId for unknown extensions

            ulong typeId = 0;
            var extensions = ExtensionsMap.Where(e => e.Value == extensionPart).ToList();
            if (extensions.Count == 1)
            {
                var extension = extensions.Single();
                typeId = extension.Key;
            }
            ulong hash = HashFileName(hashablePart);
            hash = (typeId << 51) | hash;
            return hash;
        }

        internal static string NormalizeFilePath(string filePath)
        {
            return filePath.Replace("/", "\\").TrimStart('\\');
        }

        private static string DenormalizeFilePath(string filePath)
        {
            return filePath.Replace("\\", "/");
        }

        internal static bool TryGetFileNameFromHash(ulong hash, out string fileName)
        {
            bool foundFileName = true;
            string filePath;
            string fileExtension;

            ulong extensionHash = hash >> 51;
            ulong pathHash = hash & 0x3FFFFFFFFFFFF;

            fileName = "";
            if (!HashNameDictionary.TryGetValue(pathHash, out filePath))
            {
                filePath = pathHash.ToString("x");
                foundFileName = false; 
            }

            fileName += filePath;

            if (!ExtensionsMap.TryGetValue(extensionHash, out fileExtension))
            {
                fileExtension = "_unknown";
                fileExtension += "_" + extensionHash;//tex TODO rethink
                foundFileName = false;
            }
            else
            {
                fileName += ".";
            }
            fileName += fileExtension;
            
            DebugAssertHashMatches(foundFileName, hash, fileName);

            return foundFileName;
        }

        internal static bool TryGetFileNameFromHash(ulong hash, int fileExtensionId, out string fileName)
        {
            string fileExtension = TypeExtensions[fileExtensionId];
            ulong hashMasked = hash & 0xFFFFFFFFFFFF;

            bool fileNameFound = HashNameDictionary.TryGetValue(hashMasked, out fileName);
            if (fileNameFound == false)
            {
                fileName = String.Format("{0:x}", hashMasked);
            }

            fileName = String.Format("{0}{1}", fileName, fileExtension);
            return fileNameFound;
        }

        [Conditional("DEBUG")]
        private static void DebugAssertHashMatches(bool foundFileName, ulong hash, string fileName)
        {
            if (foundFileName)
            {
                ulong hashTest = HashFileNameWithExtension(fileName);
                if (hash != hashTest)
                {
                    Debug.WriteLine("{0};{1:x};{2:x};{3:x}", fileName, hash, hashTest, (hashTest - hash));
                }
            }
        }

        public static void ReadDictionary(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                ulong hash = HashFileName(line) & 0x3FFFFFFFFFFFF;
                if (HashNameDictionary.ContainsKey(hash) == false)
                {
                    HashNameDictionary.Add(hash, line);
                }
            }
        }

        public static void ReadDictionaryLegacy(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                ulong hash = HashFileNameLegacy(line, false);
                if (HashNameDictionary.ContainsKey(hash) == false)
                {
                    HashNameDictionary.Add(hash, line);
                }
            }
        }

        internal static byte[] Md5Hash(byte[] buffer)
        {
            return Md5.ComputeHash(buffer);
        }

        internal static byte[] Md5HashText(string text)
        {
            return Md5.ComputeHash(Encoding.Default.GetBytes(text));
        }

        public static void ReadMd5Dictionary(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                byte[] md5Hash = Md5HashText(line);
                if (Md5HashNameDictionary.ContainsKey(md5Hash) == false)
                {
                    Md5HashNameDictionary.Add(md5Hash, line);
                }
            }
        }

        internal static bool TryGetFileNameFromMd5Hash(byte[] md5Hash, string entryName, out string fileName)
        {
            if (Md5HashNameDictionary.TryGetValue(md5Hash, out fileName) == false)
            {
                fileName = string.Format("{0}{1}", BitConverter.ToString(md5Hash).Replace("-", ""),
                    GetFileExtension(entryName));
                return false;
            }
            return true;
        }

        private static string GetFileExtension(string entryName)
        {
            string extension = "";
            int index = entryName.LastIndexOf(".", StringComparison.Ordinal);
            if (index != -1)
            {
                extension = entryName.Substring(index, entryName.Length - index);
            }

            return extension;
        }
    }
}
