namespace VacdmDataFaker.FlowMeasures
{
    public partial class FlowMeasureFaker
    {
        private static readonly List<string> _aircraftTypes = new()
        {
            "A320",
            "A321",
            "A339",
            "A346",
            "A359",
            "A388",
            "B727",
            "B738",
            "B744",
            "B753",
            "B764",
            "B77L",
            "B788",
            "CONC",
            "E190",
            "CL35"
        };

        private static readonly List<string> _routes = new()
        {
            "CINDY Z74 HAREM T104 ROKIL",
            "GIVMI Y101 ERNAS T161 DEBHI",
            "OBOKA Z29 TORNU DCT ABNED L980 LOGAN",
            "MARUN Y152 ARPEG Z850 HMM T281 NORKU",
            "SOBRA Y180 DIK UN857 RAPOR UZ157 VEDUS",
            "SOBRA Y180 BITBU Y181 GOPAS N852 LNO",
            "ANEKI Y163 NEKLO Y171 INKAM T725 LAMGO T721 RILAX",
            "ARNEM L620 SUVOX Z718 OSN L980 HLZ DCT BATEL T207 OGBER",
            "IDRID L980 LOGAN",
            "LOPIK N852 SUTAL DCT GTQ DCT BAMEV DCT BLM",
            "SULUS Z650 NOGRA DCT VEMUT DCT ETVIS DCT BUDEX",
            "BPK Q295 BRAIN M197 REDFA",
            "UMLAT T418 WELIN T420 ELVOS",
            "ULTIB T420 TNT UN57 POL UN601 INPIP",
            "DET L6 DVR UL9 KONAN UL607 MATUG DCT MOVUM T109 HAREM T104 ROKIL",
            "MODMI M185 MID L612 BOGNA DCT HARDY UM605 BIBAX",
            "BPK Q295 SOMVA DCT MAVAS DCT EEL M105 OPJOT T907 MUBZI T903 RIBSO",
            "BPK Q295 SOMVA DCT MAVAS DCT TALSA P729 TUDLO",
            "PETEV N872 ERNOV",
            "TOVRI N851 RIKUM DCT OTKIL Y368 LAKUT",
            "ARS N623 IBGAX DCT EBURI N623 ESEBA",
            "MERIT ROBUC3",
            "RBV Q430 COPES Q75 MXE CLIPR3",
            "DIXIE T224 JIIMS OOD",
            "EARND ELANR JAKKS2",
            "EBAKE WISMO POSTS PADDE SVM QWERI NUBER6",
            "ORCKA5 MISEN RNDRZ2",
            "LAXX1 MZB",
            "SUMMR2 STOKD SERFR SERFR4",
            "TRUKN2 DEDHD RBL LMT HAWKZ7",
            "MONTN2 SEA PAE GRIZZ7"
        };

        private static readonly List<string> _waypoints = new()
        {
            "CINDY",
            "HAREM",
            "ROKIL",
            "GIVMI",
            "ERNAS",
            "DEBHI",
            "OBOKA",
            "TORNU",
            "ABNED",
            "LOGAN",
            "MARUN",
            "ARPEG",
            "NORKU",
            "SOBRA",
            "RAPOR",
            "VEDUS",
            "BITBU",
            "GOPAS",
            "NEKLO",
            "LAMGO",
            "RILAX",
            "ARNEM",
            "SUVOX",
            "OGBER",
            "IDRID",
            "LOPIK",
            "BAMEV",
            "BUDEX",
            "REDFA",
            "UMLAT",
            "ULTIB",
            "KONAN",
            "HARDY",
            "PETEV",
            "TALSA",
            "MERIT",
            "ESEBA",
            "PADDE",
            "STOKD"
        };

        private static readonly List<string> _airports = new()
        {
            "EDDF",
            "EDDM",
            "EDDB",
            "EDDH",
            "EDDL",
            "EHAM",
            "EGLL",
            "EGCC",
            "EGPH",
            "EIDW",
            "LOWW",
            "LOWS",
            "EKCH",
            "ENGM",
            "ESSA",
            "EFHK",
            "LSZH",
            "KJFK",
            "KDCA",
            "KBOS",
            "CYYZ",
            "KORD",
            "KATL",
            "KLAX",
            "KSAN",
            "KSFO",
            "KSEA",
            "CYVR"
        };

        private static readonly List<int> _mdiAdiValues = new() { 30, 60, 90, 120, 150, 180, 210, 240, 270, 300 };

        private static readonly List<int> _fphValues = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20 };

        private static readonly List<int> _mitValues = new() { 3, 5, 10, 15, 20, 25, 30 };

        private static readonly List<int> _iasValues = new() { 220, 240, 250, 260, 280, 290, 300, 320, 330, 340 };

        private static readonly List<double> _machValues = new() { 0.73, 0.74, 0.75, 0.76, 0.77, 0.78, 0.79, 0.8 };

        private static readonly List<int> _iasRedValues = new() { 5, 10, 15, 20, 25, 30, 40, 50, 60, 70, 80 };

        private static readonly List<double> _machRedValues = new() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8 };

        private static readonly List<string> _measureTypes = new()
        {
            "minimum_departure_interval",
            "average_departure_interval",
            "per_hour",
            "miles_in_trail",
            "max_ias",
            "max_mach",
            "ias_reduction",
            "prohibit",
            "ground_stop",
            "mandatory_route"
        };

        private static readonly List<int> _flightlevels = new() { 240, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350, 360, 270, 380};

        private static readonly List<string> _filters = new()
        {
            "ADEP",
            "ADES",
            "level_above",
            "level_below",
            "level",
            "waypoint",
        };

        private static readonly List<string> _reasons = new()
        {
            "TMA Capacity",
            "Airport Capacity",
            "Staffing",
            "ATC Capacity",
            "Event Traffic",
            "RWY Capacity",
            "Holding Capacity"
        };

        private static readonly List<string> _idents = new()
        {
            "EDGG",
            "EDWW",
            "EDMM",
            "EHAA",
            "EGTT",
            "EISN",
            "EKDK",
            "ENOR",
            "ESAA",
            "EFIN",
            "LOVV",
            "LFEE",
            "EBBU",
            "KZNY",
            "KZDC"
        };

        private static readonly List<char> _letters = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    }
}
